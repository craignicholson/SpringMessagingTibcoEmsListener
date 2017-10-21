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

    using TIBCO.EMS;

    /// <inheritdoc />
    /// <summary>
    /// The exception listener used to asynchonously detect problems with connections, 
    /// like network partition events or when the Tibco EMS Server is shut down.
    /// </summary>
    public class ExceptionListener : IExceptionListener
    {
        /// <inheritdoc />
        /// <summary>
        /// The on exception.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public void OnException(EMSException exception)
        {
            Console.WriteLine($"OnException : {exception.Message}");
            if (exception.LinkedException != null)
            {
                Console.WriteLine($"OnException Linked Exception error msg  : {exception.LinkedException.Message}");
            }

            if (exception.InnerException != null)
            {
                Console.WriteLine($"OnException InnerException : {exception.InnerException.Message}");
            }

            Console.WriteLine($"OnException Time : {DateTime.Now:O}");
        }
    }
}