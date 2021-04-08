using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalThinkersHelper.Model.Entities
{
    public partial class Contact
    {
        public string[] Emails { get; set; }
        public string[] PhoneNumbers { get; set; }

        public Contact() { }

        public Contact(string[] emails, string[] phoneNumbers)
        {
            PhoneNumbers = phoneNumbers;
            Emails = emails;
        }

        public override bool Equals(object obj)
        {
            var contact = obj as Contact;
            return contact != null;
        }

        public override int GetHashCode()
        {
            return -528980569 + PhoneNumbers.GetHashCode() + Emails.GetHashCode();
        }

        public override string ToString()
        {
            return "Phone numbers: " + PhoneNumbers +
                   " E-mails: " + Emails;
        }

    }
}
