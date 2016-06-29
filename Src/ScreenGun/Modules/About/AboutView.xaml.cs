// ScreenGun
// - SettingsView.xaml.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

using MahApps.Metro.Controls;

using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace ScreenGun.Modules.Settings
{
    /// <summary>
    ///     Interaction logic for AboutView.xaml
    /// </summary>
    public partial class AboutView : MetroWindow
    {
        #region Static Fields

        /// <summary>
        ///     The invalid path chars
        /// </summary>
        private static readonly char[] InvalidPathChars = Path.GetInvalidPathChars();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SettingsView" /> class.
        /// </summary>
        public AboutView()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the view model.
        /// </summary>
        /// <value>
        ///     The view model.
        /// </value>
        public AboutViewModel ViewModel
        {
            get
            {
                return this.DataContext as AboutViewModel;
            }
        }

        #endregion
    }
}