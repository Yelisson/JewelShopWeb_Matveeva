using JewelShopService.BindingModels;
using JewelShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopService.Interfaces
{
    public interface IAdornmentService
    {
        List<AdornmentViewModel> GetList();

        AdornmentViewModel GetElement(int id);

        void AddElement(AdornmentBindingModel model);

        void UpdElement(AdornmentBindingModel model);

        void DelElement(int id);
    }
}
