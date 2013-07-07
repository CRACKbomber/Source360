using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Diagnostics;
using Source360.xZip;
using System.IO;
namespace Source360.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            ZipPackageFile file = new ZipPackageFile("zip0.360.zip");
            if (!Directory.Exists("output"))
                Directory.CreateDirectory("output");
            file.ExtractAllFiles("output");
            watch.Stop();
            Console.WriteLine("The zip was opened and extracted in {0} seconds",watch.ElapsedMilliseconds / 1000);
            Console.ReadKey();
        }
    }
}
