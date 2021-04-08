using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalThinkersHelper.Model.Entities
{
    partial class client : Entity
    { 
        public client(long user_id, string first_name, string last_name, string company_name, string contact, string note, long[] attends_events) : this() 
        {
            this.user_id = user_id;
            this.first_name = first_name;
            this.last_name = last_name;
            this.company_name = company_name;
            this.contact = contact;
            this.note = note;
            if(attends_events != null)
            {
                foreach (var e in attends_events)
                {
                    this.attends_events.Add(reservation.SelectById(e));
                }
            }
            
        }

        public override bool Equals(object obj)
        {
            var client = obj as client;
            return client != null &&
                  client.id == id;
        }

        public override int GetHashCode()
        {
            return -528980569 + id.GetHashCode();
        }

        public override string ToString()
        {
            return first_name + " " + last_name;
        }
    }

    partial class _event
    {
        public _event(string name, string type, decimal? event_price) 
        {
            this.name = name;
            this.type = type;
            this.event_price = event_price;
        }

        public override bool Equals(object obj)
        {
            var _event = obj as _event;
            return _event != null &&
                  _event.id == id;
        }

        public override int GetHashCode()
        {
            return -528980569 + id.GetHashCode();
        }

        public override string ToString()
        {
            return "Event name: " + name +
                   " Type: " + type;
        }
    }

    partial class hall
    {
        public hall(string name, ICollection<price_list> price_lists)
        {
            this.name = name;
            this.price_lists = price_lists;
        }

        public override bool Equals(object obj)
        {
            var hall = obj as hall;
            return hall != null &&
                  hall.id == id;
        }

        public override int GetHashCode()
        {
            return -528980569 + id.GetHashCode();
        }

        public override string ToString()
        {
            return "Name: " + name;
        }
    }

    partial class item
    {
        public item(string name, decimal? price, string note)
        {
            this.name = name;
            this.price = price;
            this.note = note;
        }

        public override bool Equals(object obj)
        {
            var item = obj as item;
            return item != null &&
                  item.id == id;
        }

        public override int GetHashCode()
        {
            return -528980569 + id.GetHashCode();
        }

        public override string ToString()
        {
            return "Name: " + name +
                   " Price: " + price;
        }
    }

    partial class item_list
    {
        public item_list(int item_id, int quantity)
        {
            this.item_id = item_id;
            item = item.SelectById(item_id);
            this.quantity = quantity;
        }

        public item_list(int item_id, int quantity, long term_id)
        {
            this.item_id = item_id;
            item = item.SelectById(item_id);
            this.quantity = quantity;
            this.term_id = term_id;
        }

        public override bool Equals(object obj)
        {
            var item_list = obj as item_list;
            return item_list != null &&
                  item_list.item_id == item_id && item_list.term_id == term_id;
        }

        public override int GetHashCode()
        {
            return -528980569 + item_id.GetHashCode() + term_id.GetHashCode();
        }

        public override string ToString()
        {
            return "Item: " + item_id +
                   " Term: " + term_id +
                   " Amount: " + quantity;
        }
    }

    partial class price_list
    {
        public price_list(float price_hour, TimeSpan? time_from, TimeSpan? time_to, string day)
        {
            this.price_hour = price_hour;
            this.time_from = time_from;
            this.time_to = time_to;
            this.day = day;
        }

        public override bool Equals(object obj)
        {
            var price_list = obj as price_list;
            return price_list != null &&
                  price_list.id == id;
        }

        public override int GetHashCode()
        {
            return -528980569 + id.GetHashCode();
        }

        public override string ToString()
        {
            return "Hall: " + hall_id +
                   " Time from: " + time_from +
                   " Time to: " + time_to +
                   " Price per hour: " + price_hour +
                   " Day: " + day;
        }
    }

    partial class receipt 
    {
        public receipt(long user_id, long client_id, long reservation_id, DateTime? payment_date, decimal amount, decimal? discount, bool installments, int installment_number, long? receipt_id)
        {
            this.user_id = user_id;
            this.client_id = client_id;
            this.reservation_id = reservation_id;
            this.payment_date = payment_date;
            this.amount = amount;
            this.discount = discount;
            this.installments = installments;
            this.receipt_id = receipt_id;
            paid = installment_number == 1 ? true : false;
            serial_number = GetSerialNumber(installment_number);
        }

        private string GetSerialNumber(int installment_number)
        {
            string date = "";
            if(receipt_id == null)
            {
                date = DateTime.Now.ToString("ddMMyyyy");
            } else if(payment_date != null)
            {
                date = payment_date.Value.ToString("ddMMyyyy");
            } else if(receipt_id != null && payment_date == null)
            {
                date = DateTime.Today.ToString("ddMMyyyy");
            }
            return serial_number = user_id.ToString() + client_id + reservation_id + installment_number + date;
        }

        public override bool Equals(object obj)
        {
            var receipt = obj as receipt;
            return receipt != null &&
                  receipt.id == id;
        }

        public override int GetHashCode()
        {
            return -528980569 + id.GetHashCode();
        }

        public override string ToString()
        {
            return "Client: " + client_id +
                   " Payment date: " + payment_date +
                   " Installments: " + installments +
                   " Amount: " + amount +
                   " Discount: " + discount;
        }
    }

    partial class reservation
    {
        public reservation(long user_id, long client_id, long? event_id, decimal? event_price, bool canceled)
        {
            this.user_id = user_id;
            this.client_id = client_id;
            this.event_id = event_id;
            this.event_price = event_price;
            this.canceled = canceled;
        }

        public reservation(long user_id, long client_id, long? event_id, decimal? event_price, ICollection<term> terms, bool canceled)
        {
            this.user_id = user_id;
            this.client_id = client_id;
            this.event_id = event_id;
            this.event_price = event_price;
            this.terms = terms;
            this.canceled = canceled;
        }

        public override bool Equals(object obj)
        {
            var reservation = obj as reservation;
            return reservation != null &&
                  reservation.id == id;
        }

        public override int GetHashCode()
        {
            return -528980569 + id.GetHashCode();
        }

        public override string ToString()
        {
            return "Client: " + client_id +
                   " Canceled: " + canceled;          
        }
    }

    partial class term
    {
        public term(long hall_id, DateTime rental_date, TimeSpan rent_time_start, TimeSpan rent_time_end,  string note)
        {
            this.hall_id = hall_id;
            this.rental_date = rental_date;
            this.rent_time_start = rent_time_start;
            this.rent_time_end = rent_time_end;
            this.note = note;
        }

        public term(long hall_id, DateTime rental_date, TimeSpan rent_time_start, TimeSpan rent_time_end, List<item_list> items, string note)
        {
            this.hall_id = hall_id;
            hall = hall.SelectById(hall_id);
            this.rental_date = rental_date;
            this.rent_time_start = rent_time_start;
            this.rent_time_end = rent_time_end;
            this.note = note;
            this.items = items;
        }

        public override bool Equals(object obj)
        {
            var term = obj as term;
            return term != null &&
                  term.hall_id == hall_id && term.rental_date.Equals(rental_date) && term.rent_time_start.Equals(rent_time_start) && term.rent_time_end.Equals(rent_time_end);
        }

        public override int GetHashCode()
        {
            return -528980569 + hall_id.GetHashCode() + rental_date.GetHashCode() + rent_time_start.GetHashCode() + rent_time_end.GetHashCode();
        }

        public override string ToString()
        {
            return rental_date.ToString("dd.MM.yyyy");
        }
    }

    partial class user
    {
        public user(string first_name, string last_name, string username, string password, UserType type)
        {
            this.first_name = first_name;
            this.last_name = last_name;
            this.username = username;
            this.password = password;
            this.type = type.ToString();
        }

        public override bool Equals(object obj)
        {
            var user = obj as user;
            return user != null &&
                  user.id == id;
        }

        public override int GetHashCode()
        {
            return -528980569 + id.GetHashCode();
        }

        public override string ToString()
        {
            return first_name + " " + last_name;
        }
    }
}
