// initialize MyActorSystem
var myActorSystem = ActorSystem.Create("MyActorSystem");

var consoleWriterProps = Props.Create<ConsoleWriterActor>();
var consoleWriterActor = myActorSystem.ActorOf(consoleWriterProps, "consoleWriteActor");

var validationActorProps = Props.Create(() => new ValidationActor(consoleWriterActor));
var validationActor = myActorSystem.ActorOf(validationActorProps, "validationActor");

var consoleReaderProps = Props.Create<ConsoleReaderActor>(validationActor);
var consoleReaderActor = myActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");


// tell console reader to begin
consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

// blocks the main thread from exiting until the actor system is shut down
myActorSystem.WhenTerminated.Wait();
