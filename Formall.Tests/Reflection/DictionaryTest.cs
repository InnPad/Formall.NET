using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Formall.Reflection
{
    [TestClass]
    public class DictionaryTest
    {
        [TestMethod]
        public void TestDictionaryValueGetter()
        {
            var dictionary = new Dictionary<string, object>();
            var getter = dictionary.Getter("rightPropertyName");
            dictionary["rightPropertyName"] = "someValue";

            var value = dictionary.Value("wrongPropertyName") ?? getter.DynamicInvoke(dictionary);
            if (!object.Equals(value, "someValue"))
            {
                throw new Exception();
            }
        }

        [TestMethod]
        public void TestDictionaryValueSetter()
        {
            var dictionary = new Dictionary<string, object>();
            var setter = dictionary.Setter("somePropertyName");
            setter.DynamicInvoke(dictionary, "someValue");
            
            var value = dictionary["somePropertyName"];

            if (!object.Equals(value, "someValue"))
            {
                throw new Exception();
            }
        }
    }
}
