using System;
using System.Collections.Generic;
using System.Linq;

namespace OldTracer
{
    public class ThreadInfo
    {
        private  Stack<MethodTrace> _nestedStack= new Stack<MethodTrace>();
        public  List<MethodTrace> _methods= new List<MethodTrace>();
        public long _thread_time;

        internal void StartTrace(MethodTrace mth)
        {
            if (_nestedStack.Count == 0)
            {
                _methods.Add(mth);
            }
            else
            {
                _nestedStack.Peek().NewNestedMethod(mth);
            }
            _nestedStack.Push(mth);
        }

        internal void StopTrace()
        {
            _nestedStack.Pop().StopTrace();
            _thread_time = _methods[0].time;
        }   
    }
}
