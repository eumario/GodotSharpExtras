using System.Diagnostics;

namespace Godot.Sharp.Extras;

public static class GdAssert
{
    /// <summary>
    /// See <see cref="Assert"/>
    /// </summary>
    /// <param name="assertion">True/False Assertion</param>
    /// <param name="message">Message to display when failed.</param>
    /// <param name="file">Filename being executed from.</param>
    /// <param name="line">Line Number executed from.</param>
    /// <exception cref="ApplicationException"></exception>/
    [Conditional("DEBUG")]
    public static void That(bool assertion, string message, string file = "<unknown>", int line = -1)
    {
        if (assertion)
            return;

        GD.PrintErr($"Assertion failed: {message} at {file}:{line}");
        var stackTrace = new StackTrace();
        GD.PrintErr(stackTrace.ToString());
        throw new ApplicationException($"Assertion Failed: {message}");
    }

    /// <summary>
    /// Just like GDScript's assert() function, if the assertion is true, nothing happens,
    /// however, if the assertion is false, it will throw an Exception, with message, causing
    /// the program to stop running.  Will only run when DEBUG Conditional is met.
    /// </summary>
    /// <param name="assertion">True/False Assertion</param>
    /// <param name="message">Message to display on failed assertion</param>
    /// <param name="file">Filename being executed from</param>
    /// <param name="line">Line Number being executed from</param>
    /// <exception cref="ApplicationException"></exception>
    [Conditional("DEBUG")]
    public static void Assert(bool assertion, string message, string file = "<unknown>", int line = -1)
    {
        if (assertion)
            return;
        GD.PrintErr($"Assertion failed: {message} at {file}:{line}");
        var stackTrace = new StackTrace();
        GD.PrintErr(stackTrace.ToString());
        throw new ApplicationException($"Assertion Failed: {message}");
    }
}
