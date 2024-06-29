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
    public partial class IncomeTax_Updatation : System.Web.UI.Page
    {
        IncomeTax_UpdatationBO IncomeTax_UpdatationBO = new IncomeTax_UpdatationBO();
        IncomeTax_UpdatationDA incomeTax_UpdatationDA = new IncomeTax_UpdatationDA();
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
                dt = incomeTax_UpdatationDA.FetchIncomeTax_Updatation(IncomeTax_UpdatationBO);
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
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            this.FillgrdUser();
        }
        //protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //}

        protected void btncancel_click(object sender, EventArgs e)
        {

        }
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            this.FillgrdUser();
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];

            // Assuming you have a way to retrieve the Employee Number (SpED_EmpNo)
           // int spED_EmpNo = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values["SpED_EmpNo"]);

            TextBox txtMthDed = row.FindControl("txtMthDed") as TextBox;

            incomeTAx dAPBO = new incomeTAx()
            {
               
                MonthlyDeductionamt = Convert.ToDecimal(txtMthDed.Text)
            };

            incomeTax_UpdatationDA.UpdateDAPoint(dAPBO);

            GridView1.EditIndex = -1;
            this.FillgrdUser();
        }


        protected void OnRowCancelingEdit(object sender, EventArgs e)
        {
            GridView1.EditIndex = -1;
            this.FillgrdUser();
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}