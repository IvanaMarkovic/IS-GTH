using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalThinkersHelper.Model.Entities
{
    partial class client : Entity
    {
        public static List<client> SelectAll()
        {
            return context.clients.Where(c => c.status == false).ToList();
        }

        public static client SelectById(long client_id)
        {
            var client = context.clients.Find(client_id);
            return client.status == false ? client : null;
        }

        public static List<client> SelectByName(string first_name, string last_name)
        {
            var client = context.clients.Where(c => c.first_name == first_name && c.last_name == last_name).ToList();
            return client == null ? null : client;
        }

        public static long Insert(client client)
        {
            context.clients.Add(client);
            context.SaveChanges();
            return client.id;
        }

        public static void Update(client new_client, long client_id)
        {
            var client = SelectById(client_id);
            if (client != null)
            {
                client.first_name = new_client.first_name;
                client.last_name = new_client.last_name;
                client.company_name = new_client.company_name;
                client.contact = new_client.contact;
                client.note = new_client.note;
                client.attends_events = new_client.attends_events;
                context.SaveChanges();
            }
        }

        public static void Delete(long client_id)
        {
            var client = context.clients.Find(client_id);
            if (client != null)
            {
                client.status = true;
                context.SaveChanges();
            }
        }
    }

    partial class _event : Entity
    {
        public static List<_event> SelectAll()
        {
            return context.events.Where(e => e.deleted == false).ToList();
        }

        public static _event SelectById(long? event_id)
        {
            var _event = context.events.Find(event_id);
            return _event.deleted == false ? _event : null;
        }

        public static long Insert(_event _event)
        {
            var oldEvent = context.events.Where(e => e.name == _event.name).ToList();
            if(oldEvent != null)
            {
                Update(_event, oldEvent[0].id);
                return oldEvent[0].id;
            }
            context.events.Add(_event);
            return _event.id;
        }

        public static void Update(_event new_event, long? event_id)
        {
            var _event = context.events.Find(event_id);
            if (_event != null)
            {
                _event.name = new_event.name;
                _event.type = new_event.type;
                _event.event_price = new_event.event_price;
                context.SaveChanges();
            }
        }

        public static void Delete(long? event_id)
        {
            var _event = context.events.Find(event_id);
            if (_event != null)
            {
                _event.deleted = true;
                context.SaveChanges();
            }
        }
    }

    partial class hall : Entity
    {
        public static List<hall> SelectAll()
        {
            return context.halls.Where(h => h.status ==  false).ToList();
        }

        public static hall SelectById(long hall_id)
        {
            var hall = context.halls.Find(hall_id);
            return hall.status == false ? hall : null;
        }

        public static long Insert(hall hall)
        {
            context.halls.Add(hall);
            context.SaveChanges();
            return hall.id;
        }

        public static void Update(hall new_hall, long hall_id)
        {
            var hall = context.halls.Find(hall_id);
            if (hall != null && hall.status == false)
            {
                hall.name = new_hall.name;
                foreach (price_list pl in hall.price_lists.ToList())
                {
                    if (!new_hall.price_lists.Any(p => p.id == pl.id))
                    {
                        price_list.Delete(pl.id);
                    }
                }
                foreach (price_list pl in new_hall.price_lists)
                {
                    var new_price_list = hall.price_lists.FirstOrDefault(p => p.id == pl.id);
                    if (new_price_list != null)
                    {
                        price_list.Update(new_price_list, pl.id);
                    }
                    else if (new_price_list == null)
                    {
                        pl.hall = hall;
                        price_list.Insert(pl);
                    }
                }
                context.SaveChanges();
            }
        }

        public static void Delete(long hall_id)
        {
            var hall = context.halls.Find(hall_id);
            if (hall != null)
            {
                hall.status = true;
                context.SaveChanges();
            }
        }
    }

    partial class item : Entity
    {
        public static List<item> SelectAll()
        {
            return context.items.ToList();
        }

        public static item SelectById(int item_id)
        {
            return context.items.Find(item_id);
        }

        public static long Insert(item item)
        {
            context.items.Add(item);
            context.SaveChanges();
            return item.id;
        }

        public static void Update(item new_item, int item_id)
        {
            var item = context.items.Find(item_id);
            if (item != null)
            {
                item.name = new_item.name;
                item.note = new_item.note;
                item.price = new_item.price;
                context.SaveChanges();
            }
        }

        public static void Delete(int item_id)
        {
            var item = context.items.Find(item_id);
            if (item != null)
            {
                context.items.Remove(item);
                context.SaveChanges();
            }
        }
    }

    partial class item_list : Entity
    {
        public static List<item_list> SelectAll()
        {
            return context.item_list.ToList();
        }

        public static item_list SelectById(int item_id, long term_id)
        {
            return (from items in context.item_list where items.item_id == item_id && items.term_id == term_id select items).FirstOrDefault() ;
        }

        public static bool CheckItem(int item_id, term term)
        {
            var items = context.item_list.Where(i => i.item_id == item_id).ToList();
            foreach(item_list item in items)
            {
                var check_term = term.SelectById(term.id);
                if(check_term != null && check_term.rental_date.CompareTo(term.rental_date) == 0 
                    && check_term.rent_time_end.CompareTo(term.rent_time_start) == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public static void Insert(item_list item_list)
        {
            context.item_list.Add(item_list);
            context.SaveChanges();
        }

        public static void Update(item_list item_list, long term_id)
        {
            var list = SelectById(item_list.item_id, term_id);
            if (list != null)
            {
                list.quantity = item_list.quantity;
                context.SaveChanges();
            }
        }
    }

    partial class price_list : Entity
    {
        public static List<price_list> SelectAll()
        {
            return context.price_list.Where(pl => pl.status == false).ToList();
        }

        public static price_list SelectById(long price_list_id)
        {
            var price_list = context.price_list.Find(price_list_id);
            return price_list.status == false ? price_list : null;
        }

        public static void Insert(price_list price_list)
        {
            context.price_list.Add(price_list);
            context.SaveChanges();
        }

        public static void Update(price_list new_price_list, long price_list_id)
        {
            var price_list = SelectById(price_list_id);
            if (price_list != null)
            {
                price_list.hall_id = new_price_list.hall_id;
                price_list.hall = new_price_list.hall;
                price_list.price_hour = new_price_list.price_hour;
                price_list.time_from = new_price_list.time_from;
                price_list.time_to = new_price_list.time_to;
                context.SaveChanges();
            }
        }

        public static void Delete(long price_list_id)
        {
            var price_list = SelectById(price_list_id);
            if (price_list != null)
            {
                price_list.status = true;
                context.SaveChanges();
            }
        }
    }

    partial class receipt : Entity
    {
        public static List<receipt> SelectAll()
        {
            return context.receipts.Where(r => r.status == false).ToList();
        }

        public static receipt SelectById(long? receipt_id)
        {
            var receipt = context.receipts.Find(receipt_id);
            return receipt.status == false ? receipt : null;
        }

        public static List<receipt> SelectByDate(DateTime date_from, DateTime date_to)
        {
            List<receipt> receipts = new List<receipt>();
            if (date_from != null && date_to != null)
            {
                receipts = (from r in context.receipts where r.payment_date >= date_from && r.payment_date <= date_to select r).ToList();
            }
            else if (date_from != null)
            {
                receipts = (from r in context.receipts where r.payment_date >= date_from select r).ToList();
            }
            else if (date_to != null)
            {
                receipts = (from r in context.receipts where r.payment_date <= date_to select r).ToList();
            }
            return receipts;
        }

        public static string GetPaymentDate(long? receipt_id)
        {
            var receipts = (from r in context.receipts where r.receipt_id == receipt_id select r).ToList();
            return receipts.Count > 0 ? receipts[0].payment_date.Value.ToString("ddMMyyyy") : "";
        }

        public static void Insert(receipt receipt)
        {
            context.receipts.Add(receipt);
            context.SaveChanges();
        }

        public static void Update(receipt new_receipt, long receipt_id)
        {
            var receipt = SelectById(receipt_id);
            if (receipt != null)
            {
                receipt.payment_date = new_receipt.payment_date;
                receipt.amount = new_receipt.amount;
                receipt.discount = new_receipt.discount;
                receipt.paid = new_receipt.paid;
                context.SaveChanges();
            }
        }

        public static void Delete(long receipt_id)
        {
            var receipt = SelectById(receipt_id);
            if (receipt != null)
            {
                receipt.status = true;
                context.SaveChanges();
            }
        }
    }

    partial class reservation : Entity
    {
        public static List<reservation> SelectAll()
        {
            return context.reservations.Where(r => r.status == false).ToList();
        }

        public static reservation SelectById(long reservation_id)
        {
            var reservation = context.reservations.Find(reservation_id);
            return reservation != null && reservation.status == false ? reservation : null;
        }

        public static void Insert(reservation reservation)
        {
            context.reservations.Add(reservation);
            context.SaveChanges();
        }

        public static void Update(reservation new_reservation, long reservation_id)
        {
            var reservation = SelectById(reservation_id);
            if (reservation != null)
            {

                reservation.user_id = new_reservation.user_id;
                reservation.event_id = new_reservation.event_id;
                reservation.event_price = new_reservation.event_price;
                reservation.canceled = new_reservation.canceled;
                var terms = reservation.terms.ToList();
                foreach (term term in reservation.terms)
                {
                    if (!new_reservation.terms.Any(t => t.id == term.id && term.status == false))
                    {
                        term.Delete(term.id);
                    }
                }
                foreach (term term in new_reservation.terms)
                {
                    var new_term = reservation.terms.FirstOrDefault(t => t.id == term.id);
                    if (new_term != null)
                    {
                        term.Update(new_term, term.id);
                    }
                    else
                    {
                        term.reservation_id = reservation.id;
                        term.Insert(term);
                    }
                }
                context.SaveChanges();
            }
        }

        public static void Delete(long reservation_id)
        {
            var reservation = SelectById(reservation_id);
            if (reservation != null)
            {
                reservation.status = true;
                reservation.terms.ToList().ForEach(t => t.status = true);
                context.SaveChanges();
            }
        }
    }

    partial class term : Entity
    {
        public static List<term> SelectAll()
        {
            return context.terms.Where(t => t.status == false).ToList();
        }

        public static term SelectById(long term_id)
        {
            var term = context.terms.Find(term_id);
            return term.status == false ? term : null;
        }

        public static List<term> SelectByDate(DateTime date_from, DateTime date_to)
        {
            List<term> terms = new List<term>();
            if(date_from != null && date_to != null)
            {
                terms = (from t in context.terms where t.rental_date >= date_from && t.rental_date <= date_to select t).ToList();
            } else if(date_from != null)
            {
                terms = (from t in context.terms where t.rental_date >= date_from select t).ToList();
            } else if(date_to != null)
            {
                terms = (from t in context.terms where t.rental_date <= date_to select t).ToList();
            }
            return terms;
        }

        public static void Insert(term term)
        {
            context.terms.Add(term);
            context.SaveChanges();
        }

        public static void Update(term new_term, long term_id)
        {
            var term = SelectById(term_id);
            new_term.id = term_id;
            if (term != null)
            {
                term.hall_id = new_term.hall_id;
                term.rental_date = new_term.rental_date;
                term.rent_time_start = new_term.rent_time_start;
                term.rent_time_end = new_term.rent_time_end;
                term.note = new_term.note;
                foreach (item_list item in term.items.ToList())
                {
                    if (!new_term.items.Any(i => i.item_id == item.item_id))
                    {
                        context.item_list.Remove(item);
                    }
                }
                foreach (item_list item in new_term.items)
                {
                    var new_item = term.items.FirstOrDefault(i => i.item_id == item.item_id);
                    if (new_item != null && item.quantity != new_item.quantity)
                    {
                        item_list.Update(new_item, item.term_id);
                    }
                    else if (new_item == null)
                    {
                        item.term_id = term.id;
                        item_list.Insert(item);
                    }
                }
                context.SaveChanges();
            }
        }

        public static void Delete(long term_id)
        {
            var term = SelectById(term_id);
            if (term != null)
            {
                term.status = true;
                term.items.ToList().ForEach(i => context.item_list.Remove(i));
                context.SaveChanges();
            }
        }
    }

    partial class user : Entity
    {
        public static List<user> SelectAll()
        {
            return context.users.Where(u => u.status == false).ToList();
        }

        public static user SelectById(long user_id)
        {
            var user = context.users.Find(user_id);
            return user != null && user.status == false ? user : null;
        }

        public static user SelectByUsername(string username)
        {
            var user = context.users.Where(u => u.username == username).FirstOrDefault();
            return user == null ? null : user;
        }

        public static void Insert(user user)
        {
            var oldUser = (from old in context.users where old.username == user.username && old.status == true select old).FirstOrDefault();
            if(oldUser == null)
            {
                context.users.Add(user);
            } else
            {
                Update(user, oldUser.id);
            }
            context.SaveChanges();
        }

        public static void Update(user new_user, long user_id)
        {
            var user = context.users.Find(user_id);
            if (user != null)
            {
                user.first_name = new_user.first_name;
                user.last_name = new_user.last_name;
                user.username = new_user.username;
                context.SaveChanges();
            }
        }

        public async static void Delete(long user_id)
        {
            var user = context.users.Find(user_id);
            if (user != null)
            {
                user.status = true;
                await context.SaveChangesAsync();
            }
        }
    }
}
