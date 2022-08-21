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
        [DataType(DataType.Currency)]
        public double Amount { get; set; }

        [Required]
        public Account Account { get; set; }

        [ForeignKey("Account")]
        public int AccountID { get; set; }

        [Required]
        [StringLength(40)]
        public string TransactionType { get; set; }
    }
}
