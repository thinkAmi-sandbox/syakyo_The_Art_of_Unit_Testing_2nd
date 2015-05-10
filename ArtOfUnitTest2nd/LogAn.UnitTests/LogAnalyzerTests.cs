using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn.UnitTests
{
    using Xunit;
    using NSubstitute;

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


        [Fact]
        public void IsValid_FileName_SupportedExtension_ReturnsTrue()
        {
            var myFakeManager = new FakeExtensionManager();
            myFakeManager.WillBeValid = true;

            var log = new LogAnalyzer(myFakeManager);

            var result = log.IsValidLogFileName("short.ext");
            Assert.True(result);
        }


        //---Injectionパターン

        [Fact]
        public void IsValidFileName_ExtManagerThrowsException_ReturnsFalse()
        {
            var myFakeManager = new FakeExtensionManager();
            myFakeManager.WillThrow = new Exception("this is fake");

            // コンストラクタでInjectionする
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

            // プロパティでInjectionする
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

            // ファクトリメソッドでInjectionする
            ExtensionManagerFactory.SetManager(myFakeManager);

            // コンストラクタの中でFactoryのCreateメソッドを呼ぶことで、
            // InjectionされたFactoryManagerが使える
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

            // オーバーライド可能なメソッドを持つテストクラスを作って
            // Injectionする
            var logan = new TestableLogAnalyzer(stub);

            var result = logan.IsValidLogFileName("file.ext");

            Assert.True(result);
        }

        [Fact]
        public void OverrideTestWithoutStub()
        {
            // FakeExtensionManagerというスタブを用意せずに、
            // オーバーライド可能なメソッドを持つテストクラスを作って
            // Injectionする
            // ->スタブがない分、シンプルになる(位置No.1692)
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
            // Webサービスとのテストで、mockを使う方法
            var mockWebService = new FakeWebService();
            var log = new LogAnalyzer(mockWebService);
            var tooShortFileName = "abc.ext";

            log.Analyze(tooShortFileName);

            Assert.Contains("FileName too short:abc.ext", mockWebService.LastError);
        }

        
        [Fact]
        public void Analyze_WebServiceThrows_SendMail()
        {
            // stubとmockを組み合わせるテスト
            var stubService = new FakeWebService();
            stubService.ToThrow = new Exception("fake exception");

            var mockEmail = new FakeEmailService();

            var log = new LogAnalyzer(stubService, mockEmail);
            var tooShortFileName = "abc.ext";

            log.Analyze(tooShortFileName);

            // 複数のプロパティをテスト対象とするなら、
            // 一つのオブジェクトにまとめたほうが分かりやすく変更にも強いので
            // こちらよりは、
            //Assert.Contains("someone@somewhere.com", mockEmail.To);
            //Assert.Contains("fake exception", mockEmail.Body);
            //Assert.Contains("can't loga", mockEmail.Subject);

            // こちらのほうが良い
            var expectEmail = new EmailInfo()
            {
                Body = "fake exception",
                To = "someone@somewhere.com",
                Subject = "can't log"
            };

            // xUnit.netの場合、Equalの第三引数に
            // IEqualityComparerを実装した比較用クラスを渡す
            Assert.Equal(expectEmail, mockEmail.email, new EmailInfoComparer());
        }


        //---NSubstituteの利用
        [Fact]
        public void Analyze_TooShortFileName_CallLogger()
        {
            // FakeLoggerを自作した時のテスト
            var logger = new FakeLogger();
            var analyzer = new LogAnalyzer(logger);
            analyzer.MinNameLength = 6;
            analyzer.AnalyzeWhenUsingMessageString("a.txt");

            Assert.Contains("too short", logger.LastError);
        }

        [Fact]
        public void Analyze_TooShortFileName_CallNsubLogger()
        {
            // NSubstituteを使う時のテスト
            var logger = Substitute.For<ILogger>();
            var analyzer = new LogAnalyzer(logger);

            analyzer.MinNameLength = 6;
            analyzer.AnalyzeWhenUsingMessageString("a.txt");

            // ・NSubstituteを使う場合、Assert.Containなどが不要
            // ・LogErrorの引数に期待値を設定してあげることで、
            // 　実行結果が期待値と一致するとテストが通る
            logger.Received().LogError("Filename too short: a.txt");
        }


        [Fact]
        public void Returns_ByDefault_WorksForHardCodedArgument()
        {
            // NSubstituteを使って、fake objectのメソッドが呼ばれた時の戻り値を強制する
            var fakeRules = Substitute.For<IFileNameRules>();

            // ファイル名を固定する場合
            fakeRules.IsValidLogFileName("strict.txt").Returns(true);
            
            Assert.True(fakeRules.IsValidLogFileName("strict.txt"));
        }

        [Fact]
        public void Returns_ByDefault_WorksForHardCodedArgument_IgnoreArgumentValue()
        {
            var fakeRules = Substitute.For<IFileNameRules>();

            // ファイル名を任意にする場合(引数を何でも良くする場合)
            fakeRules.IsValidLogFileName(Arg.Any<string>()).Returns(true);

            Assert.True(fakeRules.IsValidLogFileName("anything.txt"));
        }

        [Fact]
        public void Returns_ArgAny_Throw()
        {
            var fakeRules = Substitute.For<IFileNameRules>();

            // メソッドを呼んだ時に例外を出す方法
            fakeRules.When(x => x.IsValidLogFileName(Arg.Any<string>()))
                     .Do(context => { throw new Exception("fake exception"); });

            var exception = Assert.Throws<Exception>(
                () => fakeRules.IsValidLogFileName("anything"));
        }


        [Fact]
        public void Analyze_LoggerThrows_CallsWebService()
        {
            // NSubを用いないで、複数のfakeを使うテスト
            var mockWebService = new FakeWebService();
            var stubLogger = new FakeLogger2();
            stubLogger.WillThrow = new Exception("fake exception");

            var analyzer = new LogAnalyzer(stubLogger, mockWebService);
            analyzer.MinNameLength = 8;

            var tooShortFileName = "abc.ext";
            analyzer.AnalyzeWhenUsingMessageString(tooShortFileName);

            Assert.Contains("fake exception", mockWebService.MessageToWebService);
        }

        [Fact]
        public void Analyze_LoggerThrows_CallsWebServiceWithNSub()
        {
            // NSubを用いて、複数のfakeを使うテスト
            var mockWebService = Substitute.For<IWebService>();
            
            var stubLogger = Substitute.For<ILogger>();
            stubLogger.When(logger => logger.LogError(Arg.Any<string>()))
                      .Do(info => { throw new Exception("fake exception"); });

            var analyzer = new LogAnalyzer(stubLogger, mockWebService);
            analyzer.MinNameLength = 8;

            var tooShortFileName = "abc.ext";
            analyzer.AnalyzeWhenUsingMessageString(tooShortFileName);

            mockWebService.Received()
                          .Write(Arg.Is<string>(s => s.Contains("fake exception")));
        }


        [Fact]
        public void Analyze_LoggerThrows_CallsWebServiceWithNSubObject()
        {
            var mockWebService = Substitute.For<IWebServiceUsingErrorInfo>();
            var stubLogger = Substitute.For<ILogger>();

            stubLogger.When(logger => logger.LogError(Arg.Any<string>()))
                      .Do(info => { throw new Exception("fake exception"); });

            var analyzer = new LogAnalyzer(stubLogger, mockWebService);

            analyzer.MinNameLength = 10;
            analyzer.AnalyzeWhenUsingErrorInfoObject("short.txt");

            var expected = new ErrorInfo(1000, "fake exception");
            mockWebService.Received()
                          .Write(expected);
        }
    }
}
