using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using DevComponents;
using DevComponents.DotNetBar;
using DevComponents.AdvTree;
using Source360.xZip;
using Source360.xZip.XZP2;
using System.Diagnostics;
namespace vXZip
{
    public partial class Form1 : Office2007Form
    {
        private ZipPackageFile m_zipPackage;
        private int currentIndex;
        private System.Timers.Timer timer = new System.Timers.Timer(2000);
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Populates the list view from the zip directory. Usually called after updating the directory
        /// </summary>
        private void PopulateListView()
        {
            this.lv_zipListView.Items.Clear();
            this.lv_zipListView.BeginUpdate();
            foreach (ZipFile file in this.m_zipPackage.ZipDirectory)
                this.lv_zipListView.Items.Add(new ListViewItem(new string[] { file.FileName, file.FileSize.ToString(), file.Preloaded.ToString() }));
            this.lv_zipListView.EndUpdate();
            this.lv_zipListView.Update();
        }
        /// <summary>
        /// Handles new selection of file
        /// </summary>
        private void lv_zipListView_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                currentIndex = lv_zipListView.SelectedItems[0].Index;
                string indexFilename = this.m_zipPackage.ZipDirectory[currentIndex].FileName;
                string ending = indexFilename.Substring(indexFilename.LastIndexOf('.'));
                this.tbx_rename.Text = indexFilename;
                /// Check if we can preview the file
                if (ending == ".cfg" || ending == ".txt" || ending == ".nut" ||
                    ending == ".res" || ending == ".inf")
                {
                    this.lbx_previewName.Text = indexFilename;
                    this.btx_savePreview.Enabled = true;
                    this.rtb_filePreview.Text = ASCIIEncoding.ASCII.GetString(this.m_zipPackage.GetFileData(currentIndex));
                }
                else
                {
                    this.rtb_filePreview.Text = "";
                    this.btx_savePreview.Enabled = false;
                    this.lbx_previewName.Text = "";
                }
            }
            catch (System.Exception ex)
            {
            	
            }

        }
        /// <summary>
        /// Called after the 2 second timer is up to signal a change back to the file name
        /// </summary>
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.lbx_previewName.InvokeRequired)
            {
                this.lbx_previewName.Invoke(new MethodInvoker(delegate 
                    {
                        this.lbx_previewName.Text = this.m_zipPackage.ZipDirectory[currentIndex].ChildEntry.FileName;
                        timer.Stop();
                    }));
            }
            
        }
        #region Button functions
        private void btn_saveZip_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Save XZip file";
                sfd.Filter = "All Files (*.*)|*.*|XZip Files (.360.zip)|*.360.zip";
                sfd.FilterIndex = 1;
                if (sfd.ShowDialog() == DialogResult.OK)
                    this.m_zipPackage.Save(sfd.FileName, true);
            }
        }

        private void btn_deleteFile_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("Are you sure you want to delete {0}?", this.m_zipPackage.ZipDirectory[currentIndex].FileName),
                "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                this.m_zipPackage.DeleteFile(currentIndex);
                PopulateListView();
            }
            else
                return;
        }

        private void btn_updateFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = string.Format("Open file to replace {0}", this.m_zipPackage.ZipDirectory[currentIndex].FileName);
                if (ofd.ShowDialog() == DialogResult.OK)
                    this.m_zipPackage.UpdateFile(currentIndex, ofd.FileName);
                PopulateListView();
            }
        }

        private void btn_extractFile_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = string.Format("Select location to save {0}", this.m_zipPackage.ZipDirectory[currentIndex].FileName);
                if (sfd.ShowDialog() == DialogResult.OK)
                    this.m_zipPackage.ExtractFile(currentIndex, sfd.FileName);
            }
        }

        private void btn_rename_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("Are you sure you want to rename {0} to {1}?", this.m_zipPackage.ZipDirectory[currentIndex].FileName,tbx_rename.Text),
                "Confirm Renaming", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                this.m_zipPackage.RenameFile(currentIndex, this.tbx_rename.Text);
                PopulateListView();
            }
            else
                return;
            
        }

        private void btn_addFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.m_zipPackage.AddFile(ofd.FileName);
                    PopulateListView();
                }
            }
        }

        private void btn_closeZip_Click(object sender, EventArgs e)
        {

            this.m_zipPackage.Close();
            this.lv_zipListView.Items.Clear();
            currentIndex = 0;
            this.lbx_previewName.Text = this.tbx_rename.Text = this.rtb_filePreview.Text = "";
            this.btn_addFile.Enabled = this.btn_deleteFile.Enabled = this.btn_extractFile.Enabled =
                this.btn_updateFile.Enabled = this.tbx_rename.Enabled = this.btn_closeZip.Enabled =
                this.btn_extractAllFiles.Enabled = this.btn_saveZip.Enabled = this.btx_savePreview.Enabled = false;

        }

        private void btn_extractAllFiles_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                this.m_zipPackage.ExtractAllFiles(fbd.SelectedPath);
        }

        private void btn_openZip_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Open XZip file";
                ofd.Filter = "All Files (*.*)|*.*|XZip Files (.360.zip)|*.360.zip";
                ofd.FilterIndex = 1;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.m_zipPackage = new ZipPackageFile(ofd.FileName);
                    this.m_zipPackage.UnSerialize();
                    PopulateListView();
                    this.btn_addFile.Enabled = this.btn_deleteFile.Enabled = this.btn_extractFile.Enabled =
                        this.btn_updateFile.Enabled = this.tbx_rename.Enabled = this.btn_closeZip.Enabled =
                        this.btn_extractAllFiles.Enabled = this.btn_saveZip.Enabled = this.btn_rename.Enabled = true;

                }
            }
        }

        private void btx_savePreview_Click(object sender, EventArgs e)
        {
            this.lbx_previewName.Text = "Saved!";
            this.m_zipPackage.UpdateFile(currentIndex, ASCIIEncoding.ASCII.GetBytes(rtb_filePreview.Text));

            timer.Start();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
        }

        private void btn_createZip_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Choose save location";
                fbd.Description = "Choose zip folder";
                if (sfd.ShowDialog() == DialogResult.OK && fbd.ShowDialog() == DialogResult.OK)
                {
                    ZipPackageFile pack = new ZipPackageFile(fbd.SelectedPath, sfd.FileName);
                }
            }

        }
        #endregion


    }
}
