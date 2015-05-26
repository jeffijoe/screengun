// ScreenGun
// - RegionSelectorWindow.xaml.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;
using System.Windows;
using System.Windows.Input;

namespace ScreenGun.Modules.RegionSelector
{
    /// <summary>
    ///     Interaction logic for RegionSelectorView.xaml
    /// </summary>
    public partial class RegionSelectorWindow : Window
    {
        #region Fields

        /// <summary>
        ///     The end position.
        /// </summary>
        private Point endPosition;

        /// <summary>
        ///     The is dragging.
        /// </summary>
        private bool isDragging;

        /// <summary>
        ///     The recording area.
        /// </summary>
        private Rect recordingArea;

        /// <summary>
        ///     The start position.
        /// </summary>
        private Point startPosition;

        /// <summary>
        /// The virtual screen height.
        /// </summary>
        private double screenHeight = SystemParameters.VirtualScreenHeight;

        /// <summary>
        /// The virtual screen width.
        /// </summary>
        private double screenWidth = SystemParameters.VirtualScreenWidth;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegionSelectorWindow" /> class.
        /// </summary>
        public RegionSelectorWindow()
        {
            this.InitializeComponent();
            this.Top = SystemParameters.VirtualScreenTop;
            this.Left = SystemParameters.VirtualScreenLeft;
            this.Width = this.screenWidth;
            this.Height = this.screenHeight;
            this.recordingArea = new Rect();
            this.UpdateUI();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the MouseDown event of the OverlayGrid control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="MouseButtonEventArgs"/> instance containing the event data.
        /// </param>
        private void OverlayGridMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.isDragging = true;
            this.startPosition = this.endPosition = e.GetPosition((IInputElement)sender);
            this.UpdatePosition();
        }

        /// <summary>
        /// Handles the OnMouseMove event of the OverlayGrid control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="MouseEventArgs"/> instance containing the event data.
        /// </param>
        private void OverlayGridOnMouseMove(object sender, MouseEventArgs e)
        {
            if (this.isDragging == false)
            {
                return;
            }

            this.endPosition = e.GetPosition((IInputElement)sender);
            this.UpdatePosition();
        }

        /// <summary>
        /// Handles the OnMouseUp event of the OverlayGrid control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="MouseButtonEventArgs"/> instance containing the event data.
        /// </param>
        private void OverlayGridOnMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.isDragging = false;
            this.UpdatePosition();
        }

        /// <summary>
        ///     Updates the position.
        /// </summary>
        private void UpdatePosition()
        {
            var x = Math.Min(this.startPosition.X, this.endPosition.X);
            var y = Math.Min(this.startPosition.Y, this.endPosition.Y);
            var width = Math.Abs(this.startPosition.X - this.endPosition.X);
            var height = Math.Abs(this.startPosition.Y - this.endPosition.Y);
            this.recordingArea = new Rect(x, y, width, height);
            this.UpdateUI();
        }

        /// <summary>
        ///     Updates the UI.
        /// </summary>
        private void UpdateUI()
        {
            this.LeftColumn.Width = new GridLength(this.recordingArea.Left);
            this.RightColumn.Width = new GridLength(this.screenWidth - this.recordingArea.Right);
            this.TopRow.Height = new GridLength(this.recordingArea.Top);
            this.BottomRow.Height = new GridLength(this.screenHeight - this.recordingArea.Bottom);
        }

        #endregion
    }
}