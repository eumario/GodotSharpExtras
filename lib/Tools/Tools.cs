using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

namespace Godot.Sharp.Extras
{
	public static class Tools
	{
		/// <summary>
		/// Processes all Attributes for NodePaths.
		/// </summary>
		/// <remarks>
		/// This will fill in fields and register signals as per attributes such as <see cref="NodePathAttribute"/> and <see cref="SignalAttribute"/>.
		/// </remarks>
		/// <param name="node">The node.</param>
		public static void OnReady<T>(this T node)
			where T : Node
		{
			var type = node.GetType();

			if (typeMembers.TryGetValue(type, out var members) == false)
			{
				var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
				members = type.GetFields(bindingFlags).Select(fi => new MemberInfo(fi))
							.Concat(type.GetProperties(bindingFlags).Select(pi => new MemberInfo(pi)))
							.ToArray();
				typeMembers[type] = members;
			}

			foreach (var member in members)
			{
				foreach (var attr in member.CustomAttributes)
				{
					switch(attr)
					{
						case ResolveNodeAttribute resolveAttr:
							ResolveNodeFromPath(node, member, resolveAttr.TargetFieldName);
							break;
						case NodePathAttribute pathAttr:
							AssignPathToMember(node, member, pathAttr.NodePath);
							break;
						case ResourceAttribute resAttr:
							LoadResource(node, member, resAttr.ResourcePath);
							break;
					}
				}
			}

			if (signalHandlers.TryGetValue(type, out var handlers) == false)
			{
				handlers = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
							.SelectMany(mi => mi.GetCustomAttributes()
								.OfType<SignalHandlerAttribute>()
								.Select(attr => new SignalHandlerInfo(mi.Name, attr))
							)
							.ToArray();
				signalHandlers[type] = handlers;
			}

			foreach (var handler in handlers)
			{
				ConnectSignalHandler(node, handler.MethodName, handler.Attribute);
			}
		}

		private static void ConnectSignalHandler(Node node, string methodName, SignalHandlerAttribute attr) {
			var signal = attr.SignalName;

			if (!string.IsNullOrEmpty(attr.TargetNodeField))
			{
				MemberInfo[] members = typeMembers[node.GetType()];
				MemberInfo? member = members.FirstOrDefault(mi => mi.Name == attr.TargetNodeField);

				node = member?.GetValue(node) as Node
					?? throw new Exception($"SignalHandlerAttribute on '{node.GetType().FullName}.{methodName}', '{attr.TargetNodeField}' is a nonexistant field or property.");
			}

			if (!node.IsConnected(signal, node, methodName))
			{
				node.Connect(signal, node, methodName);
			}
		}

		private static void ResolveNodeFromPath(Node node, MemberInfo member, string targetFieldName) {
			var type = node.GetType();
			MemberInfo targetMember = type.GetField(targetFieldName) is FieldInfo fi
						? new MemberInfo(fi)
						: type.GetProperty(targetFieldName) is PropertyInfo pi
						? new MemberInfo(pi)
						: throw new Exception($"ResolveNodeAttribute on {type.FullName}.{member.Name} targets nonexistant field or property {targetFieldName}");
			
			NodePath path = targetMember.GetValue(node) as NodePath
					?? throw new Exception($"ResolveNodeAttribute on {type.FullName}.{member.Name} targets property {targetFieldName} which is not a NodePath");
			
			AssignPathToMember(node, member, path);
		}

		private static void LoadResource(Node node, MemberInfo member, string resourcePath) {
			Resource res;
			try {
				res = GD.Load(resourcePath);
			} catch (Exception ex) {
				throw new Exception($"Failed to load Resource '{resourcePath}', Message: '{ex.Message}'.");
			}

			try {
				member.SetValue(node, Convert.ChangeType(res, member.MemberType));
			} catch (Exception ex) {
				throw new Exception($"Failed to set variable {member.Name} with the {member.MemberType} for {resourcePath}.");
			}
		}

		private static void AssignPathToMember(Node node, MemberInfo member, NodePath path)
		{
			if (node.GetNode(path) is Node value)
			{
				try
				{
					member.SetValue(node,value);
				}
				catch (ArgumentException e)
				{
					throw new Exception($"AssignPathToMember on {node.GetType().FullName}.{member.Name} - cannot set value of type {value?.GetType().Name} on field type {member.MemberType.Name}", e);
				}
			}
			else
			{
				GD.Print($"Warning: AssignPathToMember on {node.GetType().FullName}.{member.Name} - node at \"{path}\" is null");
			}
		}

		readonly struct MemberInfo
		{
			public string Name { get; }
			public Type MemberType { get; }
			public IEnumerable<Attribute> CustomAttributes { get; }
			public Action<object, object> SetValue { get; }
			public Func<object, object> GetValue { get; }

			public MemberInfo(PropertyInfo pi)
			{
				this.Name = pi.Name;
				this.MemberType = pi.PropertyType;
				this.CustomAttributes = pi.GetCustomAttributes();
				this.SetValue = pi.SetValue;
				this.GetValue = pi.GetValue;
			}

			public MemberInfo(FieldInfo fi)
			{
				this.Name = fi.Name;
				this.MemberType = fi.FieldType;
				this.CustomAttributes = fi.GetCustomAttributes();
				this.SetValue = fi.SetValue;
				this.GetValue = fi.GetValue;
			}
		}

		readonly struct SignalHandlerInfo
		{
			public string MethodName { get; }
			public SignalHandlerAttribute Attribute { get; }

			public SignalHandlerInfo(string methodName, SignalHandlerAttribute attr) =>
				(MethodName, Attribute) = (methodName, attr);
		}

		readonly struct ResourceInfo
		{
			public string MemberName { get; }
			public ResourceAttribute Attribute { get; }
			
			public ResourceInfo(string memberName, ResourceAttribute attr) =>
				(MemberName, Attribute) = (memberName, attr);
		}

		readonly private static Dictionary<Type, MemberInfo[]> typeMembers = new Dictionary<Type, MemberInfo[]>();
		readonly private static Dictionary<Type, SignalHandlerInfo[]> signalHandlers = new Dictionary<Type, SignalHandlerInfo[]>();

	}
}