// ScreenGun
// - ScreenRecorderException.cs
// --------------------------------------------------------------------
// Authors: 
// - Jeff Hansen <jeff@jeffijoe.com>
// - Bjarke Søgaard <ekrajb123@gmail.com>
// Copyright (C) ScreenGun Authors 2015. All rights reserved.

using System;

namespace ScreenGun.Recorder
{
    /// <summary>
    ///     Screen recorder exception.
    /// </summary>
    public class ScreenRecorderException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenRecorderException"/> class.
        /// </summary>
        /// <param name="message">
        /// The not recording.
        /// </param>
        public ScreenRecorderException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenRecorderException"/> class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in
        ///     Visual Basic) if no inner exception is specified.
        /// </param>
        public ScreenRecorderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}