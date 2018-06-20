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
    public partial class FormAdornmentElement : System.Web.UI.Page
    {
        private AdornmentElementViewModel model;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var response = APIClient.GetRequest("api/Element/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<ElementViewModel> list = APIClient.GetElement<List<ElementViewModel>>(response);
                    if (list != null)
                    {
                        if (!Page.IsPostBack)
                        {
                            DropDownListElement.DataSource = list;
                            DropDownListElement.DataValueField = "id";
                            DropDownListElement.DataTextField = "elementName";
                            Page.DataBind();
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
            if (Session["SEid"] != null)
            {
                DropDownListElement.Enabled = false;
                DropDownListElement.SelectedValue = (string)Session["SEelementId"];
                TextBoxCount.Text = (string)Session["SEcount"];
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
                if (Session["SEid"] == null)
                {
                    model = new AdornmentElementViewModel
                    {
                        elementId = Convert.ToInt32(DropDownListElement.SelectedValue),
                        elementName = DropDownListElement.SelectedItem.Text,
                        count = Convert.ToInt32(TextBoxCount.Text)
                    };
                    Session["SEid"] = model.id;
                    Session["SEadornmentId"] = model.adornmentId;
                    Session["SEelementId"] = model.elementId;
                    Session["SEelementName"] = model.elementName;
                    Session["SEcount"] = model.count;
                }
                else
                {
                    model.count = Convert.ToInt32(TextBoxCount.Text);
                    Session["SEid"] = model.id;
                    Session["SEadornmentId"] = model.adornmentId;
                    Session["SEelementId"] = model.elementId;
                    Session["SEelementName"] = model.elementName;
                    Session["SEcount"] = model.count;
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