using System;

namespace ScreenGun.Modules.Settings
{
    public interface IScreenGunSettings
    {
        /// <summary>
        /// Gets or sets the framerate.
        /// </summary>
        /// <value>
        /// The framerate.
        /// </value>
        int Framerate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mic is enabled by default.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the mic is enabled by default; otherwise, <c>false</c>.
        /// </value>
        bool DefaultMicEnabled { get; set; }

        /// <summary>
        /// Gets or sets the storage path.
        /// </summary>
        /// <value>
        /// The storage path.
        /// </value>
        string StoragePath { get; set; }

        /// <summary>
        /// Occurs when the dialog is reset.
        /// </summary>
        event EventHandler DialogReset;
    }
}
