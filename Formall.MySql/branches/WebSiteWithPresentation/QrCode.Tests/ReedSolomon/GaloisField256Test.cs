using System;
using Custom.Algebra.QrCode.Encoding.ReedSolomon;
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
	public class GaloisField256Test
	{
		[Test, TestMethod]
        [TestCaseSource(typeof(GaloisField256TestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(int i, int exp)
        {
        	GaloisField256 gfield = GaloisField256.QRCodeGaloisField;
        	
        	int result = gfield.Exponent(i);
        	
        	if( exp != result)
        		Assert.Fail("Fail. request {0} Expect {1} result {2}", i, exp, result);
        }
        
        [Test, TestMethod]
        [TestCaseSource(typeof(GaloisField256TestCaseFactory), "TestCaseFromCSVFile")]
        public void Test_against_CSV_Dataset(int i, int exp)
        {
        	GaloisField256 gfield = GaloisField256.QRCodeGaloisField;
        	
        	int result = gfield.Exponent(i);
        	
        	if( exp != result)
        		Assert.Fail("Fail. request {0} Expect {1} result {2}", i, exp, result);
        }
        
        //[Test, TestMethod]
        public void Generate()
        {
        	new GaloisField256TestCaseFactory().GenerateTestDataSet();
        }
        
        
	}
}
