// ScreenGun
// - RecorderViewModel.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;
using System.Drawing;

using ScreenGun.Base;

namespace ScreenGun.Modules.Recorder
{
    /// <summary>
    ///     View Model for the Recorder.
    /// </summary>
    public class RecorderViewModel : ViewModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether we are recording.
        /// </summary>
        public bool IsRecording { get; set; }

        /// <summary>
        ///     Gets or sets the recording region.
        /// </summary>
        /// <value>
        ///     The recording region.
        /// </value>
        public Rectangle RecordingRegion { get; set; }

        #endregion

        public void StartRecording()
        {
            Console.WriteLine("Start recording");
        }

        public void ToggleFullScreen()
        {
            Console.WriteLine("Toggle Fullscreen");
        }

        public void ToggleMicrophone()
        {
            Console.WriteLine("Toggle microphone");
        }

        public void Cancel()
        {
            Console.WriteLine("Cancel");
        }
    }
}