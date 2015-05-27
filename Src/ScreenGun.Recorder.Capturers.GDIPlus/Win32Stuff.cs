// ScreenGun
// - Win32Stuff.cs
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
    ///     The win 32 stuff.
    /// </summary>
    internal class Win32Stuff
    {
        #region Constants

        /// <summary>
        ///     The curso r_ showing.
        /// </summary>
        public const int CURSOR_SHOWING = 0x00000001;

        /// <summary>
        ///     The s m_ cxscreen.
        /// </summary>
        public const int SM_CXSCREEN = 0;

        /// <summary>
        ///     The s m_ cyscreen.
        /// </summary>
        public const int SM_CYSCREEN = 1;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The copy icon.
        /// </summary>
        /// <param name="hIcon">
        /// The h icon.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport("user32.dll", EntryPoint = "CopyIcon")]
        public static extern IntPtr CopyIcon(IntPtr hIcon);

        /// <summary>
        /// The get cursor info.
        /// </summary>
        /// <param name="pci">
        /// The pci.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DllImport("user32.dll", EntryPoint = "GetCursorInfo")]
        public static extern bool GetCursorInfo(out CURSORINFO pci);

        /// <summary>
        /// The get dc.
        /// </summary>
        /// <param name="ptr">
        /// The ptr.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr ptr);

        /// <summary>
        ///     The get desktop window.
        /// </summary>
        /// <returns>
        ///     The <see cref="IntPtr" />.
        /// </returns>
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// The get icon info.
        /// </summary>
        /// <param name="hIcon">
        /// The h icon.
        /// </param>
        /// <param name="piconinfo">
        /// The piconinfo.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DllImport("user32.dll", EntryPoint = "GetIconInfo")]
        public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

        /// <summary>
        /// The get system metrics.
        /// </summary>
        /// <param name="abc">
        /// The abc.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int abc);

        /// <summary>
        /// The get window dc.
        /// </summary>
        /// <param name="ptr">
        /// The ptr.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport("user32.dll", EntryPoint = "GetWindowDC")]
        public static extern IntPtr GetWindowDC(int ptr);

        /// <summary>
        /// The release dc.
        /// </summary>
        /// <param name="hWnd">
        /// The h wnd.
        /// </param>
        /// <param name="hDc">
        /// The h dc.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        #endregion

        /// <summary>
        ///     The cursorinfo.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct CURSORINFO
        {
            /// <summary>
            ///     The cb size.
            /// </summary>
            public int cbSize; // Specifies the size, in bytes, of the structure. 

            /// <summary>
            ///     The flags.
            /// </summary>
            public int flags; // Specifies the cursor state. This parameter can be one of the following values:

            /// <summary>
            ///     The h cursor.
            /// </summary>
            public IntPtr hCursor; // Handle to the cursor. 

            /// <summary>
            ///     The pt screen pos.
            /// </summary>
            public POINT ptScreenPos; // A POINT structure that receives the screen coordinates of the cursor. 
        }

        /// <summary>
        ///     The iconinfo.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ICONINFO
        {
            /// <summary>
            ///     The f icon.
            /// </summary>
            public bool fIcon;

            // Specifies whether this structure defines an icon or a cursor. A value of TRUE specifies 

            /// <summary>
            ///     The x hotspot.
            /// </summary>
            public int xHotspot;

            // Specifies the x-coordinate of a cursor's hot spot. If this structure defines an icon, the hot 

            /// <summary>
            ///     The y hotspot.
            /// </summary>
            public int yHotspot;

            // Specifies the y-coordinate of the cursor's hot spot. If this structure defines an icon, the hot 

            /// <summary>
            ///     The hbm mask.
            /// </summary>
            public IntPtr hbmMask;

            // (HBITMAP) Specifies the icon bitmask bitmap. If this structure defines a black and white icon, 

            /// <summary>
            ///     The hbm color.
            /// </summary>
            public IntPtr hbmColor; // (HBITMAP) Handle to the icon color bitmap. This member can be optional if this 
        }

        /// <summary>
        ///     The point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>
            ///     The x.
            /// </summary>
            public int x;

            /// <summary>
            ///     The y.
            /// </summary>
            public int y;
        }
    }
}