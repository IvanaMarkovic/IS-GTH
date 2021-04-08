using GlobalThinkersHelper.Model.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GlobalThinkersHelper.Validation
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value as string;
            user u = user.SelectByUsername(str);
            if (u == null)
            {
                return new ValidationResult(false, "Nepostojeće korisničko ime");
            }
            return new ValidationResult(true, null);
        }
    }

    public class NotEmptyValidationRulePassword : ValidationRule
    {
        public int MinimalCharacters { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value as string;
            if (str == null)
            {
                return new ValidationResult(false, "Obavezno polje");
            }
            if (str.Length < MinimalCharacters)
            {
                return new ValidationResult(false, $"Lozinka mora imati bar ${MinimalCharacters} karaktera");
            }
            return new ValidationResult(true, null);
        }
    }

    public class ValidationNameRule : ValidationRule
    {
        public int MinimalCharacters { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string name = value as string;
            if (name == null || name.Length < 1)
            {
                return new ValidationResult(false, $"Polje je obavezno");
            }
            else
            {
                var match = Regex.Match(name, "^([a-zA-Z]*)$", RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    return new ValidationResult(false, "Polje ne može da sadrži simbole");
                }
                return new ValidationResult(true, null);
            }

        }
    }
    public class ValidationContactRule : ValidationRule
    {
        public int MinimalCharacters { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string contact = value as string;
            if (contact == null || contact.Length < 1)
            {
                return new ValidationResult(false, $"Polje je obavezno");
            }
            var match = Regex.Match(contact, "^[\\+(00)]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\\s\\./0-9]*$", RegexOptions.IgnoreCase);
            if (!match.Success && match != null)
            {
                return new ValidationResult(false, $"Broj ne moze sadrzati simbole i brojeve");
            }
            return new ValidationResult(true, null);

        }
    }
    public class ValidationEmailRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string contact = value as string;
            if (contact == null || contact.Length < 1)
            {
                return new ValidationResult(false, $"Polje je prazno");
            }

            if (!IsValid(contact))
            {
                return new ValidationResult(false, $"Email nije u dobrom formatu");
            }

            return new ValidationResult(true, null);

        }
        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }

}
