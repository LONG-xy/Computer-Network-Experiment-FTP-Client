using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using FTP_client.Code;

namespace FTP_client.Code
{
    public class FtpOperation : IFtpOperation
    {
        /// <summary>命令Socket</summary>
        private Socket _cmdSocket;
        /// <summary>数据Socket</summary>
        private Socket _dataSocket;
        /// <summary>命令端口</summary>
        public static readonly int CmdPort = 21;
        /// <summary>数据端口</summary>
        private int _dataPort;
        /// <summary>服务器地址</summary>
        private string _serverIp;
        /// <summary>客户端命令端口返回的最后一条消息</summary>
        private string _lastMessage;
        #region public method
        #region Connect
        public string Connect(string serverIp)        //命令Socket连接FTP服务器
        {
            _cmdSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _cmdSocket.Connect(serverIp, CmdPort);
            string message = CmdReceive();    //解码
            if (FtpCode(message) == "220")         //连接FTP成功
            {
                this._serverIp = serverIp;
                return message;
            }
            else return "连接错误";
        }
        #endregion
        #region Identify 
        public string Identify(string user, string password)       //验证身份
        {
            string userCmd = "USER " + user + "\r\n";
            CmdSend(userCmd);
            string result = CmdReceive();
            if (FtpCode(result) != "331") return "验证失败";
            string passwordCmd = "PASS " + password + "\r\n";
            CmdSend(passwordCmd);
            result = CmdReceive();
            if (FtpCode(result) != "230") return "验证失败";
            return result;
        }
        #endregion
        #region List<Fileinfo> FtpFileList
        public List<FtpFileInfo> FtpFileList()
        {
            EnterPassiveMode();                                                 //进入被动模式
            string ListCmd = "LIST" + "\r\n";
            CmdSend(ListCmd);
            if (FtpCode(CmdReceive()) != "150") return null;               //状态码为150代表传输端口已经准备好

            ConnectDataSocket();
            byte[] buff = new byte[1 * 1024 * 1024];
            MemoryStream ms = new MemoryStream();                   //创建缓冲区来接受数据      
            while (true)
            {
                int length = _dataSocket.Receive(buff);
                if (length == 0) break;                         //传输完成
                ms.Write(buff, 0, length);
            }
            _dataSocket.Close();
            if (FtpCode(CmdReceive()) != "226") return null;   //状态码226代表传输完成
            var message = Encoding.Default.GetString(ms.ToArray());
            List<FtpFileInfo> List = new List<FtpFileInfo>();
            try
            {
                List = ListParseToFileInfo(message);
            }
            catch (Exception)
            {          
                return null;
            }
            return List;
        }
        #endregion
        #region AllClose
        public string AllClose()   //彻底断开连接，没什么用，因为要保持命令端口连接
        {
            _dataSocket.Close();
            string Close = "QUIT" + "\r\n";
            CmdSend(Close);
            string msg = CmdReceive();
            _cmdSocket.Close();
            return msg;
        }
        #endregion
        #region GetLocalFileSize
        public long GetLocalFileSize(string name)
        {
            if (!File.Exists(name)) return -1;//文件不存在返回-1
            FileInfo fileInfo = new FileInfo(name);
            return fileInfo.Length;
        }
        #endregion
        #region ChangeDirectory
        public void ChangeDirectory(string dir)
        {
            EnterPassiveMode();
            ConnectDataSocket();
            string cdCmd = "CWD " + "/" + dir + "\r\n";
            CmdSend(cdCmd);
        }
        #endregion
        #region DownloadFile
        public string DownloadFile(string name, string path = null, int breakPoint = 0)
        {
            if (!File.Exists(path))
            {
                EnterPassiveMode();
                ConnectDataSocket();
                string downCmd = "RETR " + name + "\r\n";
                CmdSend(downCmd);
                CmdReceive();
                using (FileStream running = new FileStream(path, FileMode.Create))
                {
                    int total = 0;
                    while (_dataSocket.Available > 0)
                    {
                        byte[] fbytes = new byte[1024];
                        int add = _dataSocket.Receive(fbytes);
                        if (total + add > breakPoint && breakPoint > 0)
                        {
                            running.Write(fbytes, 0, breakPoint - total);
                            _dataSocket.Close();
                            CmdReceive();
                            return "BreakPoint:" + breakPoint.ToString();
                        }
                        else
                        {
                            running.Write(fbytes, 0, add);
                            total += add;
                        }
                    }
                    _dataSocket.Close();
                    return CmdReceive();
                }
            }
            else//本地存在该文件则查看是否需要断点续传
            {
                long localFileSize = GetLocalFileSize(path);//获取本地文件大小
                long ftpFileSize = GetFtpFileSize(name);//获取服务器文件大小
                if (localFileSize < ftpFileSize)//若服务器文件比本地文件大则启动断点续传
                {
                    BreakPointDownLoadFile(name, path, (int)localFileSize);
                    _dataSocket.Close();
                    return CmdReceive();
                }
                else
                    return "已经下载完成";
            }
        }
        #endregion
        #region UpLoadFile
        public string UpLoadFile(string name, string path = null, int breakPoint = 0)
        {
            if (path == null) path = name;
            long ftpFileSize = GetLocalFileSize(name);
            if (ftpFileSize == -1)
            {
                EnterPassiveMode();
                ConnectDataSocket();
                string uplodeCMD = "STOR " + name + "\r\n";
                CmdSend(uplodeCMD);
                CmdReceive();
                using (FileStream running = new FileStream(path, FileMode.Open))
                {

                    int total = 0;
                    Debug.WriteLine($@"running's length is {running.Length}");
                    while (running.Position != running.Length)
                    {
                        Debug.WriteLine(running.Position);
                        byte[] buffer = new byte[1024];
                        int add = running.Read(buffer, 0, 1024);
                        if (total + add > breakPoint && breakPoint > 0)
                        {
                            byte[] endLine = new byte[breakPoint - total];
                            Array.Copy(buffer, 0, endLine, 0, breakPoint - total);
                            _dataSocket.Send(endLine);
                            _dataSocket.Close();//传输成功后关闭数据套接字
                            CmdReceive();
                            return "BreakPoint:" + breakPoint.ToString();
                        }
                        else
                        {
                            if (add == 1024)//当数组满直接发送
                                _dataSocket.Send(buffer);
                            else//数组不满则只发送前面一部分
                            {
                                byte[] endBytes = new byte[add];
                                Array.Copy(buffer, 0, endBytes, 0, add);
                                _dataSocket.Send(endBytes);
                                break;
                            }

                            total += add;
                        }
                    }
                }
                _dataSocket.Close();
                return CmdReceive();
            }
            else
            {
                if (ftpFileSize < GetLocalFileSize(path))
                {
                    BreakPointUploadFile(name, path, (int)ftpFileSize);
                    _dataSocket.Close();
                    //         Console.WriteLine("上传失败");

                    return CmdReceive();
                }
                else
                {
                    //         Console.WriteLine("完成上传");
                    return "已经上传完成";
                }
            }
        }
        #endregion
        #region BreakPointDownLoadFile
        public string BreakPointDownLoadFile(string name, string path, int breakPoint)
        {
            if (path == null) path = name;
            EnterPassiveMode();
            ConnectDataSocket();
            string breakPointCmd = "REST " + breakPoint.ToString() + "\r\n";
            CmdSend(breakPointCmd);
            CmdReceive();
            string downCmd = "RETR " + name + "\r\n";
            CmdSend(downCmd);
            CmdReceive();
            using (FileStream running = new FileStream(path, FileMode.Open))
            {
                byte[] fbytes = new byte[1024];
                running.Seek(breakPoint, SeekOrigin.Begin);
                int add = _dataSocket.Receive(fbytes);
                while (add != 0)
                {
                    running.Write(fbytes, 0, add);
                }
            }
            return "下载完成";
        }
        #endregion
        #region BreakPointUploadFile
        public string BreakPointUploadFile(string name, string path, int breakPoint)
        {
            if (path == null) path = name;
            EnterPassiveMode();
            ConnectDataSocket();
            string breakPointCmd = "REST " + breakPoint.ToString() + "\r\n";
            CmdSend(breakPointCmd);
            CmdReceive();
            string uploadCmd = "STOR " + name + "\r\n";
            CmdSend(uploadCmd);
            CmdReceive();
            using (FileStream running = new FileStream(path, FileMode.Open))
            {
                running.Seek(breakPoint, SeekOrigin.Begin);
                byte[] fbytes = new byte[1024];
                int sum = running.Read(fbytes, 0, 1024);
                while (sum != 0)
                {
                    if (sum == 1024) _dataSocket.Send(fbytes);
                    else
                    {
                        byte[] endLine = new byte[sum];
                        Array.Copy(fbytes, 0, endLine, 0, sum);
                        _dataSocket.Send(endLine);
                        break;
                    }
                }
            }
            return "上传完成";
        }
        #endregion
        #endregion
        #region private method
        #region GetFtpFileSize
        private long GetFtpFileSize(string fileName)
        {
            //获取文件大小的命令
            string sizeCmd = "SIZE " + fileName + "\r\n";
            CmdSend(sizeCmd);
            string message = CmdReceive();
            //获取服务器返回的响应码
            string respond = FtpCode(_lastMessage);
            if (respond == "550" || respond == "451")//响应码错误
            {
                CmdReceive();//吞入服务器报错信息
                return -1;
            }
            //响应码正确则分割输出
            string[] messageSplit = Regex.Split(message, " ");
            long fileSize = long.Parse(messageSplit[messageSplit.Length - 1]);
            return fileSize;
        }
        #endregion
        #region EnterPassiveMode
        private string EnterPassiveMode()
        {
            string PassCmd = "PASV" + "\r\n";
            CmdSend(PassCmd);
            string message = CmdReceive();
            try
            {
                // 解析被动模式下服务器数据端口 (127,0,0,1,74,93) 74*256+93
                int le = message.LastIndexOf("(");
                int re = message.LastIndexOf(")");
                string portMessage = message.Substring(le + 1, re - le - 1);
                string[] data = portMessage.Split(',');
                this._dataPort = int.Parse(data[data.Length - 2]) * 256 + int.Parse(data[data.Length - 1]);       //将计算的端口值赋给DataPort
            }
            catch (Exception e)
            {
                return "解析端口失败，进入被动模式失败";
            }
            return message;
        }
        #endregion
        #region ConnectDataSocket
        private void ConnectDataSocket()
        {
            _dataSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _dataSocket.Connect(this._serverIp, _dataPort); //ServerIP与命令端口的IP相同，DataPort在进入被动模式时已经设好
        }
        #endregion
        #region CmdSend
        private void CmdSend(string cmd)       //命令端口发出命令
        {
            _cmdSocket.Send(Encoding.UTF8.GetBytes(cmd));
        }
        #endregion
        #region CmdReceive
        private string CmdReceive()          //命令端口接受消息
        {
            byte[] result = new byte[1024];
            int recieveLength = _cmdSocket.Receive(result);
            _lastMessage = Encoding.UTF8.GetString(result, 0, recieveLength);
            return _lastMessage;
        }
        #endregion
        #region FtpCode
        private string FtpCode(string result)          //取FTP返回状态码
        {
            return result.Substring(0, 3);
        }
        #endregion
        #region List<Fileinfo> ListParseToFileInfo
        private List<FtpFileInfo> ListParseToFileInfo(string fileList)
        {
            string[] files = fileList.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string pattern = @"^(?<dir>[\-ld])(?<permission>([\-r][\-w][\-xs]){3})\s+(?<filecode>\d+)\s+(?<owner>\S+)\s+(?<group>\S+)\s+(?<size>\d+)\s+(?<timestamp>((?<month>\w{3})\s+(?<day>\d{1,2})\s+(?<hour>\d{1,2}):(?<minute>\d{2}))|((?<month>\w{3})\s+(?<day>\d{1,2})\s+(?<year>\d{4})))\s+(?<name>.+)$";
            Regex regexForUnix = new Regex(pattern, RegexOptions.Compiled);
            List<FtpFileInfo> list = new List<FtpFileInfo>();
            foreach (var file in files)
            {
                FtpFileInfo remoteFile = new FtpFileInfo
                {
                    Size = "",
                    ModifiedTime = "",
                    IsDirectory = false
                };
                string[] groups = file.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (groups.Length < 4) return null;
                try
                {
                    remoteFile.ModifiedTime = groups[0] + " " + groups[1];
                    if (groups[2] == "<DIR>") remoteFile.IsDirectory = true;
                    else remoteFile.Size = SizeParseToString(long.Parse(groups[2]));
                    remoteFile.Name = groups[3];
                    for (int i = 4; i < groups.Length; i++) remoteFile.Name += " " + groups[i];
                }
                catch (Exception)
                {
                    return null;
                }
                list.Add(remoteFile);
            }
            return list;
        }
        #endregion
        #region SizeParseToString
        private string SizeParseToString(long size)
        {
            if (size < 1024) return size.ToString() + " Bytes";
            else if (size >= 1024 && size < 1024 * 1024) return (size / 1024).ToString() + " KB";
            else if (size >= 1024 * 1024 && size < 1024 * 1024 * 1024) return (size / 1024 / 1024).ToString() + " MB";
            else return (size / 1024 / 1024 / 1024).ToString() + " GB";
        }
        #endregion
        #endregion
    }

}