// ScreenGun
// - ScreenRecorderOptions.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace ScreenGun.Recorder
{
    /// <summary>
    ///     Options for the screen recorder.
    /// </summary>
    public class ScreenRecorderOptions
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenRecorderOptions"/> class.
        /// </summary>
        /// <param name="recordingRegion">
        /// The recording Region.
        /// </param>
        public ScreenRecorderOptions(Rectangle recordingRegion)
        {
            this.RecordingRegion = recordingRegion;
            this.MaterialTempFolder = Path.Combine(Path.GetTempPath(), Assembly.GetCallingAssembly().GetName().Name);
            this.OutputFilePath = AppDomain.CurrentDomain.BaseDirectory;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether to delete material when done.
        ///     Default is false.
        /// </summary>
        /// <value>
        ///     <c>true</c> if delete material when done; otherwise, <c>false</c>.
        /// </value>
        public bool DeleteMaterialWhenDone { get; set; }

        /// <summary>
        ///     Gets or sets the material temporary folder. Default is a temp folder from the user's path.
        /// </summary>
        /// <remarks>
        ///     This folder will contain the materials folder, which will be deleted when done, if
        ///     <see cref="DeleteMaterialWhenDone" /> is true.
        /// </remarks>
        /// <value>
        ///     The screenshots temporary folder.
        /// </value>
        public string MaterialTempFolder { get; set; }

        /// <summary>
        ///     Gets or sets the output file path. Default is Video.mp4 in the app's folder.
        /// </summary>
        /// <value>
        ///     The output file.
        /// </value>
        public string OutputFilePath { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to record the microphone.
        /// </summary>
        /// <value>
        ///     <c>true</c> if we want to record the microphone; otherwise, <c>false</c>.
        /// </value>
        public bool RecordMicrophone { get; set; }

        /// <summary>
        ///     Gets or sets the recording region. Not set to any default.
        /// </summary>
        /// <value>
        ///     The recording region.
        /// </value>
        public Rectangle RecordingRegion { get; set; }

        #endregion
    }
}