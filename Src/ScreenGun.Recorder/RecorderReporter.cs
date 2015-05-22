// ScreenGun
// - RecorderReporter.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.
namespace ScreenGun.Recorder
{
    /// <summary>
    ///     State telling the application what the recorder is doing.
    /// </summary>
    public class RecorderState
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RecorderState"/> class.
        /// </summary>
        /// <param name="stage">
        /// The stage.
        /// </param>
        public RecorderState(RecordingStage stage)
        {
            this.Stage = stage;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the stage.
        /// </summary>
        /// <value>
        ///     The stage.
        /// </value>
        public RecordingStage Stage { get; private set; }

        #endregion
    }
}