namespace Formall.Web.Optimization.Compass
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Hosting;
    using System.Web.Optimization;

    using SassAndCoffee.Core;
    using SassAndCoffee.Ruby.Sass;

    /// <summary>
    /// Bundle transformer for compass files
    /// </summary>
    public class CompassTransform : IBundleTransform
    {
        /// <summary>
        /// The import regular expression
        /// </summary>
        private static readonly Regex SassImportRegex = new Regex("@import [\"|'](.+)[\"|'];", RegexOptions.Compiled);

        /// <summary>
        /// The compiler pool
        /// </summary>
        private readonly Pool<ISassCompiler, SassCompilerProxy> compilerPool = new Pool<ISassCompiler, SassCompilerProxy>(() => new CompassCompiler());

        /// <summary>
        /// Processes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="response">The response.</param>
        public void Process(BundleContext context, BundleResponse response)
        {
            var builder = new StringBuilder();

            foreach (var file in response.Files)
            {
                var compassFile = new CompassFile(file.VirtualFile);

                using (var compiler = this.compilerPool.GetInstance())
                {
                    var content = File.ReadAllText(compassFile.AbsolutePath, Encoding.UTF8);
                    var matches = SassImportRegex.Matches(content);

                    var directory = Path.GetDirectoryName(compassFile.AbsolutePath);
                    var extension = Path.GetExtension(file.VirtualFile.Name);

                    var dependencies = new List<string>();

                    foreach (Match match in matches)
                    {
                        var import = match.Groups[1].Value;

                        dependencies.Add(
                            Path.Combine(
                                directory,
                                string.Concat("_", import, extension)));
                    }


                    builder.AppendLine(
                        compiler.Compile(
                            compassFile.AbsolutePath,
                            false,
                            dependencies));
                }
            }

            // Serve files as css files if optimization is disabled
            if (!BundleTable.EnableOptimizations)
            {
                var files = new List<BundleFile>();

                foreach (var file in response.Files)
                {
                    files.Add(new BundleFile(file.IncludedVirtualPath, new CompassFile(file.VirtualFile).CssFile));
                }

                response.Files = files;
            }

            response.ContentType = "text/css";
            response.Content = builder.ToString();
        }
    }
}