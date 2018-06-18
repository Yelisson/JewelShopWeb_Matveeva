using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopModel
{
    public class ProdOrder
    {
        public int id { get; set; }

        public int buyerId { get; set; }

        public int adornmentId { get; set; }

        public int? customerId { get; set; }

        public int count { get; set; }

        public decimal sum { get; set; }

        public ProdOrderStatus status { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime? DateCustom { get; set; }

        public virtual Buyer Buyer { get; set; }

        public virtual Adornment Adornment { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
