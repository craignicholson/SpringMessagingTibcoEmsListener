// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Craig">
//   MIT
// </copyright>
// <summary>
//   Defines the Program type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpringMessagingTibcoEmsListener
{
    using Common.Logging;

    using Spring.Messaging.Ems.Listener;

    /// <summary>
    /// The program.
    /// </summary>
    public static class Program
    {

        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AbstractListenerContainer));

        /// <summary>
        /// The consumer.
        /// </summary>
        private static readonly ConsumerListener Consumer = new ConsumerListener();

        /// <summary>
        /// The main entry point for this demo application.
        /// </summary>
        /// <param name="args">
        /// This sample application does not expect any incoming arguments.
        /// </param>
        public static void Main(string[] args)
        {
            Consumer.Run();
        }
    }
}
