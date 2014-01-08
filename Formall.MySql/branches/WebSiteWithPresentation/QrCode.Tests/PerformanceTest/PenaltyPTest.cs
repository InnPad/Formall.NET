using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.google.zxing.qrcode.encoder;
using Custom.Algebra.QrCode.Encoding.Masking.Scoring;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Custom.Algebra.QrCode.Encoding.Tests.PerformanceTest
{
    using Assert = NUnit.Framework.Assert;
    using CollectionAssert = NUnit.Framework.CollectionAssert;
    using TestAttribute = NUnit.Framework.TestAttribute;
    using TestCaseAttribute = NUnit.Framework.TestCaseAttribute;
    using TestCaseData = NUnit.Framework.TestCaseData;
    using TestCaseSourceAttribute = NUnit.Framework.TestCaseSourceAttribute;
    using TestFixtureAttribute = NUnit.Framework.TestFixtureAttribute;

	[TestClass, TestFixture]
	public class PenaltyPTest
	{
		 [Test, TestMethod]
        [TestCaseSource(typeof(PenaltyPTest), "ReferenceTestCase")]
		public void Penalty1PTest(ByteMatrix bMatrix, BitMatrix bitMatrix)
		{
			PerformanceTest(1, bMatrix, bitMatrix);
		}
		
		[Test, TestMethod]
        [TestCaseSource(typeof(PenaltyPTest), "ReferenceTestCase")]
		public void Penalty2PTest(ByteMatrix bMatrix, BitMatrix bitMatrix)
		{
			PerformanceTest(2, bMatrix, bitMatrix);
		}
		
		[Test, TestMethod]
        [TestCaseSource(typeof(PenaltyPTest), "ReferenceTestCase")]
		public void Penalty3PTest(ByteMatrix bMatrix, BitMatrix bitMatrix)
		{
			PerformanceTest(3, bMatrix, bitMatrix);
		}
		
		[Test, TestMethod]
        [TestCaseSource(typeof(PenaltyPTest), "ReferenceTestCase")]
		public void Penalty4PTest(ByteMatrix bMatrix, BitMatrix bitMatrix)
		{
			PerformanceTest(4, bMatrix, bitMatrix);
		}
		
		public void PerformanceTest(int rules, ByteMatrix bMatrix, BitMatrix bitMatrix)
		{
			Stopwatch sw = new Stopwatch();
			int timesofTest = 1000;
			
			Penalty penalty = new PenaltyFactory().CreateByRule((PenaltyRules)rules);
			
			
			string[] timeElapsed = new string[2];
			
			sw.Start();
			
			for(int i = 0; i < timesofTest; i++)
			{
				penalty.PenaltyCalculate(bitMatrix);
			}
			
			sw.Stop();
			
			timeElapsed[0] = sw.ElapsedMilliseconds.ToString();
			
			sw.Reset();
			
			sw.Start();
			
			for(int i = 0; i < timesofTest; i++)
			{
				switch(rules)
				{
					case 1:
						MaskUtil.applyMaskPenaltyRule1(bMatrix);
						break;
					case 2:
						MaskUtil.applyMaskPenaltyRule2(bMatrix);
						break;
					case 3:
						MaskUtil.applyMaskPenaltyRule3(bMatrix);
						break;
					case 4:
						MaskUtil.applyMaskPenaltyRule4(bMatrix);
						break;
					default:
						throw new InvalidOperationException(string.Format("Unsupport Rules {0}", rules.ToString()));
				}
			}
			sw.Stop();
			
			timeElapsed[1] = sw.ElapsedMilliseconds.ToString();
			
			
			Assert.Pass("Terminator performance {0} Tests~ QrCode.Net: {1} ZXing: {2}", timesofTest, timeElapsed[0], timeElapsed[1]);
			
		}
		
		
		private IEnumerable<TestCaseData> ReferenceTestCase
		{
			get
			{
				ByteMatrix bMatrix;
				BitMatrix bitMatrix = GetOriginal(120, new Random(), out bMatrix);
				yield return new TestCaseData(bMatrix, bitMatrix);
			}
		}
		
		
		internal BitMatrix GetOriginal(int matrixSize, Random randomizer, out ByteMatrix matrix)
        {
            matrix = new ByteMatrix(matrixSize, matrixSize);

            FillRandom(matrix, randomizer);
            return matrix.ToBitMatrix();
        }

        private void FillRandom(ByteMatrix matrix, Random randomizer)
        {
            for (int i = 0; i < matrix.Width; i++)
            {
                for (int j = 0; j < matrix.Height; j++)
                {
                    var randomValue = randomizer.Next(0, 2);
                    matrix[i, j] = (sbyte)randomValue;
                }
            }
        }
	}
}
