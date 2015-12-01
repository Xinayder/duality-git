using System;
using System.Collections.Generic;

namespace RockyTV.GitPlugin.Editor.Extensions
{
	public static class DictionaryExtensions
	{
		public static bool ContainsPair<T, U>(this Dictionary<T, U> dictionary, KeyValuePair<T, U> kvp)
		{
			return dictionary.ContainsKey(kvp.Key) && dictionary.ContainsValue(kvp.Value);
		}

		public static void AddRange<T, U>(this Dictionary<T, U> dictionary, List<KeyValuePair<T, U>> kvpList)
		{
			foreach (KeyValuePair<T, U> kvp in kvpList)
			{
				if (!dictionary.ContainsPair(kvp))
					dictionary.Add(kvp.Key, kvp.Value);
			}
		}
	}
}
