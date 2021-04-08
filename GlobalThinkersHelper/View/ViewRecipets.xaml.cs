using GlobalThinkersHelper.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace GlobalThinkersHelper.View
{
    /// <summary>
    /// Interaction logic for ViewRecipets.xaml
    /// </summary>
    public partial class ViewRecipets : UserControl
    {
        Dictionary<long, string> clients = new Dictionary<long, string>();
        Dictionary<long, string> events = new Dictionary<long, string>();
        DataGridRow currentRow = new DataGridRow();

        public ViewRecipets()
        {
            InitializeComponent();
            receipt.SelectAll().Where(parent => parent.receipt_id == null).ToList().ForEach(r => datagrid.Items.Add(r));
            client.SelectAll().ForEach(c => clients.Add(c.id, c.first_name + " " + c.last_name));
            atbClientSearch.AutoCompleteSource = clients.Values;
            _event.SelectAll().ForEach(e => events.Add(e.id, e.name));
            atbEventSearch.AutoCompleteSource = events.Values;
            cbShowReciept.ItemsSource = new List<string>()
            {
                "Svi računi",
                "Plaćeni računi",
                "Neplaćeni računi"
            };
        }

        private void atbClientSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                atbClientSearch.Focusable = false;
                tbClientSearch.Focus();
            }
        }

        private void tbClientSearch_GotFocus(object sender, TextChangedEventArgs e)
        {
            tbClientSearch.SelectAll();
            if (tbClientSearch.Text.Length > 0)
            {
                atbClientSearch.Select(tbClientSearch.Text.Length, 0);
                atbClientSearch.Focusable = true;
                atbClientSearch.Focus();
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

        private void dpDateFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDateFrom.SelectedDate != null)
            {
                dpDateTo.DisplayDateStart = dpDateFrom.SelectedDate;
            }
            else
            {
                dpDateTo.DisplayDateStart = DateTime.Today;
            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ValidateFilters();
        }

        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            ValidateFilters();
        }

        public void ValidateFilters()
        {
            var showRecipts = cbShowReciept.SelectedIndex;
            var newClient = clients.FirstOrDefault(c => c.Value.Equals(atbClientSearch.Text as string)).Key;
            var newEvent = events.FirstOrDefault(e => e.Value.Equals(atbEventSearch.Text as string)).Key;
            var dateFrom = dpDateFrom.SelectedDate;
            var dateTo = dpDateTo.SelectedDate;
            var filterData = new Predicate<object>(r => (showRecipts == 0 || (showRecipts == 1 && ((receipt)r).paid) || (showRecipts == 2 && !((receipt)r).paid)) && 
                                                        (newClient == 0 || ((receipt)r).client.id == newClient) &&
                                                        (newEvent == 0 || (((receipt)r).reservation._event != null ? ((receipt)r).reservation.event_id == newEvent : false)) &&
                                                        (dateFrom == null || (((receipt)r).payment_date != null && dateFrom != null && ((receipt)r).payment_date.Value.Date.CompareTo(dateFrom) > 0)) &&
                                                        (dateTo == null || (((receipt)r).payment_date != null && dateTo != null && ((receipt)r).payment_date.Value.Date.CompareTo(dateTo) < 0)));
            ICollectionView allTerms = datagrid.Items;
            allTerms.Filter = filterData;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            getRow(e);
            receipt selectedReceipt = datagrid.CurrentItem as receipt;
            EntityFactory.Receipt = selectedReceipt;
            EntityFactory.Reservation = selectedReceipt.reservation;
            EntityFactory.Client = selectedReceipt.client;
            CreateReceipt createReceipt = new CreateReceipt();
            createReceipt.SetAllFields();
            Content = createReceipt;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            getRow(e);
            popupDelete.IsOpen = true;
        }

        public void getRow(RoutedEventArgs e)
        {
            tbDelete.Text = "Da li ste sigurni da želite obrisati podatke o računu?";
            btnAccept.Visibility = btnCancel.Visibility = Visibility.Visible;
            DependencyObject dependencyObject = (DependencyObject)e.OriginalSource;
            while ((dependencyObject != null) && !(dependencyObject is DataGridRow))
            {
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }
            currentRow = dependencyObject as DataGridRow;
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (currentRow != null)
            {
                receipt newReciept = currentRow.Item as receipt;
                datagrid.Items.Remove(newReciept);
                datagrid.Items.Refresh();
                term.Delete(newReciept.id);
                tbDelete.Text = "Uspješno ste obrisali podatke o računu!";
            }
            else
            {
                tbDelete.Text = "Podaci o računu nisu obrisani!";
            }
            btnAccept.Visibility = btnCancel.Visibility = Visibility.Collapsed;
        }

        private void btnReceipt_Click(object sender, RoutedEventArgs e)
        {
            getRow(e);
            var selectedReceipt = currentRow.Item as receipt;
            if (selectedReceipt.paid == true)
            {
                errorPopup.IsOpen = true;
                return;
            }
            errorPopup.IsOpen = false;
        }
    }
}
