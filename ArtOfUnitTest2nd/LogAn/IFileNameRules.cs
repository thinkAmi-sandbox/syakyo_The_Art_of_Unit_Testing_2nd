using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn
{
    public interface IFileNameRules
    {
        bool IsValidLogFileName(string fileName);
    }
}
