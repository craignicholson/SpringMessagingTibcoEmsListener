// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionListener.cs" company="Craig">
//   MIT
// </copyright>
// <summary>
//   Defines the ExceptionListener type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpringMessagingTibcoEmsListener
{
    using System;

    using Common.Logging;

    using Spring.Messaging.Ems.Listener;

    using TIBCO.EMS;

    /// <inheritdoc />
    /// <summary>
    /// The exception listener used to asynchonously detect problems with connections, 
    /// like network partition events or when the Tibco EMS Server is shut down.
    /// </summary>
    public class ExceptionListener : IExceptionListener
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AbstractListenerContainer));

        /// <inheritdoc />
        /// <summary>
        /// The on exception.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void OnException(EMSException exception)
        {
            Logger.Error($"OnException : {exception.Message}");
            if (exception.LinkedException != null)
            {
                Logger.Error($"OnException Linked Exception error msg  : {exception.LinkedException.Message}");
            }

            if (exception.InnerException != null)
            {
                Logger.Error($"OnException InnerException : {exception.InnerException.Message}");
            }

            Logger.Error($"OnException Time : {DateTime.Now:O}");
        }
    }
}