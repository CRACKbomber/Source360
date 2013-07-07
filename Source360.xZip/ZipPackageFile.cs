using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Source360.IO;
using Source360.Common.Helpers;
using Source360.Common;
using Source360.xZip.XZP2;
using System.Threading;
using System.Diagnostics;
namespace Source360.xZip
{
    public class ZipPackageFile
    {
        #region Members
        private List<ZipFile> m_lZipDirectory;
        private IOReader m_zipReader;
        private PreloadSection m_preloadSection;
        private GameNames m_gameName;
        #endregion
        #region Properties
        public List<ZipFile> ZipDirectory { get { return this.m_lZipDirectory; } }
        public GameNames GameName { get { return this.m_gameName; } }
        #endregion
        #region Constructors
        /// <summary>
        /// ZipPackageFile constructor
        /// </summary>
        /// <param name="buffer">byte buffer with zip file</param>
        public ZipPackageFile(byte[] buffer)
            : this(new MemoryStream(buffer)) { }
        /// <summary>
        /// ZipPackageFile constructor
        /// </summary>
        /// <param name="file">location of zip on disc</param>
        public ZipPackageFile(string file)
            : this(File.OpenRead(file)) { }
        /// <summary>
        /// ZipPackageFile constructor
        /// </summary>
        /// <param name="input">stream with zip file</param>
        public ZipPackageFile(Stream input) { m_zipReader = new IOReader(input); }
        /// <summary>
        /// ZipPackageFile constructor for creating zips
        /// </summary>
        /// <param name="dir">directory to construct from</param>
        /// <param name="output">output to save to</param>
        public ZipPackageFile(string dir, string output) { CreateZip(dir, output); }
        #endregion
        #region File Operations
        /// <summary>
        /// Extracts all files from the zip
        /// </summary>
        /// <param name="folder">output folder</param>
        /// <returns>operation outcome</returns>
        public bool ExtractAllFiles(string folder)
        {
            try
            {
                foreach (ZipFile entry in this.m_lZipDirectory)
                {
                    this.m_zipReader.Seek(entry.Offset, SeekOrigin.Begin);
                    byte[] buffer = this.m_zipReader.ReadBytes((int)entry.FileSize);
                    string path = folder + @"\" + entry.FileName.Substring(0, entry.FileName.LastIndexOfAny(new char[] { '\\', '/' }) + 1);
                    string fileName = Path.GetFileName(entry.FileName);
                    Console.WriteLine("Extracting {0} to /output {1} bytes", entry.FileName, entry.FileSize);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using(FileStream output = new FileStream(path + fileName, FileMode.Create))
                    using (IOWriter writer = new IOWriter(output))
                    {
                        writer.Write(buffer);
                    }
                }
                return true;
            }
            catch { return false; }
        }
        /// <summary>
        /// Extracts a file
        /// </summary>
        /// <param name="index">index of file</param>
        /// <returns>buffer containing file data</returns>
        public byte[] ExtractFile(int index)
        {
            this.m_zipReader.Seek(this.m_lZipDirectory[index].Offset, SeekOrigin.Begin);
            return this.m_zipReader.ReadBytes(this.m_lZipDirectory[index].FileSize);
        }
        /// <summary>
        /// Extracts a file
        /// </summary>
        /// <param name="index">index in zip</param>
        /// <param name="fileName">save file name</param>
        public void ExtractFile(int index, string fileName)
        {
            this.m_zipReader.Seek(this.m_lZipDirectory[index].Offset, SeekOrigin.Begin);
            using (IOWriter writer = new IOWriter(File.Create(fileName)))
                writer.Write(this.m_zipReader.ReadBytes(this.m_lZipDirectory[index].FileSize));
        }
        /// <summary>
        /// Adds a file to the zip
        /// </summary>
        /// <param name="fileName">input file name</param>
        public void AddFile(string fileName)
        {
            using (IOReader reader = new IOReader(File.OpenRead(fileName)))
            {
                byte[] fileBuf = reader.ReadAllBytes();
                CentralDirectoryEntry entry = new CentralDirectoryEntry();
                entry.FileName = FixupPath(fileName);
                entry.FileSize = (uint)fileBuf.Length;
                entry.FileData = fileBuf;
                ZipFile file = new ZipFile(entry, this.m_lZipDirectory.Count + 1);
                this.m_lZipDirectory.Add(file);
                this.m_lZipDirectory.Sort();
            }
        }
        /// <summary>
        /// Deletes a file at the specified index
        /// </summary>
        /// <param name="index">index of file</param>
        public void DeleteFile(int index) { this.m_lZipDirectory.Remove(this.m_lZipDirectory[index]); }
        /// <summary>
        /// Updates a file's data
        /// </summary>
        /// <param name="index">index of original file</param>
        /// <param name="fileName">name of file on disc</param>
        public void UpdateFile(int index, string fileName)
        {
            using (IOReader reader = new IOReader(File.OpenRead(fileName)))
                UpdateFile(index,reader.ReadAllBytes());
        }
        /// <summary>
        /// Updates a file's data
        /// </summary>
        /// <param name="index">index of original file</param>
        /// <param name="buffer">buffer of new data</param>
        public void UpdateFile(int index, byte[] buffer)
        {
            this.m_lZipDirectory[index].ChildEntry.FileData = buffer;
        }
        /// <summary>
        /// Renames a file
        /// </summary>
        /// <param name="index">index in zip</param>
        /// <param name="name">new file name</param>
        public void RenameFile(int index, string name) { this.m_lZipDirectory[index].FileName = name; }
        #endregion
        #region Misc Operations
        /// <summary>
        /// Returns file data
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public byte[] GetFileData(int index)
        {
            ZipFile file = this.m_lZipDirectory[index];
            if (file.ChildEntry.HasFileData())
                return file.ChildEntry.FileData;
            this.m_zipReader.Seek(file.Offset, SeekOrigin.Begin);
            return this.m_zipReader.ReadBytes(file.FileSize);
        }
        /// <summary>
        /// Returns the index of a zip file from a file name
        /// </summary>
        /// <param name="FileName">input file name</param>
        /// <returns>index of file</returns>
        public int GetIndex(string FileName) { return this.m_lZipDirectory.FindIndex(file => file.FileName == FileName); }
        /// <summary>
        /// Unserialize zip 
        /// </summary>
        /// <param name="input">input stream</param>
        /// <returns>operation outcome</returns>
        public bool UnSerialize()
        {
            try
            {
                /// Start at the beginning of the EOF
                this.m_zipReader.Seek(-0x36, SeekOrigin.End);
                /// Get central directory info from the eof header
                CentralDirectoryRecordEOF eof = new CentralDirectoryRecordEOF();
                eof.UnSerialize(this.m_zipReader);
                this.m_lZipDirectory = new List<ZipFile>(eof.TotalCDirEntries);
                ///Seek to the beginning of the zip to read the preload local file header
                //this.m_zipReader.Seek(0, SeekOrigin.Begin);
                //LocalFileHeader preloadHdr = new LocalFileHeader();
                //preloadHdr.UnSerialize(this.m_zipReader);
                //m_preloadSection = new PreloadSection(new MemoryStream(preloadHdr.FileData));
                /// Read preload
                //Thread preloadWorker = new Thread(new ThreadStart(m_preloadSection.UnSerialize));
                //preloadWorker.Start();
                ///Parse Central Directory
                this.m_zipReader.Seek((int)eof.StartOfCDir, SeekOrigin.Begin);
                ZipFile file;
                CentralDirectoryEntry entry;
                for (int i = 1; i < eof.TotalCDirEntries; i++)
                {
                    entry = new CentralDirectoryEntry(this.m_zipReader);
                    file = new ZipFile(entry, i);
                    this.m_lZipDirectory.Add(file);
                }
                GetGame();
                return true;
            }
            catch {  return false; }
        }
        /// <summary>
        /// Saves the current zip file
        /// </summary>
        /// <param name="output">location to save</param>
        /// <returns>outcome of operation</returns>
        public bool Save(string output,bool getFileData)
        {
            try
            {
                CentralDirectoryRecordEOF eof = new CentralDirectoryRecordEOF();
                eof.TotalCDirEntries = (ushort)this.m_lZipDirectory.Count;

                /// Get all file data
                if (getFileData)
                    for (int i = 0; i < this.m_lZipDirectory.Count; i++)
                        this.m_lZipDirectory[i].ChildEntry.FileData = GetFileData(i);
                using (IOWriter writer = new IOWriter(File.Create(output)))
                {
                    /// Write local file directories
                    foreach (ZipFile file in this.m_lZipDirectory)
                        writer.Write(file.CreateHeader((int)writer.BaseStream.Position).Serialize());

                    /// Pad the beginning of the central directory
                    int cdirStart = (int)writer.BaseStream.Position.Align(0x800);
                    writer.Write(new byte[cdirStart - (int)writer.BaseStream.Position]);
                    eof.StartOfCDir = (uint)cdirStart;

                    /// Write Central Directory entires
                    foreach (ZipFile file in this.m_lZipDirectory)
                        writer.Write(file.ChildEntry.Serialize());

                    /// Pad out the end of the central directory
                    int cdirEnd = (int)writer.BaseStream.Position.Align(0x800);
                    writer.Write(new byte[cdirEnd - (int)writer.BaseStream.Position]);
                    eof.SizeOfCDir = (ushort)(cdirEnd - cdirStart);
                    /// Pad zip
                    if (writer.BaseStream.Length < (int)this.m_gameName)
                        writer.Write(new byte[((int)this.m_gameName - writer.BaseStream.Length) - 54]);
                    /// Write EOF
                    writer.Write(eof.Serialize());
                }

                return true;
            }
            catch { return false; }
            
        }
        /// <summary>
        /// Closes zip
        /// </summary>
        public void Close()
        {
            m_zipReader.Close();
            m_lZipDirectory.Clear();
        }
        /// <summary>
        /// Finds the game this zip belongs to
        /// </summary>
        private void GetGame()
        {
            if(GetIndex("materials\\models\\survivors\\coach\\coach_head.vmt") != -1)
                m_gameName = GameNames.Left4Dead2Z1;
            if (GetIndex("scripts\\game_sounds_mechanic.txt") != -1)
                m_gameName = GameNames.Left4Dead2Z0;
            if (GetIndex("resouce\\left4dead_dlc1_english.txt") != -1)
                m_gameName = GameNames.Left4DeadGOTYZ0;
            if (GetIndex("scripts\\soundscapes_airport.txt") != -1)
                m_gameName = GameNames.Left4DeadOrgZ0;
            if (GetIndex("materials\\decals\\logo_mercyhospital.360.vtf") != -1)
                m_gameName = GameNames.Left4DeadOrgZ1;
            if (GetIndex("scripts\\soundscapes_hydro.txt") != -1)
                m_gameName = GameNames.TeamFortress;
            if (GetIndex("scripts\\npc_sounds_barney.txt") != -1)
                m_gameName = GameNames.HalfLife2;
            if (GetIndex("scripts\\npc_sounds_rocket_turret.txt") != -1)
                m_gameName = GameNames.Portal;
            if (GetIndex("models\\ballbot_animations.360.ani") != -1)
                m_gameName = GameNames.Portal2Z0;
            if (GetIndex("maps\\sp_a1_intro1_commentary.txt") != -1)
                m_gameName = GameNames.Portal2Z1;
            if (GetIndex("bin\\gc\\gcfiles.txt") != -1)
                m_gameName = GameNames.CounterStrike;
        }
        /// <summary>
        /// Creates a zip
        /// </summary>
        /// <param name="dir">directory to construct from</param>
        /// <param name="output">output zip</param>
        public void CreateZip(string dir, string output)
        {
            string[] fileEntries = Directory.GetFiles(dir,"*.*",SearchOption.AllDirectories);
            CentralDirectoryEntry entry;
            ZipFile file;
            this.m_lZipDirectory = new List<ZipFile>(fileEntries.Length);
            foreach (string fileName in fileEntries)
            {
                using (IOReader reader = new IOReader(File.OpenRead(fileName)))
                {
                    entry = new CentralDirectoryEntry();
                    entry.FileData = reader.ReadAllBytes();
                    entry.FileName = fileName.Substring(dir.Length + 1);
                    file = new ZipFile(entry);
                    this.m_lZipDirectory.Add(file);
                }
            }
            GetGame();
            this.m_lZipDirectory.Sort();
            Save(output, false);
        }
        #endregion
        #region Private Methods
        /// <summary>
        /// Gets exact start of a file without the header and padding
        /// </summary>
        /// <param name="entry">central directory</param>
        /// <returns>offset of file data</returns>
        private int GetExactFileStart(CentralDirectoryEntry entry) { return (int)(entry.OffsetOfLocalFile + entry.ExtraFieldLength + 30 + entry.FileName.Length); }
        /// <summary>
        /// Used for fixing up added file pathes
        /// </summary>
        /// <param name="location">location of file</param>
        /// <returns>true path</returns>
        private string FixupPath(string location)
        {
            string[] folders = location.Split('\\');
            foreach (string folder in folders)
                if(IsValidFolder(folder))
                    return location.Substring(location.IndexOf(folder));
                else 
                    continue;
            FileInfo info = new FileInfo(location);
            return info.Name;
        }
        /// <summary>
        /// Is the folder a valid folder for fixing up
        /// </summary>
        /// <param name="folder">input folder</param>
        /// <returns>true if can be fixed</returns>
        private bool IsValidFolder(string folder)
        {
            return folder == "cfg" || folder == "classes" || folder == "expressions" ||
                folder == "maps" || folder == "media" || folder == "missions" || folder == "models"|| 
                folder == "particles" || folder == "reslists_xbox" || folder == "scenes" || folder == "scripts" ||
                folder == "sounds" || folder == "testscripts";
        }
        /// <summary>
        /// Creates a list with all the filenames in a dir
        /// </summary>
        /// <param name="files">List to create</param>
        /// <param name="dir">Directory that stores files</param>  
        #endregion
    }
}