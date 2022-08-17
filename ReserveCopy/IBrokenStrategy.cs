using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReserveCopy
{
    public interface IBrokenStrategy
    {
        void Action(string target, string[] source, NLog.Logger logger);

        bool CreateFolder(string target, string[] source, NLog.Logger logger, string folderFromSource);

        bool CopyFile(string target, string[] source, NLog.Logger logger, string folderFromSource);
    }
}
