// ScreenGun
// - SettingsViewModel.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;
using System.Globalization;

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
        ///     The framerate text
        /// </summary>
        private string framerateText;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SettingsViewModel" /> class.
        /// </summary>
        public SettingsViewModel()
        {
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
        public bool DefaultMicEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the framerate.
        /// </summary>
        /// <value>
        ///     The framerate.
        /// </value>
        public int Framerate { get; set; }

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
                    this.FramerateText = this.Framerate.ToString(CultureInfo.InvariantCulture);
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
        public string StoragePath { get; set; }

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
        ///     Resets the dialogues.
        /// </summary>
        public void ResetDialogues()
        {
            this.OnDialogReset();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [dialog reset].
        /// </summary>
        protected virtual void OnDialogReset()
        {
            EventHandler handler = this.DialogReset;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Updates the framerate.
        /// </summary>
        private void UpdateFramerate()
        {
            int framerate;
            if (!int.TryParse(this.FramerateText, out framerate))
            {
                this.ValidFps = false;
                return;
            }

            if (framerate <= 0 || framerate > 30)
            {
                this.ValidFps = false;
            }

            this.ValidFps = true;
            this.Framerate = framerate;
        }

        #endregion
    }
}