using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopService.ViewModels
{
    public class AdornmentViewModel
    {
        public int id { get; set; }

        public string adornmentName { get; set; }

        public decimal price { get; set; }

        public List<AdornmentElementViewModel> AdornmentElements { get; set; }
    }
}
