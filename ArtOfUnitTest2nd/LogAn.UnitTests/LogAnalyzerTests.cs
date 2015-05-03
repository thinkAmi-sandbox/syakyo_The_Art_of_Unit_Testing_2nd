using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace LogAn.UnitTests
{
    public class LogAnalyzerTests
    {
        // 引数なしの時のテスト
        [Fact]
        public void IsValidFileName_BadExtension_ReturnsFalse()
        {
            var analyzer = new LogAnalyzer();

            bool result = analyzer.IsValidLogFileName("filewithbadextension.foo");

            Assert.False(result);
        }

        // `IsValidLogFileName_ValidExtensions_ReturnsTrue`メソッドがあるので、
        // このメソッドはスキップする
        [Fact(Skip = "Use `IsValidLogFileName_ValidExtensions_ReturnsTrue` method")]
        public void IsValidLogFileName_GoodExtensionLowercase_ReturnsTrue()
        {
            var analyzer = new LogAnalyzer();
            bool result = analyzer.IsValidLogFileName("filewithgoodextension.slf");

            Assert.True(result);
        }

        [Fact(Skip = "Use `IsValidLogFileName_ValidExtensions_ReturnsTrue` method")]
        public void IsValidLogFileName_GoodExtensionUppercase_ReturnsTrue()
        {
            var analyzer = new LogAnalyzer();
            bool result = analyzer.IsValidLogFileName("filewithgoodextension.SLF");

            Assert.True(result);
        }

        // データを与えたい時、FactのかわりにTheoryを使う
        [Theory]
        [InlineData("filewithgoodextension.SLF")]
        [InlineData("filewithgoodextension.slf")]
        public void IsValidLogFileName_ValidExtensions_ReturnsTrue(string file)
        {
            var analyzer = new LogAnalyzer();

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
            var analyzer = new LogAnalyzer();

            bool result = analyzer.IsValidLogFileName(file);

            Assert.Equal(expected, result);
        }

        // 静的プロパティ版
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

        // 静的メンバ版
        public static IEnumerable<object> StaticMethodTestData()
        {
            return new[] {
                    new object[] { "filewithgoodextension_method.SLF", true },
                    new object[] { "filewithgoodextension_method.slf", true },
                    new object[] { "filewithgoodextension_method.foo", false },
                };
        }

    }
}
