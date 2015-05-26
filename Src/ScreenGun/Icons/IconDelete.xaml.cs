// ScreenGun
// - IconDelete.xaml.cs
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
    ///     Interaction logic for icon_delete.xaml
    /// </summary>
    public partial class IconDelete : UserControl
    {
        #region Static Fields

        /// <summary>
        ///     The icon color property.
        /// </summary>
        public static readonly DependencyProperty IconColorProperty = DependencyProperty.Register(
            "IconColor", 
            typeof(Brush), 
            typeof(IconDelete), 
            new PropertyMetadata(default(Brush)));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="IconDelete" /> class.
        /// </summary>
        public IconDelete()
        {
            this.InitializeComponent();
            this.LayoutRoot.DataContext = this;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the icon color.
        /// </summary>
        public Brush IconColor
        {
            get
            {
                return (Brush)this.GetValue(IconColorProperty);
            }

            set
            {
                this.SetValue(IconColorProperty, value);
            }
        }

        #endregion
    }
}