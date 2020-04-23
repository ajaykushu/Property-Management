namespace Presentation.ViewModels
{
    public enum FilterEnum
    {
        ByEmail = 1,
        ByFirstName = 2
    }

    public enum FilterEnumWO
    {
        ByDate = 1,
        ByAssigned = 2,
        ByStatus = 3
    }

    public enum FilterEnumWOStage
    {
        OPEN = 1,
        BIDACCEPTED = 2,
        INPROGRESS = 3,
        PENDINGAPPROVAL = 4,
        COMPLETED = 5,
    }

    public enum MenuEnum
    {
        Add_User = 1,
        View_Users = 2,
        View_Property = 3,
        Edit_User = 4,
        Add_Property = 5,
        Edit_Property = 6,
        ActDct_User = 7,
        View_User_Detail = 8,
        Act_Deact_Property = 9,
        Edit_Feature = 10,
        Access_Setting = 11,
        Create_WO = 12,
        Get_WO = 13,
        GetWO_Detail = 14,
        Edit_WO = 15,
        Post_Comment = 16,
        Assign_To_User = 17,
        WO_Operation = 18
    }
}