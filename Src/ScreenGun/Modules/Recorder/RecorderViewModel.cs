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
using System.Windows.Forms;

using PropertyChanged;

using ScreenGun.Modules.NotifyIcon;
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
        ///     The recorder.
        /// </summary>
        private readonly IScreenRecorder recorder;

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
        public RecorderViewModel(IScreenRecorder recorder)
        {
            this.recorder = recorder;
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
            this.IsRecording = true;

            Console.WriteLine("Starting");
            var opts = new ScreenRecorderOptions(this.RecordingRegion)
            {
                DeleteMaterialWhenDone = true, 
                FrameRate = 20, 
                MaterialTempFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MaterialTemp"), 
                OutputFilePath = "Recording.mp4", 
                RecordMicrophone = this.UseMicrophone
            };
            this.recorder.Start(opts);
        }

        /// <summary>
        ///     Stops the recording.
        /// </summary>
        public async void StopRecording()
        {
            if (this.IsRecording)
            {
                Console.WriteLine("Stopping");
                await this.recorder.StopAsync();
                this.IsRecording = false;
            }
        }

        /// <summary>
        ///     Toggles the full screen.
        /// </summary>
        public void ToggleFullScreen()
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
        /// Called the first time the page's LayoutUpdated event fires after it is navigated to.
        /// </summary>
        /// <param name="view">The view.</param>
        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            var icon =
                new Icon(
                    Application.GetResourceStream(new Uri("Resources/screengun_logo.ico", UriKind.Relative)).Stream);
            this.notifyIcon = new NotifyIconViewModel(icon);
            this.notifyIcon.ShowBalloonTip(
                3000, 
                "ScreenGun", 
                "When recording, click this icon to stop.", 
                ToolTipIcon.Info);
            this.notifyIcon.LeftClicked += (sender, args) => this.StopRecording();
        }

        #endregion
    }
}