using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn
{
    public class LogAnalyzer
    {
        public bool WasLastFileNameValid { get; set; }

        public bool IsValidLogFileName(string fileName)
        {
            WasLastFileNameValid = false;

            IExtensionManager mgr = new FileExtensionManager();
            WasLastFileNameValid = mgr.IsValid(fileName);

            return WasLastFileNameValid;
        }
    }
}
