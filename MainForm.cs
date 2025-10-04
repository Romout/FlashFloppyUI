using System.ComponentModel;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace FlashFloppyUI
{
	public partial class MainForm : Form
	{
		private Controller _controller;

        private int _rowIndexFromMouseDown;
		private Rectangle _dragBoxFromMouseDown;

		public MainForm()
		{
			InitializeComponent();

			_controller = new Controller(new Models.Configuration());

            aDFFileReferenceBindingSource.DataSource = _controller.Configuration.ADFFileReferences;
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
					_controller.AddADFFileReferences(files);
			}
			else
			{
				Point clientPoint = gridView.PointToClient(new Point(e.X, e.Y));
				int rowIndexOfItemUnderMouseToDrop = gridView.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

				if (rowIndexOfItemUnderMouseToDrop < 0 || _rowIndexFromMouseDown < 0)
					return;

				_controller.MoveADFFileReference(_rowIndexFromMouseDown, rowIndexOfItemUnderMouseToDrop);

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
					_controller.AddADFFileReferences(dialog.FileNames);
			}
		}

		private void buttonRemove_Click(object sender, EventArgs e)
		{
			foreach (var item in gridView.SelectedRows)
			{
				if (item is DataGridViewRow row && row.DataBoundItem is Models.ADFFileReference reference)
					_controller.RemoveADFFileReference(reference);
            }
		}

		private void buttonUpdate_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(_controller.Configuration.TargetFolder) || !Directory.Exists(_controller.Configuration.TargetFolder))
			{
				using (FolderBrowserDialog dialog = new FolderBrowserDialog())
				{
					dialog.Description = "Select target folder (USBDrive) to copy ADF files to";
					if (dialog.ShowDialog() == DialogResult.OK)
						_controller.SetTargetFolder(dialog.SelectedPath);
					else
						return;
                }
            }

			_controller.UpdateTargetFolderContent();
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

		private void buttonLoad_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog dialog = new OpenFileDialog())
			{
				dialog.RestoreDirectory = true;
				dialog.ValidateNames = true;
				dialog.Filter = "Flash Floppy Configuration files|*.ffcfg|All files|*.*";
				dialog.Multiselect = false;
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					_controller.LoadConfiguration(dialog.FileName);
                    aDFFileReferenceBindingSource.DataSource = _controller.Configuration.ADFFileReferences;
				}
			}
		}

		private void buttonSave_Click(object sender, EventArgs e)
		{
			using (SaveFileDialog dialog = new SaveFileDialog())
			{
				dialog.RestoreDirectory = true;
				dialog.ValidateNames = true;
				dialog.Filter = "Flash Floppy Configuration files|*.ffcfg|All files|*.*";
				dialog.OverwritePrompt = true;
				if (dialog.ShowDialog() == DialogResult.OK)
					_controller.SaveConfiguration(dialog.FileName);
			}
		}
	}
}
