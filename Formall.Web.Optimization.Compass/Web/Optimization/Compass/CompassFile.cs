namespace Formall.Web.Optimization.Compass
{
    using System;
    using System.IO;
    using System.Web.Hosting;

    /// <summary>
    /// The compass file.
    /// </summary>
    public class CompassFile : VirtualFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompassFile"/> class. 
        /// </summary>
        /// <param name="virtualPath">The virtual path to the resource represented by this instance. </param>
        public CompassFile(string virtualPath)
            : base(virtualPath)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompassFile" /> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public CompassFile(VirtualFileBase file)
            : base(file.VirtualPath)
        {
        }

        /// <summary>
        /// Gets the CSS virtual file.
        /// </summary>
        /// <value>The CSS virtual file.</value>
        public VirtualFile CssFile
        {
            get
            {
                return new CompassFile(string.Format("{0}.css", this.VirtualPath.Substring(0, this.VirtualPath.LastIndexOf(Path.GetExtension(this.VirtualPath), StringComparison.Ordinal))));
            }
        }

        /// <summary>
        /// Gets the absolute path.
        /// </summary>
        /// <value>The absolute path.</value>
        public string AbsolutePath
        {
            get
            {
                return (HostingEnvironment.ApplicationPhysicalPath ?? string.Empty) + this.VirtualPath;
            }
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="FileInfo" /> to <see cref="CompassFile" />.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator CompassFile(FileInfo c)
        {
            var virtualPath = string.Empty;

            if (!string.IsNullOrEmpty(HostingEnvironment.ApplicationPhysicalPath))
            {
                virtualPath = c.FullName.Substring(
                    HostingEnvironment.ApplicationPhysicalPath.Length, c.FullName.Length - HostingEnvironment.ApplicationPhysicalPath.Length);
            }

            return new CompassFile(virtualPath);
        }

        /// <summary>
        /// Gets the CSS file info.
        /// </summary>
        /// <returns>The CSS file info.</returns>
        public FileInfo ToCssFileInfo()
        {
            return new FileInfo(string.Format("{0}.css", this.AbsolutePath.Substring(0, this.AbsolutePath.IndexOf(Path.GetExtension(this.VirtualPath), StringComparison.Ordinal))));
        }

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <returns>The file info.</returns>
        public FileInfo ToFileInfo()
        {
            return new FileInfo(this.AbsolutePath);
        }

        /// <summary>
        /// When overridden in a derived class, returns a read-only stream to the virtual resource.
        /// </summary>
        /// <returns>
        /// A read-only stream to the virtual file.
        /// </returns>
        public override Stream Open()
        {
            return new MemoryStream(File.ReadAllBytes(this.AbsolutePath));
        }
    }
}