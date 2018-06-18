using JewelShopService.BindingModels;
using JewelShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopService.Interfaces
{
   public interface IReportService
    {
        void SaveAdornmentPrice(ReportBindingModel model);
        List<HangarsLoadViewModel> GetHangarsLoad();
        void SaveHangarsLoad(ReportBindingModel model);
    }
}
