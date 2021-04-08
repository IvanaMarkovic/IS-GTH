using GlobalThinkersHelper.Model.Entities;
using GlobalThinkersHelper.Validation;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GlobalThinkersHelper
{
    /// <summary>
    /// Interaction logic for CreateReservation.xaml
    /// </summary>
    public partial class CreateReservation : UserControl
    {
        public static Dictionary<long, string> Clients { get; set; }
        public static Dictionary<long, string> Events { get; set; }
        private bool ChangeItem { get; set; }
        private bool ChangeTerm { get; set; }
        Dictionary<long, string> clients = new Dictionary<long, string>();
        Dictionary<long, string> events = new Dictionary<long, string>();
        Dictionary<long, string> halls = new Dictionary<long, string>();
        Dictionary<int, string> items = new Dictionary<int, string>();
        Dictionary<int, int> itemList = new Dictionary<int, int>();
        int termIndex = 0;
        ObservableCollection<term> terms;

        public CreateReservation()
        {
            InitializeComponent();
            DataContext = new reservation();

            client.SelectAll().ForEach(c => clients.Add(c.id, c.first_name + " " + c.last_name));
            atbClientSearch.AutoCompleteSource = clients.Values;

            _event.SelectAll().ForEach(e => events.Add(e.id, e.name));
            atbEventSearch.AutoCompleteSource = events.Values;

            hall.SelectAll().ForEach(h => halls.Add(h.id, h.name));
            atbHallSearch.AutoCompleteSource = halls.Values;

            item.SelectAll().ForEach(i => items.Add(i.id, i.name));
            atbItemSearch.AutoCompleteSource = items.Values;
        }

        private void tbClientSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            tbClientSearch.SelectAll();
            if (tbClientSearch.Text.Length > 0)
            {
                atbClientSearch.Select(tbClientSearch.Text.Length, 0);
                atbClientSearch.Focusable = true;
                atbClientSearch.Focus();
            }
        }
        private void atbClientSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                atbClientSearch.Focusable = false;
                tbClientSearch.Focus();
            }
        }

        private void atbClientSearch_SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = atbClientSearch.SelectedItem as string;
            if (obj != null)
            {
                client newClient = client.SelectById(clients.FirstOrDefault(c => c.Value.Equals(obj)).Key);
            }
        }

        private void tbEventSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            tbEventSearch.SelectAll();
            if (tbEventSearch.Text.Length > 0)
            {
                atbEventSearch.Select(tbEventSearch.Text.Length, 0);
                atbEventSearch.Focusable = true;
                atbEventSearch.Focus();
            }
        }
        private void atbEventSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                atbEventSearch.Focusable = false;
                tbEventSearch.Focus();
            }
        }

        private void atbEventSearch_SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = atbEventSearch.SelectedItem as string;
            if (obj != null)
            {
                _event newEvent = _event.SelectById(events.FirstOrDefault(ev => ev.Value.Equals(obj)).Key);
                tbEventPrice.Text = newEvent.event_price.ToString();
            }
        }

        private void tbHallSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            tbHallSearch.SelectAll();
            if (tbHallSearch.Text.Length > 0)
            {
                atbHallSearch.Select(tbHallSearch.Text.Length, 0);
                atbHallSearch.Focusable = true;
                atbHallSearch.Focus();
            }
        }

        private void atbHallSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                atbHallSearch.Focusable = false;
                tbHallSearch.Focus();
            }
        }

        private void atbHallSearch_SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = atbHallSearch.SelectedItem as string;
            if (obj != null)
            {
                hall newHall = hall.SelectById(halls.FirstOrDefault(h => h.Value.Equals(obj)).Key);
            }
        }

        private void txtCalendar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (calendar.Visibility == Visibility.Collapsed)
            {
                calendar.Visibility = Visibility.Visible;
            }
            else
            {
                calendar.Visibility = Visibility.Collapsed;
            }
        }

        private void txtCalendar_GotFocus(object sender, RoutedEventArgs e)
        {
            calendar.Visibility = Visibility.Visible;
        }

        private void calendar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                calendar.Visibility = Visibility.Collapsed;
            }
        }

        private void calendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            txtCalendar.Text = calendar.SelectedDate.Value.Date.ToShortDateString();
            calendar.Visibility = Visibility.Collapsed;
        }

        private void TpTimeUntil_GotFocus(object sender, RoutedEventArgs e)
        {
            tpTimeFrom.Visibility = Visibility.Visible;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Color color = (Color)ColorConverter.ConvertFromString("#FF4CAF50");
            note.Foreground = new System.Windows.Media.SolidColorBrush(color);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Color color = (Color)ColorConverter.ConvertFromString("#DD909090");
            note.Foreground = new System.Windows.Media.SolidColorBrush(color);
        }

        private void tbItemSearch_GotFocus(object sender, TextChangedEventArgs e)
        {
            tbItemSearch.SelectAll();
            if (tbItemSearch.Text.Length > 0)
            {
                atbItemSearch.Select(tbItemSearch.Text.Length, 0);
                atbItemSearch.Focusable = true;
                atbItemSearch.Focus();
            }
        }

        private void atbItemSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                atbItemSearch.Focusable = false;
                tbItemSearch.Focus();
            }
        }

        private void atbItemSearch_SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = atbItemSearch.SelectedItem as string;
            if (obj != null)
            {
                item newItem = item.SelectById(items.FirstOrDefault(h => h.Value.Equals(obj)).Key);
            }
        }

        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Add_Item_Click(object sender, RoutedEventArgs e)
        {
            BindingExpression itemQuantity = tbItemCount.GetBindingExpression(TextBox.TextProperty);
            itemQuantity.UpdateSource();
            if (!itemQuantity.HasValidationError)
            {
                if (ChangeItem)
                {
                    item_list sel_item = (item_list)itemDatagrid.SelectedItem;
                    int index = itemDatagrid.Items.IndexOf(sel_item);
                    int quantity2;
                    int.TryParse(tbItemCount.Text, out quantity2);
                    sel_item.quantity = quantity2;
                    itemDatagrid.Items.RemoveAt(index);
                    itemDatagrid.Items.Insert(index, sel_item);
                    ChangeItem = false;
                    tbItemSearch.Text = "";
                    tbItemCount.Text = "";
                    return;
                }
                var obj = atbItemSearch.SelectedItem as string;
                int quantity;
                int.TryParse(tbItemCount.Text, out quantity);
                item newItem = null;
                if (obj != null)
                {
                    newItem = item.SelectById(items.FirstOrDefault(h => h.Value.Equals(obj)).Key);
                }
                if (newItem != null)
                {
                    var newItemList = EntityFactory.CreateItemList(newItem.id, quantity);
                    itemDatagrid.ItemsSource = null;
                    itemDatagrid.Items.Add(newItemList);
                    tbItemSearch.Text = "";
                    tbItemCount.Text = "";
                }
            }

        }

        private void AddError(BindingExpression bindingExpression, string message)
        {
            ValidationError newError = new ValidationError(new TimeValidationRule(), bindingExpression);
            newError.ErrorContent = message;
            System.Windows.Controls.Validation.MarkInvalid(bindingExpression, newError);
        }

        private void RemoveError(BindingExpression bindingExpression)
        {
            System.Windows.Controls.Validation.ClearInvalid(bindingExpression);
        }

        private void Add_Term_Click(object sender, RoutedEventArgs e)
        {
            BindingExpression timeFromV = tpTimeFrom.GetBindingExpression(TimePicker.TextProperty);
            BindingExpression timeUntilV = tpTimeUntil.GetBindingExpression(TimePicker.TextProperty);

            if (tpTimeFrom.SelectedTime == null)
            {
                AddError(timeFromV, "Morate unijeti vrijeme pocetka");
                return;
            }
            if (tpTimeUntil.SelectedTime == null)
            {
                AddError(timeUntilV, "Morate unijeti vrijeme kraja");
                return;
            }

            RemoveError(timeFromV);
            RemoveError(timeUntilV);

            var obj = atbHallSearch.SelectedItem as string;
            var dates = calendar.SelectedDates;
            var note = notes.Text;
            var timeFrom = (DateTime)tpTimeFrom.SelectedTime;
            var timeUntil = (DateTime)tpTimeUntil.SelectedTime;

            if (timeFrom > timeUntil)
            {
                AddError(timeUntilV, "Morate unijeti validno vrijeme kraja");
                return;
            }
            else
            {
                RemoveError(timeUntilV);
            }




            TimeSpan timeFromTS = new TimeSpan(timeFrom.Hour, timeFrom.Minute, timeFrom.Second);
            TimeSpan timeUntilTS = new TimeSpan(timeUntil.Hour, timeUntil.Minute, timeUntil.Second);
            hall newHall = null;
            if (obj != null)
            {
                newHall = hall.SelectById(halls.FirstOrDefault(h => h.Value.Equals(obj)).Key);
            }
            if (newHall != null)
            {
                if (dates != null)
                {
                    if (ChangeTerm)
                    {
                        term selected = (term)termDatagrid.SelectedItem;
                        int index = termIndex;
                        var newTerm = new term(newHall.id, (DateTime)calendar.SelectedDate, timeFromTS, timeUntilTS, itemDatagrid.Items.Cast<item_list>().ToList(), note);
                        if (term.SelectAll().Where(t => t.hall_id == ((term)newTerm).hall_id && t.rental_date.CompareTo(((term)newTerm).rental_date) == 0
                                                        && t.rent_time_end.CompareTo(((term)newTerm).rent_time_start) == 1).ToList().Count > 0)
                        {
                            newTerm.status = true;
                        }
                        terms[terms.IndexOf(selected)] = newTerm;
                        termDatagrid.Items.Refresh();
                        ChangeTerm = false;

                    } else
                    {
                        terms = new ObservableCollection<term>();
                        dates.ToList().ForEach(d => {
                            List<item_list> items = new List<item_list>();
                            itemDatagrid.Items.Cast<item_list>().ToList().ForEach(i => items.Add(new item_list(i.item_id, i.quantity)));
                            var newTerm = new term(newHall.id, d, timeFromTS, timeUntilTS, items, note);
                            if (term.SelectAll().Where(t => t.hall_id == ((term)newTerm).hall_id && t.rental_date.CompareTo(((term)newTerm).rental_date) == 0
                                                            && t.rent_time_end.CompareTo(((term)newTerm).rent_time_start) == 1).ToList().Count > 0)
                            {
                                newTerm.status = true;
                                terms.Insert(0, newTerm);
                            }
                            else
                            {
                                terms.Add(newTerm);
                            }
                        });
                        termDatagrid.ItemsSource = terms;
                        expander.IsExpanded = false;
                    }
                    if (terms.Count > 0 && terms.Where(t => t.status == true).ToList().Count > 0)
                    {
                        lbTermError.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        lbTermError.Visibility = Visibility.Hidden;
                    }
                    itemDatagrid.Items.Clear();
                    tbHallSearch.Text = "";
                    txtCalendar.Text = "Odaberite datum";
                    tpTimeFrom.SelectedTime = null;
                    tpTimeUntil.SelectedTime = null;
                    notes.Text = "";
                }

            }


        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            List<Boolean> validationResults = new List<Boolean>();
            BindingExpression eventPrice = tbEventPrice.GetBindingExpression(TextBox.TextProperty);
            BindingExpression clientName = tbClientSearch.GetBindingExpression(TextBox.TextProperty);

            eventPrice.UpdateSource();

            validationResults.Add(eventPrice.HasValidationError);
            if (!validationResults.Contains(true))
            {
                client selectedClient = null;
                _event selectedEvent = null;
                decimal? selectedPrice = null;

                var obj1 = atbClientSearch.SelectedItem as string;
                if (obj1 != null)  
                {
                    selectedClient = client.SelectById(clients.FirstOrDefault(c => c.Value.Equals(obj1)).Key);
                }
                var obj2 = atbEventSearch.SelectedItem as string;
                if (obj2 != null)
                {
                    selectedEvent = _event.SelectById(events.FirstOrDefault(c => c.Value.Equals(obj2)).Key);
                }
                var obj3 = tbEventPrice.Text as string;
                if (obj3 != null && !obj3.Equals(""))
                {
                    selectedPrice = Convert.ToDecimal(obj3, CultureInfo.CurrentCulture);
                }

                if (obj1 != null && obj2 != null)
                { 
                    if (lbTermError.Visibility == Visibility.Hidden)
                    {
                        lbTermError.Visibility = Visibility.Hidden;
                        lbReservationError.Visibility = Visibility.Collapsed;
                        reservation newReservation = EntityFactory.CreateReservation(EntityFactory.User.id, selectedClient.id, selectedEvent.id, selectedPrice, false);
                        reservation.Insert(newReservation);
                        var terms = termDatagrid.Items.Cast<term>().ToList();
                        terms.ForEach(t =>
                        {
                            t.reservation = newReservation;
                            t.items.ToList().ForEach(i => i.term = t);
                            term.Insert(t);

                        });
                        ClearAllFields();
                    }
                    else
                    {
                        lbReservationError.Visibility = Visibility.Visible;
                    }
                }

            }
        }

        private void ClearAllFields()
        {
            tbClientSearch.Clear();
            tbEventSearch.Clear();
            tbEventPrice.Clear();
            tbHallSearch.Clear();
            txtCalendar.Text = "Odaberite datum";
            tpTimeFrom.SelectedTime = null;
            tpTimeUntil.SelectedTime = null;
            tbItemSearch.Clear();
            tbItemCount.Clear();
            itemDatagrid.Items.Clear();
            terms = null;
            termDatagrid.ItemsSource = terms;
            expander.IsExpanded = false;
        }

        public void Update_Reservation()
        {
            reservation r = EntityFactory.Reservation;
            term term = EntityFactory.Term;
            if (r != null && term != null)
            {
                tbClientSearch.Text = r.client.ToString();
                tbEventSearch.Text = r._event.name;
                tbEventPrice.DataContext = r;

                tbHallSearch.Text = term.hall.name;
                txtCalendar.Text = term.rental_date.ToShortDateString();
                calendar.SelectedDate = term.rental_date;

                expander.DataContext = term;

                itemDatagrid.Items.Clear();
                foreach (item_list i in term.items)
                {
                    itemDatagrid.Items.Add(i);
                }
                notes.Text = term.note;

                foreach (var t in r.terms)
                {
                    termDatagrid.Items.Add(t);
                }
                ChangeTerm = true;
                expander.IsExpanded = true;
                Keyboard.ClearFocus();
            }
        }

        private void Show_Create_Event_Dialog(object sender, MouseButtonEventArgs e)
        {
            lbDialog.Content = "Kreiranje događaja";
            eventPopup.IsOpen = true;
        }

        private void Show_Create_Item_Dialog(object sender, MouseButtonEventArgs e)
        {
            lbDialog.Content = "Kreiranje stavke";
            eventPopup.IsOpen = true;
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            decimal? priceValue;
            decimal parsedPrice;

            BindingExpression price = tbPrice.GetBindingExpression(TextBox.TextProperty);
            BindingExpression name = tbName.GetBindingExpression(TextBox.TextProperty);

            if (string.IsNullOrEmpty(tbName.Text as string))
            {
                AddError(name, "Polje je obavezno");
                return;
            }
            else
            {
                System.Windows.Controls.Validation.ClearInvalid(name);
            }

            if (string.IsNullOrEmpty(tbPrice.Text as string))
            {
                priceValue = null;
            }

            else if (!decimal.TryParse(tbPrice.Text, out parsedPrice))
            {
                AddError(price, "Vrijednost nije validna");
                return;
            }
            else
            {
                priceValue = parsedPrice;
                System.Windows.Controls.Validation.ClearInvalid(price);
            }

            if (!name.HasValidationError)
            {
                if (lbDialog.Content.Equals("Kreiranje događaja"))
                {
                    tbEventSearch.Text = tbName.Text;
                    tbEventPrice.Text = tbPrice.Text;

                    _event newEvent = new _event(tbName.Text, "edukacija", priceValue);
                    _event.Insert(newEvent);
                    events.Add(newEvent.id, newEvent.name);

                }
                else if (lbDialog.Content.Equals("Kreiranje stavke"))
                {
                    tbItemSearch.Text = tbName.Text;

                    item newItem = new item(tbName.Text, priceValue, null);
                    item.Insert(newItem);
                    items.Add(newItem.id, newItem.name);
                }
                eventPopup.IsOpen = false;
                tbName.Clear();
                tbPrice.Clear();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            tbName.Text = "";
            tbPrice.Text = "";
        }

        private void btnUpdateItem_Click(object sender, RoutedEventArgs e)
        {
            DataGridRow dgr = null;
            var visParent = VisualTreeHelper.GetParent(e.OriginalSource as FrameworkElement);
            while (dgr == null && visParent != null)
            {
                dgr = visParent as DataGridRow;
                visParent = VisualTreeHelper.GetParent(visParent);
            }
            if (dgr == null) { return; }

            var rowIdx = dgr.GetIndex();

            item_list itemToUpdate = (item_list)itemDatagrid.Items.GetItemAt(rowIdx);
            atbItemSearch.SelectedItem = itemToUpdate.item;
            tbItemSearch.Text = itemToUpdate.item.name;
            tbItemCount.Text = itemToUpdate.quantity.ToString();
            ChangeItem = true;
        }

        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            DataGridRow dgr = null;
            var visParent = VisualTreeHelper.GetParent(e.OriginalSource as FrameworkElement);
            while (dgr == null && visParent != null)
            {
                dgr = visParent as DataGridRow;
                visParent = VisualTreeHelper.GetParent(visParent);
            }
            if (dgr == null) { return; }

            var rowIdx = dgr.GetIndex();
            itemDatagrid.Items.RemoveAt(rowIdx);
            tbItemSearch.Text = "";
            tbItemCount.Text = "";
        }

        private void btnUpdateTerm_Click(object sender, RoutedEventArgs e)
        {
            DataGridRow dgr = null;
            var visParent = VisualTreeHelper.GetParent(e.OriginalSource as FrameworkElement);
            while (dgr == null && visParent != null)
            {
                dgr = visParent as DataGridRow;
                visParent = VisualTreeHelper.GetParent(visParent);
            }
            if (dgr == null) { return; }

            var rowIdx = dgr.GetIndex();

            term termToUpdate = (term)termDatagrid.Items.GetItemAt(rowIdx);
            atbHallSearch.SelectedItem = termToUpdate.hall.name;
            calendar.SelectedDate = termToUpdate.rental_date;
            txtCalendar.Text = termToUpdate.rental_date.ToShortDateString();

            tpTimeFrom.Text = termToUpdate.rent_time_start.ToString(@"hh\:mm");
            tpTimeUntil.Text = termToUpdate.rent_time_end.ToString(@"hh\:mm");
            itemDatagrid.Items.Clear();
            foreach (item_list i in termToUpdate.items)
            {
                itemDatagrid.Items.Add(i);
            }
            notes.Text = termToUpdate.note;
            ChangeTerm = true;
            termIndex = rowIdx;

            expander.IsExpanded = true;
        }

        private void btnDeleteTerm_Click(object sender, RoutedEventArgs e)
        {
            DataGridRow dgr = null;
            var visParent = VisualTreeHelper.GetParent(e.OriginalSource as FrameworkElement);
            while (dgr == null && visParent != null)
            {
                dgr = visParent as DataGridRow;
                visParent = VisualTreeHelper.GetParent(visParent);
            }
            if (dgr == null) { return; }

            var rowIdx = dgr.GetIndex();
            termDatagrid.Items.RemoveAt(rowIdx);


            itemDatagrid.Items.Clear();
            tbHallSearch.Text = "";
            txtCalendar.Text = "Odaberite datum";
            tpTimeFrom.SelectedTime = null;
            tpTimeUntil.SelectedTime = null;
            notes.Text = "";
            ChangeTerm = false;
            return;
        }

        private void ClearErrors(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            System.Windows.Controls.Validation.ClearInvalid(tpTimeFrom.GetBindingExpression(TimePicker.TextProperty));
        }

        private void tbEventPrice_GotFocus(object sender, RoutedEventArgs e)
        {
            tbEventPrice.SelectAll();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClearAllFields();
        }

    }
}
