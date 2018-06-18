using System;
using System.Collections.Generic;
using System.Linq;
using JewelShopService.ViewModels;
using System.Text;
using System.Threading.Tasks;
using JewelShopModel;
using JewelShopService.BindingModels;
using JewelShopService.Interfaces;

namespace JewelShopService.ImplementationsDB
{
   public class BuyerServiceDB:IBuyerService
    {
        private AbstractDataBaseContext context;

        public BuyerServiceDB()
        {
            context = new AbstractDataBaseContext();
        }

        public BuyerServiceDB(AbstractDataBaseContext context)
        {
            this.context = context;
        }

        public List<BuyerViewModel> GetList()
        {
            List<BuyerViewModel> result = context.Buyers
                .Select(rec => new BuyerViewModel
                {
                    id = rec.id,
                    buyerName = rec.buyerName
                })
                .ToList();
            return result;
        }

        public BuyerViewModel GetElement(int id)
        {
            Buyer element = context.Buyers.FirstOrDefault(rec => rec.id == id);
            if (element != null)
            {
                return new BuyerViewModel
                {
                    id = element.id,
                    buyerName = element.buyerName
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(BuyerBindingModel model)
        {
            Buyer element = context.Buyers.FirstOrDefault(rec => rec.buyerName == model.buyerName);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            context.Buyers.Add(new Buyer
            {
                buyerName = model.buyerName
            });
            context.SaveChanges();
        }

        public void UpdElement(BuyerBindingModel model)
        {
            Buyer element = context.Buyers.FirstOrDefault(rec =>
                                    rec.buyerName == model.buyerName && rec.id != model.id);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            element = context.Buyers.FirstOrDefault(rec => rec.id == model.id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.buyerName = model.buyerName;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Buyer element = context.Buyers.FirstOrDefault(rec => rec.id == id);
            if (element != null)
            {
                context.Buyers.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
