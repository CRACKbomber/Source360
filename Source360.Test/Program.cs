using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Diagnostics;
using Source360.xZip;
using Source360.VTF;
using Source360.Common.Security;
using System.IO;
using Source360.IO;
namespace Source360.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            VICE vice = new VICE();
            using (IOReader reader = new IOReader(File.OpenRead("test.nuc")))
            {
                byte[] temp = new byte[reader.BaseStream.Length];
                int bytesLeft = (int)reader.BaseStream.Length;
                while (bytesLeft >= 8)
                {
                    byte[] tmp = new byte[8];
                    byte[] buffer = reader.ReadBytes(8);
                    vice.Decrypt("SDhfi878", ref buffer, ref tmp);
                    bytesLeft -= 8;
                    Array.Copy(tmp, temp, 8);
                }
                File.WriteAllBytes("out.nut", temp);
            }
        }
    }
}
