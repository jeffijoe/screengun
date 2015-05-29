// ScreenGun
// - FullScreenChangedArgs.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.
namespace ScreenGun.Modules.RegionSelector
{
    /// <summary>
    /// The full screen changed args.
    /// </summary>
    public class FullScreenChangedArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FullScreenChangedArgs"/> class.
        /// </summary>
        /// <param name="isFullScreen">
        /// The is full screen.
        /// </param>
        public FullScreenChangedArgs(bool isFullScreen)
        {
            this.IsFullScreen = isFullScreen;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether is full screen.
        /// </summary>
        public bool IsFullScreen { get; private set; }

        #endregion
    }
}