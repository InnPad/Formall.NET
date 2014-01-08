using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Reflection
{
    public static class DynamicExtensions
    {
        public static Delegate Getter(this IDictionary<string, object> dictionary, string name)
        {
            return (Func<IDictionary<string, object>, dynamic>)((IDictionary<string, object> o) => { return Value(o, name); });
        }

        public static Delegate Getter(this IDynamicMetaObjectProvider provider, string name)
        {
            var scope = provider.GetType();
            var parameter = Expression.Parameter(typeof(object));
            var metaObject = provider.GetMetaObject(parameter);
            var binder = (GetMemberBinder)Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, name, scope, new[] { Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(0, null) });
            var operation = metaObject.BindGetMember(binder);
            var block = Expression.Block(
                Expression.Label(CallSiteBinder.UpdateLabel),
                operation.Expression
            );
            var lambda = Expression.Lambda(block, parameter);
            return lambda.Compile();
        }

        public static Delegate Getter(this object obj, string name)
        {
            var type = obj.GetType();

            if (typeof(IDictionary<string, object>).IsAssignableFrom(type))
            {
                return (Func<IDictionary<string, object>, dynamic>)((IDictionary<string, object> o) => { return Value(o, name); });
            }

            if (typeof(IDynamicMetaObjectProvider).IsAssignableFrom(type))
            {
                return (Delegate)((Func<IDynamicMetaObjectProvider, string, Delegate>)DynamicExtensions.Getter).DynamicInvoke(obj, name);
            }

            throw new NotImplementedException();
        }

        public static Delegate Setter(this IDictionary<string, object> dictionary, string name)
        {
            return (Action<IDictionary<string, object>, object>)((IDictionary<string, object> o, object value) => { Value(o, name, value); });
        }

        public static Delegate Setter(this IDynamicMetaObjectProvider provider, string name)
        {
            var scope = provider.GetType();
            var parameters = new ParameterExpression[]
                {
                    Expression.Parameter(typeof(object)),
                    Expression.Parameter(typeof(object))
                };
            var metaObject = provider.GetMetaObject(parameters[0]);
            var binder = (SetMemberBinder)Microsoft.CSharp.RuntimeBinder.Binder.SetMember(0, name, scope, new[] { Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(0, null), Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(0, null) });
            var operation = metaObject.BindSetMember(binder, new DynamicMetaObject(parameters[1], BindingRestrictions.Empty));
            var block = Expression.Block(
                Expression.Label(CallSiteBinder.UpdateLabel),
                operation.Expression
            );
            var lambda = Expression.Lambda(block, parameters[0], parameters[1]);
            return lambda.Compile();
        }

        public static Delegate Setter(this object obj, string name)
        {
            var type = obj.GetType();

            if (typeof(IDictionary<string, object>).IsAssignableFrom(type))
            {
                return (Action<IDictionary<string, object>, object>)((IDictionary<string, object> o, object value) => { Value(o, name, value); });
            }

            if (typeof(IDynamicMetaObjectProvider).IsAssignableFrom(type))
            {
                return (Delegate)((Func<IDynamicMetaObjectProvider, string, Delegate>)DynamicExtensions.Setter).DynamicInvoke(obj, name);
            }

            throw new NotImplementedException();
        }

        public static dynamic Value(this IDynamicMetaObjectProvider provider, string name)
        {
            var getter = provider.Getter(name);
            return getter.DynamicInvoke(provider);
        }

        public static dynamic Value(this IDictionary<string, object> dictionary, string name)
        {
            object value;
            if (dictionary.TryGetValue(name, out value))
                return value;
            return null;
        }

        public static void Value(this IDynamicMetaObjectProvider provider, string name, object value)
        {
            var setter = provider.Setter(name);
            setter.DynamicInvoke(provider, value);
        }

        public static void Value(this IDictionary<string, object> dictionary, string name, object value)
        {
            dictionary[name] = value;
        }
    }
}
