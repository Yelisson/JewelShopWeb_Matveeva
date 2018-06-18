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
    public class HangarServiceList : IHangarService
    {
        private DataListSingleton source;

        public HangarServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<HangarViewModel> GetList()
        {
            List<HangarViewModel> result = new List<HangarViewModel>();
            for (int i = 0; i < source.Hangars.Count; ++i)
            {
                List<HangarElementViewModel> StockComponents = new List<HangarElementViewModel>();
                for (int j = 0; j < source.HangarElements.Count; ++j)
                {
                    if (source.HangarElements[j].hangarId == source.Hangars[i].id)
                    {
                        string componentName = string.Empty;
                        for (int k = 0; k < source.Elements.Count; ++k)
                        {
                            if (source.AdornmentElements[j].elementId == source.Elements[k].id)
                            {
                                componentName = source.Elements[k].elementName;
                                break;
                            }
                        }
                        StockComponents.Add(new HangarElementViewModel
                        {
                            id = source.HangarElements[j].id,
                            hangarId = source.HangarElements[j].hangarId,
                            elementId = source.HangarElements[j].elementId,
                            elementName = componentName,
                            count = source.HangarElements[j].count
                        });
                    }
                }
                result.Add(new HangarViewModel
                {
                    id = source.Hangars[i].id,
                    hangarName = source.Hangars[i].hangarName,
                    HangarElements = StockComponents
                });
            }
            return result;
        }

        public HangarViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Hangars.Count; ++i)
            {
                List<HangarElementViewModel> StockComponents = new List<HangarElementViewModel>();
                for (int j = 0; j < source.HangarElements.Count; ++j)
                {
                    if (source.HangarElements[j].hangarId == source.Hangars[i].id)
                    {
                        string componentName = string.Empty;
                        for (int k = 0; k < source.Elements.Count; ++k)
                        {
                            if (source.AdornmentElements[j].elementId == source.Elements[k].id)
                            {
                                componentName = source.Elements[k].elementName;
                                break;
                            }
                        }
                        StockComponents.Add(new HangarElementViewModel
                        {
                            id = source.HangarElements[j].id,
                            hangarId = source.HangarElements[j].hangarId,
                            elementId = source.HangarElements[j].elementId,
                            elementName = componentName,
                            count = source.HangarElements[j].count
                        });
                    }
                }
                if (source.Hangars[i].id == id)
                {
                    return new HangarViewModel
                    {
                        id = source.Hangars[i].id,
                        hangarName = source.Hangars[i].hangarName,
                        HangarElements = StockComponents
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(HangarBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Hangars.Count; ++i)
            {
                if (source.Hangars[i].id > maxId)
                {
                    maxId = source.Hangars[i].id;
                }
                if (source.Hangars[i].hangarName == model.hangarName)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            source.Hangars.Add(new Hangar
            {
                id = maxId + 1,
                hangarName = model.hangarName
            });
        }

        public void UpdElement(HangarBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Hangars.Count; ++i)
            {
                if (source.Hangars[i].id == model.id)
                {
                    index = i;
                }
                if (source.Hangars[i].hangarName == model.hangarName &&
                    source.Hangars[i].id != model.id)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Hangars[index].hangarName = model.hangarName;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.HangarElements.Count; ++i)
            {
                if (source.HangarElements[i].hangarId == id)
                {
                    source.HangarElements.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Hangars.Count; ++i)
            {
                if (source.Hangars[i].id == id)
                {
                    source.Hangars.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
