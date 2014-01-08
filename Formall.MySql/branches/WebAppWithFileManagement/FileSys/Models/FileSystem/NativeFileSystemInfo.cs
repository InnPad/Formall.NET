using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Win32;

namespace Custom.Models.FileSystem
{
    using File = System.IO.File;

    public class NativeFileSystemInfo : FileSystemInfo
    {
        public NativeFileSystemInfo(System.IO.FileSystemInfo copy)
        {
            
        }

        public System.IO.FileAttributes Attributes { get; set; }

        public override bool Exists
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsFolder { get { return (System.IO.FileAttributes.Directory == (System.IO.FileAttributes.Directory & Attributes)); } }

        public bool IsFile { get { return !IsFolder; } }

        public override string Name
        {
            get { throw new NotImplementedException(); }
            set { }
        }

        public string Path { get; set; }

        public uint VolumeSerialNumber { get; set; }

        public ulong FileIndex
        {
            get { return ((ulong)FileIndexHigh << 32) | FileIndexLow; }
        }

        public uint FileIndexHigh { get; set; }

        public uint FileIndexLow { get; set; }

        public uint FileAttributes { get; set; }

        public ulong FileSize
        {
            get { return ((ulong)FileSizeHigh << 32) | FileSizeLow; }
        }

        public uint FileSizeHigh { get; set; }

        public uint FileSizeLow { get; set; }

        public uint NumberOfLinks { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastAccessTime { get; set; }

        public DateTime LastWriteTime { get; set; }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        private static Ntdll.BY_HANDLE_FILE_INFORMATION GetNativeInfo(string absolutePath)
        {
            Ntdll.BY_HANDLE_FILE_INFORMATION objectFileInfo;

            try
            {
                if (File.Exists(absolutePath))
                {
                    var attributes = File.GetAttributes(absolutePath);

                    if (FileAttributes.Directory == (FileAttributes.Directory & attributes))
                    {
                        var directoryInfo = new DirectoryInfo(absolutePath);

                        IntPtr cFile = Kernel32.CreateFile(
                            absolutePath,
                            Kernel32.GENERIC_READ /* 0 or Kernel32.GENERIC_READ */,
                            FileShare.Read,
                            IntPtr.Zero /* failed try: Kernel32.OPEN_ALWAYS */,
                            (FileMode)Kernel32.OPEN_EXISTING,
                            Kernel32.FILE_FLAG_BACKUP_SEMANTICS /* 0 */,
                            IntPtr.Zero);

                        if ((int)cFile != -1)
                        {
                            Ntdll.GetFileInformationByHandle(cFile, out objectFileInfo);

                            Kernel32.CloseHandle(cFile);
                        }
                    }
                    else
                    {
                        var fileStream = File.Open(absolutePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        Ntdll.GetFileInformationByHandle(fileStream.Handle, out objectFileInfo);

                        fileStream.Close();
                    }

                    //content.Attributes = attributes;

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
            catch (UnauthorizedAccessException)
            {
            }
            catch (Exception)
            {
            }

            return objectFileInfo;
        }
    }
}
