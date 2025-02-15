using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SevenZip.Compression.LZMA
{
    [ComVisible(true)]
    public static class SevenZipHelper
    {
        private static int dictionary = 8388608;
        private static bool eos = false;
        private static CoderPropID[] propIDs = new CoderPropID[8]
        {
      CoderPropID.DictionarySize,
      CoderPropID.PosStateBits,
      CoderPropID.LitContextBits,
      CoderPropID.LitPosBits,
      CoderPropID.Algorithm,
      CoderPropID.NumFastBytes,
      CoderPropID.MatchFinder,
      CoderPropID.EndMarker
        };
        private static object[] properties = new object[8]
        {
      (object) SevenZipHelper.dictionary,
      (object) 2,
      (object) 3,
      (object) 0,
      (object) 2,
      (object) 128,
      (object) "bt4",
      (object) SevenZipHelper.eos
        };

        public static byte[] Compress(byte[] inputBytes)
        {
            MemoryStream inStream = new MemoryStream(inputBytes);
            MemoryStream outStream = new MemoryStream();
            Encoder encoder = new Encoder();
            encoder.SetCoderProperties(SevenZipHelper.propIDs, SevenZipHelper.properties);
            encoder.WriteCoderProperties((Stream)outStream);
            long length = inStream.Length;
            for (int index = 0; index < 8; ++index)
                outStream.WriteByte((byte)(length >> 8 * index));
            encoder.Code((Stream)inStream, (Stream)outStream, -1L, -1L, (ICodeProgress)null);
            return outStream.ToArray();
        }

        public static byte[] Decompress(byte[] inputBytes)
        {
            MemoryStream inStream = new MemoryStream(inputBytes);
            Decoder decoder = new Decoder();
            inStream.Seek(0L, SeekOrigin.Begin);
            MemoryStream outStream = new MemoryStream();
            byte[] numArray = new byte[5];
            if (inStream.Read(numArray, 0, 5) != 5)
                throw new Exception("input .lzma is too short");
            long outSize = 0;
            for (int index = 0; index < 8; ++index)
            {
                int num = inStream.ReadByte();
                if (num < 0)
                    throw new Exception("Can't Read 1");
                outSize |= (long)(byte)num << 8 * index;
            }
            decoder.SetDecoderProperties(numArray);
            long inSize = inStream.Length - inStream.Position;
            decoder.Code((Stream)inStream, (Stream)outStream, inSize, outSize, (ICodeProgress)null);
            return outStream.ToArray();
        }
    }
}
