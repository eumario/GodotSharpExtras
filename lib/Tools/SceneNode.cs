using System;
using System.Collections.Generic;
using System.IO;

namespace Godot.Sharp.Extras;

public static class SceneNode<T> where T : Node
{
    public static T FromScene()
    {
        if (_scenes.ContainsKey(typeof(T))) return _scenes[typeof(T)].Instantiate<T>();
        var path = string.Empty;
        var attributes = Attribute.GetCustomAttributes(typeof(T));
        foreach (var attr in attributes)
        {
            if (attr is not SceneNodeAttribute sceneNodeAttribute) continue;
            path = sceneNodeAttribute.Path;
            break;
        }

        if (string.IsNullOrEmpty(path))
        {
            throw new FileLoadException("Scene node could not be loaded. (No SceneNode attribute found)");
        }

        var scene = GD.Load<PackedScene>(path);

        if (scene == null)
        {
            throw new FileLoadException("Scene node could not be loaded. (" + path + ")");
        }

        _scenes[typeof(T)] = scene;

        return scene.Instantiate<T>();
    }

    private static Dictionary<Type, PackedScene> _scenes = new();
}