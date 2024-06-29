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
    public partial class SalaryRegister : System.Web.UI.Page
    {
        EmployeeBO employeeBO = new EmployeeBO();
        SalaryRegister_DA hrmsSalarySlipDA = new SalaryRegister_DA();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNumber"] = 1;
                Session["TakenBy"] = "ADMIN";
                Session["CurrentDate"] = DateTime.Now;

                FillBanks();

            }
        }
        protected void Count()
        {
            DataTable dataTable = hrmsSalarySlipDA.SalaryRegister(employeeBO);
            if (dataTable.Rows.Count > 0)
            {
                Session["Count"] = dataTable.Rows[0]["Cnt"];
            }

        }
        private void Dropdown()
        {
            DataTable dt = new DataTable();
            EmployeeBO employeeBO = new EmployeeBO();
            Count();
            int cnt = (int)Session["Count"];
            //employeeBO.BranchCode = Convert.ToInt32(ddlBranch.SelectedValue);
            employeeBO.BranchCode = Convert.ToInt32(ddlBranch.SelectedValue);
            employeeBO.Action = "Q";
            dt = hrmsSalarySlipDA.SalaryRegister(employeeBO);
            if (dt.Rows.Count > 0)
            {
                ddlFromEmpNo.DataSource = dt;
                ddlFromEmpNo.DataTextField = "Emp_Name";
                ddlFromEmpNo.DataValueField = "Emp_No";
                ddlFromEmpNo.DataBind();
                ddlFromEmpNo.Items.Insert(0, new ListItem("----- Select -----", "0"));
                ddlFromEmpNo.Items.Insert(1, new ListItem("Min", "1"));

                ddlToEmpNo.DataSource = dt;
                ddlToEmpNo.DataTextField = "Emp_Name";
                ddlToEmpNo.DataValueField = "Emp_No";
                ddlToEmpNo.DataBind();
                ddlToEmpNo.Items.Insert(0, new ListItem("----- Select -----", "0"));
                ddlToEmpNo.Items.Insert(1, new ListItem("Max", cnt.ToString()));




            }
            else
            {
                ddlFromEmpNo.Items.Clear();
                ddlToEmpNo.Items.Clear();
                ddlFromEmpNo.Items.Insert(0, new ListItem("Empty"));
                ddlToEmpNo.Items.Insert(0, new ListItem("Empty"));
            }
        }
        private void FillBanks()
        {
            DataTable dt = new DataTable();
            EmployeeBO employeeBO = new EmployeeBO();
            employeeBO.Action = "M";
            dt = hrmsSalarySlipDA.SalaryRegister(employeeBO);
            if (dt.Rows.Count > 0)
            {
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "CBr_Name";
                ddlBranch.DataValueField = "CBr_Code";
                ddlBranch.DataBind();

                ddlBranch.Items.Insert(0, new ListItem("All", "0"));
            }
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
        }
        private string[] SplitLongName(string name)
        {
            const int MaxLengthPerLine = 25;
            List<string> lines = new List<string>();

            while (name.Length > MaxLengthPerLine)
            {
                lines.Add(name.Substring(0, MaxLengthPerLine));
                name = name.Substring(MaxLengthPerLine);
            }

            lines.Add(name);

            return lines.ToArray();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Home.aspx");

        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dropdown();
        }
    }
}