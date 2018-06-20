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
    public partial class FormOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    var responseC = APIClient.GetRequest("api/Buyer/GetList");
                    if (responseC.Result.IsSuccessStatusCode)
                    {
                        List <BuyerViewModel > list = APIClient.GetElement<List<BuyerViewModel>>(responseC);
                        if (list != null)
                        {
                            DropDownListCustomer.DataSource = list;
                            DropDownListCustomer.DataBind();
                            DropDownListCustomer.DataTextField = "buyerName";
                            DropDownListCustomer.DataValueField = "id";
                         }
                     }
                    else
                    {
                        throw new Exception(APIClient.GetError(responseC));
                    }
                    var responseP = APIClient.GetRequest("api/Adornment/GetList");
                    if (responseP.Result.IsSuccessStatusCode)
                    {
                        List<AdornmentViewModel> list = APIClient.GetElement<List<AdornmentViewModel>>(responseP);
                        if (list != null)
                        {
                            DropDownListService.DataSource = list;
                            DropDownListService.DataBind();
                            DropDownListService.DataTextField = "adornmentName";
                            DropDownListService.DataValueField = "id";
                        }
                    }else
                    {
                        throw new Exception(APIClient.GetError(responseP));
                    }
                    Page.DataBind();
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        private void CalcSum()
        {
            
            if (DropDownListService.SelectedValue != null && !string.IsNullOrEmpty(TextBoxCount.Text))
            {
                try
                {
                    int id = Convert.ToInt32(DropDownListService.SelectedValue);
                    var responseP = APIClient.GetRequest("api/Adornment/Get/" + id);
                    if (responseP.Result.IsSuccessStatusCode)
                    {
                        AdornmentViewModel product = APIClient.GetElement<AdornmentViewModel>(responseP);
                        int count = Convert.ToInt32(TextBoxCount.Text);
                        TextBoxSum.Text = ((int)(count * product.price)).ToString();
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(responseP));
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        protected void DropDownListService_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        protected void TextBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxCount.Text))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните поле Количество');</script>");
                return;
            }
            if (DropDownListCustomer.SelectedValue == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Выберите клиента');</script>");
                return;
            }
            if (DropDownListService.SelectedValue == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Выберите изделие');</script>");
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Main/CreateOrder", new ProdOrderBindingModel
                {
                    buyerId = Convert.ToInt32(DropDownListCustomer.SelectedValue),
                    adornmentId = Convert.ToInt32(DropDownListService.SelectedValue),
                    count = Convert.ToInt32(TextBoxCount.Text),
                    sum = Convert.ToInt32(TextBoxSum.Text)
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
                    Server.Transfer("FormMain.aspx");
                }
                else
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