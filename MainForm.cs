using System.Runtime.InteropServices;
using System.Text;

namespace FlashFloppyUI
{
	public partial class MainForm : Form
	{
		/// <summary>
		/// Converts an item identifier list to a file system path. (Note: SHGetPathFromIDList calls the ANSI version, must call SHGetPathFromIDListW for .NET)
		/// </summary>
		/// <param name="pidl">Address of an item identifier list that specifies a file or directory location relative to the root of the namespace (the desktop).</param>
		/// <param name="pszPath">Address of a buffer to receive the file system path. This buffer must be at least MAX_PATH characters in size.</param>
		/// <returns>Returns TRUE if successful, or FALSE otherwise. </returns>
		[DllImport("shell32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SHGetPathFromIDListW(uint pidl, [MarshalAs(UnmanagedType.LPTStr)] ref string pszPath);

		private Models.Configuration _configuration = new Models.Configuration();
		public MainForm()
		{
			InitializeComponent();

			aDFFileReferenceBindingSource.DataSource = _configuration.ADFFileReferences;
		}

		private void gridView_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data?.GetDataPresent(DataFormats.FileDrop) ?? false)
			{
				var files = e.Data.GetData(DataFormats.FileDrop) as string[];
				if (files != null)
				{
					foreach (string file in files)
						_configuration.ADFFileReferences.Add(new Models.ADFFileReference(file));
				}
			}
		}

		private void gridView_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data?.GetDataPresent(DataFormats.FileDrop) ?? false)
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;
		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog dialog = new OpenFileDialog())
			{
				dialog.RestoreDirectory = true;
				dialog.ValidateNames = true;
				dialog.Filter = "*.adf|*.adf|All files|*.*";
				dialog.Multiselect = true;
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					foreach (string file in dialog.FileNames)
						_configuration.ADFFileReferences.Add(new Models.ADFFileReference(file));
				}
			}
		}

		private void buttonRemove_Click(object sender, EventArgs e)
		{
			foreach (var item in gridView.SelectedRows)
			{
				if (item is DataGridViewRow row && row.DataBoundItem is Models.ADFFileReference reference)
					_configuration.ADFFileReferences.Remove(reference);
			}
		}
	}
}
