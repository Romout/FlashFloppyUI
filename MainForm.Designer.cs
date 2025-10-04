namespace FlashFloppyUI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
			gridView = new DataGridView();
			Index = new DataGridViewTextBoxColumn();
			nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			filePathDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			aDFFileReferenceBindingSource = new BindingSource(components);
			toolStrip = new ToolStrip();
			buttonLoad = new ToolStripButton();
			buttonSave = new ToolStripButton();
			buttonAdd = new ToolStripButton();
			buttonRemove = new ToolStripButton();
			buttonClear = new ToolStripButton();
			buttonUpdate = new ToolStripButton();
			buttonClose = new Button();
			((System.ComponentModel.ISupportInitialize)gridView).BeginInit();
			((System.ComponentModel.ISupportInitialize)aDFFileReferenceBindingSource).BeginInit();
			toolStrip.SuspendLayout();
			SuspendLayout();
			// 
			// gridView
			// 
			gridView.AllowDrop = true;
			gridView.AllowUserToAddRows = false;
			gridView.AllowUserToResizeRows = false;
			gridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			gridView.AutoGenerateColumns = false;
			gridView.Columns.AddRange(new DataGridViewColumn[] { Index, nameDataGridViewTextBoxColumn, filePathDataGridViewTextBoxColumn });
			gridView.DataSource = aDFFileReferenceBindingSource;
			gridView.Location = new Point(12, 28);
			gridView.Name = "gridView";
			gridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
			gridView.RowTemplate.Resizable = DataGridViewTriState.False;
			gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			gridView.ShowCellErrors = false;
			gridView.ShowEditingIcon = false;
			gridView.Size = new Size(776, 388);
			gridView.TabIndex = 0;
			gridView.CellFormatting += gridView_CellFormatting;
			gridView.DragDrop += gridView_DragDrop;
			gridView.DragEnter += gridView_DragEnter;
			gridView.DragOver += gridView_DragOver;
			gridView.MouseDown += gridView_MouseDown;
			gridView.MouseMove += gridView_MouseMove;
			// 
			// Index
			// 
			dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleRight;
			Index.DefaultCellStyle = dataGridViewCellStyle1;
			Index.HeaderText = "Nr.";
			Index.Name = "Index";
			Index.ReadOnly = true;
			Index.Width = 50;
			// 
			// nameDataGridViewTextBoxColumn
			// 
			nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
			nameDataGridViewTextBoxColumn.HeaderText = "Name";
			nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
			// 
			// filePathDataGridViewTextBoxColumn
			// 
			filePathDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			filePathDataGridViewTextBoxColumn.DataPropertyName = "FilePath";
			filePathDataGridViewTextBoxColumn.HeaderText = "FilePath";
			filePathDataGridViewTextBoxColumn.Name = "filePathDataGridViewTextBoxColumn";
			filePathDataGridViewTextBoxColumn.ReadOnly = true;
			filePathDataGridViewTextBoxColumn.Resizable = DataGridViewTriState.False;
			// 
			// aDFFileReferenceBindingSource
			// 
			aDFFileReferenceBindingSource.DataSource = typeof(Models.ADFFileReference);
			// 
			// toolStrip
			// 
			toolStrip.Items.AddRange(new ToolStripItem[] { buttonLoad, buttonSave, buttonAdd, buttonRemove, buttonClear, buttonUpdate });
			toolStrip.Location = new Point(0, 0);
			toolStrip.Name = "toolStrip";
			toolStrip.Size = new Size(800, 25);
			toolStrip.TabIndex = 1;
			toolStrip.Text = "toolStrip";
			// 
			// buttonLoad
			// 
			buttonLoad.Image = Properties.Resources.Custom_Icon_Design_Pretty_Office_9_Open_file_48;
			buttonLoad.ImageTransparentColor = Color.Magenta;
			buttonLoad.Name = "buttonLoad";
			buttonLoad.Size = new Size(53, 22);
			buttonLoad.Text = "Load";
			buttonLoad.ToolTipText = "Load configuration";
			buttonLoad.Click += buttonLoad_Click;
			// 
			// buttonSave
			// 
			buttonSave.Image = Properties.Resources.Oxygen_Icons_org_Oxygen_Actions_document_save_48;
			buttonSave.ImageTransparentColor = Color.Magenta;
			buttonSave.Name = "buttonSave";
			buttonSave.Size = new Size(51, 22);
			buttonSave.Text = "Save";
			buttonSave.ToolTipText = "Save configuration";
			buttonSave.Click += buttonSave_Click;
			// 
			// buttonAdd
			// 
			buttonAdd.Image = Properties.Resources.Hopstarter_Soft_Scraps_Button_Add_48;
			buttonAdd.ImageTransparentColor = Color.Magenta;
			buttonAdd.Name = "buttonAdd";
			buttonAdd.Size = new Size(49, 22);
			buttonAdd.Text = "Add";
			buttonAdd.Click += buttonAdd_Click;
			// 
			// buttonRemove
			// 
			buttonRemove.Image = Properties.Resources.Hopstarter_Soft_Scraps_Button_Close_48;
			buttonRemove.ImageTransparentColor = Color.Magenta;
			buttonRemove.Name = "buttonRemove";
			buttonRemove.Size = new Size(70, 22);
			buttonRemove.Text = "Remove";
			buttonRemove.Click += buttonRemove_Click;
			// 
			// buttonClear
			// 
			buttonClear.Image = Properties.Resources.Hopstarter_Soft_Scraps_Button_Turn_Off_48;
			buttonClear.ImageTransparentColor = Color.Magenta;
			buttonClear.Name = "buttonClear";
			buttonClear.Size = new Size(54, 22);
			buttonClear.Text = "Clear";
			buttonClear.Click += buttonClear_Click;
			// 
			// buttonUpdate
			// 
			buttonUpdate.Image = Properties.Resources.Hopstarter_Soft_Scraps_Button_Upload_48;
			buttonUpdate.ImageTransparentColor = Color.Magenta;
			buttonUpdate.Name = "buttonUpdate";
			buttonUpdate.Size = new Size(165, 22);
			buttonUpdate.Text = "Update USB Drive Content";
			buttonUpdate.Click += buttonUpdate_Click;
			// 
			// buttonClose
			// 
			buttonClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonClose.Location = new Point(713, 422);
			buttonClose.Name = "buttonClose";
			buttonClose.Size = new Size(75, 23);
			buttonClose.TabIndex = 2;
			buttonClose.Text = "&Close";
			buttonClose.UseVisualStyleBackColor = true;
			buttonClose.Click += buttonClose_Click;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(buttonClose);
			Controls.Add(toolStrip);
			Controls.Add(gridView);
			Name = "MainForm";
			Text = "Flash Floppy UI";
			((System.ComponentModel.ISupportInitialize)gridView).EndInit();
			((System.ComponentModel.ISupportInitialize)aDFFileReferenceBindingSource).EndInit();
			toolStrip.ResumeLayout(false);
			toolStrip.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private DataGridView gridView;
		private ToolStrip toolStrip;
		private ToolStripButton buttonAdd;
		private ToolStripButton buttonRemove;
		private Button buttonClose;
		private ToolStripButton buttonUpdate;
		private BindingSource aDFFileReferenceBindingSource;
		private ToolStripButton buttonSave;
		private ToolStripButton buttonLoad;
		private DataGridViewTextBoxColumn Index;
		private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn filePathDataGridViewTextBoxColumn;
		private ToolStripButton buttonClear;
	}
}
