using System;
using System.Runtime.InteropServices;

namespace WodiLibSample.Component
{
    class FolderOpenDialog
    {
        public string Path { get; set; }
        public string Title { get; set; }

        public System.Windows.Forms.DialogResult ShowDialog()
        {
            return ShowDialog(IntPtr.Zero);
        }

        public System.Windows.Forms.DialogResult ShowDialog(System.Windows.Forms.IWin32Window owner)
        {
            return ShowDialog(owner.Handle);
        }

        public System.Windows.Forms.DialogResult ShowDialog(IntPtr owner)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            var dlg = (IFileOpenDialog) new FileOpenDialogInternal();
            try
            {
                dlg.SetOptions(FOS.FOS_PICKFOLDERS | FOS.FOS_FORCEFILESYSTEM);

                IShellItem item;
                if (!string.IsNullOrEmpty(Path))
                {
                    uint attrs = 0;
                    if (NativeMethods.SHILCreateFromPath(Path, out var idl, ref attrs) == 0)
                    {
                        if (NativeMethods.SHCreateShellItem(IntPtr.Zero, IntPtr.Zero, idl, out item) == 0)
                        {
                            dlg.SetFolder(item);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(this.Title))
                    dlg.SetTitle(this.Title);

                var hr = dlg.Show(owner);
                if (hr.Equals(NativeMethods.ERROR_CANCELLED))
                    return System.Windows.Forms.DialogResult.Cancel;
                if (!hr.Equals(0))
                    return System.Windows.Forms.DialogResult.Abort;

                dlg.GetResult(out item);
                item.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, out var outputPath);
                Path = outputPath;

                return System.Windows.Forms.DialogResult.OK;
            }
            finally
            {
                Marshal.FinalReleaseComObject(dlg);
            }
        }

        [ComImport]
        [Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")]
        private class FileOpenDialogInternal
        {
        }

        [ComImport]
        [Guid("42f85136-db7e-439c-85f1-e4075d135fc8")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IFileOpenDialog
        {
            [PreserveSig]
            UInt32 Show([In] IntPtr hwndParent);
            void SetFileTypes();    
            void SetFileTypeIndex();    
            void GetFileTypeIndex();    
            void Advise();
            void Unadvise();
            void SetOptions([In] FOS fos);
            void GetOptions();
            void SetDefaultFolder();
            void SetFolder(IShellItem psi);
            void GetFolder();
            void GetCurrentSelection();
            void SetFileName(); 
            void GetFileName(); 
            void SetTitle([In, MarshalAs(UnmanagedType.LPWStr)] string pszTitle);
            void SetOkButtonLabel();
            void SetFileNameLabel();
            void GetResult(out IShellItem ppsi);
            void AddPlace();
            void SetDefaultExtension();
            void Close();
            void SetClientGuid(); 
            void ClearClientData();
            void SetFilter();
            void GetResults();
            void GetSelectedItems();
        }

        [ComImport]
        [Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItem
        {
            void BindToHandler();
            void GetParent();
            void GetDisplayName([In] SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
            void GetAttributes(); 
            void Compare(); 
        }

        private enum SIGDN : uint
        {
            SIGDN_FILESYSPATH = 0x80058000,
        }

        [Flags]
        private enum FOS
        {
            FOS_FORCEFILESYSTEM = 0x40,
            FOS_PICKFOLDERS = 0x20,
        }

        private static class NativeMethods
        {
            [DllImport("shell32.dll")]
            public static extern int SHILCreateFromPath([MarshalAs(UnmanagedType.LPWStr)] string pszPath, out IntPtr ppIdl, ref uint rgflnOut);

            [DllImport("shell32.dll")]
            public static extern int SHCreateShellItem(IntPtr pidlParent, IntPtr psfParent, IntPtr pidl, out IShellItem ppsi);

            public const uint ERROR_CANCELLED = 0x800704C7;
        }
    }
}
