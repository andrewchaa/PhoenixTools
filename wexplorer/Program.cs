using System;
using System.Diagnostics;
using System.IO;

namespace Open
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                var fileInfos = new DirectoryInfo(".").GetFiles("*.sln");

                if (fileInfos.Length == 0)
                    Console.WriteLine("No Solution File Found");
                else
                {
                    Console.WriteLine("Opening: " + fileInfos[0].Name);
                    Process.Start(fileInfos[0].FullName);
                }
            }
            else
            {
                var path = string.Join(" ", args);
                Process.Start(new DirectoryInfo(path).FullName);
            }
        }
    }
}
