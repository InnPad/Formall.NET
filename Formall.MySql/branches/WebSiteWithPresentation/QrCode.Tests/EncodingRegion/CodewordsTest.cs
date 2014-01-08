using System.Collections.Generic;
using Custom.Algebra.QrCode.Encoding.EncodingRegion;
using Custom.Algebra.QrCode.Encoding.Positioning;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Custom.Algebra.QrCode.Encoding.Tests.EncodingRegion
{
	[TestClass, TestFixture]
	public class CodewordsTest
	{
		[Test, TestMethod]
        [TestCaseSource(typeof(CodewordsTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(int version, TriStateMatrix expected, IEnumerable<bool> codewords)
        {
        	Test_One_Case(version, expected, codewords);
        }
        
        [Test, TestMethod]
        [TestCaseSource(typeof(CodewordsTestCaseFactory), "TestCasesFromTxtFile")]
        public void Test_against_DataSet(int version, TriStateMatrix expected, IEnumerable<bool> codewords)
        {
        	Test_One_Case(version, expected, codewords);
        }
        
        private void Test_One_Case(int version, TriStateMatrix expected, IEnumerable<bool> codewords)
        {
        	BitList dcList = new BitList();
        	dcList.Add(codewords);
        	
        	TriStateMatrix target = new TriStateMatrix(expected.Width);
            PositioninngPatternBuilder.EmbedBasicPatterns(version, target);
            target.TryEmbedCodewords(dcList);
            
        	expected.AssertEquals(target);
        }
        
        //[Test, TestMethod]
        public void Generate()
        {
            new CodewordsTestCaseFactory().GenerateMaskPatternTestDataSet();
        }
        
	}
}
