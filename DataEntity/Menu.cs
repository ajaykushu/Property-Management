using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Menu : Log
    {
        public Menu()
        {
            this.RoleMenuMaps = new HashSet<RoleMenuMap>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { set; get; }

        [Column(TypeName = "varchar(50)")]
        public string MenuName { set; get; }

        public ICollection<RoleMenuMap> RoleMenuMaps { set; get; }
    }
}