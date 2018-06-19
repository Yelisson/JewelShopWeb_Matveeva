using System;
using System.Runtime.Serialization;

namespace JewelShopService.BindingModels
{
   public  class ReportBindingModel
    {
        public string fileName { get; set; }

        public DateTime? dateFrom { get; set; }

        public DateTime? dateTo { get; set; }
    }
}
