using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels.Controller.CheckList
{
    public enum Direction
    {
        Up,
        Down
    }

    public class MoveTask
    {
        public Direction d { set; get; }
        public ItemType ItemType { get; set; }
        public string InspectionId { set; get; }
        public int LocationId { set; get; }
    }

    public enum ItemType {
        List,
        Task
}
    
}
