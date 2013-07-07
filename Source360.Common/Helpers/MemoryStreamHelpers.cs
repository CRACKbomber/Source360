using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
namespace Source360.Common.Helpers
{
    public static partial class MemoryStreamHelper
    {
        public static void Write(this MemoryStream memstream, short data)
        {
            byte[] bytedata = BitConverter.GetBytes(data);
            memstream.Write(bytedata, 0, bytedata.Length);
        }
        public static void Write(this MemoryStream memstream, ushort data)
        {
            byte[] bytedata = BitConverter.GetBytes(data);
            memstream.Write(bytedata, 0, bytedata.Length);
        }
        public static void Write(this MemoryStream memstream, int data)
        {
            byte[] bytedata = BitConverter.GetBytes(data);
            memstream.Write(bytedata, 0, bytedata.Length);
        }
        public static void Write(this MemoryStream memstream, uint data)
        {
            byte[] bytedata = BitConverter.GetBytes(data);
            memstream.Write(bytedata, 0, bytedata.Length);
        }
        public static void Write(this MemoryStream memstream, float data)
        {
            byte[] bytedata = BitConverter.GetBytes(data);
            memstream.Write(bytedata, 0, bytedata.Length);
        }
        public static void Write(this MemoryStream memstream, string data)
        {
            byte[] byteString = Encoding.ASCII.GetBytes(data);
            memstream.Write(byteString, 0, byteString.Length);
        }
        public static void Write(this MemoryStream memstream, byte[] data)
        {
            memstream.Write(data, 0, data.Length);
        }
        public static ushort ReadUShort(this MemoryStream memstream)
        {
            byte[] data = new byte[2];
            memstream.Read(data, 0, 2);
            return BitConverter.ToUInt16(data, 0);
        }
        public static uint ReadUInt(this MemoryStream memstream)
        {
            byte[] data = new byte[4];
            memstream.Read(data, 0, 4);
            return BitConverter.ToUInt32(data, 0);
        }
        public static int ReadInt(this MemoryStream memstream)
        {
            byte[] data = new byte[4];
            memstream.Read(data, 0, 4);
            return BitConverter.ToInt32(data, 0);
        }
        public static short ReadShort(this MemoryStream memstream)
        {
            byte[] data = new byte[2];
            memstream.Read(data, 0, 2);
            return BitConverter.ToInt16(data, 0);
        }
        public static string ReadString(this MemoryStream memstream, int strLen)
        {
            byte[] stringData = new byte[strLen];
            memstream.Read(stringData, 0, strLen);
            return Encoding.ASCII.GetString(stringData);
        }
        public static byte[] ReadBytes(this MemoryStream memStream, int len)
        {
            byte[] buffer = new byte[len];
            memStream.Read(buffer, 0, len);
            return buffer;
        }
        public static float ReadFloat(this MemoryStream memStream)
        {
            byte[] data = new byte[4];
            memStream.Read(data, 0, 4);
            return (float)BitConverter.ToSingle(data, 0);
        }
        public static byte[] Swap(this MemoryStream memStream)
        {
            byte[] oldStream = new byte[memStream.Length];
            memStream.Read(oldStream, 0, oldStream.Length);
            Array.Reverse(oldStream);
            return oldStream;
        }
        public static T ReadStruct<T>(this MemoryStream memStream)
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
            memStream.Read(buffer, 0, Marshal.SizeOf(typeof(T)));
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            T temp = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return temp;
        }
        public static void WriteStruct<T>(this MemoryStream memStream)
        {
            int rawsize = Marshal.SizeOf(typeof(T));
            byte[] rawdata = new byte[rawsize];
            GCHandle handle = GCHandle.Alloc(rawdata, GCHandleType.Pinned);
            Marshal.StructureToPtr(typeof(T), handle.AddrOfPinnedObject(), false);
            handle.Free();
            memStream.Write(rawdata);
        }
    }
}
