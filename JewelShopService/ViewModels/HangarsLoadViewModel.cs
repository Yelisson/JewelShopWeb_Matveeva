using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JewelShopService.ViewModels
{
   public class HangarsLoadViewModel
    {
        public string hangarName { get; set; }
        public int totalCount { get; set; }
        public IEnumerable<Tuple<string, int>> Elements { get; set; }
    }

}
