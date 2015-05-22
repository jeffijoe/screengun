// ScreenGun
// - ShellViewModel.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System.Collections.ObjectModel;

using Caliburn.Micro;

using ScreenGun.Base;

namespace ScreenGun.Modules.Main
{
    /// <summary>
    ///     The shell view model.
    /// </summary>
    public class ShellViewModel : ViewModel, IShell
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        public ShellViewModel()
        {
            this.Files = new ObservableCollection<ScreenGunFile>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the files.
        /// </summary>
        /// <value>
        /// The files.
        /// </value>
        public ObservableCollection<ScreenGunFile> Files { get; set; }

        #endregion
    }
}