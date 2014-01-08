using System;
using System.Linq;

namespace Formall.Persistence
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Formall.Navigation;

    [TestClass]
    public class RavenDocumentContextTest
    {
        [TestMethod]
        public void Test_RavenDocumentContext_Import_Seeds()
        {
            var schema = Schema.Current;
            var context = new RavenDocumentContext("*", schema, new
                {
                    //ApiKey = string.Empty,
                    /*ApiKeys = new []
                    {
                        new { }
                    },*/
                    Embeddable = true,
                    DataDirectory = "~/App_Data/Base",
                    Name = "Base",
                    /*Replication = new {
                        Id = "Raven/Replication/Destinations",
                        Destinations = new []
                        {
                            new
                            {
                                ApiKey = string.Empty,
                                ClientVisibleUrl = string.Empty,
                                Database = string.Empty,
                                Disabled = false,
                                Domain = string.Empty,
                                IgnoredClient = string.Empty,
                                Password = string.Empty,
                                TransitiveReplicationBehavior = "Replicate",
                                Url = string.Empty,
                                Username = string.Empty
                            }
                        },
                        Source = string.Empty
                    },*/
                    //Secret = string.Empty,
                    //Url = string.Empty
                });

            var domain = new Domain
            {
                Culture = "en-US",
                Pattern = "*"
            };

            var metadataDir = new System.IO.DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory)
                .Parent
                .Parent
                .Parent
                .GetDirectories("Formall.Web.Mvc.Server")[0]
                .GetDirectories("App_Data")[0]
                .GetDirectories("Seeds")[0];
            
            var metadataSeeds = metadataDir.GetFiles("*.js");

            foreach (var file in metadataSeeds)
            {
                context.Import(new System.IO.FileInfo(file.FullName));
            }

            Schema.Current.Load(Guid.NewGuid(), "*", domain, context);

            var number = Schema.Current.Query("Number", "*").FirstOrDefault();

            Assert.AreNotEqual(number, null);
            Assert.AreEqual(number.Name, "Number");
        }

        public void Import()
        {

        }
    }
}
