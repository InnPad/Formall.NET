using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Text;

namespace Custom.Areas.FileSystem.Helpers
{
    using Custom.Areas.FileSystem.Extensions;
    using Models;

    public static class PathHelper
    {
        private static Dictionary<string, string> rowClasses;

        private static int _nextFileId = -1;

        private static int GetNextFileID()
        {
            return System.Threading.Interlocked.Decrement(ref _nextFileId);
        }

        public static System.Threading.ReaderWriterLockSlim _lock;
        private static Dictionary<ulong, string> _paths;

        static PathHelper()
        {
            _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            _paths = new Dictionary<ulong, string>();
            rowClasses = new Dictionary<string, string>();
            rowClasses.Add("7z", "ux-archive-file");
            rowClasses.Add("aac", "ux-audio-file");
            rowClasses.Add("ai", "ux-vector-file");
            rowClasses.Add("avi", "ux-video-file");
            rowClasses.Add("bmp", "ux-image-file");
            rowClasses.Add("divx", "ux-video-file");
            rowClasses.Add("doc", "ux-document-file");
            rowClasses.Add("eps", "ux-vector-file");
            rowClasses.Add("flac", "ux-audio-file");
            rowClasses.Add("flv", "ux-video-file");
            rowClasses.Add("gif", "ux-image-file");
            rowClasses.Add("jpg", "ux-image-file");
            rowClasses.Add("mov", "ux-video-file");
            rowClasses.Add("mp3", "ux-audio-file");
            rowClasses.Add("mpg", "ux-video-file");
            rowClasses.Add("pdf", "ux-acrobat-file");
            rowClasses.Add("png", "ux-image-file");
            rowClasses.Add("pps", "ux-presentation-file");
            rowClasses.Add("ppt", "ux-presentation-file");
            rowClasses.Add("rar", "ux-archive-file");
            rowClasses.Add("psd", "ux-image-file");
            rowClasses.Add("svg", "ux-vector-file");
            rowClasses.Add("swf", "ux-flash-file");
            rowClasses.Add("tif", "ux-image-file");
            rowClasses.Add("txt", "ux-text-file");
            rowClasses.Add("wav", "ux-audio-file");
            rowClasses.Add("wma", "ux-video-file");
            rowClasses.Add("xls", "ux-spreadsheet-file");
            rowClasses.Add("zip", "ux-archive-file");
        }

        public static string PhysicalPath(string relativePath)
        {
            if (relativePath.StartsWith("Root"))
            {
                relativePath = relativePath.Substring(4);
            }

            relativePath = relativePath.Replace("//", "/");

            var localPath = HostingEnvironment.MapPath("~/");

            if (!string.IsNullOrEmpty(relativePath))
            {
                localPath = localPath.Combine(relativePath);
            }

            return localPath;
        }

        public static FolderModel CreateFolder(string relativePath)
        {
            var directory = new DirectoryInfo(PhysicalPath(relativePath));

            if (!directory.Exists)
            {
                directory.Create();
            }

            return new FolderModel
                       {
                           Id = GetFolderId(directory.FullName),
                           Name = directory.Name,
                           Path = directory.FullName
                       };
        }

        public static void MoveFolder(FolderModel folder, string relativePath)
        {
        }

        public static void DeleteFolder(FolderModel folder)
        {
            if (!string.IsNullOrEmpty(folder.Path))
            {
                var directory = new DirectoryInfo(folder.Path);
                if (directory.Exists)
                {
                    directory.Delete();
                }
            }
        }

        public static FolderModel[] GetFolders(string relativePath)
        {
            var folder = new DirectoryInfo(relativePath);
            return folder.Exists
                       ? folder.GetDirectories()
                       .Where(o => o.Name != ".")
                       .Select(o => new FolderModel
                       {
                           Id = GetFolderId(o.FullName),
                           Name = o.Name,
                           Path = o.FullName,
                       }).ToArray() : null;
        }

        public static FileModel[] GetFiles(string relativePath)
        {
            FileModel[] result = null;

            var directory = new DirectoryInfo(PhysicalPath(relativePath));

            if (directory.Exists)
            {
                result = directory.GetFiles()
                    .Where(o => o.Name != ".")
                    .Select((o, index) => Map(o))
                    .ToArray();
            }

            return result;
        }

        private static FileModel Map(FileInfo file)
        {
            ulong id = GetFileUniqueId(file);
            bool mapped;
            string path, fullName = file.FullName;
            if (!(mapped = _paths.TryGetValue(id, out path)) || path != fullName)
            {
                if (mapped)
                    _paths[id] = fullName;
                else
                    _paths.Add(id, fullName);
            }
            
            var model = new FileModel
                           {
                               Id = (ulong)id,
                               Name = file.Name,
                               Type = file.Extension.Substring(1)
                           };

            if (file.Exists)
            {
                model.Size = file.Length;
                model.Date = file.LastAccessTimeUtc;
                model.row_class = null;//"ux-unknown-file",
                model.Thumbnail = null;
            }


            var thumb = "file_" + model.Type + ".png";

            if (File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Areas/Content/Icons/48/") + thumb) == true)
                model.Thumbnail = "../Icons/48/" + thumb;
            else
                model.Thumbnail = "../Icons/48/document_blank.png";

            string rowClass;
            if (rowClasses.TryGetValue(model.Type, out rowClass))
                model.row_class = rowClass;
            else
                model.row_class = "ux-unknown-file";

            return model;
        }

        public static bool TryGetPath(ulong id, out string path)
        {
            path = null;

            bool succeeded = false;

            if (_lock.TryEnterReadLock(1000))
            {
                string value;
                if (_paths.TryGetValue(id, out value))
                {
                    path = value;
                    succeeded = true;
                }

                _lock.ExitReadLock();
            }

            return succeeded;
        }

        public static void MapPath(ulong id, string path)
        {
            if (_lock.TryEnterUpgradeableReadLock(1000))
            {
                bool exists;
                string value;
                if (!(exists = _paths.TryGetValue(id, out value)) || path != value)
                {
                    if (_lock.TryEnterWriteLock(1000))
                    {
                        if (exists)
                            _paths[id] = path;
                        else
                            _paths.Add(id, path);

                        _lock.ExitWriteLock();
                    }
                    else
                    {
                        _lock.ExitReadLock();
                    }
                }
            }
        }

        public static string BasePath
        {
            get { return _basePath ?? (_basePath = HostingEnvironment.ApplicationPhysicalPath); }
        }
        private static string _basePath;
        
        public static VirtualPathProvider FileSystem
        {
            get { return HostingEnvironment.VirtualPathProvider; }
        }

        public static uint GetVolumeSerialNumberId(string path)
        {
            uint volumeSerialNumber = 0;

            IntPtr cFile = NativeMethods.CreateFile(
                path,
                NativeMethods.GENERIC_READ /* 0 or NativeMethods.GENERIC_READ */,
                FileShare.Read,
                IntPtr.Zero /* failed try: NativeMethods.OPEN_ALWAYS */,
                (FileMode)NativeMethods.OPEN_EXISTING,
                NativeMethods.FILE_FLAG_BACKUP_SEMANTICS /* 0 */,
                IntPtr.Zero);

            if ((int)cFile != -1)
            {
                var objectFileInfo = new WinAPI.BY_HANDLE_FILE_INFORMATION();

                WinAPI.GetFileInformationByHandle(cFile, out objectFileInfo);

                NativeMethods.CloseHandle(cFile);

                volumeSerialNumber = objectFileInfo.VolumeSerialNumber;
            }

            return volumeSerialNumber;
        }

        public static ulong GetFolderId(string path)
        {
            ulong folderIndex = 0;

            IntPtr cFile = NativeMethods.CreateFile(
                path,
                NativeMethods.GENERIC_READ /* 0 or NativeMethods.GENERIC_READ */,
                FileShare.Read,
                IntPtr.Zero /* failed try: NativeMethods.OPEN_ALWAYS */,
                (FileMode) NativeMethods.OPEN_EXISTING,
                NativeMethods.FILE_FLAG_BACKUP_SEMANTICS /* 0 */,
                IntPtr.Zero);

            if ((int) cFile != -1)
            {
                var objectFileInfo = new WinAPI.BY_HANDLE_FILE_INFORMATION();

                WinAPI.GetFileInformationByHandle(cFile, out objectFileInfo);

                NativeMethods.CloseHandle(cFile);

                folderIndex = ((ulong)objectFileInfo.FileIndexHigh << 32) | (ulong)objectFileInfo.FileIndexLow;
            }

            return folderIndex;
        }


        public static FileSystemInfo GetInfoById(ulong id, string path)
        {

            // I've actually played with this code for a while myself, and at some point decided to use ZwCreate functions, and others to get this to work.
            // However, recently, I've revisited this, and wanted to know why it didn't work.

            // Well, after going trough the code in assembly, I now know how to get it to work:

            // 1- open a volume handle on the volume where the file ID resides:
            // (remember on vista you need to be administrator to open a volume handle)

            // wsprintf(szVolumePath, TEXT("\\\\.\\%c:"), cDriveLetter[0]);
            /*hDisk=CreateFile(szVolumePath,
            GENERIC_READ,
            FILE_SHARE_READ|FILE_SHARE_WRITE,
            NULL,
            OPEN_EXISTING,
            0,
            NULL);*/

            IntPtr hPath = NativeMethods.CreateFile(
                path,
                NativeMethods.GENERIC_READ /* 0 or NativeMethods.GENERIC_READ */,
                FileShare.Read,
                IntPtr.Zero /* failed try: NativeMethods.OPEN_ALWAYS */,
                (FileMode)NativeMethods.OPEN_EXISTING,
                NativeMethods.FILE_FLAG_BACKUP_SEMANTICS /* 0 */,
                IntPtr.Zero);

            int VolumeNameSize = 1000, nFileSystemNameSize = 1000;
            StringBuilder volumeNameBuffer = new StringBuilder(VolumeNameSize), FileSystemNameBuffer = new StringBuilder(nFileSystemNameSize);
            uint VolumeSerialNumber, MaximumComponentLength, FileSystemFlags;
            if (WinAPI.GetVolumeInformationByHandleW(hPath, volumeNameBuffer, VolumeNameSize, out VolumeSerialNumber,
                                                 out MaximumComponentLength, out FileSystemFlags, FileSystemNameBuffer,
                                                 nFileSystemNameSize))
            {
                var volumeName = volumeNameBuffer.ToString();

                IntPtr hDisk = NativeMethods.CreateFile(
                "\\\\.\\c:",
                NativeMethods.GENERIC_READ /* 0 or NativeMethods.GENERIC_READ */,
                FileShare.ReadWrite,
                IntPtr.Zero /* failed try: NativeMethods.OPEN_ALWAYS */,
                (FileMode)NativeMethods.OPEN_EXISTING,
                0,
                IntPtr.Zero);

                // 2- Once you have a valid disk handle, you need to fill up your fileID stucture with some incorrect values:

                // This value was determined by looking at the assembly code
                // and has no actual corresponance to the structure size which 
                // is 16. Might be related to 64bit version?
                // fileIDDesc.dwSize             = 24;              // expected by OpenFileById
                // fileIDDesc.FileId.QuadPart = index;         // FileID
                // fileIDDesc.Type                 = FileIdType; // enum value

                /*var hFile = OpenFileById(hDisk, 
                      &fileIDDesc,
                      SYNCHRONIZE | FILE_READ_ATTRIBUTES,
                      FILE_SHARE_READ|FILE_SHARE_WRITE,
                      NULL,
                      0 );*/

                // 3- Just make sure that your FileID structure actually has enough size to deal with overflow from OpenFileById.

                // 4- Once you have that, you can do the normal stuff, and interogate your file, here is code to get the filename:

                // GetFileInformationByHandleEx(hFile, FileNameInfo, pFileNameInfo, sizeof(FILE_NAME_INFO) + 1000);

                /*if (pFileNameInfo->FileNameLength > 0)
                {
                }*/
            }

            return null;
        }

        public static int GetPathFromFileReference(ulong frn, out string path)
        {

            DateTime startTime = DateTime.Now;

            path = "Unavailable";

            int usnRtnCode = -1; //  UsnJournalReturnCode.VOLUME_NOT_NTFS;
            
            if (true/*bNtfsVolume*/)
            {
                /*
                if (_usnJournalRootHandle.ToInt32() != Win32Api.INVALID_HANDLE_VALUE)
                {
                    if (frn != 0)
                    {
                        usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;

                        long allocSize = 0;

                        Win32Api.UNICODE_STRING unicodeString;

                        Win32Api.OBJECT_ATTRIBUTES objAttributes = new Win32Api.OBJECT_ATTRIBUTES();

                        Win32Api.IO_STATUS_BLOCK ioStatusBlock = new Win32Api.IO_STATUS_BLOCK();

                        IntPtr hFile = IntPtr.Zero;

                        IntPtr buffer = Marshal.AllocHGlobal(4096);

                        IntPtr refPtr = Marshal.AllocHGlobal(8);

                        IntPtr objAttIntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(objAttributes));

 

                        //

                        // pointer >> fileid

                        //

                        Marshal.WriteInt64(refPtr, (long)frn);

 

                        unicodeString.Length = 8;

                        unicodeString.MaximumLength = 8;

                        unicodeString.Buffer = refPtr;

                        //

                        // copy unicode structure to pointer

                        //

                        Marshal.StructureToPtr(unicodeString, objAttIntPtr, true);

 

                        //

                        //  InitializeObjectAttributes

                        //

                        objAttributes.Length = Marshal.SizeOf(objAttributes);

                        objAttributes.ObjectName = objAttIntPtr;

                        objAttributes.RootDirectory = _usnJournalRootHandle;

                        objAttributes.Attributes = (int)Win32Api.OBJ_CASE_INSENSITIVE;

 

                        int fOk = Win32Api.NtCreateFile(

                            ref hFile,

                            FileAccess.Read,

                            ref objAttributes,

                            ref ioStatusBlock,

                            ref allocSize,

                            0,

                            FileShare.ReadWrite,

                            Win32Api.FILE_OPEN,

                            Win32Api.FILE_OPEN_BY_FILE_ID | Win32Api.FILE_OPEN_FOR_BACKUP_INTENT,

                            IntPtr.Zero, 0);

                        if (fOk == 0)

                        {

                            fOk = Win32Api.NtQueryInformationFile(

                                hFile,

                                ref ioStatusBlock,

                                buffer,

                                4096,

                                Win32Api.FILE_INFORMATION_CLASS.FileNameInformation);

                            if (fOk == 0)

                            {

                                //

                                // first 4 bytes are the name length

                                //

                                int nameLength = Marshal.ReadInt32(buffer, 0);

                                //

                                // next bytes are the name

                                //

                                path = Marshal.PtrToStringUni(new IntPtr(buffer.ToInt32() + 4), nameLength / 2);

                            }

                        }

                        Win32Api.CloseHandle(hFile);

                        Marshal.FreeHGlobal(buffer);

                        Marshal.FreeHGlobal(objAttIntPtr);

                        Marshal.FreeHGlobal(refPtr);

                    }

                }*/
            }

            //_elapsedTime = DateTime.Now - startTime;

            return usnRtnCode;
        }

        /*public static ulong GetFileUniqueId(FileInfo fi)
        {
            var iostatus = new WinAPI.IO_STATUS_BLOCK();

            var objectIDInfo = new WinAPI._FILE_INTERNAL_INFORMATION();

            int structSize = Marshal.SizeOf(objectIDInfo);

            var fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            IntPtr memPtr;

            unsafe
            {
                memPtr = Marshal.AllocHGlobal(sizeof(WinAPI._FILE_INTERNAL_INFORMATION));
            }

            IntPtr res = WinAPI.NtQueryInformationFile(fs.Handle, ref iostatus, memPtr, (uint)structSize,
                                                       WinAPI.FILE_INFORMATION_CLASS.FileInternalInformation);

            objectIDInfo =
                (WinAPI._FILE_INTERNAL_INFORMATION)
                Marshal.PtrToStructure(memPtr, typeof(WinAPI._FILE_INTERNAL_INFORMATION));

            fs.Close();

            Marshal.FreeHGlobal(memPtr);

            return objectIDInfo.IndexNumber;
        }*/

        public static ulong GetFileUniqueId(FileInfo fi)
        {
            var objectFileInfo = new WinAPI.BY_HANDLE_FILE_INFORMATION();

            var fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            WinAPI.GetFileInformationByHandle(fs.Handle, out objectFileInfo);

            fs.Close();

            var fileIndex = ((ulong)objectFileInfo.FileIndexHigh << 32) | (ulong)objectFileInfo.FileIndexLow;

            //objectFileInfo.VolumeSerialNumber;

            return fileIndex;
        }
    }

    public class WinAPI
    {
        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern IntPtr NtQueryInformationFile(IntPtr fileHandle, ref IO_STATUS_BLOCK IoStatusBlock, IntPtr pInfoBlock, uint length, FILE_INFORMATION_CLASS fileInformation);

        public struct IO_STATUS_BLOCK
        {
            uint status;
            ulong information;
        }
        public struct _FILE_INTERNAL_INFORMATION
        {
            public ulong IndexNumber;
        }

        // Abbreviated, there are more values than shown
        public enum FILE_INFORMATION_CLASS
        {
            FileDirectoryInformation = 1,     // 1
            FileFullDirectoryInformation,     // 2
            FileBothDirectoryInformation,     // 3
            FileBasicInformation,         // 4
            FileStandardInformation,      // 5
            FileInternalInformation      // 6
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetFileInformationByHandle(IntPtr hFile, out BY_HANDLE_FILE_INFORMATION lpFileInformation);

        [DllImport("Kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal extern static bool GetVolumeInformationByHandleW(IntPtr hFile, StringBuilder VolumeNameBuffer, int VolumeNameSize,
            out uint VolumeSerialNumber, out uint MaximumComponentLength, out uint FileSystemFlags, StringBuilder FileSystemNameBuffer,
            int nFileSystemNameSize);

        /*[DllImport("Kernel32.dll", SetLastError = true)]
        internal extern static IntPtr OpenFileById(IntPtr hFile, LPFILE_ID_DESCRIPTOR lpFileID, uint dwDesiredAccess, uint dwShareMode, 
            LPSECURITY_ATTRIBUTES lpSecurityAttributes, uint dwFlags);*/


        public struct BY_HANDLE_FILE_INFORMATION
        {
            public uint FileAttributes;
            public FILETIME CreationTime;
            public FILETIME LastAccessTime;
            public FILETIME LastWriteTime;
            public uint VolumeSerialNumber;
            public uint FileSizeHigh;
            public uint FileSizeLow;
            public uint NumberOfLinks;
            public uint FileIndexHigh;
            public uint FileIndexLow;
        }
    }

    [SuppressUnmanagedCodeSecurity()]
    internal static class NativeMethods
    {
        // Win32 constants for accessing files.
        internal const int GENERIC_READ = unchecked((int)0x80000000);

        internal const int FILE_FLAG_BACKUP_SEMANTICS = unchecked((int)0x02000000);

        internal const int OPEN_EXISTING = unchecked((int)3);

        // Allocate a file object in the kernel, then return a handle to it.
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        internal extern static IntPtr CreateFile(
           String fileName,
           int dwDesiredAccess,
           System.IO.FileShare dwShareMode,
           IntPtr securityAttrs_MustBeZero,
           System.IO.FileMode dwCreationDisposition,
           int dwFlagsAndAttributes,
           IntPtr hTemplateFile_MustBeZero);

        // Use the file handle.
        [DllImport("kernel32", SetLastError = true)]
        internal extern static int ReadFile(
           IntPtr handle,
           byte[] bytes,
           int numBytesToRead,
           out int numBytesRead,
           IntPtr overlapped_MustBeZero);

        // Free the kernel's file object (close the file).
        [DllImport("kernel32", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        internal extern static bool CloseHandle(IntPtr handle);
    }
}