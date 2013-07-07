using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Source360.Common.Helpers;
using Source360.IO;
namespace Source360.xZip.XZP2
{
    public class LocalFileHeader : ISerializable, IComparable<LocalFileHeader>
    {
        private uint m_dwSignature = 0x4034B50;
        private ushort m_wVersionExtract = 0x0A;
        private ushort m_wBitFlag = 0x00;
        private ushort m_wCompressionMethod = 0x00;
        private ushort m_wModTime = 0x00;
        private ushort m_wModDate = 0x00;
        private uint m_dwCRC32;
        private uint m_dwCompressedSize;
        private uint m_dwUnCompressedSize;
        private ushort m_wFileNameLen;
        private ushort m_wExtraFieldLen;
        private string m_sFileName;
        private byte[] m_fileData;
        public uint CRC32 { get { return this.m_dwCRC32; } set { this.m_dwCRC32 = value; } }
        public string FileName { get { return this.m_sFileName; } set { this.m_sFileName = value; this.m_wFileNameLen = (ushort)value.Length; } }
        public ushort ExtraFieldLen { get { return this.m_wExtraFieldLen; } set { this.m_wExtraFieldLen = value; } }
        public byte[] FileData { get { return this.m_fileData; } set { this.m_fileData = value; this.m_dwUnCompressedSize = this.m_dwCompressedSize = (ushort)value.Length; } }
        public int CompareTo(LocalFileHeader entry) { return string.Compare(this.m_sFileName, entry.FileName); }
        public byte[] Serialize()
        {
            using (MemoryStream fileStream = new MemoryStream(46))
            {
                fileStream.Write(this.m_dwSignature);
                fileStream.Write(this.m_wVersionExtract);
                fileStream.Write(this.m_wBitFlag);
                fileStream.Write(this.m_wCompressionMethod);
                fileStream.Write(this.m_wModTime);
                fileStream.Write(this.m_wModDate);
                fileStream.Write(this.m_dwCRC32);
                fileStream.Write(this.m_dwCompressedSize);
                fileStream.Write(this.m_dwUnCompressedSize);
                fileStream.Write(this.m_wFileNameLen);
                fileStream.Write(this.m_wExtraFieldLen);
                fileStream.Write(this.m_sFileName);
                fileStream.Write(new byte[this.m_wExtraFieldLen]);
                fileStream.Write(this.m_fileData);
                return fileStream.ToArray();
            }
        }
        public bool UnSerialize(IOReader reader)
        {
            try
            {
                if (reader.ReadUInt() != this.m_dwSignature)
                    return false;
                this.m_wVersionExtract = reader.ReadUShort();
                this.m_wBitFlag = reader.ReadUShort();
                this.m_wCompressionMethod = reader.ReadUShort();
                this.m_wModTime = reader.ReadUShort();
                this.m_wModDate = reader.ReadUShort();
                this.m_dwCRC32 = reader.ReadUInt();
                this.m_dwCompressedSize = reader.ReadUInt();
                this.m_dwUnCompressedSize = reader.ReadUInt();
                this.m_wFileNameLen = reader.ReadUShort();
                this.m_wExtraFieldLen = reader.ReadUShort();
                this.m_sFileName = Encoding.ASCII.GetString(reader.ReadBytes(this.m_wFileNameLen));
                reader.Seek(this.m_wExtraFieldLen);
                this.m_fileData = reader.ReadBytes((int)this.m_dwUnCompressedSize);
                return true;

            }
            catch { return false; }
        }
        public void Dispose() { GC.SuppressFinalize(this); }
        public LocalFileHeader() { }
        /// <summary>
        /// Allows for conversion of CentralDirectory
        /// </summary>
        /// <param name="entry">cdir entry</param>
        public LocalFileHeader(CentralDirectoryEntry entry)
        {
            this.m_dwCRC32 = entry.CRC32;
            this.m_dwUnCompressedSize = this.m_dwCompressedSize = entry.FileSize;
            this.m_wFileNameLen = (ushort)entry.FileName.Length;
            this.m_sFileName = entry.FileName;
            this.m_wExtraFieldLen = entry.ExtraFieldLength;
            this.m_fileData = entry.FileData;
        }
    }
}
