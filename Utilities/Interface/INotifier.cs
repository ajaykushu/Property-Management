using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utilities.Interface
{
    public interface INotifier
    {
        Task<bool> CreateNotification(string message, List<long> AssignedTo, string navigatorId, string Type);
    }
}