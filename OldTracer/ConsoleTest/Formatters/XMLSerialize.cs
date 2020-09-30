using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using OldTracer;

namespace ConsoleTest
{
    public class XMLSerialize: ISerializer
    {

        public string Serialize(TraceResult traceResult)
        {
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            var root = new XElement("root");
            foreach (KeyValuePair<int, ThreadInfo> threadInfo in traceResult._threadList)
            {
                var thread = new XElement("thread");
                thread.Add(new XAttribute("id", threadInfo.Key));
                thread.Add(new XAttribute("time", threadInfo.Value._thread_time));

                foreach (MethodTrace methodTrace in threadInfo.Value._methods)
                {
                    thread.Add(FormMethodData(methodTrace));
                }

                root.Add(thread);
            }
            doc.Add(root);
            string str = doc.Declaration.ToString()+"\n"+ doc.ToString();
            return str;
        }

        private static XElement FormMethodData(MethodTrace methodTrace)
        {
            var result = new XElement("method");
            result.Add(new XAttribute("name", methodTrace.name));
            result.Add(new XAttribute("time", methodTrace.time));
            result.Add(new XAttribute("class", methodTrace.classname));

            foreach (MethodTrace nestedMethodTrace in methodTrace._nestedStack)
            {
                result.Add(FormMethodData(nestedMethodTrace));
            }

            return result;
        }
    }
}
