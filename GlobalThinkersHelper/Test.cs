using GlobalThinkersHelper.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalThinkersHelper
{
    class Test
    {
        public static List<client> Function()
        {
            Database context = new Database();
            return  context.clients.ToList();
        }
    }
}
