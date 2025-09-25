using System.Runtime.InteropServices;
using System.Text;

namespace FlashFloppyUI
{
	public partial class MainForm : Form
	{
		private Models.Configuration _configuration = new Models.Configuration();
		ADFSharp.ADFLib _adf;

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

		private void buttonUpdate_Click(object sender, EventArgs e)
		{
			ADFSharp.ADFLib adf = new ADFSharp.ADFLib();
			var device = adf.CreateDevice("test.adf");
			if (adf.CreateFloppy(device, "Super Floppy!"))
			{
				var volume = adf.MountFloppy(device);
				var file = adf.OpenFile(volume, "TEST.TXT", false, true);
				var buf = Encoding.ASCII.GetBytes("Hello from FlashFloppyUI!");
				adf.WriteFile(file, buf, 0, buf.Length);
				adf.CloseFile(file);
				adf.UnmountFloppy(volume);
				adf.UnmountDevice(device);
				adf.CleanUpEnvironment();
			}
		}
	}
}
