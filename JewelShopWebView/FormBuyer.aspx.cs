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
using Unity.Attributes;

namespace JewelShopWebView
{
    public partial class FormBuyer : System.Web.UI.Page
    {
        public int Id { set { id = value; } }

        private readonly IBuyerService service = UnityConfig.Container.Resolve<IBuyerService>();

        private int id;

        private string name;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Int32.TryParse((string)Session["id"], out id))
            {
                try
                {
                    var response = APIClient.GetRequest("api/Buyer/Get/" + id);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var client = APIClient.GetElement<BuyerViewModel>(response);
                        if (!Page.IsPostBack)
                        {
                            textBoxFIO.Text = client.buyerName;
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
            if (string.IsNullOrEmpty(textBoxFIO.Text))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните ФИО');</script>");
                return;
            }
            try
            {
                Task<HttpResponseMessage> response;
                if (Int32.TryParse((string)Session["id"], out id))
                {
                    response = APIClient.PostRequest("api/Buyer/UpdElement", new BuyerBindingModel
                    {
                        id = id,
                        buyerName = textBoxFIO.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Buyer/AddElement", new BuyerBindingModel
                    {
                        buyerName = textBoxFIO.Text
                    });
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                Server.Transfer("FormBuyers.aspx");
            }
            Session["id"] = null;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
            Server.Transfer("FormBuyers.aspx");
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Session["id"] = null;
            Server.Transfer("FormBuyers.aspx");
        }
    }
}