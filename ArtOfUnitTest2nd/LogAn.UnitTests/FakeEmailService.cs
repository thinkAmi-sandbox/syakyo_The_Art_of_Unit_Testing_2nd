using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn.UnitTests
{
    public class FakeEmailService : IEmailService
    {
        // 複数のプロパティをテスト対象とするなら、
        // 一つのオブジェクトにまとめたほうが分かりやすく変更にも強いので
        // こちらよりは、
        //public string To;
        //public string Subject;
        //public string Body;
        //public void SendEmail(string to, string subject, string body)
        //{
        //    To = to;
        //    Subject = subject;
        //    Body = body;
        //}

        // こちらのほうが良い
        public EmailInfo email = null;
        public void SendEmail(string to, string subject, string body)
        {
            email = new EmailInfo()
            {
                To = to,
                Subject = subject,
                Body = body
            };
        }
    }
}
