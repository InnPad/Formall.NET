using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Authentication.Tokens
{
    using Formall.Diagnostics;

    class ClientCertificateIssuerNameRegistry : IssuerNameRegistry
    {
        public override string GetIssuerName(SecurityToken securityToken)
        {
            if (securityToken == null)
            {
                Tracing.Error("ClientCertificateIssuerNameRegistry: securityToken is null");
                throw new ArgumentNullException("securityToken");
            }

            X509SecurityToken token = securityToken as X509SecurityToken;
            if (token != null)
            {
                Tracing.Verbose("ClientCertificateIssuerNameRegistry: X509 SubjectName: " + token.Certificate.SubjectName.Name);
                Tracing.Verbose("ClientCertificateIssuerNameRegistry: X509 Thumbprint : " + token.Certificate.Thumbprint);
                return token.Certificate.Thumbprint;
            }

            return null;
        }
    }
}
