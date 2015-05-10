using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn.UnitTests.Chap7
{
    using Xunit;
    using LogAn.Chap7;

    //----StandardStringParserだけがあった時の、リファクタリング前のテストコード
    public class StringParserTests
    {
        private StandardStringParser GetParser(string input)
        {
            // パーサーのファクトリメソッド
            return new StandardStringParser(input);
        }

        [Fact]
        public void GetStringVersionsFromHeader_SingleDigit_Found()
        {
            var input = "header;version=1;\n";
            var parser = GetParser(input);

            // ファクトリメソッドの使用
            var versionFromHeader = parser.GetStringVersionFromHeader();

            // 仮実装に従ったテスト結果(実際には正しいexpectedを入れる)
            Assert.Equal("1", versionFromHeader);
        }

        [Fact]
        public void GetStringVersionsFromHeader_WithMinorVersion_Found()
        {
            var input = "header;version=1.1;\n";
            // ファクトリメソッドの使用
            var parser = GetParser(input);
            // 残りのテストを実装
        }

        [Fact]
        public void GetStringVersionsFromHeader_WithRivisionVersion_Found()
        {
            var input = "header;version=1.1.1;\n";
            // ファクトリメソッドの使用
            var parser = GetParser(input);
            // 残りのテストを実装
        }
    }

    //----XMLStringParserが加わった時に変更したテスト
    // テンプレートとなる抽象クラスを用意し、Parserごとに具象クラスとしてテストを実装
    public abstract class TemplateStringParserTest
    {
        // Testというプレフィックスを付けることで、
        // 使う人はこのメソッドをオーバーライドして使うことが重要と理解できる
        public abstract void TestGetStringVersionFromHeader_SingleDigit_Found();
        public abstract void TestGetStringVersionsFromHeader_WithMinorVersion_Found();
        public abstract void TestGetStringVersionsFromHeader_WithRivisionVersion_Found();
    }

    public class XmlStringParserTests : TemplateStringParserTest
    {
        protected IStringParser GetParser(string input)
        {
            return new XmlStringParser(input);
        }

        [Fact]
        public override void TestGetStringVersionFromHeader_SingleDigit_Found()
        {
            var parser = GetParser("<Header>1</Header>");
            var versionFromHeader = parser.GetStringVersionFromHeader();
            Assert.Equal("", versionFromHeader);
        }

        [Fact] // 実装は省略
        public override void TestGetStringVersionsFromHeader_WithMinorVersion_Found() { }

        [Fact] // 実装は省略
        public override void TestGetStringVersionsFromHeader_WithRivisionVersion_Found() { }
    }


    //----Fill in the Blanksクラスを継承した、Test Drive Class Patternのテスト
    public abstract class FillInTheBlanksStringParserTests
    {
        // 派生クラスでインスタンスを得るためのファクトリメソッド
        protected abstract IStringParser GetParser(string input);

        // 具象クラス向けの特定のフォーマットデータを渡すメソッド
        protected abstract string HeaderVersion_SingleDigit { get; }
        protected abstract string HeaderVersion_WithMinorVersion { get; }
        protected abstract string HeaderVersion_WithRevision { get; }

        // 予め用意した、派生クラスでの期待値
        public const string EXPECTED_SINGLE_DIGIT = "1";
        public const string EXPECTED_WITH_MINOR_VERSION = "1.1";
        public const string EXPECTED_WITH_RIVISION = "1.1.1";

        [Fact]
        public void GetStringVersionFromHeader_SingleDigit_Found()
        {
            // 予め定義した、具象クラス向けのテストロジック
            var input = HeaderVersion_SingleDigit;
            var parser = GetParser(input);

            var versionFromHeader = parser.GetStringVersionFromHeader();

            Assert.Equal(EXPECTED_SINGLE_DIGIT, versionFromHeader);
        }

        [Fact]  // 実装は省略
        public void GetStringVersionFromHeader_WithMinorVersion_Found() { }

        [Fact]  // 実装は省略
        public void GetStringVersionFromHeader_WithRivision_Found() { }
    }

    // 派生クラスでの実装
    public class StandardStringParserTests : FillInTheBlanksStringParserTests
    {

        protected override IStringParser GetParser(string input)
        {
            return new StandardStringParser(input);
        }

        protected override string HeaderVersion_SingleDigit
        {
            get { return string.Format("header\tversion={0}\t\n", EXPECTED_SINGLE_DIGIT); }
        }

        protected override string HeaderVersion_WithMinorVersion
        {
            get { return string.Format("header\tversion={0}\t\n", EXPECTED_WITH_MINOR_VERSION); }
        }

        protected override string HeaderVersion_WithRevision
        {
            get { return string.Format("header\tversion={0}\t\n", EXPECTED_WITH_RIVISION); }
        }
    }


    //----.NETのジェネリックを使ったテスト
    public abstract class GenericParserTests<T>
        where T:IStringParser
    {
        protected abstract string GetInputHeaderSigleDigit();

        protected T GetParser(string input)
        {
            // ジェネリックを使うことで、ファクトリメソッドのオーバーライドが不要
            return (T) Activator.CreateInstance(typeof (T), input);
        }

        [Fact]
        public void GetStringVersionFromHeader_SingleDigit_Found()
        {
            var input = GetInputHeaderSigleDigit();
            T parser = GetParser(input);

            bool result = parser.HasCorrectHeader();
            Assert.True(result);
        }
    }

    // 上記のジェネリッククラスを継承したテストクラス
    public class StandardParserGenericTests : GenericParserTests<StandardStringParser>
    {
        protected override string GetInputHeaderSigleDigit()
        {
            return "Header;1";
        }
    }
}
