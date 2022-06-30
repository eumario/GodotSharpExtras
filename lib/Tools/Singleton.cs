using Godot;

namespace Godot.Sharp.Extras {
	public static class Singleton {
		public static T Get<T>() {
			var node = (Engine.GetMainLoop() as SceneTree).Root.GetNode($"/root/{typeof(T).Name}");
			return (T)System.Convert.ChangeType(node, typeof(T));
		}

		public static T Get<T>(string name) {
			if (!name.Contains("/"))
				name = $"/root/{name}";
			var node = (Engine.GetMainLoop() as SceneTree).Root.GetNode(name);
			return (T)System.Convert.ChangeType(node, typeof(T));
		}
	}
}
