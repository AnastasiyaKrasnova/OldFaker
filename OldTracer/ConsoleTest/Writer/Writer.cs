using System;
using System.IO;
namespace ConsoleTest
{
    class Writer: IWriter
    {
        public void Write(string serialized,string path)
        {
            if (path == "")
            {
                Console.Write(serialized);
            }
            else
            {
                using (FileStream fs=File.Create(path))
                {
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(serialized);
                    fs.Write(data, 0, data.Length);
                }
            }
            
        }
    }
}
