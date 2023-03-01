using System.Text.RegularExpressions;

namespace TransportationCompany.Validation
{
    public class ValidationInputController
    {
        public static bool CheckEmailIsRightFormat(string email)
        {
            try
            {
                var emailChecker = new System.Net.Mail.MailAddress(email);
                return emailChecker.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static bool CheckPhoneVaild(string phone)
        {
            string pattern = @"^0\d{9,10}$";
            return Regex.IsMatch(phone, pattern);
        }
        public static bool IsValidPassword(string password)
        {
            // Define a set of password rules
            bool hasLength = password.Length >= 8;
            bool hasUpperCase = password.Any(char.IsUpper);
            bool hasLowerCase = password.Any(char.IsLower);
            bool hasNumber = password.Any(char.IsDigit);
            bool hasSymbol = password.Any(ch => !char.IsLetterOrDigit(ch));

            // Check if the password meets all the rules
            return hasLength && hasUpperCase && hasLowerCase && hasNumber && hasSymbol;
        }

        public static bool CheckSeatIsRightFormat(string seat)
        {
            string pattern = @"^[A-Z]\d{1,2}$";
            return Regex.IsMatch(seat, pattern);
        }
    }
}
