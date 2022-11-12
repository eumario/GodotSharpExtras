using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Godot.Sharp.Extras {
	public static class SceneTreeExtensions
	{
		/// <summary>
		/// Creates a Waiter for the Engine to complete an Process Frame.  Must use await to hold progression.
		/// In Godot 4.0, IdleFrame became ProcessFrame.
		/// </summary>
		/// <param name="object"></param>
		/// <returns>SignalAwaiter</returns>
		public static SignalAwaiter IdleFrame(this Node @object) => ProcessFrame(@object);
		
		/// <summary>
		/// Creates a Waiter for the Engine to complete an Idle Frame.  Must use await to hold progression.
		/// </summary>
		/// <param name="object"></param>
		/// <returns>SignalAwaiter</returns>
		public static SignalAwaiter ProcessFrame(this Node @object) => @object.ToSignal(Engine.GetMainLoop(), "process_frame");
		
		/// <summary>
		/// Creates a Waiter for the Engine to complete a Frame, before progressing to the next one.  Must use await to hold progression.
		/// *Note* This is the same as IdleFrame, just a convenience alias name.
		/// </summary>
		/// <param name="node"></param>
		/// <returns>SignalAwaiter</returns>
		public static SignalAwaiter NextFrame(this Node @node) => ProcessFrame(@node);
	}
}