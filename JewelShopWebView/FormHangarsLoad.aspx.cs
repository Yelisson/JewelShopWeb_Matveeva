using JewelShopService.Interfaces;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;

namespace JewelShopWebView
{
    public partial class FormHangarsLoad : System.Web.UI.Page
    {
        private readonly IReportService service = UnityConfig.Container.Resolve<IReportService>();

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                Table.Rows.Add(new TableRow());
                Table.Rows[0].Cells.Add(new TableCell());
                Table.Rows[0].Cells[0].Text = "Склад";
                Table.Rows[0].Cells.Add(new TableCell());
                Table.Rows[0].Cells[1].Text = "Компонент";
                Table.Rows[0].Cells.Add(new TableCell());
                Table.Rows[0].Cells[2].Text = "Количество";
                var dict = service.GetHangarsLoad();
                if (dict != null)
                {
                    int i = 1;
                    foreach (var elem in dict)
                    {
                        Table.Rows.Add(new TableRow());
                        Table.Rows[i].Cells.Add(new TableCell());
                        Table.Rows[i].Cells[0].Text = elem.hangarName;
                        Table.Rows[i].Cells.Add(new TableCell());
                        Table.Rows[i].Cells[1].Text = "";
                        Table.Rows[i].Cells.Add(new TableCell());
                        Table.Rows[i].Cells[2].Text = "";
                        i++;
                        foreach (var listElem in elem.Elements)
                        {
                            Table.Rows.Add(new TableRow());
                            Table.Rows[i].Cells.Add(new TableCell());
                            Table.Rows[i].Cells[0].Text = "";
                            Table.Rows[i].Cells.Add(new TableCell());
                            Table.Rows[i].Cells[1].Text = listElem.Item1;
                            Table.Rows[i].Cells.Add(new TableCell());
                            Table.Rows[i].Cells[2].Text = listElem.Item2.ToString();
                            i++;
                        }
                        Table.Rows.Add(new TableRow());
                        Table.Rows[i].Cells.Add(new TableCell());
                        Table.Rows[i].Cells[0].Text = "Итого";
                        Table.Rows[i].Cells.Add(new TableCell());
                        Table.Rows[i].Cells[1].Text = "";
                        Table.Rows[i].Cells.Add(new TableCell());
                        Table.Rows[i].Cells[2].Text = elem.totalCount.ToString();
                        i++;
                        Table.Rows.Add(new TableRow());
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ScriptAllertCreateTable", "<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            Server.Transfer("FormSaveHangarsLoad.aspx");
        }

        protected void ButtonBack_Click(object sender, EventArgs e)
        {
            Server.Transfer("FormMain.aspx");
        }
    }
}