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
        private readonly IBuyerService serviceC = UnityConfig.Container.Resolve<IBuyerService>();

        private readonly IAdornmentService serviceS = UnityConfig.Container.Resolve<IAdornmentService>();

        private readonly IMainService serviceM = UnityConfig.Container.Resolve<IMainService>();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    List<BuyerViewModel> listC = serviceC.GetList();
                    if (listC != null)
                    {
                        DropDownListCustomer.DataSource = listC;
                        DropDownListCustomer.DataBind();
                        DropDownListCustomer.DataTextField = "buyerName";
                        DropDownListCustomer.DataValueField = "id";
                    }
                    List<AdornmentViewModel> listP = serviceS.GetList();
                    if (listP != null)
                    {
                        DropDownListService.DataSource = listP;
                        DropDownListService.DataBind();
                        DropDownListService.DataTextField = "adornmentName";
                        DropDownListService.DataValueField = "id";
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
                    AdornmentViewModel product = serviceS.GetElement(id);
                    int count = Convert.ToInt32(TextBoxCount.Text);
                    TextBoxSum.Text = ((int)(count * product.price)).ToString();
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
                serviceM.CreateOrder(new ProdOrderBindingModel
                {
                    buyerId = Convert.ToInt32(DropDownListCustomer.SelectedValue),
                    adornmentId = Convert.ToInt32(DropDownListService.SelectedValue),
                    count = Convert.ToInt32(TextBoxCount.Text),
                    sum = Convert.ToInt32(TextBoxSum.Text)
                });
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
                Server.Transfer("FormMain.aspx");
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