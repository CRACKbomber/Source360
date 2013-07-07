using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Source360.IO;
namespace Source360.xZip.XZP2
{
    public class ZipFile : IComparable<ZipFile>
    {
        /// <summary>
        /// Name of the File
        /// </summary>
        public string FileName { get { return ChildEntry.FileName; } set { ChildEntry.FileName = value; } }
        /// <summary>
        /// Offset of file data
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// Length of the file in bytes
        /// </summary>
        public int FileSize { get { return (int)ChildEntry.FileSize; } set { ChildEntry.FileSize = (uint)value; } }
        /// <summary>
        /// Index of the zip file in the directory listing
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Is the file preloaded 
        /// </summary>
        public bool Preloaded { get; set; }
        /// <summary>
        /// Child central directory entry
        /// </summary>
        public CentralDirectoryEntry ChildEntry { get; set; }
        /// <summary>
        /// Child preload directory entry
        /// </summary>
        public PreloadEntry ChildPreloadEntry { get; set; }
        /// <summary>
        /// Constructor for ZipFile
        /// </summary>
        /// <param name="entry">cdir entry</param>
        /// <param name="index">zip index</param>
        public ZipFile(CentralDirectoryEntry entry, int index = -1) 
        {
            ChildEntry = entry;
            ///Compensate for the local file header and extra field
            Offset = (int)(entry.OffsetOfLocalFile + 30 + entry.ExtraFieldLength + entry.FileName.Length);
            Index = index; 
        }
        /// <summary>
        /// Parse preload directory entry
        /// </summary>
        /// <param name="entry">preload entry</param>
        public void ParsePreloadInfo(PreloadEntry entry)
        {
            if (entry.ZipIndex == Index) { Preloaded = true; ChildPreloadEntry = entry; }
            else
                return;
        }
        public LocalFileHeader CreateHeader(int offset) 
        {
            int headerSize = 30 + this.FileName.Length;
            this.ChildEntry.OffsetOfLocalFile = (uint)offset;
            this.ChildEntry.ExtraFieldLength = (ushort)(0x800 - ((offset + headerSize) % 0x800));
            return new LocalFileHeader(this.ChildEntry); 
        }
        public int CompareTo(ZipFile file) { return string.Compare(this.FileName, file.FileName); }
    }
}
