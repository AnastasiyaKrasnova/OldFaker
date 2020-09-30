using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace OldTracer
{
    public class Tracer : ITracer
    {
        private TraceResult _traceResult;
        private static readonly object SyncRoot = new object();

        public Tracer()
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
    }
}
