// ScreenGun
// - RecordingCreated.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using ScreenGun.Modules.Main.ScreenGunFile;

namespace ScreenGun.Events
{
    /// <summary>
    ///     Recording Created event.
    /// </summary>
    public class RecordingCreated
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordingCreated"/> class.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        public RecordingCreated(ScreenGunFileViewModel file)
        {
            this.File = file;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the file for the recording that was just created.
        /// </summary>
        /// <value>
        ///     The file path.
        /// </value>
        public ScreenGunFileViewModel File { get; private set; }

        #endregion
    }
}