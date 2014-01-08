using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Custom.Areas.FileSystem.Helpers
{
    /*
    0001
using System;
0002
using System.Collections.Generic;
0003
using System.Linq;
0004
using System.Text;
0005
using System.IO;
0006
 
0007
using PInvoke;
0008
using System.ComponentModel;
0009
using System.Runtime.InteropServices;
0010
using Microsoft.Win32.SafeHandles;
0011
 
0012
namespace UsnJournal
0013
{
0014
    public class NtfsUsnJournal : IDisposable
0015
    {
0016
        #region enum(s)
0017
        public enum UsnJournalReturnCode
0018
        {
0019
            INVALID_HANDLE_VALUE = -1,
0020
            USN_JOURNAL_SUCCESS = 0,
0021
            ERROR_INVALID_FUNCTION = 1,
0022
            ERROR_FILE_NOT_FOUND = 2,
0023
            ERROR_PATH_NOT_FOUND = 3,
0024
            ERROR_TOO_MANY_OPEN_FILES = 4,
0025
            ERROR_ACCESS_DENIED = 5,
0026
            ERROR_INVALID_HANDLE = 6,
0027
            ERROR_INVALID_DATA = 13,
0028
            ERROR_HANDLE_EOF = 38,
0029
            ERROR_NOT_SUPPORTED = 50,
0030
            ERROR_INVALID_PARAMETER = 87,
0031
            ERROR_JOURNAL_DELETE_IN_PROGRESS = 1178,
0032
            USN_JOURNAL_NOT_ACTIVE = 1179,
0033
            ERROR_JOURNAL_ENTRY_DELETED = 1181,
0034
            ERROR_INVALID_USER_BUFFER = 1784,
0035
            USN_JOURNAL_INVALID = 17001,
0036
            VOLUME_NOT_NTFS = 17003,
0037
            INVALID_FILE_REFERENCE_NUMBER = 17004,
0038
            USN_JOURNAL_ERROR = 17005
0039
        }
0040
 
0041
        public enum UsnReasonCode
0042
        {
0043
            USN_REASON_DATA_OVERWRITE = 0x00000001,
0044
            USN_REASON_DATA_EXTEND = 0x00000002,
0045
            USN_REASON_DATA_TRUNCATION = 0x00000004,
0046
            USN_REASON_NAMED_DATA_OVERWRITE = 0x00000010,
0047
            USN_REASON_NAMED_DATA_EXTEND = 0x00000020,
0048
            USN_REASON_NAMED_DATA_TRUNCATION = 0x00000040,
0049
            USN_REASON_FILE_CREATE = 0x00000100,
0050
            USN_REASON_FILE_DELETE = 0x00000200,
0051
            USN_REASON_EA_CHANGE = 0x00000400,
0052
            USN_REASON_SECURITY_CHANGE = 0x00000800,
0053
            USN_REASON_RENAME_OLD_NAME = 0x00001000,
0054
            USN_REASON_RENAME_NEW_NAME = 0x00002000,
0055
            USN_REASON_INDEXABLE_CHANGE = 0x00004000,
0056
            USN_REASON_BASIC_INFO_CHANGE = 0x00008000,
0057
            USN_REASON_HARD_LINK_CHANGE = 0x00010000,
0058
            USN_REASON_COMPRESSION_CHANGE = 0x00020000,
0059
            USN_REASON_ENCRYPTION_CHANGE = 0x00040000,
0060
            USN_REASON_OBJECT_ID_CHANGE = 0x00080000,
0061
            USN_REASON_REPARSE_POINT_CHANGE = 0x00100000,
0062
            USN_REASON_STREAM_CHANGE = 0x00200000,
0063
            USN_REASON_CLOSE = -1
0064
        }
0065
 
0066
        #endregion
0067
 
0068
        #region private member variables
0069
 
0070
        private DriveInfo _driveInfo = null;
0071
        private uint _volumeSerialNumber;
0072
        private IntPtr _usnJournalRootHandle;
0073
 
0074
        private bool bNtfsVolume;
0075
 
0076
        #endregion
0077
 
0078
        #region properties
0079
 
0080
        private static TimeSpan _elapsedTime;
0081
        public static TimeSpan ElapsedTime
0082
        {
0083
            get { return _elapsedTime; }
0084
        }
0085
 
0086
        public string VolumeName
0087
        {
0088
            get { return _driveInfo.Name; }
0089
        }
0090
 
0091
        public long AvailableFreeSpace
0092
        {
0093
            get { return _driveInfo.AvailableFreeSpace; }
0094
        }
0095
 
0096
        public long TotalFreeSpace
0097
        {
0098
            get { return _driveInfo.TotalFreeSpace; }
0099
        }
0100
 
0101
        public string Format
0102
        {
0103
            get { return _driveInfo.DriveFormat; }
0104
        }
0105
 
0106
        public DirectoryInfo RootDirectory
0107
        {
0108
            get { return _driveInfo.RootDirectory; }
0109
        }
0110
 
0111
        public long TotalSize
0112
        {
0113
            get { return _driveInfo.TotalSize; }
0114
        }
0115
 
0116
        public string VolumeLabel
0117
        {
0118
            get { return _driveInfo.VolumeLabel; }
0119
        }
0120
 
0121
        public uint VolumeSerialNumber
0122
        {
0123
            get { return _volumeSerialNumber; }
0124
        }
0125
 
0126
        #endregion
0127
 
0128
        #region constructor(s)
0129
 
0130
        /// <summary>
0131
        /// Constructor for NtfsUsnJournal class.  If no exception is thrown, _usnJournalRootHandle and
0132
        /// _volumeSerialNumber can be assumed to be good. If an exception is thrown, the NtfsUsnJournal
0133
        /// object is not usable.
0134
        /// </summary>
0135
        /// <param name="driveInfo">DriveInfo object that provides access to information about a volume</param>
0136
        /// <remarks>
0137
        /// An exception thrown if the volume is not an 'NTFS' volume or
0138
        /// if GetRootHandle() or GetVolumeSerialNumber() functions fail.
0139
        /// Each public method checks to see if the volume is NTFS and if the _usnJournalRootHandle is
0140
        /// valid.  If these two conditions aren't met, then the public function will return a UsnJournalReturnCode
0141
        /// error.
0142
        /// </remarks>
0143
        public NtfsUsnJournal(DriveInfo driveInfo)
0144
        {
0145
            DateTime start = DateTime.Now;
0146
            _driveInfo = driveInfo;
0147
 
0148
            if (0 == string.Compare(_driveInfo.DriveFormat, "ntfs", true))
0149
            {
0150
                bNtfsVolume = true;
0151
 
0152
                IntPtr rootHandle = IntPtr.Zero;
0153
                UsnJournalReturnCode usnRtnCode = GetRootHandle(out rootHandle);
0154
 
0155
                if (usnRtnCode == UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
0156
                {
0157
                    _usnJournalRootHandle = rootHandle;
0158
                    usnRtnCode = GetVolumeSerialNumber(_driveInfo, out _volumeSerialNumber);
0159
                    if (usnRtnCode != UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
0160
                    {
0161
                        _elapsedTime = DateTime.Now - start;
0162
                        throw new Win32Exception((int)usnRtnCode);
0163
                    }
0164
                }
0165
                else
0166
                {
0167
                    _elapsedTime = DateTime.Now - start;
0168
                    throw new Win32Exception((int)usnRtnCode);
0169
                }
0170
            }
0171
            else
0172
            {
0173
                _elapsedTime = DateTime.Now - start;
0174
                throw new Exception(string.Format("{0} is not an 'NTFS' volume.", _driveInfo.Name));
0175
            }
0176
            _elapsedTime = DateTime.Now - start;
0177
        }
0178
 
0179
        #endregion
0180
 
0181
        #region public methods
0182
 
0183
        /// <summary>
0184
        /// CreateUsnJournal() creates a usn journal on the volume. If a journal already exists this function
0185
        /// will adjust the MaximumSize and AllocationDelta parameters of the journal if the requested size
0186
        /// is larger.
0187
        /// </summary>
0188
        /// <param name="maxSize">maximum size requested for the UsnJournal</param>
0189
        /// <param name="allocationDelta">when space runs out, the amount of additional
0190
        /// space to allocate</param>
0191
        /// <param name="elapsedTime">The TimeSpan object indicating how much time this function
0192
        /// took</param>
0193
        /// <returns>a UsnJournalReturnCode
0194
        /// USN_JOURNAL_SUCCESS                 CreateUsnJournal() function succeeded.
0195
        /// VOLUME_NOT_NTFS                     volume is not an NTFS volume.
0196
        /// INVALID_HANDLE_VALUE                NtfsUsnJournal object failed initialization.
0197
        /// USN_JOURNAL_NOT_ACTIVE              usn journal is not active on volume.
0198
        /// ERROR_ACCESS_DENIED                 accessing the usn journal requires admin rights, see remarks.
0199
        /// ERROR_INVALID_FUNCTION              error generated by DeviceIoControl() call.
0200
        /// ERROR_FILE_NOT_FOUND                error generated by DeviceIoControl() call.
0201
        /// ERROR_PATH_NOT_FOUND                error generated by DeviceIoControl() call.
0202
        /// ERROR_TOO_MANY_OPEN_FILES           error generated by DeviceIoControl() call.
0203
        /// ERROR_INVALID_HANDLE                error generated by DeviceIoControl() call.
0204
        /// ERROR_INVALID_DATA                  error generated by DeviceIoControl() call.
0205
        /// ERROR_NOT_SUPPORTED                 error generated by DeviceIoControl() call.
0206
        /// ERROR_INVALID_PARAMETER             error generated by DeviceIoControl() call.
0207
        /// ERROR_JOURNAL_DELETE_IN_PROGRESS    usn journal delete is in progress.
0208
        /// ERROR_INVALID_USER_BUFFER           error generated by DeviceIoControl() call.
0209
        /// USN_JOURNAL_ERROR                   unspecified usn journal error.
0210
        /// </returns>
0211
        /// <remarks>
0212
        /// If function returns ERROR_ACCESS_DENIED you need to run application as an Administrator.
0213
        /// </remarks>
0214
        public UsnJournalReturnCode
0215
            CreateUsnJournal(ulong maxSize, ulong allocationDelta)
0216
        {
0217
            UsnJournalReturnCode usnRtnCode = UsnJournalReturnCode.VOLUME_NOT_NTFS;
0218
            DateTime startTime = DateTime.Now;
0219
 
0220
            if (bNtfsVolume)
0221
            {
0222
                if (_usnJournalRootHandle.ToInt32() != Win32Api.INVALID_HANDLE_VALUE)
0223
                {
0224
                    usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
0225
                    UInt32 cb;
0226
 
0227
                    Win32Api.CREATE_USN_JOURNAL_DATA cujd = new Win32Api.CREATE_USN_JOURNAL_DATA();
0228
                    cujd.MaximumSize = maxSize;
0229
                    cujd.AllocationDelta = allocationDelta;
0230
 
0231
                    int sizeCujd = Marshal.SizeOf(cujd);
0232
                    IntPtr cujdBuffer = Marshal.AllocHGlobal(sizeCujd);
0233
                    Win32Api.ZeroMemory(cujdBuffer, sizeCujd);
0234
                    Marshal.StructureToPtr(cujd, cujdBuffer, true);
0235
 
0236
                    bool fOk = Win32Api.DeviceIoControl(
0237
                        _usnJournalRootHandle,
0238
                        Win32Api.FSCTL_CREATE_USN_JOURNAL,
0239
                        cujdBuffer,
0240
                        sizeCujd,
0241
                        IntPtr.Zero,
0242
                        0,
0243
                        out cb,
0244
                        IntPtr.Zero);
0245
                    if (!fOk)
0246
                    {
0247
                        usnRtnCode = ConvertWin32ErrorToUsnError((Win32Api.GetLastErrorEnum)Marshal.GetLastWin32Error());
0248
                    }
0249
                    Marshal.FreeHGlobal(cujdBuffer);
0250
                }
0251
                else
0252
                {
0253
                    usnRtnCode = UsnJournalReturnCode.INVALID_HANDLE_VALUE;
0254
                }
0255
            }
0256
 
0257
            _elapsedTime = DateTime.Now - startTime;
0258
            return usnRtnCode;
0259
        }
0260
 
0261
        /// <summary>
0262
        /// DeleteUsnJournal() deletes a usn journal on the volume. If no usn journal exists, this
0263
        /// function simply returns success.
0264
        /// </summary>
0265
        /// <param name="journalState">USN_JOURNAL_DATA object for this volume</param>
0266
        /// <param name="elapsedTime">The TimeSpan object indicating how much time this function
0267
        /// took</param>
0268
        /// <returns>a UsnJournalReturnCode
0269
        /// USN_JOURNAL_SUCCESS                 DeleteUsnJournal() function succeeded.
0270
        /// VOLUME_NOT_NTFS                     volume is not an NTFS volume.
0271
        /// INVALID_HANDLE_VALUE                NtfsUsnJournal object failed initialization.
0272
        /// USN_JOURNAL_NOT_ACTIVE              usn journal is not active on volume.
0273
        /// ERROR_ACCESS_DENIED                 accessing the usn journal requires admin rights, see remarks.
0274
        /// ERROR_INVALID_FUNCTION              error generated by DeviceIoControl() call.
0275
        /// ERROR_FILE_NOT_FOUND                error generated by DeviceIoControl() call.
0276
        /// ERROR_PATH_NOT_FOUND                error generated by DeviceIoControl() call.
0277
        /// ERROR_TOO_MANY_OPEN_FILES           error generated by DeviceIoControl() call.
0278
        /// ERROR_INVALID_HANDLE                error generated by DeviceIoControl() call.
0279
        /// ERROR_INVALID_DATA                  error generated by DeviceIoControl() call.
0280
        /// ERROR_NOT_SUPPORTED                 error generated by DeviceIoControl() call.
0281
        /// ERROR_INVALID_PARAMETER             error generated by DeviceIoControl() call.
0282
        /// ERROR_JOURNAL_DELETE_IN_PROGRESS    usn journal delete is in progress.
0283
        /// ERROR_INVALID_USER_BUFFER           error generated by DeviceIoControl() call.
0284
        /// USN_JOURNAL_ERROR                   unspecified usn journal error.
0285
        /// </returns>
0286
        /// <remarks>
0287
        /// If function returns ERROR_ACCESS_DENIED you need to run application as an Administrator.
0288
        /// </remarks>
0289
        public UsnJournalReturnCode
0290
            DeleteUsnJournal(Win32Api.USN_JOURNAL_DATA journalState)
0291
        {
0292
            UsnJournalReturnCode usnRtnCode = UsnJournalReturnCode.VOLUME_NOT_NTFS;
0293
            DateTime startTime = DateTime.Now;
0294
 
0295
            if (bNtfsVolume)
0296
            {
0297
                if (_usnJournalRootHandle.ToInt32() != Win32Api.INVALID_HANDLE_VALUE)
0298
                {
0299
                    usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
0300
                    UInt32 cb;
0301
 
0302
                    Win32Api.DELETE_USN_JOURNAL_DATA dujd = new Win32Api.DELETE_USN_JOURNAL_DATA();
0303
                    dujd.UsnJournalID = journalState.UsnJournalID;
0304
                    dujd.DeleteFlags = (UInt32)Win32Api.UsnJournalDeleteFlags.USN_DELETE_FLAG_DELETE;
0305
 
0306
                    int sizeDujd = Marshal.SizeOf(dujd);
0307
                    IntPtr dujdBuffer = Marshal.AllocHGlobal(sizeDujd);
0308
                    Win32Api.ZeroMemory(dujdBuffer, sizeDujd);
0309
                    Marshal.StructureToPtr(dujd, dujdBuffer, true);
0310
 
0311
                    bool fOk = Win32Api.DeviceIoControl(
0312
                        _usnJournalRootHandle,
0313
                        Win32Api.FSCTL_DELETE_USN_JOURNAL,
0314
                        dujdBuffer,
0315
                        sizeDujd,
0316
                        IntPtr.Zero,
0317
                        0,
0318
                        out cb,
0319
                        IntPtr.Zero);
0320
 
0321
                    if (!fOk)
0322
                    {
0323
                        usnRtnCode = ConvertWin32ErrorToUsnError((Win32Api.GetLastErrorEnum)Marshal.GetLastWin32Error());
0324
                    }
0325
                    Marshal.FreeHGlobal(dujdBuffer);
0326
                }
0327
                else
0328
                {
0329
                    usnRtnCode = UsnJournalReturnCode.INVALID_HANDLE_VALUE;
0330
                }
0331
            }
0332
 
0333
            _elapsedTime = DateTime.Now - startTime;
0334
            return usnRtnCode;
0335
        }
0336
 
0337
        /// <summary>
0338
        /// GetNtfsVolumeFolders() reads the Master File Table to find all of the folders on a volume
0339
        /// and returns them in a SortedList<UInt64, Win32Api.UsnEntry> folders out parameter.
0340
        /// </summary>
0341
        /// <param name="folders">A SortedList<string, UInt64> list where string is
0342
        /// the filename and UInt64 is the parent folder's file reference number
0343
        /// </param>
0344
        /// <param name="elapsedTime">A TimeSpan object that on return holds the elapsed time
0345
        /// </param>
0346
        /// <returns>
0347
        /// USN_JOURNAL_SUCCESS                 GetNtfsVolumeFolders() function succeeded.
0348
        /// VOLUME_NOT_NTFS                     volume is not an NTFS volume.
0349
        /// INVALID_HANDLE_VALUE                NtfsUsnJournal object failed initialization.
0350
        /// USN_JOURNAL_NOT_ACTIVE              usn journal is not active on volume.
0351
        /// ERROR_ACCESS_DENIED                 accessing the usn journal requires admin rights, see remarks.
0352
        /// ERROR_INVALID_FUNCTION              error generated by DeviceIoControl() call.
0353
        /// ERROR_FILE_NOT_FOUND                error generated by DeviceIoControl() call.
0354
        /// ERROR_PATH_NOT_FOUND                error generated by DeviceIoControl() call.
0355
        /// ERROR_TOO_MANY_OPEN_FILES           error generated by DeviceIoControl() call.
0356
        /// ERROR_INVALID_HANDLE                error generated by DeviceIoControl() call.
0357
        /// ERROR_INVALID_DATA                  error generated by DeviceIoControl() call.
0358
        /// ERROR_NOT_SUPPORTED                 error generated by DeviceIoControl() call.
0359
        /// ERROR_INVALID_PARAMETER             error generated by DeviceIoControl() call.
0360
        /// ERROR_JOURNAL_DELETE_IN_PROGRESS    usn journal delete is in progress.
0361
        /// ERROR_INVALID_USER_BUFFER           error generated by DeviceIoControl() call.
0362
        /// USN_JOURNAL_ERROR                   unspecified usn journal error.
0363
        /// </returns>
0364
        /// <remarks>
0365
        /// If function returns ERROR_ACCESS_DENIED you need to run application as an Administrator.
0366
        /// </remarks>
0367
        public UsnJournalReturnCode
0368
            GetNtfsVolumeFolders(out List<Win32Api.UsnEntry> folders)
0369
        {
0370
            DateTime startTime = DateTime.Now;
0371
            folders = new List<Win32Api.UsnEntry>();
0372
            UsnJournalReturnCode usnRtnCode = UsnJournalReturnCode.VOLUME_NOT_NTFS;
0373
 
0374
            if (bNtfsVolume)
0375
            {
0376
                if (_usnJournalRootHandle.ToInt32() != Win32Api.INVALID_HANDLE_VALUE)
0377
                {
0378
                    usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
0379
 
0380
                    Win32Api.USN_JOURNAL_DATA usnState = new Win32Api.USN_JOURNAL_DATA();
0381
                    usnRtnCode = QueryUsnJournal(ref usnState);
0382
 
0383
                    if (usnRtnCode == UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
0384
                    {
0385
                        //
0386
                        // set up MFT_ENUM_DATA structure
0387
                        //
0388
                        Win32Api.MFT_ENUM_DATA med;
0389
                        med.StartFileReferenceNumber = 0;
0390
                        med.LowUsn = 0;
0391
                        med.HighUsn = usnState.NextUsn;
0392
                        Int32 sizeMftEnumData = Marshal.SizeOf(med);
0393
                        IntPtr medBuffer = Marshal.AllocHGlobal(sizeMftEnumData);
0394
                        Win32Api.ZeroMemory(medBuffer, sizeMftEnumData);
0395
                        Marshal.StructureToPtr(med, medBuffer, true);
0396
 
0397
                        //
0398
                        // set up the data buffer which receives the USN_RECORD data
0399
                        //
0400
                        int pDataSize = sizeof(UInt64) + 10000;
0401
                        IntPtr pData = Marshal.AllocHGlobal(pDataSize);
0402
                        Win32Api.ZeroMemory(pData, pDataSize);
0403
                        uint outBytesReturned = 0;
0404
                        Win32Api.UsnEntry usnEntry = null;
0405
 
0406
                        //
0407
                        // Gather up volume's directories
0408
                        //
0409
                        while (false != Win32Api.DeviceIoControl(
0410
                            _usnJournalRootHandle,
0411
                            Win32Api.FSCTL_ENUM_USN_DATA,
0412
                            medBuffer,
0413
                            sizeMftEnumData,
0414
                            pData,
0415
                            pDataSize,
0416
                            out outBytesReturned,
0417
                            IntPtr.Zero))
0418
                        {
0419
                            IntPtr pUsnRecord = new IntPtr(pData.ToInt32() + sizeof(Int64));
0420
                            while (outBytesReturned > 60)
0421
                            {
0422
                                usnEntry = new Win32Api.UsnEntry(pUsnRecord);
0423
                                //
0424
                                // check for directory entries
0425
                                //
0426
                                if (usnEntry.IsFolder)
0427
                                {
0428
                                    folders.Add(usnEntry);
0429
                                }
0430
                                pUsnRecord = new IntPtr(pUsnRecord.ToInt32() + usnEntry.RecordLength);
0431
                                outBytesReturned -= usnEntry.RecordLength;
0432
                            }
0433
                            Marshal.WriteInt64(medBuffer, Marshal.ReadInt64(pData, 0));
0434
                        }
0435
 
0436
                        Marshal.FreeHGlobal(pData);
0437
                        usnRtnCode = ConvertWin32ErrorToUsnError((Win32Api.GetLastErrorEnum)Marshal.GetLastWin32Error());
0438
                        if (usnRtnCode == UsnJournalReturnCode.ERROR_HANDLE_EOF)
0439
                        {
0440
                            usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
0441
                        }
0442
                    }
0443
                }
0444
                else
0445
                {
0446
                    usnRtnCode = UsnJournalReturnCode.INVALID_HANDLE_VALUE;
0447
                }
0448
            }
0449
 
0450
            _elapsedTime = DateTime.Now - startTime;
0451
            return usnRtnCode;
0452
        }
0453
 
0454
        /// <summary>
0455
        /// Given a file reference number GetPathFromFrn() calculates the full path in the out parameter 'path'.
0456
        /// </summary>
0457
        /// <param name="frn">A 64-bit file reference number</param>
0458
        /// <returns>
0459
        /// USN_JOURNAL_SUCCESS                 GetPathFromFrn() function succeeded.
0460
        /// VOLUME_NOT_NTFS                     volume is not an NTFS volume.
0461
        /// INVALID_HANDLE_VALUE                NtfsUsnJournal object failed initialization.
0462
        /// ERROR_ACCESS_DENIED                 accessing the usn journal requires admin rights, see remarks.
0463
        /// INVALID_FILE_REFERENCE_NUMBER       file reference number not found in Master File Table.
0464
        /// ERROR_INVALID_FUNCTION              error generated by NtCreateFile() or NtQueryInformationFile() call.
0465
        /// ERROR_FILE_NOT_FOUND                error generated by NtCreateFile() or NtQueryInformationFile() call.
0466
        /// ERROR_PATH_NOT_FOUND                error generated by NtCreateFile() or NtQueryInformationFile() call.
0467
        /// ERROR_TOO_MANY_OPEN_FILES           error generated by NtCreateFile() or NtQueryInformationFile() call.
0468
        /// ERROR_INVALID_HANDLE                error generated by NtCreateFile() or NtQueryInformationFile() call.
0469
        /// ERROR_INVALID_DATA                  error generated by NtCreateFile() or NtQueryInformationFile() call.
0470
        /// ERROR_NOT_SUPPORTED                 error generated by NtCreateFile() or NtQueryInformationFile() call.
0471
        /// ERROR_INVALID_PARAMETER             error generated by NtCreateFile() or NtQueryInformationFile() call.
0472
        /// ERROR_INVALID_USER_BUFFER           error generated by NtCreateFile() or NtQueryInformationFile() call.
0473
        /// USN_JOURNAL_ERROR                   unspecified usn journal error.
0474
        /// </returns>
0475
        /// <remarks>
0476
        /// If function returns ERROR_ACCESS_DENIED you need to run application as an Administrator.
0477
        /// </remarks>
0478
 
0479
        public UsnJournalReturnCode
0480
            GetPathFromFileReference(UInt64 frn, out string path)
0481
        {
0482
            DateTime startTime = DateTime.Now;
0483
            path = "Unavailable"
0484
                ;
0485
            UsnJournalReturnCode usnRtnCode = UsnJournalReturnCode.VOLUME_NOT_NTFS;
0486
 
0487
            if (bNtfsVolume)
0488
            {
0489
                if (_usnJournalRootHandle.ToInt32() != Win32Api.INVALID_HANDLE_VALUE)
0490
                {
0491
                    if (frn != 0)
0492
                    {
0493
                        usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
0494
 
0495
                        long allocSize = 0;
0496
                        Win32Api.UNICODE_STRING unicodeString;
0497
                        Win32Api.OBJECT_ATTRIBUTES objAttributes = new Win32Api.OBJECT_ATTRIBUTES();
0498
                        Win32Api.IO_STATUS_BLOCK ioStatusBlock = new Win32Api.IO_STATUS_BLOCK();
0499
                        IntPtr hFile = IntPtr.Zero;
0500
 
0501
                        IntPtr buffer = Marshal.AllocHGlobal(4096);
0502
                        IntPtr refPtr = Marshal.AllocHGlobal(8);
0503
                        IntPtr objAttIntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(objAttributes));
0504
 
0505
                        //
0506
                        // pointer >> fileid
0507
                        //
0508
                        Marshal.WriteInt64(refPtr, (long)frn);
0509
 
0510
                        unicodeString.Length = 8;
0511
                        unicodeString.MaximumLength = 8;
0512
                        unicodeString.Buffer = refPtr;
0513
                        //
0514
                        // copy unicode structure to pointer
0515
                        //
0516
                        Marshal.StructureToPtr(unicodeString, objAttIntPtr, true);
0517
 
0518
                        //
0519
                        //  InitializeObjectAttributes
0520
                        //
0521
                        objAttributes.Length = Marshal.SizeOf(objAttributes);
0522
                        objAttributes.ObjectName = objAttIntPtr;
0523
                        objAttributes.RootDirectory = _usnJournalRootHandle;
0524
                        objAttributes.Attributes = (int)Win32Api.OBJ_CASE_INSENSITIVE;
0525
 
0526
                        int fOk = Win32Api.NtCreateFile(
0527
                            ref hFile,
0528
                            FileAccess.Read,
0529
                            ref objAttributes,
0530
                            ref ioStatusBlock,
0531
                            ref allocSize,
0532
                            0,
0533
                            FileShare.ReadWrite,
0534
                            Win32Api.FILE_OPEN,
0535
                            Win32Api.FILE_OPEN_BY_FILE_ID | Win32Api.FILE_OPEN_FOR_BACKUP_INTENT,
0536
                            IntPtr.Zero, 0);
0537
                        if (fOk == 0)
0538
                        {
0539
                            fOk = Win32Api.NtQueryInformationFile(
0540
                                hFile,
0541
                                ref ioStatusBlock,
0542
                                buffer,
0543
                                4096,
0544
                                Win32Api.FILE_INFORMATION_CLASS.FileNameInformation);
0545
                            if (fOk == 0)
0546
                            {
0547
                                //
0548
                                // first 4 bytes are the name length
0549
                                //
0550
                                int nameLength = Marshal.ReadInt32(buffer, 0);
0551
                                //
0552
                                // next bytes are the name
0553
                                //
0554
                                path = Marshal.PtrToStringUni(new IntPtr(buffer.ToInt32() + 4), nameLength / 2);
0555
                            }
0556
                        }
0557
                        Win32Api.CloseHandle(hFile);
0558
                        Marshal.FreeHGlobal(buffer);
0559
                        Marshal.FreeHGlobal(objAttIntPtr);
0560
                        Marshal.FreeHGlobal(refPtr);
0561
                    }
0562
                }
0563
            }
0564
            _elapsedTime = DateTime.Now - startTime;
0565
            return usnRtnCode;
0566
        }
0567
 
0568
        /// <summary>
0569
        /// GetUsnJournalState() gets the current state of the USN Journal if it is active.
0570
        /// </summary>
0571
        /// <param name="usnJournalState">
0572
        /// Reference to usn journal data object filled with the current USN Journal state.
0573
        /// </param>
0574
        /// <param name="elapsedTime">The elapsed time for the GetUsnJournalState() function call.</param>
0575
        /// <returns>
0576
        /// USN_JOURNAL_SUCCESS                 GetUsnJournalState() function succeeded.
0577
        /// VOLUME_NOT_NTFS                     volume is not an NTFS volume.
0578
        /// INVALID_HANDLE_VALUE                NtfsUsnJournal object failed initialization.
0579
        /// USN_JOURNAL_NOT_ACTIVE              usn journal is not active on volume.
0580
        /// ERROR_ACCESS_DENIED                 accessing the usn journal requires admin rights, see remarks.
0581
        /// ERROR_INVALID_FUNCTION              error generated by DeviceIoControl() call.
0582
        /// ERROR_FILE_NOT_FOUND                error generated by DeviceIoControl() call.
0583
        /// ERROR_PATH_NOT_FOUND                error generated by DeviceIoControl() call.
0584
        /// ERROR_TOO_MANY_OPEN_FILES           error generated by DeviceIoControl() call.
0585
        /// ERROR_INVALID_HANDLE                error generated by DeviceIoControl() call.
0586
        /// ERROR_INVALID_DATA                  error generated by DeviceIoControl() call.
0587
        /// ERROR_NOT_SUPPORTED                 error generated by DeviceIoControl() call.
0588
        /// ERROR_INVALID_PARAMETER             error generated by DeviceIoControl() call.
0589
        /// ERROR_JOURNAL_DELETE_IN_PROGRESS    usn journal delete is in progress.
0590
        /// ERROR_INVALID_USER_BUFFER           error generated by DeviceIoControl() call.
0591
        /// USN_JOURNAL_ERROR                   unspecified usn journal error.
0592
        /// </returns>
0593
        /// <remarks>
0594
        /// If function returns ERROR_ACCESS_DENIED you need to run application as an Administrator.
0595
        /// </remarks>
0596
        public UsnJournalReturnCode
0597
            GetUsnJournalState(ref Win32Api.USN_JOURNAL_DATA usnJournalState)
0598
        {
0599
            UsnJournalReturnCode usnRtnCode = UsnJournalReturnCode.VOLUME_NOT_NTFS;
0600
            DateTime startTime = DateTime.Now;
0601
 
0602
            if (bNtfsVolume)
0603
            {
0604
                if (_usnJournalRootHandle.ToInt32() != Win32Api.INVALID_HANDLE_VALUE)
0605
                {
0606
                    usnRtnCode = QueryUsnJournal(ref usnJournalState);
0607
                }
0608
                else
0609
                {
0610
                    usnRtnCode = UsnJournalReturnCode.INVALID_HANDLE_VALUE;
0611
                }
0612
            }
0613
 
0614
            _elapsedTime = DateTime.Now - startTime;
0615
            return usnRtnCode;
0616
        }
0617
 
0618
        /// <summary>
0619
        /// Given a previous state, GetUsnJournalEntries() determines if the USN Journal is active and
0620
        /// no USN Journal entries have been lost (i.e. USN Journal is valid), then
0621
        /// it loads a SortedList<UInt64, Win32Api.UsnEntry> list and returns it as the out parameter 'usnEntries'.
0622
        /// If GetUsnJournalChanges returns anything but USN_JOURNAL_SUCCESS, the usnEntries list will
0623
        /// be empty.
0624
        /// </summary>
0625
        /// <param name="previousUsnState">The USN Journal state the last time volume
0626
        /// changes were requested.</param>
0627
        /// <param name="newFiles">List of the filenames of all new files.</param>
0628
        /// <param name="changedFiles">List of the filenames of all changed files.</param>
0629
        /// <param name="newFolders">List of the names of all new folders.</param>
0630
        /// <param name="changedFolders">List of the names of all changed folders.</param>
0631
        /// <param name="deletedFiles">List of the names of all deleted files</param>
0632
        /// <param name="deletedFolders">List of the names of all deleted folders</param>
0633
        /// <param name="currentState">Current state of the USN Journal</param>
0634
        /// <returns>
0635
        /// USN_JOURNAL_SUCCESS                 GetUsnJournalChanges() function succeeded.
0636
        /// VOLUME_NOT_NTFS                     volume is not an NTFS volume.
0637
        /// INVALID_HANDLE_VALUE                NtfsUsnJournal object failed initialization.
0638
        /// USN_JOURNAL_NOT_ACTIVE              usn journal is not active on volume.
0639
        /// ERROR_ACCESS_DENIED                 accessing the usn journal requires admin rights, see remarks.
0640
        /// ERROR_INVALID_FUNCTION              error generated by DeviceIoControl() call.
0641
        /// ERROR_FILE_NOT_FOUND                error generated by DeviceIoControl() call.
0642
        /// ERROR_PATH_NOT_FOUND                error generated by DeviceIoControl() call.
0643
        /// ERROR_TOO_MANY_OPEN_FILES           error generated by DeviceIoControl() call.
0644
        /// ERROR_INVALID_HANDLE                error generated by DeviceIoControl() call.
0645
        /// ERROR_INVALID_DATA                  error generated by DeviceIoControl() call.
0646
        /// ERROR_NOT_SUPPORTED                 error generated by DeviceIoControl() call.
0647
        /// ERROR_INVALID_PARAMETER             error generated by DeviceIoControl() call.
0648
        /// ERROR_JOURNAL_DELETE_IN_PROGRESS    usn journal delete is in progress.
0649
        /// ERROR_INVALID_USER_BUFFER           error generated by DeviceIoControl() call.
0650
        /// USN_JOURNAL_ERROR                   unspecified usn journal error.
0651
        /// </returns>
0652
        /// <remarks>
0653
        /// If function returns ERROR_ACCESS_DENIED you need to run application as an Administrator.
0654
        /// </remarks>
0655
        public UsnJournalReturnCode
0656
            GetUsnJournalEntries(Win32Api.USN_JOURNAL_DATA previousUsnState,
0657
            UInt32 reasonMask,
0658
            out List<Win32Api.UsnEntry> usnEntries,
0659
            out Win32Api.USN_JOURNAL_DATA newUsnState)
0660
        {
0661
            DateTime startTime = DateTime.Now;
0662
            usnEntries = new List<Win32Api.UsnEntry>();
0663
            newUsnState = new Win32Api.USN_JOURNAL_DATA();
0664
            UsnJournalReturnCode usnRtnCode = UsnJournalReturnCode.VOLUME_NOT_NTFS;
0665
 
0666
            if (bNtfsVolume)
0667
            {
0668
                if (_usnJournalRootHandle.ToInt32() != Win32Api.INVALID_HANDLE_VALUE)
0669
                {
0670
                    //
0671
                    // get current usn journal state
0672
                    //
0673
                    usnRtnCode = QueryUsnJournal(ref newUsnState);
0674
                    if (usnRtnCode == UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
0675
                    {
0676
                        bool bReadMore = true;
0677
                        //
0678
                        // sequentially process the usn journal looking for image file entries
0679
                        //
0680
                        int pbDataSize = sizeof(UInt64) * 0x4000;
0681
                        IntPtr pbData = Marshal.AllocHGlobal(pbDataSize);
0682
                        Win32Api.ZeroMemory(pbData, pbDataSize);
0683
                        uint outBytesReturned = 0;
0684
 
0685
                        Win32Api.READ_USN_JOURNAL_DATA rujd = new Win32Api.READ_USN_JOURNAL_DATA();
0686
                        rujd.StartUsn = previousUsnState.NextUsn;
0687
                        rujd.ReasonMask = reasonMask;
0688
                        rujd.ReturnOnlyOnClose = 0;
0689
                        rujd.Timeout = 0;
0690
                        rujd.bytesToWaitFor = 0;
0691
                        rujd.UsnJournalId = previousUsnState.UsnJournalID;
0692
                        int sizeRujd = Marshal.SizeOf(rujd);
0693
 
0694
                        IntPtr rujdBuffer = Marshal.AllocHGlobal(sizeRujd);
0695
                        Win32Api.ZeroMemory(rujdBuffer, sizeRujd);
0696
                        Marshal.StructureToPtr(rujd, rujdBuffer, true);
0697
 
0698
                        Win32Api.UsnEntry usnEntry = null;
0699
 
0700
                        //
0701
                        // read usn journal entries
0702
                        //
0703
                        while (bReadMore)
0704
                        {
0705
                            bool bRtn = Win32Api.DeviceIoControl(
0706
                                _usnJournalRootHandle,
0707
                                Win32Api.FSCTL_READ_USN_JOURNAL,
0708
                                rujdBuffer,
0709
                                sizeRujd,
0710
                                pbData,
0711
                                pbDataSize,
0712
                                out outBytesReturned,
0713
                                IntPtr.Zero);
0714
                            if (bRtn)
0715
                            {
0716
                                IntPtr pUsnRecord = new IntPtr(pbData.ToInt32() + sizeof(UInt64));
0717
                                while (outBytesReturned > 60)   // while there are at least one entry in the usn journal
0718
                                {
0719
                                    usnEntry = new Win32Api.UsnEntry(pUsnRecord);
0720
                                    if (usnEntry.USN >= newUsnState.NextUsn)      // only read until the current usn points beyond the current state's usn
0721
                                    {
0722
                                        bReadMore = false;
0723
                                        break;
0724
                                    }
0725
                                    usnEntries.Add(usnEntry);
0726
 
0727
                                    pUsnRecord = new IntPtr(pUsnRecord.ToInt32() + usnEntry.RecordLength);
0728
                                    outBytesReturned -= usnEntry.RecordLength;
0729
 
0730
                                }   // end while (outBytesReturned > 60) - closing bracket
0731
 
0732
                            }   // if (bRtn)- closing bracket
0733
                            else
0734
                            {
0735
                                Win32Api.GetLastErrorEnum lastWin32Error = (Win32Api.GetLastErrorEnum)Marshal.GetLastWin32Error();
0736
                                if (lastWin32Error == Win32Api.GetLastErrorEnum.ERROR_HANDLE_EOF)
0737
                                {
0738
                                    usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
0739
                                }
0740
                                else
0741
                                {
0742
                                    usnRtnCode = ConvertWin32ErrorToUsnError(lastWin32Error);
0743
                                }
0744
                                break;
0745
                            }
0746
 
0747
                            Int64 nextUsn = Marshal.ReadInt64(pbData, 0);
0748
                            if (nextUsn >= newUsnState.NextUsn)
0749
                            {
0750
                                break;
0751
                            }
0752
                            Marshal.WriteInt64(rujdBuffer, nextUsn);
0753
 
0754
                        }   // end while (bReadMore) - closing bracket
0755
 
0756
                        Marshal.FreeHGlobal(rujdBuffer);
0757
                        Marshal.FreeHGlobal(pbData);
0758
 
0759
                    }   // if (usnRtnCode == UsnJournalReturnCode.USN_JOURNAL_SUCCESS) - closing bracket
0760
 
0761
                }   // if (_usnJournalRootHandle.ToInt32() != Win32Api.INVALID_HANDLE_VALUE)
0762
                else
0763
                {
0764
                    usnRtnCode = UsnJournalReturnCode.INVALID_HANDLE_VALUE;
0765
                }
0766
            }   // if (bNtfsVolume) - closing bracket
0767
 
0768
            _elapsedTime = DateTime.Now - startTime;
0769
            return usnRtnCode;
0770
        }   // GetUsnJournalChanges() - closing bracket
0771
 
0772
        /// <summary>
0773
        /// tests to see if the USN Journal is active on the volume.
0774
        /// </summary>
0775
        /// <returns>true if USN Journal is active
0776
        /// false if no USN Journal on volume</returns>
0777
        public bool
0778
            IsUsnJournalActive()
0779
        {
0780
            DateTime start = DateTime.Now;
0781
            bool bRtnCode = false;
0782
 
0783
            if (bNtfsVolume)
0784
            {
0785
                if (_usnJournalRootHandle.ToInt32() != Win32Api.INVALID_HANDLE_VALUE)
0786
                {
0787
                    Win32Api.USN_JOURNAL_DATA usnJournalCurrentState = new Win32Api.USN_JOURNAL_DATA();
0788
                    UsnJournalReturnCode usnError = QueryUsnJournal(ref usnJournalCurrentState);
0789
                    if (usnError == UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
0790
                    {
0791
                        bRtnCode = true;
0792
                    }
0793
                }
0794
            }
0795
            _elapsedTime = DateTime.Now - start;
0796
            return bRtnCode;
0797
        }
0798
 
0799
        /// <summary>
0800
        /// tests to see if there is a USN Journal on this volume and if there is
0801
        /// determines whether any journal entries have been lost.
0802
        /// </summary>
0803
        /// <returns>true if the USN Journal is active and if the JournalId's are the same
0804
        /// and if all the usn journal entries expected by the previous state are available
0805
        /// from the current state.
0806
        /// false if not</returns>
0807
        public bool
0808
            IsUsnJournalValid(Win32Api.USN_JOURNAL_DATA usnJournalPreviousState)
0809
        {
0810
            DateTime start = DateTime.Now;
0811
            bool bRtnCode = false;
0812
 
0813
            if (bNtfsVolume)
0814
            {
0815
                if (_usnJournalRootHandle.ToInt32() != Win32Api.INVALID_HANDLE_VALUE)
0816
                {
0817
                    Win32Api.USN_JOURNAL_DATA usnJournalState = new Win32Api.USN_JOURNAL_DATA();
0818
                    UsnJournalReturnCode usnError = QueryUsnJournal(ref usnJournalState);
0819
 
0820
                    if (usnError == UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
0821
                    {
0822
                        if (usnJournalPreviousState.UsnJournalID == usnJournalState.UsnJournalID)
0823
                        {
0824
                            if (usnJournalPreviousState.NextUsn >= usnJournalState.NextUsn)
0825
                            {
0826
                                bRtnCode = true;
0827
                            }
0828
                        }
0829
                    }
0830
                }
0831
            }
0832
            _elapsedTime = DateTime.Now - start;
0833
            return bRtnCode;
0834
        }
0835
 
0836
        #endregion
0837
 
0838
        #region private member functions
0839
        /// <summary>
0840
        /// Converts a Win32 Error to a UsnJournalReturnCode
0841
        /// </summary>
0842
        /// <param name="Win32LastError">The 'last' Win32 error.</param>
0843
        /// <returns>
0844
        /// INVALID_HANDLE_VALUE                error generated by Win32 Api calls.
0845
        /// USN_JOURNAL_SUCCESS                 usn journal function succeeded.
0846
        /// ERROR_INVALID_FUNCTION              error generated by Win32 Api calls.
0847
        /// ERROR_FILE_NOT_FOUND                error generated by Win32 Api calls.
0848
        /// ERROR_PATH_NOT_FOUND                error generated by Win32 Api calls.
0849
        /// ERROR_TOO_MANY_OPEN_FILES           error generated by Win32 Api calls.
0850
        /// ERROR_ACCESS_DENIED                 accessing the usn journal requires admin rights.
0851
        /// ERROR_INVALID_HANDLE                error generated by Win32 Api calls.
0852
        /// ERROR_INVALID_DATA                  error generated by Win32 Api calls.
0853
        /// ERROR_HANDLE_EOF                    error generated by Win32 Api calls.
0854
        /// ERROR_NOT_SUPPORTED                 error generated by Win32 Api calls.
0855
        /// ERROR_INVALID_PARAMETER             error generated by Win32 Api calls.
0856
        /// ERROR_JOURNAL_DELETE_IN_PROGRESS    usn journal delete is in progress.
0857
        /// ERROR_JOURNAL_ENTRY_DELETED         usn journal entry lost, no longer available.
0858
        /// ERROR_INVALID_USER_BUFFER           error generated by Win32 Api calls.
0859
        /// USN_JOURNAL_INVALID                 usn journal is invalid, id's don't match or required entries lost.
0860
        /// USN_JOURNAL_NOT_ACTIVE              usn journal is not active on volume.
0861
        /// VOLUME_NOT_NTFS                     volume is not an NTFS volume.
0862
        /// INVALID_FILE_REFERENCE_NUMBER       bad file reference number - see remarks.
0863
        /// USN_JOURNAL_ERROR                   unspecified usn journal error.
0864
        /// </returns>
0865
        private UsnJournalReturnCode
0866
            ConvertWin32ErrorToUsnError(Win32Api.GetLastErrorEnum Win32LastError)
0867
        {
0868
           UsnJournalReturnCode usnRtnCode;
0869
 
0870
           switch (Win32LastError)
0871
            {
0872
                case Win32Api.GetLastErrorEnum.ERROR_JOURNAL_NOT_ACTIVE:
0873
                    usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_NOT_ACTIVE;
0874
                    break;
0875
                case Win32Api.GetLastErrorEnum.ERROR_SUCCESS:
0876
                    usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
0877
                    break;
0878
               case Win32Api.GetLastErrorEnum.ERROR_HANDLE_EOF:
0879
                    usnRtnCode = UsnJournalReturnCode.ERROR_HANDLE_EOF;
0880
                    break;
0881
                default:
0882
                    usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_ERROR;
0883
                    break;
0884
            }
0885
 
0886
           return usnRtnCode;
0887
        }
0888
 
0889
        /// <summary>
0890
        /// Gets a Volume Serial Number for the volume represented by driveInfo.
0891
        /// </summary>
0892
        /// <param name="driveInfo">DriveInfo object representing the volume in question.</param>
0893
        /// <param name="volumeSerialNumber">out parameter to hold the volume serial number.</param>
0894
        /// <returns></returns>
0895
        private UsnJournalReturnCode
0896
            GetVolumeSerialNumber(DriveInfo driveInfo, out uint volumeSerialNumber)
0897
        {
0898
            Console.WriteLine("GetVolumeSerialNumber() function entered for drive '{0}'", driveInfo.Name);
0899
 
0900
            volumeSerialNumber = 0;
0901
            UsnJournalReturnCode usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
0902
            string pathRoot = string.Concat("\\\\.\\", driveInfo.Name);
0903
 
0904
            IntPtr hRoot = Win32Api.CreateFile(pathRoot,
0905
                0,
0906
                Win32Api.FILE_SHARE_READ | Win32Api.FILE_SHARE_WRITE,
0907
                IntPtr.Zero,
0908
                Win32Api.OPEN_EXISTING,
0909
                Win32Api.FILE_FLAG_BACKUP_SEMANTICS,
0910
                IntPtr.Zero);
0911
 
0912
            if (hRoot.ToInt32() != Win32Api.INVALID_HANDLE_VALUE)
0913
            {
0914
                Win32Api.BY_HANDLE_FILE_INFORMATION fi = new Win32Api.BY_HANDLE_FILE_INFORMATION();
0915
                bool bRtn = Win32Api.GetFileInformationByHandle(hRoot, out fi);
0916
 
0917
                if (bRtn)
0918
                {
0919
                    UInt64 fileIndexHigh = (UInt64)fi.FileIndexHigh;
0920
                    UInt64 indexRoot = (fileIndexHigh << 32) | fi.FileIndexLow;
0921
                    volumeSerialNumber = fi.VolumeSerialNumber;
0922
                }
0923
                else
0924
                {
0925
                    usnRtnCode = (UsnJournalReturnCode)Marshal.GetLastWin32Error();
0926
                }
0927
 
0928
                Win32Api.CloseHandle(hRoot);
0929
            }
0930
            else
0931
            {
0932
                usnRtnCode = (UsnJournalReturnCode)Marshal.GetLastWin32Error();
0933
            }
0934
 
0935
            return usnRtnCode;
0936
        }
0937
 
0938
        private UsnJournalReturnCode
0939
            GetRootHandle(out IntPtr rootHandle)
0940
        {
0941
            //
0942
            // private functions don't need to check for an NTFS volume or
0943
            // a valid _usnJournalRootHandle handle
0944
            //
0945
            UsnJournalReturnCode usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
0946
            rootHandle = IntPtr.Zero;
0947
            string vol = string.Concat("\\\\.\\", _driveInfo.Name.TrimEnd('\\'));
0948
 
0949
            rootHandle = Win32Api.CreateFile(vol,
0950
                 Win32Api.GENERIC_READ | Win32Api.GENERIC_WRITE,
0951
                 Win32Api.FILE_SHARE_READ | Win32Api.FILE_SHARE_WRITE,
0952
                 IntPtr.Zero,
0953
                 Win32Api.OPEN_EXISTING,
0954
                 0,
0955
                 IntPtr.Zero);
0956
 
0957
            if (rootHandle.ToInt32() == Win32Api.INVALID_HANDLE_VALUE)
0958
            {
0959
                usnRtnCode = (UsnJournalReturnCode)Marshal.GetLastWin32Error();
0960
            }
0961
 
0962
            return usnRtnCode;
0963
        }
0964
 
0965
        /// <summary>
0966
        /// This function queries the usn journal on the volume.
0967
        /// </summary>
0968
        /// <param name="usnJournalState">the USN_JOURNAL_DATA object that is associated with this volume</param>
0969
        /// <returns></returns>
0970
        private UsnJournalReturnCode
0971
            QueryUsnJournal(ref Win32Api.USN_JOURNAL_DATA usnJournalState)
0972
        {
0973
            //
0974
            // private functions don't need to check for an NTFS volume or
0975
            // a valid _usnJournalRootHandle handle
0976
            //
0977
            UsnJournalReturnCode usnReturnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
0978
            int sizeUsnJournalState = Marshal.SizeOf(usnJournalState);
0979
            UInt32 cb;
0980
 
0981
            bool fOk = Win32Api.DeviceIoControl(
0982
                _usnJournalRootHandle,
0983
                Win32Api.FSCTL_QUERY_USN_JOURNAL,
0984
                IntPtr.Zero,
0985
                0,
0986
                out usnJournalState,
0987
                sizeUsnJournalState,
0988
                out cb,
0989
                IntPtr.Zero);
0990
 
0991
            if (!fOk)
0992
            {
0993
                int lastWin32Error = Marshal.GetLastWin32Error();
0994
                usnReturnCode = ConvertWin32ErrorToUsnError((Win32Api.GetLastErrorEnum)Marshal.GetLastWin32Error());
0995
            }
0996
 
0997
            return usnReturnCode;
0998
        }
0999
 
1000
        #endregion
1001
 
1002
        #region IDisposable Members
1003
 
1004
        public void Dispose()
1005
        {
1006
            Win32Api.CloseHandle(_usnJournalRootHandle);
1007
        }
1008
 
1009
        #endregion
1010
    }
1011
}
    */
}