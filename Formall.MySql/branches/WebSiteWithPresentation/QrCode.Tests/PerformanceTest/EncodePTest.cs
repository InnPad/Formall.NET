using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.google.zxing.common;
using com.google.zxing.qrcode.decoder;
using com.google.zxing.qrcode.encoder;
using Custom.Algebra.QrCode.Encoding.DataEncodation;
using Custom.Algebra.QrCode.Encoding.DataEncodation.InputRecognition;
using Custom.Algebra.QrCode.Encoding.EncodingRegion;
using Custom.Algebra.QrCode.Encoding.ErrorCorrection;
using Custom.Algebra.QrCode.Encoding.Masking;
using Custom.Algebra.QrCode.Encoding.Masking.Scoring;
using Custom.Algebra.QrCode.Encoding.Positioning;
using Custom.Algebra.QrCode.Encoding.Versions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GMode = Custom.Algebra.QrCode.Encoding.DataEncodation.Mode;
using Mode = com.google.zxing.qrcode.decoder.Mode;

namespace Custom.Algebra.QrCode.Encoding.Tests.PerformanceTest
{
    using Assert = NUnit.Framework.Assert;
    using CollectionAssert = NUnit.Framework.CollectionAssert;
    using TestAttribute = NUnit.Framework.TestAttribute;
    using TestCaseAttribute = NUnit.Framework.TestCaseAttribute;
    using TestFixtureAttribute = NUnit.Framework.TestFixtureAttribute;

	[TestClass, TestFixture]
	public class EncodePTest
	{
		[Test, TestMethod]
		public void ECPerformanceTest()
		{
			Stopwatch sw = new Stopwatch();
			int timesofTest = 1000;
			
			string[] timeElapsed = new string[2];
			string testCase = "sdg;alwsetuo1204985lkscvzlkjt;";
			sw.Start();
			
			for(int i = 0; i < timesofTest; i++)
			{
				this.Encode(testCase, 3);
			}
			
			sw.Stop();
			
			timeElapsed[0] = sw.ElapsedMilliseconds.ToString();
			
			sw.Reset();
			
			sw.Start();
			
			for(int i = 0; i < timesofTest; i++)
			{
				this.ZXEncode(testCase, 3);
			}
			sw.Stop();
			
			timeElapsed[1] = sw.ElapsedMilliseconds.ToString();
			
			
			Assert.Pass("EC performance {0} Tests~ QrCode.Net: {1} ZXing: {2}", timesofTest, timeElapsed[0], timeElapsed[1]);
			
		}
		
		
		
		
		private void ZXEncode(string content, int option)
		{
			System.String encoding = QRCodeConstantVariable.DefaultEncoding;
			ErrorCorrectionLevelInternal m_EcLevelInternal = ErrorCorrectionLevelInternal.H;
			QRCodeInternal qrCodeInternal = new QRCodeInternal();
			
			// Step 1: Choose the mode (encoding).
			Mode mode = EncoderInternal.chooseMode(content, encoding);
			
			// Step 2: Append "bytes" into "dataBits" in appropriate encoding.
			BitVector dataBits = new BitVector();
			EncoderInternal.appendBytes(content, mode, dataBits, encoding);
			// Step 3: Initialize QR code that can contain "dataBits".
			int numInputBytes = dataBits.sizeInBytes();
			EncoderInternal.initQRCode(numInputBytes, m_EcLevelInternal, mode, qrCodeInternal);
			
			// Step 4: Build another bit vector that contains header and data.
			BitVector headerAndDataBits = new BitVector();
			
			// Step 4.5: Append ECI message if applicable
			if (mode == Mode.BYTE && !QRCodeConstantVariable.DefaultEncoding.Equals(encoding))
			{
				CharacterSetECI eci = CharacterSetECI.getCharacterSetECIByName(encoding);
				if (eci != null)
				{
					EncoderInternal.appendECI(eci, headerAndDataBits);
				}
			}
			
			EncoderInternal.appendModeInfo(mode, headerAndDataBits);
			
			int numLetters = mode.Equals(Mode.BYTE)?dataBits.sizeInBytes():content.Length;
			EncoderInternal.appendLengthInfo(numLetters, qrCodeInternal.Version, mode, headerAndDataBits);
			headerAndDataBits.appendBitVector(dataBits);
			
			// Step 5: Terminate the bits properly.
			EncoderInternal.terminateBits(qrCodeInternal.NumDataBytes, headerAndDataBits);
			
			// Step 6: Interleave data bits with error correction code.
			BitVector finalBits = new BitVector();
			EncoderInternal.interleaveWithECBytes(headerAndDataBits, qrCodeInternal.NumTotalBytes, qrCodeInternal.NumDataBytes, qrCodeInternal.NumRSBlocks, finalBits);
			
			if(option == 3)
			{
				return;
			}
			
			// Step 7: Choose the mask pattern and set to "QRCodeInternal".
			ByteMatrix matrix = new ByteMatrix(qrCodeInternal.MatrixWidth, qrCodeInternal.MatrixWidth);
			qrCodeInternal.MaskPattern = EncoderInternal.chooseMaskPattern(finalBits, qrCodeInternal.EcLevelInternal, qrCodeInternal.Version, matrix);
			
			// Step 8.  Build the matrix and set it to "QRCodeInternal".
			MatrixUtil.buildMatrix(finalBits, qrCodeInternal.EcLevelInternal, qrCodeInternal.Version, qrCodeInternal.MaskPattern, matrix);
			qrCodeInternal.Matrix = matrix;
			
		}
		
		private void Encode(string content, int option)
		{
			ErrorCorrectionLevel errorLevel = ErrorCorrectionLevel.H;
			
			EncodationStruct encodeStruct = DataEncode.Encode(content, errorLevel);
			
			BitList codewords = ECGenerator.FillECCodewords(encodeStruct.DataCodewords, encodeStruct.VersionDetail);
			
			if(option == 3)
				return;
			
			TriStateMatrix triMatrix = new TriStateMatrix(encodeStruct.VersionDetail.MatrixWidth);
			PositioninngPatternBuilder.EmbedBasicPatterns(encodeStruct.VersionDetail.Version, triMatrix);
			
			triMatrix.EmbedVersionInformation(encodeStruct.VersionDetail.Version);
			triMatrix.EmbedFormatInformation(errorLevel, new Pattern0());
			triMatrix.TryEmbedCodewords(codewords);
			
			triMatrix.GetLowestPenaltyMatrix(errorLevel);
		}
	}
}
