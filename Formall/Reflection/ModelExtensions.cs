using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Formall.Reflection
{
    using Formall.Validation;

    public static class ModelExtensions
    {
        public static object GetPropertyValue(object o, string member)
        {
            if (o == null) throw new ArgumentNullException("o");
            if (member == null) throw new ArgumentNullException("member");
            Type scope = o.GetType();
            IDynamicMetaObjectProvider provider = o as IDynamicMetaObjectProvider;
            if (provider != null)
            {
                ParameterExpression param = Expression.Parameter(typeof(object));
                DynamicMetaObject mobj = provider.GetMetaObject(param);
                GetMemberBinder binder = (GetMemberBinder)Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, member, scope, new[] { Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(0, null) });
                DynamicMetaObject ret = mobj.BindGetMember(binder);
                BlockExpression final = Expression.Block(
                    Expression.Label(CallSiteBinder.UpdateLabel),
                    ret.Expression
                );
                LambdaExpression lambda = Expression.Lambda(final, param);
                Delegate del = lambda.Compile();
                return del.DynamicInvoke(o);
            }
            return o.GetType().GetProperty(member, BindingFlags.Public | BindingFlags.Instance).GetValue(o, null);
        }

        public static void SetPropertyValue(object o, string member, object value)
        {
            if (o == null) throw new ArgumentNullException("o");
            if (member == null) throw new ArgumentNullException("member");
            Type scope = o.GetType();
            IDynamicMetaObjectProvider provider = o as IDynamicMetaObjectProvider;
            if (provider != null)
            {
                ParameterExpression param = Expression.Parameter(typeof(object));
                DynamicMetaObject mobj = provider.GetMetaObject(param);
                SetMemberBinder binder = (SetMemberBinder)Microsoft.CSharp.RuntimeBinder.Binder.SetMember(0, member, scope, new[] { Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(0, null), });

                var metaValue = new DynamicMetaObject(Expression.Parameter(typeof(object), member), BindingRestrictions.Empty, value);

                DynamicMetaObject ret = mobj.BindSetMember(binder, metaValue);
                BlockExpression final = Expression.Block(
                    Expression.Label(CallSiteBinder.UpdateLabel),
                    ret.Expression
                );
                LambdaExpression lambda = Expression.Lambda(final, param);
                Delegate del = lambda.Compile();
                del.DynamicInvoke(o);
            }
            o.GetType().GetProperty(member, BindingFlags.Public | BindingFlags.Instance).SetValue(o, value);
        }

        public static Field Field(this Model model, string name)
        {
            var field = model.Fields[name];
            return field;
        }

        public static object Value(this Model model, dynamic data, string name)
        {
            var field = model.Field(name);

            var metaProvider = data as IDynamicMetaObjectProvider;
            if (metaProvider != null)
            {
                return metaProvider.Value(name);
            }
            return data.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance).GetValue(data);
        }

        public static void Value(this Model model, dynamic data, string name, object value)
        {
            var field = model.Field(name);

            var metaProvider = data as IDynamicMetaObjectProvider;
            if (metaProvider != null)
            {
                metaProvider.Value(name, value);
            }
            data.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance).SetValue(data, value);
        }

        public static T Value<T>(this Model model, string name, dynamic data)
        {
            var field = model.Field(name);

            var metaProvider = data as IDynamicMetaObjectProvider;
            var parameter = Expression.Parameter(typeof(T), name);
            var metaObject = metaProvider.GetMetaObject(parameter);
            if (metaObject.HasValue && metaObject.Value is T)
            {
                return (T)metaObject.Value;
            }
            return default(T);
        }

        public static void Value<T>(this Model model, dynamic data, string name, T value)
        {
            var field = model.Field(name);

            var metaProvider = data as IDynamicMetaObjectProvider;
            if (metaProvider != null)
            {
                metaProvider.Value(name, value);
            }
            data.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance).SetValue(data, value);

        }

        public static IEnumerable<ValidationResult> Validate(this Model model, object data)
        {
            return new ValidationResult[0];
        }
    }
}
