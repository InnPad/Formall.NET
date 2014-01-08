using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Authentication.Tokens
{
    public class X509CertificateSessionSecurityTokenHandler : SessionSecurityTokenHandler
    {
        public X509CertificateSessionSecurityTokenHandler(X509Certificate2 protectionCertificate)
            : base(CreateTransforms(protectionCertificate))
        { }

        private static ReadOnlyCollection<CookieTransform> CreateTransforms(X509Certificate2 protectionCertificate)
        {
            var transforms = new List<CookieTransform>() 
               { 
                 new DeflateCookieTransform(), 
                 new RsaEncryptionCookieTransform(protectionCertificate),
                 new RsaSignatureCookieTransform(protectionCertificate),
               };

            return transforms.AsReadOnly();
        }
    }
}
