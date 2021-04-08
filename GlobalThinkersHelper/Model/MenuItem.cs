using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GlobalThinkersHelper.Model
{
    public class MenuItem
    {
        public string  Header { get; private set; }
        public PackIconKind Icon { get; set; }
        public List<SubItem> SubItems { get; private set; }
        public UserControl Screen { get; set; }

        public MenuItem(string header, List<SubItem> subItems, PackIconKind icon)
        {
            Header = header;
            SubItems = subItems;
            Icon = icon;
        }

        public MenuItem(string header, PackIconKind icon, UserControl screen)
        {
            Header = header;
            Screen = screen;
            Icon = icon;
        }
    }
}
