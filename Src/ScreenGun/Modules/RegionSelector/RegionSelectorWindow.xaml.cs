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
    public partial class RegionSelectorWindow
    {
        #region Fields

        /// <summary>
        ///     The virtual screen height.
        /// </summary>
        private readonly double screenHeight = SystemParameters.VirtualScreenHeight;

        /// <summary>
        ///     The virtual screen width.
        /// </summary>
        private readonly double screenWidth = SystemParameters.VirtualScreenWidth;

        /// <summary>
        ///     The end position.
        /// </summary>
        private Point endPosition;

        /// <summary>
        ///     Flag used to determine if the region is being moved.
        /// </summary>
        private bool isMoving;

        /// <summary>
        ///     Flag to determine if the region is being resized.
        /// </summary>
        private bool isResizing;

        /// <summary>
        ///     The recording area.
        /// </summary>
        private Rect recordingArea;

        /// <summary>
        ///     The start position.
        /// </summary>
        private Point startPosition;

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
            if (this.isResizing)
            {
                return;
            }

            this.isResizing = true;
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
            if (this.isResizing == false)
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
            this.isResizing = false;
            this.UpdatePosition();
        }

        /// <summary>
        /// Handles the MouseLeftDown event of the RecordingRegion control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="MouseButtonEventArgs"/> instance containing the event data.
        /// </param>
        private void RecordingRegionMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            this.isMoving = true;
        }

        /// <summary>
        /// Handles the MouseLeftUp event of the RecordingRegion control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="MouseButtonEventArgs"/> instance containing the event data.
        /// </param>
        private void RecordingRegionMouseLeftUp(object sender, MouseButtonEventArgs e)
        {
            this.isMoving = false;
        }

        /// <summary>
        /// Handles the MouseMove event of the RecordingRegion control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="MouseEventArgs"/> instance containing the event data.
        /// </param>
        private void RecordingRegionMouseMove(object sender, MouseEventArgs e)
        {
            if (this.isMoving == false)
            {
                return;
            }
        }

        /// <summary>
        /// Handles the MouseDown event of ResizeGrips.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="MouseButtonEventArgs"/> instance containing the event data.
        /// </param>
        private void ResizeGripMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(this.OverlayGrid);
            this.isResizing = true;
            this.endPosition = position;

            var edges = new[]
            {
                this.recordingArea.TopLeft, 
                this.recordingArea.TopRight, 
                this.recordingArea.BottomLeft, 
                this.recordingArea.BottomRight
            };

            var furthestAway = new Point();
            double lastTotal = 0;
            foreach (var edge in edges)
            {
                double absX = Math.Abs(edge.X - position.X);
                double absY = Math.Abs(edge.Y - position.Y);
                var total = absX + absY;
                if (total > lastTotal)
                {
                    lastTotal = total;
                    furthestAway = edge;
                }
            }

            this.startPosition = furthestAway;
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