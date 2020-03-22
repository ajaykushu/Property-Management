namespace Utilities.Interface
{
    public interface ICache
    {
        public void AddItem(string key, object value);
        public object GetItem(string key);
    }
}
