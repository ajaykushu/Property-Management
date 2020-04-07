namespace Models
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
        INITWO = 1,
        WOPROGRESS = 2,
        WOCOMPLETED = 3
    }
}