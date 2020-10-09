using System;
using System.Linq;
using Faker;

namespace Plugins
{
    public class DoubleGen: IGenerator
	{
		public Type[] PossibleTypes => new Type[] { typeof(double) };
		private const int minPow = -323, maxPow = 309;
		private Random _numGen;

		public DoubleGen()
		{
			_numGen = new Random();
		}

		public DoubleGen(Random numGen)
		{
			_numGen = numGen;
		}

		public object Generate(Type type)
		{
			if (!PossibleTypes.Contains(type))
				throw new ArgumentException();

			double exponent = Math.Pow(10.0, _numGen.Next(minPow, maxPow));
			return _numGen.NextDouble() * exponent;
		}
	}
}
