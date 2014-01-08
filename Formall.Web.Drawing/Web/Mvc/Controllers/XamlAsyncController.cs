using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Threading;
using System.Windows.Threading;

namespace Formall.Web.Mvc.Controllers
{
    public class XamlAsyncController : AsyncController
    {
        #region Private

        byte[] imageData;
        string viewName;
        string asmName;
        object viewModel = null;
        Func<Control> create = null;

        #endregion

        #region Methods

        /// <summary>
        /// Start rendering the controls directly
        /// </summary>
        /// <param name="c">The control to render</param>
        /// <param name="visualState">The visual state to render</param>
        protected void StartRendering(Func<Control> create)
        {
            AsyncManager.OutstandingOperations.Increment();
            var t = new Thread(RenderControl);
            this.create = create;
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

        }



        /// <summary>
        /// Start the rendering process of the xaml
        /// </summary>
        /// <param name="viewModel">The view model that we'll bind to the control</param>
        /// <param name="viewName">The xaml user control name, located in /Visualizations/{Controllser}/{View}.xaml</param>
        /// <param name="assemblyName">The assembly from where the control is loaded from</param>
        /// <param name="visualState">The visual state to render</param>
        protected void StartRendering(string viewName = "", object viewModel = null, string assemblyName = "")
        {
            AsyncManager.OutstandingOperations.Increment();
            this.viewModel = viewModel;
            this.viewName = viewName;
            this.asmName = assemblyName;
            var t = new Thread(LoadAndRender);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

        }


        /// <summary>
        /// Load the visualization and render
        /// </summary>
        private void LoadAndRender()
        {

            string controllerName = ValueProvider.GetValue("controller").RawValue as string;

            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ValueProvider.GetValue("action").RawValue as string;
            }

            if (string.IsNullOrEmpty(asmName))
            {
                asmName = this.GetType().Assembly.GetName().Name;
            }

            var vpath = string.Format("/Visualizations/{0}/{1}.xaml", controllerName, viewName);

            System.Uri resourceLocater = new System.Uri("/" + asmName + ";component" + vpath, System.UriKind.Relative);
            Application.ResourceAssembly = this.GetType().Assembly;
            var obj = Application.LoadComponent(resourceLocater);

            RenderControl(obj as Control);
        }

        private void RenderControl()
        {
            RenderControl(create());
        }


        /// <summary>
        /// Render the control to an image after the loading is completed
        /// Note that we can't take snapshots of animations
        /// </summary>
        private void RenderControl(Control f)
        {


            Canvas c = new Canvas();
            c.Width = f.Width;
            c.Height = f.Height;
            f.HorizontalAlignment = HorizontalAlignment.Stretch;
            f.VerticalAlignment = VerticalAlignment.Stretch;
            var context = viewModel ?? (dynamic)(new DictionaryWrapper(this.ViewData));


            c.DataContext = context;
            c.Children.Add(f);


            var init = new Action(() =>
                {
                    ImageFromControl(c);
                    AsyncManager.OutstandingOperations.Decrement();
                });


            if (f is ICustomInitializer)
            {
                var cust = f as ICustomInitializer;
                cust.Initialize(context, init);
            }
            else
            {
                c.Dispatcher.Invoke(DispatcherPriority.Background, init);
            }

        }

        /// <summary>F
        /// Returns the FileContentResult that we have in the byte array
        /// </summary>
        /// <returns></returns>
        public ActionResult XamlView()
        {

            return new FileContentResult(imageData, "image/png");
        }



        /// <summary>
        /// Render the control to an image
        /// </summary>
        /// <param name="c">The parent canvas to render</param>
        private void ImageFromControl(Canvas c)
        {

            int width = (int)c.Width;
            int height = (int)c.Height;

            Transform transform = c.LayoutTransform;
            c.LayoutTransform = null;
            Arrange(c);


            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(width,
                                                                     height, 1 / 300, 1 / 300, PixelFormats.Pbgra32);

            DrawingVisual visual = new DrawingVisual();
            using (DrawingContext context = visual.RenderOpen())
            {
                VisualBrush brush = new VisualBrush(c);
                context.DrawRectangle(brush,
                                      null,
                                      new Rect(new Point(), new Size(c.Width, c.Height)));
            }

            visual.Transform = new ScaleTransform(width / c.ActualWidth, height / c.ActualHeight);
            renderBitmap.Render(visual);


            //save to memory stream
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            encoder.Save(ms);
            imageData = ms.ToArray();
        }

        /// <summary>
        /// Force an arrange
        /// </summary>
        /// <param name="c">The canvas to arrange</param>
        private static void Arrange(Canvas c)
        {
            // Get the size of canvas
            Size size = new Size(c.Width, c.Height);

            c.Measure(size);
            c.Arrange(new Rect(size));
        }

        #endregion

        #region Actions


        public void VisualAsync(string name = "")
        {
            StartRendering(viewName: name);
        }


        public ActionResult VisualCompleted(string name = "", string viewState = "")
        {
            return XamlView();
        }

        #endregion
    }
}
