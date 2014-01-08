using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Authorization
{
    public class Client : IValidatable
    {
        public Guid Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Secret
        {
            get;
            set;
        }

        public Text Summary
        {
            get;
            set;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            /*if (!HasClientSecret &&
                String.IsNullOrWhiteSpace(this.ClientSecret) &&
                (this.AllowCodeFlow || this.AllowResourceOwnerFlow))
            {
                errors.Add(new ValidationResult(Resources.Models.Client.ClientSecretRequiredError, new string[] { "ClientSecret" }));
            }

            if (this.RedirectUri == null &&
                (this.AllowCodeFlow || this.AllowImplicitFlow))
            {
                errors.Add(new ValidationResult(Resources.Models.Client.RedirectUriRequiredError, new string[] { "RedirectUri" }));
            }

            if (this.RedirectUri != null && this.RedirectUri.Scheme == Uri.UriSchemeHttp)
            {
                errors.Add(new ValidationResult(Resources.Models.Client.RedirectUriMustBeHTTPS, new string[] { "RedirectUri" }));
            }

            if (!this.AllowCodeFlow && !this.AllowResourceOwnerFlow && this.AllowRefreshToken)
            {
                errors.Add(new ValidationResult("Refresh tokens only allowed with Code or Resource Owner flows.", new string[] { "AllowRefreshToken" }));
            }*/

            return errors;
        }
    }
}
