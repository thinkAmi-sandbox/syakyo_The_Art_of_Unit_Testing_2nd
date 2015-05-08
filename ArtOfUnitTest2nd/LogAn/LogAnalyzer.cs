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


        //------- Web�T�[�r�X�Ƃ̃e�X�g����
        public IWebService Service { get; set; }
        public IEmailService Email { get; set; }

        public LogAnalyzer(IWebService service)
        {
            // Web�T�[�r�X�P��
            Service = service;
        }

        public LogAnalyzer(IWebService service, IEmailService email)
        {
            // Web�T�[�r�X��Email�T�[�r�X�Ƃ̘A�g
            Service = service;
            Email = email;
        }

        public void Analyze(string fileName)
        {
            // NSubstitute�����̃e�X�g
            if (fileName.Length < MinNameLength)
            {
                _logger.LogError(string.Format("Filename too short: {0}", fileName));
                return;
            }


            // Web�T�[�r�X�Ƃ̘A�g�̃e�X�g
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


        //-------NSubstitute���g�����e�X�g����
        private ILogger _logger;

        public LogAnalyzer(ILogger logger)
        {
            _logger = logger;
        }

        public int MinNameLength { get; set; }
    }
}
