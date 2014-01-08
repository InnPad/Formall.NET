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
	public class BitCountTest
	{
		[Test, TestMethod]
        [TestCaseSource(typeof(BitCountTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(int cValue, int expected)
        {
        	TestOneCase(cValue, expected);
        }
        
        [Test, TestMethod]
        [TestCaseSource(typeof(BitCountTestCaseFactory), "TestCasesFromCsvFile")]
        public void Test_Against_CSV_Dataset(int cValue, int expected)
        {
        	TestOneCase(cValue, expected);
        }
        
        //[Test, TestMethod]
        public void Generate()
        {
        	new BitCountTestCaseFactory().GenerateTestDataSet();
        }
        
        private void TestOneCase(int cValue, int expected)
        {
        	int actualResult = BCHCalculator.PosMSB(cValue);
        	if(actualResult != expected)
        		Assert.Fail(string.Format("actualResult: {0} expected: {1}", actualResult, expected));
        }
       
        
	}
}
