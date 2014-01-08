using System;
using Custom.Algebra.QrCode.Encoding.DataEncodation;
using Custom.Algebra.QrCode.Encoding.ErrorCorrection;
using Custom.Algebra.QrCode.Encoding.Positioning;
using Custom.Algebra.QrCode.Encoding.EncodingRegion;
using Custom.Algebra.QrCode.Encoding.Masking;
using Custom.Algebra.QrCode.Encoding.Masking.Scoring;

namespace Custom.Algebra.QrCode.Encoding
{
	internal static class QRCodeEncode
	{
		internal static BitMatrix Encode(string content, ErrorCorrectionLevel errorLevel)
		{
			EncodationStruct encodeStruct = DataEncode.Encode(content, errorLevel);
			
			BitList codewords = ECGenerator.FillECCodewords(encodeStruct.DataCodewords, encodeStruct.VersionDetail);
			
			TriStateMatrix triMatrix = new TriStateMatrix(encodeStruct.VersionDetail.MatrixWidth);
			PositioninngPatternBuilder.EmbedBasicPatterns(encodeStruct.VersionDetail.Version, triMatrix);
			
			triMatrix.EmbedVersionInformation(encodeStruct.VersionDetail.Version);
			triMatrix.EmbedFormatInformation(errorLevel, new Pattern0());
			triMatrix.TryEmbedCodewords(codewords);
			
			return triMatrix.GetLowestPenaltyMatrix(errorLevel);
			
		}
		
	}
}
