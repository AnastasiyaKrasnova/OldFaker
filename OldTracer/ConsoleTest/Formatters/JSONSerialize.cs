using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using OldTracer;

namespace ConsoleTest
{
    public class JSONSerialize: ISerializer
    {
        public void Serialize(TraceResult traceResult)
        {
            JSONFile jsfile = new JSONFile();
            jsfile.threads = new List<ThreadstoJson>();
            foreach (KeyValuePair<int, ThreadInfo> threadInfo in traceResult._threadList)
            {
                ThreadstoJson threadjs = new ThreadstoJson();
                threadjs.id = threadInfo.Key;
                threadjs.time = threadInfo.Value._thread_time;
                threadjs.methods= new List<MethodtoJson>();
                foreach (MethodTrace tracedMethod in threadInfo.Value._methods)
                {
                    MethodtoJson mth=TransformMethodInfo(tracedMethod);
                    threadjs.methods.Add(mth);
                }
                jsfile.threads.Add(threadjs);
            }
            JSONConfig(jsfile);
        }

        private MethodtoJson TransformMethodInfo(MethodTrace methodTrace)
        {
            MethodtoJson mth = new MethodtoJson();
            mth.name = methodTrace.name;
            mth.classname = methodTrace.classname;
            mth.time = methodTrace.time;
            mth.methods = new List<MethodtoJson>();
            foreach (MethodTrace nestedMethodTrace in methodTrace._nestedStack)
            {
                MethodtoJson nestedmth=TransformMethodInfo(nestedMethodTrace);
                if (nestedmth!=null)
                    mth.methods.Add(nestedmth);
            }
            return mth;
        }

        private void JSONConfig(JSONFile jsfile)
        {
            //JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string serialized = JsonConvert.SerializeObject(jsfile);
            FileStream fs = File.Create("TraceResult.json");
            byte[] ret = System.Text.Encoding.UTF8.GetBytes(serialized);
            fs.Write(ret, 0, ret.Length);
            fs.Dispose();
        }
    }

    [Serializable]
    public class JSONFile
    {
        public List<ThreadstoJson> threads;
    }

    [Serializable]
    public class ThreadstoJson
    {
        public int id;
        public long time;
        public List<MethodtoJson> methods;
    }

    [Serializable]
    public class MethodtoJson
    {
        public string name;
        public string classname;
        public long time;
        public List<MethodtoJson> methods;
    }


}
