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
    /// <summary>
    /// Klasa koja nam služi za provjeru broja telefona prilikom kreiranja klijenta.
    /// </summary>
    public class PhoneNumberValidationRule : ValidationRule
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

    /// <summary>
    /// Klasa koja nam služi za provjeru email adrese prilikom kreiranja klijenta.
    /// </summary>
    public class EmailValidationRule : ValidationRule
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

        private bool IsValid(string emailaddress)
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

    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string contact = value as string;
            if (contact == null || contact.Length == 0)
            {
                return new ValidationResult(false, $"Polje je obavezno");
            }
            return new ValidationResult(true, null);

        }
    }
}
