// ScreenGun
// - SettingsViewModel.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;
using System.IO;

using ScreenGun.Base;

namespace ScreenGun.Modules.Settings
{
    /// <summary>
    ///     The settings view model.
    /// </summary>
    public class SettingsViewModel : ViewModel, IScreenGunSettings
    {
        #region Fields

        /// <summary>
        ///     The file path
        /// </summary>
        private readonly string filePath;

        /// <summary>
        ///     The default mic enabled
        /// </summary>
        private bool defaultMicEnabled;

        /// <summary>
        ///     The storage path
        /// </summary>
        private string storagePath;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        /// <param name="folderName">
        /// The folder name.
        /// </param>
        public SettingsViewModel(string folderName)
            : this()
        {
            var rootDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                folderName);
            if (Directory.Exists(rootDirectory) == false)
            {
                Directory.CreateDirectory(rootDirectory);
            }

            this.filePath = Path.Combine(rootDirectory, "settings.conf");
            this.Load();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SettingsViewModel" /> class.
        /// </summary>
        public SettingsViewModel()
        {
            // Default settings
            this.defaultMicEnabled = false;
            this.storagePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        #endregion

        #region Public Events

        /// <summary>
        ///     Occurs when the dialog is reset.
        /// </summary>
        public event EventHandler DialogReset;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether the mic is enabled by default.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the mic is enabled by default; otherwise, <c>false</c>.
        /// </value>
        public bool DefaultMicEnabled
        {
            get
            {
                return this.defaultMicEnabled;
            }

            set
            {
                if (this.defaultMicEnabled == value)
                {
                    return;
                }

                this.defaultMicEnabled = value;
                this.NotifyOfPropertyChange(() => this.DefaultMicEnabled);
                this.SaveSettings();
            }
        }

        /// <summary>
        ///     Gets or sets the storage path.
        /// </summary>
        public string StoragePath
        {
            get
            {
                return this.storagePath;
            }

            set
            {
                if (this.storagePath == value)
                {
                    return;
                }

                this.storagePath = value;
                this.NotifyOfPropertyChange(() => this.StoragePath);
                this.SaveSettings();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Saves the settings.
        /// </summary>
        public void SaveSettings()
        {
            SettingsFile.SaveSettings(this.filePath, this);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Loads this instance.
        /// </summary>
        private void Load()
        {
            if (!File.Exists(this.filePath))
            {
                this.SaveSettings();
                return;
            }

            var settingsFile = SettingsFile.FromFile(this.filePath);
            if (string.IsNullOrEmpty(settingsFile.StoragePath) == false)
            {
                this.storagePath = settingsFile.StoragePath;
            }

            this.defaultMicEnabled = settingsFile.DefaultMicEnabled;
        }

        #endregion
    }
}