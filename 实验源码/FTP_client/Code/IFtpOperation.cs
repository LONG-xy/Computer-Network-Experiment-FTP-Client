using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTP_client.Code
{
    public interface IFtpOperation
    {
        string Connect(string serverIp);
        string Identify(string user, string password);
        List<FtpFileInfo> FtpFileList();
        string AllClose();
        long  GetLocalFileSize(string name);
        string DownloadFile(string name, string path = null, int breakPoint = 0);
        string UpLoadFile(string name, string path = null, int breakPoint = 0);
        string BreakPointDownLoadFile(string name, string path, int breakPoint);
        string BreakPointUploadFile(string name, string path, int breakPoint);
        void ChangeDirectory(string dir);
    }
}
