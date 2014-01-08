using Custom.Algebra.QrCode.Encoding.Versions;

namespace Custom.Algebra.QrCode.Encoding.DataEncodation
{
	internal struct EncodationStruct
	{
		internal VersionDetail VersionDetail { get; set; }
		internal Mode Mode { get; set; }
		internal BitList DataCodewords { get; set;}
		
		internal EncodationStruct(VersionControlStruct vcStruct)
			: this()
		{
			this.VersionDetail = vcStruct.VersionDetail;
		}
	}
}
