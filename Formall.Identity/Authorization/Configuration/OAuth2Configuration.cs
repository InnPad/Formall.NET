using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Authorization.Configuration
{
    public class OAuth2Configuration : ProtocolConfiguration
    {
        //[Display(ResourceType = typeof(Resources.Models.Configuration.OAuth2Configuration), Name = "EnableImplicitFlow", Description = "EnableImplicitFlowDescription")]
        public bool EnableImplicitFlow { get; set; }

        //[Display(ResourceType = typeof(Resources.Models.Configuration.OAuth2Configuration), Name = "EnableResourceOwnerFlow", Description = "EnableResourceOwnerFlowDescription")]
        public bool EnableResourceOwnerFlow { get; set; }

        //[Display(ResourceType = typeof(Resources.Models.Configuration.OAuth2Configuration), Name = "EnableCodeFlow", Description = "EnableCodeFlowDescription")]
        public bool EnableCodeFlow { get; set; }

        //[Display(ResourceType = typeof(Resources.Models.Configuration.OAuth2Configuration), Name = "EnableConsent", Description = "EnableConsentDescription")]
        public bool EnableConsent { get; set; }
    }
}
