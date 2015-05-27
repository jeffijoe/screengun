// ScreenGun
// - NotifyIconViewModel.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.
namespace ScreenGun.Modules.NotifyIcon
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    using ScreenGun.Base;

    /// <summary>
    /// The notify icon view model.
    /// </summary>
    public class NotifyIconViewModel : ViewModel, IDisposable
    {
        #region Fields

        /// <summary>
        ///     The actual icon
        /// </summary>
        private NotifyIcon actualIcon;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyIconViewModel"/> class.
        /// </summary>
        /// <param name="icon">
        /// The icon.
        /// </param>
        public NotifyIconViewModel(Icon icon)
            : this()
        {
            this.Icon = icon;
            this.actualIcon.Visible = true;
        }

        /// <summary>
        ///     Prevents a default instance of the <see cref="NotifyIconViewModel" /> class from being created.
        ///     <para>Sets up default values</para>
        /// </summary>
        private NotifyIconViewModel()
        {
            this.actualIcon = new NotifyIcon();
            this.actualIcon.MouseClick += (sender, args) =>
            {
                switch (args.Button)
                {
                    case MouseButtons.Left:
                        this.OnLeftClicked();
                        break;
                    case MouseButtons.Right:
                        this.OnRightClicked();
                        break;
                }
            };
            this.AddPropertyChangedEvent(
                "Icon",
                args =>
                {
                    if (this.actualIcon == null)
                    {
                        return;
                    }

                    this.actualIcon.Icon = this.Icon;
                });
        }

        #endregion

        /// <summary>
        /// Shows the balloon tip.
        /// </summary>
        /// <param name="durationMs">The duration ms.</param>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="toolTipIcon">The tool tip icon.</param>
        /// <param name="clicked">The click event to invoke when the tooltip is clicked.</param>
        public void ShowBalloonTip(int durationMs, string title, string text, ToolTipIcon toolTipIcon, EventHandler clicked)
        {
            EventHandler onClicked = null;
            EventHandler onClosed = null;

            Action disposeTooltip = () => {
                this.actualIcon.BalloonTipClosed -= onClosed;
                this.actualIcon.BalloonTipClicked -= onClicked;
            };

            onClosed = (sender, args) => disposeTooltip();
            onClicked = (sender, args) =>
            {
                clicked(sender, args);
                disposeTooltip();
            };

            this.actualIcon.BalloonTipClicked += onClicked;
            this.actualIcon.BalloonTipClosed += onClosed;

            this.actualIcon.ShowBalloonTip(durationMs, title, text, toolTipIcon);
        }

        #region Public Events

        /// <summary>
        ///     Occurs when [left clicked].
        /// </summary>
        public event EventHandler LeftClicked;

        /// <summary>
        ///     Occurs when [right clicked].
        /// </summary>
        public event EventHandler RightClicked;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the icon.
        /// </summary>
        /// <value>
        ///     The icon.
        /// </value>
        public Icon Icon { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.actualIcon != null)
            {
                this.actualIcon.Dispose();
                this.actualIcon = null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [left clicked].
        /// </summary>
        protected virtual void OnLeftClicked()
        {
            var handler = this.LeftClicked;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Called when [right clicked].
        /// </summary>
        protected virtual void OnRightClicked()
        {
            var handler = this.RightClicked;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}