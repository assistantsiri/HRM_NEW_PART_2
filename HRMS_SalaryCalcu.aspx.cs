using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRMS.App_Code;
using BL;
using System.Globalization;
using DA;

namespace HRMS
{
    public partial class HRMS_SalaryCalcu : System.Web.UI.Page
    {
        SalaryCalculationBL bL = new SalaryCalculationBL();
        SalaryCalculationDA salaryCalculationDA = new SalaryCalculationDA();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get current month and year
                //string currentMonth = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
                //string currentYear = DateTime.Now.Year.ToString();

                // Set the title label text
                string nextMonthYear = salaryCalculationDA.CurrentMonthYear();


                TitleLabel.Text = $"Salary Caluculation of Month and Year:  { nextMonthYear}";
            }
            TempMenuRemoval();

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
        protected void StartBtn_Click(object sender, EventArgs e)
        {
            string msg = bL.SalCalcu();
            Globals.Show(msg);
        }

        protected void BackBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
    }
}