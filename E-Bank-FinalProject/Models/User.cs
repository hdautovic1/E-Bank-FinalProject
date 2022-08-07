using System.ComponentModel.DataAnnotations;

namespace E_Bank_FinalProject.Models
{
    public class User
    {

        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
