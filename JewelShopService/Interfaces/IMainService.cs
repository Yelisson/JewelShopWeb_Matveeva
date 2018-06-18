using JewelShopService.BindingModels;
using JewelShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopService.Interfaces
{
    public interface IMainService
    {
        List<ProdOrderViewModel> GetList();

        void CreateOrder(ProdOrderBindingModel model);

        void TakeOrderInWork(ProdOrderBindingModel model);

        void FinishOrder(int id);

        void PayOrder(int id);

        void PutComponentOnStock(HangarElementBindingModel model);
    }
}
