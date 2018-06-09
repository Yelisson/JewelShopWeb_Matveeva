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

namespace JewelShopWebView
{
    public partial class TakeOrderInWork : System.Web.UI.Page
    {
        private readonly ICustomerService serviceP = new CustomerServiceList();

        private readonly IMainService serviceM = new MainServiceList();

        private int id;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Int32.TryParse((string)Session["id"],out id))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Не указан заказ');</script>");
                    Server.Transfer("FormMain.aspx");
                }
                List<CustomerViewModel> listI = serviceP.GetList();
                if (listI != null)
                {
                    DropDownListPerformer.DataSource = listI;
                    DropDownListPerformer.DataBind();
                    DropDownListPerformer.DataTextField = "customerName";
                    DropDownListPerformer.DataValueField = "id";
                    DropDownListPerformer.SelectedIndex = -1;
                }
                Page.DataBind();
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (DropDownListPerformer.SelectedValue == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Выберите исполнителя');</script>");
                return;
            }
            try
            {
                serviceM.TakeOrderInWork(new ProdOrderBindingModel
                {
                    id = id,
                    customerId = Convert.ToInt32(DropDownListPerformer.SelectedValue)
                });
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
                Session["id"] = null;
                Server.Transfer("FormMain.aspx");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Session["id"] = null;
            Server.Transfer("FormMain.aspx");
        }
    }
}