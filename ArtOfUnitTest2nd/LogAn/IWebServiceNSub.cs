using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn
{
    public interface IWebServiceUsingErrorInfo : IWebService
    {
        // テスト`Analyze_LoggerThrows_CallsWebServiceWithNSubObject`と落とすため、
        // IWebServiceを継承したインタフェースを作成
        void Write(ErrorInfo message);
    }
}
