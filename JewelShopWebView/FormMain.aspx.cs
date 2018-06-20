using JewelShopService.ImplementationsList;
using JewelShopService.Interfaces;
using JewelShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Unity;
using System.Web.UI.WebControls;
using JewelShopService.BindingModels;

namespace JewelShopWebView
{
    public partial class FormMain : System.Web.UI.Page
    {
        List<ProdOrderViewModel> list;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Main/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    list = APIClient.GetElement<List<ProdOrderViewModel>>(response);
                    dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[4].Visible = false;
                    dataGridView1.Columns[6].Visible = false;
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

        protected void ButtonCreateIndent_Click(object sender, EventArgs e)
        {
            Server.Transfer("FormOrder.aspx");
        }

        protected void ButtonTakeIndentInWork_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedIndex >= 0)
            {
                string index = list[dataGridView1.SelectedIndex].id.ToString();
                Session["id"] = index;
                Server.Transfer("TakeOrderInWork.aspx");
            }
        }

        protected void ButtonIndentReady_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedIndex >= 0)
            {
                int id = list[dataGridView1.SelectedIndex].id;
                try
                {
                    var response = APIClient.PostRequest("api/Main/FinishOrder", new ProdOrderBindingModel
                    {
                        id = id
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        LoadData();
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
        }

        protected void ButtonIndentPayed_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedIndex >= 0)
            {
                int id = list[dataGridView1.SelectedIndex].id;
                try
                {
                    var response = APIClient.PostRequest("api/Main/PayOrder", new ProdOrderBindingModel
                    {
                        id = id
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        LoadData();
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
        }

        protected void ButtonUpd_Click(object sender, EventArgs e)
        {
            LoadData();
            Server.Transfer("FormMain.aspx");
        }
    }
}