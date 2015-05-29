// ScreenGun
// - BetterBooleanToVisibilityConverter.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ScreenGun.Converters
{
    /// <summary>
    ///     The better boolean to visibility converter.
    /// </summary>
    public class BetterBooleanToVisibilityConverter : DependencyObject, IValueConverter
    {
        #region Static Fields

        /// <summary>
        ///     The false visibility property.
        /// </summary>
        public static readonly DependencyProperty FalseVisibilityProperty = DependencyProperty.Register(
            "FalseVisibility", 
            typeof(Visibility), 
            typeof(BetterBooleanToVisibilityConverter), 
            new PropertyMetadata(Visibility.Hidden));

        /// <summary>
        ///     The true visibility property.
        /// </summary>
        public static readonly DependencyProperty TrueVisibilityProperty = DependencyProperty.Register(
            "TrueVisibility", 
            typeof(Visibility), 
            typeof(BetterBooleanToVisibilityConverter), 
            new PropertyMetadata(Visibility.Visible));

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the false visibility.
        /// </summary>
        public Visibility FalseVisibility
        {
            get
            {
                return (Visibility)this.GetValue(FalseVisibilityProperty);
            }

            set
            {
                this.SetValue(FalseVisibilityProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the true visibility.
        /// </summary>
        public Visibility TrueVisibility
        {
            get
            {
                return (Visibility)this.GetValue(TrueVisibilityProperty);
            }

            set
            {
                this.SetValue(TrueVisibilityProperty, value);
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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                throw new InvalidOperationException("I handle bools, nothing else. Go away.");
            }

            var b = (bool)value;
            return b ? this.TrueVisibility : this.FalseVisibility;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">
        /// The value that is produced by the binding target.
        /// </param>
        /// <param name="targetType">
        /// The type to convert to.
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
        /// <exception cref="System.NotImplementedException">
        /// </exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}