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
    public class MainServiceList : IMainService
    {
        private DataListSingleton source;

        public MainServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ProdOrderViewModel> GetList()
        {
            List<ProdOrderViewModel> result = new List<ProdOrderViewModel>();
            for (int i = 0; i < source.ProdOrders.Count; ++i)
            {
                string clientFIO = string.Empty;
                for (int j = 0; j < source.Buyers.Count; ++j)
                {
                    if (source.Buyers[j].id == source.Buyers[i].id)
                    {
                        clientFIO = source.Buyers[j].buyerName;
                        break;
                    }
                }
                string productName = string.Empty;
                for (int j = 0; j < source.Adornments.Count; ++j)
                {
                    if (source.Adornments[j].id == source.ProdOrders[i].adornmentId)
                    {
                        productName = source.Adornments[j].adornmentName;
                        break;
                    }
                }
                string implementerFIO = string.Empty;
                if (source.ProdOrders[i].customerId.HasValue)
                {
                    for (int j = 0; j < source.Customers.Count; ++j)
                    {
                        if (source.Customers[j].id == source.ProdOrders[i].customerId.Value)
                        {
                            implementerFIO = source.Customers[j].customerName;
                            break;
                        }
                    }
                }
                result.Add(new ProdOrderViewModel
                {
                    id = source.ProdOrders[i].id,
                    buyerId = source.ProdOrders[i].buyerId,
                    buyerName = clientFIO,
                    adornmentId = source.ProdOrders[i].adornmentId,
                    adornmentName = productName,
                    customerId = source.ProdOrders[i].customerId,
                    customerName = implementerFIO,
                    count = source.ProdOrders[i].count,
                    sum = source.ProdOrders[i].sum,
                    DateCreate = source.ProdOrders[i].DateCreate.ToLongDateString(),
                    DateCustom = source.ProdOrders[i].DateCustom?.ToLongDateString(),
                    status = source.ProdOrders[i].status.ToString()
                });
            }
            return result;
        }

        public void CreateOrder(ProdOrderBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.ProdOrders.Count; ++i)
            {
                if (source.ProdOrders[i].id > maxId)
                {
                    maxId = source.Buyers[i].id;
                }
            }
            source.ProdOrders.Add(new ProdOrder
            {
                id = maxId + 1,
                buyerId = model.buyerId,
                adornmentId = model.adornmentId,
                DateCreate = DateTime.Now,
                count = model.count,
                sum = model.sum,
                status = ProdOrderStatus.Принят
            });
        }

        public void TakeOrderInWork(ProdOrderBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.ProdOrders.Count; ++i)
            {
                if (source.ProdOrders[i].id == model.id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            for (int i = 0; i < source.AdornmentElements.Count; ++i)
            {
                if (source.AdornmentElements[i].adornmentId == source.ProdOrders[index].adornmentId)
                {
                    int countOnStocks = 0;
                    for (int j = 0; j < source.HangarElements.Count; ++j)
                    {
                        if (source.HangarElements[j].elementId == source.AdornmentElements[i].elementId)
                        {
                            countOnStocks += source.HangarElements[j].count;
                        }
                    }
                    if (countOnStocks < source.AdornmentElements[i].count * source.ProdOrders[index].count)
                    {
                        for (int j = 0; j < source.Elements.Count; ++j)
                        {
                            if (source.Elements[j].id == source.AdornmentElements[i].elementId)
                            {
                                throw new Exception("Не достаточно компонента " + source.Elements[j].elementName +
                                    " требуется " + source.AdornmentElements[i].count + ", в наличии " + countOnStocks);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < source.AdornmentElements.Count; ++i)
            {
                if (source.AdornmentElements[i].adornmentId == source.ProdOrders[index].adornmentId)
                {
                    int countOnStocks = source.AdornmentElements[i].count * source.ProdOrders[index].count;
                    for (int j = 0; j < source.HangarElements.Count; ++j)
                    {
                        if (source.HangarElements[j].elementId == source.AdornmentElements[i].elementId)
                        {
                            if (source.HangarElements[j].count >= countOnStocks)
                            {
                                source.HangarElements[j].count -= countOnStocks;
                                break;
                            }
                            else
                            {
                                countOnStocks -= source.HangarElements[j].count;
                                source.HangarElements[j].count = 0;
                            }
                        }
                    }
                }
            }
            source.ProdOrders[index].customerId = model.customerId;
            source.ProdOrders[index].DateCustom = DateTime.Now;
            source.ProdOrders[index].status = ProdOrderStatus.Выполняется;
        }

        public void FinishOrder(int id)
        {
            int index = -1;
            for (int i = 0; i < source.ProdOrders.Count; ++i)
            {
                if (source.Buyers[i].id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.ProdOrders[index].status = ProdOrderStatus.Готов;
        }

        public void PayOrder(int id)
        {
            int index = -1;
            for (int i = 0; i < source.ProdOrders.Count; ++i)
            {
                if (source.Buyers[i].id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.ProdOrders[index].status = ProdOrderStatus.Оплачен;
        }

        public void PutComponentOnStock(HangarElementBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.HangarElements.Count; ++i)
            {
                if (source.HangarElements[i].hangarId == model.hangarId &&
                    source.HangarElements[i].elementId == model.elementId)
                {
                    source.HangarElements[i].count += model.count;
                    return;
                }
                if (source.HangarElements[i].id > maxId)
                {
                    maxId = source.HangarElements[i].id;
                }
            }
            source.HangarElements.Add(new HangarElement
            {
                id = ++maxId,
                hangarId = model.hangarId,
                elementId = model.elementId,
                count = model.count
            });
        }
    }
}
