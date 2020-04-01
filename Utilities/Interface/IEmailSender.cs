using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utilities
{
    public interface IEmailSender
    {
        Task SendAsync(string recieveremail, string subject, string body, bool isbodyHTML);

        Task SendAsync(List<string> recieveremail, string subject, string body, bool isbodyHTML);
    }
}