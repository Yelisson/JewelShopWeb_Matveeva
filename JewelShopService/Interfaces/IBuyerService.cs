using JewelShopService.BindingModels;
using JewelShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopService.Interfaces
{
    public interface IBuyerService
    {
        List<BuyerViewModel> GetList();

        BuyerViewModel GetElement(int id);

        void AddElement(BuyerBindingModel model);

        void UpdElement(BuyerBindingModel model);

        void DelElement(int id);
    }
}
