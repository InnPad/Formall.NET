using System;
using System.IO;
using com.google.zxing.qrcode.encoder;
using Custom.Algebra.QrCode.Encoding.Masking;
using Custom.Algebra.QrCode.Encoding.Positioning;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Custom.Algebra.QrCode.Encoding.Tests.Masking
{
    [TestClass, TestFixture]
    public class MaskPatternTest
    {

        [Test, TestMethod]
        [TestCaseSource(typeof(MaskPatternTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(TriStateMatrix input, MaskPatternType patternType, BitMatrix expected)
        {
            Pattern pattern = new PatternFactory().CreateByType(patternType);

            BitMatrix result = input.Apply(pattern, ErrorCorrectionLevel.H);

            expected.AssertEquals(result);
        }

        [Test, TestMethod]
        [TestCaseSource(typeof(MaskPatternTestCaseFactory), "TestCasesFromTxtFile")]
        public void Test_against_DataSet(TriStateMatrix input, MaskPatternType patternType, BitMatrix expected)
        {
            Pattern pattern = new PatternFactory().CreateByType(patternType);

            BitMatrix result = input.Apply(pattern, ErrorCorrectionLevel.H);

            expected.AssertEquals(result);
        }

        //[Test, TestMethod]
        public void Generate()
        {
            new MaskPatternTestCaseFactory().GenerateMaskPatternTestDataSet();
        }
    }
}
