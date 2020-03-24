namespace Utilities.Interface
{
    public interface ICache
    {
        public void AddItem(string key, object value, long ticks);
        public string GetItem(string key);
    }
}
