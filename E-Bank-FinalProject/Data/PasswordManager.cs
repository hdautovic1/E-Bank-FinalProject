using System.Text;

namespace E_Bank_FinalProject.Data
{
    public class PasswordManager
    {
        public static string key = "blablaThisISSecretKEY123";
        public static string Encode(string password)
        {
            if (string.IsNullOrEmpty(password)) return "";
            password += key;
            var passwordBytes=Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(passwordBytes);
        }
        public static string Decode(string encoded)
        {
            if (string.IsNullOrEmpty(encoded)) return "";
            var passwordBytes=Convert.FromBase64String(encoded);
            var password=Encoding.UTF8.GetString(passwordBytes);
            return password.Substring(0,password.Length-key.Length);           

        }
    }
}
