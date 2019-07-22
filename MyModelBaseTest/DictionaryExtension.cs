using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MyModelBaseTest
{
	public static class DictionaryExtension
	{
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
		public static bool TryGetValue(this IDictionary dictionary, object key, out object value)
		{
			value = null;

			if(dictionary == null || dictionary.Count < 1)
				return false;

			var existed = dictionary.Contains(key);

			if(existed)
				value = dictionary[key];

			return existed;
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
		public static bool TryGetValue(this IDictionary dictionary, object key, Action<object> onGot)
		{
			if(dictionary == null || dictionary.Count < 1)
				return false;

			var existed = dictionary.Contains(key);

			if(existed && onGot != null)
				onGot(dictionary[key]);

			return existed;
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
		public static bool TryGetValue<TValue>(this IDictionary dictionary, object key, out TValue value, Func<object, TValue> converter = null)
		{
			value = default(TValue);

			if(dictionary == null || dictionary.Count < 1)
				return false;

			var existed = dictionary.Contains(key);

			if(existed)
			{
				if(converter == null)
					value = Convert.ConvertValue<TValue>(dictionary[key]);
				else
					value = converter(dictionary[key]);
			}

			return existed;
		}

		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
		public static bool TryGetValue<TValue>(this IDictionary dictionary, object key, Action<object> onGot)
		{
			if(dictionary == null || dictionary.Count < 1)
				return false;

			var existed = dictionary.Contains(key);

			if(existed && onGot != null)
				onGot(Convert.ConvertValue<TValue>(dictionary[key]));

			return existed;
		}

		public static bool TryGetValue<TKey, TValue>(this IDictionary<TKey, object> dictionary, TKey key, out TValue value, Func<object, TValue> converter = null)
		{
			value = default(TValue);

			if(dictionary == null || dictionary.Count < 1)
				return false;

			object result;

			if(dictionary.TryGetValue(key, out result))
			{
				if(converter == null)
					value = Convert.ConvertValue<TValue>(result);
				else
					value = converter(result);

				return true;
			}

			return false;
		}

		public static bool TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Action<TValue> onGot)
		{
			if(dictionary == null || dictionary.Count < 1)
				return false;

			TValue value;

			if(dictionary.TryGetValue(key, out value) && onGot != null)
			{
				onGot(value);
				return true;
			}

			return false;
		}

		public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IDictionary dictionary, Func<object, TKey> keyConvert = null, Func<object, TValue> valueConvert = null)
		{
			if(dictionary == null)
				return null;

			if(keyConvert == null)
				keyConvert = key => Convert.ConvertValue<TKey>(key);

			if(valueConvert == null)
				valueConvert = value => Convert.ConvertValue<TValue>(value);

			var result = new Dictionary<TKey, TValue>(dictionary.Count);
			var iterator = dictionary.GetEnumerator();

			while(iterator.MoveNext())
			{
				result.Add(keyConvert(iterator.Entry.Key), valueConvert(iterator.Entry.Value));
			}

			return result;
		}

		public static IEnumerable<DictionaryEntry> ToDictionary(this IEnumerable source)
		{
			if(source == null)
				yield break;

			if(source is IDictionary || source is IEnumerable<DictionaryEntry>)
			{
				foreach(var item in source)
					yield return (DictionaryEntry)item;
			}
			else
			{
				foreach(var item in source)
				{
					if(item == null)
						continue;

					if(item.GetType().IsGenericType && item.GetType().GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
					{
						yield return new DictionaryEntry(
							item.GetType().GetProperty("Key", BindingFlags.Public | BindingFlags.Instance).GetValue(item),
							item.GetType().GetProperty("Value", BindingFlags.Public | BindingFlags.Instance).GetValue(item));
					}
				}
			}
		}
	}
}
