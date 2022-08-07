
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Bank_FinalProject.Models
{

    public class UserRoles
    {    
        [Key]
        public int ID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
       
        public User User { get; set; }
 
        [ForeignKey("Role")]
      
        public int RoleID { get; set; }
     
        public Role Role { get; set; }

   
    }
}
