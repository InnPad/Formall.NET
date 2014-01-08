using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zxMode = com.google.zxing.qrcode.decoder.Mode;

namespace Custom.Calculation.QrCode.Encoding.Tests
{
    using Custom.Algebra.QrCode.Encoding;
    using Custom.Algebra.QrCode.Encoding.DataEncodation;
    using Custom.Algebra.QrCode.Encoding.DataEncodation.InputRecognition;
    using TestAttribute = NUnit.Framework.TestAttribute;
    using TestCaseAttribute = NUnit.Framework.TestCaseAttribute;
    using TestFixtureAttribute = NUnit.Framework.TestFixtureAttribute;

	[TestClass, TestFixture]
	public class ModeAutoSelectTest
	{	
		[TestCase("15097435193480", "", (int)(Mode.Numeric))]
		[TestCase("35980SDLFKWETLKD", "", (int)(Mode.Alphanumeric))]
		[TestCase("r3546rraaaaÁÁÒÒÂÂöö", QRCodeConstantVariable.DefaultEncoding, (int)(Mode.EightBitByte))]
		[TestCase("r3546rraaaaｸｸｸｸﾗﾗﾗﾗｳｳｳ", "shift_jis", (int)(Mode.EightBitByte))]
		[TestCase("r3546rraaaaรรฝฝฝฝรสสสสสรร", "windows-874", (int)(Mode.EightBitByte))]
		[TestCase("ｦｦｦｦｦﾝﾝﾝﾝﾝ絞絞絞裁裁裁裁冊冊冊", QRCodeConstantVariable.UTF8Encoding, (int)(Mode.EightBitByte))]
		[TestCase("絞絞絞裁裁裁裁冊冊冊", "", (int)(Mode.Kanji))]
		[TestCase("123sad絞絞絞裁裁裁裁冊冊冊", QRCodeConstantVariable.UTF8Encoding, (int)(Mode.EightBitByte))]
		[TestCase("123sadััััใใใ์์์", "windows-874", (int)(Mode.EightBitByte))]
		[TestCase("ｦｦｦｦｦﾝﾝﾝﾝﾝ◌ัใใใ◌์◌์◌ﾝﾝﾝÑÑå", QRCodeConstantVariable.UTF8Encoding, (int)(Mode.EightBitByte))]
		[TestCase("r3546สสสส์์แแแ์", "windows-874", (int)(Mode.EightBitByte))]
		[TestCase("ｦｦｦｦｦﾝﾝﾝﾝﾝÑÑÑååสสส◌์◌์แแแ", QRCodeConstantVariable.UTF8Encoding, (int)(Mode.EightBitByte))]
		[TestCase("r3546ลลธธธลๆๆๆๆๆ", "windows-874", (int)(Mode.EightBitByte))]
		[TestCase("ｦｦｦｦｦﾝﾝﾝﾝﾝd◌ั◌ลๆๆๆๆ", QRCodeConstantVariable.UTF8Encoding, (int)(Mode.EightBitByte))]
		[TestCase("r3546rraaaaêêêêêýýýýýýþþþþ", QRCodeConstantVariable.DefaultEncoding, (int)(Mode.EightBitByte))]
		[TestCase("ｦｦｦｦｦﾝﾝﾝﾝﾝaaa¶¶¶ååaaaรรฝฝ", QRCodeConstantVariable.UTF8Encoding, (int)(Mode.EightBitByte))]
		[TestCase("r3546rraaaa¶¶¶ååÑÑÑÑåååå¶¶¶", QRCodeConstantVariable.DefaultEncoding, (int)(Mode.EightBitByte))]
		[TestCase("ｦｦｦｦｦﾝﾝﾝﾝﾝÑÑåååå¶¶¶ฝรสสส", QRCodeConstantVariable.UTF8Encoding, (int)(Mode.EightBitByte))]
		[TestCase("r3546rraaaaÆÆÆÆÔÔÔÔØØØÔÔÆÆ", QRCodeConstantVariable.DefaultEncoding, (int)(Mode.EightBitByte))]
		[TestCase("ｦｦｦｦｦﾝﾝﾝﾝﾝสสส◌์◌์", QRCodeConstantVariable.UTF8Encoding, (int)(Mode.EightBitByte))]
		public void AutoSelectTestTwo(string content, string expectEncodingName, int expectMode)
		{
			RecognitionStruct structValue = InputRecognise.Recognise(content);
				
			if(structValue.Mode != (Mode)expectMode)
				Assert.Fail("Mode return as {0}. But it should be {1}", structValue.Mode, (Mode)expectMode);
			
			if(structValue.Mode == Mode.EightBitByte)
			{
				if(structValue.EncodingName != expectEncodingName)
						Assert.Fail("Encoding Name return as {0}. But it should be {1}", structValue.EncodingName, expectEncodingName);
			}
			
			
			
		}
		
		
	}
}
