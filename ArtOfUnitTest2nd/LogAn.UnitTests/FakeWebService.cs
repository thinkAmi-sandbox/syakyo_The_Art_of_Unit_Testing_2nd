using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn.UnitTests
{
    public class FakeWebService : IWebService
    {
        public string LastError;
        public Exception ToThrow;

        public void LogError(string message)
        {
            if (ToThrow != null)
            {
                throw ToThrow;
            }

            // テストに使う内容は外部から引数として受け取り、
            // Mockの中には記載しない
            LastError = message;
        }


        // NSub用(mock向け)
        public string MessageToWebService;

        public void Write(string message)
        {
            MessageToWebService = message;
        }
    }
}
