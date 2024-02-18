// initialize MyActorSystem

var myActorSystem = ActorSystem.Create("MyActorSystem");

PrintInstructions();

// time to make your first actors!
var consoleWriterActor = myActorSystem.ActorOf(Props.Create(() =>
    new ConsoleWriterActor()));
var consoleReaderActor = myActorSystem.ActorOf(Props.Create(() =>
    new ConsoleReaderActor(consoleWriterActor)));

// tell console reader to begin
consoleReaderActor.Tell("start");
// blocks the main thread from exiting until the actor system is shut down
myActorSystem.WhenTerminated.Wait();


void PrintInstructions()
{
    Console.WriteLine("Write whatever you want into the console!");
    Console.Write("Some lines will appear as");
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.Write(" red ");
    Console.ResetColor();
    Console.Write(" and others will appear as");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write(" green! ");
    Console.ResetColor();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("Type 'exit' to quit this application at any time.\n");
}