using System.Text;

namespace WinTail;

/// <summary>
/// Monitors the file at <see>
///     <cref>filePath</cref>
/// </see>
/// for changes and sends
/// file updates to console.
/// </summary>
public class TailActor(IActorRef reporterActor, string filePath) : UntypedActor
{
    #region Message types

    /// <summary>
    /// Signal that the file has changed, and we need to 
    /// read the next line of the file.
    /// </summary>
    public class FileWrite
    {
        public FileWrite(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; private set; }
    }

    /// <summary>
    /// Signal that the OS had an error accessing the file.
    /// </summary>
    public class FileError
    {
        public FileError(string fileName, string reason)
        {
            FileName = fileName;
            Reason = reason;
        }

        public string FileName { get; private set; }

        public string Reason { get; private set; }
    }

    /// <summary>
    /// Signal to read the initial contents of the file at actor startup.
    /// </summary>
    public class InitialRead
    {
        public InitialRead(string fileName, string text)
        {
            FileName = fileName;
            Text = text;
        }

        public string FileName { get; private set; }
        public string Text { get; private set; }
    }

    #endregion

    private FileObserver _observer;
    private Stream _fileStream;
    private StreamReader _fileStreamReader;

    protected override void PreStart()
    {
        // start watching file for changes
        _observer = new FileObserver(Self, Path.GetFullPath(filePath));
        _observer.Start();

        // open the file stream with shared read/write permissions
        // (so file can be written to while open)
        _fileStream = new FileStream(Path.GetFullPath(filePath),
            FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        _fileStreamReader = new StreamReader(_fileStream, Encoding.UTF8);

        // read the initial contents of the file and send it to console as first msg
        var text = _fileStreamReader.ReadToEnd();
        Self.Tell(new InitialRead(filePath, text));
    }

    protected override void PostStop()
    {
        _observer.Dispose();
        _observer = null;
        _fileStreamReader.Close();
        _fileStreamReader.Dispose();
        base.PostStop();
    }

    protected override void OnReceive(object message)
    {
        switch (message)
        {
            case FileWrite:
            {
                // move file cursor forward
                // pull results from cursor to end of file and write to output
                // (this is assuming a log file type format that is append-only)
                var text = _fileStreamReader.ReadToEnd();
                if (!string.IsNullOrEmpty(text))
                {
                    reporterActor.Tell(text);
                }

                break;
            }
            case FileError fe:
                reporterActor.Tell($"Tail error: {fe.Reason}");
                break;
            case InitialRead ir:
                reporterActor.Tell(ir.Text);
                break;
        }
    }
}