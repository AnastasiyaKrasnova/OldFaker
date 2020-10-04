using System;
using System.Linq;

namespace Faker
{
    class FloatGen: IGenerator
    {
		public Type[] PossibleTypes => new[] { typeof(float) };
		private const int minPow = -44, maxPow = 39;
		private Random _numGen;

		public FloatGen(Random numGen)
		{
			_numGen = numGen ?? throw new ArgumentNullException();
		}

		public object Generate(Type type)
		{
			if (!PossibleTypes.Contains(type))
				throw new ArgumentException();

			double exponent = Math.Pow(10.0, _numGen.Next(minPow, maxPow));
			return (float)(_numGen.NextDouble() * exponent);
		}
	}
}
