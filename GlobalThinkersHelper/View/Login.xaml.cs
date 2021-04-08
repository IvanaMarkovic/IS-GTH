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
using BCrypt.Net;
using GlobalThinkersHelper.Validation;

namespace GlobalThinkersHelper.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
            DataContext = new user();
            //userNameLabel.Visibility = Visibility.Hidden;
            //passwordLabel.Visibility = Visibility.Hidden;
           // passwordLabel1.Visibility = Visibility.Hidden;
        }
        private void pbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pbPassword.Password.Length > 0)
            {
                ImgShowHide.Visibility = Visibility.Visible;
            }
            else
            {
                ImgShowHide.Visibility = Visibility.Hidden;
            }

        }

        private void ImgShowHide_MouseLeave(object sender, MouseEventArgs e)
        {
            HidePassword();
        }

        private void ImgShowHide_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowPassword();
        }

        private void ImgShowHide_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HidePassword();
        }

        private void ShowPassword()
        {
            ImgShowHide.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/hide.png"));
            tbShowPassword.Text = pbPassword.Password;
            pbPassword.Foreground = Brushes.Transparent;
            tbShowPassword.Foreground = Brushes.Black;
            tbShowPassword.Focus();
        }

        private void HidePassword()
        {
            ImgShowHide.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/show.png"));
            tbShowPassword.Foreground = Brushes.Transparent;
            pbPassword.Foreground = Brushes.Black;
            pbPassword.Focus();
        }

        private void pbPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            ImgShowHide.Visibility = Visibility.Visible;
        }

        private void pbPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            ImgShowHide.Visibility = Visibility.Hidden;
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            BindingExpression bindingExpr = userNameField.GetBindingExpression(TextBox.TextProperty);
            bindingExpr.UpdateSource();
            BindingExpression bePassword = pbPassword.GetBindingExpression(PasswordBoxAssistant.BoundPassword);
            bePassword.UpdateSource();
            if (bePassword.HasValidationError)
            {
                lbHelpPassword.Content = bePassword.ValidationError.ErrorContent;
                lbHelpPassword.Foreground = new SolidColorBrush(Color.FromArgb(255, 244, 67, 54));
            }
            else
            {
                lbHelpPassword.Content = "Unesite lozinku";
                lbHelpPassword.Foreground = new SolidColorBrush(Color.FromArgb(255, 144, 144, 144));
            }
            /* string username = userNameField.Text;
             string password = pbPassword.Password;
             userNameLabel.Visibility = Visibility.Hidden;
             passwordLabel.Visibility = Visibility.Hidden;
             passwordLabel1.Visibility = Visibility.Hidden;

             user u = user.SelectByUsername(username);
             if (u != null)
             {
                 if(password.Length < 8)
                 {
                     passwordLabel1.Visibility = Visibility.Visible;
                 }
                 else
                 {
                     if (u.password.Equals(password))
                     {
                         //Content = new UserRegistration();
                         EntityFactory.User = u;
                         /* string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                          Console.WriteLine(hashedPassword);
                          validPassword = BCrypt.Net.BCrypt.Verify("korisnik1", hashedPassword);
                          Console.WriteLine(validPassword);
                     }
                     else
                     {
                         passwordLabel.Visibility = Visibility.Visible;
                     }
                 }
             }
             else
             {
                 userNameLabel.Visibility = Visibility.Visible;
             }
         }*/
        }

    }
}
