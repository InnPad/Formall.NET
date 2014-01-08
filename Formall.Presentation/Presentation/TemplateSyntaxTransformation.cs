using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Presentation
{
    using Dictionary = Dictionary<string, object>;

    public abstract class TemplateSyntaxTransformation : IDynamicMetaObjectProvider
    {
        public static implicit operator Dictionary(TemplateSyntaxTransformation instance)
        {
            return instance._dictionary;
        }

        private Dictionary<string, object> _dictionary;

        protected TemplateSyntaxTransformation()
        {
            _dictionary = new Dictionary<string, object>();
        }

        protected TemplateSyntaxTransformation(Dictionary dictionary)
        {
            _dictionary = dictionary;
        }

        DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
        {
            return new TemplateSyntaxMetaObject(parameter, this);
        }

        public virtual object GetDefaultValue(string key)
        {
            return null;
        }

        public object GetValue(string key)
        {
            object value;
            return _dictionary.TryGetValue(key, out value) ? value : GetDefaultValue(key);
        }

        public object SetValue(string key, object value)
        {
            if (_dictionary.ContainsKey(key))
                _dictionary[key] = value;
            else
                _dictionary.Add(key, value);
            return value;
        }

        public object WriteMethodInfo(string methodInfo)
        {
            Console.WriteLine(methodInfo);
            return 42; // because it is the answer to everything
        }

        public override string ToString()
        {
            var message = new StringWriter();
            foreach (var item in _dictionary)
                message.WriteLine("{0}:\t{1}", item.Key, item.Value);
            return message.ToString();
        }

        private class TemplateSyntaxMetaObject : System.Dynamic.DynamicMetaObject
        {
            private readonly TemplateSyntaxTransformation _transformation;

            public TemplateSyntaxMetaObject(Expression expression, TemplateSyntaxTransformation value)
                : base(expression, BindingRestrictions.Empty, value)
            {
            }

            public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
            {
                // Method call in the containing class:
                string methodName = "GetValue";

                // One parameter
                Expression[] parameters = new Expression[]
            {
                Expression.Constant(binder.Name)
            };

                DynamicMetaObject getValue = new DynamicMetaObject(
                    Expression.Call(
                        Expression.Convert(Expression, LimitType),
                        typeof(TemplateSyntaxTransformation).GetMethod(methodName),
                        parameters),
                    BindingRestrictions.GetTypeRestriction(Expression, LimitType));
                return getValue;
            }

            public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
            {
                var paramInfo = new StringBuilder();
                paramInfo.AppendFormat("Calling {0}(", binder.Name);
                foreach (var item in args)
                    paramInfo.AppendFormat("{0}, ", item.Value);
                paramInfo.Append(")");

                var parameters = new Expression[]
                {
                    Expression.Constant(paramInfo.ToString())
                };

                DynamicMetaObject methodInfo = new DynamicMetaObject(
                    Expression.Call(
                    Expression.Convert(Expression, LimitType),
                    typeof(TemplateSyntaxTransformation).GetMethod("WriteMethodInfo"),
                    parameters),
                    BindingRestrictions.GetTypeRestriction(Expression, LimitType));
                return methodInfo;
            }

            public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
            {
                // Method to call in the containing class:
                string methodName = "SetValue";

                // setup the binding restrictions.
                var restrictions = BindingRestrictions.GetTypeRestriction(Expression, LimitType);

                // setup the parameters:
                var args = new Expression[2];
                // First parameter is the name of the property to Set
                args[0] = Expression.Constant(binder.Name);
                // Second parameter is the value
                args[1] = Expression.Convert(value.Expression, typeof(object));

                // Setup the 'this' reference
                var self = Expression.Convert(Expression, LimitType);

                // Setup the method call expression
                var methodCall = Expression.Call(self,
                        typeof(TemplateSyntaxTransformation).GetMethod(methodName),
                        args);

                // Create a meta object to invoke Set later:
                var setValue = new DynamicMetaObject(methodCall, restrictions);

                // return that dynamic object
                return setValue;
            }
        }
    }
}
