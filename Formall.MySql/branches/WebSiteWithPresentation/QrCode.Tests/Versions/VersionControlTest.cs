using Custom.Algebra.QrCode.Encoding.DataEncodation;
using Custom.Algebra.QrCode.Encoding.Tests.Versions.TestCases;
using Custom.Algebra.QrCode.Encoding.Versions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Custom.Algebra.QrCode.Encoding.Tests.Versions
{
    using Assert = NUnit.Framework.Assert;
    using CollectionAssert = NUnit.Framework.CollectionAssert;
    using TestAttribute = NUnit.Framework.TestAttribute;
    using TestCaseAttribute = NUnit.Framework.TestCaseAttribute;
    using TestCaseSourceAttribute = NUnit.Framework.TestCaseSourceAttribute;
    using TestFixtureAttribute = NUnit.Framework.TestFixtureAttribute;

	[TestClass, TestFixture]
	public class VersionControlTest
	{	
		[Test, TestMethod]
        [TestCaseSource(typeof(VersionControlTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(int numDataBits,  Mode mode, ErrorCorrectionLevel level, string encodingName)
        {
        	VersionControlStruct vcStruct = VersionControl.InitialSetup(numDataBits, mode, level, encodingName);
        	VersionCheckStatus checkStatus = VersionTest.VersionCheck(vcStruct.VersionDetail.Version, numDataBits, mode, level, encodingName);
        	
        	switch(checkStatus)
        	{
        		case VersionCheckStatus.LargerThanExpect:
        			Assert.Fail("Version {0} size not enough", vcStruct.VersionDetail.Version);
        			break;
        		case VersionCheckStatus.SmallerThanExpect:
        			Assert.Fail("Version{0}'s size too big", vcStruct.VersionDetail.Version);
        			break;
        		default:
        			break;
        	}
       
        }
        
        
        [Test, TestMethod]
        [TestCaseSource(typeof(VersionControlTestCaseFactory), "TestCasesFromCsvFile")]
        public void Test_against_CSV_Dataset(int numDataBits,  Mode mode, ErrorCorrectionLevel level, string encodingName, int expectVersionNum)
        {
        	VersionControlStruct vcStruct = VersionControl.InitialSetup(numDataBits, mode, level, encodingName);
        	
        	if(vcStruct.VersionDetail.Version != expectVersionNum)
        		Assert.Fail("Method return version number: {0} Expect value: {1}", vcStruct.VersionDetail.Version, expectVersionNum);
        }
        
        //[Test, TestMethod]
        public void Generate()
        {
        	new VersionControlTestCaseFactory().GenerateTestDataSet();
        }
        
	}
}
