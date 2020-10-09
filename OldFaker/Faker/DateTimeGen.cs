using System;
using System.Linq;

namespace Faker
{
    class DateTimeGen: IGenerator
    {
		public Type[] PossibleTypes => new[] { typeof(DateTime) };
		private const int limit = 1469334605;
		private Random _numGen;

		public DateTimeGen(Random numGen)
		{
			_numGen = numGen;
		}
		public object Generate(Type type)
		{
			if (!PossibleTypes.Contains(type))
				throw new ArgumentException();

			return new DateTime((long)_numGen.Next() * _numGen.Next(limit));
		}
	}
}
