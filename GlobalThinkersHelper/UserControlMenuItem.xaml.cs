using MenuItem = GlobalThinkersHelper.Model.MenuItem;
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
using GlobalThinkersHelper.Model;
using GlobalThinkersHelper.View;

namespace GlobalThinkersHelper
{
    /// <summary>
    /// Interaction logic for UserControlManuItem.xaml
    /// </summary>
    public partial class UserControlMenuItem : UserControl
    {
        public MenuItem Menu { get; set; }
        public MenuItem TempMenu { get; set; }

        public UserControlMenuItem(MenuItem menuItem)
        {
            InitializeComponent();
            Menu = menuItem;
            TempMenu = new MenuItem(Menu.Header, null, Menu.Icon);
            expanderMenu.Visibility = Visibility.Collapsed;
            listBoxMenuItem.Visibility = Visibility.Collapsed;
            DataContext = menuItem;
        }

        public void showItems()
        {
            expanderMenu.Visibility = Menu.SubItems == null ? Visibility.Collapsed : Visibility.Visible;
            listBoxMenuItem.Visibility = Menu.SubItems == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public void hideItems()
        {
            expanderMenu.Visibility = TempMenu.SubItems == null ? Visibility.Collapsed : Visibility.Visible;
            listBoxMenuItem.Visibility = TempMenu.SubItems == null ? Visibility.Visible : Visibility.Collapsed;
        }

        private void tbMenuItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dependencyObject = (DependencyObject)e.OriginalSource;
            var menuItemName = ((SubItem)((TextBlock)dependencyObject).DataContext).Name;
            switch (menuItemName)
            {
                case "Pregled klijenata":
                    SideMenu.Menu.mainContent.Content = new ClientAccounts();
                    break;
                case "Kreiranje klijenta":
                    SideMenu.Menu.mainContent.Content = new CreateClient();
                    break;
                case "Izmjena klijenta":
                    CreateClient createClient = new CreateClient();
                    createClient.ChangeToUpdateClient(false);
                    SideMenu.Menu.mainContent.Content = createClient;
                    break;
                case "Pregled sala":
                    SideMenu.Menu.mainContent.Content = new ViewHalls();
                    break;
                case "Kreiranje sale":
                    SideMenu.Menu.mainContent.Content = new CreateHall();
                    break;
                case "Izmjena sale":
                    CreateHall createHall = new CreateHall();
                    createHall.setUpdateFields();
                    SideMenu.Menu.mainContent.Content = createHall;
                    break;
                case "Pregled rezervacija":
                    SideMenu.Menu.mainContent.Content = new ViewReservations();
                    break;
                case "Kreiranje rezervacije":
                    CreateReservation createReservation = new CreateReservation();
                    createReservation.tbTitle.Text = "Kreiranje rezervacije";
                    SideMenu.Menu.mainContent.Content = new CreateReservation();
                    return;
                case "Izmjena rezervacije":
                    CreateReservation updateReservation = new CreateReservation();
                    updateReservation.tbTitle.Text = "Izmjena rezervacije";
                    SideMenu.Menu.mainContent.Content = updateReservation;
                    break;
                case "Pregled računa":
                    SideMenu.Menu.mainContent.Content = new ViewRecipets();
                    break;
                case "Kreiranje računa":
                    SideMenu.Menu.mainContent.Content = new CreateReceipt();
                    break;
                default:
                    break;
            }
        }

        private void listBoxMenuItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dependencyObject = (DependencyObject)e.OriginalSource;
            if (dependencyObject is TextBlock)
            {
                var menuItemName = ((TextBlock)dependencyObject).DataContext;
                switch (menuItemName)
                {
                    case "Dnevne napomene":
                        SideMenu.Menu.mainContent.Content = new DailyNotes();
                        break;
                    case "Mjesečni pregled":
                        SideMenu.Menu.mainContent.Content = new MonthSchedule();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}