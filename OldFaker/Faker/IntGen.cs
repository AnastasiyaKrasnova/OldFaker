using System;
using System.Linq;

namespace Faker
{
    class IntGen: IGenerator
    {
		public Type[] PossibleTypes => new[] { typeof(int) };
		private const int minValue = -2147483648, maxValue = 2147483647;
		private Random _numGen;

		public IntGen(Random numGen)
		{
			_numGen = numGen ?? throw new ArgumentNullException();
		}

		public object Generate(Type type)
		{
			if (!PossibleTypes.Contains(type))
				throw new ArgumentException();

			int result = _numGen.Next(minValue, maxValue);

			if (result == 0)
				result = 1;

			return result;
		}
	}
}
