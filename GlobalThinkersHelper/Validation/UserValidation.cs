using GlobalThinkersHelper.Model.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GlobalThinkersHelper.Validation
{
    /// <summary>
    /// Klasa za provjeru korisničkog imena prilikom prijave na sistem.
    /// </summary>
    public class UsernameLogInValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringUser = value as string;
            user user = user.SelectByUsername(stringUser);
            if (user == null)
            {
                return new ValidationResult(false, "Korisničko ime ne postoji");
            }
            EntityFactory.User = user;
            return new ValidationResult(true, null);
        }
    }

    /// <summary>
    /// Klasa za provjeru lozinke prilikom prijave na sistem.
    /// </summary>
    public class PasswordLogInValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value as string;
            if (str == null)
            {
                return new ValidationResult(false, "Unesite lozinku");
            }

            if (!BCrypt.Net.BCrypt.Verify(str, EntityFactory.User.password))
            {
                return new ValidationResult(false, $"Lozinka nije ispravna");
            }
            return new ValidationResult(true, null);
        }
    }

    /// <summary>
    /// Klasa za provjeru ime i prezimena korisnika prilikom registracije.
    /// </summary>
    class NameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value as string;
            if (str == null)
            {
                return new ValidationResult(false, "Obavezno polje");
            }
            if (str != null)
            {
                var match = Regex.Match(str, @"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    return new ValidationResult(false, "Polje sadrzi nedozvoljene karaktere");
                }
            }
            return new ValidationResult(true, null);
        }
    }

    /// <summary>
    /// Klasa za provjeru korisničkog imena prilikom registracije na sistem.
    /// </summary>
    class UsernameRegValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value as string;
            user user = user.SelectByUsername(str);
            if (str == null)
            {
                return new ValidationResult(false, "Obavezno polje");
            }
            if (user != null)
            {
                return new ValidationResult(false, "Korisničko ime je zauzeto");
            }
            if (str != null)
            {
                var match = Regex.Match(str, @"^[a-zA-Z0-9]+([_ -]?[a-zA-Z0-9])*$", RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    return new ValidationResult(false, "Polje sadrzi nedozvoljene karaktere");
                }
            }
            return new ValidationResult(true, null);
        }
    }

    /// <summary>
    /// Klasa za provjeru lozinke prilikom registracija na sistem.
    /// </summary>
    public class PasswordRegValidationRule : ValidationRule
    {
        public int MinimalCharacters { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var password = value as string;
            if (password == null)
            {
                return new ValidationResult(false, "Unesite lozinku");
            }
            if (password.Length < MinimalCharacters)
            {
                return new ValidationResult(false, $"Lozinka mora imati više od {MinimalCharacters} karaktera");
            }
            return new ValidationResult(true, null);
        }
    }

    /// <summary>
    /// Klasa koja se poziva prilikom validacije korisnickog imena kod izmjene podataka o korisniku.
    /// </summary>
    public class UsernameUpdateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value as string;
            user u = user.SelectByUsername(str);
            if (u != null)
            {
                if (u.username == EntityFactory.User.username)
                {
                    u.first_name = EntityFactory.User.first_name;
                    u.last_name = EntityFactory.User.last_name;
                    return new ValidationResult(true, null);
                }
                else
                {
                    return new ValidationResult(false, "Korisničko ime je zauzeto");
                }
            }
            if (str == null)
            {
                return new ValidationResult(false, "Obavezno polje");
            }
            var match = Regex.Match(str, @"[^\(\)`~!@#\$%\^\&\*_\+{}\|:\[\]\;',\.\/\\\?\*]+", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return new ValidationResult(false, "Polje ne može da sadrži simbole");
            }
            return new ValidationResult(true, null);
        }
    }
    
}
