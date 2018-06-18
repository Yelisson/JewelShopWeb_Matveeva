using JewelShopModel;
using JewelShopService.BindingModels;
using JewelShopService.Interfaces;
using JewelShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelShopService.ImplementationsDB
{
   public class HangarServiceDB:IHangarService
    {
        private AbstractDataBaseContext context;

        public HangarServiceDB()
        {
            context = new AbstractDataBaseContext();
        }

        public HangarServiceDB(AbstractDataBaseContext context)
        {
            this.context = context;
        }

        public List<HangarViewModel> GetList()
        {
            List<HangarViewModel> result = context.Hangars
                .Select(rec => new HangarViewModel
                {
                    id = rec.id,
                    hangarName = rec.hangarName,
                    HangarElements = context.HangarElements
                            .Where(recPC => recPC.hangarId == rec.id)
                            .Select(recPC => new HangarElementViewModel
                            {
                                id = recPC.id,
                                hangarId = recPC.hangarId,
                                elementId = recPC.elementId,
                                elementName = recPC.Element.elementName,
                                count = recPC.count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public HangarViewModel GetElement(int id)
        {
            Hangar element = context.Hangars.FirstOrDefault(rec => rec.id == id);
            if (element != null)
            {
                return new HangarViewModel
                {
                    id = element.id,
                    hangarName = element.hangarName,
                    HangarElements = context.HangarElements
                            .Where(recPC => recPC.hangarId == element.id)
                            .Select(recPC => new HangarElementViewModel
                            {
                                id = recPC.id,
                                hangarId = recPC.hangarId,
                                elementId = recPC.elementId,
                                elementName = recPC.Element.elementName,
                                count = recPC.count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(HangarBindingModel model)
        {
            Hangar element = context.Hangars.FirstOrDefault(rec => rec.hangarName == model.hangarName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            context.Hangars.Add(new Hangar
            {
                hangarName = model.hangarName
            });
            context.SaveChanges();
        }

        public void UpdElement(HangarBindingModel model)
        {
            Hangar element = context.Hangars.FirstOrDefault(rec =>
                                        rec.hangarName == model.hangarName && rec.id != model.id);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            element = context.Hangars.FirstOrDefault(rec => rec.id == model.id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.hangarName = model.hangarName;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Hangar element = context.Hangars.FirstOrDefault(rec => rec.id == id);
                    if (element != null)
                    {
                        context.HangarElements.RemoveRange(
                                            context.HangarElements.Where(rec => rec.hangarId == id));
                        context.Hangars.Remove(element);
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
