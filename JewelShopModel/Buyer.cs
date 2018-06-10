using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopModel
{
    public class Buyer
    {
        public int id { get; set; }
        [Required]
        public string buyerName { get; set; }
        [ForeignKey("buyerId")]
        public virtual List<ProdOrder> Orders { get; set; }
    }
}
