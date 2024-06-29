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
    public partial class Special_Earning_And_Deduction : System.Web.UI.Page
    {
        Special_Earning_And_DeductionDA dataAccess = new Special_Earning_And_DeductionDA();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindEmployeeDropdown();
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

        }

        private void BindEmployeeDropdown()
        {
            DataTable dtEmployees = dataAccess.GetEmployees();

            if (dtEmployees != null && dtEmployees.Rows.Count > 0)
            {
                dtEmployees.Columns.Add("EmpDisplay", typeof(string), "Emp_No + ' - ' + Emp_Name");

                // Bind the modified DataTable to the DropDownList
                ddlEmployee.DataSource = dtEmployees;
                ddlEmployee.DataTextField = "EmpDisplay"; // Display both Emp_No and Emp_Name
                ddlEmployee.DataValueField = "Emp_No";    // Use Emp_No as the value  
                ddlEmployee.DataBind();
            }

            // Add the "Please Select Employee" option at the top
            ddlEmployee.Items.Insert(0, new ListItem("Please Select Employee", ""));
        }
        protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlEmployee.SelectedValue))
            {
                int empNo = Convert.ToInt32(ddlEmployee.SelectedValue);
                DataTable dtEmployeeDetails = dataAccess.GetEmployeeSpecialEarnDed(empNo);
                gvEmployeeDetails.DataSource = dtEmployeeDetails;
                gvEmployeeDetails.DataBind();
            }
            else
            {
                gvEmployeeDetails.DataSource = null;
                gvEmployeeDetails.DataBind();
            }
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvEmployeeDetails.PageIndex = e.NewPageIndex;
            this.BindEmployeeDropdown();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int EmpNo= Convert.ToInt32(ddlEmployee.SelectedValue);
                string spEDInd = ddlEarningDeductions.SelectedValue; 
                int code = Convert.ToInt32(txtEarningDeductionsCode.Text.Trim());
                decimal amount = Convert.ToDecimal(txtAmount.Text.Trim());
                string payableNonPayable = ddlPayableNonPayable.Text;
                DateTime PayableDate = Convert.ToDateTime(txtDate.Text);
                int NOOFDAYS = Convert.ToInt32(txtNoOfDays.Text);





                Special_Earn_And_DedBO specialEarnAndDedBO = new Special_Earn_And_DedBO();
                specialEarnAndDedBO.SpEDInd = spEDInd;
                specialEarnAndDedBO.Code = code;
                specialEarnAndDedBO.SpEDAmt = amount;
                specialEarnAndDedBO.SpEDPayable = payableNonPayable;
                specialEarnAndDedBO.SpEDNoDays = NOOFDAYS;
                specialEarnAndDedBO.SpEDProcessDt = PayableDate;


                dataAccess. InsertSpecialEarningDeduction(specialEarnAndDedBO);

               
                BindEmployeeDropdown(); 

               
                //ClearFormInputs();
            }
            catch (Exception ex)
            {
                // Handle exceptions (logging, error message to user, etc.)
                // Example: lblMessage.Text = "Error: " + ex.Message;
            }
        }
    }
}