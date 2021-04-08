using GlobalThinkersHelper.Model;
using GlobalThinkersHelper.Model.Entities;
using GlobalThinkersHelper.View;
using MaterialDesignThemes.Wpf;
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
using MenuItem = GlobalThinkersHelper.Model.MenuItem;

namespace GlobalThinkersHelper
{
    /// <summary>
    /// Interaction logic for SideMenu.xaml
    /// </summary>
    public partial class SideMenu : UserControl
    {
        public static SideMenu Menu { get; set; }

        public SideMenu()
        {
            InitializeComponent();
            DataContext = EntityFactory.User = user.SelectById(2);
            btnInfo.Content = EntityFactory.User;
            tbUsername.Text = EntityFactory.User.username;
            var menuDashboard = new MenuItem("Dnevne napomene", PackIconKind.ViewDashboard, this);
            var menuClientItems = new List<SubItem>()
            {
                new SubItem("Pregled klijenata"),
                new SubItem("Kreiranje klijenta"),
                new SubItem("Izmjena klijenta")
            };
            var menuClient = new MenuItem("Rad sa klijentima", menuClientItems, PackIconKind.People);

            var menuHallItems = new List<SubItem>()
            {
                new SubItem("Pregled sala"),
                new SubItem("Kreiranje sale"),
                new SubItem("Izmjena sale")
            };
            var menuHall = new MenuItem("Rad sa salama", menuHallItems, PackIconKind.DoorOpen);

            var menuReservationItems = new List<SubItem>()
            {
                new SubItem("Pregled rezervacija"),
                new SubItem("Kreiranje rezervacije"),
                new SubItem("Izmjena rezervacije")
            };
            var menuReservation = new MenuItem("Rad sa rezervacijama", menuReservationItems, PackIconKind.Events);

            var menuRecieptItems = new List<SubItem>()
            {
                new SubItem("Pregled računa"),
                new SubItem("Kreiranje računa"),
            };
            var menuReciept = new MenuItem("Rad sa računima", menuRecieptItems, PackIconKind.Receipt);
            var menuSchedule = new MenuItem("Mjesečni pregled", PackIconKind.Calendar, this);

            stackMenu.Children.Add(new UserControlMenuItem(menuDashboard));
            stackMenu.Children.Add(new UserControlMenuItem(menuClient));
            stackMenu.Children.Add(new UserControlMenuItem(menuHall));
            stackMenu.Children.Add(new UserControlMenuItem(menuReservation));
            stackMenu.Children.Add(new UserControlMenuItem(menuReciept));
            stackMenu.Children.Add(new UserControlMenuItem(menuSchedule));
            Menu = this;
        }

        private void btnOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            btnOpenMenu.Visibility = Visibility.Collapsed;
            btnCloseMenu.Visibility = Visibility.Visible;
            foreach (var element in stackMenu.Children)
            {
                ((UserControlMenuItem)element).showItems();
            }

        }

        public void btnCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            btnOpenMenu.Visibility = Visibility.Visible;
            btnCloseMenu.Visibility = Visibility.Collapsed;
            foreach (var element in stackMenu.Children)
            {
                ((UserControlMenuItem)element).hideItems();
            }
        }

        private void btUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            popupContent.Content = new UpdateUser();
            popupUpdate.IsOpen = true;
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (((UpdateUser)popupContent.Content).ValidateFields())
            {
                popupUpdate.IsOpen = false;
            }
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            Content = new Login();
        }
    }
}

