// ScreenGun
// - ShellViewModel.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System.Collections.ObjectModel;

using Caliburn.Micro;

using ScreenGun.Base;
using ScreenGun.Modules.Main.ScreenGunFile;
using ScreenGun.Modules.Recorder;
using ScreenGun.Modules.Settings;

namespace ScreenGun.Modules.Main
{
    /// <summary>
    ///     The shell view model.
    /// </summary>
    public class ShellViewModel : ViewModel, IShell
    {
        #region Fields

        /// <summary>
        ///     The window manager.
        /// </summary>
        private readonly IWindowManager windowManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        /// <param name="windowManager">
        /// The window Manager.
        /// </param>
        public ShellViewModel(IWindowManager windowManager)
        {
            this.windowManager = windowManager;
            this.Files = new ObservableCollection<ScreenGunFileViewModel>();
            this.Files.Add(new ScreenGunFileViewModel("Recording 1.mp4", "C:\\Recordings\\Recording 1.mp4"));
            this.Files.Add(new ScreenGunFileViewModel("Recording 2.mp4", "C:\\Recordings\\Recording 2.mp4"));
            this.Files.Add(new ScreenGunFileViewModel("Screenshot 1.png", "C:\\Screenshots\\Screenshot 1.png"));
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the files.
        /// </summary>
        /// <value>
        ///     The files.
        /// </value>
        public ObservableCollection<ScreenGunFileViewModel> Files { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Shows the Recorder view.
        /// </summary>
        public void NewRecording()
        {
            this.windowManager.ShowWindow(IoC.Get<RecorderViewModel>());
        }

        /// <summary>
        ///     Shows the settings view.
        /// </summary>
        public void Settings()
        {
            this.windowManager.ShowWindow(IoC.Get<SettingsViewModel>());
        }

        #endregion
    }
}