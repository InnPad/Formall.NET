using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Custom.Security
{
    using Custom.Data.Persistence;
    using Raven.Client;

    public class DatabaseUser : IUserProvider
    {
        public IDocumentStore DocumentStore { get; set; }
        public DatabaseUser(IDocumentStore documentStore)
        {
            DocumentStore = documentStore;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier)
        {
            using (var session = DocumentStore.OpenSession())
            {
                var member = session.Query<Member>().SingleOrDefault(x => x.Identifier == identifier);

                if (member == null)
                    return null;

                return new UserIdentity
                {
                    Username = member.DisplayName,
                    Claims = new[]
                {
                    "NewUser",
                    "CanComment"
                }
                };
            }
        }
    }
}