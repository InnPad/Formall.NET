using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System.Win32
{
    public static class Kernel32
    {
        // Win32 constants for accessing files.
        public const int GENERIC_READ = unchecked((int)0x80000000);

        public const int FILE_FLAG_BACKUP_SEMANTICS = unchecked((int)0x02000000);

        public const int OPEN_EXISTING = unchecked((int)3);

        // Allocate a file object in the kernel, then return a handle to it.
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static IntPtr CreateFile(
           String fileName,
           int dwDesiredAccess,
           System.IO.FileShare dwShareMode,
           IntPtr securityAttrs_MustBeZero,
           System.IO.FileMode dwCreationDisposition,
           int dwFlagsAndAttributes,
           IntPtr hTemplateFile_MustBeZero);

        // Use the file handle.
        [DllImport("kernel32", SetLastError = true)]
        public extern static int ReadFile(
           IntPtr handle,
           byte[] bytes,
           int numBytesToRead,
           out int numBytesRead,
           IntPtr overlapped_MustBeZero);

        // Free the kernel's file object (close the file).
        [DllImport("kernel32", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public extern static bool CloseHandle(IntPtr handle);
    }
}
