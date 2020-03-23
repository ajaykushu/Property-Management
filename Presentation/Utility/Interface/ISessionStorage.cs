using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Utility.Interface
{
    public interface ISessionStorage
    {
        void AddItem(long key, object value);
        object GetItem(long key);
        void RemoveItem(long key);
    }
}
