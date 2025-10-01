using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using FlashFloppyUI.AdfSharp.Interop;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FlashFloppyUI
{
	namespace AdfSharp.Interop
	{
		// Enum mapping for ADF_ENV_PROPERTY
		public enum AdfEnvProperty
		{
			PR_VFCT = 1,
			PR_WFCT = 2,
			PR_EFCT = 3,
			PR_NOTFCT = 4,
			PR_USEDIRC = 5,
			PR_USE_NOTFCT = 6,
			PR_PROGBAR = 7,
			PR_USE_PROGBAR = 8,
			PR_RWACCESS = 9,
			PR_USE_RWACCESS = 10,
			PR_IGNORE_CHECKSUM_ERRORS = 11,
			PR_QUIET = 12
		}

		public enum AdfFileMode {
			Read = 0x01,   /* 01 */
			Write = 0x02,   /* 10 */
			//ADF_FILE_MODE_READWRITE = 0x03    /* 11 */
		}

		// Maps AdfAccessMode enum from native code
		public enum AdfAccessMode : int
		{
			ReadWrite = 0,
			ReadOnly = 1
		}
		public enum AdfRetCode : int
		{
			OK = 0,
			ERROR = -1,
			ERROR_INTERNAL = -2,

			MALLOC = 1,
			VOLFULL = 2,

			FOPEN = 1 << 10,
			NULLPTR = 1 << 12,
			NAME_TOO_LONG = 1 << 13,

			// adfRead*Block()
			BLOCKTYPE = 1,
			BLOCKSTYPE = 1 << 1,
			BLOCKSUM = 1 << 2,
			HEADERKEY = 1 << 3,
			BLOCKREAD = 1 << 4,

			// adfWrite*Block
			BLOCKWRITE = 1 << 4,

			// adfVolReadBlock()
			BLOCKOUTOFRANGE = 1,
			BLOCKNATREAD = 1 << 1,

			// adfVolWriteBlock()
			BLOCKNATWRITE = 1 << 1,
			BLOCKREADONLY = 1 << 2,

			// adfInitDumpDevice()
			// FOPEN
			// MALLOC

			// adfNativeReadBlock(), adfReadDumpSector()
			BLOCKSHORTREAD = 1,
			BLOCKFSEEK = 1 << 1,

			// adfNativeWriteBlock(), adfWriteDumpSector()
			BLOCKSHORTWRITE = 1,
			// BLOCKFSEEK

			// adfReadRDSKblock
			BLOCKID = 1 << 5,

			// adfWriteRDSKblock()
			// BLOCKREADONLY
		}

		// Enum mapping for AdfDevRdbStatus
		public enum AdfDevRdbStatus
		{
			Unknown,
			Unreadable,
			NotFound,
			Exist,
			ChecksumError,
			Ok,
			SameGeometry
		}

		// Struct mapping for AdfDevGeometry (define fields as per adf_dev_type.h)
		[StructLayout(LayoutKind.Sequential)]
		public struct AdfDevGeometry
		{
			public uint cylinders;
			public uint heads;
			public uint sectors;
			// Add other fields if present in adf_dev_type.h
		}

		// Struct mapping for AdfDevice (simplified, pointers as IntPtr)
		[StructLayout(LayoutKind.Sequential)]
		public struct AdfDevice
		{
			public IntPtr name;
			public int type;
			public int @class;
			[MarshalAs(UnmanagedType.I1)]
			public bool readOnly;
			public uint sizeBlocks;

			public RigidDiskBlock rdb;
			public AdfDevGeometry geometry;
			public IntPtr drv;
			public IntPtr drvData;
			[MarshalAs(UnmanagedType.I1)]
			public bool mounted;
			public int nVol;
			public IntPtr volList;

			[StructLayout(LayoutKind.Sequential)]
			public struct RigidDiskBlock
			{
				public AdfDevRdbStatus status;
				public IntPtr block;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct AdfBitmap
		{
			public uint Size; // size in blocks

			public IntPtr Blocks; // Pointer to ADF_SECTNUM array
			public IntPtr Table; // Pointer to AdfBitmapBlock* array
			public IntPtr BlocksChg; // Pointer to bool array
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct AdfVolume
		{
			public IntPtr Dev; // Pointer to AdfDevice

			public uint FirstBlock; // First block of data area (from beginning of device)
			public uint LastBlock; // Last block of data area (from beginning of device)
			public uint RootBlock; // Root block (from FirstBlock)

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public char[] FsId; // Filesystem ID ("DOS", "PFS", etc.)
			public byte FsType; // Filesystem type (e.g., FFS/OFS, DIRCACHE, INTERNATIONAL)

			[MarshalAs(UnmanagedType.I1)]
			public bool BootCode; // Indicates if boot code is present
			[MarshalAs(UnmanagedType.I1)]
			public bool ReadOnly; // Indicates if the volume is read-only

			public uint DataBlockSize; // Data block size (e.g., 488 or 512)
			public uint BlockSize; // Block size (e.g., 512)

			public IntPtr VolName; // Pointer to volume name (char*)

			[MarshalAs(UnmanagedType.I1)]
			public bool Mounted; // Indicates if the volume is mounted

			public AdfBitmap Bitmap; // Bitmap structure

			public uint CurDirPtr; // Current directory pointer
		}

		public static class AdfInterop
		{
			private const string DllName = "adf.dll";
			public const int ADF_DOSFS_DIRCACHE = 4;

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern AdfRetCode adfToRootDir(IntPtr vol);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern AdfRetCode adfChangeDir(IntPtr vol, string name);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern AdfRetCode adfParentDir(IntPtr vol);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern AdfRetCode adfEntryRead(IntPtr vol, uint nSect, ref ADFEntry entry, IntPtr entryBlk);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr adfGetDirEnt(IntPtr vol, uint nSect);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr adfGetRDirEnt(IntPtr vol, uint nSect, bool recurs);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern void adfFreeDirList(IntPtr list);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern void adfFreeEntry(ref ADFEntry entry);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfGetEntry(IntPtr vol, uint dirPtr, string name, ref ADFEntry entry);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern uint adfGetEntryBlockNum(IntPtr vol, uint dirPtr, string name);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern uint adfGetEntryBlock(IntPtr vol, uint dirPtr, string name, IntPtr entry);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern AdfRetCode adfCreateFile(IntPtr vol, uint parent, string name, IntPtr fhdr);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern AdfRetCode adfCreateDir(IntPtr vol, uint parent, string name);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern uint adfCreateEntry(IntPtr vol, IntPtr dir, string name, uint thisSect);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfRemoveEntry(IntPtr vol, uint pSect, string name);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfRenameEntry(IntPtr vol, uint pSect, string oldName, uint nPSect, string newName);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfSetEntryAccess(IntPtr vol, uint parSect, string name, int newAcc);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfSetEntryComment(IntPtr vol, uint parSect, string name, string newCmt);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern bool adfIsDirEmpty(IntPtr dir);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfDirCountEntries(IntPtr vol, uint dirPtr);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfReadEntryBlock(IntPtr vol, uint nSect, IntPtr ent);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfWriteEntryBlock(IntPtr vol, uint nSect, IntPtr ent);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfWriteDirBlock(IntPtr vol, uint nSect, IntPtr dir);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfEntBlock2Entry(IntPtr entryBlk, ref ADFEntry entry);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern uint adfNameToEntryBlk(IntPtr vol, IntPtr ht, string name, IntPtr entry, ref uint nUpdSect);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr adfEntryGetInfo(ref ADFEntry entry);

            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern AdfRetCode adfLibInit();
            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern void adfLibCleanUp();

            [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern IntPtr adfDevCreate(
				string driverName,
				string name,
				uint cylinders,
				uint heads,
				uint sectors);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern IntPtr adfDevOpen(
				string name,
				int mode);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern IntPtr adfDevOpenWithDriver(
				string driverName,
				string name,
				int mode);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern void adfDevClose(IntPtr dev);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfDevType(IntPtr dev);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfDevMount(IntPtr dev);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern void adfDevUnMount(IntPtr dev);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfDevReadBlock(
				IntPtr dev,
				uint pSect,
				uint size,
				IntPtr buf);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfDevWriteBlock(
				IntPtr dev,
				uint pSect,
				uint size,
				IntPtr buf);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr adfDevGetInfo(IntPtr dev);

			// adfMountFlop: Mounts a floppy device
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfMountFlop(IntPtr dev);

			// adfCreateFlop: Creates a floppy volume on the device
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern AdfRetCode adfCreateFlop(
				IntPtr dev,
				string volName,
				byte volType
			);

			// Create a volume with specified location, size, name, and filesystem type
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern IntPtr adfVolCreate(
				IntPtr dev,
				uint start,
				uint len,
				string volName,
				byte volType
			);

			// Mount a volume by partition number and access mode
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr adfVolMount(
				IntPtr dev,
				int nPart,
				AdfAccessMode mode
			);

			// Remount an already mounted volume (to change the mode)
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfVolRemount(
				IntPtr vol,
				int mode
			);

			// Unmount a volume
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern void adfVolUnMount(
				IntPtr vol
			);

			// Write the provided bootblock to volume
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfVolInstallBootBlock(
				IntPtr vol,
				IntPtr code
			);

			// True if given block number is valid (within the volume)
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			[return: MarshalAs(UnmanagedType.I1)]
			public static extern bool adfVolIsSectNumValid(
				IntPtr vol,
				uint nSect
			);

			// Read volume's block
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfVolReadBlock(
				IntPtr vol,
				uint nSect,
				IntPtr buf
			);

			// Write volume's block
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfVolWriteBlock(
				IntPtr vol,
				uint nSect,
				IntPtr buf
			);

			// Get filesystem's id string ("OFS", "FFS", ...)
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr adfVolGetFsStr(
				IntPtr vol
			);

			// Get info string about the volume device (must be freed after use)
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr adfVolGetInfo(
				IntPtr vol
			);

			// Open a file on the volume
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public static extern IntPtr adfFileOpen(
				IntPtr vol,
				string name,
				AdfFileMode mode // AdfFileMode: 0x01=read, 0x02=write
			);

			// Close a file
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern void adfFileClose(
				IntPtr file
			);

			// Read from a file
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern uint adfFileRead(
				IntPtr file,
				uint n,
				ref byte[] buffer
			);

			// Write to a file
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern uint adfFileWrite(
				IntPtr file,
				uint n,
				ref byte[] buffer
			);

			// Seek to a position in the file
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfFileSeek(
				IntPtr file,
				uint pos
			);

			// Truncate the file to a new size
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfFileTruncate(
				IntPtr file,
				uint fileSizeNew
			);

			// Flush file buffers
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfFileFlush(
				IntPtr file
			);

			// Read an extension block by number
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfFileReadExtBlockN(
				IntPtr file,
				int extBlock,
				IntPtr fext // pointer to AdfFileExtBlock
			);

			// Get blocks to remove for truncation
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfFileTruncateGetBlocksToRemove(
				IntPtr file,
				uint fileSizeNew,
				IntPtr blocksToRemove // pointer to AdfVectorSectors
			);


			// Initialize environment to default
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern void adfEnvInitDefault();

			// Clean up environment
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern void adfEnvCleanUp();

			// Set callback functions (function pointers)
			// Note: Delegates must be kept alive by the caller to avoid GC
			[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
			public delegate void AdfLogFct([MarshalAs(UnmanagedType.LPStr)] string format, IntPtr args);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void AdfNotifyFct(uint sectNum, int value);

			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern void adfEnvSetFct(
				AdfLogFct eFct,
				AdfLogFct wFct,
				AdfLogFct vFct,
				AdfNotifyFct notifyFct
			);

			// Set environment property
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern int adfEnvSetProperty(
				AdfEnvProperty property,
				IntPtr newValue
			);

			// Get environment property
			[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
			public static extern IntPtr adfEnvGetProperty(
				AdfEnvProperty property
			);
		}
	}

	public class IntPtrContainer
	{
		internal IntPtrContainer(IntPtr ptr)
		{
			_ptr = ptr;
		}
		internal IntPtr getPtr() => _ptr;
		protected IntPtr _ptr;
	}
	public class ADFDevice : IntPtrContainer 
	{
		internal ADFDevice(IntPtr ptr) : base(ptr) { }
	}

	public class ADFVolume : IntPtrContainer
	{
		internal ADFVolume(IntPtr ptr) : base(ptr) { }

		private AdfVolume GetVolume() => Marshal.PtrToStructure<AdfVolume>(base._ptr);
		public uint FirstBlock => GetVolume().FirstBlock;
		public uint LastBlock => GetVolume().LastBlock;
		public uint RootBlock => GetVolume().RootBlock;
		public uint CurrentDirectory => GetVolume().CurDirPtr;
	}

	public class ADFFile : IntPtrContainer
	{
		internal ADFFile(IntPtr ptr) : base(ptr) { }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ADFEntry
	{
		public int Type;
		public IntPtr Name; // char*
		public uint Sector;
		public uint Real;
		public uint Parent;
		public IntPtr Comment; // char*
		public uint Size;
		public int Access;

		public int Year;
		public int Month;
		public int Days;

		public int Hour;
		public int Mins;
		public int Secs;
	}
}

namespace FlashFloppyUI
{
	public static class ADFSharp
	{
        // Boot Block Data (from ADFinder project)
        private static byte[] _kick13BootBlock = {
			0x44, 0x4F, 0x53, 0x00, 0xDF, 0x10, 0x1A, 0x2A, 0x00, 0x00, 0x03, 0x70, 0x43, 0xFA, 0x00, 0x18,
			0x4E, 0xAE, 0xFF, 0xA0, 0x4A, 0x80, 0x67, 0x0A, 0x20, 0x40, 0x20, 0x68, 0x00, 0x16, 0x70, 0x00,
			0x4E, 0x75, 0x70, 0xFF, 0x60, 0xFA, 0x64, 0x6F, 0x73, 0x2E, 0x6C, 0x69, 0x62, 0x72, 0x61, 0x72,
			0x79, 0x00
		};

        private static byte[] _kick20BootBlock = {
			0x44, 0x4F, 0x53, 0x01, 0x43, 0x1A, 0x4A, 0x2A, 0x00, 0x00, 0x03, 0x70, 0x43, 0xFA, 0x00, 0x18,
			0x4E, 0xAE, 0xFF, 0xA0, 0x4A, 0x80, 0x67, 0x0A, 0x20, 0x40, 0x20, 0x68, 0x00, 0x16, 0x70, 0x00,
			0x4E, 0x75, 0x70, 0xFF, 0x60, 0xFA, 0x64, 0x6F, 0x73, 0x2E, 0x6C, 0x69, 0x62, 0x72, 0x61, 0x72,
			0x79, 0x00
		};

		public enum BootBlockType
		{
			Kick13,
            Kick20,
			Kick30,
        }

        static ADFSharp()
		{
			AdfInterop.adfLibInit();
		}

        public static void InitializeEnvironment()
		{
			AdfInterop.adfEnvInitDefault();
		}

		public static void CleanUpEnvironment()
		{
			AdfInterop.adfEnvCleanUp();
		}

		public static ADFDevice CreateDevice(string adfFile)
		{
            return new ADFDevice(AdfInterop.adfDevCreate("dump", adfFile, 80, 2, 11));
		}

		public static void MountDevice(ADFDevice device)
		{
			AdfInterop.adfDevMount(device.getPtr());
		}

		public static bool CreateFloppy(ADFDevice device, string floppyName)
		{
			AdfRetCode rc = AdfInterop.adfCreateFlop(device.getPtr(), floppyName, AdfInterop.ADF_DOSFS_DIRCACHE);
			return rc == AdfRetCode.OK;
		}

		public static ADFVolume MountFloppy(ADFDevice floppy)
		{
			return new ADFVolume(AdfInterop.adfVolMount(floppy.getPtr(), 0, AdfAccessMode.ReadWrite));
		}

		public static void UnmountFloppy(ADFVolume volume)
		{
			AdfInterop.adfVolUnMount(volume.getPtr());
		}

		public static void UnmountDevice(ADFDevice floppy)
		{
			AdfInterop.adfDevUnMount(floppy.getPtr());
		}

		public static void InstallBootBlock(ADFVolume volume, BootBlockType type)
		{
			byte[] data = null;
			if (type == BootBlockType.Kick30)
			{
				using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("FlashFloppyUI.stdboot3.bbk"))
				using (var reader = new BinaryReader(stream))
				{
					data = reader.ReadBytes((int)stream.Length);
				}
			}
			else if (type == BootBlockType.Kick13)
				data = _kick13BootBlock;
			else
				data = _kick20BootBlock;
		
			GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			AdfInterop.adfVolInstallBootBlock(volume.getPtr(), handle.AddrOfPinnedObject());
			handle.Free();
        }

		public static void CloseDevice(ADFDevice floppy)
		{
			AdfInterop.adfDevClose(floppy.getPtr());
		}

		public static ADFFile OpenFile(ADFVolume volume, string filename, AdfFileMode mode)
		{
			return new ADFFile(AdfInterop.adfFileOpen(volume.getPtr(), filename, mode));
		}

		public static uint WriteFile(ADFFile file, byte[] buf)
		{
			return AdfInterop.adfFileWrite(file.getPtr(), (uint)buf.Length, ref buf);

		}
		public static void CloseFile(ADFFile file)
		{
			AdfInterop.adfFileClose(file.getPtr());
		}

		public static AdfRetCode ToRootDir(ADFVolume volume)
		{
			return AdfInterop.adfToRootDir(volume.getPtr());
		}

		public static AdfRetCode ChangeDir(ADFVolume volume, string name)
		{
			return AdfInterop.adfChangeDir(volume.getPtr(), name);
		}

		public static AdfRetCode ParentDir(ADFVolume volume)
		{
			return AdfInterop.adfParentDir(volume.getPtr());
		}

		public static AdfRetCode EntryRead(ADFVolume volume, uint sector, ref ADFEntry entry, IntPtr entryBlock)
		{
			return AdfInterop.adfEntryRead(volume.getPtr(), sector, ref entry, entryBlock);
		}

		public static IntPtr GetDirEnt(ADFVolume volume, uint sector)
		{
			return AdfInterop.adfGetDirEnt(volume.getPtr(), sector);
		}

		public static IntPtr GetRDirEnt(ADFVolume volume, uint sector, bool recursive)
		{
			return AdfInterop.adfGetRDirEnt(volume.getPtr(), sector, recursive);
		}

		public static void FreeDirList(IntPtr list)
		{
			AdfInterop.adfFreeDirList(list);
		}

		public static void FreeEntry(ref ADFEntry entry)
		{
			AdfInterop.adfFreeEntry(ref entry);
		}

		public static int GetEntry(ADFVolume volume, uint dirPtr, string name, ref ADFEntry entry)
		{
			return AdfInterop.adfGetEntry(volume.getPtr(), dirPtr, name, ref entry);
		}

		public static uint GetEntryBlockNum(ADFVolume volume, uint dirPtr, string name)
		{
			return AdfInterop.adfGetEntryBlockNum(volume.getPtr(), dirPtr, name);
		}

		public static uint GetEntryBlock(ADFVolume volume, uint dirPtr, string name, IntPtr entry)
		{
			return AdfInterop.adfGetEntryBlock(volume.getPtr(), dirPtr, name, entry);
		}

		public static AdfRetCode CreateFile(ADFVolume volume, uint parent, string name)
		{
			byte[] fileHeader = new byte[512];
			GCHandle handle = GCHandle.Alloc(fileHeader, GCHandleType.Pinned);
			AdfRetCode retVal = AdfInterop.adfCreateFile(volume.getPtr(), parent, name, handle.AddrOfPinnedObject());
			handle.Free();
			return retVal;
		}

		public static AdfRetCode CreateDir(ADFVolume volume, uint parent, string name)
		{
			return AdfInterop.adfCreateDir(volume.getPtr(), parent, name);
		}

		public static uint CreateEntry(ADFVolume volume, IntPtr dir, string name, uint thisSector)
		{
			return AdfInterop.adfCreateEntry(volume.getPtr(), dir, name, thisSector);
		}

		public static int RemoveEntry(ADFVolume volume, uint parentSector, string name)
		{
			return AdfInterop.adfRemoveEntry(volume.getPtr(), parentSector, name);
		}

		public static int RenameEntry(ADFVolume volume, uint parentSector, string oldName, uint newParentSector, string newName)
		{
			return AdfInterop.adfRenameEntry(volume.getPtr(), parentSector, oldName, newParentSector, newName);
		}

		public static int SetEntryAccess(ADFVolume volume, uint parentSector, string name, int newAccess)
		{
			return AdfInterop.adfSetEntryAccess(volume.getPtr(), parentSector, name, newAccess);
		}

		public static int SetEntryComment(ADFVolume volume, uint parentSector, string name, string newComment)
		{
			return AdfInterop.adfSetEntryComment(volume.getPtr(), parentSector, name, newComment);
		}

		public static bool IsDirEmpty(IntPtr dir)
		{
			return AdfInterop.adfIsDirEmpty(dir);
		}

		public static int DirCountEntries(ADFVolume volume, uint dirPtr)
		{
			return AdfInterop.adfDirCountEntries(volume.getPtr(), dirPtr);
		}

		public static int ReadEntryBlock(ADFVolume volume, uint sector, IntPtr entry)
		{
			return AdfInterop.adfReadEntryBlock(volume.getPtr(), sector, entry);
		}

		public static int WriteEntryBlock(ADFVolume volume, uint sector, IntPtr entry)
		{
			return AdfInterop.adfWriteEntryBlock(volume.getPtr(), sector, entry);
		}

		public static int WriteDirBlock(ADFVolume volume, uint sector, IntPtr dir)
		{
			return AdfInterop.adfWriteDirBlock(volume.getPtr(), sector, dir);
		}

		public static int EntBlockToEntry(IntPtr entryBlock, ref ADFEntry entry)
		{
			return AdfInterop.adfEntBlock2Entry(entryBlock, ref entry);
		}

		public static uint NameToEntryBlock(ADFVolume volume, IntPtr hashTable, string name, IntPtr entry, ref uint updatedSector)
		{
			return AdfInterop.adfNameToEntryBlk(volume.getPtr(), hashTable, name, entry, ref updatedSector);
		}

		public static string EntryGetInfo(ref ADFEntry entry)
		{
			IntPtr infoPtr = AdfInterop.adfEntryGetInfo(ref entry);
			string info = Marshal.PtrToStringAnsi(infoPtr);
			Marshal.FreeHGlobal(infoPtr); // Free the memory allocated by the native library
			return info;
		}
	}
}
