namespace WinTail;

public class Messages
{
    #region Neutral/system messages

    /// <summary>
    /// Marker class to continue processing.
    /// </summary>
    public class ContinueProcessing;

    #endregion

    #region Success messages

    /// <summary>
    /// Base class for signalling that user input is valid
    /// </summary>
    /// <param name="reason"></param>
    public class InputSuccess(string reason)
    {
        public string Reason { get; set; } = reason;
    }

    #endregion

    #region Error messages

    /// <summary>
    /// Base class for signalling that user input was invalid.
    /// </summary>
    /// <param name="reason"></param>
    public class InputError(string reason)
    {
        public string Reason { get; set; } = reason;
    }

    /// <summary>
    /// User provided blank input
    /// </summary>
    /// <param name="reason"></param>
    public class NullInputError(string reason) : InputError(reason);

    /// <summary>
    /// User provided invalid input (current, input w/odd # chars)
    /// </summary>
    /// <param name="reason"></param>
    public class ValidationError(string reason) : InputError(reason);

    #endregion
}