// initialize MyActorSystem
var myActorSystem = ActorSystem.Create("MyActorSystem");

// time to make your first actors!
var consoleWriterActor = myActorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()));
var consoleReaderActor = myActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(consoleWriterActor)));

// tell console reader to begin
consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

// blocks the main thread from exiting until the actor system is shut down
myActorSystem.WhenTerminated.Wait();
