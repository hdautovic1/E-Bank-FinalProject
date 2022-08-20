using System.ComponentModel.DataAnnotations;

namespace E_Bank_FinalProject.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

    }
}
