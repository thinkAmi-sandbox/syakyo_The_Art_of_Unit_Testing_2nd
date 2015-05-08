using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn.UnitTests
{
    // NSub用(stub向け)
    public class FakeLogger2 : ILogger
    {
        public Exception WillThrow = null;
        public string LoggerGetMessage = null;
        
        public void LogError(string message)
        {
            LoggerGetMessage = message;
            if (WillThrow != null)
            {
                throw WillThrow;
            }
        }
    }
}
