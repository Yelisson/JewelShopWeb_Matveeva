using JewelShopService.BindingModels;
using JewelShopService.ImplementationsList;
using JewelShopService.Interfaces;
using JewelShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Unity;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Threading.Tasks;

namespace JewelShopWebView
{
    public partial class FormHangar : System.Web.UI.Page
    {
        private int id;

        private string name;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Int32.TryParse((string)Session["id"], out id))
            {
                try
                {
                    var response = APIClient.GetRequest("api/Hangar/Get/" + id);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var view = APIClient.GetElement<HangarViewModel>(response);
                        if (view != null)
                        {
                            if (!Page.IsPostBack)
                            {
                                textBoxName.Text = view.hangarName;
                            }
                            dataGridView.DataSource = view.HangarElements;
                            dataGridView.DataBind();
                            dataGridView.ShowHeaderWhenEmpty = true;
                        }
                    }else
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
                    Page.DataBind();
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
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните название');</script>");
                return;
            }
            try
            {
                Task<HttpResponseMessage> response;
                if (Int32.TryParse((string)Session["id"], out id))
                {
                    response = APIClient.PostRequest("api/Hangar/UpdElement", new HangarBindingModel
                    {
                        id = id,
                        hangarName = textBoxName.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Hangar/AddElement", new HangarBindingModel
                    {
                        hangarName = textBoxName.Text
                    });
                }
                if (response.Result.IsSuccessStatusCode)
                {
                    Session["id"] = null;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
                    Server.Transfer("FormHangars.aspx");
                }else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                Server.Transfer("FormHangars.aspx");
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Session["id"] = null;
            Server.Transfer("FormHangars.aspx");
        }
    }
}