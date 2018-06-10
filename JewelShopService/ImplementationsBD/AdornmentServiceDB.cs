using JewelShopModel;
using JewelShopService.BindingModels;
using JewelShopService.Interfaces;
using JewelShopService.ViewModels;
using JewelShopService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopService.ImplementationsDB
{
    public class AdornmentServiceDB:IAdornmentService
    {
        private AbstractDataBaseContext context;

        public AdornmentServiceDB()
        {
            context = new AbstractDataBaseContext();
        }

        public AdornmentServiceDB(AbstractDataBaseContext context)
        {
            this.context = context;
        }

        public List<AdornmentViewModel> GetList()
        {
            List<AdornmentViewModel> result = context.Adornments
                .Select(rec => new AdornmentViewModel
                {
                    id = rec.id,
                    adornmentName = rec.adornmentName,
                    price = rec.price,
                    AdornmentElements = context.AdornmentElements
                            .Where(recPC => recPC.adornmentId == rec.id)
                            .Select(recPC => new AdornmentElementViewModel
                            {
                                id = recPC.id,
                                adornmentId = recPC.adornmentId,
                                elementId = recPC.elementId,
                                elementName = recPC.Element.elementName,
                                count = recPC.count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public AdornmentViewModel GetElement(int id)
        {
            Adornment element = context.Adornments.FirstOrDefault(rec => rec.id == id);
            if (element != null)
            {
                return new AdornmentViewModel
                {
                    id = element.id,
                    adornmentName = element.adornmentName,
                    price = element.price,
                    AdornmentElements = context.AdornmentElements
                            .Where(recPC => recPC.adornmentId == element.id)
                            .Select(recPC => new AdornmentElementViewModel
                            {
                                id = recPC.id,
                                adornmentId = recPC.adornmentId,
                                elementId = recPC.elementId,
                                elementName = recPC.Element.elementName,
                                count = recPC.count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(AdornmentBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Adornment element = context.Adornments.FirstOrDefault(rec => rec.adornmentName == model.adornmentName);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = new Adornment
                    {
                        adornmentName = model.adornmentName,
                        price = model.cost
                    };
                    context.Adornments.Add(element);
                    context.SaveChanges();
                    var groupComponents = model.AdornmentComponents
                                                .GroupBy(rec => rec.elementId)
                                                .Select(rec => new
                                                {
                                                    elementId = rec.Key,
                                                    Count = rec.Sum(r => r.count)
                                                });
                    foreach (var groupComponent in groupComponents)
                    {
                        context.AdornmentElements.Add(new AdornmentElement
                        {
                            adornmentId = element.id,
                            elementId = groupComponent.elementId,
                            count = groupComponent.Count
                        });
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void UpdElement(AdornmentBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Adornment element = context.Adornments.FirstOrDefault(rec =>
                                        rec.adornmentName == model.adornmentName && rec.id != model.id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = context.Adornments.FirstOrDefault(rec => rec.id == model.id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.adornmentName = model.adornmentName;
                    element.price = model.cost;
                    context.SaveChanges();

                    var compIds = model.AdornmentComponents.Select(rec => rec.elementId).Distinct();
                    var updateComponents = context.AdornmentElements
                                                    .Where(rec => rec.adornmentId == model.id &&
                                                        compIds.Contains(rec.elementId));
                    foreach (var updateComponent in updateComponents)
                    {
                        updateComponent.count = model.AdornmentComponents
                                                        .FirstOrDefault(rec => rec.id == updateComponent.id).count;
                    }
                    context.SaveChanges();
                    context.AdornmentElements.RemoveRange(
                                        context.AdornmentElements.Where(rec => rec.adornmentId == model.id &&
                                                                            !compIds.Contains(rec.elementId)));
                    context.SaveChanges();
                    var groupComponents = model.AdornmentComponents
                                                .Where(rec => rec.id == 0)
                                                .GroupBy(rec => rec.elementId)
                                                .Select(rec => new
                                                {
                                                    elementId = rec.Key,
                                                    count = rec.Sum(r => r.count)
                                                });
                    foreach (var groupComponent in groupComponents)
                    {
                        AdornmentElement elementPC = context.AdornmentElements
                                                .FirstOrDefault(rec => rec.adornmentId == model.id &&
                                                                rec.elementId == groupComponent.elementId);
                        if (elementPC != null)
                        {
                            elementPC.count += groupComponent.count;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.AdornmentElements.Add(new AdornmentElement
                            {
                                adornmentId = model.id,
                                elementId = groupComponent.elementId,
                                count = groupComponent.count
                            });
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Adornment element = context.Adornments.FirstOrDefault(rec => rec.id == id);
                    if (element != null)
                    {
                        context.AdornmentElements.RemoveRange(
                                            context.AdornmentElements.Where(rec => rec.adornmentId == id));
                        context.Adornments.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
