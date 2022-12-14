using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Bank_FinalProject.Models
{
    public class CreditCard
    {
        [Key]
        public int CreditCardID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Credit card name")]
        public string CreditCardName { get; set; }

        [Required]
        [StringLength(12)]
        [Display(Name = "Credit card number")]
        public string CreditCardNumber { get; set; }


        [ForeignKey("Account")]

        public int AccountID { get; set; }


        [Required]
        public Account Account { get; set; }
    }
}
