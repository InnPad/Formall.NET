using System;
using System.Diagnostics;
using com.google.zxing.qrcode.decoder;
using com.google.zxing.qrcode.encoder;
using Custom.Algebra.QrCode.Encoding.Versions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mode = Custom.Algebra.QrCode.Encoding.DataEncodation.Mode;
using ZMode = com.google.zxing.qrcode.decoder.Mode;

namespace Custom.Algebra.QrCode.Encoding.Tests.PerformanceTest
{
    using Assert = NUnit.Framework.Assert;
    using CollectionAssert = NUnit.Framework.CollectionAssert;
    using TestAttribute = NUnit.Framework.TestAttribute;
    using TestCaseAttribute = NUnit.Framework.TestCaseAttribute;
    using TestFixtureAttribute = NUnit.Framework.TestFixtureAttribute;

	[TestClass, TestFixture]
	public class VersionControlPTest
	{
		[Test, TestMethod]
		public void SmallLengthTest()
		{
			PTest(40);
		}
		
		[Test, TestMethod]
		public void HugeLengthTest()
		{
			PTest(9776);
		}
		
		private void PTest(int contentLength)
		{
			Stopwatch sw = new Stopwatch();
			int timesofTest = 1000;
			
			string[] timeElapsed = new string[2];
			sw.Start();
			
			for(int i = 0; i < timesofTest; i++)
			{
				VersionControl.InitialSetup(contentLength, Mode.Alphanumeric, ErrorCorrectionLevel.H, QRCodeConstantVariable.DefaultEncoding);
			}
			
			sw.Stop();
			
			timeElapsed[0] = sw.ElapsedMilliseconds.ToString();
			
			sw.Reset();
			
			QRCodeInternal qrInternal = new QRCodeInternal();
			
			int byteLength = contentLength / 8;
			
			sw.Start();
			
			for(int i = 0; i < timesofTest; i++)
			{
				EncoderInternal.initQRCode(byteLength, ErrorCorrectionLevelInternal.H, ZMode.ALPHANUMERIC, qrInternal);
			}
			
			sw.Stop();
			
			timeElapsed[1] = sw.ElapsedMilliseconds.ToString();
			
			
			Assert.Pass("VersionControl {0} Tests~ QrCode.Net: {1} ZXing: {2}", timesofTest, timeElapsed[0], timeElapsed[1]);
			
		}
	}
}
