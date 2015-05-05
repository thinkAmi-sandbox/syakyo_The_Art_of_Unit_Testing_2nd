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

        private IExtensionManager manager;

        // 過去との互換性のため、引数なしのコンストラクタも用意
        public LogAnalyzer()
        {
            manager = new FileExtensionManager();
        }

        // テストコードから呼ぶことのできる(Injectionする)コンストラクタを作成
        public LogAnalyzer(IExtensionManager mgr)
        {
            manager = mgr;
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
    }
}
