using JewelShopService.BindingModels;
using JewelShopService.ImplementationsList;
using JewelShopService.Interfaces;
using JewelShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;

namespace JewelShopWebView
{
    public partial class FormPutOnHangar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    var responseC = APIClient.GetRequest("api/Element/GetList");
                    if (responseC.Result.IsSuccessStatusCode)
                    {
                       List<ElementViewModel> list = APIClient.GetElement<List<ElementViewModel>>(responseC);
                       if (list != null)
                       {
                           DropDownListElement.DataSource = list;
                           DropDownListElement.DataBind();
                           DropDownListElement.DataTextField = "elementName";
                           DropDownListElement.DataValueField = "id";
                       }
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(responseC));
                    }
                    var responseS = APIClient.GetRequest("api/Hangar/GetList");
                    if (responseS.Result.IsSuccessStatusCode)
                    {
                        List <HangarViewModel > list = APIClient.GetElement<List<HangarViewModel>>(responseS);
                        if (list != null)
                        {
                            DropDownListStorage.DataSource = list;
                            DropDownListStorage.DataBind();
                            DropDownListStorage.DataTextField = "hangarName";
                            DropDownListStorage.DataValueField = "id";
                         }
                    }else
                    {
                        throw new Exception(APIClient.GetError(responseC));
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
            if (string.IsNullOrEmpty(TextBoxCount.Text))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните поле Количество');</script>");
                return;
            }
            if (DropDownListElement.SelectedValue == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Выберите компонент');</script>");
                return;
            }
            if (DropDownListStorage.SelectedValue == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Выберите склад');</script>");
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Main/PutComponentOnStock", new HangarElementBindingModel
                {
                    elementId = Convert.ToInt32(DropDownListElement.SelectedValue),
                    hangarId = Convert.ToInt32(DropDownListStorage.SelectedValue),
                    count = Convert.ToInt32(TextBoxCount.Text)
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
                    Server.Transfer("FormMain.aspx");
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

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Server.Transfer("FormMain.aspx");
        }
    }
}