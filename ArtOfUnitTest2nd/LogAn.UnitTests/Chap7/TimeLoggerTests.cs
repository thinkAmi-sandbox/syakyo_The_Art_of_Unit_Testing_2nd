using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn.UnitTests.Chap7
{
    using Xunit;
    using LogAn.Chap7;
    public class TimeLoggerTests : IDisposable
    {
        public void Dispose()
        {
            // 他のテストに影響しないよう、
            // テストごとにシステム時刻を初期化する
            SystemTime.Reset();
        }

        [Fact]
        public void SettingSystemTime_Always_ChangesTime()
        {
            SystemTime.Set(new DateTime(2000, 1, 1));

            var output = TimeLogger.CreateMessage("a");

            Assert.Contains("2000/01/01", output);
        }
    }
}
