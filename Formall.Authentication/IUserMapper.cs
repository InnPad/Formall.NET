using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Custom.Authentication
{
    /// <summary>
    /// Provides a mapping between forms auth guid identifiers and
    /// real usernames
    /// </summary>
    public interface IUserProvider
    {
        /// <summary>
        /// Get the real username from an identifier
        /// </summary>
        /// <param name="identifier">User identifier</param>
        /// <returns>Matching populated IUserIdentity object, or empty</returns>
        IUserIdentity GetUserFromIdentifier(Guid identifier);
    }
}