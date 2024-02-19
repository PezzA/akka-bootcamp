namespace WinTail;

/// <summary>
/// Actor responsible for reading FROM the console. 
/// Also responsible for calling <see cref="ActorSystem.Terminate"/>.
/// </summary>
internal class ConsoleReaderActor(IActorRef consoleWriterActor) : UntypedActor
{
    public const string ExitCommand = "exit";
    public const string StartCommand = "start";

    protected override void OnReceive(object message)
    {
        if (message.Equals(StartCommand))
        {
            DoPrintInstructions();
        }else if (message is Messages.InputError error)
        {
            consoleWriterActor.Tell(error);
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

        if (string.IsNullOrEmpty(message))
        {
            Self.Tell(new Messages.NullInputError("No input received"));
        } 
        else if(string.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
        {
            Context.System.Terminate();
        }
        else
        {
            var valid = IsValid(message);

            if (valid)
            {
                consoleWriterActor.Tell(new Messages.InputSuccess("Thank You! Message was valid"));
                
                Self.Tell(new Messages.ContinueProcessing());
            }
            else
            {
                Self.Tell(new Messages.ValidationError("Invalid: input had odd number of characters"));
            }
        }
    }

    /// <summary>
    /// Validates <see cref="message"/>.
    /// Currently says messages are valid if contain even number of characters.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private static bool IsValid(string message)
    {
        var valid = message.Length % 2 == 0;
        return valid;
    }

}