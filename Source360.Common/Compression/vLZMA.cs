using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using SevenZip.Compression.LZMA;
using Source360.Common.Helpers;
namespace Source360.Common.Compression
{
    public static class vLZMA
    {
        private static Encoder LZMAEncoder { get; set; }
        private static Decoder LZMADecoder { get; set; }
        private static int LZMAID { get { return 1095588428; } set { } }
        public static byte[] Compress(this byte[] buffer)
        {
            MemoryStream inputStream = new MemoryStream(buffer);
            MemoryStream outputStream = new MemoryStream(buffer.Length / 8);
            LZMAEncoder = new Encoder();
            byte[] properties = LZMAEncoder.getCoderProperties();
            try
            {
                LZMAEncoder.Code(inputStream, outputStream, buffer.Length, buffer.Length / 8, null);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            byte[] compressedData = outputStream.ToArray();
            int size = 17 + compressedData.Length;
            using (MemoryStream finalStream = new MemoryStream(size))
            {
                ///Check if compression got worse if did return original value
                if (size > buffer.Length)
                    return buffer;
                finalStream.Write(LZMAID);
                finalStream.Write(buffer.Length);
                finalStream.Write(compressedData.Length);
                finalStream.Write(properties);
                finalStream.Write(compressedData);
                return finalStream.ToArray();
            }
        }
        public static byte[] Decompress(this byte[] buffer)
        {
            if (buffer.Length > 17 && buffer.IsCompressed())
            {
                MemoryStream inputStream = new MemoryStream(buffer);
                int lzmaID = inputStream.ReadInt();
                int acutalSize = inputStream.ReadInt();
                int lzmaSize = inputStream.ReadInt();
                byte[] props = inputStream.ReadBytes(5);
                int lzmaBufferSize = buffer.Length - 17;
                if (lzmaBufferSize != lzmaSize)
                    throw new Exception(string.Format("LZMA data corruption. Expected {0} bytes got {1}", lzmaSize, lzmaBufferSize));
                byte[] uncompressedBuffer = new byte[acutalSize];
                MemoryStream outputStream = new MemoryStream(uncompressedBuffer);
                try
                {
                    LZMADecoder = new Decoder();
                    LZMADecoder.SetDecoderProperties(props);
                    LZMADecoder.Code(inputStream, outputStream, inputStream.Length, outputStream.Length, null);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                return uncompressedBuffer;
            }
            else
                throw new Exception("Buffer is not compressed!");
        }
        public static bool IsCompressed(this byte[] buffer)
        {
            MemoryStream memStream = new MemoryStream(buffer);
            int id = memStream.ReadInt();
            if (id == LZMAID)
                return true;
            else
                return false;
        }
    }
}
