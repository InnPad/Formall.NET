using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Win32;

namespace Custom.Storage
{
    public static class ContentHelper
    {
        internal static readonly string Root;

        internal static readonly Dictionary<ulong, string> IndexNumberToRelativePath;

        internal static readonly Dictionary<int, ulong> HashCodeToFileIndexNumber;

        static ContentHelper()
        {
            Root = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath.TrimEnd('\\', ' ');
            IndexNumberToRelativePath = new Dictionary<ulong, string>();
            HashCodeToFileIndexNumber = new Dictionary<int, ulong>();
        }

        public static Models.FolderModel QueryFolder(DirectoryInfo info)
        {
            if (!info.Exists)
                throw new ArgumentException("Folder does not exist");
            else if (FileAttributes.Directory != (FileAttributes.Directory & info.Attributes))
                throw new ArgumentException("path is not a folder");

            string relativePath = info.FullName.Substring(Root.Length);

            var hashCode = relativePath.GetHashCode();

            ulong indexNumber;
            string storedPath;

            if (!HashCodeToFileIndexNumber.TryGetValue(hashCode, out indexNumber) || !IndexNumberToRelativePath.TryGetValue(indexNumber, out storedPath) || !string.Equals(storedPath, relativePath, StringComparison.InvariantCultureIgnoreCase))
            {
                IntPtr hFolder = Kernel32.CreateFile(
                    info.FullName,
                    Kernel32.GENERIC_READ /* 0 or Kernel32.GENERIC_READ */,
                    FileShare.Read,
                    IntPtr.Zero /* failed try: Kernel32.OPEN_ALWAYS */,
                    (FileMode)Kernel32.OPEN_EXISTING,
                    Kernel32.FILE_FLAG_BACKUP_SEMANTICS /* 0 */,
                    IntPtr.Zero);

                if ((int)hFolder == -1)
                    throw new Exception("Application INternal error");

                Ntdll.BY_HANDLE_FILE_INFORMATION objectFileInfo;

                Ntdll.GetFileInformationByHandle(hFolder, out objectFileInfo);

                Kernel32.CloseHandle(hFolder);

                indexNumber = (ulong)objectFileInfo.FileIndexHigh << 32 | objectFileInfo.FileIndexLow;

                if (IndexNumberToRelativePath.ContainsKey(indexNumber))
                {
                    IndexNumberToRelativePath.Add(indexNumber, relativePath);
                }
                else
                {
                    IndexNumberToRelativePath[indexNumber] = relativePath;
                }

                if (HashCodeToFileIndexNumber.ContainsKey(hashCode))
                {
                    HashCodeToFileIndexNumber[hashCode] = indexNumber;
                }
                else
                {
                    HashCodeToFileIndexNumber.Add(hashCode, indexNumber);
                }
            }

            return new Models.FolderModel
            {
                Id = indexNumber,
                Name = info.Name
            };
        }

        public static string GetRelativePath(ulong indexNumber)
        {
            if (indexNumber == 0)
                return string.Empty;

            string storedPath;
            if (!IndexNumberToRelativePath.TryGetValue(indexNumber, out storedPath))
                throw new ArgumentException("Index not found");

            return storedPath;
        }

        public static Models.FolderModel QueryFolderAndChildFolders(string path)
        {
            var absolutePath = Root + path;

            var info = new DirectoryInfo(absolutePath);

            var model = QueryFolder(info);

            var children = new List<Models.FolderModel>();

            foreach (var childInfo in info.GetDirectories())
            {
                children.Add(QueryFolder(childInfo));
            }

            model.Children = children;

            return model;
        }

        //http://twitter.com/graymalkinsblog http://practicalhardware.blogspot.com/
        /*public static bool Find(Models.Content content)
        {
            // http://msdn.microsoft.com/en-us/library/windows/desktop/aa387254(v=vs.85).aspx
            var searcher = new System.Management.ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                var sn = queryObj["SerialNumber"];
            }

            return false;
        }*/
    }
}
