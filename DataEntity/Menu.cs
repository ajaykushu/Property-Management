using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Menu
    {
        public Menu()
        {

            this.RoleMenuMaps = new HashSet<RoleMenuMap>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { set; get; }
        [Required]
        [Column(TypeName = "varchar(30)")]
        public string MenuName { set; get; }
        [Required]
        public long MainMenuId { set; get; }
        public MainMenu MainMenu { set; get; }
        public ICollection<RoleMenuMap> RoleMenuMaps { set; get; }
    }
}
