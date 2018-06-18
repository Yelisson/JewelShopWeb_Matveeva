using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopService.ViewModels
{
    public class AdornmentElementViewModel
    {
        public int id { get; set; }

        public int adornmentId { get; set; }

        public int elementId { get; set; }

        public string elementName { get; set; }

        public int count { get; set; }
    }
}
