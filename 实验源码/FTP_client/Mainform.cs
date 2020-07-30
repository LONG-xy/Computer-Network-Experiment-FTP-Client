using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using FTP_client.Code;
using Microsoft.VisualBasic;

namespace FTP_client
{
    public partial class Mainform : Form
    {
        public Mainform()
        {
            InitializeComponent();
            listBox_Ftp_server.DrawItem += listBox_Ftp_server_DrawItem;
        }
        FtpOperation _ftpc = new FtpOperation();

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitTreeView();
        }
        #region InitTreeView 初始化本地磁盘信息
        private void InitTreeView()
        {
            string[] drives = Environment.GetLogicalDrives();
            int i = 0;
            foreach (string drive in drives)
            {
                DriveInfo d = new DriveInfo(drive);
                if ((d.DriveType & DriveType.Fixed) == DriveType.Fixed)
                {
                    string drive1 = drive.Substring(0, drive.Length - 1);
                    this.treeLocal.Nodes[0].Nodes.Add(drive1);
                    this.treeLocal.Nodes[0].Nodes[i].ImageIndex = 1;
                    this.treeLocal.Nodes[0].Nodes[i].SelectedImageIndex = 1;
                    this.treeLocal.Nodes[0].Nodes[i].Tag = drive1;
                    this.treeLocal.Nodes[0].Nodes[i].Nodes.Add("");
                    i++;
                }
            }
        }
        #endregion
        #region treeLocal_BeforeExpand 各磁盘下文件的展示
        private void treeLocal_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Level > 0)
            {
                //点击之前，先填充节点：
                string path = e.Node.FullPath.Substring(e.Node.FullPath.IndexOf("\\") + 1) + "\\";
                e.Node.Nodes.Clear();
                string[] files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
                int i = 0;
                foreach (string file in files)
                {
                    FileInfo f = new FileInfo(file);
                    if ((f.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden && (f.Attributes & FileAttributes.System) != FileAttributes.System)
                    {
                        e.Node.Nodes.Add(Path.GetFileName(file));
                        e.Node.Nodes[i].ImageIndex = 3;
                        e.Node.Nodes[i].SelectedImageIndex = 3;
                        e.Node.Nodes[i].Tag = file;
                        i++;
                    }
                }
                string[] dirs = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
                foreach (string dir in dirs)
                {
                    DirectoryInfo d = new DirectoryInfo(dir);
                    if ((d.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden && (d.Attributes & FileAttributes.System) != FileAttributes.System)
                    {
                        e.Node.Nodes.Add(Path.GetFileName(dir));
                        e.Node.Nodes[i].ImageIndex = 2;
                        e.Node.Nodes[i].SelectedImageIndex = 2;
                        e.Node.Nodes[i].Tag = dir;
                        e.Node.Nodes[i].Nodes.Add("");
                        i++;
                    }
                }
            }
        }
        #endregion
        #region btn_connect_click 连接事件
        private void btn_connect_click(object sender, EventArgs e)
        {
            try
            {
                if (button_connect.Text == "  退出")
                {
                    _ftpc.AllClose();
                    listBox_Ftp_server.Items.Clear();
                    listBox_log.Items.Clear();
                    listBox_log.Items.Add("已退出连接");
                }
                else
                {
                    _ftpc.Connect(textBox_server.Text);
                    _ftpc.Identify(textBox_user.Text, textBox_password.Text);
                    listBox_log.Items.Add("已连接");
                    List<FtpFileInfo> list = _ftpc.FtpFileList();
                    if (list == null)
                        Console.WriteLine("error");
                    foreach (FtpFileInfo file in list)
                    {
                        listBox_Ftp_server.DrawMode = DrawMode.OwnerDrawFixed;
                        AddFileItems(list);
                    }
                    button_connect.Text = "  退出";
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region btn_upload_click 上传事件
        private void btn_upload_click(object sender, EventArgs e)
        {
           
            try 
            {
               _ftpc.UpLoadFile(treeLocal.SelectedNode.Text,
                treeLocal.SelectedNode.FullPath.Substring(treeLocal.SelectedNode.FullPath.IndexOf("\\") + 1), 0);
                listBox_log.Items.Add("上传完成：" + treeLocal.SelectedNode.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region GetFileName 获取文件名
        private string GetFileName(string raw)
        {
            var strings = Regex.Split(raw, "(\\d)+\\sKB");
            return strings[0].Trim();
        }
        #endregion
        #region btn_download_click 下载事件
        private void btn_download_click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                var localPath = fbd.SelectedPath;
                listBox_log.Items.Add("选择的下载目录：" + localPath);
                FtpFileInfo ftpFileInfo = (FtpFileInfo)listBox_Ftp_server.Items[listBox_Ftp_server.SelectedIndex];
                string fileName = ftpFileInfo.Name;
                _ftpc.DownloadFile(fileName, localPath + "\\" + fileName, 0);
                listBox_log.Items.Add("下载完成" + fileName);

            }
        }
        #endregion
        #region btn_refresh_click 刷新服务器端
        private void btn_refresh_click(object sender, EventArgs e)
        {
            List<FtpFileInfo> list = _ftpc.FtpFileList();
            AddFileItems(list);
            listBox_log.Items.Add("已刷新服务器文件列表");
        }
        #endregion
        #region listBox_Ftp_server_DrawItem 服务器列表图标绘制
        private void listBox_Ftp_server_DrawItem(object sender, DrawItemEventArgs e)
        {
            Brush myBrush = Brushes.Black;
            Color RowBackColorSel = Color.FromArgb(150, 200, 250);//选择项目颜色
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                myBrush = new SolidBrush(RowBackColorSel);
            }
            else
            {
                myBrush = new SolidBrush(Color.White);
            }
            e.Graphics.FillRectangle(myBrush, e.Bounds);
            e.DrawFocusRectangle();//焦点框

            //绘制图标
            Image image = imageList2.Images[e.Index];           
            Graphics graphics = e.Graphics;
            Rectangle bound = e.Bounds;
            Rectangle imgRec = new Rectangle(
                bound.X,
                bound.Y,
                bound.Height,
                bound.Height);
            Rectangle textRec = new Rectangle(
                imgRec.Right,
                bound.Y,
                bound.Width - imgRec.Right,
                bound.Height);
            if (image != null)
            {
                e.Graphics.DrawImage(
                    image,
                    imgRec,
                    0,
                    0,
                    image.Width,
                    image.Height,
                    GraphicsUnit.Pixel);
                //绘制字体
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                e.Graphics.DrawString(listBox_Ftp_server.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.Black), textRec, stringFormat);
            }
        }
        #endregion
        #region AddFileItems 服务器列表添加
        private void AddFileItems(List<FtpFileInfo> list)
        {
            listBox_Ftp_server.Items.Clear();
            listBox_Ftp_server.Items.Add(new FtpFileInfo
            {
                IsDirectory = true,
                ModifiedTime = "",
                Name = "..",
                Size = ""
            });

            listBox_Ftp_server.Items.AddRange(list.ToArray());
        }
        #endregion
        #region btn_pause_click 断点、续传事件
        private void btn_pause_click(object sender, EventArgs e)
        {
         try 
         {
;           string breakpoint;
                if (treeLocal.SelectedNode.Text != null)//上传
                {
                    if (button_pause.Text == "  续传")
                    {
                        _ftpc.BreakPointUploadFile(treeLocal.SelectedNode.Text,
               treeLocal.SelectedNode.FullPath.Substring(treeLocal.SelectedNode.FullPath.IndexOf("\\") + 1), 0);
                        listBox_log.Items.Add(treeLocal.SelectedNode.Text + "上传完成");
                    }
                    else
                    {
                        breakpoint = Microsoft.VisualBasic.Interaction.InputBox("输入断点值", "断点续传", "", -1, -1);
                        listBox_log.Items.Add(treeLocal.SelectedNode.Text + "上传中断");
                        listBox_log.Items.Add("断点：" + breakpoint + "    Bytes");
                        int breakpointint = int.Parse(breakpoint);
                        _ftpc.UpLoadFile(treeLocal.SelectedNode.Text,
                        treeLocal.SelectedNode.FullPath.Substring(treeLocal.SelectedNode.FullPath.IndexOf("\\") + 1), breakpointint);
                        button_pause.Text = "  续传";
                        button_pause.Image = Image.FromFile(@"C: \Users\xy\source\repos\FTP_client\Image\player_play_pause_16px_1222646_easyicon.net.ico");
                    }
                }
                else//下载
                {
                    if (button_pause.Text == "  续传")
                    {
                        FolderBrowserDialog fbd = new FolderBrowserDialog();
                        if (fbd.ShowDialog() == DialogResult.OK)
                        {
                            var localPath = fbd.SelectedPath;
                            listBox_log.Items.Add("选择的下载目录：" + localPath);
                            FtpFileInfo ftpFileInfo = (FtpFileInfo)listBox_Ftp_server.Items[listBox_Ftp_server.SelectedIndex];
                            string fileName = ftpFileInfo.Name;
                            _ftpc.DownloadFile(fileName, localPath + "\\" + fileName, 0);
                            listBox_log.Items.Add("下载完成" + fileName);
                        }
                    }
                    else
                    {
                        breakpoint = Microsoft.VisualBasic.Interaction.InputBox("输入断点值", "断点续传", "", -1, -1);
                        listBox_log.Items.Add(treeLocal.SelectedNode.Text + "上传中断");
                        listBox_log.Items.Add("断点：" + breakpoint + "    Bytes");
                        int breakpointint = int.Parse(breakpoint);
                        FolderBrowserDialog fbd = new FolderBrowserDialog();
                        if (fbd.ShowDialog() == DialogResult.OK)
                        {
                            var localPath = fbd.SelectedPath;
                            listBox_log.Items.Add("选择的下载目录：" + localPath);
                            FtpFileInfo ftpFileInfo = (FtpFileInfo)listBox_Ftp_server.Items[listBox_Ftp_server.SelectedIndex];
                            string fileName = ftpFileInfo.Name;
                            _ftpc.DownloadFile(fileName, localPath + "\\" + fileName, breakpointint);
                            listBox_log.Items.Add("下载完成" + fileName);
                            button_pause.Text = "  续传";
                        button_pause.Image = Image.FromFile(@"C: \Users\xy\source\repos\FTP_client\Image\player_play_pause_16px_1222646_easyicon.net.ico");
                    }
                }
         }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region listBox_Ftp_server_DoubleClick 双击进入服务器端文件夹
        private void listBox_Ftp_server_DoubleClick(object sender, EventArgs e)
        {
            var listBoxFtpSever = (ListBox)sender;
            FtpFileInfo selectedItem = listBoxFtpSever.SelectedItem as FtpFileInfo;
            bool isDirectory = selectedItem.IsDirectory;
            //if it is a directory
            Debug.WriteLine(selectedItem.IsDirectory);

            if (isDirectory)
            {
                _ftpc.ChangeDirectory(selectedItem.Name);
            }

            List<FtpFileInfo> list = _ftpc.FtpFileList();
            AddFileItems(list);
        }
        #endregion
    }
}
