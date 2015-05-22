// ScreenGun
// - FFMPEGScreenRecorderOptions.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.
namespace ScreenGun.Recorder
{
    /// <summary>
    ///     The ffmpeg screen recorder options.
    /// </summary>
    public class FFMPEGScreenRecorderOptions
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FFMPEGScreenRecorderOptions"/> class.
        /// </summary>
        /// <param name="ffmpegPath">
        /// The ffmpeg path.
        /// </param>
        /// <param name="frameCaptureBackend">
        /// The frame capture backend.
        /// </param>
        public FFMPEGScreenRecorderOptions(string ffmpegPath, IFrameCaptureBackend frameCaptureBackend)
        {
            this.FfmpegPath = ffmpegPath;
            this.FrameCaptureBackend = frameCaptureBackend;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the ffmpeg path.
        /// </summary>
        public string FfmpegPath { get; private set; }

        /// <summary>
        ///     Gets the frame capture backend.
        /// </summary>
        /// <value>
        ///     The frame capture backend.
        /// </value>
        public IFrameCaptureBackend FrameCaptureBackend { get; private set; }

        #endregion
    }
}