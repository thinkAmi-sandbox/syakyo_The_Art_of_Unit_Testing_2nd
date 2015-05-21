using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn.UnitTests.Chap7
{
    using Xunit;
    using NSubstitute;
    using LogAn.Chap7;
    
    //---静的リソースを複数のテストケースで共有しているパターン
    public class LogAnalyzerTestsBefore : IDisposable
    {
        public void Dispose()
        {
            // テストごとに静的リソースはクリアする必要がある
            LoggingFacility.Logger = null;
        }

        [Fact]
        public void Analyze_EmptyFile_ThrowsException()
        {
            LoggingFacility.Logger = Substitute.For<ILoggerChap7>();
            var la = new LogAnalyzerChap7();
            la.Analyze("myemptyfile.txt");
            // 残りのテストを実装
        }
    }

    public class ConfigurationManagerTestsBefore : IDisposable
    {
        public void Dispose()
        {
            LoggingFacility.Logger = null;
        }

        [Fact]
        public void Analyze_EmptyFile_ThrowsException()
        {
            LoggingFacility.Logger = Substitute.For<ILoggerChap7>();
            var cm = new ConfigurationManager();
            var configured = cm.IsConfigured("something");
            // 残りのテストを実装
        }
    }


    // 上記をベースとなるクラスを作り、そちらで初期化などを行うようにリファクタリング
    // ただし、可読性が落ちるので注意
    public class BaseTestsClass : IDisposable
    {
        // 各テストで使う読みやすいヘルパーメソッドを用意する
        public ILoggerChap7 FakeTheLogger()
        {
            LoggingFacility.Logger = Substitute.For<ILoggerChap7>();
            return LoggingFacility.Logger;
        }

        public void Dispose()
        {
            LoggingFacility.Logger = null;
        }
    }

    public class LogAnalyzerTestsAfter : BaseTestsClass
    {
        [Fact]
        public void Analyze_EmptyFile_ThrowsException()
        {
            // ヘルパーメソッドを読んで、静的リソースを初期化
            FakeTheLogger();
            var la = new LogAnalyzerChap7();
            la.Analyze("myemptyfile.txt");
            // 残りのテストを実装
        }
    }

    public class ConfigurationManagerTestsAfter : BaseTestsClass
    {
        [Fact] // 実装は省略
        public void Analyze_EmptyFile_ThrowsException() { }
    }
}
