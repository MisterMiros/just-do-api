using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.Model.Utils
{
    public static class ValidationUtils
    {

        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            return Regex.IsMatch(email, "^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$");
        }

        public static bool ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            if (password.Length < 6)
            {
                return false;
            }

            if (Regex.IsMatch(password, "\\s"))
            {
                return false;
            }

            if (Regex.Matches(password, "[a-z]").Count < 2)
            {
                return false;
            }

            if (Regex.Matches(password, "[A-Z]").Count < 2)
            {
                return false;
            }

            if (!Regex.IsMatch(password, "[0-9]"))
            {
                return false;
            }

            if (!Regex.IsMatch(password, "[*.!@#$%^&(){}[\\]:;<>,.?\\/~_ +\\-=|\\\\]"))
            {
                return false;
            }

            return true;
        }
    }
}
