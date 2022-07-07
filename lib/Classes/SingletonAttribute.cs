using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Godot.Sharp.Extras
{
	/// <summary>
	/// Sets up a Singleton/Autoload to be loaded into the Given Variable.  This Singleton needs to be added in
	/// the Godot Editor, in order for it to be properly loaded into the SceneTree and fetched by this
	/// attribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class SingletonAttribute : Attribute
	{
		public string Name { get; set; }

		/// <summary>
		/// Constructs a <see cref="SingletonAttribute"/>
		/// </summary>
		/// <param name="path">Optional Name / Path to the Singleton</param>
		public SingletonAttribute(string path = "") {
			this.Name = path;
		}
	}
}
