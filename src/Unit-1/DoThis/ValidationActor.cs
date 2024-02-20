namespace WinTail;

public class ValidationActor(IActorRef consoleWriterActor) : UntypedActor
{
    protected override void OnReceive(object message)
    {
        var msg = message as string;

        if (string.IsNullOrEmpty(msg))
        {
            consoleWriterActor.Tell(new Messages.NullInputError("No input received"));
        }
        else
        {
            var valid = IsValid(msg);
            if (valid)
            {
                consoleWriterActor.Tell(new Messages.InputSuccess("Valid: Message was valid."));
            }
            else
            {
                consoleWriterActor.Tell(new Messages.ValidationError("Invalid: Message had odd number of characters"));
            }
        }

        Sender.Tell(new Messages.ContinueProcessing());
    }

    /// <summary>
    /// Determines if the message received is valid.
    /// Checks if number of chars in message received is even.
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    private static bool IsValid(string msg)
    {
        var valid = msg.Length % 2 == 0;
        return valid;
    }
}