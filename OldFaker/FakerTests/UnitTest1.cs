using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Faker;

namespace UnitTests
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

		public Test(long[] _testRO, string _String)
		{
			testRO = _testRO;
			String = _String;
			FakerTests.isSecondConstructorCalled = true;

		}
		public Test()
		{
		}
	}
	class Test2
	{
		public Test Test1 { get; set; }
	}

	[TestClass]
	public class FakerTests
	{
		private static OldFaker faker;
		private static Test test;
		public static bool isSecondConstructorCalled=false;

		[ClassInitialize]
		public static void ClassInitilize(TestContext context)
		{
			faker = new OldFaker();
			test = faker.Create<Test>();
		}

		[TestMethod]
		public void FullObjectNotNull()
		{
			Assert.IsNotNull(test, "Faker hasn't created instance of DTO");
		}

		[TestMethod]
		public void IntegerPropertyArrIsNotNull()
		{
			if (test.Prop == null)
				Assert.Fail("Faker hasn't created array of integer property");
		}

		[TestMethod]
		public void LongFieldArrIsNotNull()
		{

			if (test.testRO == null || test.testRO[0]==0)
				Assert.Fail("Faker hasn't created long field");
		}

		[TestMethod]
		public void NestedClass1IsNotNull()
		{
			Assert.IsNotNull(test.Test1, "Faker hasn't created nested DTO property");
		}

		[TestMethod]
		public void QueueOfIntArrPropertyIsNotNull()
		{
			Assert.IsNotNull(test.list, "Faker hasn't created queue of integer array property");
		}

		[TestMethod]
		public void FloatArrPropertyIsNotNull()
		{
			Assert.IsNotNull(test.flNums, "Faker hasn't created queue of integer array property");
		}

		[TestMethod]
		public void StringPropertyIsNotNull()
		{
			Assert.IsNotNull(test.String, "Faker hasn't created string property");
		}

		[TestMethod]
		public void DateTimePropertyIsNotNull()
		{
			Assert.IsNotNull(test.DateTime, "Faker hasn't created DateTime property");
		}

		[TestMethod]
		public void IntFieldIsNotNull()
		{
			if (test.test == 0)
				Assert.Fail("Faker hasn't created integer field");
		}

		[TestMethod]
		public void isStructNull()
		{
			Assert.IsNotNull(test.TStruct);
		}
		[TestMethod]
		public void NestedClass2IsNotNull()
		{
			Assert.IsNotNull(test.Test2, "Faker hasn't created nested DTO property");
		}

		[TestMethod]
		public void FakerCallConstructorWithALargeNumberOfParameters()
		{
			Assert.IsTrue(isSecondConstructorCalled, "Faker has called constructor with a less number of parameters");
		}
	}
}