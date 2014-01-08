
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Custom.Algebra.QrCode.Encoding.Tests.Alignment
{
    using Custom.Algebra.QrCode.Encoding.Positioning;
    using Custom.Algebra.QrCode.Encoding.Tests.Positioning.TestCases;

    [TestClass, TestFixture]
    public class PositioningPatternsTest
    {

        [Test, TestMethod]
        [TestCaseSource(typeof(PositioningPatternsTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(int version, TriStateMatrix expected)
        {
      
            TriStateMatrix target = new TriStateMatrix(expected.Width);
            PositioninngPatternBuilder.EmbedBasicPatterns(version, target);
            expected.AssertEquals(target);
        }

        [Test, TestMethod]
        [TestCaseSource(typeof(PositioningPatternsTestCaseFactory), "TestCasesFromTxtFile")]
        public void Test_against_DataSet(int version, TriStateMatrix expected)
        {
            TriStateMatrix target = new TriStateMatrix(expected.Width);
            PositioninngPatternBuilder.EmbedBasicPatterns(version, target);
            expected.AssertEquals(target);
        }

        //[Test, TestMethod]
        public void Generate()
        {
            new PositioningPatternsTestCaseFactory().GenerateMaskPatternTestDataSet();
        }
    }
}
