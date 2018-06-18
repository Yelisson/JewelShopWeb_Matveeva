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

namespace JewelShopWebView
{
    public partial class FormAdornment : System.Web.UI.Page
    {
        private readonly IAdornmentService service = new AdornmentServiceList();

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
                        textBoxName.Text = view.adornmentName;
                        textBoxPrice.Text = view.price.ToString();
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
                if (service.GetList().Count == 0 || service.GetList().Last().adornmentName != null)
                {
                    productComponents = new List<AdornmentElementViewModel>();
                    LoadData();
                }
                else
                {
                    productComponents = service.GetList().Last().AdornmentElements;
                    LoadData();
                }
            }
            if (Session["SEId"] != null)
            {
                model = new AdornmentElementViewModel
                {
                    id = (int)Session["SEId"],
                    adornmentId = (int)Session["SEServiceId"],
                    elementId = (int)Session["SEElementId"],
                    elementName = (string)Session["SEElementName"],
                    count = (int)Session["SECount"]
                };
                if (Session["SEIs"] != null)
                {
                    productComponents[(int)Session["SEIs"]] = model;
                }
                else
                {
                    productComponents.Add(model);
                }
            }
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
            if (productComponentBM.Count != 0)
            {
                if (service.GetList().Count == 0 || service.GetList().Last().adornmentName != null)
                {
                    service.AddElement(new AdornmentBindingModel
                    {
                        adornmentName = null,
                        cost = -1,
                        AdornmentComponents = productComponentBM
                    });
                }
                else
                {
                    service.UpdElement(new AdornmentBindingModel
                    {
                        id = service.GetList().Last().id,
                        adornmentName = null,
                        cost = -1,
                        AdornmentComponents = productComponentBM
                    });
                }

            }
            try
            {
                if (productComponents != null)
                {
                    dataGridView.DataBind();
                    dataGridView.DataSource = productComponents;
                    dataGridView.DataBind();
                    dataGridView.ShowHeaderWhenEmpty = true;
                    dataGridView.SelectedRowStyle.BackColor = Color.Silver;
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Visible = false;
                    dataGridView.Columns[3].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('" + ex.Message + "');</script>");
            }
            Session["SEId"] = null;
            Session["SEServiceId"] = null;
            Session["SEElementId"] = null;
            Session["SEElementName"] = null;
            Session["SECount"] = null;
            Session["SEIs"] = null;
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
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Visible = false;
                    dataGridView.Columns[3].Visible = false;
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
                Session["SEId"] = model.id;
                Session["SEServiceId"] = model.adornmentId;
                Session["SEElementId"] = model.elementId;
                Session["SEElementName"] = model.elementName;
                Session["SECount"] = model.count;
                Session["SEIs"] = dataGridView.SelectedIndex;
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
                service.DelElement(service.GetList().Last().id);
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
            Session["id"] = null;
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