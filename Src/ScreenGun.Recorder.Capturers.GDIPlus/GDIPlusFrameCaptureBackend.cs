// ScreenGun
// - GDIPlusFrameCaptureBackend.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ScreenGun.Recorder.Capturers.GDIPlus
{
    /// <summary>
    ///     GDI+ Back-end.
    /// </summary>
    public class GDIPlusFrameCaptureBackend : IFrameCaptureBackend
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
        public Bitmap CaptureFrame(Rectangle region)
        {
            // Create a new bitmap.
            var frameBitmap = new Bitmap(
                region.Width, 
                region.Height, 
                PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            using (var gfxScreenshot = Graphics.FromImage(frameBitmap))
            {
                // Take the screenshot from the upper left corner to the right bottom corner.
                gfxScreenshot.CopyFromScreen(
                    region.X, 
                    region.Y, 
                    0, 
                    0, 
                    region.Size, 
                    CopyPixelOperation.SourceCopy);

                Point position = Cursor.Position;
                var x = position.X;
                var y = position.Y;
                var cursorBmp = CursorHelper.CaptureCursor(ref x, ref y);

                // We need to offset the cursor position by the region, to position it correctly
                // in the image.
                position = new Point(
                    x - region.X, 
                    y - region.Y);
                if (cursorBmp != null)
                {
                    gfxScreenshot.DrawImage(cursorBmp, position);
                }

                cursorBmp.Dispose();
            }

            return frameBitmap;
        }

        #endregion
    }
}