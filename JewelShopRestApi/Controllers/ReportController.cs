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
    public class ReportController : ApiController
    {
        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetHangarsLoad()
        {
            var list = _service.GetHangarsLoad();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void SaveAdornmentPrice(ReportBindingModel model)
        {
            _service.SaveAdornmentPrice(model);
        }

        [HttpPost]
        public void SaveHangarsLoad(ReportBindingModel model)
        {
            _service.SaveHangarsLoad(model);
        }
    }
}
