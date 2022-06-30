using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Godot.Sharp.Extras
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class SingletonAttribute : Attribute
	{
		public string Name { get; set; }

		public SingletonAttribute(string path = "") {
			this.Name = path;
		}
	}
}
