namespace Formall.Web.Optimization.Compass
{
    using System.Web.Optimization;

    /// <summary>
    /// Bundle designed specifically for processing SASS stylesheets
    /// </summary>
    public class CompassBundle : Bundle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Web.Optimization.Bundles.Compass.CompassBundle"/> class.
        /// 
        /// </summary>
        /// <param name="virtualPath">The virtual path used to reference the <see cref="T:Web.Optimization.Bundles.Compass.CompassBundle"/> from within a view or Web page.</param>
        public CompassBundle(string virtualPath)
            : base(virtualPath, new IBundleTransform[2] { new CompassTransform(), new CssMinify() })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Web.Optimization.Bundles.Compass.CompassBundle"/> class.
        /// 
        /// </summary>
        /// <param name="virtualPath">The virtual path used to reference the <see cref="T:Web.Optimization.Bundles.Compass.CompassBundle"/> from within a view or Web page.</param>
        /// <param name="cdnPath">An alternate url for the bundle when it is stored in a content delivery network.</param>
        public CompassBundle(string virtualPath, string cdnPath)
            : base(virtualPath, cdnPath, new IBundleTransform[2] { new CompassTransform(), new CssMinify() })
        {
        }
    }
}