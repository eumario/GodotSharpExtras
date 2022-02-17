using System;

namespace Godot.Sharp.Extras
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
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

		public SignalHandlerAttribute(string signalName, string targetFieldName) {
			this.SignalName = signalName;
			this.TargetNodeField = targetFieldName;
		}
	}
}