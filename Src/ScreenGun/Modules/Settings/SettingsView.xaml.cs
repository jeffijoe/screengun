using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using MahApps.Metro.Controls;

using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Path = System.IO.Path;

namespace ScreenGun.Modules.Settings
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : MetroWindow
    {
        private readonly char[] invalidPathChars = Path.GetInvalidPathChars();

        public SettingsView()
        {
            InitializeComponent();
        }

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

        public SettingsViewModel ViewModel
        {
            get
            {
                return this.DataContext as SettingsViewModel;
            }
        }

        private void TxtStoragePath_OnLostFocus(object sender, RoutedEventArgs e)
        {
            string storagePath = this.ViewModel.StoragePath ?? string.Empty;
            bool isNullOrEmpty = string.IsNullOrEmpty(storagePath);
            bool containsInvalidChars = storagePath.Any(this.invalidPathChars.Contains);
            DriveInfo[] driveInfos = DriveInfo.GetDrives();
            bool driveExists = storagePath.Contains("\\") && driveInfos.Any(k =>
            {
                string lower = storagePath.Split('\\').FirstOrDefault().ToLower();
                return k.Name.ToLower() == string.Format("{0}\\", lower);
            });

            if (isNullOrEmpty || containsInvalidChars || !driveExists)
            {
                this.ViewModel.StoragePath = "C:\\ScreenGun\\Clips";
            }
        }

        private void TxtStoragePath_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return)
            {
                return;
            }
            this.TxtStoragePath_OnLostFocus(sender, e);
        }
    }
}
