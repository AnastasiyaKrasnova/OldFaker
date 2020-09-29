using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace OldTracer
{
    public class Tracer : ITracer
    {
        private TraceResult _traceResult;

        private static Tracer Instance;
        private static readonly object SyncRoot = new object();

        private Tracer()
        {
            _traceResult = new TraceResult();
        }


        public void StartTrace()
        {
            StackTrace _stackTrace = new StackTrace(1);
            StackFrame _frame = _stackTrace.GetFrame(0);
            MethodBase _method = _frame.GetMethod();
            _traceResult.StartTrace(Thread.CurrentThread.ManagedThreadId, _method);
        }

        public void StopTrace()
        {
            _traceResult.StopTrace(Thread.CurrentThread.ManagedThreadId);
        }

        public TraceResult GetTraceResult()
        {
            return _traceResult;
        }

        public static Tracer GetInstance()
        {
            if (Instance == null)
            {
                lock (SyncRoot)
                {
                    if (Instance == null)
                    {
                        Instance = new Tracer();
                    }
                }
            }

            return Instance;
        }
    }
}
