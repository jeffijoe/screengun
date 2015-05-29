// ScreenGun
// - BooleanToColorConverter.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ScreenGun.Converters
{
    /// <summary>
    ///     The boolean to color converter.
    /// </summary>
    public class BooleanToColorConverter : DependencyObject, IValueConverter
    {
        #region Static Fields

        /// <summary>
        ///     The false color property.
        /// </summary>
        public static readonly DependencyProperty FalseColorProperty = DependencyProperty.Register(
            "FalseColor", 
            typeof(Brush), 
            typeof(BooleanToColorConverter), 
            new PropertyMetadata(default(Brush)));

        /// <summary>
        ///     The true color property.
        /// </summary>
        public static readonly DependencyProperty TrueColorProperty = DependencyProperty.Register(
            "TrueColor", 
            typeof(Brush), 
            typeof(BooleanToColorConverter), 
            new PropertyMetadata(default(Brush)));

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the false color.
        /// </summary>
        public Brush FalseColor
        {
            get
            {
                return (Brush)this.GetValue(FalseColorProperty);
            }

            set
            {
                this.SetValue(FalseColorProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the true color.
        /// </summary>
        public Brush TrueColor
        {
            get
            {
                return (Brush)this.GetValue(TrueColorProperty);
            }

            set
            {
                this.SetValue(TrueColorProperty, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">
        /// The value produced by the binding source.
        /// </param>
        /// <param name="targetType">
        /// The type of the binding target property.
        /// </param>
        /// <param name="parameter">
        /// The converter parameter to use.
        /// </param>
        /// <param name="culture">
        /// The culture to use in the converter.
        /// </param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// I only work with bools, yo.
        /// </exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                throw new InvalidOperationException("I only work with bools, yo.");
            }

            var b = (bool)value;
            return b ? this.TrueColor : this.FalseColor;
        }

        /// <summary>
        /// The convert back.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}