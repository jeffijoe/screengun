// ScreenGun
// - IScreenGunSettings.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;

namespace ScreenGun.Modules.Settings
{
    /// <summary>
    ///     The ScreenGunSettings interface.
    /// </summary>
    public interface IScreenGunSettings
    {
        #region Public Events

        /// <summary>
        ///     Occurs when the dialog is reset.
        /// </summary>
        event EventHandler DialogReset;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether the mic is enabled by default.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the mic is enabled by default; otherwise, <c>false</c>.
        /// </value>
        bool DefaultMicEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the storage path.
        /// </summary>
        /// <value>
        ///     The storage path.
        /// </value>
        string StoragePath { get; set; }

        /// <summary>
        /// The recording device to use.
        /// </summary>
        int RecordingDeviceNumber { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Saves the settings.
        /// </summary>
        void SaveSettings();

        #endregion
    }
}