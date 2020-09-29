using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

namespace OldTracer
{
    public class MethodTrace
    {
        public List<MethodTrace> _nestedStack=new List<MethodTrace>();
        private Stopwatch _stopwatch=new Stopwatch();

       public string name { get; }
       public string classname { get; }
       public long time { get; set; }

        internal MethodTrace(MethodBase mth)
        {
            name = mth.Name;
            classname = mth.DeclaringType.Name;
            _stopwatch.Start();
        }

        internal void StopTrace()
        {
            _stopwatch.Stop();
            time = _stopwatch.ElapsedMilliseconds;
        }

        internal void NewNestedMethod(MethodTrace methodTraceInfo)
        {
            _nestedStack.Add(methodTraceInfo);
        }
    }
}
