using Godot;

namespace Godot.Sharp.Extras {

	public static class ExtensionFunctions
	{
		public static SignalAwaiter IdleFrame(this Node @object) {
			return @object.ToSignal(Engine.GetMainLoop(), "idle_frame");
		}

		public static SignalAwaiter WaitTimer(this Node @object, int milliseconds) {
			return @object.WaitTimer(milliseconds / 1000.0f);
		}

		public static SignalAwaiter WaitTimer(this Node @object, float seconds) {
			return @object.ToSignal(@object.GetTree().CreateTimer(seconds), "timeout");
		}
	}

}