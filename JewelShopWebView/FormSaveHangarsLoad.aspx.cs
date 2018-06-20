using JewelShopService.BindingModels;
using JewelShopService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;

namespace JewelShopWebView
{
    public partial class FormSaveHangarsLoad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment; filename=WebHangarsLoa.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            try
            {
                var response = APIClient.PostRequest("api/Report/SaveHangarsLoad", new ReportBindingModel
                {
                    fileName = "D:\\WebHangarsLoad.xls"
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    Response.WriteFile("D:\\WebHangarsLoad.xls");
                }else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ScriptAllert", "<script>alert('" + ex.Message + "');</script>");
            }
            Response.End();
        }
    }
}