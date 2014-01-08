using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Authorization.Configuration
{
    public class KeyMaterialConfiguration
    {
        //[Display(ResourceType = typeof(Resources.Models.Configuration.KeyMaterialConfiguration), Name = "SigningCertificate", Description = "SigningCertificateDescription")]
        //[Required]
        public X509Certificate2 SigningCertificate { get; set; }

        //[Display(ResourceType = typeof(Resources.Models.Configuration.KeyMaterialConfiguration), Name = "DecryptionCertificate", Description = "DecryptionCertificateDescription")]
        public X509Certificate2 DecryptionCertificate { get; set; }

        //[Display(Name = "RSA Signing Key", Description = "The RSA key to sign outgoing JWT tokens")]
        //public RSA RSASigningKey { get; set; }

        //[Display(ResourceType = typeof(Resources.Models.Configuration.KeyMaterialConfiguration), Name = "SymmetricSigningKey", Description = "SymmetricSigningKeyDescription")]
        //[Required]
        public string SymmetricSigningKey { get; set; }
    }
}
