﻿using System;
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
	public class PolyXorMultyTest
	{
		private const int s_Primitive = 0x011D;
		
		[Test, TestMethod]
        [TestCaseSource(typeof(PolyXorMultyTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(int[] aCoeff, int[] bCoeff, string option, int[] expect)
        {
        	TestOneCase(aCoeff, bCoeff, option, expect);
        }
        
        private int[] resultCoeff(Polynomial apoly, Polynomial bpoly, string option)
        {
        	switch(option)
        	{
        		case "xor":
        			return apoly.AddOrSubtract(bpoly).Coefficients;
        		case "multy":
        			return apoly.Multiply(bpoly).Coefficients;
        		default:
        			throw new ArgumentException("No such math option for polynomial");
        	}
        }
        
        private void TestOneCase(int[] aCoeff, int[] bCoeff, string option, int[] expect)
        {
        	GaloisField256 gfield = GaloisField256.QRCodeGaloisField;
        	Polynomial apoly = new Polynomial(gfield, aCoeff);
        	Polynomial bpoly = new Polynomial(gfield, bCoeff);
        	
        	int[] result = resultCoeff(apoly, bpoly, option);
        	
        	if(!PolynomialExtensions.isEqual(result, expect))
        		Assert.Fail("result {0} expect {1} option {2}", result[0], expect[0], option);
        }
        
        [Test, TestMethod]
        [TestCaseSource(typeof(PolyXorMultyTestCaseFactory), "TestCaseFromTxtFile")]
        public void Test_against_TXT_Dataset(int[] aCoeff, int[] bCoeff, string option, int[] expect)
        {
        	TestOneCase(aCoeff, bCoeff, option, expect);
        }
        
        //[Test, TestMethod]
        public void Generate()
        {
        	new PolyXorMultyTestCaseFactory().GenerateTestDataSet();
        }
        
	}
}
