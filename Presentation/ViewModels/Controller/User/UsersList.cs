namespace Presentation.ViewModels
{
    public class UsersList
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string PrimaryProperty { get; set; }
        public string Email { get; set; }
        
        public string Roles { get; set; }
        public bool IsActive { get; set; }
    }
}