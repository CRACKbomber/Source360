using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Source360.VTF
{
    enum tagVTFImageFormat
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
    public struct VTFFileHeader
    {
        public uint TypeIdent;
        public uint VersionMajor;
        public uint VersionMinor;
        public uint HeaderSize;
    }
    public class VTFFile
    {
        private bool m_bIs360;
        private VTFFileHeader m_hdrVTF;

    }
}
