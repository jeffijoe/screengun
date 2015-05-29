// ScreenGun
// - IconMicOff.xaml.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScreenGun.Icons
{
    /// <summary>
    ///     Interaction logic for ic_mic_off_24px.xaml
    /// </summary>
    public partial class IconMicOff : UserControl
    {
        #region Static Fields

        /// <summary>
        /// The icon background property.
        /// </summary>
        public static readonly DependencyProperty IconBackgroundProperty = DependencyProperty.Register(
            "IconBackground", 
            typeof(Brush), 
            typeof(IconMicOff), 
            new PropertyMetadata(Brushes.White));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IconMicOff"/> class.
        /// </summary>
        public IconMicOff()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the icon background.
        /// </summary>
        public Brush IconBackground
        {
            get
            {
                return (Brush)this.GetValue(IconBackgroundProperty);
            }

            set
            {
                this.SetValue(IconBackgroundProperty, value);
            }
        }

        #endregion
    }
}