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

        // ---コンストラクタ系

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
    }
}
