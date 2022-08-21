using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Bank_FinalProject.Models
{
    public class Account
    {
        [Key]
        public int AccountID { get; set; }

        [Required]
        [StringLength(12)]
        [Display(Name = "Account number:")]
        public string AccountNumber { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Account name:")]
        public string AccountName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date created:")]
        public DateTime DateCreated { get; set; }

        [StringLength(250)]
        [Display(Name = "Account description:")]
        public string AccountDescription { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double Balance { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double Limit { get; set; }

        [Required]
        public User User { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
    }
}
