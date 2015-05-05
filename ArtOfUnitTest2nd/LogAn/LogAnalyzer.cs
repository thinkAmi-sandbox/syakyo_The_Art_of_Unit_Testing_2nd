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

        // �ߋ��Ƃ̌݊����̂��߁A�����Ȃ��̃R���X�g���N�^���p��
        public LogAnalyzer()
        {
            manager = new FileExtensionManager();
        }

        // �e�X�g�R�[�h����ĂԂ��Ƃ̂ł���R���X�g���N�^���쐬
        public LogAnalyzer(IExtensionManager mgr)
        {
            manager = mgr;
        }

        public bool IsValidLogFileName(string fileName)
        {
            WasLastFileNameValid = false;

            WasLastFileNameValid = manager.IsValid(fileName);

            return WasLastFileNameValid;
        }
    }
}
