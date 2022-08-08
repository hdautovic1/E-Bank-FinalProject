using System.ComponentModel.DataAnnotations;

namespace E_Bank_FinalProject.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

      
        [StringLength(50)]
        [Display(Name = "Username")]
        public string UserName { get; set; }
      
        [StringLength(50)]
        [Display(Name ="First name")]
        public string FirstName { get; set; }

        [StringLength(50)]

        [Display(Name = "Last name")]
        public string LastName { get; set; }
        
        [StringLength(100)]
        public string Email { get; set; }

        
        [DataType(DataType.Password)]
        [StringLength(50)]
        [Display(Name = "password")]
        public string Password { get; set; }

        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match!")]
        [StringLength(50)]

        [Display(Name = "password")]
        public string ConfirmedPassword { get; set; }

        }
}
