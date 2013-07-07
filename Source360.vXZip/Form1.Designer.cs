namespace vXZip
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.sideBar1 = new DevComponents.DotNetBar.SideBar();
            this.sbpi_zipOperations = new DevComponents.DotNetBar.SideBarPanelItem();
            this.btn_openZip = new DevComponents.DotNetBar.ButtonItem();
            this.btn_saveZip = new DevComponents.DotNetBar.ButtonItem();
            this.btn_closeZip = new DevComponents.DotNetBar.ButtonItem();
            this.btn_createZip = new DevComponents.DotNetBar.ButtonItem();
            this.labelItem2 = new DevComponents.DotNetBar.LabelItem();
            this.btn_extractAllFiles = new DevComponents.DotNetBar.ButtonItem();
            this.btn_addFile = new DevComponents.DotNetBar.ButtonItem();
            this.btn_deleteFile = new DevComponents.DotNetBar.ButtonItem();
            this.btn_updateFile = new DevComponents.DotNetBar.ButtonItem();
            this.btn_extractFile = new DevComponents.DotNetBar.ButtonItem();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.lbl_renameFile = new DevComponents.DotNetBar.LabelItem();
            this.tbx_rename = new DevComponents.DotNetBar.TextBoxItem();
            this.btn_rename = new DevComponents.DotNetBar.ButtonItem();
            this.lv_zipListView = new System.Windows.Forms.ListView();
            this.clm_fileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clm_size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rtb_filePreview = new System.Windows.Forms.RichTextBox();
            this.btx_savePreview = new DevComponents.DotNetBar.ButtonX();
            this.lbx_previewName = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2007VistaGlass;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(87)))), ((int)(((byte)(154))))));
            // 
            // sideBar1
            // 
            this.sideBar1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.sideBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.sideBar1.Appearance = DevComponents.DotNetBar.eSideBarAppearance.Flat;
            this.sideBar1.BorderStyle = DevComponents.DotNetBar.eBorderType.None;
            this.sideBar1.ExpandedPanel = this.sbpi_zipOperations;
            this.sideBar1.Location = new System.Drawing.Point(12, 12);
            this.sideBar1.Name = "sideBar1";
            this.sideBar1.Panels.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.sbpi_zipOperations});
            this.sideBar1.Size = new System.Drawing.Size(146, 483);
            this.sideBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.sideBar1.TabIndex = 2;
            this.sideBar1.Text = "v";
            this.sideBar1.UsingSystemColors = true;
            // 
            // sbpi_zipOperations
            // 
            this.sbpi_zipOperations.BackgroundStyle.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.sbpi_zipOperations.BackgroundStyle.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.sbpi_zipOperations.BackgroundStyle.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.sbpi_zipOperations.BackgroundStyle.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(45)))), ((int)(((byte)(150)))));
            this.sbpi_zipOperations.BackgroundStyle.ForeColor.Color = System.Drawing.SystemColors.ControlText;
            this.sbpi_zipOperations.FontBold = true;
            this.sbpi_zipOperations.HeaderHotStyle.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(173)))), ((int)(((byte)(228)))));
            this.sbpi_zipOperations.HeaderHotStyle.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.sbpi_zipOperations.HeaderHotStyle.ForeColor.Color = System.Drawing.SystemColors.ControlText;
            this.sbpi_zipOperations.HeaderHotStyle.GradientAngle = 90;
            this.sbpi_zipOperations.HeaderSideHotStyle.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(173)))), ((int)(((byte)(228)))));
            this.sbpi_zipOperations.HeaderSideHotStyle.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.sbpi_zipOperations.HeaderSideHotStyle.GradientAngle = 90;
            this.sbpi_zipOperations.HeaderSideStyle.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.sbpi_zipOperations.HeaderSideStyle.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.sbpi_zipOperations.HeaderSideStyle.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.sbpi_zipOperations.HeaderSideStyle.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(45)))), ((int)(((byte)(150)))));
            this.sbpi_zipOperations.HeaderSideStyle.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Top)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.sbpi_zipOperations.HeaderSideStyle.GradientAngle = 90;
            this.sbpi_zipOperations.HeaderStyle.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.sbpi_zipOperations.HeaderStyle.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(173)))), ((int)(((byte)(228)))));
            this.sbpi_zipOperations.HeaderStyle.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.sbpi_zipOperations.HeaderStyle.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(45)))), ((int)(((byte)(150)))));
            this.sbpi_zipOperations.HeaderStyle.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Right | DevComponents.DotNetBar.eBorderSide.Top)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.sbpi_zipOperations.HeaderStyle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.sbpi_zipOperations.HeaderStyle.ForeColor.Color = System.Drawing.SystemColors.ControlText;
            this.sbpi_zipOperations.HeaderStyle.GradientAngle = 90;
            this.sbpi_zipOperations.Name = "sbpi_zipOperations";
            this.sbpi_zipOperations.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btn_openZip,
            this.btn_saveZip,
            this.btn_closeZip,
            this.btn_createZip,
            this.labelItem2,
            this.btn_extractAllFiles,
            this.btn_addFile,
            this.btn_deleteFile,
            this.btn_updateFile,
            this.btn_extractFile,
            this.labelItem1,
            this.lbl_renameFile,
            this.tbx_rename,
            this.btn_rename});
            this.sbpi_zipOperations.Text = "Zip Operations";
            // 
            // btn_openZip
            // 
            this.btn_openZip.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btn_openZip.ImagePosition = DevComponents.DotNetBar.eImagePosition.Right;
            this.btn_openZip.Name = "btn_openZip";
            this.btn_openZip.Text = "Open Zip";
            this.btn_openZip.Click += new System.EventHandler(this.btn_openZip_Click);
            // 
            // btn_saveZip
            // 
            this.btn_saveZip.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btn_saveZip.Enabled = false;
            this.btn_saveZip.ImagePosition = DevComponents.DotNetBar.eImagePosition.Right;
            this.btn_saveZip.Name = "btn_saveZip";
            this.btn_saveZip.Text = "Save Zip";
            this.btn_saveZip.Click += new System.EventHandler(this.btn_saveZip_Click);
            // 
            // btn_closeZip
            // 
            this.btn_closeZip.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btn_closeZip.Enabled = false;
            this.btn_closeZip.ImagePosition = DevComponents.DotNetBar.eImagePosition.Right;
            this.btn_closeZip.Name = "btn_closeZip";
            this.btn_closeZip.Text = "Close Zip";
            this.btn_closeZip.Click += new System.EventHandler(this.btn_closeZip_Click);
            // 
            // btn_createZip
            // 
            this.btn_createZip.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btn_createZip.Name = "btn_createZip";
            this.btn_createZip.Text = "Create New Zip";
            this.btn_createZip.Click += new System.EventHandler(this.btn_createZip_Click);
            // 
            // labelItem2
            // 
            this.labelItem2.Name = "labelItem2";
            // 
            // btn_extractAllFiles
            // 
            this.btn_extractAllFiles.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btn_extractAllFiles.Enabled = false;
            this.btn_extractAllFiles.Name = "btn_extractAllFiles";
            this.btn_extractAllFiles.Text = "Extract All Files";
            this.btn_extractAllFiles.Click += new System.EventHandler(this.btn_extractAllFiles_Click);
            // 
            // btn_addFile
            // 
            this.btn_addFile.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btn_addFile.Enabled = false;
            this.btn_addFile.Name = "btn_addFile";
            this.btn_addFile.Text = "Add File";
            this.btn_addFile.Click += new System.EventHandler(this.btn_addFile_Click);
            // 
            // btn_deleteFile
            // 
            this.btn_deleteFile.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btn_deleteFile.Enabled = false;
            this.btn_deleteFile.Name = "btn_deleteFile";
            this.btn_deleteFile.Text = "Delete File";
            this.btn_deleteFile.Click += new System.EventHandler(this.btn_deleteFile_Click);
            // 
            // btn_updateFile
            // 
            this.btn_updateFile.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btn_updateFile.Enabled = false;
            this.btn_updateFile.Name = "btn_updateFile";
            this.btn_updateFile.Text = "Update File";
            this.btn_updateFile.Click += new System.EventHandler(this.btn_updateFile_Click);
            // 
            // btn_extractFile
            // 
            this.btn_extractFile.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btn_extractFile.Enabled = false;
            this.btn_extractFile.ImagePosition = DevComponents.DotNetBar.eImagePosition.Right;
            this.btn_extractFile.Name = "btn_extractFile";
            this.btn_extractFile.Text = "Extract File";
            this.btn_extractFile.Click += new System.EventHandler(this.btn_extractFile_Click);
            // 
            // labelItem1
            // 
            this.labelItem1.Name = "labelItem1";
            // 
            // lbl_renameFile
            // 
            this.lbl_renameFile.Name = "lbl_renameFile";
            this.lbl_renameFile.Text = "Rename File";
            // 
            // tbx_rename
            // 
            this.tbx_rename.Enabled = false;
            this.tbx_rename.Name = "tbx_rename";
            this.tbx_rename.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbx_rename.WatermarkColor = System.Drawing.SystemColors.GrayText;
            // 
            // btn_rename
            // 
            this.btn_rename.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btn_rename.Enabled = false;
            this.btn_rename.Name = "btn_rename";
            this.btn_rename.Text = "Rename";
            this.btn_rename.Click += new System.EventHandler(this.btn_rename_Click);
            // 
            // lv_zipListView
            // 
            this.lv_zipListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lv_zipListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clm_fileName,
            this.clm_size});
            this.lv_zipListView.GridLines = true;
            this.lv_zipListView.Location = new System.Drawing.Point(164, 12);
            this.lv_zipListView.Name = "lv_zipListView";
            this.lv_zipListView.Size = new System.Drawing.Size(508, 483);
            this.lv_zipListView.TabIndex = 3;
            this.lv_zipListView.UseCompatibleStateImageBehavior = false;
            this.lv_zipListView.View = System.Windows.Forms.View.Details;
            this.lv_zipListView.SelectedIndexChanged += new System.EventHandler(this.lv_zipListView_SelectedIndexChanged);
            // 
            // clm_fileName
            // 
            this.clm_fileName.Text = "File Name";
            this.clm_fileName.Width = 390;
            // 
            // clm_size
            // 
            this.clm_size.Text = "Size";
            this.clm_size.Width = 111;
            // 
            // rtb_filePreview
            // 
            this.rtb_filePreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb_filePreview.Location = new System.Drawing.Point(678, 12);
            this.rtb_filePreview.Name = "rtb_filePreview";
            this.rtb_filePreview.Size = new System.Drawing.Size(427, 454);
            this.rtb_filePreview.TabIndex = 4;
            this.rtb_filePreview.Text = "";
            // 
            // btx_savePreview
            // 
            this.btx_savePreview.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btx_savePreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btx_savePreview.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btx_savePreview.Enabled = false;
            this.btx_savePreview.Location = new System.Drawing.Point(1030, 472);
            this.btx_savePreview.Name = "btx_savePreview";
            this.btx_savePreview.Size = new System.Drawing.Size(75, 23);
            this.btx_savePreview.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btx_savePreview.TabIndex = 5;
            this.btx_savePreview.Text = "Save";
            this.btx_savePreview.Click += new System.EventHandler(this.btx_savePreview_Click);
            // 
            // lbx_previewName
            // 
            this.lbx_previewName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.lbx_previewName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbx_previewName.Location = new System.Drawing.Point(678, 472);
            this.lbx_previewName.Name = "lbx_previewName";
            this.lbx_previewName.Size = new System.Drawing.Size(346, 23);
            this.lbx_previewName.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1117, 507);
            this.Controls.Add(this.lbx_previewName);
            this.Controls.Add(this.btx_savePreview);
            this.Controls.Add(this.rtb_filePreview);
            this.Controls.Add(this.lv_zipListView);
            this.Controls.Add(this.sideBar1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "vXZip";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }



        #endregion

        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.SideBar sideBar1;
        private DevComponents.DotNetBar.SideBarPanelItem sbpi_zipOperations;
        private DevComponents.DotNetBar.ButtonItem btn_openZip;
        private DevComponents.DotNetBar.ButtonItem btn_closeZip;
        private DevComponents.DotNetBar.LabelItem labelItem2;
        private DevComponents.DotNetBar.ButtonItem btn_addFile;
        private DevComponents.DotNetBar.ButtonItem btn_deleteFile;
        private DevComponents.DotNetBar.ButtonItem btn_updateFile;
        private DevComponents.DotNetBar.ButtonItem btn_extractFile;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        private DevComponents.DotNetBar.LabelItem lbl_renameFile;
        private DevComponents.DotNetBar.TextBoxItem tbx_rename;
        private System.Windows.Forms.ListView lv_zipListView;
        private System.Windows.Forms.ColumnHeader clm_fileName;
        private System.Windows.Forms.ColumnHeader clm_size;
        private DevComponents.DotNetBar.ButtonItem btn_createZip;
        private DevComponents.DotNetBar.ButtonItem btn_extractAllFiles;
        private DevComponents.DotNetBar.ButtonItem btn_saveZip;
        private System.Windows.Forms.RichTextBox rtb_filePreview;
        private DevComponents.DotNetBar.ButtonX btx_savePreview;
        private DevComponents.DotNetBar.LabelX lbx_previewName;
        private DevComponents.DotNetBar.ButtonItem btn_rename;
    }
}

