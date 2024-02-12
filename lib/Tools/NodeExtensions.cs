using Godot.Collections;

namespace Godot.Sharp.Extras;

public static class NodeExtensions
{
    /// <summary>
    /// Adds the Node to a group with a name equal to the Node's type name.
    /// </summary>
    /// <param name="node"></param>
    public static void AddToGroup(this Node node) =>
        node.AddToGroup(node.GetType().Name);

    /// <summary>
    /// Get a node by the name of the Type of node.
    /// </summary>
    /// <typeparam name="T">Node Type to Get</typeparam>
    /// <param name="node"></param>
    /// <returns>Node named the same as Type</returns>
    public static T GetNode<T>(this Node node) where T : Node =>
        node.GetNode<T>(typeof(T).Name);

    /// <summary>
    /// Returns all children as an Godot.Collections.Array<T>.
    /// </summary>
    /// <typeparam name="T">Type of Node</typeparam>
    /// <param name="node"></param>
    /// <returns>Godot.Collections.Array<T></returns>
    public static Array<T> GetChildren<T>(this Node node) where T : Node
    {
        var children = node.GetChildren().Cast<Node>();
        return new Array<T>(children.Select(x => x as T).ToArray<T>());
    }

    /// <summary>
    /// Returns the First Node of the Specified Type.
    /// </summary>
    /// <typeparam name="T">Type of Node</typeparam>
    /// <param name="node"></param>
    /// <returns>First Node of T type.</returns>
    public static T GetFirstNodeOfType<T>(this Node node) where T : Node
    {
        var children = node.GetChildren();
        foreach (var child in children)
        {
            if (child is T t)
                return t;
        }
        return default;
    }

    /// <summary>
    /// Returns All Nodes that match the type of T in a Godot.Collections.Array<T>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node"></param>
    /// <returns>Godot.Collections.Array<T></returns>
    public static Array<T> GetNodesOfType<T>(this Node node)
    {
        var result = new Array<T>();
        var children = node.GetChildren();
        foreach (var child in children)
        {
            if (child is T t)
                result.Add(t);
        }
        return new Array<T>(result.ToArray<T>());
    }

    /// <summary>
    /// Removes All Children from the node, and calls QueueFree on the children.
    /// </summary>
    /// <param name="n"></param>
    public static void RemoveAndQueueFreeChildren(this Node n)
    {
        foreach (var child in n.GetChildren())
        {
            if (child is Node childNode)
            {
                n.RemoveChild(childNode);
                childNode.QueueFree();
            }
        }
    }

    /// <summary>
    /// Calls QueueFree on all children of the Node.
    /// </summary>
    /// <param name="n"></param>
    public static void QueueFreeChildren(this Node n)
    {
        foreach (var child in n.GetChildren())
        {
            if (child is Node childNode)
            {
                childNode.QueueFree();
            }
        }
    }

    /// <summary>
    /// Returns the first node in the Ancestry of the Node Tree that matches the T given.
    /// </summary>
    /// <typeparam name="T">Type of Node</typeparam>
    /// <param name="n"></param>
    /// <returns>First Matching node of Type, or Null</returns>
    public static T GetAncestor<T>(this Node n) where T : Node
    {
        Node currentNode = n;
        while (currentNode != n.GetTree().Root && currentNode is not T)
            currentNode = currentNode.GetParent();

        return currentNode is T ancestor ? ancestor : null;
    }

    /// <summary>
    /// Gets the Last Child indexed child of the node.
    /// </summary>
    /// <param name="n"></param>
    /// <returns>Last Indexed child of the node.</returns>
    public static Node GetLastChild(this Node n)
    {
        var count = n.GetChildCount();
        if (count == 0)
            return null;
        return n.GetChild(count - 1);
    }
}
