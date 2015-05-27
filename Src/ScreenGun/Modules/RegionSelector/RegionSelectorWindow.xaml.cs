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
        /// The virtual screen.
        /// </summary>
        private readonly Rect virtualScreen = new Rect(
            new Point(
                SystemParameters.VirtualScreenLeft,
                SystemParameters.VirtualScreenTop),
            new Size(
                SystemParameters.VirtualScreenWidth,
                SystemParameters.VirtualScreenHeight));

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
        private Rect relativeRecordingArea;

        /// <summary>
        ///     The start position.
        /// </summary>
        private Point startPosition;

        /// <summary>
        /// The mouse position that was determined in the previous event. Used for calculating point deltas.
        /// </summary>
        private Point lastMousePosition;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegionSelectorWindow" /> class.
        /// </summary>
        public RegionSelectorWindow()
        {
            this.InitializeComponent();
            this.Top = this.virtualScreen.Top;
            this.Left = this.virtualScreen.Left;
            this.Width = this.virtualScreen.Width;
            this.Height = this.virtualScreen.Height;
            this.relativeRecordingArea = new Rect();
            this.UpdateUI();
        }

        #endregion

        /// <summary>
        /// Gets the recording area.
        /// </summary>
        /// <value>
        /// The recording area.
        /// </value>
        public Rect RecordingArea
        {
            get
            {
                return new Rect(
                    this.relativeRecordingArea.X + this.virtualScreen.X, 
                    this.relativeRecordingArea.Y + this.virtualScreen.Y, 
                    this.relativeRecordingArea.Width, 
                    this.relativeRecordingArea.Height);                
            }
        }

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
            if (this.isResizing || this.isMoving)
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
            this.lastMousePosition = e.GetPosition(this.OverlayGrid);
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

            var position = e.GetPosition(this.OverlayGrid);
            if (this.lastMousePosition == default(Point))
            {
                this.lastMousePosition = position;
            }

            var delta = position - this.lastMousePosition;
            this.lastMousePosition = position;
            this.startPosition += delta;
            this.endPosition += delta;
            this.UpdatePosition();
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
            if (this.isResizing)
            {
                return;
            }

            Point position = e.GetPosition(this.OverlayGrid);
            this.isResizing = true;
            this.endPosition = position;

            var edges = new[]
            {
                this.relativeRecordingArea.TopLeft, 
                this.relativeRecordingArea.TopRight, 
                this.relativeRecordingArea.BottomLeft, 
                this.relativeRecordingArea.BottomRight
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
            this.relativeRecordingArea = new Rect(x, y, width, height);

            // This makes sure that the recording area is never out of bounds.
            var relativeVirtualScreen = new Rect(
                this.virtualScreen.X,
                this.virtualScreen.Y,
                this.virtualScreen.Width,
                this.virtualScreen.Height);

            relativeVirtualScreen.Offset(Math.Abs(this.virtualScreen.X), Math.Abs(this.virtualScreen.Y));
            this.relativeRecordingArea.Intersect(relativeVirtualScreen);
            this.UpdateUI();
        }

        /// <summary>
        ///     Updates the UI.
        /// </summary>
        private void UpdateUI()
        {
            this.LeftColumn.Width = new GridLength(this.relativeRecordingArea.Left);
            this.RightColumn.Width = new GridLength(this.virtualScreen.Width - this.relativeRecordingArea.Right);
            this.TopRow.Height = new GridLength(this.relativeRecordingArea.Top);
            this.BottomRow.Height = new GridLength(this.virtualScreen.Height - this.relativeRecordingArea.Bottom);
        }

        #endregion
    }
}