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
                Console.WriteLine("Open .                : open windows explorer in a current folder.");
                Console.WriteLine("Open C:\\program files : open windows explorer in C:\\program files");

                return;
            }

            string path = string.Join(" ", args);
            Process.Start(new DirectoryInfo(path).FullName);
        }
    }
}
