namespace ArcaeaPatcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            MainSplitter = new Splitter();
            CoreFileLoadedInfo = new Label();
            MainDataGridView = new DataGridView();
            RightClickMenuStrip = new ContextMenuStrip(components);
            EditTitleLocalizedText = new ToolStripMenuItem();
            EditDifficultiesText = new ToolStripMenuItem();
            DelRowItem = new ToolStripMenuItem();
            FileItemButton = new ToolStripDropDownButton();
            FileItemButton_IPA = new ToolStripMenuItem();
            FileItemButton_Patch = new ToolStripMenuItem();
            SonglistItemButton = new ToolStripDropDownButton();
            SonglistItemButton_imSonglist = new ToolStripMenuItem();
            SonglistItemButton_exExcel = new ToolStripMenuItem();
            AboutItemButton = new ToolStripButton();
            ReadResItemButton = new ToolStripButton();
            MaintoolStrip = new ToolStrip();
            SaveResItemButton = new ToolStripButton();
            PackManagement = new ToolStripButton();
            MainSeparator = new ToolStripSeparator();
            ShowRowsCountLabel = new ToolStripLabel();
            ToolDetailsText = new ToolStripStatusLabel();
            ProgressBarText = new ToolStripStatusLabel();
            MainstatusStrip = new StatusStrip();
            SongIDText = new Label();
            SongNameText = new Label();
            BGpictureBox = new PictureBox();
            BGpictureBoxText = new Label();
            BGinversepictureBox = new PictureBox();
            BGinversepictureBoxText = new Label();
            BasepictureBoxText = new Label();
            BasepictureBox = new PictureBox();
            Base256pictureBox = new PictureBox();
            Base256pictureBoxText = new Label();
            ((System.ComponentModel.ISupportInitialize)MainDataGridView).BeginInit();
            RightClickMenuStrip.SuspendLayout();
            MaintoolStrip.SuspendLayout();
            MainstatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)BGpictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BGinversepictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BasepictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Base256pictureBox).BeginInit();
            SuspendLayout();
            // 
            // MainSplitter
            // 
            MainSplitter.Location = new Point(0, 25);
            MainSplitter.Name = "MainSplitter";
            MainSplitter.Size = new Size(309, 426);
            MainSplitter.TabIndex = 2;
            MainSplitter.TabStop = false;
            // 
            // CoreFileLoadedInfo
            // 
            CoreFileLoadedInfo.AutoSize = true;
            CoreFileLoadedInfo.Location = new Point(12, 38);
            CoreFileLoadedInfo.Name = "CoreFileLoadedInfo";
            CoreFileLoadedInfo.Size = new Size(104, 17);
            CoreFileLoadedInfo.TabIndex = 4;
            CoreFileLoadedInfo.Text = "核心文件：未加载";
            // 
            // MainDataGridView
            // 
            MainDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            MainDataGridView.ContextMenuStrip = RightClickMenuStrip;
            MainDataGridView.Dock = DockStyle.Fill;
            MainDataGridView.Location = new Point(309, 25);
            MainDataGridView.Name = "MainDataGridView";
            MainDataGridView.Size = new Size(699, 426);
            MainDataGridView.TabIndex = 5;
            MainDataGridView.CellClick += MainDataGridView_CellClick;
            MainDataGridView.CellValidating += MainDataGridView_CellValidating;
            // 
            // RightClickMenuStrip
            // 
            RightClickMenuStrip.Items.AddRange(new ToolStripItem[] { EditTitleLocalizedText, EditDifficultiesText, DelRowItem });
            RightClickMenuStrip.Name = "RightClickMenuStrip";
            RightClickMenuStrip.Size = new Size(161, 70);
            // 
            // EditTitleLocalizedText
            // 
            EditTitleLocalizedText.Name = "EditTitleLocalizedText";
            EditTitleLocalizedText.Size = new Size(160, 22);
            EditTitleLocalizedText.Text = "编辑本地化信息";
            EditTitleLocalizedText.Click += EditTitleLocalizedText_Click;
            // 
            // EditDifficultiesText
            // 
            EditDifficultiesText.Name = "EditDifficultiesText";
            EditDifficultiesText.Size = new Size(160, 22);
            EditDifficultiesText.Text = "编辑难度信息";
            EditDifficultiesText.Click += EditDifficultiesText_Click;
            // 
            // DelRowItem
            // 
            DelRowItem.Name = "DelRowItem";
            DelRowItem.Size = new Size(160, 22);
            DelRowItem.Text = "删除此行";
            DelRowItem.Click += DelRowItem_Click;
            // 
            // FileItemButton
            // 
            FileItemButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            FileItemButton.DropDownItems.AddRange(new ToolStripItem[] { FileItemButton_IPA, FileItemButton_Patch });
            FileItemButton.Image = (Image)resources.GetObject("FileItemButton.Image");
            FileItemButton.ImageTransparentColor = Color.Magenta;
            FileItemButton.Name = "FileItemButton";
            FileItemButton.Size = new Size(45, 22);
            FileItemButton.Text = "文件";
            // 
            // FileItemButton_IPA
            // 
            FileItemButton_IPA.Name = "FileItemButton_IPA";
            FileItemButton_IPA.Size = new Size(198, 22);
            FileItemButton_IPA.Text = "加载核心文件（*.ipa）";
            FileItemButton_IPA.ToolTipText = "为苹果端的安装程序文件\r\n同时提取已有的资源文件";
            FileItemButton_IPA.Click += FileItemButton_IPA_Click;
            // 
            // FileItemButton_Patch
            // 
            FileItemButton_Patch.Name = "FileItemButton_Patch";
            FileItemButton_Patch.Size = new Size(198, 22);
            FileItemButton_Patch.Text = "直接将资源注入补丁";
            FileItemButton_Patch.Click += FileItemButton_Patch_Click;
            // 
            // SonglistItemButton
            // 
            SonglistItemButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            SonglistItemButton.DropDownItems.AddRange(new ToolStripItem[] { SonglistItemButton_imSonglist, SonglistItemButton_exExcel });
            SonglistItemButton.Image = (Image)resources.GetObject("SonglistItemButton.Image");
            SonglistItemButton.ImageTransparentColor = Color.Magenta;
            SonglistItemButton.Name = "SonglistItemButton";
            SonglistItemButton.Size = new Size(69, 22);
            SonglistItemButton.Text = "歌曲列表";
            // 
            // SonglistItemButton_imSonglist
            // 
            SonglistItemButton_imSonglist.Name = "SonglistItemButton_imSonglist";
            SonglistItemButton_imSonglist.Size = new Size(193, 22);
            SonglistItemButton_imSonglist.Text = "从其他songlist中导入";
            SonglistItemButton_imSonglist.Click += SonglistItemButton_imSonglist_Click;
            // 
            // SonglistItemButton_exExcel
            // 
            SonglistItemButton_exExcel.Name = "SonglistItemButton_exExcel";
            SonglistItemButton_exExcel.Size = new Size(193, 22);
            SonglistItemButton_exExcel.Text = "导出为Excel";
            SonglistItemButton_exExcel.Click += SonglistItemButton_exExcel_Click;
            // 
            // AboutItemButton
            // 
            AboutItemButton.Alignment = ToolStripItemAlignment.Right;
            AboutItemButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            AboutItemButton.Image = (Image)resources.GetObject("AboutItemButton.Image");
            AboutItemButton.ImageTransparentColor = Color.Magenta;
            AboutItemButton.Name = "AboutItemButton";
            AboutItemButton.Size = new Size(36, 22);
            AboutItemButton.Text = "关于";
            AboutItemButton.TextImageRelation = TextImageRelation.TextBeforeImage;
            AboutItemButton.Click += AboutItemButton_Click;
            // 
            // ReadResItemButton
            // 
            ReadResItemButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            ReadResItemButton.Image = (Image)resources.GetObject("ReadResItemButton.Image");
            ReadResItemButton.ImageTransparentColor = Color.Magenta;
            ReadResItemButton.Name = "ReadResItemButton";
            ReadResItemButton.Size = new Size(60, 22);
            ReadResItemButton.Text = "读取资源";
            ReadResItemButton.ToolTipText = "点击即可自动加载资源文件\r\n加载核心文件之后方可使用";
            ReadResItemButton.Click += ReadResItemButton_Click;
            // 
            // MaintoolStrip
            // 
            MaintoolStrip.Items.AddRange(new ToolStripItem[] { FileItemButton, SonglistItemButton, AboutItemButton, ReadResItemButton, SaveResItemButton, PackManagement, MainSeparator, ShowRowsCountLabel });
            MaintoolStrip.Location = new Point(0, 0);
            MaintoolStrip.Name = "MaintoolStrip";
            MaintoolStrip.Size = new Size(1008, 25);
            MaintoolStrip.TabIndex = 1;
            // 
            // SaveResItemButton
            // 
            SaveResItemButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            SaveResItemButton.Image = (Image)resources.GetObject("SaveResItemButton.Image");
            SaveResItemButton.ImageTransparentColor = Color.Magenta;
            SaveResItemButton.Name = "SaveResItemButton";
            SaveResItemButton.Size = new Size(60, 22);
            SaveResItemButton.Text = "保存资源";
            SaveResItemButton.ToolTipText = "保存修改后的歌曲列表\r\n删除不在列表中的曲子";
            SaveResItemButton.Click += SaveResItemButton_Click;
            // 
            // PackManagement
            // 
            PackManagement.DisplayStyle = ToolStripItemDisplayStyle.Text;
            PackManagement.Image = (Image)resources.GetObject("PackManagement.Image");
            PackManagement.ImageTransparentColor = Color.Magenta;
            PackManagement.Name = "PackManagement";
            PackManagement.Size = new Size(60, 22);
            PackManagement.Text = "曲包管理";
            PackManagement.Click += PackManagement_Click;
            // 
            // MainSeparator
            // 
            MainSeparator.Name = "MainSeparator";
            MainSeparator.Size = new Size(6, 25);
            // 
            // ShowRowsCountLabel
            // 
            ShowRowsCountLabel.Name = "ShowRowsCountLabel";
            ShowRowsCountLabel.Size = new Size(63, 22);
            ShowRowsCountLabel.Text = "读取到0项";
            // 
            // ToolDetailsText
            // 
            ToolDetailsText.Name = "ToolDetailsText";
            ToolDetailsText.Size = new Size(391, 17);
            ToolDetailsText.Text = "此工具由SweelLong开发 - 专为Arcaea 苹果端定制 (右侧数据支持右键)";
            // 
            // ProgressBarText
            // 
            ProgressBarText.DisplayStyle = ToolStripItemDisplayStyle.Text;
            ProgressBarText.Name = "ProgressBarText";
            ProgressBarText.Size = new Size(68, 17);
            ProgressBarText.Text = "加载完毕！";
            // 
            // MainstatusStrip
            // 
            MainstatusStrip.Items.AddRange(new ToolStripItem[] { ToolDetailsText, ProgressBarText });
            MainstatusStrip.Location = new Point(0, 451);
            MainstatusStrip.Name = "MainstatusStrip";
            MainstatusStrip.Size = new Size(1008, 22);
            MainstatusStrip.TabIndex = 0;
            // 
            // SongIDText
            // 
            SongIDText.AutoSize = true;
            SongIDText.Location = new Point(12, 84);
            SongIDText.Name = "SongIDText";
            SongIDText.Size = new Size(69, 17);
            SongIDText.TabIndex = 6;
            SongIDText.Text = "歌曲ID：无";
            // 
            // SongNameText
            // 
            SongNameText.AutoSize = true;
            SongNameText.Location = new Point(12, 132);
            SongNameText.Name = "SongNameText";
            SongNameText.Size = new Size(80, 17);
            SongNameText.TabIndex = 7;
            SongNameText.Text = "歌曲名称：无";
            // 
            // BGpictureBox
            // 
            BGpictureBox.BackgroundImageLayout = ImageLayout.Zoom;
            BGpictureBox.BorderStyle = BorderStyle.FixedSingle;
            BGpictureBox.Location = new Point(12, 331);
            BGpictureBox.Name = "BGpictureBox";
            BGpictureBox.Size = new Size(104, 106);
            BGpictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            BGpictureBox.TabIndex = 8;
            BGpictureBox.TabStop = false;
            // 
            // BGpictureBoxText
            // 
            BGpictureBoxText.AutoSize = true;
            BGpictureBoxText.Location = new Point(13, 308);
            BGpictureBoxText.Name = "BGpictureBoxText";
            BGpictureBoxText.Size = new Size(60, 17);
            BGpictureBoxText.TabIndex = 9;
            BGpictureBoxText.Text = "bg图片：";
            // 
            // BGinversepictureBox
            // 
            BGinversepictureBox.BackgroundImageLayout = ImageLayout.Zoom;
            BGinversepictureBox.BorderStyle = BorderStyle.FixedSingle;
            BGinversepictureBox.Location = new Point(171, 331);
            BGinversepictureBox.Name = "BGinversepictureBox";
            BGinversepictureBox.Size = new Size(104, 106);
            BGinversepictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            BGinversepictureBox.TabIndex = 10;
            BGinversepictureBox.TabStop = false;
            // 
            // BGinversepictureBoxText
            // 
            BGinversepictureBoxText.AutoSize = true;
            BGinversepictureBoxText.Location = new Point(171, 308);
            BGinversepictureBoxText.Name = "BGinversepictureBoxText";
            BGinversepictureBoxText.Size = new Size(106, 17);
            BGinversepictureBoxText.TabIndex = 11;
            BGinversepictureBoxText.Text = "bg_inverse图片：";
            // 
            // BasepictureBoxText
            // 
            BasepictureBoxText.AutoSize = true;
            BasepictureBoxText.Location = new Point(13, 182);
            BasepictureBoxText.Name = "BasepictureBoxText";
            BasepictureBoxText.Size = new Size(72, 17);
            BasepictureBoxText.TabIndex = 12;
            BasepictureBoxText.Text = "base图片：";
            // 
            // BasepictureBox
            // 
            BasepictureBox.BackgroundImageLayout = ImageLayout.Zoom;
            BasepictureBox.BorderStyle = BorderStyle.FixedSingle;
            BasepictureBox.Location = new Point(12, 202);
            BasepictureBox.Name = "BasepictureBox";
            BasepictureBox.Size = new Size(104, 106);
            BasepictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            BasepictureBox.TabIndex = 13;
            BasepictureBox.TabStop = false;
            // 
            // Base256pictureBox
            // 
            Base256pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
            Base256pictureBox.BorderStyle = BorderStyle.FixedSingle;
            Base256pictureBox.Location = new Point(171, 202);
            Base256pictureBox.Name = "Base256pictureBox";
            Base256pictureBox.Size = new Size(104, 106);
            Base256pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            Base256pictureBox.TabIndex = 14;
            Base256pictureBox.TabStop = false;
            // 
            // Base256pictureBoxText
            // 
            Base256pictureBoxText.AutoSize = true;
            Base256pictureBoxText.Location = new Point(171, 182);
            Base256pictureBoxText.Name = "Base256pictureBoxText";
            Base256pictureBoxText.Size = new Size(98, 17);
            Base256pictureBoxText.TabIndex = 15;
            Base256pictureBoxText.Text = "base_256图片：";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1008, 473);
            Controls.Add(Base256pictureBoxText);
            Controls.Add(Base256pictureBox);
            Controls.Add(BasepictureBox);
            Controls.Add(BasepictureBoxText);
            Controls.Add(BGinversepictureBoxText);
            Controls.Add(BGinversepictureBox);
            Controls.Add(BGpictureBoxText);
            Controls.Add(BGpictureBox);
            Controls.Add(SongNameText);
            Controls.Add(SongIDText);
            Controls.Add(MainDataGridView);
            Controls.Add(CoreFileLoadedInfo);
            Controls.Add(MainSplitter);
            Controls.Add(MaintoolStrip);
            Controls.Add(MainstatusStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Arcaea 补丁程序 - 苹果端";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)MainDataGridView).EndInit();
            RightClickMenuStrip.ResumeLayout(false);
            MaintoolStrip.ResumeLayout(false);
            MaintoolStrip.PerformLayout();
            MainstatusStrip.ResumeLayout(false);
            MainstatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)BGpictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)BGinversepictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)BasepictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)Base256pictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Splitter MainSplitter;
        private Label CoreFileLoadedInfo;
        private DataGridView MainDataGridView;
        private ToolStripDropDownButton FileItemButton;
        private ToolStripMenuItem FileItemButton_IPA;
        private ToolStripDropDownButton SonglistItemButton;
        private ToolStripMenuItem SonglistItemButton_imSonglist;
        private ToolStripMenuItem SonglistItemButton_exExcel;
        private ToolStripButton AboutItemButton;
        private ToolStripButton ReadResItemButton;
        private ToolStrip MaintoolStrip;
        private ToolStripStatusLabel ToolDetailsText;
        private ToolStripStatusLabel ProgressBarText;
        private StatusStrip MainstatusStrip;
        private ToolStripButton SaveResItemButton;
        private Label SongIDText;
        private Label SongNameText;
        private PictureBox BGpictureBox;
        private Label BGpictureBoxText;
        private PictureBox BGinversepictureBox;
        private Label BGinversepictureBoxText;
        private ContextMenuStrip RightClickMenuStrip;
        private ToolStripMenuItem EditTitleLocalizedText;
        private ToolStripMenuItem EditDifficultiesText;
        private ToolStripMenuItem DelRowItem;
        private ToolStripMenuItem FileItemButton_Patch;
        private Label BasepictureBoxText;
        private PictureBox BasepictureBox;
        private PictureBox Base256pictureBox;
        private Label Base256pictureBoxText;
        private ToolStripSeparator MainSeparator;
        private ToolStripLabel ShowRowsCountLabel;
        private ToolStripButton PackManagement;
    }
}
