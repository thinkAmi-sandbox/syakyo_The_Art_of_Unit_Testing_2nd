using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn.Chap7
{
    public interface IStringParser
    {
        string StringToParse { get; }

        bool HasCorrectHeader();
        string GetStringVersionFromHeader();
    }

    public abstract class BaseStringParser : IStringParser
    {
        public string StringToParse { get; private set; }

        protected BaseStringParser(string filename)
        {
            StringToParse = filename;
        }

        public abstract bool HasCorrectHeader();

        public abstract string GetStringVersionFromHeader();
    }

    public class XmlStringParser : BaseStringParser
    {
        public XmlStringParser(string toParse) : base(toParse) { }

        public override bool HasCorrectHeader() { return true; }
        public override string GetStringVersionFromHeader() { return ""; }
    }
    public class StandardStringParser : BaseStringParser
    {
        public StandardStringParser(string toParse) : base(toParse) { }

        // 以下の実装は例示のための仮実装
        public override bool HasCorrectHeader() { return true; }
        public override string GetStringVersionFromHeader() { return "1"; }
    }
}
