using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn
{
    // internal�ȃR���X�g���N�^���e�X�g���猩����悤�ɂ��邽�߁A
    // ������t�^����
    using System.Runtime.CompilerServices;
    [assembly: InternalsVisibleTo("LogAn.UnitTests")]
    public class LogAnalyzer
    {
        public bool WasLastFileNameValid { get; set; }

        private IExtensionManager manager;

        // ---�R���X�g���N�^�n

        // �t�@�N�g�����\�b�h���g���āAIExtensionManager�̃C���X�^���X�𐶐�
        public LogAnalyzer()
        {
            manager = ExtensionManagerFactory.Create();
        }

        // �e�X�g�R�[�h����ĂԂ��Ƃ̂ł���(Injection����)�R���X�g���N�^���쐬
        public LogAnalyzer(IExtensionManager mgr)
        {
            manager = mgr;
        }

        // �ʏ�͊O�����猩���Ȃ����A��L�̂Ƃ����
        // [InternalsVisibleTo()]���g�����ƂŁA
        // �e�X�g�N���X�����������悤�ɂȂ�
        // ���r���h�ł���悤�A�֋X��A�����̌^��ς��Ă���
        internal LogAnalyzer(FileExtensionManager extensionManager)
        {
            manager = extensionManager;
        }


        // �e�X�g�R�[�h����Injection���邱�Ƃ̂ł���v���p�e�B���쐬
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
