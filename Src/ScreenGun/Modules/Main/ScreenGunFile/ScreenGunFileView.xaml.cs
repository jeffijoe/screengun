// ScreenGun
// - ScreenGunFileView.xaml.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScreenGun.Modules.Main.ScreenGunFile
{
    /// <summary>
    ///     Interaction logic for ScreenGunFileView.xaml
    /// </summary>
    public partial class ScreenGunFileView : UserControl
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScreenGunFileView" /> class.
        /// </summary>
        public ScreenGunFileView()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the view model.
        /// </summary>
        public ScreenGunFileViewModel ViewModel
        {
            get
            {
                return (ScreenGunFileViewModel)this.DataContext;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The ui element_ on preview mouse left button down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void UIElement_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(
                this,
                new DataObject(DataFormats.FileDrop, new[] { this.ViewModel.FilePath }), 
                DragDropEffects.Copy);
        }

        #endregion
    }
}