// ScreenGun
// - ScreenGunFileViewModel.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using ScreenGun.Base;
using ScreenGun.Properties;
using ScreenGun.Recorder;

namespace ScreenGun.Modules.Main.ScreenGunFile
{
    /// <summary>
    ///     The screen gun file.
    /// </summary>
    public class ScreenGunFileViewModel : ViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScreenGunFileViewModel" /> class.
        /// </summary>
        public ScreenGunFileViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenGunFileViewModel"/> class.
        /// </summary>
        /// <param name="outputFilePath">
        /// The output file path.
        /// </param>
        /// <param name="stage">
        /// The stage.
        /// </param>
        public ScreenGunFileViewModel(string outputFilePath, RecordingStage stage = RecordingStage.Done)
        {
            this.FileName = Path.GetFileName(outputFilePath);
            this.FilePath = outputFilePath;
            this.RecordingStage = stage;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///     Occurs when recording has been deleted.
        /// </summary>
        public event RecordingDeletedHandler RecordingDeleted;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets a value indicating whether this instance can delete.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance can delete; otherwise, <c>false</c>.
        /// </value>
        public bool CanDelete
        {
            get
            {
                return this.RecordingStage == RecordingStage.Done;
            }
        }

        /// <summary>
        ///     Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     Gets or sets the file path.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        ///     Gets or sets the recording stage.
        /// </summary>
        /// <value>
        ///     The recording stage.
        /// </value>
        public RecordingStage RecordingStage { get; set; }

        /// <summary>
        ///     Gets a value indicating whether the recording stage should be shown.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [should show recording stage]; otherwise, <c>false</c>.
        /// </value>
        public bool ShouldShowRecordingStage
        {
            get
            {
                return this.RecordingStage != RecordingStage.Done;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The delete.
        /// </summary>
        public void Delete()
        {
            File.Delete(this.FilePath);
            var handler = this.RecordingDeleted;
            if (handler != null)
            {
                handler.Invoke(this);
            }

            this.RecordingDeleted = null;
        }

        /// <summary>
        /// Opens the video.
        /// </summary>
        public void OpenVideo()
        {
            // If we can delete, then we deffo can open it as well.
            if (this.CanDelete)
            {
                var info = new ProcessStartInfo(this.FilePath) { UseShellExecute = true };
                Process.Start(info);
            }
        }

        #endregion
    }
}