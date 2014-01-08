using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Authorization.Configuration
{
    public class WSTrustConfiguration : ProtocolConfiguration
    {
        //[Display(ResourceType = typeof(Resources.Models.Configuration.WSTrustConfiguration), Name = "EnableMessageSecurity", Description = "EnableMessageSecurityDescription")]
        public bool EnableMessageSecurity { get; set; }

        //[Display(ResourceType = typeof(Resources.Models.Configuration.WSTrustConfiguration), Name = "EnableMixedModeSecurity", Description = "EnableMixedModeSecurityDescription")]
        public bool EnableMixedModeSecurity { get; set; }

        //[Display(ResourceType = typeof(Resources.Models.Configuration.WSTrustConfiguration), Name = "EnableClientCertificateAuthentication", Description = "EnableClientCertificateAuthenticationDescription")]
        public bool EnableClientCertificateAuthentication { get; set; }

        //[Display(ResourceType = typeof(Resources.Models.Configuration.WSTrustConfiguration), Name = "EnableFederatedAuthentication", Description = "EnableFederatedAuthenticationDescription")]
        public bool EnableFederatedAuthentication { get; set; }

        //[Display(ResourceType = typeof(Resources.Models.Configuration.WSTrustConfiguration), Name = "EnableDelegation", Description = "EnableDelegationDescription")]
        public Boolean EnableDelegation { get; set; }
    }
}
