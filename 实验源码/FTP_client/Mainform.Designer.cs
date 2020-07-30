namespace FTP_client
{
    partial class Mainform
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mainform));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_pause = new System.Windows.Forms.Button();
            this.button_refresh = new System.Windows.Forms.Button();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button_download = new System.Windows.Forms.Button();
            this.button_upload = new System.Windows.Forms.Button();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.textBox_user = new System.Windows.Forms.TextBox();
            this.textBox_server = new System.Windows.Forms.TextBox();
            this.button_connect = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeLocal = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.listBox_log = new System.Windows.Forms.ListBox();
            this.listBox_Ftp_server = new System.Windows.Forms.ListBox();
            this.saveFileDialog_downloadto = new System.Windows.Forms.SaveFileDialog();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_pause);
            this.panel1.Controls.Add(this.button_refresh);
            this.panel1.Controls.Add(this.textBox_port);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.button_download);
            this.panel1.Controls.Add(this.button_upload);
            this.panel1.Controls.Add(this.textBox_password);
            this.panel1.Controls.Add(this.textBox_user);
            this.panel1.Controls.Add(this.textBox_server);
            this.panel1.Controls.Add(this.button_connect);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // button_pause
            // 
            resources.ApplyResources(this.button_pause, "button_pause");
            this.button_pause.Name = "button_pause";
            this.button_pause.UseVisualStyleBackColor = true;
            this.button_pause.Click += new System.EventHandler(this.btn_pause_click);
            // 
            // button_refresh
            // 
            resources.ApplyResources(this.button_refresh, "button_refresh");
            this.button_refresh.Name = "button_refresh";
            this.button_refresh.UseVisualStyleBackColor = true;
            this.button_refresh.Click += new System.EventHandler(this.btn_refresh_click);
            // 
            // textBox_port
            // 
            resources.ApplyResources(this.textBox_port, "textBox_port");
            this.textBox_port.Name = "textBox_port";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // button_download
            // 
            resources.ApplyResources(this.button_download, "button_download");
            this.button_download.Name = "button_download";
            this.button_download.UseVisualStyleBackColor = true;
            this.button_download.Click += new System.EventHandler(this.btn_download_click);
            // 
            // button_upload
            // 
            resources.ApplyResources(this.button_upload, "button_upload");
            this.button_upload.Name = "button_upload";
            this.button_upload.UseVisualStyleBackColor = true;
            this.button_upload.Click += new System.EventHandler(this.btn_upload_click);
            // 
            // textBox_password
            // 
            resources.ApplyResources(this.textBox_password, "textBox_password");
            this.textBox_password.Name = "textBox_password";
            // 
            // textBox_user
            // 
            resources.ApplyResources(this.textBox_user, "textBox_user");
            this.textBox_user.Name = "textBox_user";
            // 
            // textBox_server
            // 
            resources.ApplyResources(this.textBox_server, "textBox_server");
            this.textBox_server.Name = "textBox_server";
            // 
            // button_connect
            // 
            this.button_connect.CausesValidation = false;
            resources.ApplyResources(this.button_connect, "button_connect");
            this.button_connect.Name = "button_connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.btn_connect_click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeLocal);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listBox_log);
            this.splitContainer1.Panel2.Controls.Add(this.listBox_Ftp_server);
            // 
            // treeLocal
            // 
            resources.ApplyResources(this.treeLocal, "treeLocal");
            this.treeLocal.ForeColor = System.Drawing.SystemColors.WindowText;
            this.treeLocal.ImageList = this.imageList1;
            this.treeLocal.Name = "treeLocal";
            this.treeLocal.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("treeLocal.Nodes")))});
            this.treeLocal.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeLocal_BeforeExpand);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "computer.gif");
            this.imageList1.Images.SetKeyName(1, "drive.gif");
            this.imageList1.Images.SetKeyName(2, "folder.ico");
            this.imageList1.Images.SetKeyName(3, "file.ico");
            // 
            // listBox_log
            // 
            this.listBox_log.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.listBox_log, "listBox_log");
            this.listBox_log.ForeColor = System.Drawing.Color.Black;
            this.listBox_log.FormattingEnabled = true;
            this.listBox_log.Name = "listBox_log";
            // 
            // listBox_Ftp_server
            // 
            this.listBox_Ftp_server.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.listBox_Ftp_server, "listBox_Ftp_server");
            this.listBox_Ftp_server.FormattingEnabled = true;
            this.listBox_Ftp_server.Name = "listBox_Ftp_server";
            this.listBox_Ftp_server.Sorted = true;
            this.listBox_Ftp_server.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_Ftp_server_DrawItem);
            this.listBox_Ftp_server.DoubleClick += new System.EventHandler(this.listBox_Ftp_server_DoubleClick);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "folder.ico");
            this.imageList2.Images.SetKeyName(1, "file.ico");
            this.imageList2.Images.SetKeyName(2, "file.ico");
            this.imageList2.Images.SetKeyName(3, "folder.ico");
            this.imageList2.Images.SetKeyName(4, "file.ico");
            this.imageList2.Images.SetKeyName(5, "file.ico");
            this.imageList2.Images.SetKeyName(6, "file.ico");
            this.imageList2.Images.SetKeyName(7, "file.ico");
            this.imageList2.Images.SetKeyName(8, "file.ico");
            this.imageList2.Images.SetKeyName(9, "file.ico");
            this.imageList2.Images.SetKeyName(10, "file.ico");
            this.imageList2.Images.SetKeyName(11, "file.ico");
            this.imageList2.Images.SetKeyName(12, "file.ico");
            // 
            // imageList3
            // 
            this.imageList3.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.imageList3, "imageList3");
            this.imageList3.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Mainform
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "Mainform";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_download;
        private System.Windows.Forms.Button button_upload;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.TextBox textBox_user;
        private System.Windows.Forms.TextBox textBox_server;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeLocal;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListBox listBox_Ftp_server;
        private System.Windows.Forms.ListBox listBox_log;
        private System.Windows.Forms.SaveFileDialog saveFileDialog_downloadto;
        private System.Windows.Forms.Button button_refresh;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ImageList imageList3;
        private System.Windows.Forms.Button button_pause;
    }
}

