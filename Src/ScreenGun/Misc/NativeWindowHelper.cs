// ScreenGun
// - NativeWindowHelper.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ScreenGun.Misc
{
    /// <summary>
    ///     The native window helper.
    /// </summary>
    public static class NativeWindowHelper
    {
        #region Constants

        /// <summary>
        ///     The gw l_ exstyle.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", 
            Justification = "Reviewed. Suppression is OK here.")]
        private const int GWL_EXSTYLE = -20;

        /// <summary>
        ///     The w s_ e x_ transparent.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", 
            Justification = "Reviewed. Suppression is OK here.")]
        private const int WS_EX_TRANSPARENT = 0x00000020;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The set window ex transparent.
        /// </summary>
        /// <param name="hwnd">
        /// The hwnd.
        /// </param>
        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get window long.
        /// </summary>
        /// <param name="hwnd">
        /// The hwnd.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        /// <summary>
        /// The set window long.
        /// </summary>
        /// <param name="hwnd">
        /// The hwnd.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="newStyle">
        /// The new style.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        #endregion
    }
}