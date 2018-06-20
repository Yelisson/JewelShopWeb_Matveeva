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
    public class ElementController : ApiController
    {
        private readonly IElementService _service;

        public ElementController(IElementService service)
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

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var element = _service.GetElement(id);
            if (element == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(element);
        }

        [HttpPost]
        public void AddElement(ElementBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(ElementBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(ElementBindingModel model)
        {
            _service.DelElement(model.id);
        }
    }
}
