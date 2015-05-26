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
using ScreenGun.Modules.RegionSelector;
using ScreenGun.Modules.Screenshot;

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
        ///     Gets or sets the files.
        /// </summary>
        /// <value>
        ///     The files.
        /// </value>
        public ObservableCollection<ScreenGunFileViewModel> Files { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The new screenshot.
        /// </summary>
        public void NewScreenshot()
        {
            this.windowManager.ShowWindow(new ScreenshotViewModel());
        }

        #endregion
    }
}