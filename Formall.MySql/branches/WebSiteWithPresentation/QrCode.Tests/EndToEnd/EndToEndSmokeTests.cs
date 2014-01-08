using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Custom.Algebra.QrCode.Encoding.Tests
{
    [TestClass, TestFixture]
    public class EndToEndSmokeTests
    {

        [Test, TestMethod]
        [TestCaseSource(typeof(EndToEndSmokeTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(string inputData, ErrorCorrectionLevel errorCorrectionLevel, BitMatrix expectedMatrix)
        {
            QrEncoder encoder = new QrEncoder(errorCorrectionLevel);
            BitMatrix resultMatrix = encoder.Encode(inputData).Matrix;
            expectedMatrix.AssertEquals(resultMatrix);
        }

        [Test, TestMethod]
        [TestCaseSource(typeof(EndToEndSmokeTestCaseFactory), "TestCasesFromCsvFile")]
        public void Test_against_csv_DataSet(string inputData, ErrorCorrectionLevel errorCorrectionLevel, BitMatrix expectedMatrix)
        {
            QrEncoder encoder = new QrEncoder(errorCorrectionLevel);
            BitMatrix resultMatrix = encoder.Encode(inputData).Matrix;
            expectedMatrix.AssertEquals(resultMatrix);
        }
        
        //[Test, TestMethod]
        public void Generate()
        {
        	new EndToEndSmokeTestCaseFactory().RecordToFile();
        }
    }
}
