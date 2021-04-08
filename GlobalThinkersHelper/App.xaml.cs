using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GlobalThinkersHelper
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTk2Mzk1QDMxMzcyZTM0MmUzMFFXUUdFZHVHckxEeFdETkI1dDV1djA2d2VrT3ozL2JYM0lWQUVLbFdKUms9");
        }
    }
}
