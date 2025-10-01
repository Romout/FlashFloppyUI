using System.Runtime.InteropServices;
using System.Text;

namespace FlashFloppyUI
{
	public partial class MainForm : Form
	{
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

		private void Log(string message, IntPtr args)
		{
			Console.WriteLine(message);
		}
		private void Notify(uint sectNum, int value)
		{ }

		private void buttonUpdate_Click(object sender, EventArgs e)
		{
			AdfSharp.Interop.AdfInterop.adfEnvSetFct(Log, Log, Log, Notify);
			AdfSharp.Interop.AdfInterop.adfEnvSetProperty(AdfSharp.Interop.AdfEnvProperty.PR_EFCT, 1);
			AdfSharp.Interop.AdfInterop.adfEnvSetProperty(AdfSharp.Interop.AdfEnvProperty.PR_WFCT, 1);
			AdfSharp.Interop.AdfInterop.adfEnvSetProperty(AdfSharp.Interop.AdfEnvProperty.PR_VFCT, 1);
			ADFSharp.InitializeEnvironment();

			string fileName = "test.adf";
			if (File.Exists(fileName))
				File.Delete(fileName);

			var device = ADFSharp.CreateDevice(Path.GetFullPath(fileName));
			if (ADFSharp.CreateFloppy(device, "Super Floppy!"))
			{
				var volume = ADFSharp.MountFloppy(device);

				var file = ADFSharp.OpenFile(volume, "TEST.TXT", AdfSharp.Interop.AdfFileMode.Write);
				var buf = Encoding.ASCII.GetBytes("Hello from FlashFloppyUI!");
				ADFSharp.WriteFile(file, buf);
				ADFSharp.CloseFile(file);

                ADFSharp.InstallBootBlock(volume, ADFSharp.BootBlockType.Kick13);

                ADFSharp.UnmountFloppy(volume);
				ADFSharp.UnmountDevice(device);
				ADFSharp.CleanUpEnvironment();
			}
		}
	}
}
