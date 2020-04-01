namespace Utilities.Interface
{
    public interface ICache
    {
        public void AddItem(string key, object value, long ticks);

        public object GetItem(string key);

        void RemoveItem(string v);
    }
}