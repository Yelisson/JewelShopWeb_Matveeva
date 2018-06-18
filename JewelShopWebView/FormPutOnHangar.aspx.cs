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
        private readonly IHangarService serviceS = UnityConfig.Container.Resolve<IHangarService>();

        private readonly IElementService serviceE = UnityConfig.Container.Resolve<IElementService>();

        private readonly IMainService serviceM = UnityConfig.Container.Resolve<IMainService>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    List<HangarViewModel> listH = serviceS.GetList();
                    if (listH != null)
                    {
                        DropDownListStorage.DataSource = listH;
                        DropDownListStorage.DataBind();
                        DropDownListStorage.DataTextField = "hangarName";
                        DropDownListStorage.DataValueField = "id";
                    }
                    List<ElementViewModel> listE = serviceE.GetList();
                    if (listE != null)
                    {
                        DropDownListElement.DataSource = listE;
                        DropDownListElement.DataBind();
                        DropDownListElement.DataTextField = "elementName";
                        DropDownListElement.DataValueField = "id";
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
                serviceM.PutComponentOnStock(new HangarElementBindingModel
                {
                    elementId = Convert.ToInt32(DropDownListElement.SelectedValue),
                    hangarId = Convert.ToInt32(DropDownListStorage.SelectedValue),
                    count = Convert.ToInt32(TextBoxCount.Text)
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