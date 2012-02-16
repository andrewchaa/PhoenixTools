using System.Diagnostics;
using System.IO;

namespace wex
{
    class Program
    {
        static void Main()
        {
            Process.Start(new DirectoryInfo(".").FullName);
        }
    }
}
