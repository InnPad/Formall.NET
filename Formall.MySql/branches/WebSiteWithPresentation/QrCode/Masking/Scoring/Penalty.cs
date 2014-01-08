namespace Custom.Algebra.QrCode.Encoding.Masking.Scoring
{
	public abstract class Penalty
	{
		internal abstract int PenaltyCalculate(BitMatrix matrix);
	}
}
