using System;
using Godot;

namespace Godot.Sharp.Extras {

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class ResourceAttribute : Attribute
	{
		public string ResourcePath { get; set; }

		public ResourceAttribute(string path) {
			this.ResourcePath = path;
		}
	}

}