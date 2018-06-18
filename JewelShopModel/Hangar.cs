using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopModel
{
    public class Hangar
    {
        public int id { get; set; }

        [Required]
        public string hangarName { get; set; }
        [ForeignKey("hangarId")]
        public virtual List<HangarElement> HangarElements { get; set; }
    }
}
