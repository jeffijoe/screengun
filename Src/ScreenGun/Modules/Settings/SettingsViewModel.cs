// ScreenGun
// - SettingsViewModel.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;
using System.Globalization;
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
        ///     The framerate
        /// </summary>
        private int frameRate;

        /// <summary>
        ///     The framerate text
        /// </summary>
        private string framerateText;

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
            this.frameRate = 20;
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
        ///     Gets or sets the framerate.
        /// </summary>
        /// <value>
        ///     The framerate.
        /// </value>
        public int FrameRate
        {
            get
            {
                return this.frameRate;
            }

            set
            {
                if (this.frameRate == value)
                {
                    return;
                }

                this.frameRate = value;
                this.NotifyOfPropertyChange(() => this.FrameRate);
                this.SaveSettings();
            }
        }

        /// <summary>
        ///     Gets or sets the framerate text.
        /// </summary>
        /// <value>
        ///     The framerate text.
        /// </value>
        public string FramerateText
        {
            get
            {
                if (string.IsNullOrEmpty(this.framerateText))
                {
                    this.FramerateText = this.FrameRate.ToString(CultureInfo.InvariantCulture);
                }

                return this.framerateText;
            }

            set
            {
                if (this.framerateText == value)
                {
                    return;
                }

                this.framerateText = value;
                this.NotifyOfPropertyChange(() => this.FramerateText);
                this.UpdateFramerate();
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

        /// <summary>
        ///     Gets a value indicating whether [valid FPS].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [valid FPS]; otherwise, <c>false</c>.
        /// </value>
        public bool ValidFps { get; private set; }

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

            if (settingsFile.FrameRate != 0)
            {
                this.frameRate = settingsFile.FrameRate;
            }

            this.defaultMicEnabled = settingsFile.DefaultMicEnabled;
        }

        /// <summary>
        ///     Updates the framerate.
        /// </summary>
        private void UpdateFramerate()
        {
            int fps;
            if (!int.TryParse(this.FramerateText, out fps))
            {
                this.ValidFps = false;
                return;
            }

            if (fps <= 0 || fps > 30)
            {
                this.ValidFps = false;
            }

            this.ValidFps = true;
            this.FrameRate = fps;
        }

        #endregion
    }
}