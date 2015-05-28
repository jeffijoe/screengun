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
    ///     Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : MetroWindow
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
        public SettingsView()
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
        public SettingsViewModel ViewModel
        {
            get
            {
                return this.DataContext as SettingsViewModel;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the OnClick event of the BtnBrowse control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void BtnBrowse_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            DialogResult dialogResult = dialog.ShowDialog();
            if (dialogResult != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            this.ViewModel.StoragePath = dialog.SelectedPath;
        }

        /// <summary>
        /// Handles the OnKeyDown event of the TxtStoragePath control.
        ///     <para>
        /// Triggers the LostFocus event if the key is Enter
        ///     </para>
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.
        /// </param>
        private void TxtStoragePath_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return && e.Key != Key.Enter)
            {
                return;
            }

            this.TxtStoragePath_OnLostFocus(sender, e);
        }

        /// <summary>
        /// Handles the OnLostFocus event of the TxtStoragePath control.
        ///     <para>
        /// Ensures the specified path is a valid path
        ///     </para>
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void TxtStoragePath_OnLostFocus(object sender, RoutedEventArgs e)
        {
            string storagePath = this.ViewModel.StoragePath ?? string.Empty;
            bool isNullOrEmpty = string.IsNullOrEmpty(storagePath);
            bool containsInvalidChars = storagePath.Any(InvalidPathChars.Contains);
            DriveInfo[] driveInfos = DriveInfo.GetDrives();
            bool driveExists = storagePath.Contains("\\") && driveInfos.Any(
                k => {
                    string lower = storagePath.Split('\\').FirstOrDefault().ToLower();
                    return k.Name.ToLower() == string.Format("{0}\\", lower);
                });

            if (isNullOrEmpty || containsInvalidChars || !driveExists)
            {
                this.ViewModel.StoragePath = "C:\\ScreenGun\\Clips";
            }
        }

        #endregion
    }
}