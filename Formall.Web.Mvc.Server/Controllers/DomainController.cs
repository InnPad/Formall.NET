using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Formall.Web.Mvc.Controllers
{
    using Formall.Navigation;
    using Formall.Persistence;
    using Formall.Presentation;
    using Formall.Reflection;
    using Formall.Validation;

    public class DomainController : Controller
    {   
        #region - by name -

        public ActionResult Index(string name)
        {
            var segment = Schema.Current.Query(name, "*").FirstOrDefault();

            if (segment == null)
            {
                return Json(new { success = false, message = "Not found" }, JsonRequestBehavior.AllowGet);
            }

            var writer = new SegmentOutput(segment, true, true, false, 2);

            return new ContentWriteResult
            {
                ContentType = MediaType.Json,
                Write = writer.Write
            };
        }

        public ActionResult Deep(string name)
        {
            var segment = Schema.Current.Query(name, "*").FirstOrDefault();

            if (segment == null)
            {
                return Json(new { success = false, message = "Not found" }, JsonRequestBehavior.AllowGet);
            }

            var writer = new SegmentOutput(segment, true, true, true, 2);

            return new ContentWriteResult
            {
                ContentType = MediaType.Json,
                Write = writer.Write
            };
        }

        #endregion - by name -

        #region - by keyPrefix and/or id -

        public ActionResult Data(string keyPrefix, Guid? id)
        {
            if (id.HasValue)
            {
                var document = Schema.Current.Get(keyPrefix, id.Value, "*").FirstOrDefault();

                if (document == null)
                {
                    return Json(new { success = false, message = "Not found" }, JsonRequestBehavior.AllowGet);
                }

                var entity = document as Entity;

                if (entity == null)
                {
                    return Json(new { success = false, message = "Not an entity" }, JsonRequestBehavior.AllowGet);
                }

                var writer = new DocumentOutput(document, true, false, 2);

                return new ContentWriteResult
                {
                    ContentType = MediaType.Json,
                    Write = writer.Write
                };
            }
            else
            {
                var documents = Schema.Current.Get(keyPrefix, "*");

                var writer = new DocumentArrayOutput(documents, true, false, 2);

                return new ContentWriteResult
                {
                    ContentType = MediaType.Json,
                    Write = writer.Write
                };
            }
        }

        public ActionResult Metadata(string keyPrefix, Guid? id)
        {
            if (id.HasValue)
            {
                var document = Schema.Current.Get(keyPrefix, id.Value, "*").FirstOrDefault();

                if (document == null)
                {
                    return Json(new { success = false, message = "Not found" }, JsonRequestBehavior.AllowGet);
                }

                var writer = new DocumentOutput(document, false, true, 2);

                return new ContentWriteResult
                {
                    ContentType = MediaType.Json,
                    Write = writer.Write
                };
            }
            else
            {
                var documents = Schema.Current.Get(keyPrefix, "*");

                var writer = new DocumentArrayOutput(documents, false, true, 2);

                return new ContentWriteResult
                {
                    ContentType = MediaType.Json,
                    Write = writer.Write
                };
            }
        }

        public ActionResult Read(string keyPrefix, Guid? id)
        {
            if (id.HasValue)
            {
                var document = Schema.Current.Get(keyPrefix, id.Value, "*").FirstOrDefault();

                if (document == null)
                {
                    return Json(new { success = false, message = "Not found" }, JsonRequestBehavior.AllowGet);
                }

                var entity = document as IEntity;

                if (entity == null)
                {
                    return Json(new { success = false, message = "Not an entity" }, JsonRequestBehavior.AllowGet);
                }

                var writer = new DocumentOutput(document, true, false, 2);

                return new ContentWriteResult
                {
                    ContentType = MediaType.Json,
                    Write = writer.Write
                };
            }
            else
            {
                var documents = Schema.Current.Get(keyPrefix, "*");

                var writer = new DocumentArrayOutput(documents, true, false, 2);

                return new ContentWriteResult
                {
                    ContentType = MediaType.Json,
                    Write = writer.Write
                };
            }
        }

        #endregion - by keyPrefix & id -

        /*
        //
        // GET: data.{domain}/{*name}/{id}

        public ActionResult Default(string name, Guid id)
        {
            ViewBag.Title = "Data Dictionary";
            return View("~/Views/Ext/Dictionary.cshtml");
        }
        */
        //
        // GET: Data/{*name}/$metadata
        /*
        class MetadataRequestExtra
        {
            public string[] Requires { get; set; }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Metadata(string path)
        {
            const string MissmatchError = "{0} is fully quialified by name. No need to provide id. Where you expecting a model?";
            const string UnexpectedError = "Unexpected error";

            bool success = true;
            string message = null;
            RavenJObject data = null;
            var descriptor = Schema.Current.Describe(path);
            var extra = InputRavenJObject.Deserialize<MetadataRequestExtra>();
            var requires = extra != null ? extra.Requires : new string[] { };

            if (descriptor == null)
            {
                success = false;
                message = "Name not Found";
            }
            else
            {
                switch (descriptor.Type)
                {
                    case NodeKinds.Area:
                        data = descriptor.Metadata(requires);
                        break;

                    case NodeKinds.Error:
                        {
                            success = false;
                            message = (descriptor as Error).Message;
                        }
                        break;

                    case NodeKinds.Name:
                        data = descriptor.Metadata(requires);
                        break;

                    case NodeKinds.Type:
                        {
                            var type = descriptor as Prototype;

                            switch (type.Category)
                            {
                                case TypeCategories.Enum:
                                    data = descriptor.Metadata(requires);
                                    break;

                                case TypeCategories.Model:
                                    data = descriptor.Metadata(requires);
                                    break;

                                case TypeCategories.Unit:
                                    data = descriptor.Metadata(requires);
                                    break;

                                case TypeCategories.Value:
                                    data = descriptor.Metadata(requires);
                                    break;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            if (!success)
            {
                if (data == null)
                    data = new RavenJObject();

                data["url"] = new RavenJValue((RouteData.Route as Route).Url);

                foreach (var item in RouteData.Values)
                    data[item.Key] = new RavenJValue(item.Value);

                data["name"] = new RavenJValue(path);

                return Failure(data, message);
            }

            return Success(data);
        }

        //
        // GET: Data/{*name}
        // GET: Data/{*name}/{Select|Read}
        // GET: Data/{*name}/{id}
        // GET: Data/{*name}/{id}/{Select|Read}

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Read(string path, Guid id)
        {
            return Select(path, id);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Select(string path, Guid id)
        {
            const string MissmatchError = "Only model type entities can be accessed this way ({0} found on this path). If you are looking for the metadata, use Data/[*name]/$metadata path instead.";
            const string UnexpectedError = "Unexpected error";

            RavenJObjectResult result = null;

            var descriptor = Schema.Current.Describe(path);

            if (descriptor == null)
            {
                result = Failure("Name not Found");
            }
            else if (descriptor.Type == NodeKinds.Error)
            {
                result = Failure((descriptor as Error).Message);
            }
            else if (descriptor.Type != NodeKinds.Type)
            {
                result = Failure(string.Format(MissmatchError, System.Enum.GetName(typeof(NodeKinds), descriptor.Type)));
            }
            else
            {
                var type = descriptor as Prototype;

                if (TypeCategories.Model != type.Category)
                {
                    result = Failure(string.Format(MissmatchError, System.Enum.GetName(typeof(NodeKinds), descriptor.Type)));
                }
                else
                {
                    var model = type as ModelDescriptor;

                    var repository = model.Repository;

                    if (repository != null)
                    {
                        if (model.Definition.Singleton)
                        {
                            result = new RavenJObjectResult { Content = repository.Read(Guid.Empty) };
                        }
                        else if (Guid.Empty.Equals(id))
                        {
                            result = new RavenJObjectResult { Content = repository.Read(0, 100) };
                        }
                        else
                        {
                            result = new RavenJObjectResult { Content = repository.Read(id) };
                        }
                    }
                    else
                    {
                        result = Failure("Could not resolve repository");
                    }
                }
            }

            result.Content["route"] = new RavenJValue((RouteData.Route as Route).Url);

            foreach (var item in RouteData.Values)
            {
                result.Content[item.Key] = new RavenJValue(item.Value);
            }

            result.Content["name"] = new RavenJValue(path);
            if (!id.Equals(Guid.Empty))
            {
                result.Content["id"] = new RavenJValue(id);
            }

            return result;
        }

        //
        // POST: Data/{*name}
        // POST: Data/{*name}/{Insert|Create}

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(string path)
        {
            return Insert(path);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Insert(string name)
        {
            var data = new RavenJObject();
            data["url"] = new RavenJValue((RouteData.Route as Route).Url);

            foreach (var item in RouteData.Values)
                data[item.Key] = new RavenJValue(item.Value);

            data["name"] = new RavenJValue(name);

            return Success(data);
        }

        // 
        // PUT: Data/{*name}/{id}?patch={patch}
        // PUT: Data/{*name}/{id}/{Update|Edit}?patch={patch}
        // PATCH: Data/{*name}/{id}
        // PATCH: Data/{*name}/{id}/{Update|Edit}
        // POST: Data/{*name}/{id}/{Update|Edit}?patch={patch}

        [AcceptVerbs(HttpVerbs.Put | HttpVerbs.Patch | HttpVerbs.Post)]
        public ActionResult Edit(string path, Guid id, bool patch)
        {
            return Update(path, id, patch);
        }

        [AcceptVerbs(HttpVerbs.Put | HttpVerbs.Patch | HttpVerbs.Post)]
        public ActionResult Update(string path, Guid id, bool patch)
        {
            const string MissmatchError = "Only model type entities can be accessed this way ({0} found on this path). If you are looking for the metadata, use Data/[*name]/$metadata path instead.";
            const string UnexpectedError = "Unexpected error";

            RavenJObjectResult result = null;

            var descriptor = Schema.Current[path];

            if (descriptor == null)
            {
                result = Failure("Name not Found");
            }
            else if (descriptor.Type == NodeKinds.Error)
            {
                result = Failure((descriptor as Error).Message);
            }
            else if (descriptor.Type != NodeKinds.Type)
            {
                result = Failure(string.Format(MissmatchError, System.Enum.GetName(typeof(NodeKinds), descriptor.Type)));
            }
            else
            {
                var type = descriptor as Prototype;

                if (TypeCategories.Model != type.Category)
                {
                    result = Failure(string.Format(MissmatchError, System.Enum.GetName(typeof(NodeKinds), descriptor.Type)));
                }
                else
                {
                    var model = type as Model;

                    var repository = model.Repository;

                    if (repository != null)
                    {
                        var body = this.InputRavenJObject;
                        var data = body["data"];

                        if (data != null)
                        {
                            switch (data.Type)
                            {
                                case JTokenType.Array:
                                    result = new RavenJObjectResult { Content = repository.Update(data as RavenJArray, patch) };
                                    break;

                                case JTokenType.Object:
                                    if (Guid.Empty.Equals(id))
                                    {
                                        result = new RavenJObjectResult { Content = repository.Update(data as RavenJObject, patch) };
                                    }
                                    else
                                    {
                                        result = new RavenJObjectResult { Content = repository.Update(id, data as RavenJObject, patch) };
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            result = Failure("Invalid request. 'data' property expected");
                        }

                    }
                    else
                    {
                        result = Failure("Could not resolve repository");
                    }
                }
            }

            result.Content["route"] = new RavenJValue((RouteData.Route as Route).Url);

            foreach (var item in RouteData.Values)
            {
                result.Content[item.Key] = new RavenJValue(item.Value);
            }

            result.Content["name"] = new RavenJValue(path);
            if (!id.Equals(Guid.Empty))
            {
                result.Content["id"] = new RavenJValue(id);
            }

            return result;
        }

        // 
        // DELETE: Data/{*name}/{id}
        // DELETE: Data/{*name}/{id}/{Delete|Destroy}
        // POST: Data/{*name}/{id}/{Delete|Destroy}
        // GET: Data/{*name}/{id}/{Delete|Destroy}

        // 
        // DELETE: Data/{*name}/{id}/{property}/{index}
        // DELETE: Data/{*name}/{id}/{property}/{index}/{Delete|Destroy}
        // POST: Data/{*name}/{id}/{property}/{index}/{Delete|Destroy}
        // GET: Data/{*name}/{id}/{property}/{index}/{Delete|Destroy}

        [AcceptVerbs(HttpVerbs.Delete | HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Delete(string name, Guid id, string property, string index)
        {
            const string MissmatchError = "Only model type entities can be accessed this way ({0} found on this path). If you are looking for the metadata, use Data/[*name]/$metadata path instead.";
            const string UnexpectedError = "Unexpected error";

            RavenJObjectResult result = null;

            var descriptor = Schema.Current.Describe(name);

            if (descriptor == null)
            {
                result = Failure("Name not Found");
            }
            else if (descriptor.Type == NodeKinds.Error)
            {
                result = Failure((descriptor as Error).Message);
            }
            else if (descriptor.Type != NodeKinds.Type)
            {
                result = Failure(string.Format(MissmatchError, System.Enum.GetName(typeof(NodeKinds), descriptor.Type)));
            }
            else
            {
                var type = descriptor as Prototype;

                if (TypeCategories.Model != type.Category)
                {
                    result = Failure(string.Format(MissmatchError, System.Enum.GetName(typeof(NodeKinds), descriptor.Type)));
                }
                else
                {
                    var model = type as ModelDescriptor;

                    var repository = model.Repository;

                    if (repository != null)
                    {
                        var body = this.InputRavenJObject;
                        var data = body["data"];

                        if (data != null)
                        {
                            switch (data.Type)
                            {
                                case JTokenType.Array:
                                    result = new RavenJObjectResult { Content = repository.Delete(data as RavenJArray) };
                                    break;

                                case JTokenType.Object:
                                    result = new RavenJObjectResult { Content = repository.Delete(data as RavenJObject) };
                                    break;
                            }
                        }
                        else if (Guid.Empty.Equals(id))
                        {
                            result = Failure("Invalid request. id param expected");
                        }
                        else
                        {
                            result = new RavenJObjectResult { Content = repository.Delete(id) };
                        }
                    }
                    else
                    {
                        result = Failure("Could not resolve repository");
                    }
                }
            }

            result.Content["route"] = new RavenJValue((RouteData.Route as Route).Url);

            foreach (var item in RouteData.Values)
            {
                result.Content[item.Key] = new RavenJValue(item.Value);
            }

            result.Content["name"] = new RavenJValue(name);
            if (!id.Equals(Guid.Empty))
            {
                result.Content["id"] = new RavenJValue(id);
            }

            return result;
        }

        [AcceptVerbs(HttpVerbs.Delete | HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Destroy(string name, Guid id, string property, string index)
        {
            return Delete(name, id, property, index);
        }

        // POST: Data/{*name}/Validate
        // POST: Data/{*name}/{id}/Validate

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Validate(string name, Guid id)
        {
            var data = new RavenJObject();
            data["url"] = new RavenJValue((RouteData.Route as Route).Url);

            foreach (var item in RouteData.Values)
                data[item.Key] = new RavenJValue(item.Value);

            data["name"] = new RavenJValue(name);
            data["id"] = new RavenJValue(id);

            return Failure(data, "Not found");
        }

        // GET: Data/{*name}/{Invoke|Call}/{fn}
        // GET: Data/{*name}/{id}/{Invoke|Call}/{fn}
        // POST: Data/{*name}/{Invoke|Call}/{fn}
        // POST: Data/{*name}/{id}/{Invoke|Call}/{fn}

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Call(string name, Guid id, string fn)
        {
            return Invoke(name, id, fn);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Invoke(string name, Guid id, string fn)
        {
            var data = new RavenJObject();
            data["url"] = new RavenJValue((RouteData.Route as Route).Url);

            foreach (var item in RouteData.Values)
                data[item.Key] = new RavenJValue(item.Value);

            data["name"] = new RavenJValue(name);
            data["id"] = new RavenJValue(id);

            return Failure(data, "Not found");
        }

        public ActionResult Options(string name)
        {
            var accept = Request.Headers["Accept"];
            var acceptEncoding = Request.Headers["Accept-Encoding"];
            var acceptLanguage = Request.Headers["Accept-Language"];
            var acceptHeaders = Request.Headers["Access-Control-Request-Headers"];
            var acceptMethod = Request.Headers["Access-Control-Request-Method"];

            var content = new RavenJObject();
            content["Accept"] = Request.Headers["Accept"];
            content["Accept-Encoding"] = Request.Headers["Accept-Encoding"];
            content["Accept-Language"] = Request.Headers["Accept-Language"];
            content["Access-Control-Request-Headers"] = Request.Headers["Access-Control-Request-Headers"];
            content["Access-Control-Request-Method"] = Request.Headers["Access-Control-Request-Method"];

            Response.Headers["Access-Control-Allow-Origin"] = "*";

            return Success(content);
        }
         */
    }
}
