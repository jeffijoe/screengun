// ScreenGun
// - RecordingStage.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.
namespace ScreenGun.Recorder
{
    /// <summary>
    ///     An enumeration describing what is being done right now.
    /// </summary>
    public enum RecordingStage
    {
        /// <summary>
        ///     Nothing is being done.
        /// </summary>
        DoingNothing, 

        /// <summary>
        ///     We are recording.
        /// </summary>
        Recording, 

        /// <summary>
        ///     The recording has been stopped, and is currently being encoded.
        /// </summary>
        Encoding, 

        /// <summary>
        ///     Encoding is done, recording complete.
        /// </summary>
        Done
    }
}