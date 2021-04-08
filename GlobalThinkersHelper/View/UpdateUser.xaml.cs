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
    /// Interaction logic for UserProfile.xaml
    /// </summary>
    public partial class UpdateUser : UserControl
    {
        public UpdateUser()
        {
            InitializeComponent();
            EntityFactory.User = user.SelectById(2);
            DataContext = EntityFactory.User;
        }

        public bool ValidateFields()
        {
            BindingExpression firstNameValidation = tbFirstName.GetBindingExpression(TextBox.TextProperty);
            BindingExpression lastNameValidation = tbLastName.GetBindingExpression(TextBox.TextProperty);
            BindingExpression usernameValidation = tbUsername.GetBindingExpression(TextBox.TextProperty);
            firstNameValidation.UpdateSource();
            lastNameValidation.UpdateSource();
            usernameValidation.UpdateSource();
            List<bool> errors = new List<bool>()
            {
                firstNameValidation.HasValidationError,
                lastNameValidation.HasValidationError,
                usernameValidation.HasValidationError
            };
            if (!errors.Contains(true))
            {
                user newUser = EntityFactory.CreateUser(tbFirstName.Text.First().ToString().ToUpper() + tbFirstName.Text.Substring(1),
                   tbLastName.Text.First().ToString().ToUpper() + tbLastName.Text.Substring(1), tbUsername.Text, "", UserType.regular);
                user.Update(newUser, EntityFactory.User.id);
                return true;
            }
            return false;
        }

        private void tbFirstName_GotFocus(object sender, RoutedEventArgs e)
        {
            tbFirstName.SelectAll();
        }

        private void tbLastName_GotFocus(object sender, RoutedEventArgs e)
        {
            tbLastName.SelectAll();
        }

        private void tbUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            tbUsername.SelectAll();
        }
    }
}
