using Godot;

namespace Godot.Sharp.Extras {
	public static class TimerExtensions
	{

		/// <summary>
		/// Creates a SceneTreeTimer, and waits for x amount of milliseconds before continuing execution.  Must await to halt progression.
		/// </summary>
		/// <param name="object"></param>
		/// <param name="milliseconds"></param>
		/// <returns>SignalAwaiter</returns>
		public static SignalAwaiter WaitTimer(this Node @object, int milliseconds) => @object.WaitTimer(milliseconds / 1000.0f);

		/// <summary>
		/// Creates a SceneTreeTimer, and waits for x amount of seconds before continuing execution.  Must await to halt progression.
		/// </summary>
		/// <param name="object"></param>
		/// <param name="seconds"></param>
		/// <returns>SignalAwaiter</returns>
		public static SignalAwaiter WaitTimer(this Node @object, float seconds) => @object.ToSignal(@object.GetTree().CreateTimer(seconds), "timeout");

		/// <summary>
		/// Creates a SignalAwaiter for Timeout event on a Timer.  Can be used to await before continuing execution.
		/// </summary>
		/// <param name="timer"></param>
		/// <returns>SignalAwaiter</returns>
		public static SignalAwaiter Timeout(this Timer @timer) => @timer.ToSignal(@timer, "timeout");

		/// <summary>
		/// Creates a SignalAwaiter for Timeout event on a SceneTreeTimer.  Can be used to await before continuing execution.
		/// </summary>
		/// <param name="timer"></param>
		/// <returns></returns>
		public static SignalAwaiter Timeout(this SceneTreeTimer @timer) => @timer.ToSignal(@timer, "timeout");
	}
}