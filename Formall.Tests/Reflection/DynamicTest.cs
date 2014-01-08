using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Dynamic;

namespace Formall.Reflection
{
    [TestClass]
    public class DynamicTest
    {
        [TestMethod]
        public void TestDynamicValueGetter()
        {
            dynamic obj = new System.Dynamic.ExpandoObject();
            obj.somePropertyName = "someValue";

            var provider = obj as IDynamicMetaObjectProvider;
            var value = provider.Value("somePropertyName");
            if (!object.Equals(value, "someValue"))
            {
                throw new Exception();
            }
        }

        [TestMethod]
        public void TestDynamicValueSetter()
        {
            dynamic obj = new System.Dynamic.ExpandoObject();
            var provider = obj as IDynamicMetaObjectProvider;
            
            provider.Value("somePropertyName", "someValue");
            var value = obj.somePropertyName;

            if (!object.Equals(value, "someValue"))
            {
                throw new Exception();
            }
        }

        [TestMethod]
        public void TestDynamicTwoLevelValueGetter()
        {
            dynamic obj1 = new System.Dynamic.ExpandoObject();
            dynamic obj2 = new System.Dynamic.ExpandoObject();
            obj1.somePropertyName = obj2;
            obj1.somePropertyName.somePropertyName = "someValue";

            var provider = obj1 as IDynamicMetaObjectProvider;
            var value = ((obj1 as IDynamicMetaObjectProvider).Value("somePropertyName") as IDynamicMetaObjectProvider).Value("somePropertyName");
            if (!object.Equals(value, "someValue"))
            {
                throw new Exception();
            }
        }

        [TestMethod]
        public void TestDynamicTwoLevelValueSetter()
        {
            dynamic obj1 = new System.Dynamic.ExpandoObject();
            dynamic obj2 = new System.Dynamic.ExpandoObject();
            
            (obj1 as IDynamicMetaObjectProvider).Value("somePropertyName", (object)obj2);
            (obj2 as IDynamicMetaObjectProvider).Value("somePropertyName", "someValue");
            var value = obj1.somePropertyName.somePropertyName;

            if (!object.Equals(value, "someValue"))
            {
                throw new Exception();
            }
        }
    }
}
