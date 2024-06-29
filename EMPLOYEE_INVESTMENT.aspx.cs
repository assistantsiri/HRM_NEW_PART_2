using BO;
using DA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMS
{
    public partial class EMPLOYEE_INVESTMENT : System.Web.UI.Page
    {
        EmpInvDA empInvDA = new EmpInvDA();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetEmpDet();
                BindSectionCodes();
                BindSubSections();
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            int employeeNumber = Convert.ToInt32(ddlEmpNo.SelectedValue);
            DataTable dt = empInvDA.GetEmployeeInvestments(employeeNumber);

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int empNo = Convert.ToInt32(ddlEmpNo.SelectedValue);
            int mCode = Convert.ToInt32(ddsection.SelectedValue);
            string sCode = ddsubsection.SelectedValue;
            var SubSectionArray = ddsubsection.SelectedItem.Text.Split('-');
            var Description = SubSectionArray[0];
            var SubCode = SubSectionArray[1];
            var Sub_No = Convert.ToInt32(SubSectionArray[2]);
            var ItCode = Convert.ToInt32(SubSectionArray[3]);
            decimal amount = Convert.ToDecimal(txtAmount.Text);
            DateTime date = Convert.ToDateTime(txtinvestmentDate.Text);
            string remarks = txtRemarks.Text;
            string action = "CREATE";

            EmployeeInvestment investment = new EmployeeInvestment
            {
                EmpNo = empNo,
                MCode = mCode,
                SCode = SubCode,
                SubNo = Sub_No,
                Amount = amount,
                Date = date,
                Remarks = remarks,
                ITCode = ItCode,
                Action = action
            };

            EmpInvDA empInvDA = new EmpInvDA();
            int newSlno = empInvDA.InsertEmployee1(investment);

            // Optionally, handle the result (e.g., show a success message or handle errors)
           
        }


        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int empNo = Convert.ToInt32(ddlEmpNo.SelectedValue);
            int mCode = Convert.ToInt32(ddsection.SelectedValue);
            string sCode = ddsubsection.SelectedValue;
            var SubSectionArray = ddsubsection.SelectedItem.Text.Split('-');
            var Description = SubSectionArray[0];
            var SubCode = SubSectionArray[1];
            var Sub_No = Convert.ToInt32(SubSectionArray[2]);
            var ItCode = Convert.ToInt32(SubSectionArray[3]);
            decimal amount = Convert.ToDecimal(txtAmount.Text);
            DateTime date = Convert.ToDateTime(txtinvestmentDate.Text);
            string remarks = txtRemarks.Text;
            string action = "EDIT";
            EmployeeInvestment investment = new EmployeeInvestment
            {
                EmpNo = empNo,
                MCode = mCode,
                SCode = SubCode,
                SubNo = Sub_No,
                Amount = amount,
                Date = date,
                Remarks = remarks,
                ITCode = ItCode,
                Action = action
            };

            empInvDA.EditEmployeeInvestment(investment);
        }

     

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        private void BindSectionCodes()
        {
            DataTable dt = empInvDA.GetSectionCodes();
            ddsection.DataSource = dt;
            ddsection.DataTextField = "SEC_CODE";
            ddsection.DataValueField = "SEC_CODE";
            ddsection.DataBind();
            ddsection.Items.Insert(0, new ListItem("--Select Section--", "0"));
        }
        private void GetEmpDet()
        {
            DataTable dt = empInvDA.GetEmpDet();

           
            dt.Columns.Add("DisplayField", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                row["DisplayField"] = row["EMP_NO"].ToString() + " - " + row["EMP_NAME"].ToString() + " - " + row["HRM_DESC"].ToString();
            }

            ddlEmpNo.DataSource = dt;
            ddlEmpNo.DataTextField = "DisplayField";
            ddlEmpNo.DataValueField = "EMP_NO";

            ddlEmpNo.DataBind();

            ddlEmpNo.Items.Insert(0, new ListItem("--Select Section--", "0"));
        }
        private void BindSubSections()
        {
            DataTable dt = empInvDA.GetSubSections(); 

           
            dt.Columns.Add("DisplayField", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                string displayText = $"{row["Sub_Desc"]} - {row["Sub_SCode"]} - {row["Sub_SubNo"]} - {row["Sub_ITCode"]}";
                row["DisplayField"] = displayText;
            }

           
            ddsubsection.DataSource = dt;
            ddsubsection.DataTextField = "DisplayField";
            ddsubsection.DataValueField = "Sub_SCode"; 
            
            ddsubsection.DataBind();

            // Optionally, insert a default item
            ddsubsection.Items.Insert(0, new ListItem("--Select Sub Section--", "0"));
        }

        protected void InsertEmployee(object sender, EventArgs e)
        {
            int empNo = Convert.ToInt32(ddlEmpNo.SelectedValue);
            int mCode = Convert.ToInt32(ddsection.SelectedValue); // Assuming this maps to MCode
            string sCode = ddsubsection.SelectedValue; // Assuming this maps to SCode
           // int subNo = 0; // Assuming this maps to SubNo (not clear in form)
            decimal amount = Convert.ToDecimal(txtAmount.Text);
            DateTime date = Convert.ToDateTime(txtinvestmentDate.Text);
            string remarks = txtRemarks.Text;
           // int itCode = 0; // Assuming this maps to ITCode (not clear in form)
            string action = "INSERT"; // Assuming you want to perform an insert operation

            // Create an instance of your BO class (EmployeeInvestment)
            EmployeeInvestment investment = new EmployeeInvestment
            {
                EmpNo = empNo,
                MCode = mCode,
                SCode = sCode,
                //SubNo = subNo,
                Amount = amount,
                Date = date,
                Remarks = remarks,
               // ITCode = itCode,
                Action = action
            };
            empInvDA.InsertEmployee(investment);
        }




    }
}