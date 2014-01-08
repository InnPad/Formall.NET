using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Formall.Web.Mvc
{
    public static class ImageResulExtensions
    {
        public static string Image<T>(this HtmlHelper helper, Expression<Action<T>> action, int width, int height)
        where T : Controller
        {
            return helper.Image<T>(action, width, height, "");
        }

        public static string Image<T>(this HtmlHelper helper, Expression<Action<T>> action, int width, int height, string alt)
            where T : Controller
        {
            string url = /*helper.*/BuildUrlFromExpression<T>(action);
            return string.Format("<img src=\"{0}\" width=\"{1}\" height=\"{2}\" alt=\"{3}\" />", url, width, height, alt);
        }

        private static string BuildUrlFromExpression<T>(Expression<Action<T>> action)
            where T : Controller
        {
            return string.Empty;
        }
    }
}