namespace Models
{
    public class Pagination<T>
    {
        public int ItemsPerPage { get; set; }
        public T Payload { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
    }
}