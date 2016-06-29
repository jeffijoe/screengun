using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenGun.Modules.Settings
{
    /// <summary>
    /// The recording device.
    /// </summary>
    public class RecordingDevice
    {
        /// <summary>
        /// The display name.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// The device number.
        /// </summary>
        public int DeviceNumber { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="deviceNumber"></param>
        /// <param name="displayName"></param>
        public RecordingDevice(int deviceNumber, string displayName)
        {
            this.DisplayName = displayName;
            this.DeviceNumber = deviceNumber;
        }
    }
}
