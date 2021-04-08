using GlobalThinkersHelper.Model.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ViewHalls.xaml
    /// </summary>
    public partial class ViewHalls : UserControl
    {
        Dictionary<long, string> halls = new Dictionary<long, string>();
        List<DataGridRow> rows = new List<DataGridRow>();
        DataGridRow currentRow = new DataGridRow();
        static List<string> days = new List<string>()
        {
            "Ponedjeljak",
            "Utorak",
            "Srijeda",
            "Četvrtak",
            "Petak",
            "Subota",
            "Nedjelja"
        };

        public ViewHalls()
        {
            InitializeComponent();
            Entity.context = new Database();
            DataContext = new price_list();
            price_list.SelectAll().ForEach(p => {
                p.daySr = days[(int)Enum.Parse(typeof(DayInWeek), p.day)];
                datagrid.Items.Add(p);
            });
            hall.SelectAll().ForEach(h => halls.Add(h.id, h.name));
            atbHallSearch.AutoCompleteSource = halls.Values;
        }

        private void atbHallSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                atbHallSearch.Focusable = false;
                Keyboard.ClearFocus();
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

        private void atbHallSearch_SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = atbHallSearch.SelectedItem as string;
            for (int i = 0; i < datagrid.Items.Count; i++)
            {
                rows.Add((DataGridRow)datagrid.ItemContainerGenerator.ContainerFromIndex(i));
            }
            if (obj != null)
            {
                hall newHall = hall.SelectById(halls.FirstOrDefault(h => h.Value.Equals(obj)).Key);
                rows.ForEach(r =>
                {
                    if (((price_list)r.Item).hall_id != newHall.id)
                    {
                        r.Visibility = Visibility.Collapsed;
                    } else
                    {
                        r.Visibility = Visibility.Visible;
                    }
                });

            }
            else
            {
                rows.ForEach(r => r.Visibility = Visibility.Visible);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            getRow(e);
            EntityFactory.Hall = ((price_list)currentRow.Item).hall;
            EntityFactory.PriceList = (price_list)currentRow.Item;
            CreateHall contentCreateHall = new CreateHall();
            contentCreateHall.FillUpdateFields();
            Content = contentCreateHall;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            getRow(e);
            popupDelete.IsOpen = true;
        }

        public void getRow(RoutedEventArgs e)
        {
            tbDelete.Text = "Da li ste sigurni da želite obrisati podatke o cjenovniku?";
            btnAccept.Visibility = btnCancel.Visibility = Visibility.Visible;
            DependencyObject dependencyObject = (DependencyObject)e.OriginalSource;
            while ((dependencyObject != null) && !(dependencyObject is DataGridRow))
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
                price_list newPriceList = currentRow.Item as price_list;
                datagrid.Items.Remove(newPriceList);
                datagrid.Items.Refresh();
                price_list.Delete(newPriceList.id);
                tbDelete.Text = "Uspješno ste obrisali podatke o cjenovniku!";
            }
            else
            {
                tbDelete.Text = "Podaci o cjenovniku nisu obrisani!";
            }
            btnAccept.Visibility = btnCancel.Visibility = Visibility.Collapsed;
        }
    }
}
