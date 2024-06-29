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
    public partial class Salary_Update : System.Web.UI.Page
    {
        SalaryUpdatationDA salaryUpdatationDA = new SalaryUpdatationDA();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable salaryUpdateData = salaryUpdatationDA.FetchSalaryUpdate();
                Response.Write("<script>alert('Salary update  successfully!');</script>");



            }
            catch (Exception ex)
            {
               
                Response.Write("An error !  " + ex.Message);
            }

        }

        protected void btnEnd_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
    }
}