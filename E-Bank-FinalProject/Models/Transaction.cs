using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Bank_FinalProject.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date created:")]
        public DateTime TransactionDate { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public Account Account { get; set; }

        [ForeignKey("Account")]
        public int AccountID { get; set; }

        public CreditCard CreditCard { get; set; }

        [ForeignKey("CreditCard")]
        public int CreditCardID { get; set; }
    }
}
