﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Bank_FinalProject.Models
{
    public class Account
    {
        [Key]
        public int AccountID { get; set; }
        [Required]
        [StringLength(100)]
        public string AccountName { get; set; }  
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
        public string AccountDescription { get; set; }

        [Required]
        public User User { get; set; }
        
        [ForeignKey("User")]

        public int UserID { get; set; }
    }
}