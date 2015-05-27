// ScreenGun
// - GDIStuff.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace ScreenGun.Recorder.Capturers.GDIPlus
{
    /// <summary>
    ///     The gdi stuff.
    /// </summary>
    internal class GDIStuff
    {
        #region Constants

        /// <summary>
        ///     The srccopy.
        /// </summary>
        public const int SRCCOPY = 13369376;

        #endregion

        #region Enums

        /// <summary>
        ///     Specifies a raster-operation code. These codes define how the color data for the
        ///     source rectangle is to be combined with the color data for the destination
        ///     rectangle to achieve the final color.
        /// </summary>
        public enum TernaryRasterOperations
        {
            /// <summary>dest = source</summary>
            SRCCOPY = 0x00CC0020, 

            /// <summary>dest = source OR dest</summary>
            SRCPAINT = 0x00EE0086, 

            /// <summary>dest = source AND dest</summary>
            SRCAND = 0x008800C6, 

            /// <summary>dest = source XOR dest</summary>
            SRCINVERT = 0x00660046, 

            /// <summary>dest = source AND (NOT dest)</summary>
            SRCERASE = 0x00440328, 

            /// <summary>dest = (NOT source)</summary>
            NOTSRCCOPY = 0x00330008, 

            /// <summary>dest = (NOT src) AND (NOT dest)</summary>
            NOTSRCERASE = 0x001100A6, 

            /// <summary>dest = (source AND pattern)</summary>
            MERGECOPY = 0x00C000CA, 

            /// <summary>dest = (NOT source) OR dest</summary>
            MERGEPAINT = 0x00BB0226, 

            /// <summary>dest = pattern</summary>
            PATCOPY = 0x00F00021, 

            /// <summary>dest = DPSnoo</summary>
            PATPAINT = 0x00FB0A09, 

            /// <summary>dest = pattern XOR dest</summary>
            PATINVERT = 0x005A0049, 

            /// <summary>dest = (NOT dest)</summary>
            DSTINVERT = 0x00550009, 

            /// <summary>dest = BLACK</summary>
            BLACKNESS = 0x00000042, 

            /// <summary>dest = WHITE</summary>
            WHITENESS = 0x00FF0062, 

            /// <summary>
            ///     Capture window as seen on screen.  This includes layered windows
            ///     such as WPF windows with AllowsTransparency="true"
            /// </summary>
            CAPTUREBLT = 0x40000000
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The bit blt.
        /// </summary>
        /// <param name="hdcDest">
        /// The hdc dest.
        /// </param>
        /// <param name="xDest">
        /// The x dest.
        /// </param>
        /// <param name="yDest">
        /// The y dest.
        /// </param>
        /// <param name="wDest">
        /// The w dest.
        /// </param>
        /// <param name="hDest">
        /// The h dest.
        /// </param>
        /// <param name="hdcSource">
        /// The hdc source.
        /// </param>
        /// <param name="xSrc">
        /// The x src.
        /// </param>
        /// <param name="ySrc">
        /// The y src.
        /// </param>
        /// <param name="RasterOp">
        /// The raster op.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        public static extern bool BitBlt(
            IntPtr hdcDest, 
            int xDest, 
            int yDest, 
            int wDest, 
            int hDest, 
            IntPtr hdcSource, 
            int xSrc, 
            int ySrc, 
            int RasterOp);

        /// <summary>
        /// The create compatible bitmap.
        /// </summary>
        /// <param name="hdc">
        /// The hdc.
        /// </param>
        /// <param name="nWidth">
        /// The n width.
        /// </param>
        /// <param name="nHeight">
        /// The n height.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap
            (IntPtr hdc, int nWidth, int nHeight);

        /// <summary>
        /// The create compatible dc.
        /// </summary>
        /// <param name="hdc">
        /// The hdc.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        /// <summary>
        /// The create dc.
        /// </summary>
        /// <param name="lpszDriver">
        /// The lpsz driver.
        /// </param>
        /// <param name="lpszDevice">
        /// The lpsz device.
        /// </param>
        /// <param name="lpszOutput">
        /// The lpsz output.
        /// </param>
        /// <param name="lpInitData">
        /// The lp init data.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport("gdi32.dll", EntryPoint = "CreateDC")]
        public static extern IntPtr CreateDC(IntPtr lpszDriver, string lpszDevice, IntPtr lpszOutput, IntPtr lpInitData);

        /// <summary>
        /// The delete dc.
        /// </summary>
        /// <param name="hDc">
        /// The h dc.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern IntPtr DeleteDC(IntPtr hDc);

        /// <summary>
        /// The delete object.
        /// </summary>
        /// <param name="hDc">
        /// The h dc.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern IntPtr DeleteObject(IntPtr hDc);

        /// <summary>
        /// The select object.
        /// </summary>
        /// <param name="hdc">
        /// The hdc.
        /// </param>
        /// <param name="bmp">
        /// The bmp.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);

        #endregion
    }
}