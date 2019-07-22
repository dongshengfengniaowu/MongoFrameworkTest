

using System;
using System.Collections;

namespace MyModelBaseTest
{
	public interface IDictionarySerializer
	{
		IDictionary Serialize(object graph);
		void Serialize(object graph, IDictionary dictionary);

		object Deserialize(IDictionary dictionary, Type type);
	}
}
