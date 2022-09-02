using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Godot.Sharp.Extras {
	public static class TweenExtensions
	{
		/// <summary>
		/// Awaits for the Signal tween_completed before continuing execution.  Must use await to hold progression.
		/// </summary>
		/// <param name="tween">Tween</param>
		/// <returns>SignalAwaiter</returns>
		public static SignalAwaiter IsCompleted(this Tween @tween) => @tween.ToSignal(@tween, "tween_completed");

		/// <summary>
		/// Awaits for the Signal finished before continuing execution.  Must use await to hold progression.
		/// </summary>
		/// <param name="tween">SceneTreeTween</param>
		/// <returns>SignalAwaiter</returns>
		public static SignalAwaiter IsFinished(this SceneTreeTween @tween) =>
			@tween.ToSignal(@tween, "finished");

		/// <summary>
		/// Awaits for the Signal tween_all_completed before continuing execution.  Must use await to hold progression.
		/// </summary>
		/// <param name="tween"></param>
		/// <returns>SignalAwaiter</returns>
		public static SignalAwaiter AreAllCompleted(this Tween @tween) => @tween.ToSignal(@tween, "tween_all_completed");

		/// <summary>
		/// Awaits for all tween_completed signals that are returned, waiting for the specified object to be completed.
		/// </summary>
		/// <param name="tween"></param>
		/// <param name="interpolatedObject"></param>
		/// <returns>Task</returns>
		public static async Task IsCompleted(this Tween @tween, Object interpolatedObject) {
			object[] result;
			do {
				result = await tween.IsCompleted();
			} while (!(result.Length == 2 && result[0] == interpolatedObject));
		}
	}
}