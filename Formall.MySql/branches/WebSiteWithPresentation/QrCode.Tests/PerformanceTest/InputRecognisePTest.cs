using System;
using System.Diagnostics;
using com.google.zxing.qrcode.encoder;
using Custom.Algebra.QrCode.Encoding.DataEncodation.InputRecognition;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Custom.Algebra.QrCode.Encoding.Tests.PerformanceTest
{
    using Assert = NUnit.Framework.Assert;
    using CollectionAssert = NUnit.Framework.CollectionAssert;
    using TestAttribute = NUnit.Framework.TestAttribute;
    using TestCaseAttribute = NUnit.Framework.TestCaseAttribute;
    using TestCaseSourceAttribute = NUnit.Framework.TestCaseSourceAttribute;
    using TestFixtureAttribute = NUnit.Framework.TestFixtureAttribute;

	[TestClass, TestFixture]
	public class InputRecognisePTest
	{
		private const string s_TestCase = "wektjzoiyt19367-09zcvSLDKTUWEOSDVKXWTknb,hw;taoeitgaggh;lqwi62o46ikbhziuxcy346i";
		
		[Test, TestMethod]
		public void PerformanceTest()
		{
			Stopwatch sw = new Stopwatch();
			int timesofTest = 1000;
			
			string[] timeElapsed = new string[2];
			
			sw.Start();
			
			for(int i = 0; i < timesofTest; i++)
			{
				InputRecognise.Recognise(s_TestCase);
			}
			
			sw.Stop();
			
			timeElapsed[0] = sw.ElapsedMilliseconds.ToString();
			
			sw.Reset();
			
			sw.Start();
			
			for(int i = 0; i < timesofTest; i++)
			{
				EncoderInternal.chooseMode(s_TestCase, QRCodeConstantVariable.DefaultEncoding);
			}
			sw.Stop();
			
			timeElapsed[1] = sw.ElapsedMilliseconds.ToString();
			
			
			Assert.Pass("InputRecognise performance {0} Tests~ QrCode.Net: {1} ZXing: {2}", timesofTest, timeElapsed[0], timeElapsed[1]);
			
		}
	}
}
