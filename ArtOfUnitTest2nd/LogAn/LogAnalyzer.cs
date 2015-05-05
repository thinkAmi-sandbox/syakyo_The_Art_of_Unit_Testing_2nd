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
