using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn.UnitTests
{
    public class EmailInfoComparer : IEqualityComparer<EmailInfo>
    {
        public bool Equals(EmailInfo x, EmailInfo y)
        {
            return
                x.To.Equals(y.To, StringComparison.Ordinal) &&
                x.Subject.Equals(y.Subject, StringComparison.Ordinal) &&
                x.Body.Equals(y.Body, StringComparison.Ordinal);
        }

        public int GetHashCode(EmailInfo info)
        {
            return
                info.To.GetHashCode() ^
                info.Subject.GetHashCode() ^
                info.Body.GetHashCode();
        }
    }
}
