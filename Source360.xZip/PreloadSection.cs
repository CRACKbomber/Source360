using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Source360.IO;
using Source360.Common.Helpers;
namespace Source360.xZip
{
    public class PreloadEntry
    {
        private int m_dwZipIndex;
        private int m_dwPreloadIndex;
        private int m_dwOffset;
        private int m_dwLength;
        public int ZipIndex { get { return this.m_dwZipIndex; } set { this.m_dwZipIndex = value; } }
        public int PreloadIndex { get { return this.m_dwPreloadIndex; } set { this.m_dwPreloadIndex = value; } }
        public int Offset { get { return this.m_dwOffset; } set { this.m_dwOffset = value; } }
        public int Length { get { return this.m_dwLength; } set { this.m_dwLength = value; } }
    }
    public class PreloadSection
    {
        private List<PreloadEntry> m_lPreloadDirectory;
        private uint m_dwVersion = 3;
        private uint m_dwDirectoryEntries;
        private uint m_dwPreloadEntries;
        private uint m_dwAlignment = 0x800;
        private int m_dwEntriesStart;
        private Stream m_input;
        public PreloadSection(Stream input) { this.m_input = input; }
        public List<PreloadEntry> PreloadDirectory { get { return this.m_lPreloadDirectory; } }
        public void UnSerialize()
        {
            try
            {
                using (IOReader reader = new IOReader(m_input, Endian.Big))
                {
                    this.m_dwVersion = reader.ReadUInt();
                    this.m_dwDirectoryEntries = reader.ReadUInt();
                    this.m_dwPreloadEntries = reader.ReadUInt();
                    this.m_dwAlignment = reader.ReadUInt();
                    this.m_lPreloadDirectory = new List<PreloadEntry>((int)this.m_dwPreloadEntries);
                    PreloadEntry entry;
                    for (int i = 0; i < this.m_dwPreloadEntries;i++ )
                    {
                        entry = new PreloadEntry();
                        entry.PreloadIndex = i;
                        entry.Length = reader.ReadInt();
                        entry.Offset = reader.ReadInt();
                        this.m_lPreloadDirectory.Add(entry);
                    }
                    int j = 0;
                    for (int i = 0; i < this.m_dwDirectoryEntries; i++)
                    {
                        ushort iPre = reader.ReadUShort();
                        if (iPre == 0xFFFF)
                            continue;
                        PreloadDirectory[j].ZipIndex = i;
                        j++;
                    }
                    this.m_dwEntriesStart = (int)reader.BaseStream.Position;
                }
            }
            catch { }
        }
    }
}
