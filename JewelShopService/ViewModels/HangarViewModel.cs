using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopService.ViewModels
{
    public class HangarViewModel
    {
        public int id { get; set; }

        public string hangarName { get; set; }

        public List<HangarElementViewModel> HangarElements { get; set; }
    }
}
