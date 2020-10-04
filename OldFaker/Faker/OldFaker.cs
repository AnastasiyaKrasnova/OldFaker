using System;

namespace Faker
{
    public class OldFaker
    {
		private FakeCreator _FakeCreator=new FakeCreator();

		public T Create<T>()
		{
			T instance = (T)_FakeCreator.CreateInstance(typeof(T));
			return instance;
		}
	}
}
