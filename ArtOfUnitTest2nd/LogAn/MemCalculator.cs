using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn
{
    public class MemCalculator
    {
        private int sum = 0;

        public void Add(int number)
        {
            sum += number;
        }

        public int Sum()
        {
            var temp = sum;
            sum = 0;
            return temp;
        }

    }
}
