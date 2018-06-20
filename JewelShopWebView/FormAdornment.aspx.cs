using JewelShopService.BindingModels;
using JewelShopService.ImplementationsList;
using JewelShopService.Interfaces;
using JewelShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;

namespace JewelShopWebView
{
    public partial class FormAdornment : System.Web.UI.Page
    {
        private int id;

        private List<AdornmentElementViewModel> productComponents;

        private AdornmentElementViewModel model;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Int32.TryParse((string)Session["id"], out id))
            {
                try
                {
                    var response = APIClient.GetRequest("api/Adornment/Get/" + id);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var view = APIClient.GetElement<AdornmentViewModel>(response);
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
            /*
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
                                    //adornmentName = "-0",
                                    //cost = 0,
                                    adornmentName = textBoxName.Text,
                                    cost = Convert.ToInt32(textBoxPrice.Text),
                                    AdornmentComponents = productComponentBM
                                });
                                Session["id"] = service.GetList().Last().id.ToString();
                                Session["Change"] = "0";
                            }

                        }
                        */

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
                var response = APIClient.GetRequest("api/Adornment/Get/" + id);
                model = APIClient.GetElement<AdornmentViewModel>(response).AdornmentElements[dataGridView.SelectedIndex];
                Session["SEid"] = model.id.ToString();
                Session["SEadornmentId"] = model.adornmentId.ToString();
                Session["SEelementId"] = model.elementId.ToString();

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
                Task<HttpResponseMessage> response;
                if (Int32.TryParse((string)Session["id"], out id))
                {
                    response = APIClient.PostRequest("api/Adornment/UpdElement", new AdornmentBindingModel
                    {
                        id = id,
                        adornmentName = textBoxName.Text,
                        cost = Convert.ToInt32(textBoxPrice.Text),
                        AdornmentComponents = productComponentBM
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Adornment/AddElement", new AdornmentBindingModel
                    {
                        adornmentName = textBoxName.Text,
                        cost = Convert.ToInt32(textBoxPrice.Text),
                        AdornmentComponents = productComponentBM
                    });
                }
                if (response.Result.IsSuccessStatusCode)
                {
                    Session["id"] = null;
                    Session["Change"] = null;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Scripts", "<script>alert('Сохранение прошло успешно');</script>");
                    Server.Transfer("FormAdornments.aspx");
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
            var response = APIClient.GetRequest("api/Adornment/GetList");
            var view = APIClient.GetElement<List<AdornmentViewModel>>(response);
            if (view.Count != 0 && view.Last().adornmentName == null)
            {
                response = APIClient.PostRequest("api/Adornment/DelElement", new AdornmentBindingModel { id = view.Last().id });
                if (!response.Result.IsSuccessStatusCode)
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            if (!String.Equals(Session["Change"], null))
            {
                response = APIClient.PostRequest("api/Adornment/DelElement", new AdornmentBindingModel { id = id });
                if (!response.Result.IsSuccessStatusCode)
                {
                    throw new Exception(APIClient.GetError(response));
                }
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
