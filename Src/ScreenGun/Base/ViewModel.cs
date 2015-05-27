using Caliburn.Micro;

using PropertyChanged;

namespace ScreenGun.Base
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// The view model.
    /// </summary>
    [ImplementPropertyChanged]
    public class ViewModel : PropertyChangedBase
    {
        protected void AddPropertyChangedEvent(string propertyName, Action<PropertyChangedEventArgs> action)
        {
            this.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.ToUpper() != propertyName.ToUpper())
                {
                    return;
                }
                action(args);
            };
        }
    }
}