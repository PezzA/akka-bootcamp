// initialize MyActorSystem
var myActorSystem = ActorSystem.Create("MyActorSystem");


var consoleWriterProps = Props.Create<ConsoleWriterActor>();
var consoleWriterActor = myActorSystem.ActorOf(consoleWriterProps, "consoleWriteActor");

var tailCoordinatorProps = Props.Create(() => new TailCoordinatorActor());
var tailCoordinatorActor = myActorSystem.ActorOf(tailCoordinatorProps, "tailCoordinatorActor");

var fileValidatorActorProps = Props.Create(() => new FileValidatorActor(consoleWriterActor, tailCoordinatorActor));
var fileValidatorActor  = myActorSystem.ActorOf(fileValidatorActorProps, "validationActor");

var consoleReaderProps = Props.Create<ConsoleReaderActor>(fileValidatorActor );
var consoleReaderActor = myActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");

consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

// blocks the main thread from exiting until the actor system is shut down
myActorSystem.WhenTerminated.Wait();
