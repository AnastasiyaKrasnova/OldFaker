using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldTracer
{
    public class TextSerialize: ISerializer
    {
        public void Serialize(TraceResult traceResult)
        {
            foreach (KeyValuePair<int, ThreadInfo> threadInfo in traceResult._threadList)
            {
                Console.WriteLine($"id: {threadInfo.Key}, time: {threadInfo.Value._thread_time}");
                foreach (MethodTrace tracedMethod in threadInfo.Value._methods)
                {
                    DisplayMethods(tracedMethod);
                }
            }
        }

        private void DisplayMethods(MethodTrace mth, int level = 0)
        {
            string tab = string.Format($"{{0, {level * 4 + 1}}}", string.Empty);

            Console.WriteLine($"{tab}method: {mth.name}");
            Console.WriteLine($"{tab}class: {mth.classname}");
            Console.WriteLine($"{tab}time: {mth.time} ms");

            foreach (MethodTrace nestedmth in mth._nestedStack)
            {
                DisplayMethods(nestedmth, level + 1);
            }
        }
    }
}
