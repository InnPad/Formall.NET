using System;
using System.Collections.Generic;
using Custom.Algebra.QrCode.Encoding.ErrorCorrection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Custom.Algebra.QrCode.Encoding.Tests.ErrorCorrection
{
	[TestClass, TestFixture]
	public class ECTest
	{
		[Test, TestMethod]
        [TestCaseSource(typeof(ECTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(IEnumerable<bool> dataCodewords, VersionDetail vc, IEnumerable<bool> expected)
        {
        	TestOneCase(dataCodewords, vc, expected);
        }
        
        [Test, TestMethod]
        [TestCaseSource(typeof(ECTestCaseFactory), "TestCaseFromTxtFile")]
        public void Test_against_TXT_Dataset(IEnumerable<bool> dataCodewords, VersionDetail vc, IEnumerable<bool> expected)
        {
        	TestOneCase(dataCodewords, vc, expected);
        }
        
        private void TestOneCase(IEnumerable<bool> dataCodewords, VersionDetail vc, IEnumerable<bool> expected)
        {
        	BitList dcList = new BitList();
        	dcList.Add(dataCodewords);
        	
        	IEnumerable<bool> actualResult = ECGenerator.FillECCodewords(dcList, vc);
        	BitVectorTestExtensions.CompareIEnumerable(actualResult, expected, "string");
        }
        
        //[Test, TestMethod]
        public void Generate()
        {
        	new ECTestCaseFactory().GenerateTestDataSet();
        }
	}
}
