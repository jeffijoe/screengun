// ScreenGun
// - RegionChangeArgs.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System.Drawing;

namespace ScreenGun.Modules.RegionSelector
{
    /// <summary>
    ///     The region change args.
    /// </summary>
    public class RegionChangeArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionChangeArgs"/> class.
        /// </summary>
        /// <param name="region">
        /// The region.
        /// </param>
        public RegionChangeArgs(Rectangle region)
        {
            this.Region = region;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the region.
        /// </summary>
        public Rectangle Region { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Region: {0}", this.Region);
        }

        #endregion
    }
}