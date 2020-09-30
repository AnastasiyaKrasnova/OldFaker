using System;
using System.IO;
namespace ConsoleTest
{
    interface IWriter
    {
        void Write(string serialized,string path);
    }
}
