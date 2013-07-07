using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Source360.Common.Helpers;
namespace Source360.IO
{
    public class IOWriter : BinaryWriter
    {
        #region Private Members
        private Endian BYTE_ORDER;
        #endregion
        #region Properties
        /// <summary>
        /// Byte order of the file
        /// </summary>
        public Endian ByteOrder { get { return BYTE_ORDER; } set { BYTE_ORDER = value; } }
        #endregion
        #region Constructors
        /// <summary>
        /// IOWriter, defaults Little Endian
        /// </summary>
        /// <param name="input">input stream</param>
        public IOWriter(Stream input)
            : this(input, Endian.Little) { }
        /// <summary>
        /// IOWriter
        /// </summary>
        /// <param name="input">input stream</param>
        /// <param name="byteOrder">File Byte Order</param>
        public IOWriter(Stream input, Endian byteOrder)
            : base(input) { this.BYTE_ORDER = byteOrder; }
        #endregion
        #region Writing Methods
        /// <summary>
        /// Write an unsigned integer
        /// </summary>
        /// <param name="value">integer to write</param>
        public override void Write(uint value)
        {
            if (this.BYTE_ORDER == Endian.Big)
                base.Write(value.Swap());
            else
                base.Write(value);
        }
        /// <summary>
        /// Write a signed integer
        /// </summary>
        /// <param name="value">integer to write</param>
        public override void Write(int value)
        {
            if (this.BYTE_ORDER == Endian.Big)
                base.Write(value.Swap());
            else
                base.Write(value);
        }
        /// <summary>
        /// Writes an unsigned short
        /// </summary>
        /// <param name="value">short to write</param>
        public override void Write(ushort value)
        {
            if (this.BYTE_ORDER == Endian.Big)
                base.Write(value.Swap());
            else
                base.Write(value);
        }
        /// <summary>
        /// Writes a signed short
        /// </summary>
        /// <param name="value">short to write</param>
        public override void Write(short value)
        {
            if (this.BYTE_ORDER == Endian.Big)
                base.Write(value.Swap());
            else
                base.Write(value);
        }
        /// <summary>
        /// Writes a signed long
        /// </summary>
        /// <param name="value">long to write</param>
        public void Write(long value)
        {
            if (this.BYTE_ORDER == Endian.Big)
                base.Write(value.Swap());
            else
                base.Write(value);
        }
        /// <summary>
        /// Writes an unsigned long value
        /// </summary>
        /// <param name="value">long to write</param>
        public override void Write(ulong value)
        {
            if (this.BYTE_ORDER == Endian.Big)
                base.Write(value.Swap());
            else
                base.Write(value);
        }
        /// <summary>
        /// Writes a byte
        /// </summary>
        /// <param name="value">byte to write</param>
        public void Write(byte value)
        {
            base.Write(value);
        }
        /// <summary>
        /// Writes a signed byte
        /// </summary>
        /// <param name="value">byte to write</param>
        public override void Write(sbyte value)
        {
            base.Write(value);
        }
        /// <summary>
        /// Writes a boolean
        /// </summary>
        /// <param name="value">boolean to write</param>
        public void Write(bool value)
        {
            base.Write(value);
        }
        /// <summary>
        /// Writes an array of bytes
        /// </summary>
        /// <param name="value">byte array to write</param>
        public void Write(byte[] value)
        {
            base.Write(value);
        }
        /// <summary>
        /// Writes a char
        /// </summary>
        /// <param name="value">char to write</param>
        public void Write(char value)
        {
            base.Write(value);
        }
        /// <summary>
        /// Writes an array of chars
        /// </summary>
        /// <param name="value">char array to write</param>
        public void Write(char[] value)
        {
            base.Write(value);
        }
        /// <summary>
        /// Writes a Single(float) 
        /// </summary>
        /// <param name="value">Single(float) to write</param>
        public void Write(Single value)
        {
            if (this.BYTE_ORDER == Endian.Big)
                base.Write(value.Swap());
            else
                base.Write(value);
        }
        /// <summary>
        /// Writes an unmanaged data type (Structs)
        /// </summary>
        /// <typeparam name="T">type of data</typeparam>
        /// <param name="value">object of type</param>
        public void WriteStructure<T>(T value)
            where T : struct
        {
            int rawsize = Marshal.SizeOf(typeof(T));
            byte[] rawdata = new byte[rawsize];
            GCHandle handle = GCHandle.Alloc(rawdata, GCHandleType.Pinned);
            Marshal.StructureToPtr(typeof(T), handle.AddrOfPinnedObject(), false);
            handle.Free();
            base.Write(rawdata);
        }
        #endregion
        /// <summary>
        /// Closes the current stream
        /// </summary>
        public void Close()
        {
            base.Flush();
            base.Close();
        }
        /// <summary>
        /// Flushes the current stream
        /// </summary>
        public void Flush()
        {
            base.Flush();
        }
        /// <summary>
        /// Disposes of the IOWriter object and it's hierarchy 
        /// </summary>
        public void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
