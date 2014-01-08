using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Imaging
{
    public static class Barcode
    {
        private static readonly Dictionary<string, ZXing.BarcodeFormat> _map = new Dictionary<string, ZXing.BarcodeFormat>
            {
               // { "All1D", ZXing.BarcodeFormat.All_1D },
                { "AZTEC", ZXing.BarcodeFormat.AZTEC },
                { "CODABAR", ZXing.BarcodeFormat.CODABAR },
                { "CODE128", ZXing.BarcodeFormat.CODE_128 },
                { "CODE39", ZXing.BarcodeFormat.CODE_39 },
                //{ "CODE93", ZXing.BarcodeFormat.CODE_93 },
                { "DATAMATRIX", ZXing.BarcodeFormat.DATA_MATRIX },
                { "EAN13", ZXing.BarcodeFormat.EAN_13 },
                { "EAN8", ZXing.BarcodeFormat.EAN_8 },
                { "ITF", ZXing.BarcodeFormat.ITF },
                //{ "MAXICODE", ZXing.BarcodeFormat.MAXICODE },
                { "MSI", ZXing.BarcodeFormat.MSI },
                { "PDF417", ZXing.BarcodeFormat.PDF_417 },
                { "PLESSEY", ZXing.BarcodeFormat.PLESSEY },
                { "QRCODE", ZXing.BarcodeFormat.QR_CODE },
                //{ "RSS14", ZXing.BarcodeFormat.RSS_14 },
                //{ "RSSEXPANDED", ZXing.BarcodeFormat.RSS_EXPANDED },
                { "UPCA", ZXing.BarcodeFormat.UPC_A },
                //{ "UPCE", ZXing.BarcodeFormat.UPC_E },
                //{ "UPCEANEXTENSION", ZXing.BarcodeFormat.UPC_EAN_EXTENSION }
            };


        public static BitMatrix Encode(string contents, int width, int height, string format)
        {
            ZXing.BarcodeFormat zxingFormat;
            if (!_map.TryGetValue(string.Concat(format.Split(new [] { ' ', '-', '_' })).ToUpperInvariant(), out zxingFormat))
            {
                zxingFormat = ZXing.BarcodeFormat.QR_CODE;
            }
            return Encode(contents, width, height, zxingFormat);
        }

        /// <summary>
        /// Encode string content to BitMatrix matrix
        /// </summary>
        /// <exception cref="InputOutOfBoundaryException">
        /// This exception for string content is null, empty or too large</exception>
        private static BitMatrix Encode(string contents, int width, int height, ZXing.BarcodeFormat format)
        {
            var writer = new ZXing.BarcodeWriter
            {
                Format = format,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = 0,
                    PureBarcode = false
                }
            };
            var matrix = writer.Encode(contents);
            return new ZXingBitMatrix(matrix);
        }

        public static DecodeResult DecodeGray8(byte[] luminanceArray, int width, int height)
        {
            var reader = new ZXing.BarcodeReader();
            var result = reader.Decode(luminanceArray, width, height, ZXing.RGBLuminanceSource.BitmapFormat.Gray8);
            return new DecodeResult(result);
        }

        public static DecodeResult DecodeRGB24(byte[] rgbArray, int width, int height)
        {
            var reader = new ZXing.BarcodeReader();
            var result = reader.Decode(rgbArray, width, height, ZXing.RGBLuminanceSource.BitmapFormat.RGB24);
            return new DecodeResult(result);
        }

        public static class Aztec
        {
            public static DecodeResult DecodeRGB24(byte[] rgbArray, int width, int height)
            {
                var result = Decode(rgbArray, width, height, ZXing.RGBLuminanceSource.BitmapFormat.RGB24);
                return new Barcode.DecodeResult(result);
            }

            public static DecodeResult DecodeGray8(byte[] luminanceArray, int width, int height)
            {
                var result = Decode(luminanceArray, width, height, ZXing.RGBLuminanceSource.BitmapFormat.Gray8);
                return new Barcode.DecodeResult(result);
            }

            private static ZXing.Result Decode(byte[] luminanceArray, int width, int height, ZXing.RGBLuminanceSource.BitmapFormat bitmapFormat)
            {
                ZXing.LuminanceSource luminanceSource = new ZXing.RGBLuminanceSource(luminanceArray, width, height, bitmapFormat);
                ZXing.Binarizer binarizer = new ZXing.Common.HybridBinarizer(luminanceSource);
                var binary = new ZXing.BinaryBitmap(binarizer);
                var reader = new ZXing.Aztec.AztecReader();
                var hints = new Dictionary<ZXing.DecodeHintType, object>();
                return reader.decode(binary, hints);
            }

            /// <summary>
            /// Encode string content to BitMatrix matrix
            /// </summary>
            /// <exception cref="InputOutOfBoundaryException">
            /// This exception for string content is null, empty or too large</exception>
            public static BitMatrix Encode(string contents, int width, int height)
            {
                var writer = new ZXing.Aztec.AztecWriter();
                var hints = new Dictionary<ZXing.EncodeHintType, object>();
                var matrix = writer.encode(contents, ZXing.BarcodeFormat.AZTEC, width, height, hints);
                return new ZXingBitMatrix(matrix);
            }
        }

        public sealed class DecodeResult
        {
            internal DecodeResult(ZXing.Result result)
                : this(result.Text, result.RawBytes, result.ResultPoints, result.BarcodeFormat, result.ResultMetadata, result.Timestamp)
            {
            }

            internal DecodeResult(string text, byte[] rawBytes, ZXing.ResultPoint[] resultPoints, ZXing.BarcodeFormat format, IDictionary<ZXing.ResultMetadataType, object> metadata, long timestamp)
            {
                if ((text == null) && (rawBytes == null))
                {
                    throw new ArgumentException("Text and bytes are null");
                }
                this.Text = text;
                this.RawBytes = rawBytes;
                this.Points = resultPoints.Select(o => new Point(o.X, o.Y)).ToArray();
                this.BarcodeFormat = format.ToString();
                this.Metadata = metadata.ToDictionary(o => o.Key.ToString(), o => o.Value);
                this.Timestamp = timestamp;
            }

            public string BarcodeFormat { get; private set; }

            public byte[] RawBytes { get; private set; }

            public IDictionary<string, object> Metadata { get; private set; }

            public Point[] Points { get; private set; }

            public string Text { get; private set; }

            public long Timestamp { get; private set; }

            public override string ToString()
            {
                if (this.Text == null)
                {
                    return ("[" + this.RawBytes.Length + " bytes]");
                }
                return this.Text;
            }
        }

        public static class QrCode
        {
            public static DecodeResult DecodeRGB24(byte[] rgbArray, int width, int height)
            {
                var result = Decode(rgbArray, width, height, ZXing.RGBLuminanceSource.BitmapFormat.RGB24);
                return new DecodeResult(result);
            }

            public static DecodeResult DecodeGray8(byte[] luminanceArray, int width, int height)
            {
                var result = Decode(luminanceArray, width, height, ZXing.RGBLuminanceSource.BitmapFormat.Gray8);
                return new DecodeResult(result);
            }

            private static ZXing.Result Decode(byte[] luminanceArray, int width, int height, ZXing.RGBLuminanceSource.BitmapFormat bitmapFormat)
            {
                ZXing.LuminanceSource luminanceSource = new ZXing.RGBLuminanceSource(luminanceArray, width, height, bitmapFormat);
                ZXing.Binarizer binarizer = new ZXing.Common.HybridBinarizer(luminanceSource);
                var binary = new ZXing.BinaryBitmap(binarizer);
                var reader = new ZXing.QrCode.QRCodeReader();
                var hints = new Dictionary<ZXing.DecodeHintType, object>();
                return reader.decode(binary, hints);
            }

            /// <summary>
            /// Encode string content to BitMatrix matrix
            /// </summary>
            /// <exception cref="InputOutOfBoundaryException">
            /// This exception for string content is null, empty or too large</exception>
            public static BitMatrix Encode(string contents, int width, int height)
            {
                var writer = new ZXing.QrCode.QRCodeWriter();
                var hints = new Dictionary<ZXing.EncodeHintType, object>();
                var matrix = writer.encode(contents, ZXing.BarcodeFormat.QR_CODE, width, height, hints);
                return new ZXingBitMatrix(matrix);
            }
        }

        public class MetadataType
        {
            public static implicit operator string(MetadataType type)
            {
                return type != null ? type._name : null;
            }

            public static implicit operator MetadataType(string name)
            {
                return typeof(MetadataType)
                    .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                    .Select(o => o.GetValue(null) as MetadataType)
                    .Where(o => o != null)
                    .FirstOrDefault(o => string.Equals(o._name, name, StringComparison.OrdinalIgnoreCase)) ?? MetadataType.Other;
            }

            public static readonly MetadataType Other = new MetadataType("OTHER");
            public static readonly MetadataType Orientation = new MetadataType("ORIENTATION");
            public static readonly MetadataType ByteSegments = new MetadataType("BYTE_SEGMENTS");
            public static readonly MetadataType ErrorCorrectionLevel = new MetadataType("ERROR_CORRECTION_LEVEL");
            public static readonly MetadataType IssueNumber = new MetadataType("ISSUE_NUMBER");
            public static readonly MetadataType SuggestedPrice = new MetadataType("SUGGESTED_PRICE");
            public static readonly MetadataType PossibleCountry = new MetadataType("POSSIBLE_COUNTRY");
            public static readonly MetadataType UpcEanExtension = new MetadataType("UPC_EAN_EXTENSION");
            public static readonly MetadataType StructuredAppendSequence = new MetadataType("STRUCTURED_APPEND_SEQUENCE");
            public static readonly MetadataType StructuredAppendParity = new MetadataType("STRUCTURED_APPEND_PARITY");
            public static readonly MetadataType PDF417ExtraMetadata = new MetadataType("PDF417_EXTRA_METADATA");

            private readonly string _name;

            private MetadataType(string name)
            {
                _name = name;
            }
        }

        class ZXingBitMatrix : BitMatrix
        {
            private readonly ZXing.Common.BitMatrix _internal;

            public ZXingBitMatrix(ZXing.Common.BitMatrix matrix)
            {
                _internal = matrix;
            }

            public override bool this[int i, int j]
            {
                get { return _internal[i, j]; }
            }

            public override int Width
            {
                get { return _internal.Width; }
            }

            public override int Height
            {
                get { return _internal.Height; }
            }
        }
    }
}
