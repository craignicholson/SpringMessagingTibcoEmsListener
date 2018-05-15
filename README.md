# SpringMessagingTibcoEmsListener

This application listens to a Tibco EMS queue and writes the output to the console. The goal of this
example is to test Spring.Messaging.Ems framework for working with Tibco EMS.

This application builds on the [tibcoems-tutorials](https://github.com/craignicholson/tibcoems-tutorials) and you
can use [SendTextMessage](https://github.com/craignicholson/tibcoems-tutorials/tree/master/3_SendTextMessage)
to post a message for testing.

The main purpose for using Spring.Messaging.Ems is for the feature **RefreshConnectionUntilSuccessful** which works
better than tyring to code up native C# using the Tibco.EMS.Connection.ExceptionHandler.

## Quick Start for Testing

How to test recovery from a loss of the Tibco EMS Server

-Start EMS Server
-Use SendTextMessage to send first message to the queue
-Start SpringMessagingTibcoEmsListener
-Observe the message is delivered to SpringMessagingTibcoEmsListener
-Stop EMS Server
-Observe the Logging for a period of time
-Re-start EMS Server
-Use SendTextMessage to send second message to the queue
-Observe the message is delivered to SpringMessagingTibcoEmsListener

## Requirements

-Tibco EMS (TIB_ems-dev_8.3.0_win_x86_64_vc10)
-NuGets
--Spring.Messaging.Ems
--And all other dependences required with Spring.Messaging.Ems

## Logging

Adding this block to the App.config allows the logger Spring.net implements to be written
to the console. You can remove this block for production applications.

```xml

  <configSections>
    <sectionGroup name="common">
      <section name="logging" type="Common.Logging.ConfigurationSectionHandler, Common.Logging" />
    </sectionGroup>
  </configSections>

  <common>
    <logging>
      <factoryAdapter type="Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter, Common.Logging">
        <arg key="level" value="INFO" />
        <arg key="showLogName" value="true" />
        <arg key="showDateTime" value="true" />
        <arg key="dateTimeFormat" value="yyyy/MM/dd HH:mm:ss:fff" />
      </factoryAdapter>
    </logging>
  </common>


```

## Workflow for following how the RefreshConnectionUntilSuccessful works

- http://springframework.net/
- https://github.com/spring-projects/spring-net
- https://github.com/spring-projects/spring-net/blob/ad64f00abe5ececdd47620cfdee97243f0784bfc/src/Spring/Spring.Messaging.Ems/Messaging/Ems/Listener/SimpleMessageListenerContainer.cs#L196
- https://github.com/spring-projects/spring-net/blob/ad64f00abe5ececdd47620cfdee97243f0784bfc/src/Spring/Spring.Messaging.Nms/Messaging/Nms/Listener/SimpleMessageListenerContainer.cs#L234
- https://github.com/spring-projects/spring-net/blob/ad64f00abe5ececdd47620cfdee97243f0784bfc/src/Spring/Spring.Messaging.Nms/Messaging/Nms/Listener/AbstractListenerContainer.cs#L427
- https://github.com/spring-projects/spring-net/blob/ad64f00abe5ececdd47620cfdee97243f0784bfc/src/Spring/Spring.Messaging.Ems/Messaging/Ems/Listener/SimpleMessageListenerContainer.cs#L214

```bash

2017/10/22 10:19:05:110 [ERROR] Spring.Messaging.Ems.Common.EmsConnection - No exception handler registered with EmsConnection wrapper class.
=======================================================(inner most exception)===
 (1) TIBCO.EMS.EMSException
================================================================================
Method        :  <unavailable>
Type          :  <unavailable>
Assembly      :  <unavailable>
Assembly Path :  <unavailable>
Source        :
Thread        :  4 'EMS TCPLink Reader (Server-2)'
Helplink      :

Message:
"Connection has been terminated"

Properties:
  EMSException.ErrorCode = ""
  EMSException.LinkedException = "System.IO.IOException: Unable to read data from the transport connection: An existing connection was forcibly closed by the remote host. ---> System.Net.Sockets.SocketException: An existing connection was forcibly closed by the remote host
   at System.Net.Sockets.Socket.Receive(Byte[] buffer, Int32 offset, Int32 size, SocketFlags socketFlags)
   at System.Net.Sockets.NetworkStream.Read(Byte[] buffer, Int32 offset, Int32 size)
   --- End of inner exception stack trace ---
   at System.Net.Sockets.NetworkStream.Read(Byte[] buffer, Int32 offset, Int32 size)
   at TIBCO.EMS.LinkTcp._readEx(Byte[] buffer, Int32 offset, Int32 size)
   at TIBCO.EMS.LinkTcp._ReadWireMsg()
   at TIBCO.EMS.LinkTcp.LinkReader.Work()"

Stack Trace:
```


Start with no Tibco Running

```bash
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
```

1. Kill Tibco Process
1. connectionFactory initialized
1. Destination - SSNODRQ
1. Listener.cs. created.
1. Listener started.
1. Press <ENTER> to exit.

```bash
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

```

How to detect when connection re-EstablishSharedConnection
   
