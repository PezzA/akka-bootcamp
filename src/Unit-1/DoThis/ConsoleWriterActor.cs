namespace WinTail;

/// <summary>
/// Actor responsible for serializing message writes to the console.
/// (write one message at a time, champ :)
/// </summary>
internal class ConsoleWriterActor : UntypedActor
{
    protected override void OnReceive(object message)
    {
        switch (message)
        {
            case Messages.InputError msgError:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(msgError.Reason);
                break;
            case Messages.InputSuccess msgSuccess:
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(msgSuccess.Reason);
                break;
            default:
                Console.WriteLine(message);
                break;
        }

        Console.ResetColor();
    }
}