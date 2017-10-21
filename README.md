# SpringMessagingTibcoEmsListener

## Requirements
-Tibco EMS
-NuGets
--Spring.Messaging.Ems
--And all other dependences 

https://github.com/net-commons/common-logging


http://springframework.net/
https://github.com/spring-projects/spring-net
https://github.com/spring-projects/spring-net/blob/ad64f00abe5ececdd47620cfdee97243f0784bfc/src/Spring/Spring.Messaging.Ems/Messaging/Ems/Listener/SimpleMessageListenerContainer.cs#L196
https://github.com/spring-projects/spring-net/blob/ad64f00abe5ececdd47620cfdee97243f0784bfc/src/Spring/Spring.Messaging.Nms/Messaging/Nms/Listener/SimpleMessageListenerContainer.cs#L234
https://github.com/spring-projects/spring-net/blob/ad64f00abe5ececdd47620cfdee97243f0784bfc/src/Spring/Spring.Messaging.Nms/Messaging/Nms/Listener/AbstractListenerContainer.cs#L427

https://github.com/spring-projects/spring-net/blob/ad64f00abe5ececdd47620cfdee97243f0784bfc/src/Spring/Spring.Messaging.Ems/Messaging/Ems/Listener/SimpleMessageListenerContainer.cs#L214



Start with no Tibco Running


connectionFactory initialized
Destination - SSNODRQ
Listener.cs. created.
EMSExceptionFailed to connect to the server at localhost
EMSException StackTrace
   at TIBCO.EMS.CFImpl._CreateConnection(String userName, String password, Boolean xa)
   at TIBCO.EMS.ConnectionFactory.CreateConnection()
   at Spring.Messaging.Ems.Common.EmsConnectionFactory.CreateConnection() in c:\_prj\spring-net\src\Spring\Spring.Messaging.Ems\Messaging\Ems\Common\EmsConnectionFactory.cs:line 90
   at Spring.Messaging.Ems.Support.EmsAccessor.CreateConnection() in c:\_prj\spring-net\src\Spring\Spring.Messaging.Ems\Messaging\Ems\Support\EmsAccessor.cs:line 148
   at Spring.Messaging.Ems.Listener.AbstractListenerContainer.CreateSharedConnection() in c:\_prj\spring-net\src\Spring\Spring.Messaging.Ems\Messaging\Ems\Listener\AbstractListenerContainer.cs:line 452
   at Spring.Messaging.Ems.Listener.AbstractListenerContainer.EstablishSharedConnection() in c:\_prj\spring-net\src\Spring\Spring.Messaging.Ems\Messaging\Ems\Listener\AbstractListenerContainer.cs:line 414
   at Spring.Messaging.Ems.Listener.AbstractListenerContainer.DoStart() in c:\_prj\spring-net\src\Spring\Spring.Messaging.Ems\Messaging\Ems\Listener\AbstractListenerContainer.cs:line 335
   at Spring.Messaging.Ems.Listener.SimpleMessageListenerContainer.DoStart() in c:\_prj\spring-net\src\Spring\Spring.Messaging.Ems\Messaging\Ems\Listener\SimpleMessageListenerContainer.cs:line 175
   at Spring.Messaging.Ems.Listener.AbstractListenerContainer.Initialize() in c:\_prj\spring-net\src\Spring\Spring.Messaging.Ems\Messaging\Ems\Listener\AbstractListenerContainer.cs:line 274
   at Spring.Messaging.Ems.Listener.AbstractListenerContainer.AfterPropertiesSet() in c:\_prj\spring-net\src\Spring\Spring.Messaging.Ems\Messaging\Ems\Listener\AbstractListenerContainer.cs:line 223
   at SpringMessagingTibcoEmsListener.Program.Main(String[] args) in C:\CSharp\source\SpringMessagingTibcoEmsListener\SpringMessagingTibcoEmsListener\Program.cs:line 158
EMSException Linked Exception error msg
No connection could be made because the target machine actively refused it 127.0.0.1:7222
EMSException Linked StackTrace:
   at TIBCO.EMS.LinkTcp._CreateSocket()
Exception : Cannot start Listener - Failed to connect to the server at localhost
Press <ENTER> to exit.


Kill Tibco Process
connectionFactory initialized
Destination - SSNODRQ
Listener.cs. created.
Listener started.
Press <ENTER> to exit.
OnException - Connection has been terminated
OnException Linked Exception error msg
Unable to read data from the transport connection: An existing connection was forcibly closed by the remote host.
OnException Time - 2017-10-20T18:21:18.7775468-05:00


Console.WriteLine($"OnException Linked StackTrace:\n{exception.LinkedException.StackTrace}");
OnException Linked StackTrace:
   at System.Net.Sockets.NetworkStream.Read(Byte[] buffer, Int32 offset, Int32 size)
   at TIBCO.EMS.LinkTcp._readEx(Byte[] buffer, Int32 offset, Int32 size)
   at TIBCO.EMS.LinkTcp._ReadWireMsg()
   at TIBCO.EMS.LinkTcp.LinkReader.Work()

   How to detect when connection re-EstablishSharedConnection