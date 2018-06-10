using JewelShopModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopService
{
    [Table("AbstractDatabase")]
    public class AbstractDataBaseContext:DbContext
    {
        public AbstractDataBaseContext() //: base("JewelShop")
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public virtual DbSet<Buyer> Buyers { get; set; }

        public virtual DbSet<Element> Elements { get; set; }

        public virtual DbSet<ProdOrder> ProdOrders { get; set; }

        public virtual DbSet<Adornment> Adornments { get; set; }

        public virtual DbSet<HangarElement> HangarElements { get; set; }

        public virtual DbSet<Hangar> Hangars { get; set; }

        public virtual DbSet<AdornmentElement> AdornmentElements { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

    }
}
