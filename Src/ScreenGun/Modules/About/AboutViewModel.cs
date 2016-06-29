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
using System.Reflection;
using System.Diagnostics;

namespace ScreenGun.Modules.Settings
{
    /// <summary>
    ///     The about view model.
    /// </summary>
    public class AboutViewModel : ViewModel
    {
        /// <summary>
        /// App version.
        /// </summary>
        public string Version
        {
            get
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fvi.FileVersion;
                return version;
            }
        }
    }
}