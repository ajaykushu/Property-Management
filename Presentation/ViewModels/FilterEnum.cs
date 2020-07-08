using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public enum FilterEnum
    {
        Email = 1,
        [Display(Name = "First Property")]
        FirstName = 2,
        [Display(Name = "Primary Property")]
        Property = 3,
    }

    public enum MenuEnum
    {
        [Display(Name = "Add User")]
        Add_User = 1,
        [Display(Name = "View Users")]
        View_Users = 2,
        [Display(Name = "View Property")]
        View_Property = 3,
        [Display(Name = "Edit User")]
        Edit_User = 4,
        [Display(Name = "Add Property")]
        Add_Property = 5,
        [Display(Name = "Edit Property")]
        Edit_Property = 6,
        [Display(Name = "ActDct User")]
        ActDct_User = 7,
        [Display(Name = "View User Detail")]
        View_User_Detail = 8,
        [Display(Name = "Act Deact Property")]
        Act_Deact_Property = 9,
        [Display(Name = "Edit Feature")]
        Edit_Feature = 10,
        [Display(Name = "Access Setting")]
        Access_Setting = 11,
        [Display(Name = "Create WO")]
        Create_WO = 12,
        [Display(Name = "Get WO")]
        Get_WO = 13,
        [Display(Name = "GetWO Detail")]
        GetWO_Detail = 14,
        [Display(Name = "Edit WO")]
        Edit_WO = 15,
        [Display(Name = "Post Comment")]
        Post_Comment = 16,
        [Display(Name = "Assign To User")]
        Assign_To_User = 17,
        [Display(Name = "WO Operation")]
        WO_Operation = 18
    }
}