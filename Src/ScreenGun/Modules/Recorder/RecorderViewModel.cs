// ScreenGun
// - RecorderViewModel.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;
using System.Drawing;

using Caliburn.Micro;

using PropertyChanged;

namespace ScreenGun.Modules.Recorder
{
    /// <summary>
    ///     View Model for the Recorder.
    /// </summary>
    [ImplementPropertyChanged]
    public class RecorderViewModel : Screen
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is full screen.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is full screen; otherwise, <c>false</c>.
        /// </value>
        public bool IsFullScreen { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether we are recording.
        /// </summary>
        public bool IsRecording { get; set; }

        /// <summary>
        ///     Gets or sets the recording region.
        /// </summary>
        /// <value>
        ///     The recording region.
        /// </value>
        public Rectangle RecordingRegion { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to use microphone or not.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [use microphone]; otherwise, <c>false</c>.
        /// </value>
        public bool UseMicrophone { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Cancels this instance.
        /// </summary>
        public void Cancel()
        {
            this.TryClose();
        }

        /// <summary>
        ///     Starts the recording.
        /// </summary>
        public void StartRecording()
        {
            this.IsRecording = true;
        }

        /// <summary>
        ///     Toggles the full screen.
        /// </summary>
        public void ToggleFullscreen()
        {
            this.IsFullScreen = !this.IsFullScreen;
        }

        /// <summary>
        ///     Toggles the microphone.
        /// </summary>
        public void ToggleMicrophone()
        {
            this.UseMicrophone = !this.UseMicrophone;
        }

        /// <summary>
        /// Closes the recorder - can only be done when not recording.
        /// </summary>
        public void Close()
        {
            if (this.IsRecording == false)
            {
                this.Cancel();
            }
        }

        #endregion
    }
}