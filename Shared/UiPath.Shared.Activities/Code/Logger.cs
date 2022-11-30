using System.IO;

namespace UiPath.Shared.Activities.Code;

/// <summary>
/// Writes log entries to log file.
/// </summary>
public class Logger
{
    private string _file;

    /// <summary>
    /// Constructs a logger object.
    /// </summary>
    /// <param name="fileName">The file name to log to.</param>
    public Logger(string fileName)
    {
        _file = fileName;

        if (!string.IsNullOrEmpty(_file) && !File.Exists(_file))
        {
            File.WriteAllText(_file, string.Empty);
        }
    }

    /// <summary>
    /// Writes the specified message to the log file.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void Write(string message)
    {
        File.AppendAllText(_file, message);   
    }
    
    /// <summary>
    /// Writes the specified message to the log file as a line.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void WriteLine(string message)
    {
        File.AppendAllText(_file, message + "\r\n");   
    }

    /// <summary>
    /// Clears the contents of a log file.
    /// </summary>
    public void Clear()
    {
        File.WriteAllText(_file, string.Empty);
    }
}