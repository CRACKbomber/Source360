using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Source360.IO;
using System.IO;
using Source360.Common.Helpers;
namespace Source360.xZip.XZP2
{
    public class CentralDirectoryEntry : ISerializable
    {
        #region Private Members
        private uint m_dwSignature = 0x2014B50;
        private ushort m_wVersionMadeBy = 0x14;
        private ushort m_wVersionExtract = 0x0A;
        private ushort m_wGeneralBitFlag = 0x00;
        private ushort m_wCompressionMethod = 0x00;
        private ushort m_wLastModTime = 0x00;
        private ushort m_wLastModDate = 0x00;
        private uint m_dwCRC32;
        private uint m_dwCompressedSize;
        private uint m_dwUnCompressedSize;
        private ushort m_wFilenameLen;
        private ushort m_wExtraFieldLen;
        private ushort m_wCommentLen = 0x00;
        private ushort m_wDiskNumber = 0x00;
        private ushort m_wInternalArrtib = 0x00;
        private uint m_wExternalArrtib = 0x00;
        private uint m_dwOffsetLocalFile;
        private string m_sFileName;
        private byte[] m_fileBuffer;
        #endregion
        #region Properties
        public uint CRC32 { get { return this.m_dwCRC32; } }
        public uint FileSize { get { return m_dwUnCompressedSize; } set { m_dwUnCompressedSize = m_dwCompressedSize = value; } }
        public string FileName { get { return m_sFileName; } set { m_sFileName = value; m_wFilenameLen = (ushort)value.Length; } }
        public uint OffsetOfLocalFile { get { return m_dwOffsetLocalFile; } set { m_dwOffsetLocalFile = value; } }
        public ushort ExtraFieldLength { get { return m_wExtraFieldLen; } set { m_wExtraFieldLen = value; } }
        public byte[] FileData { get { return this.m_fileBuffer; } set { m_fileBuffer = value; this.m_dwCRC32 = m_fileBuffer.CRC32(); FileSize = (uint)value.Length; } }
        #endregion
        public CentralDirectoryEntry() { }
        public CentralDirectoryEntry(IOReader reader) { UnSerialize(reader); }
        public bool UnSerialize(IOReader reader)
        {

            try
            {
                if (reader.ReadUInt() != m_dwSignature)
                    return false;
                this.m_wVersionMadeBy = reader.ReadUShort();
                this.m_wVersionMadeBy = reader.ReadUShort();
                this.m_wGeneralBitFlag = reader.ReadUShort();
                this.m_wCompressionMethod = reader.ReadUShort();
                this.m_wLastModTime = reader.ReadUShort();
                this.m_wLastModDate = reader.ReadUShort();
                this.m_dwCRC32 = reader.ReadUInt();
                this.m_dwCompressedSize = reader.ReadUInt();
                this.m_dwUnCompressedSize = reader.ReadUInt();
                this.m_wFilenameLen = reader.ReadUShort();
                this.m_wExtraFieldLen = reader.ReadUShort();
                this.m_wCommentLen = reader.ReadUShort();
                this.m_wDiskNumber = reader.ReadUShort();
                this.m_wInternalArrtib = reader.ReadUShort();
                this.m_wExternalArrtib = reader.ReadUInt();
                this.m_dwOffsetLocalFile = reader.ReadUInt();
                this.m_sFileName = ASCIIEncoding.ASCII.GetString(reader.ReadBytes(this.m_wFilenameLen));
                return true;
            }
            catch { return false; }
        
        }
        public byte[] Serialize()
        {
            using (MemoryStream fileStream = new MemoryStream(46))
            {
                fileStream.Write(this.m_dwSignature);
                fileStream.Write(this.m_wVersionMadeBy);
                fileStream.Write(this.m_wVersionExtract);
                fileStream.Write(this.m_wGeneralBitFlag);
                fileStream.Write(this.m_wCompressionMethod);
                fileStream.Write(this.m_wLastModDate);
                fileStream.Write(this.m_wLastModTime);
                fileStream.Write(this.m_dwCRC32);
                fileStream.Write(this.m_dwCompressedSize);
                fileStream.Write(this.m_dwUnCompressedSize);
                fileStream.Write(this.m_wFilenameLen);
                fileStream.Write(this.m_wExtraFieldLen);
                fileStream.Write(this.m_wCommentLen);
                fileStream.Write(this.m_wDiskNumber);
                fileStream.Write(this.m_wInternalArrtib);
                fileStream.Write(this.m_wExternalArrtib);
                fileStream.Write(this.m_dwOffsetLocalFile);
                fileStream.Write(ASCIIEncoding.ASCII.GetBytes(this.m_sFileName));
                return fileStream.ToArray();
            }
        }
        public bool HasFileData() { return this.m_fileBuffer != null; }
        
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
