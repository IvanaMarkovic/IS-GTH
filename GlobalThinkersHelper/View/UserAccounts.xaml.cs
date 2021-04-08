using GlobalThinkersHelper.Model.Entities;
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
    /// Interaction logic for UserAccounts.xaml
    /// </summary>
    public partial class UserAccounts : UserControl
    {

        Dictionary<long, string> users = new Dictionary<long, string>();
        List<DataGridRow> rows = new List<DataGridRow>();
        DataGridRow currentRow = new DataGridRow();

        public UserAccounts()
        {
            InitializeComponent();
            Entity.context = new Database();
            DataContext = EntityFactory.User = user.SelectById(2);
            tbUsername.Text = EntityFactory.User.username;
            DataContext = new user(); 
            user.SelectAll().ForEach(c => datagrid.Items.Add(c));
            user.SelectAll().ForEach(u => users.Add(u.id, u.first_name + " " + u.last_name));
            atbUserSearch.AutoCompleteSource = users.Values;
        }

        private void atbUserSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                atbUserSearch.Focusable = false;
            }
        }

        private void atbUserSearch_SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = atbUserSearch.SelectedItem as string;
            for (int i = 0; i < datagrid.Items.Count; i++)
            {
                rows.Add((DataGridRow)datagrid.ItemContainerGenerator.ContainerFromIndex(i));
            }
            if (obj != null)
            {
                user newUser = user.SelectById(users.FirstOrDefault(u => u.Value.Equals(obj)).Key);
                rows.ForEach(r =>
                {
                    if (!r.Item.Equals(newUser))
                    {
                        r.Visibility = Visibility.Collapsed;
                    } else
                    {
                        r.Visibility = Visibility.Visible;
                    }
                });

            } else
            {
                rows.ForEach(r => r.Visibility = Visibility.Visible);
            }
        }

        private void tbUserSearch_GotFocus(object sender, TextChangedEventArgs e)
        {
            tbUserSearch.SelectAll();
            if (tbUserSearch.Text.Length > 0)
            {
                atbUserSearch.Select(tbUserSearch.Text.Length, 0);
                atbUserSearch.Focusable = true;
                atbUserSearch.Focus();
            }
        }

        private void btnUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            popupContent.Content = new UpdateUser();
            popupUpdate.IsOpen = true;
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            Content = new Login();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            getRow(e);
            if(popupContent.Content == null)
            {
                popupContent.Content = new UpdateUser();
            }
            var content = (UpdateUser)popupContent.Content;
            content.DataContext = currentRow.Item as user;
            popupUpdate.IsOpen = true;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            getRow(e);
            popupDelete.IsOpen = true;
        }

        public void getRow(RoutedEventArgs e)
        {
            tbDelete.Text = "Da li ste sigurni da želite obrisati nalog?";
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
                user newUser = currentRow.Item as user;
                datagrid.Items.Remove(newUser);
                datagrid.Items.Refresh();
                user.Delete(newUser.id);
                tbDelete.Text = "Uspješno ste obrisali nalog!";
            } else
            {
                tbDelete.Text = "Nalog nije obrisan!";
            }
            btnAccept.Visibility = btnCancel.Visibility = Visibility.Collapsed;
        }

        private void btnAcceptUpdate_Click(object sender, RoutedEventArgs e)
        {
            if(((UpdateUser)popupContent.Content).ValidateFields())
            {
                if(currentRow != null)
                {
                    user newUser = ((user)currentRow.Item);
                    currentRow.Item = user.SelectById(newUser.id);
                    users[newUser.id] = newUser.first_name + " " + newUser.last_name;
                }
                popupUpdate.IsOpen = false;
            }
            
        }

    }
}
