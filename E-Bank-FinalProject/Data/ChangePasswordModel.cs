using System.ComponentModel.DataAnnotations;

namespace E_Bank_FinalProject.Data
{
    public class ChangePasswordModel
    {
        [DataType(DataType.Password)]
        [StringLength(50)]
        [Required]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [StringLength(50)]
        [Required]
        [Display(Name = "New password")]

        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match!")]
        [StringLength(50)]
        [Required]
        [Display(Name = "Confirm new password")]

        public string ConfirmNewPassword { get; set; }

    }
}
