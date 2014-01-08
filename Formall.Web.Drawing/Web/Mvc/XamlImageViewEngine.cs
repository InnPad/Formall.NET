// Copyright (c) 2011 Chris Pietschmann <http://pietschsoft.com>
//
// This file is part of MvcXaml <http://mvcxaml.codeplex.com>
//
// For licensing info goto <http://mvcxaml.codeplex.com/license>

using System;
using System.Web.Mvc;

namespace Formall.Web.Mvc
{
    using Formall.Imaging;

    public class XamlImageViewEngine : VirtualPathProviderViewEngine
    {
        public XamlImageViewEngine()
        {
            base.ViewLocationFormats = new string[] { "~/Views/{1}/{0}.xaml", "~/Views/Shared/{0}.xaml" };
            base.AreaViewLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.xaml", "~/Areas/{2}/Views/Shared/{0}.xaml" };
            base.PartialViewLocationFormats = base.ViewLocationFormats;
            base.AreaPartialViewLocationFormats = base.AreaViewLocationFormats;
        }

        public XamlImageViewEngine(ImageFormat imageFormat)
            : this()
        {
            this.ImageFormat = imageFormat;
        }

        /// <summary>
        /// The format to encode the resulting images to. Default is Png
        /// </summary>
        public ImageFormat ImageFormat { get; set; }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new XamlImageView(partialPath, this.ImageFormat);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new XamlImageView(viewPath, this.ImageFormat);
        }
    }
}
