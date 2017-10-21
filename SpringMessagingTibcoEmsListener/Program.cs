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
    using System;
    using System.Configuration;

    using Spring.Messaging.Ems.Listener;

    using TIBCO.EMS;

    /// <summary>
    /// The program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The AckMode sets whether how we treat incoming messages.  Messages need to be acknowledge so they message will
        /// be remove from the queue once the message is delivered.
        /// </summary>
        private const int AckMode = Session.AUTO_ACKNOWLEDGE;

        /// <summary>
        /// The target host name is the name of the server or endpoint without the url and port number.
        /// Examples {localhost, etss-appdev, tibco.test01.amu.ssnsgs.net}
        /// </summary>
        // ReSharper disable once StyleCop.SA1650
        private static readonly string TargetHostName = ConfigurationManager.AppSettings["TargetHostName"];

        /// <summary>
        /// The Uri, the endpoint for the Tibco EMS server
        /// Examples {localhost,tcp://10.86.1.31:7222,tcp://etss-appdev:7222, ssl://tibco.test01.amu.ssnsgs.net:7243}
        /// </summary>
        // ReSharper disable once StyleCop.SA1650
        private static readonly string Uri = ConfigurationManager.AppSettings["Uri"];

        /// <summary>
        /// Use SSL is a flag to indicate we will use SSL to encrypt the network traffic.
        /// </summary>
        private static readonly bool UseSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["Ssl"]);

        /// <summary>
        /// The destination or queue name we are listening for messages.
        /// </summary>
        private static readonly string DestinationName = ConfigurationManager.AppSettings["DestinationName"];

        /// <summary>
        /// The User if required for Tibco EMS.
        /// </summary>
        private static readonly string User = ConfigurationManager.AppSettings["User"];

        /// <summary>
        /// The Password if required for Tibco EMS.
        /// </summary>
        private static readonly string Pwd = ConfigurationManager.AppSettings["Pwd"];

        /// <summary>
        /// The SSL client certificate file path to the file.
        /// </summary>
        private static readonly string SslClientCert = ConfigurationManager.AppSettings["SslClientCert"];

        /// <summary>
        /// The SSL client certificate password.
        /// </summary>
        private static readonly string SslClientCertPassword = ConfigurationManager.AppSettings["SslClientCertPassword"];

        /// <summary>
        /// The max recovery time in days.
        /// </summary>
        private static readonly int MaxRecoveryTimeInDays = Convert.ToInt32(ConfigurationManager.AppSettings["MaxRecoveryTimeInDays"]);

        /// <summary>
        /// The connection factory used to setup the connection to the Tibco EMS server.
        /// </summary>
        private static Spring.Messaging.Ems.Common.EmsConnectionFactory connectionFactory;

        /// <summary>
        /// The main entry point for this demo application.
        /// </summary>
        /// <param name="args">
        /// This sample application does not expect any incoming arguments.
        /// </param>
        public static void Main(string[] args)
        {
            try
            {
                if (string.IsNullOrEmpty(Uri))
                {
                    throw new ArgumentException("Uri can not be empty");
                }

                connectionFactory = new Spring.Messaging.Ems.Common.EmsConnectionFactory(Uri);
                if (!string.IsNullOrEmpty(User))
                {
                    connectionFactory.UserName = User;
                }

                if (!string.IsNullOrEmpty(Pwd))
                {
                    connectionFactory.UserPassword = Pwd;
                }

                if (!string.IsNullOrEmpty(TargetHostName))
                {
                    connectionFactory.TargetHostName = TargetHostName;
                }

                if (UseSsl)
                {
                    if (!System.IO.File.Exists(SslClientCert))
                    {
                        throw new Exception("SslClientCert File is missing");
                    }

                    // LOAD CLIENT CERTIFICATE FROM A FILE.
                    var clientCert = new System.Security.Cryptography.X509Certificates.X509Certificate2(SslClientCert, SslClientCertPassword);
                    Console.WriteLine($"Certificate File:{SslClientCert}");
                    Console.WriteLine($"Certificate Subject:{clientCert.Subject}");
                    Console.WriteLine($"Certificate Public Key:{clientCert.PublicKey.Key}");
                    Console.WriteLine($"Certificate Has private key?:{(clientCert.HasPrivateKey ? "Yes" : "No")}");

                    // LOAD CERTIFICATE GLOBALLY. THIS IS AVAILABLE FOR ALL CONNECTIONS
                    var sslStore = new EMSSSLFileStoreInfo();
                    sslStore.SetSSLClientIdentity(clientCert);
                    connectionFactory.NativeConnectionFactory.SetCertificateStoreType(EMSSSLStoreType.EMSSSL_STORE_TYPE_FILE, sslStore);
                }

                // Check if we have established a connection since the TargetHostName, Uri, User, or Pwd could be incorrect
                if (connectionFactory == null)
                {
                    throw new Exception("connectionFactory has not been initialized");
                }

                Console.WriteLine("connectionFactory initialized");               
                try
                {
                    Console.WriteLine($"Destination - {DestinationName}");
                    using (var listenerContainer = new SimpleMessageListenerContainer())
                    {
                        listenerContainer.ConnectionFactory = connectionFactory;
                        listenerContainer.DestinationName = DestinationName;
                        listenerContainer.ConcurrentConsumers = 1;
                        listenerContainer.PubSubDomain = false;
                        listenerContainer.MessageListener = new Listener();
                        listenerContainer.ExceptionListener = new ExceptionListener();
                        listenerContainer.MaxRecoveryTime = new TimeSpan(MaxRecoveryTimeInDays, 0, 0, 0);
                        listenerContainer.RecoveryInterval = new TimeSpan(0, 0, 0, 10, 0); // set to 10 Minutes  
                        listenerContainer.AcceptMessagesWhileStopping = false;
                        listenerContainer.SessionAcknowledgeMode = AckMode;
                        listenerContainer.AfterPropertiesSet();
                        Console.WriteLine("Listener started.");
                        Console.WriteLine("Press <ENTER> to exit.");
                        Console.ReadLine();
                    }
                }
                catch (EMSException e)
                {
                    Console.WriteLine($"EMSException : {e.Message}");
                    Console.WriteLine($"EMSException StackTrace : {e.StackTrace}");

                    if (e.LinkedException != null)
                    {
                        Console.WriteLine($"EMSException Linked Exception error msg : {e.LinkedException.Message}");
                        Console.WriteLine($"EMSException Linked StackTrace : {e.LinkedException.StackTrace}");
                    }

                    throw new Exception("Cannot start Listener - " + e.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"InnerException : {ex.InnerException.Message}");
                }
            }

            // Block Auto Closing of CLI
            Console.WriteLine("Press <ENTER> to exit.");
            Console.ReadLine();
        }
    }
}
