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
    public class CustomerServiceDB:ICustomerService
    {
        private AbstractDataBaseContext context;

        public CustomerServiceDB()
        {
            context = new AbstractDataBaseContext();
        }

        public CustomerServiceDB(AbstractDataBaseContext context)
        {
            this.context = context;
        }

        public List<CustomerViewModel> GetList()
        {
            List<CustomerViewModel> result = context.Customers
                .Select(rec => new CustomerViewModel
                {
                    id = rec.id,
                    customerName = rec.customerName
                })
                .ToList();
            return result;
        }

        public CustomerViewModel GetElement(int id)
        {
            Customer element = context.Customers.FirstOrDefault(rec => rec.id == id);
            if (element != null)
            {
                return new CustomerViewModel
                {
                    id = element.id,
                    customerName = element.customerName
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(CustomerBindingModel model)
        {
            Customer element = context.Customers.FirstOrDefault(rec => rec.customerName == model.customerName);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            context.Customers.Add(new Customer
            {
                customerName = model.customerName
            });
            context.SaveChanges();
        }

        public void UpdElement(CustomerBindingModel model)
        {
            Customer element = context.Customers.FirstOrDefault(rec =>
                                        rec.customerName == model.customerName && rec.id != model.id);
            if (element != null)
            {
                throw new Exception("Уже есть сотрудник с таким ФИО");
            }
            element = context.Customers.FirstOrDefault(rec => rec.id == model.id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.customerName = model.customerName;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Customer element = context.Customers.FirstOrDefault(rec => rec.id == id);
            if (element != null)
            {
                context.Customers.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
