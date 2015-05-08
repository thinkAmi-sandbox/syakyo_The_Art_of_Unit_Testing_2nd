using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn
{
    // internalなコンストラクタをテストから見えるようにするため、
    // 属性を付与する
    using System.Runtime.CompilerServices;
    [assembly: InternalsVisibleTo("LogAn.UnitTests")]
    public class LogAnalyzer
    {
        public bool WasLastFileNameValid { get; set; }

        private IExtensionManager manager;


        // ファクトリメソッドを使って、IExtensionManagerのインスタンスを生成
        public LogAnalyzer()
        {
            manager = ExtensionManagerFactory.Create();
        }

        // テストコードから呼ぶことのできる(Injectionする)コンストラクタを作成
        public LogAnalyzer(IExtensionManager mgr)
        {
            manager = mgr;
        }

        // 通常は外部から見えないが、上記のとおりに
        // [InternalsVisibleTo()]を使うことで、
        // テストクラスからも見えるようになる
        // ＊ビルドできるよう、便宜上、引数の型を変えている
        internal LogAnalyzer(FileExtensionManager extensionManager)
        {
            manager = extensionManager;
        }


        // テストコードからInjectionすることのできるプロパティを作成
        public IExtensionManager ExtensionManager
        {
            get { return manager; }
            set { manager = value; }
        }
        

        public bool IsValidLogFileName(string fileName)
        {
            WasLastFileNameValid = false;

            WasLastFileNameValid = manager.IsValid(fileName);

            return WasLastFileNameValid;
        }


        //------- Webサービスとのテスト向け
        public IWebService Service { get; set; }
        public IEmailService Email { get; set; }

        public LogAnalyzer(IWebService service)
        {
            // Webサービス単体
            Service = service;
        }

        public LogAnalyzer(IWebService service, IEmailService email)
        {
            // WebサービスとEmailサービスとの連携
            Service = service;
            Email = email;
        }

        public void Analyze(string fileName)
        {
            // Webサービスとの連携のテスト
            if (fileName.Length < 8)
            {
                try
                {
                    Service.LogError("FileName too short:" + fileName);
                }
                catch (Exception e)
                {
                    Email.SendEmail("someone@somewhere.com", "can't log", e.Message);
                }
            }
        }


        //-------NSubstituteを使ったテスト向け
        private ILogger _logger;

        public LogAnalyzer(ILogger logger)
        {
            _logger = logger;
        }

        public int MinNameLength { get; set; }

        public void AnalyzeWhenUsingMessageString(string fileName)
        {
            if (fileName.Length < MinNameLength)
            {
                try
                {
                    _logger.LogError(string.Format("Filename too short: {0}", fileName));
                }
                catch (Exception e)
                {
                    _webService.Write("Error From Logger: " + e);
                }
            }
        }

        public void AnalyzeWhenUsingErrorInfoObject(string fileName)
        {
            if (fileName.Length < MinNameLength)
            {
                try
                {
                    _logger.LogError(string.Format("Filename too short: {0}", fileName));
                }
                catch (Exception e)
                {
                    _webServiceErrorInfo.Write(new ErrorInfo(1000, e.Message));
                }
            }
        }


        //-------NSubstituteを使って複数のfakeを使ったテスト向け
        private IWebService _webService;

        public LogAnalyzer(ILogger logger, IWebService webService)
        {
            _logger = logger;
            _webService = webService;
        }

        //------NSubstitute + ErrorInfoを使って複数のfakeを使ったテスト向け
        private IWebServiceUsingErrorInfo _webServiceErrorInfo;

        public LogAnalyzer(ILogger logger, IWebServiceUsingErrorInfo webService)
        {
            _logger = logger;
            _webServiceErrorInfo = webService;
        }
    }
}
