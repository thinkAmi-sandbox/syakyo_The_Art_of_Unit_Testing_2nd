using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn.Chap7
{
    public static class TimeLogger
    {
        public static string CreateMessage(string info)
        {
            // これでは時間をInjectionできない
            //return DateTime.Now.ToShortDateString() + " " + info;

            // DateTimeをラップしたクラスを用意して、
            // そちらで取得した値を返すようにする
            return SystemTime.Now.ToShortDateString() + " " + info;
        }
    }

    public static class SystemTime
    {
        private static DateTime _date;

        // SystemTimeの現在時刻を変更できるようにする
        public static void Set(DateTime custom)
        {
            _date = custom;
        }

        // 現在時刻をリセットできるようにする
        // リセットした後は、一番古い日時になる
        public static void Reset()
        {
            _date = DateTime.MinValue;
        }

        // Nowメソッドは設定により、
        // システム時刻or設定したFakeの時刻を返す
        public static DateTime Now
        {
            get
            {
                if (_date != DateTime.MinValue)
                {
                    return _date;
                }
                return DateTime.Now;
            }
        }
    }
}
