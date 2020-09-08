using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Tracer
{
    public class TraceResult : ITracer
    {
        static public int _listPos = 0;
        static internal List<Info> _data = new List<Info>();
        static private List<_TreeNode> _Tree = new List<_TreeNode>();
        private struct node_data
        {
            public string name;
            public string mclass;
            public int threadid;
            public string time;
        };
        private struct _TreeNode
        {
            public List<node_data> parents;
            public List<node_data> children;
            public node_data data;
        }

        public void GetTraceResult()
        {
            foreach (Info info in _data)
            {
                Console.WriteLine("Thread id: {0},Method name:{2}, method class: {3}, time: {1}, Method Data: ", info._threadId, info._sw.ElapsedMilliseconds.ToString(), info._method_name, info._method_class);
                foreach (StackFrame frame in info._stack)
                {
                    MethodInfo mi = (MethodInfo)frame.GetMethod();
                    Console.WriteLine(mi.Name + "    " + mi.DeclaringType.ToString());
                }
            }
            MakeTree(_data);
        }
        public void StartTrace()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            StackTrace stackTrace = new StackTrace();
            StackFrame[] frames = stackTrace.GetFrames();
            string name = frames[1].GetMethod().Name;
            string mclass = frames[1].GetMethod().DeclaringType.Name;
            _data.Add(new Info(Thread.CurrentThread.ManagedThreadId, frames, sw, name, mclass));
            _listPos++;

        }

        public void StopTrace()
        {
            _listPos--;
            _data[_listPos]._sw.Stop();
        }

        private node_data CreateNode(Info info)
        {
            node_data data = new node_data();
            if (info != null)
            {
                data.name = info._method_name;
                data.mclass = info._method_class;
                data.time = info._sw.ElapsedMilliseconds.ToString();
                data.threadid = info._threadId;
            }
            return data;
        }

        private void AddParent(ref _TreeNode node, Info meth_p)
        {
            node_data d_p = CreateNode(meth_p);
            if (node.parents == null)
            {
                node.parents = new List<node_data>();
            }
            if (d_p.name != null && !node.parents.Exists(x => x.name == d_p.name && x.mclass == d_p.mclass && x.threadid == d_p.threadid))
            {
                node.parents.Add(d_p);
            }
        }

        private void AddChild(ref _TreeNode node, Info meth_c)
        {
            node_data d_c = CreateNode(meth_c);
            if (node.children == null)
            {
                node.children = new List<node_data>();
            }
            if (d_c.name != null && !node.children.Exists(x => x.name == d_c.name && x.mclass == d_c.mclass && x.threadid == d_c.threadid))
            {
                node.children.Add(d_c);
            }
        }
        private void AddNode(Info meth, Info meth_p, Info meth_c)
        {
            _TreeNode node;
            if (_Tree.Exists(x => x.data.name == meth._method_name && x.data.mclass == meth._method_class && x.data.threadid == meth._threadId))
            {
                node = _Tree.Find(x => x.data.name == meth._method_name && x.data.mclass == meth._method_class && x.data.threadid == meth._threadId);
                AddParent(ref node, meth_p);
                AddChild(ref node, meth_c);
            }
            else
            {
                node = new _TreeNode();
                node.data = CreateNode(meth);
                AddParent(ref node, meth_p);
                AddChild(ref node, meth_c);
                _Tree.Add(node);
            }
        }

        private void MakeTree(List<Info> data)
        {
            foreach (Info info in data)
            {
                for (int i = 1; i < info._stack.Count(); i++)
                {
                    MethodInfo mi = (MethodInfo)info._stack[i].GetMethod();
                    if (mi.Name == "ThreadStart_Context" && mi.DeclaringType.FullName == "System.Threading.ThreadHelper")
                    {
                        break;
                    }
                    Info meth = data.Find(x => x._method_name == mi.Name && x._method_class == mi.DeclaringType.Name && x._threadId == info._threadId);
                    Info meth_p = null;
                    Info meth_c = null;
                    if (i < info._stack.Count() - 1)
                    {
                        MethodInfo mi_p = (MethodInfo)info._stack[i + 1].GetMethod();
                        meth_p = data.Find(x => x._method_name == mi_p.Name && x._method_class == mi_p.DeclaringType.Name && x._threadId == info._threadId);
                    }
                    if (i > 1)
                    {
                        MethodInfo mi_c = (MethodInfo)info._stack[i - 1].GetMethod();
                        meth_c = data.Find(x => x._method_name == mi_c.Name && x._method_class == mi_c.DeclaringType.Name && x._threadId == info._threadId);
                    }
                    AddNode(meth, meth_p, meth_c);
                }
            }
        }

    }

    internal class Info
    {
        public int _threadId;
        public StackFrame[] _stack;
        public string _method_name;
        public string _method_class;
        public Stopwatch _sw;
        public Info(int threadId, StackFrame[] stack, Stopwatch sw, string name, string mclass)
        {
            _threadId = threadId;
            _stack = stack;
            _sw = sw;
            _method_name = name;
            _method_class = mclass;

        }
    }
}
