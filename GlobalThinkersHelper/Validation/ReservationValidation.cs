using GlobalThinkersHelper.Model.Entities;
using GlobalThinkersHelper.View;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GlobalThinkersHelper.Validation
{
    class ReservationValidation
    {
    }

    public class NumberValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var content = value as string;
            if (content == null) 
            {
                return ValidationResult.ValidResult;
            }
            if (content != null)
            {
                var match = Regex.Match(content, @"-?\d+(\.\d+)?", RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    return new ValidationResult(false, "Morate unijeti broj");
                }
            }
            return new ValidationResult(true, null);
        }
    }

    public class NumberValidation2 : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var content = value as string;
            if (content == null || content.Equals(""))
            {
                return new ValidationResult(false, "Morate unijeti broj");
            }
            else
            {
                int result1;
                double result2;
                if (int.TryParse(content, out result1) == true || double.TryParse(content, out result2) == true)
                {
                    return new ValidationResult(true, null);
                }
                else
                {
                    return new ValidationResult(false, "Morate unijeti broj");
                }
            }
        }
    }

    public class ClientValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var content = value as string;
            if (content == null)
            {
                return new ValidationResult(false, "Morate odabrati klijenta");
            }
            long clientId = CreateReservation.Clients.FirstOrDefault(c => c.Value.Equals(content)).Key;
            if (clientId < 1)
            {
                return new ValidationResult(false, "Morate odabrati postojeceg klijenta");
            }
            client newClient = client.SelectById(clientId);
            if (newClient == null)
            {
                return new ValidationResult(false, "Morate odabrati klijenta");
            }
            return new ValidationResult(true, null);
        }
    }

    public class TimeValidationRule : ValidationRule
    {
        public DateTime? TimeFrom { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
           
            if (value == null || value as string == "")
            {
                return new ValidationResult(true, null);
            }
            var content = Convert.ToDateTime(value as string);
            TimeFrom = CreateHall.TimeFrom;
            if (TimeFrom != null && TimeFrom.Value.CompareTo(content)> 0)
            {
                return new ValidationResult(false, $"Morate unijeti vrijeme veće od {TimeFrom.Value.ToString("HH:mm")}");
            }
            return new ValidationResult(true, null);
        }
    }

    public class EventValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var content = value as string;
            if (content == null)
            {
                return new ValidationResult(false, "Morate odabrati događaj");
            }
            long eventId = CreateReservation.Events.FirstOrDefault(e => e.Value.Equals(content)).Key;
            if (eventId < 1)
            {
                return new ValidationResult(false, "Morate odabrati postojeci događaj");
            }
            client newEvent = client.SelectById(eventId);
            if (newEvent == null)
            {
                return new ValidationResult(false, "Morate odabrati događaj");
            }
            return new ValidationResult(true, null);
        }
    }

    public class Wrapper : DependencyObject
    {
        public static readonly DependencyProperty TimeUntilProperty =
            DependencyProperty.Register("TimeUntil", typeof(object), typeof(Wrapper), new FrameworkPropertyMetadata(TimePicker.TextProperty));

        public TimePicker TimeUntil { get; set; }

    }

    public class TimeValidationRule2 : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var content = value as string;
            if (content == null)
            {
                return new ValidationResult(false, "Morate odabrati vrijeme početka");
            }
            if (Wrapper.TimeUntilProperty == null)
            {
                return new ValidationResult(false, "Morate odabrati vrijeme početka");
            }
            return new ValidationResult(true, null);
        }

    }

    public class NotEmpty : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var content = value as string;
            if (content == null || content.Equals(""))
            {
                return new ValidationResult(false, "Polje je obavezno");
            }
            return new ValidationResult(false, "Polje je obavezno");
            //return new ValidationResult(true, null);
        }
    }

}
