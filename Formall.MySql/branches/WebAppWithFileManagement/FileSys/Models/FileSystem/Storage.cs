using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom.Models.FileSystem
{
    public class Storage
    {
        private readonly string _root;
        private readonly DbContext _db;

        public Storage(string root)
            : this(root, string.Empty)
        {
        }

        public Storage(string root, string nameOrConnectionString)
        {
            _root = root ?? string.Empty;
            _db = new DbContext(nameOrConnectionString);
        }

        public File Root
        {
            get { return Query("~").SingleOrDefault() ?? Create(); }
        }

        protected File Create()
        {
            return null;
        }

        public File Create(File folder, System.IO.Stream content)
        {
            Debug.Assert(folder != null && folder.Type == FileType.Folder);

            return null;
        }

        public void Delete()
        {
        }

        public File Query(int id)
        {
            return null;
        }

        public IEnumerable<File> Query(string path)
        {
            if (string.IsNullOrEmpty(path))
                return new File[] { };

            var nodes = path.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

            if (nodes.Length == 0)
                return new File[] { };

            if (nodes[0] == "~")
            {
            }

            return new File[] { };;
        }

        public void Rename()
        {
        }
    }
}
