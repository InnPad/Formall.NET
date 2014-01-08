using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Win32;

namespace Custom.Models.FileSystem
{
    using File = System.IO.File;

    internal static class StorageHelper
    {
        public static FileSystemInfo GetFileSystemInfo(string path)
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

        public static Ntdll.BY_HANDLE_FILE_INFORMATION GetNativeFileSystemInfo(FileSystemInfo fsi)
        {
        }
    }
}
