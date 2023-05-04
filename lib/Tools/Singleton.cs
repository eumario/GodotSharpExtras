namespace Godot.Sharp.Extras;

public static class Singleton {
	/// <summary>
	/// Get's a Singleton/Autoload from the Root of the SceneTree.  Needs to be added to the Godot Editor.
	/// </summary>
	/// <typeparam name="T">Class for Singleton/Autoload</typeparam>
	/// <returns>Instance of the Singleton/Autoload</returns>
	public static T Get<T>() {
		var node = (Engine.GetMainLoop() as SceneTree).Root.GetNode($"/root/{typeof(T).Name}");
		return (T)System.Convert.ChangeType(node, typeof(T));
	}

	/// <summary>
	/// Get's a Singleton/Autoload from the Root of the SceneTree.  Needs to be added to the Godot Editor.
	/// </summary>
	/// <typeparam name="T">Class for Singleton/Autoload</typeparam>
	/// <param name="name">Name used for Singleton/Autoload</param>
	/// <returns>Instance of the Singleton/Autoload</returns>
	public static T Get<T>(string name) {
		if (!name.Contains("/"))
			name = $"/root/{name}";
		var node = (Engine.GetMainLoop() as SceneTree).Root.GetNode(name);
		return (T)System.Convert.ChangeType(node, typeof(T));
	}
}
