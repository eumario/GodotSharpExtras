using System;

namespace Godot.Sharp.Extras;

/// <summary>
/// Sets up a Class to be a Scene Node, that loads a scene, and instantiates it, instead of using new function.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class SceneNodeAttribute : Attribute
{
    public string Path { get; set; }

    /// <summary>
    /// Constructs a <see cref="SceneNodeAttribute"/>
    /// </summary>
    /// <param name="path">The path to the Scene file to load and instantiate.</param>
    public SceneNodeAttribute(string path)
    {
        this.Path = path;
    }
}