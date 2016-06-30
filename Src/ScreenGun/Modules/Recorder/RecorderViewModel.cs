// ScreenGun
// - RecorderViewModel.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

using Caliburn.Micro;

using PropertyChanged;

using ScreenGun.Events;
using ScreenGun.Modules.Main.ScreenGunFile;
using ScreenGun.Modules.NotifyIcon;
using ScreenGun.Modules.Settings;
using ScreenGun.Recorder;

using Application = System.Windows.Application;
using Screen = Caliburn.Micro.Screen;

namespace ScreenGun.Modules.Recorder
{
    /// <summary>
    ///     View Model for the Recorder.
    /// </summary>
    [ImplementPropertyChanged]
    public class RecorderViewModel : Screen
    {
        #region Fields

        /// <summary>
        ///     The event aggregator.
        /// </summary>
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        ///     The recorder.
        /// </summary>
        private readonly IScreenRecorder recorder;

        /// <summary>
        ///     The settings.
        /// </summary>
        private readonly IScreenGunSettings settings;

        /// <summary>
        ///     The file view model.
        /// </summary>
        private ScreenGunFileViewModel fileViewModel;

        /// <summary>
        ///     The notify icon.
        /// </summary>
        private NotifyIconViewModel notifyIcon;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RecorderViewModel"/> class.
        /// </summary>
        /// <param name="recorder">
        /// The recorder.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="eventAggregator">
        /// The event aggregator.
        /// </param>
        public RecorderViewModel(
            IScreenRecorder recorder, 
            IScreenGunSettings settings, 
            IEventAggregator eventAggregator)
        {
            this.recorder = recorder;
            this.settings = settings;
            this.eventAggregator = eventAggregator;
            this.UseMicrophone = this.settings.DefaultMicEnabled;
            this.SetupNotifyIcon();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is full screen.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is full screen; otherwise, <c>false</c>.
        /// </value>
        public bool IsFullScreen { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether we are recording.
        /// </summary>
        public bool IsRecording { get; set; }

        /// <summary>
        ///     Gets or sets the recording region.
        /// </summary>
        /// <value>
        ///     The recording region.
        /// </value>
        public Rectangle RecordingRegion { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to use microphone or not.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [use microphone]; otherwise, <c>false</c>.
        /// </value>
        public bool UseMicrophone { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Cancels this instance.
        /// </summary>
        public void Cancel()
        {
            this.TryClose();
            this.eventAggregator.PublishOnUIThread(new RecordingViewClosed());
        }

        /// <summary>
        ///     Closes the recorder - can only be done when not recording.
        /// </summary>
        public void Close()
        {
            if (this.IsRecording == false)
            {
                this.Cancel();
            }
        }

        /// <summary>
        ///     Starts the recording.
        /// </summary>
        public void StartRecording()
        {
            this.notifyIcon.HideBalloonTip();
            this.IsRecording = true;

            var fileName = string.Format("Recording {0}.mp4", DateTime.Now.ToString("yy-MM-dd HH-mm-ss"));
            var outputFilePath = Path.Combine(this.settings.StoragePath, fileName);
            this.fileViewModel = new ScreenGunFileViewModel(outputFilePath, RecordingStage.DoingNothing);

            var opts = new ScreenRecorderOptions(this.RecordingRegion)
            {
                DeleteMaterialWhenDone = true, 
                OutputFilePath = outputFilePath, 
                RecordMicrophone = this.UseMicrophone,
                AudioRecordingDeviceNumber = this.settings.RecordingDeviceNumber
            };

            var progress = new Progress<RecorderState>(state => this.fileViewModel.RecordingStage = state.Stage);
            this.recorder.Start(opts, progress);
        }

        /// <summary>
        ///     Stops the recording.
        /// </summary>
        public async void StopRecording()
        {
            if (this.IsRecording)
            {
                this.TryClose();
                this.eventAggregator.PublishOnUIThread(new RecordingCreated(this.fileViewModel));
                var stopTask = this.recorder.StopAsync();
                this.eventAggregator.BeginPublishOnUIThread(new RecordingViewClosed());
                await stopTask;
                this.IsRecording = false;
                this.notifyIcon.Dispose();
            }
        }

        /// <summary>
        ///     Toggles the full screen.
        /// </summary>
        public void ToggleFullscreen()
        {
            this.IsFullScreen = !this.IsFullScreen;
        }

        /// <summary>
        ///     Toggles the microphone.
        /// </summary>
        public void ToggleMicrophone()
        {
            this.UseMicrophone = !this.UseMicrophone;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when deactivating.
        /// </summary>
        /// <param name="close">
        /// Inidicates whether this instance will be closed.
        /// </param>
        protected override void OnDeactivate(bool close)
        {
            this.notifyIcon.Dispose();
            base.OnDeactivate(close);
        }

        /// <summary>
        ///     Setups the notify icon.
        /// </summary>
        private void SetupNotifyIcon()
        {
            var icon =
                new Icon(
                    Application.GetResourceStream(
                        new Uri("Resources/record_icon.ico", UriKind.Relative)).Stream);
            this.notifyIcon = new NotifyIconViewModel(icon);
            this.notifyIcon.ShowBalloonTip(
                7000, 
                "ScreenGun", 
                "When recording, click this icon to stop.", 
                ToolTipIcon.Info);
            this.notifyIcon.LeftClicked += (sender, args) => this.StopRecording();
        }

        #endregion
    }
}