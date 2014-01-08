using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Authorization.Configuration
{
    public class WSFederationConfiguration : ProtocolConfiguration
    {
        //[Display(ResourceType = typeof(Resources.Models.Configuration.WSFederationConfiguration), Name = "EnableAuthentication", Description = "EnableAuthenticationDescription")]
        public bool EnableAuthentication { get; set; }

        //[Display(ResourceType = typeof(Resources.Models.Configuration.WSFederationConfiguration), Name = "EnableFederation", Description = "EnableFederationDescription")]
        public bool EnableFederation { get; set; }

        //[Display(ResourceType = typeof(Resources.Models.Configuration.WSFederationConfiguration), Name = "EnableHrd", Description = "EnableHrdDescription")]
        public bool EnableHrd { get; set; }

        //[Display(ResourceType = typeof(Resources.Models.Configuration.WSFederationConfiguration), Name = "AllowReplyTo", Description = "AllowReplyToDescription")]
        public bool AllowReplyTo { get; set; }

        //[Display(ResourceType = typeof(Resources.Models.Configuration.WSFederationConfiguration), Name = "RequireReplyToWithinRealm", Description = "RequireReplyToWithinRealmDescription")]
        public Boolean RequireReplyToWithinRealm { get; set; }

        //[Display(ResourceType = typeof(Resources.Models.Configuration.WSFederationConfiguration), Name = "RequireSslForReplyTo", Description = "RequireSslForReplyToDescription")]
        public Boolean RequireSslForReplyTo { get; set; }
    }
}
