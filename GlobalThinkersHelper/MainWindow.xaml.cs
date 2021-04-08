using GlobalThinkersHelper.Model.Entities;
using GlobalThinkersHelper.View;
using Newtonsoft.Json;
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

namespace GlobalThinkersHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public object AddDataContext { get; internal set; }
        public object Schedule { get; internal set; }

        public MainWindow()
        {
            InitializeComponent();
            Entity.context = new Database();
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("sr-Latn-RS");
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("sr-Latn-RS");
            Content = new Login();
        }
    }
}
