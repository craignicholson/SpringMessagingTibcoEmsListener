// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageListener.cs" company="Craig">
//   MIT
// </copyright>
// <summary>
//   Defines the Listener type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpringMessagingTibcoEmsListener
{
    using System;
    using Spring.Messaging.Ems.Common;
    using Spring.Messaging.Ems.Listener;

    using TIBCO.EMS;

    /// <inheritdoc />
    /// <summary>
    /// The Listener implementation for Spring.Messaging.Ems.Listener
    /// </summary>
    public class MessageListener : ISessionAwareMessageListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageListener"/> class.
        /// </summary>
        public MessageListener()
        {
            Console.WriteLine("Listener.cs. created.");
        }

        /// <inheritdoc />
        /// <summary>
        /// On Message Handler, runs when the consumer receives a message from the Tibco EMS Queue
        /// </summary>
        /// <param name="message">Tibco EMS Message</param>
        /// <param name="session">Spring.Messaging.Ems.Common.ISession when event occurs.</param>
        public void OnMessage(Message message, ISession session)
        {
            var ssnJobIdToken = message.GetStringProperty("SSN_JOB_ID_TOKEN"); 
            var ssnJobStatusToken = message.GetStringProperty("SSN_JOB_STATUS_TOKEN");  // Two states {Failure and Completed}
            Console.WriteLine($"MessageId :{message.MessageID}");
            Console.WriteLine($"CorrelationID :{message.CorrelationID}");
            Console.WriteLine($"DeliveryTime :{message.DeliveryTime}");
            try
            {
                switch (message)
                {
                    case TextMessage textMessage:
                        Console.WriteLine($"TextMessage.Text\n{textMessage.Text}");
                        break;
                    case BytesMessage byteMessage:
                        Console.WriteLine($"BytesMessage.BodyLength : {byteMessage.BodyLength}");
                        break;
                    case MapMessage mapMessage:
                        Console.WriteLine($"MapMessage.FieldCount : {mapMessage.FieldCount}");
                        break;
                    case StreamMessage streamMessage:
                        Console.WriteLine($"StreamMessage.FieldCount : {streamMessage.FieldCount}");
                        break;
                    case ObjectMessage objectMessage:
                        Console.WriteLine($"ObjectMessage.MessageID : {objectMessage.MessageID}");
                        break;
                }

                Console.WriteLine($"OnMessage received message : {DateTime.Now:O}");
                message.Acknowledge();
            }
            catch (Exception e)
            {
                var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var expiryTime = (DateTime.UtcNow.AddSeconds(86400) - unixEpoch).TotalMilliseconds;
                message.Expiration = (long)expiryTime;
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
