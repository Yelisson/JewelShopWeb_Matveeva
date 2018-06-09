using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopService.ViewModels
{
    public class ProdOrderViewModel
    {
        public int id { get; set; }

        public int buyerId { get; set; }

        public string buyerName { get; set; }

        public int adornmentId { get; set; }

        public string adornmentName { get; set; }

        public int? customerId { get; set; }

        public string customerName { get; set; }

        public int count { get; set; }

        public decimal sum { get; set; }

        public string status { get; set; }

        public string DateCreate { get; set; }

        public string DateCustom { get; set; }
    }
}
