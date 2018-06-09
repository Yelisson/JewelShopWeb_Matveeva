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
    public class ElementServiceList : IElementService
    {
        private DataListSingleton source;

        public ElementServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ElementViewModel> GetList()
        {
            List<ElementViewModel> result = new List<ElementViewModel>();
            for (int i = 0; i < source.Elements.Count; ++i)
            {
                result.Add(new ElementViewModel
                {
                    id = source.Elements[i].id,
                    elementName = source.Elements[i].elementName
                });
            }
            return result;
        }

        public ElementViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Elements.Count; ++i)
            {
                if (source.Elements[i].id == id)
                {
                    return new ElementViewModel
                    {
                        id = source.Elements[i].id,
                        elementName = source.Elements[i].elementName
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ElementBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Elements.Count; ++i)
            {
                if (source.Elements[i].id > maxId)
                {
                    maxId = source.Elements[i].id;
                }
                if (source.Elements[i].elementName == model.elementName)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
            }
            source.Elements.Add(new Element
            {
                id = maxId + 1,
                elementName = model.elementName
            });
        }

        public void UpdElement(ElementBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Elements.Count; ++i)
            {
                if (source.Elements[i].id == model.id)
                {
                    index = i;
                }
                if (source.Elements[i].elementName == model.elementName &&
                    source.Elements[i].id != model.id)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Elements[index].elementName = model.elementName;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Elements.Count; ++i)
            {
                if (source.Elements[i].id == id)
                {
                    source.Elements.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
