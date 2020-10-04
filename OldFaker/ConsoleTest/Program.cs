using System;
using System.Collections.Generic;
using Faker;

namespace ConsoleTest
{
	struct TestStruct
	{
		public TestStruct(int i)
		{
			Prop = i;
		}

		public int Prop { get; set; }
	}

	class Test
	{
		public TestStruct TStruct { get; set; }
		public int[,,] Prop { get; set; }
		public long Prop2 { get; set; }
		public Queue<int[]> list { get; set; }
		public Test2 Test1 { get; set; }
		public Test2 Test2 { get; set; }
		public string String { get; set; }
		public DateTime DateTime { get; set; }
		public double doubleTest { get; set; }
		public float[] flNums { get; set; }
		public int test;
		public long[] testRO;

		private Test(long[] _testRO, string _String)
        {
				testRO = _testRO;
				String = _String;
		}				

		private Test()
		{
		}
	}
	class Test2
	{
		public Test Test1 { get; set; }
	}
	class Program
	{

		static void Main(string[] args)
		{
			OldFaker creator = new OldFaker();
			Test test = creator.Create<Test>();
			Console.WriteLine(test.Prop2);
			Console.WriteLine(test.DateTime);
			Console.WriteLine(test.Test2.Test1.test);
			Console.ReadLine();
		}
	}
}
