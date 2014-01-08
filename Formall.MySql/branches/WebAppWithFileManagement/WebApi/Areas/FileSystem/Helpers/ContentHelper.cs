using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Web;

namespace Custom.Areas.FileSystem.Helpers
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
                IntPtr hFolder = NativeMethods.CreateFile(
                    info.FullName,
                    NativeMethods.GENERIC_READ /* 0 or NativeMethods.GENERIC_READ */,
                    FileShare.Read,
                    IntPtr.Zero /* failed try: NativeMethods.OPEN_ALWAYS */,
                    (FileMode)NativeMethods.OPEN_EXISTING,
                    NativeMethods.FILE_FLAG_BACKUP_SEMANTICS /* 0 */,
                    IntPtr.Zero);

                if ((int)hFolder == -1)
                    throw new Exception("Application INternal error");

                WinAPI.BY_HANDLE_FILE_INFORMATION objectFileInfo;

                WinAPI.GetFileInformationByHandle(hFolder, out objectFileInfo);

                NativeMethods.CloseHandle(hFolder);

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

        public static Models.NativeFileSystemInfo GetContent(string path)
        {
            Models.NativeFileSystemInfo content = null;

            try
            {
                var absolutePath = Path.GetFullPath(path);

                if (File.Exists(absolutePath))
                {
                    var attributes = File.GetAttributes(absolutePath);
                    if (FileAttributes.Directory == (FileAttributes.Directory & attributes))
                    {
                        var directoryInfo = new DirectoryInfo(absolutePath);

                        content = GetFolder(directoryInfo);
                    }
                    else
                    {
                        var fileInfo = new FileInfo(absolutePath);

                        content = GetFile(fileInfo);
                    }

                    content.Attributes = attributes;
                }
            }
            catch (UnauthorizedAccessException)
            {
                content = null;
            }
            catch (Exception)
            {
                content = null;
            }

            return content;
        }

        public static FileSystemInfo Info(string path)
        {
            FileSystemInfo info = null;

            try
            {
                var absolutePath = Path.GetFullPath(path);

                if (File.Exists(absolutePath))
                {
                    info = (FileAttributes.Directory == (FileAttributes.Directory & File.GetAttributes(absolutePath)))
                        ? (FileSystemInfo)new DirectoryInfo(absolutePath)
                        : (FileSystemInfo)new FileInfo(absolutePath);
                }
            }
            catch (UnauthorizedAccessException)
            {
                info = null;
            }
            catch (Exception)
            {
                info = null;
            }

            return info;
        }

        public static Models.NativeFileSystemInfo GetFile(FileInfo fileInfo)
        {
            Models.NativeFileSystemInfo content = null;

            if (fileInfo.Exists)
            {
                WinAPI.BY_HANDLE_FILE_INFORMATION objectFileInfo;

                var fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                WinAPI.GetFileInformationByHandle(fileStream.Handle, out objectFileInfo);

                fileStream.Close();

                content = CreateContent(objectFileInfo);
            }

            return content;
        }

        public static Models.NativeFileSystemInfo GetFolder(DirectoryInfo directoryInfo)
        {
            Models.NativeFileSystemInfo content = null;

            ulong folderIndex = 0;

            IntPtr cFile = NativeMethods.CreateFile(
                directoryInfo.FullName,
                NativeMethods.GENERIC_READ /* 0 or NativeMethods.GENERIC_READ */,
                FileShare.Read,
                IntPtr.Zero /* failed try: NativeMethods.OPEN_ALWAYS */,
                (FileMode)NativeMethods.OPEN_EXISTING,
                NativeMethods.FILE_FLAG_BACKUP_SEMANTICS /* 0 */,
                IntPtr.Zero);

            if ((int)cFile != -1)
            {
                WinAPI.BY_HANDLE_FILE_INFORMATION objectFileInfo;

                WinAPI.GetFileInformationByHandle(cFile, out objectFileInfo);

                NativeMethods.CloseHandle(cFile);

                content = CreateContent(objectFileInfo);
            }

            return content;
        }

        private static NativeFileSystemInfo CreateContent(WinAPI.BY_HANDLE_FILE_INFORMATION objectFileInfo)
        {
            return new NativeFileSystemInfo
            {
                VolumeSerialNumber = objectFileInfo.VolumeSerialNumber,
                FileIndexHigh = objectFileInfo.FileIndexHigh,
                FileIndexLow = objectFileInfo.FileIndexLow,
                FileAttributes = objectFileInfo.FileAttributes,
                FileSizeHigh = objectFileInfo.FileSizeHigh,
                FileSizeLow = objectFileInfo.FileSizeLow,
                NumberOfLinks = objectFileInfo.NumberOfLinks,
                CreationTime = DateTime.FromFileTime(((long)objectFileInfo.CreationTime.dwHighDateTime << 32) | objectFileInfo.CreationTime.dwLowDateTime),
                LastAccessTime = DateTime.FromFileTime(((long)objectFileInfo.LastAccessTime.dwHighDateTime << 32) | objectFileInfo.LastAccessTime.dwLowDateTime),
                LastWriteTime = DateTime.FromFileTime(((long)objectFileInfo.LastWriteTime.dwHighDateTime << 32) | objectFileInfo.LastWriteTime.dwLowDateTime),
            };
        }
    }
}