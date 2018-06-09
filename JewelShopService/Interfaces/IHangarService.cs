using JewelShopService.BindingModels;
using JewelShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopService.Interfaces
{
    public interface IHangarService
    {
        List<HangarViewModel> GetList();

        HangarViewModel GetElement(int id);

        void AddElement(HangarBindingModel model);

        void UpdElement(HangarBindingModel model);

        void DelElement(int id);
    }
}
