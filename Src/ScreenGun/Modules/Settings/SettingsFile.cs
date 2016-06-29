// ScreenGun
// - SettingsFile.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System.IO;

using Newtonsoft.Json;

namespace ScreenGun.Modules.Settings
{
    /// <summary>
    ///     The settings file.
    /// </summary>
    public class SettingsFile
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether default mic enabled.
        /// </summary>
        public bool DefaultMicEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the storage path.
        /// </summary>
        public string StoragePath { get; set; }

        /// <summary>
        /// Recording device number.
        /// </summary>
        public int RecordingDeviceNumber { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Froms the file.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        /// <returns>
        /// The settings file.
        /// </returns>
        public static SettingsFile FromFile(string filePath)
        {
            var readAllText = File.ReadAllText(filePath);
            var settingsFile = JsonConvert.DeserializeObject<SettingsFile>(readAllText);
            return settingsFile;
        }

        /// <summary>
        /// Creates a settings file from the given settings.
        /// </summary>
        /// <param name="settings">
        /// The settings view model.
        /// </param>
        /// <returns>
        /// The settings file.
        /// </returns>
        public static SettingsFile FromSettings(IScreenGunSettings settings)
        {
            return new SettingsFile
            {
                DefaultMicEnabled = settings.DefaultMicEnabled,
                StoragePath = settings.StoragePath,
                RecordingDeviceNumber = settings.RecordingDeviceNumber
            };
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        public static void SaveSettings(string filePath, IScreenGunSettings settings)
        {
            var settingsFile = FromSettings(settings);
            var serializeObject = JsonConvert.SerializeObject(settingsFile);
            File.WriteAllText(filePath, serializeObject);
        }

        #endregion
    }
}