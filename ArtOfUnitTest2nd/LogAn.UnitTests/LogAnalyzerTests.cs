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
    }
}
