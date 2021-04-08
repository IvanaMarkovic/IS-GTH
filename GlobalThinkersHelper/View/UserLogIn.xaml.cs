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
using MaterialDesignThemes.Wpf;

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
        }

        private void pbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pbPassword.Password.Length > 0)
            {
                ImgShowHide.Visibility = Visibility.Visible;
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
            ImgShowHide.Kind = PackIconKind.Hide;
            tbShowPassword.Text = pbPassword.Password;
            pbPassword.Foreground = Brushes.Transparent;
            tbShowPassword.Foreground = Brushes.Black;
            tbShowPassword.Focus();
        }

        private void HidePassword()
        {
            ImgShowHide.Kind = PackIconKind.Show;
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
            BindingExpression usernameValidation = userNameField.GetBindingExpression(TextBox.TextProperty);
            usernameValidation.UpdateSource();
            if(!usernameValidation.HasValidationError)
            {
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
                    EntityFactory.User = user.SelectByUsername(userNameField.Text); 
                    if(EntityFactory.User.type.Equals(UserType.regular.ToString()))
                    {
                        SideMenu userControl = new SideMenu();
                        userControl.mainContent.Content = new DailyNotes();
                        Content = userControl;
                    } else
                    {
                        Content = new UserAccounts();
                    }
                    
                }
            }
            
        }

        private void hylnkRegistration_Click(object sender, RoutedEventArgs e)
        {
            Content = new UserRegistration();
        }
    }
}
