using System;
using System.Diagnostics;
using Godot;

namespace Godot.Sharp.Extras {
	public static class GdAssert
	{
		[Conditional("DEBUG")]
		public static void That(bool assertion, string message, string file = "<unknown>", int line = -1) {
			if (assertion)
				return;
			
			GD.PrintErr($"Assertion failed: {message} at {file}:{line}");
			var stackTrace = new StackTrace();
			GD.PrintErr(stackTrace.ToString());
			throw new ApplicationException($"Assertion Failed: {message}");
		}
		
	}
}
