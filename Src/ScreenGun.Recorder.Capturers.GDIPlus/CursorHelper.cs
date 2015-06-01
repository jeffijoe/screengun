// ScreenGun
// - CursorHelper.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ScreenGun.Recorder.Capturers.GDIPlus
{
    /// <summary>
    ///     The rt global cursor.
    /// </summary>
    public class CursorHelper
    {
        #region Constants

        /// <summary>
        ///     The curso r_ showing.
        /// </summary>
        private const int CURSOR_SHOWING = 1;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The capture cursor.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <returns>
        /// The <see cref="Bitmap"/>.
        /// </returns>
        public static Bitmap CaptureCursor(ref int x, ref int y)
        {
            Win32Stuff.CURSORINFO cursorInfo = new Win32Stuff.CURSORINFO();
            cursorInfo.cbSize = Marshal.SizeOf(cursorInfo);
            if (!Win32Stuff.GetCursorInfo(out cursorInfo))
            {
                return null;
            }

            if (cursorInfo.flags != Win32Stuff.CURSOR_SHOWING)
            {
                return null;
            }

            IntPtr hicon = Win32Stuff.CopyIcon(cursorInfo.hCursor);
            if (hicon == IntPtr.Zero)
            {
                return null;
            }

            Win32Stuff.ICONINFO iconInfo;
            if (!Win32Stuff.GetIconInfo(hicon, out iconInfo))
            {
                return null;
            }

            x = cursorInfo.ptScreenPos.x - ((int)iconInfo.xHotspot);
            y = cursorInfo.ptScreenPos.y - ((int)iconInfo.yHotspot);

            using (Bitmap maskBitmap = Bitmap.FromHbitmap(iconInfo.hbmMask))
            {
                // Is this a monochrome cursor?
                if (maskBitmap.Height == maskBitmap.Width * 2)
                {
                    Bitmap resultBitmap = new Bitmap(maskBitmap.Width, maskBitmap.Width);

                    Graphics desktopGraphics = Graphics.FromHwnd(Win32Stuff.GetDesktopWindow());
                    IntPtr desktopHdc = desktopGraphics.GetHdc();

                    IntPtr maskHdc = GDIStuff.CreateCompatibleDC(desktopHdc);
                    IntPtr oldPtr = GDIStuff.SelectObject(maskHdc, maskBitmap.GetHbitmap());

                    using (Graphics resultGraphics = Graphics.FromImage(resultBitmap))
                    {
                        IntPtr resultHdc = resultGraphics.GetHdc();

                        // These two operation will result in a black cursor over a white background.
                        // Later in the code, a call to MakeTransparent() will get rid of the white background.
                        GDIStuff.BitBlt(
                            resultHdc, 
                            0, 
                            0, 
                            32, 
                            32, 
                            maskHdc, 
                            0, 
                            32, 
                            (int)GDIStuff.TernaryRasterOperations.SRCCOPY);
                        GDIStuff.BitBlt(
                            resultHdc, 
                            0, 
                            0, 
                            32, 
                            32, 
                            maskHdc, 
                            0, 
                            0, 
                            (int)GDIStuff.TernaryRasterOperations.SRCINVERT);

                        resultGraphics.ReleaseHdc(resultHdc);
                    }

                    IntPtr newPtr = GDIStuff.SelectObject(maskHdc, oldPtr);
                    GDIStuff.DeleteObject(newPtr);
                    GDIStuff.DeleteDC(maskHdc);
                    desktopGraphics.ReleaseHdc(desktopHdc);

                    // Remove the white background from the BitBlt calls,
                    // resulting in a black cursor over a transparent background.
                    resultBitmap.MakeTransparent(Color.White);
                    return resultBitmap;
                }
            }

            //// Delete the mask, if present.
            // if (iconInfo.hbmMask != IntPtr.Zero)
            // {
            // DeleteObject(iconInfo.hbmMask);
            // }

            //// Delete the color bitmap, if present.
            // if (iconInfo.hbmColor != IntPtr.Zero)
            // {
            // DeleteObject(iconInfo.hbmColor);
            // }
            using (Icon icon = Icon.FromHandle(hicon))
            {
                return icon.ToBitmap();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The copy icon.
        /// </summary>
        /// <param name="hIcon">
        /// The h icon.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport("user32.dll")]
        private static extern IntPtr CopyIcon(IntPtr hIcon);

        /// <summary>
        /// The delete object.
        /// </summary>
        /// <param name="hDc">
        /// The h dc.
        /// </param>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        [DllImport("gdi32.dll")]
        private static extern IntPtr DeleteObject(IntPtr hDc);

        /// <summary>
        /// The destroy icon.
        /// </summary>
        /// <param name="hIcon">
        /// The h icon.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DllImport("user32.dll")]
        private static extern bool DestroyIcon(IntPtr hIcon);

        /// <summary>
        /// The get cursor info.
        /// </summary>
        /// <param name="pci">
        /// The pci.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DllImport("user32.dll")]
        private static extern bool GetCursorInfo(out CURSORINFO pci);

        /// <summary>
        ///     The get gdi handle count.
        /// </summary>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        private static int GetGDIHandleCount()
        {
            return GetGuiResources(Process.GetCurrentProcess().Handle, 0);
        }

        /// <summary>
        /// The get gui resources.
        /// </summary>
        /// <param name="hProcess">
        /// The h process.
        /// </param>
        /// <param name="uiFlags">
        /// The ui flags.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        [DllImport("user32.dll")]
        private static extern int GetGuiResources(IntPtr hProcess, int uiFlags);

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
        [DllImport("user32.dll")]
        private static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

        /// <summary>
        ///     The get user handle count.
        /// </summary>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        private static int GetUserHandleCount()
        {
            return GetGuiResources(Process.GetCurrentProcess().Handle, 1);
        }

        /// <summary>
        /// The handle message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        private static void HandleMessage(string message)
        {
            Debug.WriteLine("HC: " + message + ": GDI: " + GetGDIHandleCount() + ": User: " + GetUserHandleCount());
        }

        #endregion

        /// <summary>
        ///     The cursorinfo.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct CURSORINFO
        {
            // Fields
            /// <summary>
            ///     The cb size.
            /// </summary>
            [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here."),
            SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
            public int cbSize;

            /// <summary>
            ///     The flags.
            /// </summary>
            [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here."),
            SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
            public int flags;

            /// <summary>
            ///     The h cursor.
            /// </summary>
            [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here."),
            SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
            public IntPtr hCursor;

            /// <summary>
            ///     The pt screen pos.
            /// </summary>
            [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here."),
            SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
            public POINT ptScreenPos;
        }

        /// <summary>
        ///     The iconinfo.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct ICONINFO
        {
            // Fields
            /// <summary>
            ///     The f icon.
            /// </summary>
            public bool fIcon;

            /// <summary>
            ///     The x hotspot.
            /// </summary>
            public int xHotspot;

            /// <summary>
            ///     The y hotspot.
            /// </summary>
            public int yHotspot;

            // Handle of the icon’s bitmask bitmap.
            /// <summary>
            ///     The hbm mask.
            /// </summary>
            public IntPtr hbmMask;

            // Handle of the icon’s color bitmap. Optional for monochrome icons.
            /// <summary>
            ///     The hbm color.
            /// </summary>
            public IntPtr hbmColor;
        }

        /// <summary>
        ///     The point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            // Fields
            /// <summary>
            ///     The x.
            /// </summary>
            public int x;

            /// <summary>
            ///     The y.
            /// </summary>
            public int y;
        }

        ///// <summary>
        ///// The capture cursor.
        ///// </summary>
        ///// <param name="x">
        ///// The x.
        ///// </param>
        ///// <param name="y">
        ///// The y.
        ///// </param>
        ///// <returns>
        ///// The <see cref="Bitmap"/>.
        ///// </returns>
        // public static Bitmap CaptureCursor(ref int x, ref int y)
        // {
        // try
        // {
        // // Return value initially nothing
        // Bitmap bmp = null;

        // CURSORINFO curInfo = new CURSORINFO();
        // curInfo.cbSize = Marshal.SizeOf(curInfo);

        // // HandleMessage("Start")
        // if (GetCursorInfo(ref curInfo))
        // {
        // if (curInfo.flags == CURSOR_SHOWING)
        // {
        // IntPtr hicon = CopyIcon(curInfo.hCursor);

        // if (hicon != IntPtr.Zero)
        // {
        // ICONINFO icoInfo = default(ICONINFO);
        // if (GetIconInfo(hicon, ref icoInfo))
        // {
        // // Delete the mask, if present.
        // if (icoInfo.hbmMask != IntPtr.Zero)
        // {
        // DeleteObject(icoInfo.hbmMask);
        // }

        // // Delete the color bitmap, if present.
        // if (icoInfo.hbmColor != IntPtr.Zero)
        // {
        // DeleteObject(icoInfo.hbmColor);
        // }

        // x = curInfo.ptScreenPos.x - icoInfo.xHotspot;
        // y = curInfo.ptScreenPos.y - icoInfo.yHotspot;
        // }

        // Icon ic = Icon.FromHandle(hicon);
        // bmp = ic.ToBitmap();

        // // Must destroy the icon object we got from CopyIcon
        // DestroyIcon(hicon);
        // }
        // }
        // }

        // // HandleMessage("End")
        // return bmp;
        // }
        // catch
        // {
        // return null;
        // }
        // }
    }
}