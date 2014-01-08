using System;
using System.Collections.Generic;
using Custom.Algebra.QrCode.Encoding.DataEncodation;
using Custom.Algebra.QrCode.Encoding.ErrorCorrection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Custom.Algebra.QrCode.Encoding.Tests.ErrorCorrection
{
	[TestClass, TestFixture]
	public class CodewordsBitListTest
	{
		[Test, TestMethod]
        [TestCaseSource(typeof(CodewordsBitListTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(string inputString, ErrorCorrectionLevel eclevel, IEnumerable<bool> expected)
        {
        	TestOneCase(inputString, eclevel, expected);
        }
        
        private void TestOneCase(string inputString, ErrorCorrectionLevel eclevel, IEnumerable<bool> expected)
        {
        	EncodationStruct encodeStruct = DataEncode.Encode(inputString, eclevel);
			
			IEnumerable<bool> actualResult = ECGenerator.FillECCodewords(encodeStruct.DataCodewords, encodeStruct.VersionDetail);
        	
        	BitVectorTestExtensions.CompareIEnumerable(actualResult, expected, "string");
        }
	}
}
