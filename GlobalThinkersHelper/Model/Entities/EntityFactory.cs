using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalThinkersHelper.Model.Entities
{
    public class EntityFactory
    {
        public static user User { get; set; }
        public static client Client { get; set; }
        public static hall Hall { get; set; }
        public static reservation Reservation { get; set; }
        public static receipt Receipt { get; set; }
        public static term Term { get; set; }
        public static price_list PriceList { get; set; }

        public static client CreateClient(long user_id, string first_name, string last_name, string company_name, string contact, string note, long[] attends_events)
        {
            return new client(user_id, first_name, last_name, company_name, contact, note, attends_events);
        }

        public static Contact CreateContact(string[] emails, string[] phoneNumbers)
        {
            return new Contact(emails, phoneNumbers);
        }

        public static _event CreateEvent(string name, string type, decimal? event_price)
        {
            return new _event(name, type, event_price);
        }

        public static hall CreateHall(string name, ICollection<price_list> price_lists)
        {
            return new hall(name, price_lists);
        }

        public static item CreateItem(string name, decimal? price, string note)
        {
            return new item(name, price, note);
        }

        public static item_list CreateItemList(int item_id, int quantity)
        {
            return new item_list(item_id, quantity);
        }

        public static price_list CreatePriceList(float price_hour, TimeSpan? time_from, TimeSpan? time_to, string day)
        {
            return new price_list(price_hour, time_from, time_to, day);
        }

        public static receipt CreateReciept(long user_id, long client_id, long reservation_id, DateTime? payment_date, decimal amount, decimal? discount, bool installments, int installment_number, long? receipt_id)
        {
            return new receipt(user_id, client_id, reservation_id, payment_date, amount, discount, installments, installment_number, receipt_id);
        }

        public static reservation CreateReservation(long user_id, long client_id, long? event_id, decimal? event_price, bool canceled)
        {
            return new reservation(user_id, client_id, event_id, event_price, canceled);
        }

        public static term CreateTerm(long hall_id, DateTime rental_date, TimeSpan rent_time_start, TimeSpan rent_time_end, List<item_list> items, string note)
        {
            return new term(hall_id, rental_date, rent_time_start, rent_time_end, items, note);
        }

        public static user CreateUser(string first_name, string last_name, string username, string password, UserType type)
        {
            return new user(first_name, last_name, username, password, type);
        }
    }
}
