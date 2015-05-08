using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace LogAn.UnitTests
{
    public class LogAnalyzerTests : IDisposable
    {
        private LogAnalyzer analyzer = null;
        readonly Xunit.Abstractions.ITestOutputHelper output;

        public LogAnalyzerTests(Xunit.Abstractions.ITestOutputHelper output)
        {
            // ITestOutputHelper�̓R���X�g���N�^������ݒ肷�邾���Ŏg����悤�ɂȂ�
            this.output = output;

            // Setup�̓R���X�g���N�^�ōs��
            analyzer = new LogAnalyzer();
            output.WriteLine("Setup");

            // v2�ł�Trace��Debug�͎g���Ȃ�
            System.Diagnostics.Trace.WriteLine("Trace Setup");
            System.Diagnostics.Debug.WriteLine("Debug Setup");
        }

        public void Dispose()
        {
            // TearDown��IDisposable.Dispose�ōs��
            output.WriteLine("TearDown");
        }



        // �����Ȃ��̎��̃e�X�g
        [Fact]
        [Trait("category", "fast test")]    // Category�̑�ֈ�
        public void IsValidFileName_BadExtension_ReturnsFalse()
        {
            // ���[�J���ϐ���Analyzer���g���ꍇ
            var localAnalyzer = new LogAnalyzer();

            bool result = localAnalyzer.IsValidLogFileName("filewithbadextension.foo");

            Assert.False(result);
        }

        // `IsValidLogFileName_ValidExtensions_ReturnsTrue`���\�b�h������̂ŁA
        // ���̃��\�b�h�̓X�L�b�v����
        [Fact(Skip = "Use `IsValidLogFileName_ValidExtensions_ReturnsTrue` method")]
        public void IsValidLogFileName_GoodExtensionLowercase_ReturnsTrue()
        {
            // �R���X�g���N�^�Őݒ肵��Analyzer���g���ꍇ
            bool result = analyzer.IsValidLogFileName("filewithgoodextension.slf");

            Assert.True(result);
        }

        [Fact(Skip = "Use `IsValidLogFileName_ValidExtensions_ReturnsTrue` method")]
        public void IsValidLogFileName_GoodExtensionUppercase_ReturnsTrue()
        {
            bool result = analyzer.IsValidLogFileName("filewithgoodextension.SLF");

            Assert.True(result);
        }

        // �f�[�^��^���������AFact�̂�����Theory���g��
        [Theory]
        [InlineData("filewithgoodextension.SLF")]
        [InlineData("filewithgoodextension.slf")]
        public void IsValidLogFileName_ValidExtensions_ReturnsTrue(string file)
        {
            bool result = analyzer.IsValidLogFileName(file);

            Assert.True(result);
        }

        // xUnit.net v2����APropertyData��MemberData�ւƕύX
        // �ÓI�v���p�e�B�̂ق��A�ÓI���\�b�h��ÓI�����o�ϐ������p�\
        [Theory]
        [MemberData("StaticPropertyTestData")]
        [MemberData("StaticMethodTestData")]
        public void IsValidLogFileName_ValidExtensions_ChecksThem(string file, bool expected)
        {
            bool result = analyzer.IsValidLogFileName(file);

            Assert.Equal(expected, result);
        }



        // MemberData�p�ÓI�v���p�e�B
        public static IEnumerable<object> StaticPropertyTestData
        {
            get
            {
                return new[] {
                    new object[] { "filewithgoodextension_property.SLF", true },
                    new object[] { "filewithgoodextension_property.slf", true },
                    new object[] { "filewithgoodextension_property.foo", false },
                };
            }
        }

        // MemberData�p�ÓI�����o
        public static IEnumerable<object> StaticMethodTestData()
        {
            return new[] {
                    new object[] { "filewithgoodextension_method.SLF", true },
                    new object[] { "filewithgoodextension_method.slf", true },
                    new object[] { "filewithgoodextension_method.foo", false },
                };
        }


        [Fact]
        public void IsValidFileName_EmptyFileName_ThrowsException()
        {
            // [ExceptedException]�̂����ɁAAssert.Throws���g��
            var exception = Assert.Throws<ArgumentException>(
                () => analyzer.IsValidLogFileName(string.Empty));

            Assert.Equal("filename has to be provided", exception.Message);

            
        }


        // �I�u�W�F�N�g�̏��(�v���p�e�B)���e�X�g
        [Fact]
        public void IsValidFileName_WhenCalled_ChnagesWasLastFileNameValid()
        {
            analyzer.IsValidLogFileName("badname.foo");

            Assert.False(analyzer.WasLastFileNameValid);
        }

        [Theory]
        [InlineData("badfile.foo", false)]
        [InlineData("goodfile.slf", true)]
        public void IsValidFileName_WhenCalled_ChangesWasLastFileNameValid(string file, bool expected)
        {
            analyzer.IsValidLogFileName(file);

            Assert.Equal(expected, analyzer.WasLastFileNameValid);
        }


        [Fact]
        public void IsValid_FileName_SupportedExtension_ReturnsTrue()
        {
            var myFakeManager = new FakeExtensionManager();
            myFakeManager.WillBeValid = true;

            var log = new LogAnalyzer(myFakeManager);

            var result = log.IsValidLogFileName("short.ext");
            Assert.True(result);
        }


        //---Injection�p�^�[��

        [Fact]
        public void IsValidFileName_ExtManagerThrowsException_ReturnsFalse()
        {
            var myFakeManager = new FakeExtensionManager();
            myFakeManager.WillThrow = new Exception("this is fake");

            // �R���X�g���N�^��Injection����
            var log = new LogAnalyzer(myFakeManager);

            var exception = Assert.Throws<Exception>(
                () => log.IsValidLogFileName("anything.anyextension"));

            Assert.Equal("this is fake", exception.Message);
        }

        [Fact]
        public void IsValidFileName_ExtManagerThrowsExceptionByProperty_ReturnsFalse()
        {
            var myFakeManager = new FakeExtensionManager();
            myFakeManager.WillThrow = new Exception("this is fake by property");

            // �v���p�e�B��Injection����
            var log = new LogAnalyzer();
            log.ExtensionManager = myFakeManager;

            var exception = Assert.Throws<Exception>(
                () => log.IsValidLogFileName("anything.anyextension"));

            Assert.Equal("this is fake by property", exception.Message);
        }

        public void IsValidFileName_ExtManagerThrowsExceptionByFactory_ReturnsFalse()
        {
            var myFakeManager = new FakeExtensionManager();
            myFakeManager.WillThrow = new Exception("this is fake by factory");

            // �t�@�N�g�����\�b�h��Injection����
            ExtensionManagerFactory.SetManager(myFakeManager);

            // �R���X�g���N�^�̒���Factory��Create���\�b�h���ĂԂ��ƂŁA
            // Injection���ꂽFactoryManager���g����
            var log = new LogAnalyzer();

            var exception = Assert.Throws<Exception>(
                () => log.IsValidLogFileName("anything.anyextension"));

            Assert.Equal("this is fake by property", exception.Message);
        }


        [Fact]
        public void OverrideTest()
        {
            var stub = new FakeExtensionManager();
            stub.WillBeValid = true;

            // �I�[�o�[���C�h�\�ȃ��\�b�h�����e�X�g�N���X�������
            // Injection����
            var logan = new TestableLogAnalyzer(stub);

            var result = logan.IsValidLogFileName("file.ext");

            Assert.True(result);
        }

        [Fact]
        public void OverrideTestWithoutStub()
        {
            // FakeExtensionManager�Ƃ����X�^�u��p�ӂ����ɁA
            // �I�[�o�[���C�h�\�ȃ��\�b�h�����e�X�g�N���X�������
            // Injection����
            // ->�X�^�u���Ȃ����A�V���v���ɂȂ�(�ʒuNo.1692)
            var logan = new TestableLogAnalyzer();
            logan.IsSupported = true;

            bool result = logan.IsValidLogFileNameByOverrideResult("file.ext");

            Assert.True(result);
        }

        class TestableLogAnalyzer : LogAnalyzerUsingFactoryMethod
        {
            public IExtensionManager Manager;
            public bool IsSupported;

            public TestableLogAnalyzer() { }

            public TestableLogAnalyzer(IExtensionManager mgr)
            {
                Manager = mgr;
            }

            protected override IExtensionManager GetManager()
            {
                return Manager;
            }

            protected override bool IsValid(string fileName)
            {
                return IsSupported;
            }
        }


        [Fact]
        public void Analyze_TooShortFileName_CallsWebService()
        {
            // Web�T�[�r�X�Ƃ̃e�X�g�ŁAmock���g�����@
            var mockWebService = new FakeWebService();
            var log = new LogAnalyzer(mockWebService);
            var tooShortFileName = "abc.ext";

            log.Analyze(tooShortFileName);

            Assert.Contains("FileName too short:abc.ext", mockWebService.LastError);
        }

        
        [Fact]
        public void Analyze_WebServiceThrows_SendMail()
        {
            // stub��mock��g�ݍ��킹��e�X�g
            var stubService = new FakeWebService();
            stubService.ToThrow = new Exception("fake exception");

            var mockEmail = new FakeEmailService();

            var log = new LogAnalyzer(stubService, mockEmail);
            var tooShortFileName = "abc.ext";

            log.Analyze(tooShortFileName);

            // �����̃v���p�e�B���e�X�g�ΏۂƂ���Ȃ�A
            // ��̃I�u�W�F�N�g�ɂ܂Ƃ߂��ق���������₷���ύX�ɂ������̂�
            // ��������́A
            //Assert.Contains("someone@somewhere.com", mockEmail.To);
            //Assert.Contains("fake exception", mockEmail.Body);
            //Assert.Contains("can't loga", mockEmail.Subject);

            // ������̂ق����ǂ�
            var expectEmail = new EmailInfo()
            {
                Body = "fake exception",
                To = "someone@somewhere.com",
                Subject = "can't log"
            };

            // xUnit.net�̏ꍇ�AEqual�̑�O������
            // IEqualityComparer������������r�p�N���X��n��
            Assert.Equal(expectEmail, mockEmail.email, new EmailInfoComparer());
        }
    }
}
