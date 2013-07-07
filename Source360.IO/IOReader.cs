using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Source360.Common.Helpers;
using System.Runtime.InteropServices;
namespace Source360.IO
{
    public class IOReader : BinaryReader
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
        /// IOReader, defaults byte order to little and starting offset of 0
        /// </summary>
        /// <param name="input">input stream</param>
        public IOReader(Stream input)
            : this(input, Endian.Little, 0) { }
        /// <summary>
        /// IOReader, defaults starting position to 0
        /// </summary>
        /// <param name="input">input stream</param>
        /// <param name="this.BYTE_ORDER">file byte order</param>
        public IOReader(Stream input, Endian byteOrder)
            : this(input, byteOrder, 0) { }
        /// <summary>
        /// IOReader, defaults byte order to little
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startingOffset"></param>
        public IOReader(Stream input, long startingOffset)
            : this(input, Endian.Little, startingOffset) { }
        /// <summary>
        /// IOReader
        /// </summary>
        /// <param name="input">input stream</param>
        /// <param name="this.BYTE_ORDER">byte order of file</param>
        /// <param name="startingOffset">starting offset</param>
        public IOReader(Stream input, Endian byteOrder, long startingOffset)
            : base(input)
        {
            /// Make sure it's a legal position
            if (startingOffset > base.BaseStream.Length)
                throw new EndOfStreamException();
            else base.BaseStream.Position = startingOffset;
            this.BYTE_ORDER = byteOrder;
        }
        #endregion
        #region Reading Methods
        /// <summary>
        /// Seeks the stream, seek origin is set to current position by default
        /// </summary>
        /// <param name="position">position realitive to seek origin</param>
        /// <param name="origin">seek origin</param>
        public void Seek(int position, SeekOrigin origin = SeekOrigin.Current)
        {
            base.BaseStream.Seek((long)position, origin);
        }
        /// <summary>
        /// Reads a byte
        /// </summary>
        /// <returns>byte</returns>
        public override byte ReadByte()
        {
            return base.ReadByte();
        }
        /// <summary>
        /// reads a signed byte
        /// </summary>
        /// <returns>signed byte</returns>
        public override sbyte ReadSByte()
        {
            return base.ReadSByte();
        }
        /// <summary>
        /// reads a boolean
        /// </summary>
        /// <returns>boolean</returns>
        public bool ReadBool()
        {
            return base.ReadBoolean();
        }
        /// <summary>
        /// reads a unsigned 16byte integer
        /// </summary>
        /// <returns>uint16</returns>
        public ushort ReadUShort()
        {
            var value = base.ReadUInt16();
            return this.BYTE_ORDER == Endian.Big ? value.Swap() : value;
        }
        /// <summary>
        /// Reads a signed 16byte integer
        /// </summary>
        /// <returns>int16</returns>
        public short ReadShort()
        {
            var value = base.ReadInt16();
            return this.BYTE_ORDER == Endian.Big ? value.Swap() : value;
        }
        /// <summary>
        /// reads a signed 32byte integer
        /// </summary>
        /// <returns>int32</returns>
        public int ReadInt()
        {
            var value = base.ReadInt32();
            return this.BYTE_ORDER == Endian.Big ? value.Swap() : value;
        }
        /// <summary>
        /// reads a unsigned 32byte integer
        /// </summary>
        /// <returns>uint32</returns>
        public uint ReadUInt()
        {
            var value = base.ReadUInt32();
            return this.BYTE_ORDER == Endian.Big ? value.Swap() : value;
        }
        /// <summary>
        /// reads a char
        /// </summary>
        /// <returns>char</returns>
        public char ReadChar()
        {
            return base.ReadChar();
        }
        /// <summary>
        /// reads a signed 64byte integer
        /// </summary>
        /// <returns>int64</returns>
        public long ReadLong()
        {
            var value = base.ReadInt64();
            return this.BYTE_ORDER == Endian.Big ? value.Swap() : value;
        }
        /// <summary>
        /// reads a unsigned 64byte integer
        /// </summary>
        /// <returns>uint64</returns>
        public ulong ReadULong()
        {
            var value = base.ReadUInt64();
            return this.BYTE_ORDER == Endian.Big ? value.Swap() : value;
        }
        /// <summary>
        /// reads a float
        /// </summary>
        /// <returns>float</returns>
        public float ReadFloat()
        {
            var value = base.ReadSingle();
            return this.BYTE_ORDER == Endian.Big ? value.Swap() : value;
        }
        /// <summary>
        /// reads an array of bytes
        /// </summary>
        /// <param name="numBytes">number of bytes to read</param>
        /// <returns>byte array</returns>
        public byte[] ReadBytes(int numBytes)
        {
            byte[] value = base.ReadBytes(numBytes);
            if (this.BYTE_ORDER == Endian.Big)
                Array.Reverse(value);
            return value;
        }
        /// <summary>
        /// reads an array of chars
        /// </summary>
        /// <param name="numChars">number of chars to read</param>
        /// <returns>char array</returns>
        public char[] ReadChars(int numChars)
        {
            var chars = base.ReadChars(numChars);
            if (this.BYTE_ORDER == Endian.Big)
                Array.Reverse(chars);
            return chars;
        }
        /// <summary>
        /// Reads all bytes from a file
        /// </summary>
        /// <returns>byte array containing the file</returns>
        public byte[] ReadAllBytes()
        {
            return base.ReadBytes((int)base.BaseStream.Length);
        }
        /// <summary>
        /// Reads an unmanaged structure
        /// </summary>
        /// <typeparam name="T">structure type</typeparam>
        /// <returns>managed structure</returns>
        public T ReadStructure<T>()
            where T : struct
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
            base.Read(buffer, 0, Marshal.SizeOf(typeof(T)));
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            T temp = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return temp;
        }
        #endregion
    }
}
