using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopService.BindingModels
{
    public class ProdOrderBindingModel
    {
        public int id { get; set; }

        public int buyerId { get; set; }

        public int adornmentId { get; set; }

        public int? customerId { get; set; }

        public int count { get; set; }

        public decimal sum { get; set; }
    }
}
