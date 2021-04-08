using GlobalThinkersHelper.Model.Entities;
using GlobalThinkersHelper.Validation;
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
    /// Interaction logic for UserRegistration.xaml
    /// </summary>
    public partial class UserRegistration : UserControl
    {
        public UserRegistration()
        {
            InitializeComponent();
            DataContext = new user();
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            BindingExpression firstNameValidation = nameBox.GetBindingExpression(TextBox.TextProperty);
            firstNameValidation.UpdateSource();

            BindingExpression lastNameValidation = surnameBox.GetBindingExpression(TextBox.TextProperty);
            lastNameValidation.UpdateSource();

            BindingExpression usernameValidation = usernameBox.GetBindingExpression(TextBox.TextProperty);
            usernameValidation.UpdateSource();

            BindingExpression passwordValidation = pbPassword.GetBindingExpression(PasswordBoxAssistant.BoundPassword);
            passwordValidation.UpdateSource();
            if (passwordValidation.HasValidationError)
            {
                lbHelpPassword.Content = passwordValidation.ValidationError.ErrorContent;
                lbHelpPassword.Foreground = new SolidColorBrush(Color.FromArgb(255, 244, 67, 54));
            }
            else
            {
                lbHelpPassword.Content = "Unesite lozinku";
                lbHelpPassword.Foreground = new SolidColorBrush(Color.FromArgb(255, 144, 144, 144));
            }

            List<bool> errors = new List<bool>()
            {
                firstNameValidation.HasValidationError,
                lastNameValidation.HasValidationError,
                usernameValidation.HasValidationError,
                passwordValidation.HasValidationError
            };

            if(!errors.Contains(true))
            {
                var password = BCrypt.Net.BCrypt.HashPassword(pbPassword.Password, pbPassword.Password.ToString().Length);
                user newUser = EntityFactory.CreateUser(nameBox.Text.First().ToString().ToUpper() + nameBox.Text.Substring(1),
                    surnameBox.Text.First().ToString().ToUpper() + surnameBox.Text.Substring(1), usernameBox.Text, password, UserType.regular);
                user.Insert(newUser);
                EntityFactory.User = newUser;
                Content = new SideMenu();
            }
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
            ImgShowHide.Kind = MaterialDesignThemes.Wpf.PackIconKind.Hide;
            tbShowPassword.Text = pbPassword.Password;
            pbPassword.Foreground = Brushes.Transparent;
            tbShowPassword.Foreground = Brushes.Black;
        }

        private void HidePassword()
        {

            ImgShowHide.Kind = MaterialDesignThemes.Wpf.PackIconKind.Show;
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

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Content = new Login();
        }
    }
}
