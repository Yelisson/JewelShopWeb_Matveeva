using JewelShopModel;
using JewelShopService.BindingModels;
using JewelShopService.Interfaces;
using JewelShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopService.ImplementationsDB
{
    public class MainServiceDB:IMainService
    {
        private AbstractDataBaseContext context;

        public MainServiceDB()
        {
            context = new AbstractDataBaseContext();
        }

        public MainServiceDB(AbstractDataBaseContext context)
        {
            this.context = context;
        }

        public List<ProdOrderViewModel> GetList()
        {
            List<ProdOrderViewModel> result = context.ProdOrders
                .Select(rec => new ProdOrderViewModel
                {
                    id = rec.id,
                    buyerId = rec.buyerId,
                    adornmentId = rec.adornmentId,
                    customerId = rec.customerId,
                    DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateCreate),
                    DateCustom = rec.DateCustom == null ? "" :
                                        SqlFunctions.DateName("dd", rec.DateCustom.Value) + " " +
                                        SqlFunctions.DateName("mm", rec.DateCustom.Value) + " " +
                                        SqlFunctions.DateName("yyyy", rec.DateCustom.Value),
                    status = rec.status.ToString(),
                    count = rec.count,
                    sum = rec.sum,
                    buyerName = rec.Buyer.buyerName,
                    adornmentName = rec.Adornment.adornmentName,
                    customerName = rec.Customer.customerName
                })
                .ToList();
            return result;
        }

        public void CreateOrder(ProdOrderBindingModel model)
        {
            context.ProdOrders.Add(new ProdOrder
            {
                buyerId = model.buyerId,
                adornmentId = model.adornmentId,
                DateCreate = DateTime.Now,
                count = model.count,
                sum = model.sum,
                status = ProdOrderStatus.Принят
            });
            context.SaveChanges();
        }

        public void TakeOrderInWork(ProdOrderBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    ProdOrder element = context.ProdOrders.FirstOrDefault(rec => rec.id == model.id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    var productComponents = context.AdornmentElements
                                                .Where(rec => rec.adornmentId == element.adornmentId);
                    foreach (var productComponent in productComponents)
                    {
                        int countOnStocks = productComponent.count * element.count;
                        var stockComponents = context.HangarElements
                                                    .Where(rec => rec.elementId == productComponent.elementId);
                        foreach (var stockComponent in stockComponents)
                        {
                            if (stockComponent.count >= countOnStocks)
                            {
                                stockComponent.count -= countOnStocks;
                                countOnStocks = 0;
                                context.SaveChanges();
                                break;
                            }
                            else
                            {
                                countOnStocks -= stockComponent.count;
                                stockComponent.count = 0;
                                context.SaveChanges();
                            }
                        }
                        if (countOnStocks > 0)
                        {
                            throw new Exception("Недостаточно компонента " +
                                productComponent.Element.elementName + " требуется " +
                                productComponent.count + ", не хватает " + countOnStocks);
                        }
                    }
                    element.customerId = model.customerId;
                    element.DateCustom = DateTime.Now;
                    element.status = ProdOrderStatus.Выполняется;
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void FinishOrder(int id)
        {
            ProdOrder element = context.ProdOrders.FirstOrDefault(rec => rec.id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.status = ProdOrderStatus.Готов;
            context.SaveChanges();
        }

        public void PayOrder(int id)
        {
            ProdOrder element = context.ProdOrders.FirstOrDefault(rec => rec.id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.status = ProdOrderStatus.Оплачен;
            context.SaveChanges();
        }

        public void PutComponentOnStock(HangarElementBindingModel model)
        {
            HangarElement element = context.HangarElements
                                                .FirstOrDefault(rec => rec.hangarId == model.hangarId &&
                                                                    rec.elementId == model.elementId);
            if (element != null)
            {
                element.count += model.count;
            }
            else
            {
                context.HangarElements.Add(new HangarElement
                {
                    hangarId = model.hangarId,
                    elementId = model.elementId,
                    count = model.count
                });
            }
            context.SaveChanges();
        }
    }
}
