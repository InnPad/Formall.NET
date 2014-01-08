using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Custom.Authentication
{
    /// <summary>
    /// Defines the core functionality of an identity
    /// </summary>
    public interface IUserIdentity
    {
        /// <summary>
        /// The username of the authenticated user.
        /// </summary>
        /// <value>A <see cref="string"/> containing the username.</value>
        string Username { get; }

        /// <summary>
        /// The claims of the authenticated user.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/>, containing the claims.</value>
        IEnumerable<string> Claims { get; }
    }
}