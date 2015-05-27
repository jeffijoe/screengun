// ScreenGun
// - ShellView.xaml.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using MahApps.Metro.Controls;

namespace ScreenGun.Modules.Main
{
    using System;
    using System.Windows;

    /// <summary>
    /// Shell view.
    /// </summary>
    public partial class ShellView : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellView"/> class.
        /// </summary>
        public ShellView()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.ViewModel.Initialize(this);
        }

        public ShellViewModel ViewModel
        {
            get
            {
                return this.DataContext as ShellViewModel;
            }
        }
    }
}