///Warning!!!
///This project was created purely for educational purposes.
///In it's current state the functions work, but really don't do anything important.
///I still need to figure out how the directory system works.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Source360.IO;
using Source360.Common.Helpers;
namespace Source360.VTF
{
    public class VTFFile
    {
        /// <summary>
        /// VTF stuff taken from the SDK
        /// </summary>
        #region  VTF SDK
        private static int VTF_RSRC_MAX_DICTIONARY_ENTRIES = 32;
        public struct Vector
        {
            private float m_flVecX;
            private float m_flVecY;
            private float m_flVecZ;
            public float VecX { get { return this.m_flVecX; } }
            public float VecY { get { return this.m_flVecY; } }
            public float VecZ { get { return this.m_flVecZ; } }
            public void UnSerialize(IOReader reader)
            {
                this.m_flVecX = reader.ReadFloat();
                this.m_flVecY = reader.ReadFloat();
                this.m_flVecZ = reader.ReadFloat();
            }
            public byte[] Serialize(Endian endian)
            {
                byte[] x = BitConverter.GetBytes(this.m_flVecX);
                byte[] y = BitConverter.GetBytes(this.m_flVecY);
                byte[] z = BitConverter.GetBytes(this.m_flVecZ);
                xMemoryStream memStream = new xMemoryStream(endian);
                memStream.Write(x);
                memStream.Write(y);
                memStream.Write(z);
                return memStream.ToArray();
            }
        }
        public enum VTFImageFormat
        {
            IMAGE_FORMAT_RGBA8888 = 0,				//!<  = Red, Green, Blue, Alpha - 32 bpp
            IMAGE_FORMAT_ABGR8888,					//!<  = Alpha, Blue, Green, Red - 32 bpp
            IMAGE_FORMAT_RGB888,					//!<  = Red, Green, Blue - 24 bpp
            IMAGE_FORMAT_BGR888,					//!<  = Blue, Green, Red - 24 bpp
            IMAGE_FORMAT_RGB565,					//!<  = Red, Green, Blue - 16 bpp
            IMAGE_FORMAT_I8,						//!<  = Luminance - 8 bpp
            IMAGE_FORMAT_IA88,						//!<  = Luminance, Alpha - 16 bpp
            IMAGE_FORMAT_P8,						//!<  = Paletted - 8 bpp
            IMAGE_FORMAT_A8,						//!<  = Alpha- 8 bpp
            IMAGE_FORMAT_RGB888_BLUESCREEN,			//!<  = Red, Green, Blue, "BlueScreen" Alpha - 24 bpp
            IMAGE_FORMAT_BGR888_BLUESCREEN,			//!<  = Red, Green, Blue, "BlueScreen" Alpha - 24 bpp
            IMAGE_FORMAT_ARGB8888,					//!<  = Alpha, Red, Green, Blue - 32 bpp
            IMAGE_FORMAT_BGRA8888,					//!<  = Blue, Green, Red, Alpha - 32 bpp
            IMAGE_FORMAT_DXT1,						//!<  = DXT1 compressed format - 4 bpp
            IMAGE_FORMAT_DXT3,						//!<  = DXT3 compressed format - 8 bpp
            IMAGE_FORMAT_DXT5,						//!<  = DXT5 compressed format - 8 bpp
            IMAGE_FORMAT_BGRX8888,					//!<  = Blue, Green, Red, Unused - 32 bpp
            IMAGE_FORMAT_BGR565,					//!<  = Blue, Green, Red - 16 bpp
            IMAGE_FORMAT_BGRX5551,					//!<  = Blue, Green, Red, Unused - 16 bpp
            IMAGE_FORMAT_BGRA4444,					//!<  = Red, Green, Blue, Alpha - 16 bpp
            IMAGE_FORMAT_DXT1_ONEBITALPHA,			//!<  = DXT1 compressed format with 1-bit alpha - 4 bpp
            IMAGE_FORMAT_BGRA5551,					//!<  = Blue, Green, Red, Alpha - 16 bpp
            IMAGE_FORMAT_UV88,						//!<  = 2 channel format for DuDv/Normal maps - 16 bpp
            IMAGE_FORMAT_UVWQ8888,					//!<  = 4 channel format for DuDv/Normal maps - 32 bpp
            IMAGE_FORMAT_RGBA16161616F,				//!<  = Red, Green, Blue, Alpha - 64 bpp
            IMAGE_FORMAT_RGBA16161616,				//!<  = Red, Green, Blue, Alpha signed with mantissa - 64 bpp
            IMAGE_FORMAT_UVLX8888,					//!<  = 4 channel format for DuDv/Normal maps - 32 bpp
            IMAGE_FORMAT_R32F,						//!<  = Luminance - 32 bpp
            IMAGE_FORMAT_RGB323232F,				//!<  = Red, Green, Blue - 96 bpp
            IMAGE_FORMAT_RGBA32323232F,				//!<  = Red, Green, Blue, Alpha - 128 bpp
            IMAGE_FORMAT_NV_DST16,
            IMAGE_FORMAT_NV_DST24,
            IMAGE_FORMAT_NV_INTZ,
            IMAGE_FORMAT_NV_RAWZ,
            IMAGE_FORMAT_ATI_DST16,
            IMAGE_FORMAT_ATI_DST24,
            IMAGE_FORMAT_NV_NULL,
            IMAGE_FORMAT_ATI2N,
            IMAGE_FORMAT_ATI1N,
            //XBox:
            IMAGE_FORMAT_X360_DST16,
            IMAGE_FORMAT_X360_DST24,
            IMAGE_FORMAT_X360_DST24F,
            IMAGE_FORMAT_LINEAR_BGRX8888,			//!<  = Blue, Green, Red, Unused - 32 bpp		
            IMAGE_FORMAT_LINEAR_RGBA8888,			//!<  = Red, Green, Blue, Alpha - 32 bpp
            IMAGE_FORMAT_LINEAR_ABGR8888,			//!<  = Alpha, Blue, Green, Red - 32 bpp
            IMAGE_FORMAT_LINEAR_ARGB8888,			//!<  = Alpha, Red, Green, Blue - 32 bpp
            IMAGE_FORMAT_LINEAR_BGRA8888,			//!<  = Blue, Green, Red, Alpha - 32 bpp
            IMAGE_FORMAT_LINEAR_RGB888,				//!<  = Red, Green, Blue - 24 bpp
            IMAGE_FORMAT_LINEAR_BGR888,				//!<  = Blue, Green, Red - 24 bpp
            IMAGE_FORMAT_LINEAR_BGRX5551,			//!<  = Blue, Green, Red, Unused - 16 bpp
            IMAGE_FORMAT_LINEAR_I8,					//!<  = Luminance - 8 bpp
            IMAGE_FORMAT_LINEAR_RGBA16161616,		//!<  = Red, Green, Blue, Alpha signed with mantissa - 64 bpp
            IMAGE_FORMAT_LE_BGRX8888,				//!<  = Blue, Green, Red, Unused - 32 bpp
            IMAGE_FORMAT_LE_BGRA8888,				//!<  = Blue, Green, Red, Alpha - 32 bpp
            IMAGE_FORMAT_COUNT,
            IMAGE_FORMAT_NONE = -1
        }
        [Flags]
        public enum VTFFlags
        {
            // flags from the *.txt config file
            TEXTUREFLAGS_POINTSAMPLE = 0x00000001,
            TEXTUREFLAGS_TRILINEAR = 0x00000002,
            TEXTUREFLAGS_CLAMPS = 0x00000004,
            TEXTUREFLAGS_CLAMPT = 0x00000008,
            TEXTUREFLAGS_ANISOTROPIC = 0x00000010,
            TEXTUREFLAGS_HINT_DXT5 = 0x00000020,
            TEXTUREFLAGS_SRGB = 0x00000040,
            TEXTUREFLAGS_NORMAL = 0x00000080,
            TEXTUREFLAGS_NOMIP = 0x00000100,
            TEXTUREFLAGS_NOLOD = 0x00000200,
            TEXTUREFLAGS_ALL_MIPS = 0x00000400,
            TEXTUREFLAGS_PROCEDURAL = 0x00000800,

            // These are automatically generated by vtex from the texture data.
            TEXTUREFLAGS_ONEBITALPHA = 0x00001000,
            TEXTUREFLAGS_EIGHTBITALPHA = 0x00002000,

            // newer flags from the *.txt config file
            TEXTUREFLAGS_ENVMAP = 0x00004000,
            TEXTUREFLAGS_RENDERTARGET = 0x00008000,
            TEXTUREFLAGS_DEPTHRENDERTARGET = 0x00010000,
            TEXTUREFLAGS_NODEBUGOVERRIDE = 0x00020000,
            TEXTUREFLAGS_SINGLECOPY = 0x00040000,

            TEXTUREFLAGS_UNUSED_00080000 = 0x00080000,
            TEXTUREFLAGS_UNUSED_00100000 = 0x00100000,
            TEXTUREFLAGS_UNUSED_00200000 = 0x00200000,
            TEXTUREFLAGS_UNUSED_00400000 = 0x00400000,

            TEXTUREFLAGS_NODEPTHBUFFER = 0x00800000,

            TEXTUREFLAGS_UNUSED_01000000 = 0x01000000,

            TEXTUREFLAGS_CLAMPU = 0x02000000,

            TEXTUREFLAGS_VERTEXTEXTURE = 0x04000000,					// Useable as a vertex texture

            TEXTUREFLAGS_SSBUMP = 0x08000000,

            TEXTUREFLAGS_UNUSED_10000000 = 0x10000000,

            // Clamp to border color on all texture coordinates
            TEXTUREFLAGS_BORDER = 0x20000000,

            TEXTUREFLAGS_UNUSED_40000000 = 0x40000000,
            VERSIONED_VTF_FLAGS_MASK_7_3 = 780663807,
            //TEXTUREFLAGS_UNUSED_80000000 = 0x80000000,
        };
        private class VTFFileHeader
        {
            public uint TypeIdent;
            public uint VersionMajor;
            public uint VersionMinor;
            public uint HeaderSize;
            public void UnSerialize(IOReader reader)
            {
                this.TypeIdent = reader.ReadUInt();
                this.VersionMajor = reader.ReadUInt();
                this.VersionMinor = reader.ReadUInt();
                this.HeaderSize = reader.ReadUInt();
            }
        }
        private class VTFHeader_7_1
        {
            public ushort Width;
            public ushort Height;
            public VTFFlags Flags;
            public ushort NumFrames;
            public ushort StartFrame;
            public Vector Reflectivity = new Vector();
            public float BumpScale;
            public VTFImageFormat ImageFormat;
            public byte NumMipLevels;
            public VTFImageFormat LowResImgFmt;
            public byte LowResImgWidth;
            public byte LowResImgHeight;
            public void UnSerialize(IOReader reader)
            {
                this.Width = reader.ReadUShort();
                this.Height = reader.ReadUShort();
                this.Flags = (VTFFlags)reader.ReadInt();
                this.NumFrames = reader.ReadUShort();
                this.StartFrame = reader.ReadUShort();
                this.Reflectivity.UnSerialize(reader);
                this.BumpScale = reader.ReadFloat();
                this.ImageFormat = (VTFImageFormat)reader.ReadInt();
                this.NumMipLevels = reader.ReadByte();
                this.LowResImgFmt = (VTFImageFormat)reader.ReadInt();
                this.LowResImgWidth = reader.ReadByte();
                this.LowResImgHeight = reader.ReadByte();
            }
        }
        private class VTFHeader_7_2 : VTFHeader_7_1
        {
            public ushort Depth;
            public void UnSerialize(IOReader reader)
            {
                base.UnSerialize(reader);
                this.Depth = reader.ReadUShort();
            }
        }
        private class VTFHeader_7_3 : VTFHeader_7_2
        {
            public uint NumResources;
            public void UnSerialize(IOReader reader)
            {
                base.UnSerialize(reader);
                this.NumResources = reader.ReadUInt();
            }
        }
        private class VTFHeader_7_4 : VTFHeader_7_3
        {
            public void UnSerialize(IOReader reader)
            {
                base.UnSerialize(reader);
            }
        }
        private class VTFHeader_X360
        {
            public VTFFlags Flags;
            public ushort Width;
            public ushort Height;
            public ushort Depth;
            public ushort NumFrames;
            public ushort PreloadSize;
            public byte MipSkipCount;
            public byte NumResources;
            public Vector Reflectivity = new Vector();
            public float BumpScale;
            public VTFImageFormat ImageFormat;
            public byte[] LowResImgSmple = new byte[4];
            public uint CompressedSize;
            public void UnSerialize(IOReader reader)
            {
                // Make sure we are reading big endian
                reader.ByteOrder = Endian.Big;
                this.Flags = (VTFFlags)reader.ReadInt();
                this.Width = reader.ReadUShort();
                this.Height = reader.ReadUShort();
                this.Depth = reader.ReadUShort();
                this.NumFrames = reader.ReadUShort();
                this.PreloadSize = reader.ReadUShort();
                this.MipSkipCount = reader.ReadByte();
                this.NumResources = reader.ReadByte();
                this.Reflectivity.UnSerialize(reader);
                this.BumpScale = reader.ReadFloat();
                this.ImageFormat = (VTFImageFormat)reader.ReadInt();
                this.LowResImgSmple = reader.ReadBytes(4);
                this.CompressedSize = reader.ReadUInt();
            }
        }
        private class VTFHeader_PC : VTFHeader_7_4
        {
            List<ResourceEntry> Resources = new List<ResourceEntry>(VTF_RSRC_MAX_DICTIONARY_ENTRIES);
            List<ResourceData> ResourceData = new List<ResourceData>(VTF_RSRC_MAX_DICTIONARY_ENTRIES);
        }
        private enum ResourceEntryTypeFlag
        {
            RSRCF_HAS_NO_DATA_CHUNK = 33554432,
            RSRCF_MASK = -16777216,
            VTF_LEGACY_RSRC_LOW_RES_IMAGE = 1,
            VTF_LEGACY_RSRC_IMAGE = 48,
            VTF_RSRC_SHEET = 16
        }
        private enum HeaderDetails
        {
            MAX_RSRC_DICTIONARY_ENTRIES = 32,
            MAX_X360_RSRC_DICTIONARY_ENTRIES = 4,
        }
        private class ResourceEntry
        {
            public int eType;
            public byte[] ID;
            public ResourceEntryTypeFlag flags;
            public uint resData;
            public void UnSerialize(IOReader reader)
            {
                this.eType = reader.ReadInt();
                this.flags = (ResourceEntryTypeFlag)eType;
                this.ID = ASCIIEncoding.ASCII.GetBytes(eType.UnMakeID());
                this.resData =reader.ReadUInt();
            }
        }
        private class ResourceData
        {
            public int Size;
            public byte[] Data;
        }
        private struct TextureLODControlSettings
        {
            public byte ResolutionClampX;
            public byte ResolutionClampY;
            public byte ResolutionClampX_360;
            public byte ResolutionClampY_360;
        }
        private struct TextureSettingsEx_t
        {
            public enum Flags0			// flags0 byte mask
            {
                UNUSED = 0x01,
            };
            public byte m_flags0;		// a bitwise combination of Flags0
            public byte m_flags1;		// set to zero. for future expansion.
            public byte m_flags2;		// set to zero. for future expansion.
            public byte m_flags3;		// set to zero. for future expansion.
        };
        private int BYTE_POS(int byteVal, int shift)
        {
            return ((byte)byteVal << (byte)(shift * 8));
        }
        private int MakeRSRCID(int a, int b, int c)
        {
            return (BYTE_POS(a, 0) | BYTE_POS(b, 1) | BYTE_POS(c, 2));
        }
        private int MakeRSRCF(int d)
        {
            return BYTE_POS(d, 3);
        }
        private int MakeRSRCID_360(int a, int b, int c)
        {
            return (BYTE_POS(a, 3) | BYTE_POS(b, 2) | BYTE_POS(c, 1));
        }
        private int MakeRSRCF_360(int d)
        {
            return BYTE_POS(d, 0);
        }
        private const int VTF_RSRC_TEXTURE_LOD_SETTINGS = 4476748;
        private const int VTF_RSRC_TEXTURE_SETTINGS_EX = 3167060;
        private const int VTF_RSRC_TEXTURE_CRC = 4411971;
        private class VTFImageFormatInfo_t
        {
            public VTFImageFormatInfo_t(string name,int bits,int bytes,int rBits,int gBits,int bBits,int aBits,bool compressed,bool supported)
            {
                this.name = name;
                bBitsPerPixle = bits;
                bytesPerPixle = bytes;
                rBitsPerPixle = rBits;
                gBitsPerPixle = gBits;
                bBitsPerPixle = bBits;
                aBitsPerPixle = aBits;
                isCompressed = compressed;
                isSupported = supported;
            }
            public string name;
            public int bitsPerPixle;
            public int bytesPerPixle;
            public int rBitsPerPixle;
            public int gBitsPerPixle;
            public int bBitsPerPixle;
            public int aBitsPerPixle;
            public bool isCompressed;
            public bool isSupported;
        }
        VTFImageFormatInfo_t[] VTFImageFormatInfo = new VTFImageFormatInfo_t[]
        {
            new VTFImageFormatInfo_t("RGBA8888",			 32,  4,  8,  8,  8,  8, false,  true ),		// IMAGE_FORMAT_RGBA8888,
	        new VTFImageFormatInfo_t( "ABGR8888",			 32,  4,  8,  8,  8,  8, false,  true ),		// IMAGE_FORMAT_ABGR8888, 
	        new VTFImageFormatInfo_t( "RGB888",				 24,  3,  8,  8,  8,  0, false,  true ),		// IMAGE_FORMAT_RGB888,
	        new VTFImageFormatInfo_t( "BGR888",				 24,  3,  8,  8,  8,  0, false,  true ),		// IMAGE_FORMAT_BGR888,
	        new VTFImageFormatInfo_t( "RGB565",				 16,  2,  5,  6,  5,  0, false,  true ),		// IMAGE_FORMAT_RGB565, 
	        new VTFImageFormatInfo_t( "I8",					  8,  1,  0,  0,  0,  0, false,  true ),		// IMAGE_FORMAT_I8,
	        new VTFImageFormatInfo_t( "IA88",				 16,  2,  0,  0,  0,  8, false,  true ),		// IMAGE_FORMAT_IA88
	        new VTFImageFormatInfo_t( "P8",					  8,  1,  0,  0,  0,  0, false, false ),		// IMAGE_FORMAT_P8
	        new VTFImageFormatInfo_t( "A8",					  8,  1,  0,  0,  0,  8, false,  true ),		// IMAGE_FORMAT_A8
	        new VTFImageFormatInfo_t( "RGB888 Bluescreen",	 24,  3,  8,  8,  8,  0, false,  true ),		// IMAGE_FORMAT_RGB888_BLUESCREEN
	        new VTFImageFormatInfo_t( "BGR888 Bluescreen",	 24,  3,  8,  8,  8,  0, false,  true ),		// IMAGE_FORMAT_BGR888_BLUESCREEN
	        new VTFImageFormatInfo_t( "ARGB8888",			 32,  4,  8,  8,  8,  8, false,  true ),		// IMAGE_FORMAT_ARGB8888
	        new VTFImageFormatInfo_t( "BGRA8888",			 32,  4,  8,  8,  8,  8, false,  true ),		// IMAGE_FORMAT_BGRA8888
	        new VTFImageFormatInfo_t( "DXT1",				  4,  0,  0,  0,  0,  0,  true,  true ),		// IMAGE_FORMAT_DXT1
	        new VTFImageFormatInfo_t( "DXT3",				  8,  0,  0,  0,  0,  8,  true,  true ),		// IMAGE_FORMAT_DXT3
	        new VTFImageFormatInfo_t( "DXT5",				  8,  0,  0,  0,  0,  8,  true,  true ),		// IMAGE_FORMAT_DXT5
	        new VTFImageFormatInfo_t( "BGRX8888",			 32,  4,  8,  8,  8,  0, false,  true ),		// IMAGE_FORMAT_BGRX8888
	        new VTFImageFormatInfo_t( "BGR565",				 16,  2,  5,  6,  5,  0, false,  true ),		// IMAGE_FORMAT_BGR565
	        new VTFImageFormatInfo_t( "BGRX5551",			 16,  2,  5,  5,  5,  0, false,  true ),		// IMAGE_FORMAT_BGRX5551
	        new VTFImageFormatInfo_t( "BGRA4444",			 16,  2,  4,  4,  4,  4, false,  true ),		// IMAGE_FORMAT_BGRA4444
	        new VTFImageFormatInfo_t( "DXT1 One Bit Alpha",	  4,  0,  0,  0,  0,  1,  true,  true ),		// IMAGE_FORMAT_DXT1_ONEBITALPHA
	        new VTFImageFormatInfo_t( "BGRA5551",			 16,  2,  5,  5,  5,  1, false,  true ),		// IMAGE_FORMAT_BGRA5551
	        new VTFImageFormatInfo_t( "UV88",				 16,  2,  8,  8,  0,  0, false,  true ),		// IMAGE_FORMAT_UV88
	        new VTFImageFormatInfo_t( "UVWQ8888",			 32,  4,  8,  8,  8,  8, false,  true ),		// IMAGE_FORMAT_UVWQ8899
	        new VTFImageFormatInfo_t( "RGBA16161616F",	     64,  8, 16, 16, 16, 16, false,  true ),		// IMAGE_FORMAT_RGBA16161616F
	        new VTFImageFormatInfo_t( "RGBA16161616",	     64,  8, 16, 16, 16, 16, false,  true ),		// IMAGE_FORMAT_RGBA16161616
	        new VTFImageFormatInfo_t( "UVLX8888",			 32,  4,  8,  8,  8,  8, false,  true ),		// IMAGE_FORMAT_UVLX8888
	        new VTFImageFormatInfo_t( "R32F",				 32,  4, 32,  0,  0,  0, false,  true ),		// IMAGE_FORMAT_R32F
	        new VTFImageFormatInfo_t( "RGB323232F",			 96, 12, 32, 32, 32,  0, false,  true ),		// IMAGE_FORMAT_RGB323232F
	        new VTFImageFormatInfo_t( "RGBA32323232F",		128, 16, 32, 32, 32, 32, false,  true ),		// IMAGE_FORMAT_RGBA32323232F
	        new VTFImageFormatInfo_t( "nVidia DST16",		 16,  2,  0,  0,  0,  0, false,  true ),		// IMAGE_FORMAT_NV_DST16
	        new VTFImageFormatInfo_t( "nVidia DST24",		 24,  3,  0,  0,  0,  0, false,  true ),		// IMAGE_FORMAT_NV_DST24
	        new VTFImageFormatInfo_t( "nVidia INTZ",		 32,  4,  0,  0,  0,  0, false,  true ),		// IMAGE_FORMAT_NV_INTZ
	        new VTFImageFormatInfo_t( "nVidia RAWZ",		 32,  4,  0,  0,  0,  0, false,  true ),		// IMAGE_FORMAT_NV_RAWZ
	        new VTFImageFormatInfo_t( "ATI DST16",			 16,  2,  0,  0,  0,  0, false,  true ),		// IMAGE_FORMAT_ATI_DST16
	        new VTFImageFormatInfo_t( "ATI DST24",			 24,  3,  0,  0,  0,  0, false,  true ),		// IMAGE_FORMAT_ATI_DST24
	        new VTFImageFormatInfo_t( "nVidia NULL",		 32,  4,  0,  0,  0,  0, false,  true ),		// IMAGE_FORMAT_NV_NULL
	        new VTFImageFormatInfo_t( "ATI1N",				  4,  0,  0,  0,  0,  0,  true,  true ),		// IMAGE_FORMAT_ATI1N
	        new VTFImageFormatInfo_t( "ATI2N",				  8,  0,  0,  0,  0,  0,  true,  true ),		// IMAGE_FORMAT_ATI2N
	        new VTFImageFormatInfo_t( "Xbox360 DST16",		 16,  0,  0,  0,  0,  0, false,  true ),		// IMAGE_FORMAT_X360_DST16
	        new VTFImageFormatInfo_t( "Xbox360 DST24",		 24,  0,  0,  0,  0,  0, false,  true ),		// IMAGE_FORMAT_X360_DST24
	        new VTFImageFormatInfo_t( "Xbox360 DST24F",		 24,  0,  0,  0,  0,  0, false , true ),		// IMAGE_FORMAT_X360_DST24F
	        new VTFImageFormatInfo_t( "Linear BGRX8888",	 32,  4,  8,  8,  8,  0, false,  true ),		// IMAGE_FORMAT_LINEAR_BGRX8888
	        new VTFImageFormatInfo_t( "Linear RGBA8888",     32,  4,  8,  8,  8,  8, false,  true ),		// IMAGE_FORMAT_LINEAR_RGBA8888
	        new VTFImageFormatInfo_t( "Linear ABGR8888",	 32,  4,  8,  8,  8,  8, false,  true ),		// IMAGE_FORMAT_LINEAR_ABGR8888
	        new VTFImageFormatInfo_t( "Linear ARGB8888",	 32,  4,  8,  8,  8,  8, false,  true ),		// IMAGE_FORMAT_LINEAR_ARGB8888
	        new VTFImageFormatInfo_t( "Linear BGRA8888",	 32,  4,  8,  8,  8,  8, false,  true ),		// IMAGE_FORMAT_LINEAR_BGRA8888
	        new VTFImageFormatInfo_t( "Linear RGB888",		 24,  3,  8,  8,  8,  0, false,  true ),		// IMAGE_FORMAT_LINEAR_RGB888
	        new VTFImageFormatInfo_t( "Linear BGR888",		 24,  3,  8,  8,  8,  0, false,  true ),		// IMAGE_FORMAT_LINEAR_BGR888
	        new VTFImageFormatInfo_t( "Linear BGRX5551",	 16,  2,  5,  5,  5,  0, false,  true ),		// IMAGE_FORMAT_LINEAR_BGRX5551
	        new VTFImageFormatInfo_t( "Linear I8",			  8,  1,  0,  0,  0,  0, false,  true ),		// IMAGE_FORMAT_LINEAR_I8
	        new VTFImageFormatInfo_t( "Linear RGBA16161616", 64,  8, 16, 16, 16, 16, false,  true ),		// IMAGE_FORMAT_LINEAR_RGBA16161616
	        new VTFImageFormatInfo_t( "LE BGRX8888",         32,  4,  8,  8,  8,  0, false,  true ),		// IMAGE_FORMAT_LE_BGRX8888
	        new VTFImageFormatInfo_t( "LE BGRA8888",		 32,  4,  8,  8,  8,  8, false,  true ),
        };
        #endregion
        #region Private Members
        private int m_dwVersionMajor;
        private int m_dwVersionMinor;
        private int m_dwWidth;
        private int m_dwHeight;
        private int m_dwDepth;
        private VTFImageFormat m_Format;
        private int m_dwMipCount;
        private int m_dwFaceCount;
        private int m_dwFrameCount;
        private VTFFlags m_dwFlags;
        private byte[] m_ImageData;
        private Vector m_vecReflectivity;
        private float m_flBumpScale;
        // Alpha thresholds
        private float m_flAlphaThreshold;
        private float m_flAlphaHiFreqThreshold;
        // Low Res Image Stuff
        private VTFImageFormat m_LowResImageFormat;
        private int m_dwLowResImageWidth;
        private int m_dwLowResImageHeight;
        private byte[] m_LowResImageData;
        // 360 stuff
        private bool m_bIs360;
        private int m_dwPreloadDataSize;
        private int m_dwCompressedSize;
        private int m_dwMipSkipCount;
        private byte[] m_LowResImageSample;
        // Resource Stuff
        private int m_dwResourceCount;
        List<ResourceEntry> m_ResourcesInfo;
        List<byte[]> m_ResourceData;
        List<byte[]> m_ResourceData_preserved;
        // Reader and Writer
        private IOReader m_vtfReader;
        private IOWriter m_vtfWriter;

        
        #endregion
        public VTFFile(string input)
            : this(File.OpenRead(input)) { }
        public VTFFile(byte[] buffer)
            : this(new MemoryStream(buffer)) { }
        public VTFFile(Stream input) { UnSerialize(input); }
        private void UnSerialize(Stream input)
        {
            m_vtfReader = new IOReader(input);
            int ident = this.m_vtfReader.ReadInt();
            if (ident == "VTF\0".MakeID())
                this.m_bIs360 = false;
            else if (ident == "VTFX".MakeID())
            {
                this.m_bIs360 = true;
                this.m_vtfReader.ByteOrder = Endian.Big;
            }
            else
                throw new BadImageFormatException();
            // rewind stream
            m_vtfReader.Seek(-4);
            VTFFileHeader vtfHDR = new VTFFileHeader();
            vtfHDR.UnSerialize(this.m_vtfReader);
            this.m_dwVersionMajor = (int)vtfHDR.VersionMajor;
            this.m_dwVersionMinor = (int)vtfHDR.VersionMinor;

            VTFHeader_PC hdr = new VTFHeader_PC();
            hdr.UnSerialize(this.m_vtfReader);
            this.m_dwResourceCount = (int)hdr.NumResources;
            this.m_dwHeight = hdr.Height;
            this.m_dwWidth = hdr.Width;
            this.m_Format = hdr.ImageFormat;
            this.m_dwFlags = hdr.Flags;
            this.m_dwFrameCount = hdr.NumFrames;
            this.m_dwDepth = hdr.Depth;
            // Envmaps only have 1 face, if its not then it must be a cubemap with 7 sides
            if (m_dwFlags.IsFlagSet<VTFFlags>(VTFFlags.TEXTUREFLAGS_ENVMAP))
                this.m_dwFaceCount = 1;
            else
                this.m_dwFaceCount = 7;
            this.m_vecReflectivity = hdr.Reflectivity;
            this.m_flBumpScale = hdr.BumpScale;
            this.m_dwLowResImageWidth = hdr.LowResImgWidth;
            this.m_dwLowResImageHeight = hdr.LowResImgHeight;
            this.m_LowResImageFormat = hdr.LowResImgFmt;
            this.m_dwMipCount = ComputeMipCount();
        }
        private int ComputeMipCount()
        {
            // 360 culls unused mips
            if (this.m_bIs360 && this.m_dwVersionMajor == 0x360 && (this.m_dwFlags & VTFFlags.TEXTUREFLAGS_NOMIP) != 0)
                return 0;
            else
            {
                if (this.m_dwDepth <= 0)
                {
                    this.m_dwDepth = 1;
                }

                if (this.m_dwWidth < 1 || this.m_dwHeight < 1 || this.m_dwDepth < 1)
                    return 0;

                int numMipLevels = 1;
                while (true)
                {
                    if (this.m_dwWidth == 1 && this.m_dwHeight == 1 && this.m_dwDepth == 1)
                        break;

                    this.m_dwWidth >>= 1;
                    this.m_dwHeight >>= 1;
                    this.m_dwDepth >>= 1;
                    if (this.m_dwWidth < 1)
                    {
                        this.m_dwWidth = 1;
                    }
                    if (this.m_dwHeight < 1)
                    {
                        this.m_dwHeight = 1;
                    }
                    if (this.m_dwDepth < 1)
                    {
                        this.m_dwDepth = 1;
                    }
                    numMipLevels++;
                }
                return numMipLevels;
            }
        }
        /// <summary>
        /// Computes the dimensions of a specific mip level
        /// </summary>
        /// <param name="mipLevel">mip level to compute</param>
        /// <param name="mipWidth">mip width</param>
        /// <param name="mipHeight">mip height</param>
        /// <param name="mipDepth">mip depth</param>
        private void ComputeMipLevelDimensions(int mipLevel,ref int mipWidth,ref int mipHeight, ref int mipDepth)
        {
            mipWidth = this.m_dwWidth >> mipLevel;
            mipHeight = this.m_dwHeight >> mipLevel;
            mipDepth = this.m_dwDepth >> mipLevel;
            if (mipWidth < 1)
                mipWidth = 1;
            if (mipHeight < 1)
                mipHeight = 1;
            if (mipDepth < 1)
                mipDepth = 1;
        }
        private ResourceEntry FindResourceEntry(ResourceEntryTypeFlag flag)
        {
            foreach (ResourceEntry info in this.m_ResourcesInfo)
            {
                if ((info.flags & flag) != 0)
                    return info;
                else
                    continue;
            }
            return null;
        }
        private int ComputeFaceSize(int startingMipLevel)
        {
            throw new NotImplementedException();
        }
        private int ComputeImageSize(int width, int height, int depth, int mipMaps, VTFImageFormat imgFmt)
        {
            int imgSize = 0;
            for (int i = 0; i < mipMaps; i++)
            {
                imgSize += ComputeImageSize(width, height, depth, mipMaps, imgFmt);
                width >>= 1;
                height >>= 1;
                depth >>= 1;
                if (width < 1)
                    width = 1;

                if (height < 1)
                    height = 1;

                if (depth < 1)
                    depth = 1;
            }
            return imgSize;
        }
        /// <summary>
        /// Computes image size from a format
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        /// <param name="fmt"></param>
        /// <returns>size in bytes</returns>
        private int ComputeFormatSize(int width, int height, int depth, VTFImageFormat fmt)
        {
            switch (fmt)
            {
                case(VTFImageFormat.IMAGE_FORMAT_DXT1):
                case(VTFImageFormat.IMAGE_FORMAT_DXT1_ONEBITALPHA):
                    if (width < 4 && width > 0)
                        width = 4;
                    if (height < 4 && height > 0)
                        height = 4;
                    return ((width + 3) / 4) * ((height + 3) / 4) * 8 * depth;
                case(VTFImageFormat.IMAGE_FORMAT_DXT3):
                case (VTFImageFormat.IMAGE_FORMAT_DXT5):
                    if (width < 4 && width > 0)
                        width = 4;
                    if (height < 4 && height > 0)
                        height = 4;
                    return ((width + 3) / 4) * ((height + 3) / 4) * 16 * depth;
                default:
                    return width * height * depth * GetImageFormatInfo(fmt).bytesPerPixle;
            }
        }
        private VTFImageFormatInfo_t GetImageFormatInfo(VTFImageFormat fmt) { return VTFImageFormatInfo[(int)fmt]; }
    }
}
