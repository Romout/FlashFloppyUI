using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace FlashFloppyUI
{
	public partial class MainForm : Form
	{
		private Models.Configuration _configuration = new Models.Configuration();

		private int _rowIndexFromMouseDown;
		private Rectangle _dragBoxFromMouseDown;

		public MainForm()
		{
			InitializeComponent();

			aDFFileReferenceBindingSource.DataSource = _configuration.ADFFileReferences;
		}


		private void gridView_MouseDown(object sender, MouseEventArgs e)
		{
			_rowIndexFromMouseDown = gridView.HitTest(e.X, e.Y).RowIndex;
			if (_rowIndexFromMouseDown != -1)
			{
				Size dragSize = SystemInformation.DragSize;
				_dragBoxFromMouseDown = new Rectangle(
					new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)),
					dragSize);
			}
			else
			{
				_dragBoxFromMouseDown = Rectangle.Empty;
			}
		}

		private void gridView_MouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
			{
				if (_dragBoxFromMouseDown != Rectangle.Empty &&
					!_dragBoxFromMouseDown.Contains(e.X, e.Y))
				{
					gridView.DoDragDrop(gridView.Rows[_rowIndexFromMouseDown], DragDropEffects.Move);
				}
			}
		}

		private void gridView_DragOver(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
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
			else
			{
				Point clientPoint = gridView.PointToClient(new Point(e.X, e.Y));
				int rowIndexOfItemUnderMouseToDrop = gridView.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

				if (rowIndexOfItemUnderMouseToDrop < 0 || _rowIndexFromMouseDown < 0)
					return;

				var list = (BindingList<Models.ADFFileReference>)aDFFileReferenceBindingSource.DataSource;
				var item = list[_rowIndexFromMouseDown];
				list.RemoveAt(_rowIndexFromMouseDown);
				list.Insert(rowIndexOfItemUnderMouseToDrop, item);

				gridView.ClearSelection();
				gridView.Rows[rowIndexOfItemUnderMouseToDrop].Selected = true;
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
			ADFSharp.InitializeEnvironment();

			string fileName = "startup-list.adf";
			if (File.Exists(fileName))
				File.Delete(fileName);

			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("FlashFloppyUI.empty.adf"))
			using (var targetStream = File.OpenWrite(fileName))
				stream?.CopyTo(targetStream);

			var device = ADFSharp.OpenDevice(Path.GetFullPath(fileName));
			ADFSharp.MountDevice(device);
			var volume = ADFSharp.MountFloppy(device);

			var file = ADFSharp.OpenFile(volume, "files.txt", AdfSharp.Interop.AdfFileMode.Write);
			var buf = Encoding.ASCII.GetBytes("Hello from FlashFloppyUI!");
			ADFSharp.WriteFile(file, buf);
			ADFSharp.CloseFile(file);

			ADFSharp.UnmountFloppy(volume);
			ADFSharp.UnmountDevice(device);

			ADFSharp.CleanUpEnvironment();
		}

		private void gridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			// Check if this is the "Index" column
			if (gridView.Columns[e.ColumnIndex].Name == "Index")
			{
				// Display the row number (1-based)
				e.Value = $"{e.RowIndex + 1}";
				e.FormattingApplied = true;
			}
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
