﻿
namespace Models.ResponseModels
{
    public class UsersListModel
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Roles { get; set; }
        public bool IsActive { get; set; }

    }
}