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
    public class AdornmentServiceList : IAdornmentService
    {
        private DataListSingleton source;

        public AdornmentServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<AdornmentViewModel> GetList()
        {
            List<AdornmentViewModel> result = new List<AdornmentViewModel>();
            for (int i = 0; i < source.Adornments.Count; ++i)
            {
                List<AdornmentElementViewModel> productComponents = new List<AdornmentElementViewModel>();
                for (int j = 0; j < source.AdornmentElements.Count; ++j)
                {
                    if (source.AdornmentElements[j].adornmentId == source.Adornments[i].id)
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
                        productComponents.Add(new AdornmentElementViewModel
                        {
                            id = source.AdornmentElements[j].id,
                            adornmentId = source.AdornmentElements[j].adornmentId,
                            elementId = source.AdornmentElements[j].elementId,
                            elementName = componentName,
                            count = source.AdornmentElements[j].count
                        });
                    }
                }
                result.Add(new AdornmentViewModel
                {
                    id = source.Adornments[i].id,
                    adornmentName = source.Adornments[i].adornmentName,
                    price = source.Adornments[i].price,
                    AdornmentElements = productComponents
                });
            }
            return result;
        }

        public AdornmentViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Adornments.Count; ++i)
            { 
                List<AdornmentElementViewModel> productComponents = new List<AdornmentElementViewModel>();
                for (int j = 0; j < source.AdornmentElements.Count; ++j)
                {
                    if (source.AdornmentElements[j].adornmentId == source.Adornments[i].id)
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
                        productComponents.Add(new AdornmentElementViewModel
                        {
                            id = source.AdornmentElements[j].id,
                            adornmentId = source.AdornmentElements[j].adornmentId,
                            elementId = source.AdornmentElements[j].elementId,
                            elementName = componentName,
                            count = source.AdornmentElements[j].count
                        });
                    }
                }
                if (source.Adornments[i].id == id)
                {
                    return new AdornmentViewModel
                    {
                        id = source.Adornments[i].id,
                        adornmentName = source.Adornments[i].adornmentName,
                        price = source.Adornments[i].price,
                        AdornmentElements = productComponents
                    };
                }
            }

            throw new Exception("Элемент не найден");
        }

        public void AddElement(AdornmentBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Adornments.Count; ++i)
            {
                if (source.Adornments[i].id > maxId)
                {
                    maxId = source.Adornments[i].id;
                }
                if (source.Adornments[i].adornmentName == model.adornmentName)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            source.Adornments.Add(new Adornment
            {
                id = maxId + 1,
                adornmentName = model.adornmentName,
                price = model.cost
            });
            int maxPCId = 0;
            for (int i = 0; i < source.AdornmentElements.Count; ++i)
            {
                if (source.AdornmentElements[i].id > maxPCId)
                {
                    maxPCId = source.AdornmentElements[i].id;
                }
            }
            for (int i = 0; i < model.AdornmentComponents.Count; ++i)
            {
                for (int j = 1; j < model.AdornmentComponents.Count; ++j)
                {
                    if (model.AdornmentComponents[i].elementId ==
                        model.AdornmentComponents[j].elementId)
                    {
                        model.AdornmentComponents[i].count +=
                            model.AdornmentComponents[j].count;
                        model.AdornmentComponents.RemoveAt(j--);
                    }
                }
            }
            for (int i = 0; i < model.AdornmentComponents.Count; ++i)
            {
                source.AdornmentElements.Add(new AdornmentElement
                {
                    id = ++maxPCId,
                    adornmentId = maxId + 1,
                    elementId = model.AdornmentComponents[i].elementId,
                    count = model.AdornmentComponents[i].count
                });
            }
        }

        public void UpdElement(AdornmentBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Adornments.Count; ++i)
            {
                if (source.Adornments[i].id == model.id)
                {
                    index = i;
                }
                if (source.Adornments[i].adornmentName == model.adornmentName &&
                    source.Adornments[i].id != model.id)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Adornments[index].adornmentName = model.adornmentName;
            source.Adornments[index].price = model.cost;
            int maxPCId = 0;
            for (int i = 0; i < source.AdornmentElements.Count; ++i)
            {
                if (source.AdornmentElements[i].id > maxPCId)
                {
                    maxPCId = source.AdornmentElements[i].id;
                }
            }
            for (int i = 0; i < source.AdornmentElements.Count; ++i)
            {
                if (source.AdornmentElements[i].adornmentId == model.id)
                {
                    bool flag = true;
                    for (int j = 0; j < model.AdornmentComponents.Count; ++j)
                    {
                        if (source.AdornmentElements[i].id == model.AdornmentComponents[j].id)
                        {
                            source.AdornmentElements[i].count = model.AdornmentComponents[j].count;
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        source.AdornmentElements.RemoveAt(i--);
                    }
                }
            }
            for (int i = 0; i < model.AdornmentComponents.Count; ++i)
            {
                if (model.AdornmentComponents[i].id == 0)
                {
                    for (int j = 0; j < source.AdornmentElements.Count; ++j)
                    {
                        if (source.AdornmentElements[j].adornmentId == model.id &&
                            source.AdornmentElements[j].elementId == model.AdornmentComponents[i].elementId)
                        {
                            source.AdornmentElements[j].count += model.AdornmentComponents[i].count;
                            model.AdornmentComponents[i].id = source.AdornmentElements[j].id;
                            break;
                        }
                    }
                    if (model.AdornmentComponents[i].id == 0)
                    {
                        source.AdornmentElements.Add(new AdornmentElement
                        {
                            id = ++maxPCId,
                            adornmentId = model.id,
                            elementId = model.AdornmentComponents[i].elementId,
                            count = model.AdornmentComponents[i].count
                        });
                    }
                }
            }
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.AdornmentElements.Count; ++i)
            {
                if (source.AdornmentElements[i].adornmentId == id)
                {
                    source.AdornmentElements.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Adornments.Count; ++i)
            {
                if (source.Adornments[i].id == id)
                {
                    source.Adornments.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
