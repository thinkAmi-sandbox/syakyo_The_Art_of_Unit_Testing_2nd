using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn
{
    public class FakeWebService : IWebService
    {
        public string LastError;

        public void LogError(string message)
        {
            // テストに使う内容は外部から引数として受け取り、
            // Mockの中には記載しない
            LastError = message;
        }
    }
}
