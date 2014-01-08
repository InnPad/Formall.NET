using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Custom.Algebra.QrCode.Encoding.Tests.ReedSolomon
{
    using Assert = NUnit.Framework.Assert;
    using CollectionAssert = NUnit.Framework.CollectionAssert;
    using TestAttribute = NUnit.Framework.TestAttribute;
    using TestCaseAttribute = NUnit.Framework.TestCaseAttribute;
    using TestCaseSourceAttribute = NUnit.Framework.TestCaseSourceAttribute;

    using TestFixtureAttribute = NUnit.Framework.TestFixtureAttribute;
	[TestClass, TestFixture]
	public class ByteConvertTest
	{
		[Test, TestMethod]
        [TestCaseSource(typeof(ByteConvertTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(byte[] test)
        {
        	TestOneData(test);
		}
        
        [Test, TestMethod]
        [TestCaseSource(typeof(ByteConvertTestCaseFactory), "TestCaseFromTxtFile")]
        public void Test_against_TXT_Dataset(byte[] test)
        {
        	TestOneData(test);
        }
        
        private void TestOneData(byte[] test)
        {
        	BitList bitList = BitListExtensions.ToBitList(test);
        	
        	byte[] result = bitList.ToByteArray();
        	
        	if(!PolynomialExtensions.isEqual(result, test))
        		Assert.Fail("Byte convert fail. result {0}, expect {1}", result[0], test[0]);
        }
        
        
        
        //[Test, TestMethod]
        public void Generate()
        {
        	new ByteConvertTestCaseFactory().GenerateTestDataSet();
        }
	}
}
