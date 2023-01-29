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

			if (TypeMembers.TryGetValue(type, out var members) == false)
			{
				var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
				members = type.GetFields(bindingFlags).Select(fi => new MemberInfo(fi))
							.Concat(type.GetProperties(bindingFlags).Select(pi => new MemberInfo(pi)))
							.ToArray();
				TypeMembers[type] = members;
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
						case SingletonAttribute singAttr:
							LoadSingleton(node, member, singAttr.Name);
							break;
					}
				}
			}

			if (SignalHandlers.TryGetValue(type, out var handlers) == false)
			{
				handlers = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
							.SelectMany(mi => mi.GetCustomAttributes()
								.OfType<SignalHandlerAttribute>()
								.Select(attr => new SignalHandlerInfo(mi.Name, attr))
							)
							.ToArray();
				SignalHandlers[type] = handlers;
			}

			foreach (var handler in handlers)
			{
				ConnectSignalHandler(node, handler.MethodName, handler.Attribute);
			}
		}

		private static void ConnectSignalHandler(Node node, string methodName, SignalHandlerAttribute attr) {
			var signal = attr.SignalName;
			Node sender = null;

			if (!string.IsNullOrEmpty(attr.TargetNodeField))
			{
				MemberInfo[] members = TypeMembers[node.GetType()];
				MemberInfo? member = members.FirstOrDefault(mi => mi.Name == attr.TargetNodeField);

				sender = member?.GetValue(node) as Node
					?? throw new Exception($"SignalHandlerAttribute on '{node.GetType().FullName}.{methodName}', '{attr.TargetNodeField}' is a nonexistent field or property.");
			} else {
				sender = node;
			}

			if (sender == null) {
				throw new Exception($"SignalHandlerAttribute on '{node.GetType().FullName}.{methodName}', '{attr.TargetNodeField}' is a null value, or property, unable to get.");
			}

			if (!sender.IsConnected(signal, new Callable(node, methodName)))
			{
				sender.Connect(signal, new Callable(node, methodName));
			}
		}

		private static void ResolveNodeFromPath(Node node, MemberInfo member, string targetFieldName) {
			var type = node.GetType();
			MemberInfo targetMember = type.GetField(targetFieldName) is FieldInfo fi
						? new MemberInfo(fi)
						: type.GetProperty(targetFieldName) is PropertyInfo pi
						? new MemberInfo(pi)
						: throw new Exception($"ResolveNodeAttribute on {type.FullName}.{member.Name} targets nonexistent field or property {targetFieldName}");
			
			NodePath path = targetMember.GetValue(node) as NodePath
					?? throw new Exception($"ResolveNodeAttribute on {type.FullName}.{member.Name} targets property {targetFieldName} which is not a NodePath");
			
			AssignPathToMember(node, member, path);
		}

		private static void LoadResource(Node node, MemberInfo member, string resourcePath) {
			Resource res;
			try {
				res = GD.Load(resourcePath);
			} catch (Exception ex) {
				throw new Exception($"Failed to load Resource '{resourcePath}', Message: '{ex.Message}'.", ex);
			}

			if (res == null) {
				throw new Exception($"Failed to load Resource '{resourcePath}`, File not found!");
			}

			try {
				member.SetValue(node, res);
			} catch (Exception ex)
			{
				throw new Exception($"Failed to set variable {member.Name} with the {member.MemberType} for {resourcePath}.",ex);
			}
		}

		private static Node TryGetNode(Node node, List<string> names)
		{
			foreach(var name in names) {
				if (string.IsNullOrEmpty(name)) continue;
				var target = node.GetNodeOrNull(name);
				if (target != null)
					return target;
				if (node.Owner == null) continue;
				target = node.Owner.GetNodeOrNull(name);
				if (target != null)
					return target;
			}
			return null;
		}

		private static void LoadSingleton(Node node, MemberInfo member, string name)
		{
			var name1 = member.Name;
			if (!name1.StartsWith("_"))
				name1 = string.Empty;
			else
			{
				name1 = char.ToUpperInvariant(member.Name[1]) + member.Name[2..];
				// name1 = member.Name.Replace("_", string.Empty);
				// name1 = char.ToUpperInvariant(name1[0]) + name[1..];
			}
			List<string> names = new List<string>()
			{
				string.IsNullOrEmpty(name) ? name : $"/root/{name}",
				$"/root/{member.Name}",
				string.IsNullOrEmpty(name1) ? name1 : $"/root/{name1}",
				$"/root/{member.MemberType.Name}"
			};

			if (names.Contains(""))
				names.RemoveAll(string.IsNullOrEmpty);

			Node value = TryGetNode(node, names);

			if (value == null) {
				throw new Exception($"Failed to load Singleton/Autoload for {member.MemberType.Name}.  Node was not found at /root with the following names: {string.Join(",", names.ToArray())}");
			}
			try {
				member.SetValue(node, value);
			} catch (Exception ex) {
				throw new Exception($"Failed to load Singleton/Autoload for {member.MemberType.Name}.  Error setting node value for {member.Name}.", ex);
			}
		}

		private static void AssignPathToMember(Node node, MemberInfo member, NodePath path)
		{
			var name1 = member.Name;
			if (!name1.StartsWith("_"))
				name1 = string.Empty;
			else
			{
				name1 = char.ToUpperInvariant(member.Name[1]) + member.Name[2..];
				// name1 = member.Name.Replace("_", string.Empty);
				// name1 = char.ToUpperInvariant(name1[0]) + name1.Substring(1);
			}
			List<string> names = new List<string>()
			{
				path.ToString(),
				member.Name,
				$"%{member.Name}",
				name1,
				string.IsNullOrEmpty(name1) ? "" : $"%{name1}",
				member.MemberType.Name
			};

			if (names.Contains(""))
				names.RemoveAll(string.IsNullOrEmpty);

			Node value = TryGetNode(node, names);

			if (value == null)
				throw new Exception($"AssignPathToMember on {node.GetType().FullName}.{member.Name} - Unable to find node with the following names: {string.Join(",", names.ToArray())}");
			try
			{
				member.SetValue(node,value);
			}
			catch (ArgumentException e)
			{
				throw new Exception($"AssignPathToMember on {node.GetType().FullName}.{member.Name} - cannot set value of type {value?.GetType().Name} on field type {member.MemberType.Name}", e);
			}
		}

		private readonly struct MemberInfo
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

		private readonly struct SignalHandlerInfo
		{
			public string MethodName { get; }
			public SignalHandlerAttribute Attribute { get; }

			public SignalHandlerInfo(string methodName, SignalHandlerAttribute attr) =>
				(MethodName, Attribute) = (methodName, attr);
		}

		private static readonly Dictionary<Type, MemberInfo[]> TypeMembers = new Dictionary<Type, MemberInfo[]>();
		private static readonly Dictionary<Type, SignalHandlerInfo[]> SignalHandlers = new Dictionary<Type, SignalHandlerInfo[]>();

	}
}