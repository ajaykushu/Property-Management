using Presentation.Utility.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Utility
{
    public class SessionStorage:ISessionStorage
    {
        private readonly Dictionary<long, object> _dict;
        public SessionStorage()
        {
            _dict = new Dictionary<long, object>();
        }

        public void AddItem(long key, object value )
        {
            lock(_dict){
                if (_dict.ContainsKey(key))
                    _dict.Remove(key);
                _dict.Add(key, value);

               }
        }
        public object GetItem(long key)
        {
            lock (_dict)
            {
                if (_dict.TryGetValue(key, out object value))
                    return value;
                else
                    return null;
            }
        }
        public void RemoveItem(long key)
        {
            lock (_dict)
            {
                if (_dict.ContainsKey(key))
                    _dict.Remove(key);
            }
        }
    }
}
