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
    public partial class FormAdornmentElement : System.Web.UI.Page
    {
        private readonly IElementService service = new ElementServiceList();

        private AdornmentElementViewModel model;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                List<ElementViewModel> list = service.GetList();
                if (list != null)
                {
                    DropDownListElement.DataSource = list;
                    DropDownListElement.DataValueField = "id";
                    DropDownListElement.DataTextField = "elementName";
                    DropDownListElement.SelectedIndex = -1;
                    Page.DataBind();
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
            if (Session["SEId"] != null)
            {
                DropDownListElement.Enabled = false;
                DropDownListElement.SelectedValue = (string)Session["SEElementId"];
                TextBoxCount.Text = (string)Session["SECount"];
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
            try
            {
                if (Session["SEId"] == null)
                {
                    model = new AdornmentElementViewModel
                    {
                        elementId = Convert.ToInt32(DropDownListElement.SelectedValue),
                        elementName = DropDownListElement.SelectedItem.Text,
                        count = Convert.ToInt32(TextBoxCount.Text)
                    };
                    Session["SEId"] = model.id;
                    Session["SEServiceId"] = model.adornmentId;
                    Session["SEElementId"] = model.elementId;
                    Session["SEElementName"] = model.elementName;
                    Session["SECount"] = model.count;
                }
                else
                {
                    model.count = Convert.ToInt32(TextBoxCount.Text);
                    Session["SEId"] = model.id;
                    Session["SEServiceId"] = model.adornmentId;
                    Session["SEElementId"] = model.elementId;
                    Session["SEElementName"] = model.elementName;
                    Session["SECount"] = model.count;
                }
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
                Server.Transfer("FormAdornment.aspx");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            Server.Transfer("FormAdornment.aspx");
        }
    }
}