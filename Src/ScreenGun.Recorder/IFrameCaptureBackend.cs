// ScreenGun
// - IFrameCaptureBackend.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System.Drawing;

namespace ScreenGun.Recorder
{
    /// <summary>
    ///     Interface for frame capturing back-ends.
    /// </summary>
    public interface IFrameCaptureBackend
    {
        #region Public Methods and Operators

        /// <summary>
        /// Captures a frame.
        /// </summary>
        /// <param name="region">
        /// The region.
        /// </param>
        /// <returns>
        /// A bitmap containing the captured frame.
        /// </returns>
        Bitmap CaptureFrame(Rectangle region);

        #endregion
    }
}