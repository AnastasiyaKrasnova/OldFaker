using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OldTracer;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private static ITracer _tracer = Tracer.GetInstance();
        private static A _A;
        private static B _B;

        [TestInitialize]
        public void Setup()
        {
            _tracer.StartTrace();
            Thread thread1 = new Thread(M2);
            Thread thread2 = new Thread(M2);
            _A = new A(_tracer);
            _B = new B(_tracer);
            _A.MethodA();
            _B.MethodB();

            thread1.Start();
            thread2.Start();
            _tracer.StopTrace();
            thread1.Join();
            thread2.Join();
            _tracer.GetTraceResult();
        }


        

        [TestMethod]
        public void TestMethodNames()
        {
            Assert.AreEqual("MethodB", _tracer.GetTraceResult()._threadList[14]._methods[0]._nestedStack[1].name);
            Assert.AreEqual("MethodA1", _tracer.GetTraceResult()._threadList[4]._methods[0]._nestedStack[0]._nestedStack[0].name);
            Assert.AreEqual("M2", _tracer.GetTraceResult()._threadList[13]._methods[0].name);
        }

        [TestMethod]
        public void TestMethodClasses()
        {
            Assert.AreEqual("B", _tracer.GetTraceResult()._threadList[14]._methods[0]._nestedStack[1].classname);
           
        }

        [TestMethod]
        public void TestExecutionTime()
        {
            Assert.IsTrue(_tracer.GetTraceResult()._threadList[13]._methods[0].time > _tracer.GetTraceResult()._threadList[13]._methods[0]._nestedStack[0].time);
            Assert.IsTrue(_tracer.GetTraceResult()._threadList[4]._methods[0].time == _tracer.GetTraceResult()._threadList[4]._thread_time);
        }

        [TestMethod]
        public void TestCountThread()
        {
            Assert.AreEqual(3, _tracer.GetTraceResult()._threadList.Count);
        }

        public static void M2()
        {
            _tracer.StartTrace();
            _A.MethodA();
            Thread.Sleep(200);
            _B.MethodB();
            _tracer.StopTrace();
        }
    }

    public class A
    {
        private ITracer _trace;
        private B _B;
        public static bool fl = false;
        public A(ITracer trace)
        {
            _trace = trace;
            _B = new B(_trace);
        }
        public void MethodA()
        {
            _trace.StartTrace();
            if (!fl)
                MethodA1();
            _B.MethodB();
            Thread.Sleep(30);
            _trace.StopTrace();
        }

        public void MethodA1()
        {
            _trace.StartTrace();
            for (int i = 0; i < 3; i++)
            {
                fl = true;
                MethodA();
            }
            Thread.Sleep(30);
            _trace.StopTrace();
        }
    }
    class B
    {
        private ITracer _trace;
        public B(ITracer trace)
        {
            _trace = trace;
        }
        public void MethodB()
        {
            _trace.StartTrace();
            Thread.Sleep(40);
            _trace.StopTrace();
        }
    }
}


