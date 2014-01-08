// This file is part of MvcXaml <http://mvcxaml.codeplex.com>
//
// For licensing info goto <http://mvcxaml.codeplex.com/license>

using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Formall.Imaging
{
    public class XamlImage : IDisposable
    {
        #region "Static Methods"

        /// <summary>
        /// Renders the XAML contained within a file to PNG format and writes it to the Stream
        /// </summary>
        /// <param name="xamlPath">The path of the XAML file to render</param>
        /// <param name="streamToWriteTo">The Stream to write the renderd image to</param>
        public static void RenderPath(string xamlPath, Stream streamToWriteTo)
        {
            XamlImage.RenderPath(xamlPath, null, streamToWriteTo);
        }

        /// <summary>
        /// Renders the XAML contained within a file to PNG format and writes it to the Stream
        /// </summary>
        /// <param name="xamlPath">The path of the XAML file to render</param>
        /// <param name="dataContext">The DataContext to pass to the XAML control</param>
        /// <param name="streamToWriteTo">The Stream to write the renderd image to</param>
        public static void RenderPath(string xamlPath, object dataContext, Stream streamToWriteTo)
        {
            XamlImage.RenderPath(xamlPath, dataContext, streamToWriteTo, ImageFormat.Png);
        }

        /// <summary>
        /// Renders the XAML contained within a file to the specified ImageFormat and writes it to the Stream
        /// </summary>
        /// <param name="xamlPath">The path of the XAML file to render</param>
        /// <param name="dataContext">The DataContext to pass to the XAML control</param>
        /// <param name="streamToWriteTo">The Stream to write the renderd image to</param>
        /// <param name="imageFormat">The ImageFormat to encode the rendered image</param>
        public static void RenderPath(string xamlPath, object dataContext, Stream streamToWriteTo, ImageFormat imageFormat)
        {
            using (var img = new XamlImage
            {
                Path = xamlPath,
                DataContext = dataContext,
                ImageFormat = imageFormat
            })
            {
                img.Render();

                if (img.Stream != null)
                {
                    img.Stream.WriteTo(streamToWriteTo);
                }
            }
        }

        #endregion

        public XamlImage()
        {
            this.ImageFormat = ImageFormat.Png;
        }

        /// <summary>
        /// The MemoryStream that contains the final rendered image
        /// </summary>
        public MemoryStream Stream { get; private set; }

        /// <summary>
        /// Gets or sets the data context for passing to the Xaml Control when rendered
        /// </summary>
        public object DataContext { get; set; }

        /// <summary>
        /// The path to the Xaml file to render
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The ImageFormat to encode the renderd image in. Default is Png
        /// </summary>
        public ImageFormat ImageFormat { get; set; }

        /// <summary>
        /// Renders the specified Xaml file as a MemoryStream accessible from the XamlImageStream property.
        /// </summary>
        /// <param name="obj">The string path of the Xaml file to load and render</param>
        public void Render()
        {
            var t = new Thread(new ThreadStart(render_Worker));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            // Give the thread some time to execute, and essentially "timeout" after
            // so that this doesn't get trapped in an infinite loop
            t.Join(5000);
        }

        private void render_Worker()
        {
            var xamlPath = this.Path;

            object xaml = null;
            using (var stream = File.OpenRead(xamlPath))
            {
                xaml = XamlReader.Load(stream);
            }

            if (xaml == null)
            {
                throw new InvalidOperationException(string.Format("XAML File Not Found ({0})", xamlPath));
            }

            if (xaml is FrameworkElement)
            {
                var ctrl = (FrameworkElement)xaml;
                ctrl.DataContext = this.DataContext;

                ctrl.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() =>
                {
                    this.Stream = RenderHelper.RenderControlAsImageStream(ctrl, this.ImageFormat);
                }));

                return;
            }

            throw new InvalidOperationException(string.Format("Unsupported XAML Type ({0})", xaml.GetType().Name));
        }

        public void Dispose()
        {
            if (this.Stream != null)
            {
                this.Stream.Dispose();
            }
        }
    }
}
