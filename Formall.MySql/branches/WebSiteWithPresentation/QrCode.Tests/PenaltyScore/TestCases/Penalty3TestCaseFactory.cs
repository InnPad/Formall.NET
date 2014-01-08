﻿using NUnit.Framework;
using com.google.zxing.qrcode.encoder;
using Custom.Algebra.QrCode.Encoding.Masking;
using Custom.Algebra.QrCode.Encoding.Masking.Scoring;


namespace Custom.Algebra.QrCode.Encoding.Tests.PenaltyScore
{
	public class Penalty3TestCaseFactory : PenaltyScoreTestCaseFactory
	{
		protected override string TxtFileName { get { return "Penalty3TestDataSet.txt"; } }
		
		protected override NUnit.Framework.TestCaseData GenerateRandomTestCaseData(int matrixSize, System.Random randomizer, MaskPatternType pattern)
		{
			return base.GenerateRandomTestCaseData(matrixSize, randomizer, pattern, PenaltyRules.Rule03);
		}
		
	}
}