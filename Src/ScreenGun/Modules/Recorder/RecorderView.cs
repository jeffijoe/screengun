// ScreenGun
// - RecorderView.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

using ScreenGun.Misc;
using ScreenGun.Modules.RegionSelector;

namespace ScreenGun.Modules.Recorder
{
    /// <summary>
    ///     The screenshot view.
    /// </summary>
    public class RecorderView : RegionSelectorWindow
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecorderView" /> class.
        /// </summary>
        public RecorderView()
        {
            this.RegionChange += this.OnRegionChange;
            this.FullScreenChanged += this.OnFullScreenChange;
            this.DataContextChanged += this.OnDataContextChanged;
            this.Title = "New Recording";
            var command = new Command(() => this.ViewModel.Close());
            this.InputBindings.Add(new KeyBinding(command, Key.Escape, ModifierKeys.None));
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the view model.
        /// </summary>
        /// <value>
        ///     The view model.
        /// </value>
        public RecorderViewModel ViewModel
        {
            get
            {
                return (RecorderViewModel)this.DataContext;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when [data context changed].
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="dependencyPropertyChangedEventArgs">
        /// The <see cref="DependencyPropertyChangedEventArgs"/> instance
        ///     containing the event data.
        /// </param>
        private void OnDataContextChanged(
            object sender, 
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            this.ViewModel.PropertyChanged += this.ViewModelOnPropertyChanged;
        }

        /// <summary>
        /// Called when full screen changes.
        /// </summary>
        /// <param name="args">
        /// The arguments.
        /// </param>
        private void OnFullScreenChange(FullScreenChangedArgs args)
        {
            // ReSharper disable once RedundantCheckBeforeAssignment
            if (this.ViewModel.IsFullScreen != args.IsFullScreen)
            {
                this.ViewModel.IsFullScreen = args.IsFullScreen;
            }
        }

        /// <summary>
        /// Called when the region changes.
        /// </summary>
        /// <param name="args">
        /// The arguments.
        /// </param>
        private void OnRegionChange(RegionChangeArgs args)
        {
            this.ViewModel.RecordingRegion = args.Region;
        }

        /// <summary>
        /// The view model on property changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="propertyChangedEventArgs">
        /// The property changed event args.
        /// </param>
        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "IsRecording")
            {
                this.Locked = this.ViewModel.IsRecording;
            }

            if (propertyChangedEventArgs.PropertyName == "IsFullScreen")
            {
                this.IsFullScreen = this.ViewModel.IsFullScreen;
            }
        }

        #endregion
    }
}