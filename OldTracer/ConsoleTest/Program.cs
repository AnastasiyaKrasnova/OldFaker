using System;
using System.Threading;
using OldTracer;


namespace ConsoleTest
{
    public class Program
    {
        private static ITracer _tracer = Tracer.GetInstance();
        private static A _A;
        private static B _B;
        public static void Main(string[] args)
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
            DisplayResult();
            Console.ReadKey();
        }

        public static void M2()
        {
            _tracer.StartTrace();
            _A.MethodA();
            Thread.Sleep(200);
            _B.MethodB();
            _tracer.StopTrace();
        }

        static void DisplayResult()
        {
            ISerializer _serializer;
            IWriter _writer=new Writer();
            _serializer = new TextSerialize();
            _writer.Write(_serializer.Serialize(_tracer.GetTraceResult()),"");
            _serializer = new XMLSerialize();
            _writer.Write(_serializer.Serialize(_tracer.GetTraceResult()),"TraceResult.xml");
            _serializer = new JSONSerialize();
            _writer.Write(_serializer.Serialize(_tracer.GetTraceResult()),"TraceResult.json");
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

