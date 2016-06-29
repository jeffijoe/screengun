// ScreenGun
// - ShellViewModel.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using Caliburn.Micro;

using Jeffijoe.MessageFormat;

using ScreenGun.Base;
using ScreenGun.Events;
using ScreenGun.Modules.Main.ScreenGunFile;
using ScreenGun.Modules.Recorder;
using ScreenGun.Modules.Settings;
using System;

namespace ScreenGun.Modules.Main
{
    /// <summary>
    ///     The shell view model.
    /// </summary>
    public class ShellViewModel : ViewModel, IHandle<RecordingCreated>, IHandle<RecordingViewClosed>
    {
        #region Fields

        /// <summary>
        ///     The settings.
        /// </summary>
        private readonly IScreenGunSettings settings;

        /// <summary>
        ///     The window manager.
        /// </summary>
        private readonly IWindowManager windowManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
        /// </summary>
        /// <param name="windowManager">
        /// The window Manager.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <param name="eventAggregator">
        /// The event Aggregator.
        /// </param>
        public ShellViewModel(
            IWindowManager windowManager, 
            IScreenGunSettings settings, 
            IEventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.settings = settings;
            this.Files = new BindableCollection<ScreenGunFileViewModel>();
            this.SearchForFiles();
            eventAggregator.Subscribe(this);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the files.
        /// </summary>
        /// <value>
        ///     The files.
        /// </value>
        public ObservableCollection<ScreenGunFileViewModel> Files { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether this instance has any recordings.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has any recordings; otherwise, <c>false</c>.
        /// </value>
        public bool HasAnyRecordings
        {
            get
            {
                return this.Files.Any();
            }
        }

        /// <summary>
        ///     Gets the status.
        /// </summary>
        /// <value>
        ///     The status.
        /// </value>
        public string Status
        {
            get
            {
                var count = this.Files.Count;
                return MessageFormatter.Format(
                    @"There {fileCount, plural,
                            zero {are no recordings}
                            one {is just one recording}
                            =3 {are 3 recordings, and as we all know, 3 is a magic number}
                            other {are # recordings}
                            }.", 
                    new
                    {
                        fileCount = count
                    });
            }
        }

        /// <summary>
        /// Set by the view so we can minimize the main view when showing the recorder.
        /// </summary>
        public Action<bool> ToggleWindow { get; internal set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Handle(RecordingCreated message)
        {
            var file = message.File;
            file.RecordingDeleted += this.RemoveFile;
            this.AddFile(file);
        }

        /// <summary>
        ///     Shows the Recorder view.
        /// </summary>
        public void NewRecording()
        {
            this.ToggleWindow(false);
            this.windowManager.ShowWindow(IoC.Get<RecorderViewModel>());
        }

        /// <summary>
        ///     Shows the settings view.
        /// </summary>
        public void Settings()
        {
            this.windowManager.ShowWindow(IoC.Get<SettingsViewModel>());
        }

        /// <summary>
        /// Shows the About view.
        /// </summary>
        public void About()
        {
            this.windowManager.ShowWindow(IoC.Get<AboutViewModel>());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the file.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        private void AddFile(ScreenGunFileViewModel file)
        {
            this.Files.Insert(0, file);
            this.NotifyOfPropertyChange(() => this.Status);
            this.NotifyOfPropertyChange(() => this.HasAnyRecordings);
        }

        /// <summary>
        /// Removes the file.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        private void RemoveFile(ScreenGunFileViewModel model)
        {
            this.Files.Remove(model);
            this.NotifyOfPropertyChange(() => this.Status);
            this.NotifyOfPropertyChange(() => this.HasAnyRecordings);
        }

        /// <summary>
        ///     Searches for files.
        /// </summary>
        private void SearchForFiles()
        {
            var files = Directory.EnumerateFiles(this.settings.StoragePath, "*.mp4").OrderByDescending(x => x);
            foreach (var file in files)
            {
                var item = new ScreenGunFileViewModel(file);
                item.RecordingDeleted += this.RemoveFile;
                this.Files.Add(item);
            }

            this.NotifyOfPropertyChange(() => this.Status);
            this.NotifyOfPropertyChange(() => this.HasAnyRecordings);
        }

        /// <summary>
        /// When the recorder closes, show the main view.
        /// </summary>
        /// <param name="message"></param>
        public void Handle(RecordingViewClosed message)
        {
            this.ToggleWindow(true);
        }

        #endregion
    }
}