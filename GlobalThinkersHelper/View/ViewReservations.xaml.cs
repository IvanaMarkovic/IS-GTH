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
    /// Interaction logic for ViewReservations.xaml
    /// </summary>
    public partial class ViewReservations : UserControl
    {
        Dictionary<long, string> clients = new Dictionary<long, string>();
        Dictionary<long, string> halls = new Dictionary<long, string>();
        Dictionary<long, string> events = new Dictionary<long, string>();
        DataGridRow currentRow = new DataGridRow();

        public ViewReservations()
        {
            InitializeComponent();

            term.SelectAll().ForEach(t => datagrid.Items.Add(t));
            client.SelectAll().ForEach(c => clients.Add(c.id, c.first_name + " " + c.last_name));
            atbClientSearch.AutoCompleteSource = clients.Values;
            hall.SelectAll().ForEach(h => halls.Add(h.id, h.name));
            atbHallSearch.AutoCompleteSource = halls.Values;
            _event.SelectAll().ForEach(e => events.Add(e.id, e.name));
            atbEventSearch.AutoCompleteSource = events.Values;
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
       
        private void atbHallSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                atbHallSearch.Focusable = false;
                tbEventSearch.Focus();
            }
        }

        private void tbHallSearch_GotFocus(object sender, TextChangedEventArgs e)
        {
            tbHallSearch.SelectAll();
            if (tbHallSearch.Text.Length > 0)
            {
                atbHallSearch.Select(tbHallSearch.Text.Length, 0);
                atbHallSearch.Focusable = true;
                atbHallSearch.Focus();
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
            if(dpDateFrom.SelectedDate != null)
            {
                dpDateTo.DisplayDateStart = dpDateFrom.SelectedDate;
            } else
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
            var newClient = clients.FirstOrDefault(c => c.Value.Equals(atbClientSearch.Text as string)).Key;
            var newHall = halls.FirstOrDefault(h => h.Value.Equals(atbHallSearch.Text as string)).Key;
            var newEvent = events.FirstOrDefault(e => e.Value.Equals(atbEventSearch.Text as string)).Key;
            var dateFrom = dpDateFrom.SelectedDate;
            var dateTo = dpDateTo.SelectedDate;
            var filterData = new Predicate<object>(t => (newClient == 0 || ((term)t).reservation.client.id == newClient) &&
                                                        (newHall == 0 || ((term)t).hall.id == newHall) &&
                                                        (newEvent == 0 || (((term)t).reservation._event != null ? ((term)t).reservation.event_id == newEvent : false)) &&
                                                        (dateFrom == null || (dateFrom != null && ((term)t).rental_date.CompareTo(dateFrom) > 0)) &&
                                                        (dateTo == null || (dateTo != null && ((term)t).rental_date.CompareTo(dateTo) < 0)));
            ICollectionView allTerms = datagrid.Items;
            allTerms.Filter = filterData;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            getRow(e);
            EntityFactory.Term = currentRow.Item as term;
            EntityFactory.Reservation = EntityFactory.Term.reservation;
            CreateReservation createReservationControl = new CreateReservation();
            createReservationControl.Update_Reservation();
            Content = createReservationControl;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            getRow(e);
            popupDelete.IsOpen = true;
        }

        public void getRow(RoutedEventArgs e)
        {
            tbDelete.Text = "Da li ste sigurni da želite obrisati podatke o terminu?";
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
                term newTerm = currentRow.Item as term;
                datagrid.Items.Remove(newTerm);
                datagrid.Items.Refresh();
                term.Delete(newTerm.id);
                tbDelete.Text = "Uspješno ste obrisali podatke o terminu!";
            }
            else
            {
                tbDelete.Text = "Podaci o terminu nisu obrisani!";
            }
            btnAccept.Visibility = btnCancel.Visibility = Visibility.Collapsed;
        }
    }
}
