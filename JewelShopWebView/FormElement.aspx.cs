using JewelShopService.BindingModels;
using JewelShopService.ImplementationsList;
using JewelShopService.Interfaces;
using JewelShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;

namespace JewelShopWebView
{
    public partial class FormElement : System.Web.UI.Page
    {
        private int id;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Int32.TryParse((string)Session["id"], out id))
            {
                try
                {
                    var response = APIClient.GetRequest("api/Element/Get/" + id);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var component = APIClient.GetElement<ElementViewModel>(response);
                        if (component != null)
                        {
                            if (!Page.IsPostBack)
                            {
                                textBoxName.Text = component.elementName;
                            }
                        }
                    }else
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните ФИО');</script>");
                return;
            }
            try
            {
                Task<HttpResponseMessage> response;
                if (Int32.TryParse((string)Session["id"], out id))
                {
                    response = APIClient.PostRequest("api/Element/UpdElement", new ElementBindingModel
                    {
                        id = id,
                        elementName = textBoxName.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Element/AddElement", new ElementBindingModel
                    {
                        elementName = textBoxName.Text
                    });
                }
                if (response.Result.IsSuccessStatusCode)
                    {
                    Session["id"] = null;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
                    Server.Transfer("FormElements.aspx");
                    }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                Server.Transfer("FormElements.aspx");
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Session["id"] = null;
            Server.Transfer("FormElements.aspx");
        }
    }
}