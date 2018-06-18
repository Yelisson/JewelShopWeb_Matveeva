using JewelShopService.BindingModels;
using JewelShopService.ImplementationsList;
using JewelShopService.Interfaces;
using JewelShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;

namespace JewelShopWebView
{
    public partial class FormAdornment : System.Web.UI.Page
    {
        private readonly IAdornmentService service = UnityConfig.Container.Resolve<IAdornmentService>();

        private int id;

        private List<AdornmentElementViewModel> productComponents;

        private AdornmentElementViewModel model;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Int32.TryParse((string)Session["id"], out id))
            {
                try
                {
                    AdornmentViewModel view = service.GetElement(id);
                    if (view != null)
                    {
                        if (!Page.IsPostBack)
                        {
                            textBoxName.Text = view.adornmentName;
                            textBoxPrice.Text = ((int)view.price).ToString();
                        }
                        productComponents = view.AdornmentElements;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
            }
            else
            {
                productComponents = Session["productComponents"] as List<AdornmentElementViewModel>;
                if (productComponents == null)
                    productComponents = new List<AdornmentElementViewModel>();
            }
            if (Session["SEid"] != null)
            {
                if (Session["SEIs"] != null)
                {
                    model = new AdornmentElementViewModel
                    {
                        id = (int)Session["SEid"],
                        adornmentId = (int)Session["SEadornmentId"],
                        elementId = (int)Session["SEelementId"],
                        elementName = (string)Session["SEelementName"],
                        count = (int)Session["SEcount"]
                    };
                    productComponents[(int)Session["SEIs"]] = model;
                }
                else
                {
                    model = new AdornmentElementViewModel
                    {
                        id = (int)Session["SEid"],
                        adornmentId = (int)Session["SEadornmentId"],
                        elementId = (int)Session["SEelementId"],
                        elementName = (string)Session["SEelementName"],
                        count = (int)Session["SEcount"]
                    };
                    productComponents.Add(model);
                }
                Session["SEid"] = null;
                Session["SEadornmentId"] = null;
                Session["SEelementId"] = null;
                Session["SEelementName"] = null;
                Session["SEсount"] = null;
                Session["SEIs"] = null;

                Session["productComponents"] = productComponents;
            }

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                if (productComponents != null)
                {
                    dataGridView.DataBind();
                    dataGridView.DataSource = productComponents;
                    dataGridView.DataBind();
                    dataGridView.ShowHeaderWhenEmpty = true;
                    dataGridView.SelectedRowStyle.BackColor = Color.Silver;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            Server.Transfer("FormAdornmentElement.aspx");
        }

        protected void ButtonChange_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedIndex >= 0)
            {
                model = service.GetElement(id).AdornmentElements[dataGridView.SelectedIndex];
                Session["SEid"] = model.id.ToString();
                Session["SEadornmentId"] = model.adornmentId.ToString();
                Session["SEelementId"] = model.elementId.ToString();
                Session["SEcount"] = model.count.ToString();
                Session["SEIs"] = dataGridView.SelectedIndex.ToString();
                Session["Change"] = "0";

                Server.Transfer("FormAdornmentElement.aspx");
            }
        }

        protected void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedIndex >= 0)
            {
                try
                {
                    productComponents.RemoveAt(dataGridView.SelectedIndex);
                }
                catch (Exception ex)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
                }
                LoadData();
            }
        }

        protected void ButtonUpd_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните название');</script>");
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните цену');</script>");
                return;
            }
            if (productComponents == null || productComponents.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Заполните компоненты');</script>");
                return;
            }
            try
            {
                List<AdornmentElementBindingModel> productComponentBM = new List<AdornmentElementBindingModel>();
                for (int i = 0; i < productComponents.Count; ++i)
                {
                    productComponentBM.Add(new AdornmentElementBindingModel
                    {
                        id = productComponents[i].id,
                        adornmentId = productComponents[i].adornmentId,
                        elementId = productComponents[i].elementId,
                        count = productComponents[i].count
                    });
                }
                if (Int32.TryParse((string)Session["id"], out id))
                {
                    service.UpdElement(new AdornmentBindingModel
                    {
                        id = id,
                        adornmentName = textBoxName.Text,
                        cost = Convert.ToInt32(textBoxPrice.Text),
                        AdornmentComponents = productComponentBM
                    });
                }
                else
                {
                    service.AddElement(new AdornmentBindingModel
                    {
                        adornmentName = textBoxName.Text,
                        cost = Convert.ToInt32(textBoxPrice.Text),
                        AdornmentComponents = productComponentBM
                    });
                }
                Session["id"] = null;
                Session["Change"] = null;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
                Server.Transfer("FormAdornments.aspx");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            if (service.GetList().Count != 0 && service.GetList().Last().adornmentName == null)
            {
                service.DelElement(service.GetList().Last().id);
            }
            if (!String.Equals(Session["Change"], null))
            {
                service.DelElement(id);
            }
            Session["id"] = null;
            Session["Change"] = null;
            Server.Transfer("FormAdornments.aspx");
        }

        protected void dataGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;
        }
    }
}
