using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn
{
    public class ExtensionManagerFactory
    {
        // 位置No1581ではstaticになっていなかったが、
        // 本文に合わせていずれもstaticで作った
        private static IExtensionManager customManager = null;

        /// <summary>
        /// ファクトリメソッド
        /// </summary>
        /// <returns></returns>
        public static IExtensionManager Create()
        {
            if (customManager != null)
            {
                return customManager;
            }

            return new FileExtensionManager();
        }

        
        /// <summary>
        /// ファクトリメソッドで使うインスタンスをセットする
        /// </summary>
        /// <param name="mgr"></param>
        public static void SetManager(IExtensionManager mgr)
        {
            customManager = mgr;
        }
    }
}
