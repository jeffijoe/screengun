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
    using System;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Navigation;
    using System.Windows.Resources;

    using ScreenGun.Modules.NotifyIcon;

    using Application = System.Windows.Application;
    using MessageBox = System.Windows.MessageBox;

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

        private NotifyIconViewModel notifyIconViewModel;

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

        /// <summary>
        ///     The new screenshot.
        /// </summary>
        public void NewTooltip()
        {
            this.notifyIconViewModel.ShowBalloonTip(
                2500,
                "Huehuehue",
                "Well fak u ya little cunt!",
                ToolTipIcon.Info,
                (sender, args) => MessageBox.Show("Huehuehue", "caption", MessageBoxButton.OK));
        }

        #endregion

        public void Initialize(FrameworkElement frameworkElement)
        {
            var stream = Application.GetResourceStream(new Uri("Resources/screengun_logo.ico", UriKind.Relative)).Stream;
            this.notifyIconViewModel = new NotifyIconViewModel(new Icon(stream));
            this.notifyIconViewModel.RightClicked += (sender, args) => Console.WriteLine("Rightclick");
            this.notifyIconViewModel.LeftClicked += (sender, args) => Console.WriteLine("Leftclick");

            this.notifyIconViewModel.
        }
    }
}