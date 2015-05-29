// ScreenGun
// - IScreenRecorder.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;
using System.Threading.Tasks;

namespace ScreenGun.Recorder
{
    /// <summary>
    ///     The ScreenRecorder interface.
    /// </summary>
    public interface IScreenRecorder
    {
        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether we are recording or not.
        /// </summary>
        bool IsRecording { get; }

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
        void Start(ScreenRecorderOptions options, IProgress<RecorderState> progressReporter = null);

        /// <summary>
        ///     The stop.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        Task StopAsync();

        #endregion
    }
}