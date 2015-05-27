// ScreenGun
// - ViewModel.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;
using System.ComponentModel;

using Caliburn.Micro;

using PropertyChanged;

namespace ScreenGun.Base
{
    /// <summary>
    ///     The view model.
    /// </summary>
    [ImplementPropertyChanged]
    public class ViewModel : PropertyChangedBase
    {
        #region Methods

        /// <summary>
        /// The add property changed event.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected void AddPropertyChangedEvent(string propertyName, Action<PropertyChangedEventArgs> action)
        {
            this.PropertyChanged += (sender, args) => {
                if (args.PropertyName.ToUpper() != propertyName.ToUpper())
                {
                    return;
                }

                action(args);
            };
        }

        #endregion
    }
}