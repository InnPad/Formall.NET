using Custom.Algebra.QrCode.Encoding.Masking.Scoring;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Custom.Algebra.QrCode.Encoding.Tests.PenaltyScore
{
	[TestClass, TestFixture]
	public class Penalty2Test : PenaltyTestBase
	{
		[Test, TestMethod]
        [TestCaseSource(typeof(Penalty2TestCaseFactory), "TestCasesFromReferenceImplementation")]
		public override void Test_against_reference_implementation(BitMatrix input, PenaltyRules penaltyRule, int expected)
		{
			base.Test_against_reference_implementation(input, penaltyRule, expected);
		}
		
		[Test, TestMethod]
        [TestCaseSource(typeof(Penalty2TestCaseFactory), "TestCasesFromTxtFile")]
        public void Test_against_DataSet(BitMatrix input, PenaltyRules penaltyRule, int expected)
        {
            base.Test_against_reference_implementation(input, penaltyRule, expected);
        }
		
		//[Test, TestMethod]
        public void Generate()
        {
        	new Penalty2TestCaseFactory().GenerateTestDataSet();
        }
	}
}
