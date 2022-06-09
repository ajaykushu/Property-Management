using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.CheckList.RequestModels
{
    public class MoveTask
    {
        public Direction d { set; get; }
        public ItemType ItemType { set; get; }
        public string InspectionId { set; get; }
        public int ItemId { get; set; }
        public int LocationId { set; get; }
    }
    public enum Direction
    {
        Up,
        Down
    }
    public enum ItemType
    {
        List,
        Task
    }
}
