using GlobalThinkersHelper.Model.Entities;
using Newtonsoft.Json;
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
    /// Interaction logic for ClientAccounts.xaml
    /// </summary>
    public partial class ClientAccounts : UserControl
    {
        Dictionary<long, string> clients = new Dictionary<long, string>();
        List<DataGridRow> rows = new List<DataGridRow>();
        DataGridRow currentRow = new DataGridRow();

        public ClientAccounts()
        {
            InitializeComponent();
            Entity.context = new Database();
            DataContext = new client();
            client.SelectAll().ForEach(c => {
                c.Contact = JsonConvert.DeserializeObject<Contact>(c.contact);
                datagrid.Items.Add(c);
            });
            client.SelectAll().ForEach(c => clients.Add(c.id, c.first_name + " " + c.last_name));
            atbClientSearch.AutoCompleteSource = clients.Values;
        }

        private void atbClientSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                atbClientSearch.Focusable = false;
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

        private void atbClientSearch_SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = atbClientSearch.SelectedItem as string;
            if (obj != null)
            {
                long newClient = clients.FirstOrDefault(u => u.Value.Equals(obj)).Key;
                var filterData = new Predicate<object>(c => ((client)c).id == newClient);
                ICollectionView allTerms = datagrid.Items;
                allTerms.Filter = filterData;

            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            getRow(e);
            EntityFactory.Client = currentRow.Item as client;
            CreateClient updateClient = new CreateClient();
            updateClient.ChangeToUpdateClient(true);
            Content = updateClient;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            getRow(e);
            popupDelete.IsOpen = true;
        }

        public void getRow(RoutedEventArgs e)
        {
            tbDelete.Text = "Da li ste sigurni da želite obrisati podatke o klijentu?";
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
                client newClient = currentRow.Item as client;
                datagrid.Items.Remove(newClient);
                datagrid.Items.Refresh();
                client.Delete(newClient.id);
                tbDelete.Text = "Uspješno ste obrisali podatke o klijentu!";
            }
            else
            {
                tbDelete.Text = "Podaci o klijentu nisu obrisani!";
            }
            btnAccept.Visibility = btnCancel.Visibility = Visibility.Collapsed;
        }

    }
}
