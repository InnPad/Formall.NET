using Formall.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Formall.Web.Mvc
{
    public class ControllerFactory : DefaultControllerFactory
    {
        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            var controllerType = base.GetControllerType(requestContext, controllerName);

            if (controllerType == null || controllerType == typeof(Formall.Web.Mvc.Controllers.DomainController))
            {
                var path = requestContext.HttpContext.Request.Path;

                path = string.IsNullOrEmpty(path) ? string.Empty : path.Trim('/');

                var pathSplit = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                var action = "Index";

                var actionIndex = pathSplit
                    .Select((item, index) => new { Action = item, Index = index })
                    .Where(o => o.Action.Length > 1 && o.Action[0] == '$')
                    .LastOrDefault();

                if (actionIndex != null)
                {
                    action = actionIndex.Action;

                    action = action.TrimStart('$');

                    var tokens = action.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                    // Remove '-' and title case the action name
                    action = string.Concat(tokens.Select(token => token.Substring(0, 1).ToUpperInvariant() + token.Substring(1).ToLowerInvariant()));
                }

                if (pathSplit[0] == "$")
                {
                    var keyPrefix = pathSplit.Skip(1);

                    if (actionIndex != null)
                    {
                        keyPrefix = keyPrefix.Take(actionIndex.Index - 1);
                    }
                    else
                    {
                        action = "Read";
                    }

                    if (actionIndex == null || actionIndex.Index < pathSplit.Length - 1)
                    {
                        Guid id;

                        if (Guid.TryParse(pathSplit[pathSplit.Length - 1], out id))
                        {
                            if (actionIndex == null)
                            {
                                keyPrefix = keyPrefix.Take(pathSplit.Length - 2);
                            }

                            requestContext.RouteData.Values["id"] = id;
                        }
                    }

                    requestContext.RouteData.Values["keyPrefix"] = string.Join("/", keyPrefix) + '/';
                }
                else if (actionIndex != null)
                {
                    requestContext.RouteData.Values["name"] = string.Join("/", pathSplit.Take(actionIndex.Index));
                }
                else
                {
                    requestContext.RouteData.Values["name"] = string.Join("/", pathSplit);
                }

                requestContext.RouteData.Values["action"] = action;

                controllerType = typeof(Formall.Web.Mvc.Controllers.DomainController);

                return controllerType;
            }

            var areaName = requestContext.RouteData.Values["area"] as string;
            var actionName = requestContext.RouteData.Values["action"] as string;
            var httpMethod = requestContext.HttpContext.Request.HttpMethod.ToUpperInvariant();

            HttpVerbs httpVerb;
            if (!System.Enum.TryParse<HttpVerbs>(requestContext.HttpContext.Request.HttpMethod, true, out httpVerb))
                httpVerb = HttpVerbs.Get;

            /*switch (areaName)
            {
                case "Data":
                    {
                        CrudActions crudAction;

                        if (string.IsNullOrEmpty(actionName) || !System.Enum.TryParse<CrudActions>(actionName, true, out crudAction))
                            switch (httpVerb)
                            {
                                case HttpVerbs.Delete:
                                    crudAction = CrudActions.Delete;
                                    break;

                                case HttpVerbs.Get:
                                    crudAction = CrudActions.Select;
                                    break;

                                case HttpVerbs.Patch:
                                    crudAction = CrudActions.Update;
                                    requestContext.RouteData.Values["patch"] = true;
                                    break;

                                case HttpVerbs.Post:
                                    crudAction = CrudActions.Create;
                                    break;

                                case HttpVerbs.Put:
                                    crudAction = CrudActions.Update;
                                    break;

                                case HttpVerbs.Options:
                                    requestContext.RouteData.Values["action"] = "Options";
                                    return controllerType;

                                default:
                                    crudAction = CrudActions.Default;
                                    break;
                            }

                        switch (crudAction)
                        {
                            case CrudActions.Update:
                                // fix patch
                                var patchValue = requestContext.RouteData.Values["patch"];
                                if (patchValue == null || patchValue.GetType() != typeof(bool))
                                    requestContext.RouteData.Values["patch"] = true;
                                break;
                        }

                        if (actionName == null)
                        {
                            actionName = System.Enum.GetName(typeof(CrudActions), crudAction);

                            requestContext.RouteData.Values["action"] = actionName;
                        }
                    }
                    break;
            }*/

            return controllerType;
        }
    }
}