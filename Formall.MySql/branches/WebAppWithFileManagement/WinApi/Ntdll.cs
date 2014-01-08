using System.Runtime.InteropServices;
using System.Text;

namespace System.Win32
{
    public static class Ntdll
    {
        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern IntPtr NtQueryInformationFile(IntPtr fileHandle, ref IO_STATUS_BLOCK IoStatusBlock, IntPtr pInfoBlock, uint length, FILE_INFORMATION_CLASS fileInformation);

        public struct IO_STATUS_BLOCK
        {
            uint status;
            ulong information;
        }

        public struct _FILE_public_INFORMATION
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
            FilepublicInformation      // 6
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetFileInformationByHandle(IntPtr hFile, out BY_HANDLE_FILE_INFORMATION lpFileInformation);

        [DllImport("Kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool GetVolumeInformationByHandleW(IntPtr hFile, StringBuilder VolumeNameBuffer, int VolumeNameSize,
            out uint VolumeSerialNumber, out uint MaximumComponentLength, out uint FileSystemFlags, StringBuilder FileSystemNameBuffer,
            int nFileSystemNameSize);

        /*[DllImport("Kernel32.dll", SetLastError = true)]
        public extern static IntPtr OpenFileById(IntPtr hFile, LPFILE_ID_DESCRIPTOR lpFileID, uint dwDesiredAccess, uint dwShareMode, 
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
}
