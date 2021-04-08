using GlobalThinkersHelper.Model.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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

namespace GlobalThinkersHelper.View
{
    /// <summary>
    /// Interaction logic for CreateClient.xaml
    /// </summary>
    public partial class CreateClient : UserControl
    {
        Dictionary<long, string> reservations = new Dictionary<long, string>();
        Dictionary<long, string> clients = new Dictionary<long, string>();
        public CreateClient()
        {
            InitializeComponent();
            DataContext = new client();
            var res = reservation.SelectAll();
            reservation.SelectAll().Where(r => r._event != null && r.terms.Count > 0).ToList().ForEach(r => reservations.Add(r.id, r._event.name + " - " + r.terms.First().rental_date.ToString("dd/MM/yyyy")));
            autocomplete.AutoCompleteSource = reservations.Values;
            client.SelectAll().ForEach(c => clients.Add(c.id, c.first_name + " " + c.last_name));
            autocompleteSearch.AutoCompleteSource = clients.Values;

        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            BindingExpression bindingExprContact = contact.GetBindingExpression(TextBox.TextProperty);
            bindingExprContact.UpdateSource();
            string contact1 = contact.Text;
            int counter = 0;
            if (!bindingExprContact.HasValidationError)
            {
                counter = 0;
                foreach (var item in contacts.Items)
                {
                    if (item.Equals(contact1))
                    {
                        counter++;
                    }
                }
                if (counter == 0)
                {
                    contacts.Items.Add(contact1);
                    contacts.SelectedIndex = 0;
                    contact.Clear();
                    email.Clear();
                }
            }
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

        private void Image_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            contacts.Items.Remove(contacts.SelectedItem);
        }

        private void Image_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {
            string email1 = email.Text;
            BindingExpression bindingExprEmail = email.GetBindingExpression(TextBox.TextProperty);
            bindingExprEmail.UpdateSource();
            int counter = 0;
            if (!bindingExprEmail.HasValidationError)
            {
                counter = 0;
                foreach (var item in emails.Items)
                {
                    if (item.Equals(email1))
                    {
                        counter++;
                    }
                }
                if (counter == 0)
                {
                    emails.Items.Add(email1);
                    emails.SelectedIndex = 0;
                    email.Clear();
                    contact.Clear();
                }
            }
        }

        private void Image_MouseLeftButtonUp_3(object sender, MouseButtonEventArgs e)
        {
            emails.Items.Remove(emails.SelectedItem);
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            List<Boolean> list = new List<Boolean>();
            BindingExpression bindingExprFirstName = firstName.GetBindingExpression(TextBox.TextProperty);
            BindingExpression bindingExprLastName = lastName.GetBindingExpression(TextBox.TextProperty);
            bindingExprFirstName.UpdateSource();
            bindingExprLastName.UpdateSource();
            list.Add(bindingExprFirstName.HasValidationError);
            list.Add(bindingExprLastName.HasValidationError);
            if (!list.Contains(true))
            {
                var emailsArray = emails.Items.Cast<Object>().Select(item => item.ToString()).ToArray();
                var phoneArray = contacts.Items.Cast<Object>().Select(item => item.ToString()).ToArray();
                Contact contact = EntityFactory.CreateContact(emailsArray, phoneArray);
                long[] attends_events = new long[events.Items.Count];
                for (int i = 0; i < events.Items.Count; i++)
                {
                    attends_events[i] = reservations.FirstOrDefault(r => r.Value.Equals(events.Items.GetItemAt(i))).Key;
                }
                client client = EntityFactory.CreateClient(EntityFactory.User.id, firstName.Text, lastName.Text, companyName.Text, JsonConvert.SerializeObject(contact), notes.Text, attends_events);
                if (search.Visibility.Equals(Visibility.Hidden))
                {
                    client.Insert(client);
                } else
                {
                    client.Update(client, EntityFactory.Client.id);
                } 
                DataContext = new client();
                firstName.Text = "";
                lastName.Text = "";
                companyName.Text = "";
                contacts.Items.Clear();
                emails.Items.Clear();
                events.Items.Clear();
            }
        }

        private void tbSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            tbSearch.SelectAll();
            if (tbSearch.Text.Length > 0)
            {
                autocomplete.Select(tbSearch.Text.Length, 0);
                autocomplete.Focusable = true;
                autocomplete.Focus();
            }
        }


        private void autocomplete_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                autocomplete.Focusable = false;
            }
        }

        private void Image_MouseLeftButtonUp_4(object sender, MouseButtonEventArgs e)
        {
            string event1 = tbSearch.Text;
            int counter = 0;
            foreach (var item in events.Items)
            {
                if (item.Equals(event1))
                {
                    counter++;
                }
            }
            if (counter == 0)
            {
                events.Items.Add(event1);
                events.SelectedIndex = 0;
                tbSearch.Clear();
            }
        }

        private void Image_MouseLeftButtonUp_5(object sender, MouseButtonEventArgs e)
        {
            events.Items.Remove(events.SelectedItem);
        }

        private void Contact_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Image_MouseLeftButtonUp(sender, null);
            }
        }

        private void Email_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Image_MouseLeftButtonUp_2(sender, null);
            }
        }

        private void TbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Image_MouseLeftButtonUp_4(sender, null);
                Keyboard.ClearFocus();
            }
        }

        private void search_GotFocus(object sender, TextChangedEventArgs e)
        {
            search.SelectAll();
            if (search.Text.Length > 0)
            {
                autocompleteSearch.Select(search.Text.Length, 0);
                autocompleteSearch.Focusable = true;
                autocompleteSearch.Focus();
            }
        }

        private void Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                autocompleteSearch.Focusable = false;
            }
        }

        public void ChangeToUpdateClient(bool isEdit)
        {
            autocompleteSearch.Visibility = Visibility.Visible;
            search.Visibility = Visibility.Visible;
            searchImg.Visibility = Visibility.Visible;
            title.Content = "Izmjena klijenta";
            if (isEdit)
            {
                setFields();
            }
        }

        

        private void ClientSearch_SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = autocompleteSearch.SelectedItem as string;
            emails.Items.Clear();
            contacts.Items.Clear();
            if (obj != null)
            {
                client newClient = client.SelectById(clients.FirstOrDefault(c => c.Value.Equals(obj)).Key);
                EntityFactory.Client = newClient;
                setFields();
            }
        }

        private void setFields()
        {
            DataContext = EntityFactory.Client;
            Contact con = JsonConvert.DeserializeObject<Contact>(EntityFactory.Client.contact);
            FillComboBox(con.Emails, emails);
            FillComboBox(con.PhoneNumbers, contacts);
            EntityFactory.Client.contact = "";
            EntityFactory.Client.attends_events.ToList().ForEach(r => events.Items.Add(r._event.name + " - " + r.terms.First().rental_date.ToString("dd/MM/yyyy")));
            contact.Text = "";
            email.Text = "";
            events.SelectedIndex = -1;
        }

        private void FillComboBox(string[] elements, ComboBox cb)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                cb.Items.Add(elements[i]);
            }
            cb.SelectedIndex = -1;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new client();
            contacts.Items.Clear();
            emails.Items.Clear();
            events.Items.Clear();
        }
    }
}
