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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScreenGun.Recorder
{
    /// <summary>
    ///     An implementation of a screen recorder using FFMPEG.
    /// </summary>
    public class FFMPEGScreenRecorder : IScreenRecorder
    {
        #region Constants

        /// <summary>
        ///     The frame rate.
        /// </summary>
        public const int FrameRate = 20;

        #endregion

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
        /// The saved frames.
        /// </summary>
        private ConcurrentQueue<Frame> savedFrames;

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

        /// <summary>
        /// Tracks when the recording was started.
        /// </summary>
        private DateTime recordingStartedAt;

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
            this.savedFrames = new ConcurrentQueue<Frame>();
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
            this.recordingStartedAt = DateTime.Now;
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

            await Task.Delay(200);
            this.ReportProgress(new RecorderState(RecordingStage.Encoding));
            await Task.WhenAll(this.frameSaverTasks.ToArray());
            this.SaveFrames();
            var inputFilePath = await this.CreateInputFile();
            await Task.Run(() => this.Encode(inputFilePath));
            this.ReportProgress(new RecorderState(RecordingStage.Done));
            if (this.recorderOptions.DeleteMaterialWhenDone)
            {
                Directory.Delete(this.materialFolder, true);
            }
        }

        /// <summary>
        /// Generates an input file for FFMPEG
        /// </summary>
        /// <returns></returns>
        private async Task<string> CreateInputFile()
        {
            var path = Path.Combine(this.materialFolder, "frames.txt");
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (var fs = new FileStream(path, FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                var arr = this.savedFrames.ToArray();
                for (var i = 0; i < arr.Length; i++)
                {
                    var frame = arr[i];
                    var next = i + 1;
                    double duration = 1;
                    if (next != arr.Length)
                    {
                        var nextFrame = arr[next];
                        var diff = nextFrame.CapturedAt - frame.CapturedAt;
                        duration = diff.TotalSeconds;
                    }

                    await sw.WriteLineAsync(string.Format("file '{0}'", Path.Combine(this.materialFolder, frame.FileName)));
                    await sw.WriteLineAsync(string.Format(CultureInfo.InvariantCulture, "duration {0}", duration));
                }
            }

            return path;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates the FFMPEG cmd arguments.
        /// </summary>
        /// <param name="inputFilePath">The input file path.</param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        private string CreateFFMPEGArgs(string inputFilePath)
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
            var sb = new StringBuilder();
            sb.AppendFormat(
                "-f concat -i \"{0}\" ", 
                inputFilePath);

            if (this.recorderOptions.RecordMicrophone)
            {
                sb.AppendFormat("-i \"{0}\" ", this.micFilePath);
            }

            // Use H.264
            sb.Append("-c:v libx264 ");

            // If we recorded the mic, we need to add it as an input.
            if (this.recorderOptions.RecordMicrophone)
            {
                sb.AppendFormat("-c:a mp3 ");
            }

            // Since we're using YUV-4:2:0, H.264 needs the dimensions to be divisible by 2.
            var width = this.recorderOptions.RecordingRegion.Width;
            var height = this.recorderOptions.RecordingRegion.Height;
            if (width % 2 != 0)
            {
                Debug.WriteLine("Adjusting width");
                width--;
            }
            if (height % 2 != 0)
            {
                Debug.WriteLine("Adjusting height");
                height++;
            }

            sb.AppendFormat("-vf scale={0}:{1} ", width, height);
            sb.Append("-pix_fmt yuv420p ");

            var startFrame = this.savedFrames.First();
            var endFrame = this.savedFrames.Last();
            var duration = endFrame.CapturedAt - startFrame.CapturedAt;
            sb.AppendFormat("-t {0} ", duration);
            sb.AppendFormat("\"{0}\" -y", this.recorderOptions.OutputFilePath);
            return sb.ToString();
        }

        /// <summary>
        ///     Encodes the video.
        /// </summary>
        private void Encode(string inputFilePath)
        {
            var ffmpegArgs = this.CreateFFMPEGArgs(inputFilePath);

            // Start a CMD in the background, which itself will run FFMPEG.
            var cmdArgs = string.Format("/C \"\"{0}\" {1}\"", this.ffmpegPath, ffmpegArgs);
            var startInfo = new ProcessStartInfo("cmd.exe", cmdArgs)
            {
                UseShellExecute = true, 
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };
            Debug.WriteLine(cmdArgs);
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
            this.timer.Change(1000 / FrameRate, 1000 / FrameRate);

            if (this.recorderOptions.RecordMicrophone)
            {
                this.micRecorder = new MicrophoneRecorder(this.micFilePath, this.recorderOptions.AudioRecordingDeviceNumber);
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
            DateTime capturedAt;
            try
            {
                frameBitmap = this.frameCaptureBackend.CaptureFrame(this.recorderOptions.RecordingRegion);
                capturedAt = DateTime.Now;
                this.lastFrameBitmap = frameBitmap;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (this.lastFrameBitmap != null)
                {
                    frameBitmap = this.lastFrameBitmap;
                    capturedAt = DateTime.Now;
                }
                else
                {
                    return;
                }
            }

            Interlocked.Increment(ref this.frameCounter);
            var fileName = string.Format("img{0}.png", this.frameCounter.ToString("D6"));
            var path = Path.Combine(this.recorderOptions.MaterialTempFolder, this.recordingName, fileName);

            var frame = new Frame(fileName, this.frameCounter, capturedAt, frameBitmap);
            
            this.frames.Enqueue(frame);
            this.savedFrames.Enqueue(frame);
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
            public Frame(string fileName, int frameNumber, DateTime capturedAt, Bitmap frameBitmap)
            {
                this.FileName = fileName;
                this.FrameNumber = frameNumber;
                this.FrameBitmap = frameBitmap;
                this.CapturedAt = capturedAt;
            }

            #endregion

            #region Public Properties

            /// <summary>
            /// The file name.
            /// </summary>
            public string FileName { get; private set; }

            /// <summary>
            ///     Gets the frame bitmap.
            /// </summary>
            public Bitmap FrameBitmap { get; private set; }

            /// <summary>
            ///     Gets the frame number.
            /// </summary>
            public int FrameNumber { get; private set; }

            /// <summary>
            /// Timestamp from when the frame was shot.
            /// </summary>
            public DateTime CapturedAt { get; private set; }

            #endregion
        }
    }
}