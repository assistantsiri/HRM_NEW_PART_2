using BA;
using BO;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMS
{
    public partial class DAP : System.Web.UI.Page
    {
        DApointBO dApointBO = new DApointBO();
        DAP_BL dAP_BL = new DAP_BL();
        DAP_DA dAP_DA = new DAP_DA();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                FillgrdUser();
            }
        }
        protected void FillgrdUser()
        {

            try
            {
                DataTable dt = new DataTable();
                dt = dAP_BL.FetchDAPoints(dApointBO);
                if (dt.Rows.Count > 0)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                }

            }
            catch
            {
                throw;
            }
            finally
            {

            }
        }
        protected void Insert(object sender, EventArgs e)
        {

            DApointBO dAPBO = new DApointBO()
            {
                DAP_FROM = DateTime.TryParse(txtFrom.Text, out var tempDate) ? tempDate : DateTime.MinValue,
                DAP_TO = DateTime.TryParse(txtTo.Text, out var tempDateTo) ? tempDateTo : DateTime.MinValue,
                DAP_PER = Convert.ToDecimal(txtPer.Text),
                //DAP_PER = Convert.ToDecimal(txtPer.Text),
                //DAP_CrDate = DateTime.TryParse(txtCrdate.Text, out var tempDateCrDate) ? tempDateCrDate : DateTime.MinValue
            };
            dAP_DA.InsertDAPoint(dAPBO);
            this.FillgrdUser();
        }
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            this.FillgrdUser();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int dapSlNo = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            TextBox txtFrom = row.FindControl("txtFrom") as TextBox;
            TextBox txtTo = row.FindControl("txtTo") as TextBox;
            TextBox txtPoint = row.FindControl("txtPoint") as TextBox;
            TextBox txtPer = row.FindControl("txtPer") as TextBox;
            TextBox txtCrDate = row.FindControl("txtCrdate") as TextBox;

            DApointBO dAPBO = new DApointBO()
            {
                DAP_SINo = dapSlNo,
                DAP_FROM = DateTime.TryParse(txtFrom.Text, out var tempDate) ? tempDate : DateTime.MinValue,
                DAP_TO = DateTime.TryParse(txtTo.Text, out var tempDateTo) ? tempDateTo : DateTime.MinValue,
                DAP_Point = Convert.ToDecimal(txtPoint.Text),
                DAP_PER = Convert.ToDecimal(txtPer.Text),
                DAP_CrDate = DateTime.TryParse(txtCrDate.Text, out var tempDateCrDate) ? tempDateCrDate : DateTime.MinValue
            };
            dAP_DA.UpdateDAPoint(dAPBO);
            GridView1.EditIndex = -1;
            this.FillgrdUser();
        }

        protected void OnRowCancelingEdit(object sender, EventArgs e)
        {
            GridView1.EditIndex = -1;
            this.FillgrdUser();
        }
        //protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    int dapSlNo = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);

        //    // Create a DAPBO object with the dapSlNo
        //    DApointBO dAPBO = new DApointBO()
        //    {
        //        DAP_SINo = dapSlNo
        //    };

        //    // Call the DeleteDAPoint method passing the dAPBO object
        //    dAP_DA.DeleteDAPoint(dAPBO);

        //    this.FillgrdUser(); // Optionally rebind the grid after deleting the data
        //}
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            //{
            //    (e.Row.Cells[2].Controls[2] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            //}
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            this.FillgrdUser();
        }

        protected void btnsave_click(object sender, EventArgs e)
        {

        }

        protected void btncancel_click(object sender, EventArgs e)
        {
            Response.Redirect("~/Home.aspx");
        }
    }
}