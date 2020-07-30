using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTP_client.Code
{
    public class FtpFileInfo
    {
        public bool IsDirectory { get; set; }        //是否是文件夹
        public string Name { get; set; }             //文件名
        public string Size { get; set; }             //文件大小
        public string ModifiedTime { get; set; }     //最近修改时间
        public override string ToString()
        {
            return Name + "          " + Size + "      " + ModifiedTime;
        }
    }
}
