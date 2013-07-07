using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Source360.Common.Helpers;
namespace Source360.IO
{
    public class xMemoryStream : MemoryStream
    {
        private Endian BYTE_ORDER;
        private byte[] tmpBuffer;
        public Endian ByteOrder { get { return this.BYTE_ORDER; } set { this.BYTE_ORDER = value; } }
        #region Constructors
        public xMemoryStream(Endian byteOrder)
            : base() { this.BYTE_ORDER = byteOrder; }
        public xMemoryStream(byte[] buffer, Endian byteOrder)
            : base(buffer) { this.BYTE_ORDER = byteOrder; }
        public xMemoryStream(int capacity, Endian byteOrder)
            : base(capacity) { this.BYTE_ORDER = byteOrder; }
        public xMemoryStream(byte[] buffer, bool writable, Endian byteOrder)
            : base(buffer, writable) { this.BYTE_ORDER = byteOrder; }
        public xMemoryStream(byte[] buffer, int index, int count, Endian byteOrder)
            : base(buffer, index, count) { this.BYTE_ORDER = byteOrder; }
        public xMemoryStream(byte[] buffer, int index, int count, bool writable, Endian byteOrder)
            : base(buffer, index, count, writable) { this.BYTE_ORDER = byteOrder; }
        public xMemoryStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible, Endian byteOrder)
            : base(buffer, index, count, writable, publiclyVisible) { this.BYTE_ORDER = byteOrder; }
        #endregion
        #region Reading Methods
        public byte ReadByte()
        {
            return (byte)base.ReadByte();
        }
        public sbyte ReadSByte()
        {
            return (sbyte)base.ReadByte();
        }
        public short ReadShort()
        {
            tmpBuffer = new byte[2];
            base.Read(tmpBuffer, 0, 2);
            return this.BYTE_ORDER == Endian.Big ? BitConverter.ToInt16(tmpBuffer, 0).Swap() : BitConverter.ToInt16(tmpBuffer, 0);
        }
        public ushort ReadUShort()
        {
            tmpBuffer = new byte[2];
            base.Read(tmpBuffer, 0, 2);
            return this.BYTE_ORDER == Endian.Big ? BitConverter.ToUInt16(tmpBuffer, 0).Swap() : BitConverter.ToUInt16(tmpBuffer, 0);
        }
        public int ReadInt()
        {
            tmpBuffer = new byte[4];
            base.Read(tmpBuffer, 0, 4);
            return this.BYTE_ORDER == Endian.Big ? BitConverter.ToInt32(tmpBuffer, 0).Swap() : BitConverter.ToInt32(tmpBuffer, 0);
        }
        public uint ReadUInt()
        {
            tmpBuffer = new byte[4];
            base.Read(tmpBuffer, 0, 4);
            return this.BYTE_ORDER == Endian.Big ? BitConverter.ToUInt32(tmpBuffer, 0).Swap() : BitConverter.ToUInt32(tmpBuffer, 0);
        }
        public float ReadFloat()
        {
            tmpBuffer = new byte[4];
            base.Read(tmpBuffer, 0, 4);
            return this.BYTE_ORDER == Endian.Big ? BitConverter.ToSingle(tmpBuffer, 0).Swap() : BitConverter.ToSingle(tmpBuffer, 0);
        }
        public byte[] ReadBytes(int count)
        {
            tmpBuffer = new byte[count];
            base.Read(tmpBuffer, 0, count);
            return tmpBuffer;
        }
        public string ReadString(int strLen, Encoding enc)
        {
            tmpBuffer = new byte[strLen];
            base.Read(tmpBuffer, 0, strLen);
            return enc.GetString(tmpBuffer);
        }
        public T ReadStruct<T>()
        {
            tmpBuffer = new byte[Marshal.SizeOf(typeof(T))];
            base.Read(tmpBuffer, 0, Marshal.SizeOf(typeof(T)));
            GCHandle handle = GCHandle.Alloc(tmpBuffer, GCHandleType.Pinned);
            T temp = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return temp;
        }
        #endregion
        #region Writing Methods

        public void Write(byte data) { base.Write(new byte[1] { data }, 0, 1); }
        public void Write(short data)
        {
            if (BYTE_ORDER == Endian.Big)
                data.Swap();
            base.Write(BitConverter.GetBytes(data), 0, 2);
        }
        public void Write(ushort data) 
        {
            if (BYTE_ORDER == Endian.Big)
                data.Swap(); 
            base.Write(BitConverter.GetBytes(data), 0, 2);
        }
        public void Write(int data) 
        {
            if (BYTE_ORDER == Endian.Big)
                data.Swap();
            base.Write(BitConverter.GetBytes(data), 0, 4); 
        }
        public void Write(uint data) 
        {
            if (BYTE_ORDER == Endian.Big)
                data.Swap();
            base.Write(BitConverter.GetBytes(data), 0, 4); 
        }
        public void Write(float data) 
        {
            if (BYTE_ORDER == Endian.Big)
                data.Swap();
            base.Write(BitConverter.GetBytes(data), 0, 4); 
        }
        public void Write(byte[] data) 
        {
            if (BYTE_ORDER == Endian.Big)
                data.Swap();
            base.Write(data, 0, data.Length); 
        }
        public void WriteStruct<T>(T structure)
        {
            int rawsize = Marshal.SizeOf(typeof(T));
            byte[] rawdata = new byte[rawsize];
            GCHandle handle = GCHandle.Alloc(rawdata, GCHandleType.Pinned);
            Marshal.StructureToPtr(typeof(T), handle.AddrOfPinnedObject(), false);
            handle.Free();
            if (BYTE_ORDER == Endian.Big)
                Array.Reverse(rawdata);
            Write(rawdata);
        }
#endregion
    }
}
