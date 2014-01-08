using System;
using System.Diagnostics;
using com.google.zxing.qrcode.encoder;
using Custom.Algebra.QrCode.Encoding.ReedSolomon;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Custom.Algebra.QrCode.Encoding.Tests.PerformanceTest
{
    using Assert = NUnit.Framework.Assert;
    using CollectionAssert = NUnit.Framework.CollectionAssert;
    using TestAttribute = NUnit.Framework.TestAttribute;
    using TestCaseAttribute = NUnit.Framework.TestCaseAttribute;
    using TestFixtureAttribute = NUnit.Framework.TestFixtureAttribute;

	[TestClass, TestFixture]
	public class ReedSolomonPTest
	{
		[Test, TestMethod]
		public void PerformanceTest()
		{
			Random randomizer = new Random();
			sbyte[] zxTestCase = PolynomialExtensions.GenerateSbyteArray(40, randomizer);
			int ecBytes = 50;
			byte[] testCase = PolynomialExtensions.ToByteArray(zxTestCase);
			
			Stopwatch sw = new Stopwatch();
			int timesofTest = 10000;
			
			string[] timeElapsed = new string[2];
			
			sw.Start();
			GaloisField256 gf256 = GaloisField256.QRCodeGaloisField;
			GeneratorPolynomial generator = new GeneratorPolynomial(gf256);
			for(int i = 0; i < timesofTest; i++)
			{
				ReedSolomonEncoder.Encode(testCase, ecBytes, generator);
			}
			
			sw.Stop();
			
			timeElapsed[0] = sw.ElapsedMilliseconds.ToString();
			
			sw.Reset();
			
			sw.Start();
			
			for(int i = 0; i < timesofTest; i++)
			{
				EncoderInternal.generateECBytes(zxTestCase, ecBytes);
			}
			sw.Stop();
			
			timeElapsed[1] = sw.ElapsedMilliseconds.ToString();
			
			
			Assert.Pass("ReedSolomon performance {0} Tests~ QrCode.Net: {1} ZXing: {2}", timesofTest, timeElapsed[0], timeElapsed[1]);
			
		}
	}
}
