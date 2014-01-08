namespace Formall.Web.Optimization.Compass
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    using SassAndCoffee.Ruby.Sass;

    /// <summary>
    /// Compiler for compass files
    /// </summary>
    public class CompassCompiler : ISassCompiler
    {
        /// <summary>
        /// The resource locker.
        /// </summary>
        private readonly object locker = new object();

        /// <summary>
        /// The processInfo
        /// </summary>
        private ProcessStartInfo processInfo;

        /// <summary>
        /// The initialized indicator
        /// </summary>
        private bool initialized;

        /// <summary>
        /// Compiles the specified file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="compressed">if set to <c>true</c> [compressed].</param>
        /// <param name="dependentFileList">The dependent file list.</param>
        /// <returns>The compiled file.</returns>
        public string Compile(string path, bool compressed, IList<string> dependentFileList)
        {
            if (path == null)
            {
                throw new ArgumentException("path cannot be null.", "path");
            }

            var pathInfo = new FileInfo(path);
            if (!pathInfo.Exists)
            {
                return null;
            }

            lock (this.locker)
            {
                this.Initialize();

                var directoryPath = pathInfo.DirectoryName;

                if (directoryPath != null && !directoryPath.Contains("\'"))
                {
                    this.processInfo.WorkingDirectory = directoryPath;
                }

                this.processInfo.Arguments = string.Format("/C compass compile {0}", pathInfo.Name);

                var process = new Process() { StartInfo = this.processInfo };
                process.Start();

                var output = process.StandardOutput.ReadToEnd();

                Trace.WriteLine(output);

                if (output.TrimStart().StartsWith("error"))
                {
                    throw new InvalidOperationException(string.Format("Error when compiling '{0}'", pathInfo.Name), new FormatException(output.Trim()));
                }

                var cssFile = ((CompassFile)pathInfo).ToCssFileInfo();

                if (!cssFile.Exists)
                {
                    return null;
                }

                var result = File.ReadAllText(cssFile.FullName);

                process.WaitForExit();

                return result;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Initializes the compiler.
        /// </summary>
        private void Initialize()
        {
            if (!this.initialized)
            {
                this.processInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                };

                this.initialized = true;
            }
        }
    }
}