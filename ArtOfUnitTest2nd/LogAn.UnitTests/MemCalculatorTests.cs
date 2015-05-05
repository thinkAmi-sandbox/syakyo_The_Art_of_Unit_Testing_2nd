using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn.UnitTests
{
    using Xunit;

    public class MemCalculatorTests
    {
        [Fact]
        public void Sum_ByDefault_ReturnsZero()
        {
            var calc = new MemCalculator();

            var lastSum = calc.Sum();

            Assert.Equal(0, lastSum);
        }

        [Fact]
        public void Add_WhenCalled_ChangesSum()
        {
            var calc = MakeCalc();

            calc.Add(1);
            var sum = calc.Sum();

            Assert.Equal(1, sum);
        }

        // MemCalculatorのファクトリメソッド
        private static MemCalculator MakeCalc()
        {
            return new MemCalculator();
        }
    }
}
