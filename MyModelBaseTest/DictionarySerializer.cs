﻿
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MyModelBaseTest
{
	public class DictionarySerializer : IDictionarySerializer
	{
		#region 单例字段
		public static readonly DictionarySerializer Default = new DictionarySerializer();
		#endregion

		#region 序列方法
		public IDictionary Serialize(object graph)
		{
			var dictionary = new Dictionary<string, object>();
			this.Serialize(graph, dictionary);
			return dictionary;
		}

		public void Serialize(object graph, IDictionary dictionary)
		{
			if(graph == null)
				return;

			if(dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));

			this.Serialize(graph, dictionary, null, new HashSet<object>());
		}

		private void Serialize(object graph, IDictionary dictionary, string prefix, HashSet<object> hashset)
		{
			if(graph == null || hashset.Contains(graph))
				return;

			if(dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));

			if(graph.GetType().IsScalarType())
			{
				//写入单值到字典中
				this.WriteScalarValue(graph, dictionary, prefix);

				return;
			}

			//将当前序列化对象加入到已解析的栈中
			hashset.Add(graph);

			//将当前序列化对象的类型名加入到字典中
			dictionary.Add(string.IsNullOrEmpty(prefix) ? "$type" : "$type:" + prefix, graph.GetType().AssemblyQualifiedName);

			//获取当前序列化对象的属性集
			var properties = graph.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

			foreach(var property in properties)
			{
				if(!property.CanRead || property.GetIndexParameters().Length > 0)
					continue;

				var key = string.IsNullOrEmpty(prefix) ? property.Name : prefix + "." + property.Name;

				if(property.PropertyType.IsScalarType())
				{
					this.WriteScalarValue(property.GetValue(graph), dictionary, key);
				}
				else
				{
					if(property.PropertyType.IsDictionary())
					{
						var entries = (IEnumerable)property.GetValue(graph);

						foreach(var entry in entries)
						{
							var entryKey = entry.GetType().GetProperty("Key", BindingFlags.Public | BindingFlags.Instance).GetValue(entry);
							var entryValue = entry.GetType().GetProperty("Value", BindingFlags.Public | BindingFlags.Instance).GetValue(entry);

							this.Serialize(entry, dictionary, key + $"[{entryKey}]", hashset);
						}
					}
					else if(property.PropertyType.IsEnumerable())
					{
						var entries = (IEnumerable)property.GetValue(graph);
						var index = 0;

						foreach(var entry in entries)
						{
							this.Serialize(entry, dictionary, key + $"[{index++}]", hashset);
						}
					}
					else
					{
						this.Serialize(property.GetValue(graph), dictionary, key, hashset);
					}
				}
			}
		}

		private void WriteScalarValue(object value, IDictionary dictionary, string prefix)
		{
			if(value.GetType().IsArray)
			{
				var index = 0;

				foreach(var entry in (IEnumerable)value)
				{
					dictionary.Add((string.IsNullOrEmpty(prefix) ? string.Empty : prefix) + $"[{index++}]", entry);
				}
			}
			else
			{
				dictionary.Add(string.IsNullOrEmpty(prefix) ? string.Empty : prefix, value);
			}
		}
		#endregion

		#region 反序列化
		public T Deserialize<T>(IDictionary dictionary)
		{
			return (T)this.Deserialize(dictionary, typeof(T), null);
		}

		public T Deserialize<T>(IDictionary dictionary, Func<MemberGettingContext, MemberGettingResult> resolve)
		{
			return (T)this.Deserialize(dictionary, typeof(T), resolve);
		}

		public object Deserialize(IDictionary dictionary, Type type)
		{
			return this.Deserialize(dictionary, type, null);
		}

		public object Deserialize(IDictionary dictionary, Type type, Func<MemberGettingContext, MemberGettingResult> resolve)
		{
			if(dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));
			if(type == null)
				throw new ArgumentNullException(nameof(type));

			var result = Activator.CreateInstance(type, dictionary);

			if(result == null)
				return null;

			var properties = result.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach(var property in properties)
			{
				object propertyValue = null;

				if(property.GetIndexParameters().Length > 0)
					continue;

				if(property.PropertyType.IsDictionary())
				{
					object value;

					if(dictionary.TryGetValue(property.Name, out value))
					{
						if(value == null)
							continue;

						if(value.GetType().IsDictionary())
						{
							propertyValue = property.GetValue(result);

							if(property == null && property.CanWrite)
								propertyValue = Activator.CreateInstance(property.PropertyType, value);
						}
					}
				}
				else if(property.PropertyType.IsCollection())
				{
				}
				else
				{
					if(!property.CanWrite)
						continue;

				}
			}

			throw new NotImplementedException();
		}

		private void SetProperties(object target, IEnumerable source)
		{
			if(target == null || source == null)
				return;

			if(target.GetType().IsDictionary())
			{
				var entries = DictionaryExtension.ToDictionary(source);


			}
		}
		#endregion
	}
}
