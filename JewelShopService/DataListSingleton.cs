using JewelShopModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopService
{
    class DataListSingleton
    {
        private static DataListSingleton instance;

        public List<Buyer> Buyers{ get; set; }

        public List<Element> Elements { get; set; }

        public List<Customer> Customers { get; set; }

        public List<ProdOrder> ProdOrders { get; set; }

        public List<Adornment> Adornments { get; set; }

        public List<AdornmentElement> AdornmentElements { get; set; }

        public List<Hangar> Hangars { get; set; }

        public List<HangarElement> HangarElements { get; set; }

        private DataListSingleton()
        {
            Buyers = new List<Buyer>();
            Elements = new List<Element>();
            Customers = new List<Customer>();
            ProdOrders = new List<ProdOrder>();
            Adornments = new List<Adornment>();
            AdornmentElements = new List<AdornmentElement>();
            Hangars = new List<Hangar>();
            HangarElements = new List<HangarElement>();
        }

        public static DataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataListSingleton();
            }

            return instance;
        }
    }
}
