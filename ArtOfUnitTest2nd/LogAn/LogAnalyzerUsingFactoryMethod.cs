using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn
{
    public class LogAnalyzerUsingFactoryMethod
    {
        public bool IsValidLogFileName(string fileName)
        {
            return GetManager().IsValid(fileName);
        }

        public bool IsValidLogFileNameByOverrideResult(string fileName)
        {
            return this.IsValid(fileName);
        }

        protected virtual bool IsValid(string fileName)
        {
            var mgr = new FileExtensionManager();
            return mgr.IsValid(fileName);
        }

        // protected virtualにすることで、
        // テストにて継承・オーバーライドの利用を可能にする
        protected virtual IExtensionManager GetManager()
        {
            // ハードコードした値を返す
            return new FileExtensionManager();
        }
    }
}
