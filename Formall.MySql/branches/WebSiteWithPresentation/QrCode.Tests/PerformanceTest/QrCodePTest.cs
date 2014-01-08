using System;
using System.Diagnostics;
using com.google.zxing.qrcode.decoder;
using com.google.zxing.qrcode.encoder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Custom.Algebra.QrCode.Encoding.Tests.PerformanceTest
{
    using Assert = NUnit.Framework.Assert;
    using CollectionAssert = NUnit.Framework.CollectionAssert;
    using TestAttribute = NUnit.Framework.TestAttribute;
    using TestCaseAttribute = NUnit.Framework.TestCaseAttribute;
    using TestCaseSourceAttribute = NUnit.Framework.TestCaseSourceAttribute;
    using TestFixtureAttribute = NUnit.Framework.TestFixtureAttribute;

	[TestClass, TestFixture]
	public class QrCodePTest
	{
		[Test, TestMethod]
		public void QrPerformanceTest()
		{
			Stopwatch sw = new Stopwatch();
			int timesofTest = 1000;
			
			string[] timeElapsed = new string[2];
			string testCase = "sdg;alwsetuo1204985lkscvzlkjt;sdfjwltkja;slkdfjoiutLSAFAJ;GLKAJS;LDKJT;LKJ";
			
			QrEncoder encoder = new QrEncoder(ErrorCorrectionLevel.H);
			
			sw.Start();
			
			for(int i = 0; i < timesofTest; i++)
			{
				encoder.Encode(testCase);
			}
			
			sw.Stop();
			
			timeElapsed[0] = sw.ElapsedMilliseconds.ToString();
			
			sw.Reset();
			
			ErrorCorrectionLevelInternal level = ErrorCorrectionLevelConverter.ToInternal(ErrorCorrectionLevel.H);
            QRCodeInternal qrCodeInternal = new QRCodeInternal();
           
			sw.Start();
			
			for(int i = 0; i < timesofTest; i++)
			{
				 EncoderInternal.encode(testCase, level, qrCodeInternal);
			}
			sw.Stop();
			
			timeElapsed[1] = sw.ElapsedMilliseconds.ToString();
			
			
			Assert.Pass("Encode performance {0} Tests~ QrCode.Net: {1} ZXing: {2}", timesofTest, timeElapsed[0], timeElapsed[1]);
			
		}
	}
}
