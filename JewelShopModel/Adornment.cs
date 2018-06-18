using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopModel
{
    public class Adornment
    {
        public int id { get; set; }
        [Required]
        public string adornmentName { get; set; }
        [Required]
        public decimal price { get; set; }
        [ForeignKey("adornmentId")]
        public virtual List<ProdOrder> Orders { get; set; }

        [ForeignKey("adornmentId")]
        public virtual List<AdornmentElement> AdornmentElements { get; set; }
    }
}
