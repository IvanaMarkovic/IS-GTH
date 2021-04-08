using GlobalThinkersHelper.Model.Entities;
using System;
using System.Collections.Generic;
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


namespace GlobalThinkersHelper.View
{
    /// <summary>
    /// Interaction logic for Hall.xaml
    /// </summary>
    public partial class CreateHall : UserControl
    {
        Dictionary<long, string> halls = new Dictionary<long, string>();
        DataGridRow currentRow = new DataGridRow();
        bool editPricelist = false;
        public static DateTime? TimeFrom { get; set; }
        List<string> items;

        public CreateHall()
        {
            InitializeComponent();
            hall.SelectAll().ForEach(h => halls.Add(h.id, h.name));
            atbHallSearch.AutoCompleteSource = halls.Values;

            DataContext = new price_list();
            items = new List<string>() { "Ponedjeljak", "Utorak", "Srijeda", "Četvrtak", "Petak", "Subota", "Nedjelja" };
            ComboBox_dan.ItemsSource = items;
        }

        public void setUpdateFields()
        {
            Label_CreateHall.Content = "Izmjena sale";
            atbHallSearch.Visibility = tbHallSearch.Visibility = searchImg.Visibility = Visibility.Visible;
        }

        public void FillUpdateFields()
        {
            setUpdateFields();
            DataContext = EntityFactory.Hall;
            TextBox_NazivSale.Text = EntityFactory.Hall.name;
            pricelistGrid.DataContext = EntityFactory.PriceList;
            datagrid.ItemsSource = EntityFactory.Hall.price_lists;
            datagrid.SelectedItem = EntityFactory.PriceList;
            expander.IsExpanded = true;
            ComboBox_dan.ItemsSource.Cast<string>().ToList().Where(i => i.Equals(EntityFactory.PriceList.daySr)).ToList().ForEach(pl => ComboBox_dan.SelectedItem = pl);
            editPricelist = true;
        }
       

        public void setCreateFields()
        {
            Label_CreateHall.Content = "Kreiranje sale";
            atbHallSearch.Visibility = tbHallSearch.Visibility = searchImg.Visibility = Visibility.Hidden;
        }

        public void delete()
        {
            TextBox_NazivSale.Text = "";
            tbPriceHour.Text = "";
            Time_VrijemeOd.Text = "";
            Time_VrijemeDo.Text = "";
            datagrid.ItemsSource = null;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            delete();
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
                Keyboard.ClearFocus();
            }
        }
        private void atbHallSearch_SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = atbHallSearch.SelectedItem as string;
            if (obj != null)
            {
                hall oldHall = hall.SelectById(halls.FirstOrDefault(h => h.Value.Equals(obj)).Key);
                EntityFactory.Hall = oldHall;
                TextBox_NazivSale.Text = oldHall.name;
                EntityFactory.Hall.price_lists.Cast<price_list>().ToList().ForEach(p => p.daySr = items[(int)Enum.Parse(typeof(DayInWeek), p.day)]);
                datagrid.ItemsSource = oldHall.price_lists;
                editPricelist = true;
            }
        }

        private void BtnNewPricelist_Click(object sender, RoutedEventArgs e)
        {
            BindingExpression priceValidation = tbPriceHour.GetBindingExpression(TextBox.TextProperty);
            priceValidation.UpdateSource();
            TimeSpan? timeFrom = null;
            if(!Time_VrijemeOd.Text.Equals(""))
            {
                TimeFrom = Convert.ToDateTime(Time_VrijemeOd.Text.ToString());
                timeFrom = new TimeSpan(TimeFrom.Value.Hour, TimeFrom.Value.Minute, TimeFrom.Value.Second);
            }
            BindingExpression timeToValidation = Time_VrijemeDo.GetBindingExpression(MaterialDesignThemes.Wpf.TimePicker.TextProperty);
            timeToValidation.UpdateSource();
            if (!priceValidation.HasValidationError && !timeToValidation.HasValidationError)
            {
                TimeSpan? timeTo = null;
                if(!Time_VrijemeDo.Text.Equals(""))
                {
                    timeTo = (TimeSpan)((price_list)timeToValidation.DataItem).time_to;
                }
                if (CheckBox_Cijena.IsChecked == true)
                {
                    for (int i = 0; i < ComboBox_dan.Items.Count; i++)
                    {
                        ((price_list)priceValidation.DataItem).day = ((DayInWeek)i).ToString();
                        price_list new_price_list = new price_list(((price_list)priceValidation.DataItem).price_hour, timeFrom, timeTo, ((price_list)priceValidation.DataItem).day);
                        new_price_list.daySr = ComboBox_dan.Items.GetItemAt(i).ToString();
                        datagrid.Items.Add(new_price_list);
                    }
                } else
                {
                    ((price_list)priceValidation.DataItem).day = ((DayInWeek)ComboBox_dan.SelectedIndex).ToString();
                    price_list new_price_list = new price_list(((price_list)priceValidation.DataItem).price_hour, timeFrom, timeTo, ((price_list)priceValidation.DataItem).day);
                    new_price_list.daySr = ComboBox_dan.SelectedItem.ToString();
                    if(editPricelist)
                    {
                        price_list oldPricelist = datagrid.Items.Cast<price_list>().ToList()[datagrid.Items.CurrentPosition];
                        oldPricelist = new_price_list;
                        oldPricelist.time_from = new_price_list.time_from;
                        datagrid.Items.Refresh();
                        editPricelist = false;
                    } else
                    {
                        datagrid.Items.Add(new_price_list);
                    }
                    
                }
                clearPriceListFields();
            }
        }

        private void BtnNewPricelist_KeyDown(object sender, KeyEventArgs e)
        {
            BtnNewPricelist_Click(sender, e);
        }


        public void clearPriceListFields()
        {
            CheckBox_Cijena.IsChecked = false;
            ComboBox_dan.SelectedIndex = -1;
            pricelistGrid.DataContext = new price_list();
        }

        public void setPriceListFields(price_list price_list)
        {
            ComboBox_dan.SelectedIndex = (int)(Enum.Parse(typeof(DayInWeek), price_list.day));
            tbPriceHour.Text = price_list.price_hour.ToString();
            Time_VrijemeOd.SelectedTime = new DateTime(price_list.time_from.Value.Ticks);
            Time_VrijemeDo.SelectedTime = new DateTime(price_list.time_to.Value.Ticks);
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            pricelistGrid.DataContext = EntityFactory.PriceList = datagrid.SelectedItem as price_list;
            Time_VrijemeOd.Text = EntityFactory.PriceList.time_from.ToString();
            ComboBox_dan.ItemsSource.Cast<string>().ToList().Where(i => i.Equals(EntityFactory.PriceList.daySr)).ToList().ForEach(pl => ComboBox_dan.SelectedItem = pl);
            editPricelist = true;
            expander.IsExpanded = true;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            datagrid.Items.Remove(datagrid.SelectedItem as price_list);
            datagrid.Items.Refresh();
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            BindingExpression bindingExpr2 = TextBox_NazivSale.GetBindingExpression(TextBox.TextProperty);
            bindingExpr2.UpdateSource();
            if(!bindingExpr2.HasValidationError)
            {
                hall newHall = EntityFactory.CreateHall(TextBox_NazivSale.Text, datagrid.Items.Cast<price_list>().ToList());
                if (Label_CreateHall.Content.Equals("Kreiranje sale"))
                {
                    hall.Insert(newHall);
                }
                else
                {
                    hall.Update(newHall, EntityFactory.Hall.id);
                }
            }
            delete();
        }
       
    }
}
