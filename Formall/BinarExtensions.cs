using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall
{
    public static class BinaryExtensions
    {
        public static Binary Compress(this Binary binary)
        {
            if (!binary.Compressed)
            {
                using (var output = new MemoryStream(binary.Data.Length))
                {
                    using (GZipStream compress = new GZipStream(output, CompressionMode.Compress))
                    {
                        compress.Write(binary.Data, 0, binary.Data.Length);

                        compress.Flush();
                    }

                    output.Flush();

                    binary.Data = output.GetBuffer();
                    binary.Compressed = true;
                }
            }

            return binary;
        }

        public static Binary Decompress(this Binary binary)
        {
            if (binary != null && binary.Compressed && binary.Data != null && binary.Data.Length > 0)
            {
                using (var output = new MemoryStream(binary.Data.Length))
                {
                    using (GZipStream decompress = new GZipStream(output, CompressionMode.Decompress))
                    {
                        decompress.Write(binary.Data, 0, binary.Data.Length);

                        decompress.Flush();
                    }

                    output.Flush();

                    binary.Data = output.GetBuffer();
                    binary.Compressed = false;
                }
            }

            return binary;
        }

        public static void Encode(this Binary binary, string value, Encoding encoding)
        {
            binary.Encoding = encoding.WebName;
            var encoder = (encoding ?? UnicodeEncoding.Default).GetEncoder();

            binary.Data = Encoding.Convert(UnicodeEncoding.Default, encoding, GetBytes(value));
        }

        public static string Decode(this Binary binary)
        {
            if (binary == null)
                return null;

            if (binary.Data == null)
                return null;

            if (binary.Encoding == null)
                return GetString(binary.Data);

            return GetString(Encoding.Convert(Encoding.GetEncoding(binary.Encoding), UnicodeEncoding.Default, binary.Data));
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
