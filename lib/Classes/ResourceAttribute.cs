namespace Godot.Sharp.Extras;


/// <summary>
/// Sets up a Resource to be loaded into variable given.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class ResourceAttribute : Attribute
{
	public string ResourcePath { get; set; }

	/// <summary>
	/// Constructs a <see cref="ResourceAttribute"/>
	/// </summary>
	/// <param name="path">The path to the Resource to load into the variable.</param>
	public ResourceAttribute(string path) {
		this.ResourcePath = path;
	}
}