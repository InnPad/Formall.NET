using System;
using Custom.Algebra.QrCode.Encoding.Tests.Versions.TestCases;
using Custom.Algebra.QrCode.Encoding.Versions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Custom.Algebra.QrCode.Encoding.Tests.Versions
{
    using Assert = NUnit.Framework.Assert;
    using CollectionAssert = NUnit.Framework.CollectionAssert;
    using TestAttribute = NUnit.Framework.TestAttribute;
    using TestCaseAttribute = NUnit.Framework.TestCaseAttribute;
    using TestCaseSourceAttribute = NUnit.Framework.TestCaseSourceAttribute;
    using TestFixtureAttribute = NUnit.Framework.TestFixtureAttribute;
	
	[TestClass, TestFixture]
	public class VersionTableTests
	{
		private const string s_AssertFormate = "{0}: {1}, Expect: {2};";
		
		[Test, TestMethod]
        [TestCaseSource(typeof(VersionTableTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(int versionNum, int totalCodewords, int level, int numECCodewords, string ecBlockString)
        {
        	this.TestOneDataRow(versionNum, totalCodewords, level, numECCodewords, ecBlockString);
        }
        
        [Test, TestMethod]
        [TestCaseSource(typeof(VersionTableTestCaseFactory), "TestCasesFromCsvFile")]
        public void Test_against_CSV_Dataset(int versionNum, int totalCodewords, int level, int numECCodewords, string ecBlockString)
        {
        	this.TestOneDataRow(versionNum, totalCodewords, level, numECCodewords, ecBlockString);
        }
        
        public void TestOneDataRow(int versionNum, int totalCodewords, int level, int numECCodewords, string ecBlockString)
        {
        	VersionTableTestProperties properties = VersionTableTest.GetVersionInfo(versionNum, (ErrorCorrectionLevel)level);
        	
        	string failResult = "";
        	
        	failResult = properties.TotalNumOfCodewords == totalCodewords ? failResult 
        		: string.Join(" ", failResult, string.Format(s_AssertFormate, "TotalCodewords", properties.TotalNumOfCodewords, totalCodewords));
        	
        	failResult = properties.NumOfECCodewords == numECCodewords ? failResult 
        		: string.Join(" ", failResult, string.Format(s_AssertFormate, "NumErrorCorrectionCodeWords", properties.NumOfECCodewords, numECCodewords));
        	
        	failResult = properties.ECBlockString == ecBlockString ? failResult 
        		: string.Join(" ", failResult, string.Format(s_AssertFormate, "ECBlockString", properties.ECBlockString, ecBlockString));
        	
        	if(failResult != "")
        		Assert.Fail(failResult);
        }
        
        //[Test, TestMethod]
        public void Generate()
        {
        	new VersionTableTestCaseFactory().GenerateTestDataSet();
        }
	}
}
