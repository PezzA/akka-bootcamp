namespace WinTail;

/// <summary>
/// Actor responsible for reading FROM the console. 
/// Also responsible for calling <see cref="ActorSystem.Terminate"/>.
/// </summary>
internal class ConsoleReaderActor(IActorRef consoleValidationActor) : UntypedActor
{
    public const string ExitCommand = "exit";
    public const string StartCommand = "start";

    protected override void OnReceive(object message)
    {
        if (message.Equals(StartCommand))
        {
            DoPrintInstructions();
        }

        GetAndValidateInput();
    }

    private static void DoPrintInstructions()
    {
        Console.WriteLine("Write whatever you want into the console!");
        Console.WriteLine("Some entries will pass validation, and some won't...\n\n");
        Console.WriteLine("Type 'exit' to quit this application at any time.\n");
    }

    private void GetAndValidateInput()
    {
        var message = Console.ReadLine();

        if (string.IsNullOrEmpty(message) && string.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
        {
            Context.System.Terminate();
            return;
        }

        consoleValidationActor.Tell(message);
    }
}