using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Custom.Algebra.QrCode.Encoding.Terminate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Custom.Algebra.QrCode.Encoding.Tests.Terminate
{
    using Assert = NUnit.Framework.Assert;
    using CollectionAssert = NUnit.Framework.CollectionAssert;
    using TestAttribute = NUnit.Framework.TestAttribute;
    using TestCaseAttribute = NUnit.Framework.TestCaseAttribute;
    using TestCaseSourceAttribute = NUnit.Framework.TestCaseSourceAttribute;
    using TestFixtureAttribute = NUnit.Framework.TestFixtureAttribute;

	[TestClass, TestFixture]
	public class TerminatorTest
	{
		[Test, TestMethod]
        [TestCaseSource(typeof(TerminatorTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(IEnumerable<bool> data, int numTotalByte, IEnumerable<bool> expected)
        {
        	TestOneData(data, numTotalByte, expected);
		}
        
        [Test, TestMethod]
        [TestCaseSource(typeof(TerminatorTestCaseFactory), "TestCasesFromCsvFile")]
        public void Test_against_TXT_Dataset(IEnumerable<bool> data, int numTotalByte, IEnumerable<bool> expected)
        {
        	TestOneData(data, numTotalByte, expected);
        }
        
        private void TestOneData(IEnumerable<bool> data, int numTotalByte, IEnumerable<bool> expected)
        {
        	BitList tData = new BitList();
        	tData.Add(data);
        	tData.TerminateBites(tData.Count, numTotalByte);
        	
        	IEnumerable actualResult = tData;
        	
        	CollectionAssert.AreEquivalent(expected.ToList(), actualResult);
        }
        
        //[Test, TestMethod]
        public void Generate()
        {
        	new TerminatorTestCaseFactory().GenerateTestDataSet();
        }
        
        
	}
}
