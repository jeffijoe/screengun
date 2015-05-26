// ScreenGun
// - ScreenGunFileViewModel.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;

using ScreenGun.Base;

namespace ScreenGun.Modules.Main.ScreenGunFile
{
    /// <summary>
    ///     The screen gun file.
    /// </summary>
    public class ScreenGunFileViewModel : ViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScreenGunFileViewModel" /> class.
        /// </summary>
        public ScreenGunFileViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenGunFileViewModel"/> class.
        /// </summary>
        /// <param name="fileName">
        /// Name of the file.
        /// </param>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        public ScreenGunFileViewModel(string fileName, string filePath)
        {
            this.FileName = fileName;
            this.FilePath = filePath;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     Gets or sets the file path.
        /// </summary>
        public string FilePath { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The delete.
        /// </summary>
        public void Delete()
        {
            Console.WriteLine("Delete file..");
        }

        #endregion
    }
}