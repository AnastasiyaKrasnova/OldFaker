using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Faker
{
    class CollectionGen: IGenerator
    {
		public Type[] PossibleTypes => new Type[]{ typeof(List<>), typeof(Stack<>), typeof(Queue<>) };
		private ObjectCreator _objCreator;

		public CollectionGen(ObjectCreator objCreator)
		{
			_objCreator = objCreator;
		}

		public object Generate(Type type)
		{
			if (!PossibleTypes.Contains(type.GetGenericTypeDefinition()))
				throw new ArgumentException();

			ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(IEnumerable<>).MakeGenericType(type.GenericTypeArguments) });

			object[] args = new[] { _objCreator.CreateInstance(type.GenericTypeArguments[0].MakeArrayType()) };

			return constructor.Invoke(args);
		}
	}
}
