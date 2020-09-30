using System.Collections.Generic;
using System.Text;
using OldTracer;

namespace ConsoleTest
{
    public class TextSerialize: ISerializer
    {
        public string Serialize(TraceResult traceResult)
        {
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<int, ThreadInfo> threadInfo in traceResult._threadList)
            {
                str.AppendLine($"id: {threadInfo.Key}, time: {threadInfo.Value._thread_time}");
                foreach (MethodTrace tracedMethod in threadInfo.Value._methods)
                {
                    str.AppendLine(DisplayMethods(tracedMethod));
                }
            }
            return str.ToString();
        }

        private string DisplayMethods(MethodTrace mth, int level = 0)
        {
            StringBuilder str = new StringBuilder();
            string tab = string.Format($"{{0, {level * 4 + 1}}}", string.Empty);
            str.AppendLine($"{tab}method: {mth.name}");
            str.AppendLine($"{tab}class: {mth.classname}");
            str.AppendLine($"{tab}time: {mth.time} ms");

            foreach (MethodTrace nestedmth in mth._nestedStack)
            {
                str.AppendLine(DisplayMethods(nestedmth,level + 1));
            }

            return str.ToString();
        }
    }
}
