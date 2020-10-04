using System;
using System.Linq;
using System.Text;

namespace Faker
{
    class StringGen: IGenerator
    {
		public Type[] PossibleTypes => new[] { typeof(string) };
		private const int minLength = 5, maxLength = 20;
		private Random _numGen;

		public StringGen(Random numGen)
		{
			_numGen = numGen ?? throw new ArgumentNullException();
		}

		public object Generate(Type type)
		{
			if (!PossibleTypes.Contains(type))
				throw new ArgumentException();

			byte[] chars = new byte[_numGen.Next(minLength, maxLength)];
			_numGen.NextBytes(chars);
			return Encoding.UTF8.GetString(chars); 
		}
	}
}
