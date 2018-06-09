using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopService.BindingModels
{
    public class HangarElementBindingModel
    {
        public int id { get; set; }

        public int hangarId { get; set; }

        public int elementId { get; set; }

        public int count { get; set; }
    }
}
