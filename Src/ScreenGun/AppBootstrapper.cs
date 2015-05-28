// ScreenGun
// - AppBootstrapper.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

using Caliburn.Micro;

using ScreenGun.Modules.Main;
using ScreenGun.Modules.Recorder;
using ScreenGun.Recorder;
using ScreenGun.Recorder.Capturers.GDIPlus;

namespace ScreenGun
{
    /// <summary>
    ///     The app bootstrapper.
    /// </summary>
    public class AppBootstrapper : BootstrapperBase
    {
        #region Fields

        /// <summary>
        ///     The container.
        /// </summary>
        private SimpleContainer container;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AppBootstrapper" /> class.
        /// </summary>
        public AppBootstrapper()
        {
            this.Initialize();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The build up.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        protected override void BuildUp(object instance)
        {
            this.container.BuildUp(instance);
        }

        /// <summary>
        ///     The configure.
        /// </summary>
        protected override void Configure()
        {
            this.container = new SimpleContainer();

            this.container.Instance(this.CreateRecorder());
            this.container.Singleton<IWindowManager, WindowManager>();
            this.container.Singleton<IEventAggregator, EventAggregator>();
            this.container.PerRequest<RecorderViewModel>();
            this.container.PerRequest<IShell, ShellViewModel>();
        }

        /// <summary>
        /// Creates the recorder.
        /// </summary>
        /// <returns></returns>
        private IScreenRecorder CreateRecorder()
        {
            string ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg.exe");
            var capturer = new GDIPlusFrameCaptureBackend();
            var opts = new FFMPEGScreenRecorderOptions(ffmpegPath, capturer);
            return new FFMPEGScreenRecorder(opts);
        }

        /// <summary>
        /// The get all instances.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return this.container.GetAllInstances(service);
        }

        /// <summary>
        /// The get instance.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        protected override object GetInstance(Type service, string key)
        {
            var instance = this.container.GetInstance(service, key);
            if (instance != null)
            {
                return instance;
            }

            throw new InvalidOperationException("Could not locate any instances.");
        }

        /// <summary>
        /// The on startup.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            this.DisplayRootViewFor<IShell>();
        }

        #endregion
    }
}