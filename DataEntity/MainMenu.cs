using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class MainMenu
    {
        public MainMenu()
        {
            Menus = new HashSet<Menu>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { set; get; }
        [Required]
        [Column(TypeName = "varchar(30)")]
        public string MainMenuName { set; get; }
        public ICollection<Menu> Menus { get; set; }
    }
}
