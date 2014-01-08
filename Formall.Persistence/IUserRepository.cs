using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Formall
{
    public interface IUserRepository
    {
        bool ValidateUser(string userName, string password);

        bool ValidateUser(X509Certificate2 clientCertificate, out string userName);

        IEnumerable<string> GetRoles(string userName);
    }
}
