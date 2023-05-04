namespace Godot.Sharp.Extras;

/// <summary>
/// Marks a function as a Event Handler for signals coming from Godot nodes.
/// </summary>
/// <remarks>
/// This is used to make code more readable, and easier to extend, without having to constantly add to the _Ready() function.
/// </remarks>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class SignalHandlerAttribute : Attribute
{
	public string TargetNodeField {
		get;
		set;
	}

	public string SignalName {
		get;
		set;
	}

	/// <summary>
	/// Constructs a <see cref="ResolveNameAttribute"/>
	/// </summary>
	/// <param name="signalName">The name of the signal you wish to connect to.</param>
	/// <param name="targetFieldName">(Optional) The nameof() the Node variable you want to connect to.  When none is given, the instance (this) is used.</param>
	public SignalHandlerAttribute(string signalName, string targetFieldName="") {
		this.SignalName = signalName;
		this.TargetNodeField = targetFieldName;
	}
}