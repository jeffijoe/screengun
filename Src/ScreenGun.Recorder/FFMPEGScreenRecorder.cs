// ScreenGun
// - FFMPEGScreenRecorder.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScreenGun.Recorder
{
    /// <summary>
    ///     An implementation of a screen recorder using FFMPEG.
    /// </summary>
    public class FFMPEGScreenRecorder
    {
        #region Fields

        /// <summary>
        ///     The ffmpeg path.
        /// </summary>
        private readonly string ffmpegPath;

        /// <summary>
        ///     The frame capture backend.
        /// </summary>
        private readonly IFrameCaptureBackend frameCaptureBackend;

        /// <summary>
        ///     The frame saver tasks.
        /// </summary>
        private readonly List<Task> frameSaverTasks;

        /// <summary>
        ///     The frame counter.
        /// </summary>
        private int frameCounter;

        /// <summary>
        ///     The frames.
        /// </summary>
        private ConcurrentQueue<Frame> frames;

        /// <summary>
        ///     The last frame bitmap.
        /// </summary>
        private Bitmap lastFrameBitmap;

        /// <summary>
        ///     The material folder.
        /// </summary>
        private string materialFolder;

        /// <summary>
        ///     The mic file path.
        /// </summary>
        private string micFilePath;

        /// <summary>
        ///     The mic recorder.
        /// </summary>
        private MicrophoneRecorder micRecorder;

        /// <summary>
        ///     The progress.
        /// </summary>
        private IProgress<RecorderState> progress;

        /// <summary>
        ///     The recorder options.
        /// </summary>
        private ScreenRecorderOptions recorderOptions;

        /// <summary>
        ///     The recording name. Used for creating a material folder.
        /// </summary>
        private string recordingName;

        /// <summary>
        ///     The timer.
        /// </summary>
        private Timer timer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FFMPEGScreenRecorder"/> class.
        /// </summary>
        /// <param name="ffmpegScreenRecorderOptions">
        /// The ffmpeg screen recorder options.
        /// </param>
        public FFMPEGScreenRecorder(FFMPEGScreenRecorderOptions ffmpegScreenRecorderOptions)
        {
            this.ffmpegPath = Path.GetFullPath(ffmpegScreenRecorderOptions.FfmpegPath).Trim('\\');
            this.frameCaptureBackend = ffmpegScreenRecorderOptions.FrameCaptureBackend;
            this.frameSaverTasks = new List<Task>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether we are recording or not.
        /// </summary>
        public bool IsRecording { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Starts recording. Does not block.
        /// </summary>
        /// <param name="options">
        /// The recorder options.
        /// </param>
        /// <param name="progressReporter">
        /// The progress reporter, if any..
        /// </param>
        public void Start(ScreenRecorderOptions options, IProgress<RecorderState> progressReporter = null)
        {
            if (this.IsRecording)
            {
                throw new ScreenRecorderException("Already recording.");
            }

            this.recorderOptions = options;
            this.progress = progressReporter;
            this.frameSaverTasks.Clear();
            this.frames = new ConcurrentQueue<Frame>();
            this.recordingName = string.Format("Recording {0}", DateTime.Now.ToString("yy-MM-dd hh-mm-ss"));
            this.materialFolder = Path.Combine(this.recorderOptions.MaterialTempFolder, this.recordingName);
            if (Directory.Exists(this.materialFolder))
            {
                Directory.Delete(this.micFilePath, true);
            }

            Directory.CreateDirectory(this.materialFolder);
            this.micFilePath = Path.Combine(this.materialFolder, "Microphone.wav");
            Task.Run((Action)this.Record);
        }

        /// <summary>
        ///     The stop.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public async Task StopAsync()
        {
            if (this.IsRecording == false)
            {
                throw new ScreenRecorderException("Not recording.");
            }

            this.timer.Dispose();
            this.IsRecording = false;
            if (this.recorderOptions.RecordMicrophone)
            {
                this.micRecorder.Stop();
            }

            this.ReportProgress(new RecorderState(RecordingStage.Encoding));
            await Task.WhenAll(this.frameSaverTasks.ToArray());
            this.SaveFrames();

            await Task.Run((Action)this.Encode);
            this.ReportProgress(new RecorderState(RecordingStage.Done));
            if (this.recorderOptions.DeleteMaterialWhenDone)
            {
                Directory.Delete(this.materialFolder, true);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates the FFMPEG cmd arguments.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        private string CreateFFMPEGArgs()
        {
            var sb = new StringBuilder();
            sb.AppendFormat(
                "-f image2 -i \"{0}\" ", 
                Path.Combine(this.materialFolder, "img%06d.png"), 
                this.recorderOptions.FrameRate, 
                this.recorderOptions.RecordingRegion.Width, 
                this.recorderOptions.RecordingRegion.Height);

            if (this.recorderOptions.RecordMicrophone)
            {
                sb.AppendFormat("-i \"{0}\" ", this.micFilePath);
            }

            sb.AppendFormat(
                "-vf \"setpts=1.54*PTS\" -r {0} -s {1}x{2} -c:v libx264 ", 
                this.recorderOptions.FrameRate, 
                this.recorderOptions.RecordingRegion.Width, 
                this.recorderOptions.RecordingRegion.Height);

            if (this.recorderOptions.RecordMicrophone)
            {
                sb.AppendFormat("-c:a mp3 ");
            }

            sb.AppendFormat("\"{0}\" -y", this.recorderOptions.OutputFilePath);
            return sb.ToString();
        }

        /// <summary>
        ///     Encodes the video.
        /// </summary>
        private void Encode()
        {
            /**
             * FFMPEG arg explanation:
             * -f image2 - create a movie from an image sequence.
             * -i "{0}" - input file pattern. In our case, it's the material path + "img000000.png", "img000001.png" and so on.
             * -vf "setpts=1.50*PTS" = vf means video filter, PTS is the amount we are slowing each frame down. Magic number, really.
             * -r {1} - the framerate.
             * -c:v libx264 - video codec, in our case h264.
             * {4} - output file.
             */
            var ffmpegArgs = this.CreateFFMPEGArgs();

            // Start a CMD in the background, which itself will run FFMPEG.
            var cmdArgs = string.Format("/C \"\"{0}\" {1}\"", this.ffmpegPath, ffmpegArgs);
            var startInfo = new ProcessStartInfo("cmd.exe", cmdArgs)
            {
                UseShellExecute = true, 
                // WindowStyle = ProcessWindowStyle.Hidden
            };

            var process = Process.Start(startInfo);
            process.WaitForExit();
        }

        /// <summary>
        ///     Records this instance.
        /// </summary>
        private void Record()
        {
            this.IsRecording = true;
            this.frameCounter = 0;

            this.timer = new Timer(_ => this.RecordFrame());
            this.timer.Change(1000 / this.recorderOptions.FrameRate, 1000 / this.recorderOptions.FrameRate);

            if (this.recorderOptions.RecordMicrophone)
            {
                this.micRecorder = new MicrophoneRecorder(this.micFilePath);
                this.micRecorder.Start();
            }

            this.ReportProgress(new RecorderState(RecordingStage.Recording));
        }

        /// <summary>
        ///     The record frame.
        /// </summary>
        private void RecordFrame()
        {
            if (this.IsRecording == false)
            {
                return;
            }

            // Capture a frame.
            Bitmap frameBitmap;
            try
            {
                frameBitmap = this.frameCaptureBackend.CaptureFrame(this.recorderOptions.RecordingRegion);
                this.lastFrameBitmap = frameBitmap;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (this.lastFrameBitmap != null)
                {
                    frameBitmap = this.lastFrameBitmap;
                }
                else
                {
                    return;
                }
            }

            Interlocked.Increment(ref this.frameCounter);
            this.frames.Enqueue(new Frame(this.frameCounter, frameBitmap));

            if (this.frames.Count > 30)
            {
                var task = Task.Run(
                    () => this.SaveFrames());
                this.frameSaverTasks.Add(task);
            }
        }

        /// <summary>
        /// Reports the progress.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        private void ReportProgress(RecorderState state)
        {
            if (this.progress != null)
            {
                this.progress.Report(state);
            }
        }

        /// <summary>
        ///     The save frames.
        /// </summary>
        private void SaveFrames()
        {
            Frame frame;
            while (this.frames.TryDequeue(out frame))
            {
                if (frame == null)
                {
                    continue;
                }

                var fileName = string.Format("img{0}.png", frame.FrameNumber.ToString("D6"));
                var path = Path.Combine(this.recorderOptions.MaterialTempFolder, this.recordingName, fileName);
                frame.FrameBitmap.Save(path, ImageFormat.Png);
                frame.FrameBitmap.Dispose();
            }
        }

        #endregion

        /// <summary>
        ///     The frame.
        /// </summary>
        private class Frame
        {
            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Frame"/> class.
            /// </summary>
            /// <param name="frameNumber">
            /// The frame number.
            /// </param>
            /// <param name="frameBitmap">
            /// The frame bitmap.
            /// </param>
            public Frame(int frameNumber, Bitmap frameBitmap)
            {
                this.FrameNumber = frameNumber;
                this.FrameBitmap = frameBitmap;
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets the frame bitmap.
            /// </summary>
            public Bitmap FrameBitmap { get; private set; }

            /// <summary>
            ///     Gets the frame number.
            /// </summary>
            public int FrameNumber { get; private set; }

            #endregion
        }
    }
}