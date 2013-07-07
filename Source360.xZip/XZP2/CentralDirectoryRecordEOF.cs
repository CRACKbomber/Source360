using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Source360.IO;
using Source360.Common.Helpers;
namespace Source360.xZip.XZP2
{
    public class CentralDirectoryRecordEOF : ISerializable
    {
        #region Private Members
        private uint m_dwSignature = 0x6054b50;
        private ushort m_wDiskNumber = 0x00;
        private ushort m_wDiskNumberCDir = 0x00;
        private ushort m_wNumCDirEntries;
        private ushort m_wTotalCDirEntries;
        private uint m_dwSizeOfCDir;
        private uint m_dwStartOfCDir;
        private ushort m_wCommentLen = 0x20;
        private string m_Comment = "XZP2 2048\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0";
        #endregion
        #region Properties
        public uint StartOfCDir { get { return m_dwStartOfCDir; } set { m_dwStartOfCDir = value; } }
        public uint SizeOfCDir { get { return m_dwSizeOfCDir; } set { m_dwSizeOfCDir = value; } }
        public ushort TotalCDirEntries { get { return m_wNumCDirEntries; } set { m_wTotalCDirEntries = m_wNumCDirEntries = value; } }
        #endregion
        public byte[] Serialize()
        {
            using (MemoryStream fileStream = new MemoryStream(54))
            {
                fileStream.Write(this.m_dwSignature);
                fileStream.Write(this.m_wDiskNumber);
                fileStream.Write(this.m_wDiskNumberCDir);
                fileStream.Write(this.m_wNumCDirEntries);
                fileStream.Write(this.m_wTotalCDirEntries);
                fileStream.Write(this.m_dwSizeOfCDir);
                fileStream.Write(this.m_dwStartOfCDir);
                fileStream.Write(this.m_wCommentLen);
                fileStream.Write(this.m_Comment);
                return fileStream.ToArray();
            }
        }
        public bool UnSerialize(IOReader reader)
        {
            try
            {
                if (reader.ReadUInt() != m_dwSignature)
                    return false;
                this.m_wDiskNumber = reader.ReadUShort();
                this.m_wDiskNumberCDir = reader.ReadUShort();
                this.m_wNumCDirEntries = reader.ReadUShort();
                this.m_wTotalCDirEntries = reader.ReadUShort();
                this.m_dwSizeOfCDir = reader.ReadUInt();
                this.m_dwStartOfCDir = reader.ReadUInt();
                this.m_wCommentLen = reader.ReadUShort();
                this.m_Comment = Encoding.ASCII.GetString(reader.ReadBytes(this.m_wCommentLen));
                return true;

            }
            catch { return false; }
        }
        public void Dispose() { GC.SuppressFinalize(this); }
    }
}
