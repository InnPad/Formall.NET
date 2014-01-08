// Copyright (c) 2011 Chris Pietschmann <http://pietschsoft.com>
//
// This file is part of MvcXaml <http://mvcxaml.codeplex.com>
//
// For licensing info goto <http://mvcxaml.codeplex.com/license>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Formall.Web.Mvc
{
    using Formall.Imaging;

    public class XamlImageView : IView
    {
        public XamlImageView(string viewPath)
        {
            this.ViewPath = viewPath;
            this.ImageFormat = ImageFormat.Png;
        }

        public XamlImageView(string viewPath, ImageFormat imageFormat)
            : this(viewPath)
        {
            this.ImageFormat = imageFormat;
        }

        public string ViewPath { get; private set; }

        /// <summary>
        /// The format to encode the resulting image to. Default is Png
        /// </summary>
        public ImageFormat ImageFormat { get; private set; }

        public void Render(ViewContext viewContext, System.IO.TextWriter writer)
        {
            object dataContext = null;
            if (viewContext.ViewData.Model != null)
            {
                dataContext = viewContext.ViewData.Model;
            }
            else
            {
                dataContext = (dynamic)(new DynamicDictionary(viewContext.ViewData));
            }

            var xamlPath = viewContext.HttpContext.Server.MapPath(this.ViewPath);

            var response = viewContext.HttpContext.Response;
            response.ContentType = GetContentType(this.ImageFormat);
            XamlImage.RenderPath(xamlPath, dataContext, response.OutputStream, this.ImageFormat);
        }

        private static string GetContentType(ImageFormat format)
        {
            switch (format)
            {
                case ImageFormat.Png:
                    return "image/png";
                case ImageFormat.Jpeg:
                    return "image/jpeg";
                case ImageFormat.Bmp:
                    return "image/bmp";
                case ImageFormat.Gif:
                    return "image/gif";
                case ImageFormat.Tiff:
                    return "image/tiff";
                case ImageFormat.Wmp:
                    return "image/wmp";
                default:
                    return string.Empty;
            }
        }
    }
}
