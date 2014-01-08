using System;
using System.Diagnostics;
using com.google.zxing.qrcode.encoder;
using Custom.Algebra.QrCode.Encoding.ErrorCorrection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Custom.Algebra.QrCode.Encoding.Tests.PerformanceTest
{
    using Assert = NUnit.Framework.Assert;
    using CollectionAssert = NUnit.Framework.CollectionAssert;
    using TestAttribute = NUnit.Framework.TestAttribute;
    using TestCaseAttribute = NUnit.Framework.TestCaseAttribute;
    using TestFixtureAttribute = NUnit.Framework.TestFixtureAttribute;

	[TestClass, TestFixture]
	public class ErrorCorrectionPTest
	{
		private BitVector GenerateDataCodewords(int numDataCodewords, Random randomizer)
		{
			BitVector result = new BitVector();
			for(int numDC = 0; numDC < numDataCodewords; numDC++)
			{
				result.Append((randomizer.Next(0, 256) & 0xFF), 8);
			}
			if(result.sizeInBytes() == numDataCodewords)
				return result;
			else
				throw new Exception("Auto generate data codewords fail");
		}
		
		private static VersionDetail s_vcInfo = new VersionDetail(1, 26, 19, 1);
		
		[Test, TestMethod]
		public void PerformanceTest()
		{
			Random randomizer = new Random();
			BitVector dataCodewordsV = GenerateDataCodewords(s_vcInfo.NumDataBytes, randomizer);
			
			BitList dataCodewordsL = new BitList();
			dataCodewordsL.Add(dataCodewordsV);
			
			Stopwatch sw = new Stopwatch();
			int timesofTest = 1000;
			
			string[] timeElapsed = new string[2];
			
			sw.Start();
			for(int i = 0; i < timesofTest; i++)
			{
				ECGenerator.FillECCodewords(dataCodewordsL, s_vcInfo);
			}
			sw.Stop();
			
			timeElapsed[0] = sw.ElapsedMilliseconds.ToString();
			
			sw.Reset();
			
			sw.Start();
			for(int i = 0; i < timesofTest; i++)
			{
				BitVector finalBits = new BitVector();
				EncoderInternal.interleaveWithECBytes(dataCodewordsV, s_vcInfo.NumTotalBytes, s_vcInfo.NumDataBytes, s_vcInfo.NumECBlocks, finalBits);
			}
			sw.Stop();
			
			timeElapsed[1] = sw.ElapsedMilliseconds.ToString();
			
			
			Assert.Pass("ErrorCorrection performance {0} Tests~ QrCode.Net: {1} ZXing: {2}", timesofTest, timeElapsed[0], timeElapsed[1]);
			
			
			
		}
		
		
	}
}
