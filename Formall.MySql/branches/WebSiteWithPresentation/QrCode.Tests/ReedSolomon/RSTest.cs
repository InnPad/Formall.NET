using System;
using com.google.zxing.qrcode.encoder;
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
	public class RSTest
	{
		private static GaloisField256 m_gfield = GaloisField256.QRCodeGaloisField;
		private static GeneratorPolynomial m_cacheGeneratorPoly = new GeneratorPolynomial(m_gfield);
		
		[Test, TestMethod]
        [TestCaseSource(typeof(RSEncoderTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(byte[] data, int ecLength, byte[] expectResult)
        {
        	
        	TestOneCase(data, ecLength, expectResult);
        }
        
        private void TestOneCase(byte[] data, int ecLength, byte[] expectResult)
        {
        	byte[] result = ReedSolomonEncoder.Encode(data, ecLength, m_cacheGeneratorPoly);
        	
        	if(!PolynomialExtensions.isEqual(result, expectResult))
        		Assert.Fail("Remainder not same. result {0}, expect {1}", result.Length, expectResult.Length);
        }
        
        [Test, TestMethod]
        [TestCaseSource(typeof(RSEncoderTestCaseFactory), "TestCaseFromTxtFile")]
        public void Test_against_TXT_Dataset(byte[] data, int ecLength, byte[] expectResult)
        {
        	TestOneCase(data, ecLength, expectResult);
        }
        
        //[Test, TestMethod]
        public void Generate()
        {
        	new RSEncoderTestCaseFactory().GenerateTestDataSet();
        }
        
        
	}
}
