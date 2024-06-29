using BL;
using BO;
using HRMS.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMS
{
    public partial class HRMS_Emp_Investments : System.Web.UI.Page
    {
        EmpInvBO bO = new EmpInvBO();
        EmpInvBL bL = new EmpInvBL();
        DataTable dt = new DataTable();
        
        DateTime now = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    TempMenuRemoval();
                    DropDown();
                    HRMS_EmInv_Panel2.Visible = true;
                    
                }
            }
            catch
            {
                throw;
            }
        }
        protected void TempMenuRemoval()
        {
            if (Master is HRMS masterPage)
            {
                HRMS master = (HRMS)this.Master;
                Menu hrmsMenu = (Menu)masterPage.FindControl("HRMSMMenu");
                if (hrmsMenu != null)
                {
                    hrmsMenu.Enabled = false;
                    hrmsMenu.Visible = false;
                }
            }
        }
        protected void DropDown()
        {
            try
            {
                bO.Action = "A";
                dt = bL.FetchEmp(bO);
                if (dt.Rows.Count > 0)
                {
                    EmIn_Emp.DataValueField = "Emp_No";
                    EmIn_Emp.DataTextField = "Emp_Name";
                    EmIn_Emp.DataSource = dt;
                    EmIn_Emp.DataBind();
                    EmIn_Emp.Items.Insert(0, new ListItem("----- Select -----", "0"));
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                dt.Clear();
            }
        }
        protected void FillGrid()
        {
            bO.Action = "B";
            bO.Emp_Name = EmIn_Emp.SelectedItem.ToString();
            try
            {
                dt = bL.FetchEmp(bO);
                if(dt.Rows.Count>0)
                {
                    EmIn_Grd.DataSource = dt;
                    EmIn_Grd.DataBind();
                }
                else
                {
                    Globals.Show("No Investments exists for this Employee.");
                    EmIn_Grd.DataSource = null;
                    EmIn_Grd.DataBind();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                dt.Clear();
            }
        }
        protected void EmIn_BackBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void SubmitBtn_Click(object sender, EventArgs e)
        {
            FillGrid();
        }

        protected void EmIn_Grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            EmIn_Grd.PageIndex = e.NewPageIndex;
            FillGrid();
        }
    }
}