using System.Collections.Generic;
using Custom.Algebra.QrCode.Encoding.EncodingRegion;
using Custom.Algebra.QrCode.Encoding.Masking;
using Custom.Algebra.QrCode.Encoding.Positioning;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Custom.Algebra.QrCode.Encoding.Tests.EncodingRegion
{
	[TestClass, TestFixture]
	public class FormatInfoTest
	{
		[Test, TestMethod]
        [TestCaseSource(typeof(FormatInfoTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(int version, MaskPatternType patternType, TriStateMatrix expected)
        {
        	Test_One_Case(version, patternType,  expected);
        }
        
        [Test, TestMethod]
        [TestCaseSource(typeof(FormatInfoTestCaseFactory), "TestCasesFromTxtFile")]
        public void Test_against_DataSet(int version, MaskPatternType patternType, TriStateMatrix expected)
        {
        	Test_One_Case(version, patternType, expected);
        }
        
        private void Test_One_Case(int version, MaskPatternType patternType, TriStateMatrix expected)
        {
        	TriStateMatrix target = new TriStateMatrix(expected.Width);
            PositioninngPatternBuilder.EmbedBasicPatterns(version, target);
            PatternFactory pf = new PatternFactory();
            Pattern pt = pf.CreateByType(patternType);
            target.EmbedFormatInformation(ErrorCorrectionLevel.H, pt);
            
        	expected.AssertEquals(target);
        }
        
        //[Test, TestMethod]
        public void Generate()
        {
            new FormatInfoTestCaseFactory().GenerateMaskPatternTestDataSet();
        }
	}
}
