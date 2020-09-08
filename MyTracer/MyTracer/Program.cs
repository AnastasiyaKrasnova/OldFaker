using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Tracer;

namespace ConsoleTests
{
    public class Program
    {
        private static A _A;
        private static B _B;
        private static TraceResult _trace;
        public static void Main(string[] args)
        {
            _trace = new TraceResult();
            _trace.StartTrace();
            Thread thread1 = new Thread(M2);
            Thread thread2 = new Thread(M2);
            _A = new A(_trace);
            _B = new B(_trace);
            _A.MethodA();
            _B.MethodB();

            thread1.Start();
            thread2.Start();
            _trace.StopTrace();
            thread1.Join();
            thread2.Join();
            _trace.GetTraceResult();
            Console.ReadKey();
        }

        public static void M2()
        {
            _trace.StartTrace();
            _A.MethodA();
            Thread.Sleep(200);
            _trace.StopTrace();
        }
    }
    public class A
    {
        private TraceResult _trace;
        private B _B;
        public A(TraceResult trace)
        {
            _trace = trace;
            _B = new B(_trace);
        }
        public void MethodA()
        {
            _trace.StartTrace();
            _B.MethodB();
            Thread.Sleep(30);
            _trace.StopTrace();
        }
    }
    class B
    {
        private TraceResult _trace;
        public B(TraceResult trace)
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
