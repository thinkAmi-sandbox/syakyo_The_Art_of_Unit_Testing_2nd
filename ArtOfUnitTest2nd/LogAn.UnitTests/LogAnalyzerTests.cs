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
            // ITestOutputHelperはコンストラクタ引数を設定するだけで使えるようになる
            this.output = output;

            // Setupはコンストラクタで行う
            analyzer = new LogAnalyzer();
            output.WriteLine("Setup");

            // v2ではTraceやDebugは使えない
            System.Diagnostics.Trace.WriteLine("Trace Setup");
            System.Diagnostics.Debug.WriteLine("Debug Setup");
        }

        public void Dispose()
        {
            // TearDownはIDisposable.Disposeで行う
            output.WriteLine("TearDown");
        }



        // 引数なしの時のテスト
        [Fact]
        [Trait("category", "fast test")]    // Categoryの代替案
        public void IsValidFileName_BadExtension_ReturnsFalse()
        {
            // ローカル変数のAnalyzerを使う場合
            var localAnalyzer = new LogAnalyzer();

            bool result = localAnalyzer.IsValidLogFileName("filewithbadextension.foo");

            Assert.False(result);
        }

        // `IsValidLogFileName_ValidExtensions_ReturnsTrue`メソッドがあるので、
        // このメソッドはスキップする
        [Fact(Skip = "Use `IsValidLogFileName_ValidExtensions_ReturnsTrue` method")]
        public void IsValidLogFileName_GoodExtensionLowercase_ReturnsTrue()
        {
            // コンストラクタで設定したAnalyzerを使う場合
            bool result = analyzer.IsValidLogFileName("filewithgoodextension.slf");

            Assert.True(result);
        }

        [Fact(Skip = "Use `IsValidLogFileName_ValidExtensions_ReturnsTrue` method")]
        public void IsValidLogFileName_GoodExtensionUppercase_ReturnsTrue()
        {
            bool result = analyzer.IsValidLogFileName("filewithgoodextension.SLF");

            Assert.True(result);
        }

        // データを与えたい時、FactのかわりにTheoryを使う
        [Theory]
        [InlineData("filewithgoodextension.SLF")]
        [InlineData("filewithgoodextension.slf")]
        public void IsValidLogFileName_ValidExtensions_ReturnsTrue(string file)
        {
            bool result = analyzer.IsValidLogFileName(file);

            Assert.True(result);
        }

        // xUnit.net v2から、PropertyDataがMemberDataへと変更
        // 静的プロパティのほか、静的メソッドや静的メンバ変数も利用可能
        [Theory]
        [MemberData("StaticPropertyTestData")]
        [MemberData("StaticMethodTestData")]
        public void IsValidLogFileName_ValidExtensions_ChecksThem(string file, bool expected)
        {
            bool result = analyzer.IsValidLogFileName(file);

            Assert.Equal(expected, result);
        }



        // MemberData用静的プロパティ
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

        // MemberData用静的メンバ
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
            // [ExceptedException]のかわりに、Assert.Throwsを使う
            var exception = Assert.Throws<ArgumentException>(
                () => analyzer.IsValidLogFileName(string.Empty));

            Assert.Equal("filename has to be provided", exception.Message);

            
        }


        // オブジェクトの状態(プロパティ)をテスト
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
    }
}
