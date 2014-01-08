using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Formall.Web.Mvc
{
    using Formall.Navigation;
    using Formall.Persistence;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static class RouteNames
        {
            public const string StoreRouteName = "Store_Default";
            public const string StoreDetailRouteName = "Store_Detail";
            public const string DataGreedyRouteName = "Data_Greedy";
            public const string DataGroupRouteName = "Data_Group";
            public const string DataStoreRouteName = "Data_Repository";
            public const string DataRouteName = "Data_Default";

            public const string AreaSetRouteName = "Area_Set";
            public const string AreaItemRouteName = "Area_Item";
            public const string EnumSetRouteName = "Enum_Set";
            public const string EnumItemRouteName = "Enum_Item";
            public const string ModelSetRouteName = "Model_Set";
            public const string ModelItemRouteName = "Model_Item";
            public const string ModelItemInvokeRouteName = "Model_Item_Invoke";
            public const string ModelItemEntityInvokeRouteName = "Model_Item_Entity_Invoke";
            public const string ModelItemEntityItemRouteName = "Model_Item_Entity_Item";
            public const string ModelItemEntitySetRouteName = "Model_Item_Entity_Set";
            public const string StoreSetRouteName = "Store_Set";
            public const string StoreItemRouteName = "Store_Item";
            public const string TypeSetRouteName = "Type_Set";
            public const string TypeItemRouteName = "Type_Item";
            public const string ValueSetRouteName = "Value_Set";
            public const string ValueItemRouteName = "Value_Item";

            public const string DataMetadataRouteName = "Data_Metadata";
            public const string DataNameMetadataRouteName = "Data_Name_Metadata";

            public const string ExtRouteName = "Scripts_Ext";
            public const string ExtAreaRouteName = "Scripts_Area_Ext";
            public const string DataDomainRouteName = "Data_Domain";
            public const string DataRootActionRouteName = "Data_Root_Action";
            public const string DataRootIndexRouteName = "Data_Root_Children";
            public const string DataNameIndexRouteName = "Data_Name_Children";
            public const string DataEntityRouteName = "Data_Entity_Item";
            public const string DataPropertyRouteName = "Data_Property_Item";
            public const string DataEntityInvokeRouteName = "Data_Entity_Invoke";
            public const string DataTypeRouteName = "Data_Type";
            public const string DataTypeInvokeRouteName = "Data_Type_Invoke";

            public const string DefaultPathRouteName = "Default_Path";
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            ControllerBuilder.Current.SetControllerFactory(typeof(Formall.Web.Mvc.ControllerFactory));

            Seed();
        }

        private void Seed()
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

            var metadataDir = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Seeds");

            var metadataSeeds = System.IO.Directory.GetFiles(metadataDir, "*.js", System.IO.SearchOption.AllDirectories);

            foreach (var file in metadataSeeds)
            {
                context.Import(new System.IO.FileInfo(file));
            }

            Schema.Current.Load(Guid.NewGuid(), "*", domain, context);

            var number = Schema.Current.Query("Number", "*").FirstOrDefault();

            System.Diagnostics.Debug.Assert(number != null);
            System.Diagnostics.Debug.Assert(number.Name == "Number");
        }
    }
}