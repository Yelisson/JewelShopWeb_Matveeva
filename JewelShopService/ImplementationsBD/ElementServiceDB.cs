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
   public class ElementServiceDB:IElementService
    {
        private AbstractDataBaseContext context;

        public ElementServiceDB()
        {
            context = new AbstractDataBaseContext();
        }

        public ElementServiceDB(AbstractDataBaseContext context)
        {
            this.context = context;
        }

        public List<ElementViewModel> GetList()
        {
            List<ElementViewModel> result = context.Elements
                .Select(rec => new ElementViewModel
                {
                    id = rec.id,
                    elementName = rec.elementName
                })
                .ToList();
            return result;
        }

        public ElementViewModel GetElement(int id)
        {
            Element element = context.Elements.FirstOrDefault(rec => rec.id == id);
            if (element != null)
            {
                return new ElementViewModel
                {
                    id = element.id,
                    elementName = element.elementName
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ElementBindingModel model)
        {
            Element element = context.Elements.FirstOrDefault(rec => rec.elementName == model.elementName);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            context.Elements.Add(new Element
            {
                elementName = model.elementName
            });
            context.SaveChanges();
        }

        public void UpdElement(ElementBindingModel model)
        {
            Element element = context.Elements.FirstOrDefault(rec =>
                                        rec.elementName == model.elementName && rec.id != model.id);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            element = context.Elements.FirstOrDefault(rec => rec.id == model.id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.elementName = model.elementName;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Element element = context.Elements.FirstOrDefault(rec => rec.id == id);
            if (element != null)
            {
                context.Elements.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
