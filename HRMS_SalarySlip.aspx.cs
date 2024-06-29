using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BL;
using BO;
using HRMS.App_Code;

namespace HRMS
{
    public partial class HRMS_SalarySlip : System.Web.UI.Page
    {
        SalarySlipBO bO = new SalarySlipBO();
        SalarySlipBL bL = new SalarySlipBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            TempMenuRemoval();
        }
        protected void StartBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable table = new DataTable();
                DataTable dt = new DataTable();

                bO.Action = "N";
                dt = bL.FetchSalarySlip(bO);
                for (int i=0;i<dt.Rows.Count; i++)
                {
                    bO.Emp_No = (int)dt.Rows[i]["Emp_No"];
                    bO.Action = "S";
                    table = bL.FetchSalarySlip(bO);
                    if (table.Rows.Count>0)
                    {
                        

                    }
                    
                }
            }

            catch
            {
                throw;
            }
        }
        protected void BackBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
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
    }
}