
using JewelShopService.BindingModels;
using JewelShopService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JewelShopRestApi.Controllers
{
    public class MainController : ApiController
    {
        private readonly IMainService _service;

        public MainController(IMainService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetList()
        {
            var list = _service.GetList();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void CreateOrder(ProdOrderBindingModel model)
        {
            _service.CreateOrder(model);
        }

        [HttpPost]
        public void TakeOrderInWork(ProdOrderBindingModel model)
        {
            _service.TakeOrderInWork(model);
        }

        [HttpPost]
        public void FinishOrder(ProdOrderBindingModel model)
        {
            _service.FinishOrder(model.id);
        }

        [HttpPost]
        public void PayOrder(ProdOrderBindingModel model)
        {
            _service.PayOrder(model.id);
        }

        [HttpPost]
        public void PutComponentOnStock(HangarElementBindingModel model)
        {
            _service.PutComponentOnStock(model);
        }
    }
}
