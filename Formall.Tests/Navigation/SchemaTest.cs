using System;
using System.Globalization;
using System.Linq;

namespace Formall.Navigation
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SchemaTest
    {
        [TestMethod]
        public void TestSchemaOptions()
        {
            var options = RouteOption.FromHost("data.innpad.formall.com").ToArray();

            Assert.AreEqual(options.Length, 8);

            Assert.AreEqual(options[0].Pattern, "data.innpad.formall.com");
            Assert.AreEqual(options[0].Redirect, null);

            Assert.AreEqual(options[1].Pattern, "data.innpad.*");
            Assert.AreEqual(options[1].Redirect, null);

            Assert.AreEqual(options[2].Pattern, "*.innpad.formall.com");
            Assert.AreEqual(options[2].Redirect, null);

            Assert.AreEqual(options[3].Pattern, "*.innpad.*");
            Assert.AreEqual(options[3].Redirect, null);

            Assert.AreEqual(options[4].Pattern, "*.formall.com");
            Assert.AreEqual(options[4].Redirect, "innpad.formall.com");

            Assert.AreEqual(options[5].Pattern, "*.*");
            Assert.AreEqual(options[5].Redirect, "innpad.formall.com");

            Assert.AreEqual(options[6].Pattern, "formall.com");
            Assert.AreEqual(options[6].Redirect, "formall.com");

            Assert.AreEqual(options[7].Pattern, "*");
            Assert.AreEqual(options[7].Redirect, "formall.com");

            var culture = new CultureInfo("en-US");
            var principal = "data.innpad.formall.com";
            var original = culture.Name + '.' + principal;
            options = options = RouteOption.FromHost(original).ToArray();

            Assert.AreEqual(options.Length, 8);

            Assert.AreEqual(options[0].Culture, culture);
            Assert.AreEqual(options[0].Original, original);
            Assert.AreEqual(options[0].Pattern, principal);
            Assert.AreEqual(options[0].Redirect, null);

            Assert.AreEqual(options[1].Culture, culture);
            Assert.AreEqual(options[1].Original, original);
            Assert.AreEqual(options[1].Pattern, "data.innpad.*");
            Assert.AreEqual(options[1].Redirect, null);

            Assert.AreEqual(options[2].Culture, culture);
            Assert.AreEqual(options[2].Original, original);
            Assert.AreEqual(options[2].Pattern, "*.innpad.formall.com");
            Assert.AreEqual(options[2].Redirect, null);

            Assert.AreEqual(options[3].Culture, culture);
            Assert.AreEqual(options[3].Original, original);
            Assert.AreEqual(options[3].Pattern, "*.innpad.*");
            Assert.AreEqual(options[3].Redirect, null);

            Assert.AreEqual(options[4].Culture, culture);
            Assert.AreEqual(options[4].Original, original);
            Assert.AreEqual(options[4].Pattern, "*.formall.com");
            Assert.AreEqual(options[4].Redirect, "en-US.innpad.formall.com");

            Assert.AreEqual(options[5].Culture, culture);
            Assert.AreEqual(options[5].Original, original);
            Assert.AreEqual(options[5].Pattern, "*.*");
            Assert.AreEqual(options[5].Redirect, "en-US.innpad.formall.com");

            Assert.AreEqual(options[6].Culture, culture);
            Assert.AreEqual(options[6].Original, original);
            Assert.AreEqual(options[6].Pattern, "formall.com");
            Assert.AreEqual(options[6].Redirect, "en-US.formall.com");

            Assert.AreEqual(options[7].Culture, culture);
            Assert.AreEqual(options[7].Original, original);
            Assert.AreEqual(options[7].Pattern, "*");
            Assert.AreEqual(options[7].Redirect, "en-US.formall.com");
        }
    }
}
