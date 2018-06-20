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
    public class AdornmentController : ApiController
    {
        private readonly IAdornmentService _service;

        public AdornmentController(IAdornmentService service)
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
        public void AddElement(AdornmentBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(AdornmentBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(AdornmentBindingModel model)
        {
            _service.DelElement(model.id);
        }
    }
}
