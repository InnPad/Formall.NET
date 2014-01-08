using System.Collections.Generic;
using Custom.Algebra.QrCode.Encoding.EncodingRegion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Custom.Algebra.QrCode.Encoding.Tests.EncodingRegion
{
    using Assert = NUnit.Framework.Assert;
    using CollectionAssert = NUnit.Framework.CollectionAssert;
    using TestAttribute = NUnit.Framework.TestAttribute;
    using TestCaseAttribute = NUnit.Framework.TestCaseAttribute;
    using TestCaseSourceAttribute = NUnit.Framework.TestCaseSourceAttribute;
    using TestFixtureAttribute = NUnit.Framework.TestFixtureAttribute;

	[TestClass, TestFixture]
	public class BCHTest
	{
		[Test, TestMethod]
		[TestCaseSource(typeof(BCHTestCaseFactory), "TestCasesFromReferenceImplementation")]
		public void Test_Against_Reference_Implementation(int inputNum, int expected)
		{
			TestOneCase(inputNum, expected);
		}
		
		[Test, TestMethod]
		[TestCaseSource(typeof(BCHTestCaseFactory), "TestCasesFromCsvFile")]
		public void Test_Against_CSV_Dataset(int inputNum, int expected)
		{
			TestOneCase(inputNum, expected);
		}
		
		//[Test, TestMethod]
		public void Generate()
		{
			new BCHTestCaseFactory().GenerateTestDataSet();
		}
		
		// From Appendix D in JISX0510:2004 (p. 67)
		private const int VERSION_INFO_POLY = 0x1f25; // 1 1111 0010 0101
		
		// From Appendix C in JISX0510:2004 (p.65).
		private const int TYPE_INFO_POLY = 0x537;
		
		private void TestOneCase(int inputNum, int expected)
		{
			int bchNum = BCHCalculator.CalculateBCH(inputNum, VERSION_INFO_POLY);
			if(bchNum != expected)
				Assert.Fail("InputNum: {0} Actual: {1} Expect: {2}", inputNum, bchNum, expected);
		}
		
	}
}
