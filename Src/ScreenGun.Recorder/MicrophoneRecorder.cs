// ScreenGun
// - MicrophoneRecorder.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using NAudio.Wave;

namespace ScreenGun.Recorder
{
    /// <summary>
    ///     The microphone recorder.
    /// </summary>
    public class MicrophoneRecorder
    {
        #region Fields

        /// <summary>
        ///     The material folder.
        /// </summary>
        private readonly string outputFilePath;

        /// <summary>
        ///     The wave file.
        /// </summary>
        private WaveFileWriter waveFile;

        /// <summary>
        ///     The wave source.
        /// </summary>
        private WaveInEvent waveSource;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrophoneRecorder"/> class.
        /// </summary>
        /// <param name="outputFilePath">
        /// The material folder.
        /// </param>
        public MicrophoneRecorder(string outputFilePath)
        {
            this.outputFilePath = outputFilePath;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The start.
        /// </summary>
        public void Start()
        {
            this.waveSource = new WaveInEvent
            {
                WaveFormat = new WaveFormat(44100, 2)
            };

            this.waveFile = new WaveFileWriter(
                this.outputFilePath, 
                this.waveSource.WaveFormat);
            this.waveSource.DataAvailable += this.OnWaveSourceOnDataAvailable;
            this.waveSource.StartRecording();
        }

        /// <summary>
        ///     The stop.
        /// </summary>
        public void Stop()
        {
            if (this.waveSource != null)
            {
                this.waveSource.StopRecording();
                this.waveSource.DataAvailable -= this.OnWaveSourceOnDataAvailable;
                this.waveSource.Dispose();
                this.waveSource = null;
            }

            if (this.waveFile != null)
            {
                this.waveFile.Dispose();
                this.waveFile = null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on wave source on data available.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        private void OnWaveSourceOnDataAvailable(object sender, WaveInEventArgs args)
        {
            if (this.waveFile != null)
            {
                this.waveFile.Write(args.Buffer, 0, args.BytesRecorded);
                this.waveFile.Flush();
            }
        }

        #endregion
    }
}