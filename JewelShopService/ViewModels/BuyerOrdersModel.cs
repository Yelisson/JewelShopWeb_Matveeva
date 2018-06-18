using System.Runtime.Serialization;

namespace JewelShopService.ViewModels
{
    public class BuyerOrdersModel
    {
        public string buyerName { get; set; }
        public string dateCustom { get; set; }
        public string adornmentName { get; set; }
        public int count { get; set; }
        public decimal sum { get; set; }
        public string status { get; set; }
    }
}
