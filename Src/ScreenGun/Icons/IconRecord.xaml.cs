// ScreenGun
// - IconRecord.xaml.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System.Windows;
using System.Windows.Media;

namespace ScreenGun.Icons
{
    /// <summary>
    ///     Interaction logic for IconRecord.xaml
    /// </summary>
    public partial class IconRecord
    {
        #region Static Fields

        /// <summary>
        ///     The inner color property.
        /// </summary>
        public static readonly DependencyProperty InnerColorProperty = DependencyProperty.Register(
            "InnerColor", 
            typeof(Brush), 
            typeof(IconRecord), 
            new PropertyMetadata(Brushes.Red));

        /// <summary>
        ///     The outer color property.
        /// </summary>
        public static readonly DependencyProperty OuterColorProperty = DependencyProperty.Register(
            "OuterColor", 
            typeof(Brush), 
            typeof(IconRecord), 
            new PropertyMetadata(Brushes.DarkSlateGray));

        /// <summary>
        ///     The size property.
        /// </summary>
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size", 
            typeof(double), 
            typeof(IconRecord), 
            new PropertyMetadata(16d));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="IconRecord" /> class.
        /// </summary>
        public IconRecord()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the inner color.
        /// </summary>
        public Brush InnerColor
        {
            get
            {
                return (Brush)this.GetValue(InnerColorProperty);
            }

            set
            {
                this.SetValue(InnerColorProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the outer color.
        /// </summary>
        public Brush OuterColor
        {
            get
            {
                return (Brush)this.GetValue(OuterColorProperty);
            }

            set
            {
                this.SetValue(OuterColorProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the size.
        /// </summary>
        public double Size
        {
            get
            {
                return (double)this.GetValue(SizeProperty);
            }

            set
            {
                this.SetValue(SizeProperty, value);
            }
        }

        /// <summary>
        /// Gets the size fourth.
        /// </summary>
        public double SizeEighth
        {
            get
            {
                return this.Size / 8;
            }
        }

        /// <summary>
        /// Gets the size half.
        /// </summary>
        public double SizeHalf
        {
            get
            {
                return this.Size / 2;
            }
        }

        #endregion
    }
}