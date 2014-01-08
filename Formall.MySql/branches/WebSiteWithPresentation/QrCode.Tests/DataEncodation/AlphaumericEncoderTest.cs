using System.Collections.Generic;
using Custom.Algebra.QrCode.Encoding.DataEncodation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Custom.Algebra.QrCode.Encoding.Tests.DataEncodation
{
    [TestClass, TestFixture]
    public class AlphaumericEncoderTest : EncoderTestBase
    {
        [Test, TestMethod] [TestCaseSource(typeof(AlphanumericEncoderTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public override void Test_against_reference_implementation(string inputString, IEnumerable<bool> expected)
        {
            base.Test_against_reference_implementation(inputString, expected);
        }

        [Test, TestMethod] [TestCaseSource(typeof(AlphanumericEncoderTestCaseFactory), "TestCasesFromCsvFile")]
        public override void Test_against_csv_DataSet(string inputString, IEnumerable<bool> expected)
        {
            base.Test_against_csv_DataSet(inputString, expected);
        }
        
        [Test, TestMethod] [TestCaseSource(typeof(AlphanumericEncoderTestCaseFactory), "TestCasesDataEncodeReferenceImplementation")]
        public override void DataEncode_Test_against_reference_DataSet(string inputString, IEnumerable<bool> expected)
        {
            base.DataEncode_Test_against_reference_DataSet(inputString, expected);
        }
        
        [Test, TestMethod] [TestCaseSource(typeof(AlphanumericEncoderTestCaseFactory), "TestCasesDataEncodeFromCsvFile")]
        public override void DataEncode_Test_against_csv_DataSet(string inputString, IEnumerable<bool> expected)
        {
            base.DataEncode_Test_against_reference_DataSet(inputString, expected);
        }

        protected override EncoderBase CreateEncoder()
        {
            return new AlphanumericEncoder();
        }
        
        //[Test, TestMethod]
        public void Generate()
        {
            new AlphanumericEncoderTestCaseFactory().GenerateTestDataSet("encoder");
        }
        
        //[Test, TestMethod]
        public void DataEncodeGenerate()
        {
            new AlphanumericEncoderTestCaseFactory().GenerateTestDataSet("dataencode");
        }
    }
}
