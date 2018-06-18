using JewelShopModel;
using JewelShopService.BindingModels;
using JewelShopService.Interfaces;
using JewelShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopService.ImplementationsList
{
    public class BuyerServiceList : IBuyerService
    {
        private DataListSingleton source;

        public BuyerServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<BuyerViewModel> GetList()
        {
            List<BuyerViewModel> result = new List<BuyerViewModel>();
            for (int i = 0; i < source.Buyers.Count; ++i)
            {
                result.Add(new BuyerViewModel
                {
                    id = source.Buyers[i].id,
                    buyerName = source.Buyers[i].buyerName
                });
            }
            return result;
        }

        public BuyerViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Buyers.Count; ++i)
            {
                if (source.Buyers[i].id == id)
                {
                    return new BuyerViewModel
                    {
                        id = source.Buyers[i].id,
                        buyerName = source.Buyers[i].buyerName
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(BuyerBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Buyers.Count; ++i)
            {
                if (source.Buyers[i].id > maxId)
                {
                    maxId = source.Buyers[i].id;
                }
                if (source.Buyers[i].buyerName == model.buyerName)
                {
                    throw new Exception("Уже есть клиент с таким ФИО");
                }
            }
            source.Buyers.Add(new Buyer
            {
                id = maxId + 1,
                buyerName = model.buyerName
            });
        }

        public void UpdElement(BuyerBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Buyers.Count; ++i)
            {
                if (source.Buyers[i].id == model.id)
                {
                    index = i;
                }
                if (source.Buyers[i].buyerName == model.buyerName &&
                    source.Buyers[i].id != model.id)
                {
                    throw new Exception("Уже есть клиент с таким ФИО");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Buyers[index].buyerName = model.buyerName;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Buyers.Count; ++i)
            {
                if (source.Buyers[i].id == id)
                {
                    source.Buyers.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
