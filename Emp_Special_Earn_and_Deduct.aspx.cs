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
    public partial class Emp_Special_Earn_and_Deduct : System.Web.UI.Page
    {
        Special_Earn_And_DedBO Special_Earn_And_DedBO = new Special_Earn_And_DedBO();
        Emp_Special_Earn_and_Ded_DA emp_Special_Earn_And_Ded_DA = new Emp_Special_Earn_and_Ded_DA();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //int empNo;
                //if (int.TryParse(txtEmp.Text, out empNo))
                //{
                //    BindGrid(empNo);
                //}
                btnView_Click(sender, e);
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
           
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //// GridViewRow row = gvSpecialEarningsDeductions.Rows[e.RowIndex];
            // string SpEDInd = (row.FindControl("txtSpED_Ind") as TextBox).Text;
            // // string CodeDesc = (row.FindControl("txtCodeDesc") as TextBox).Text;
            // int Code = Convert.ToInt32((row.FindControl("txtCodeDesc") as TextBox).Text);
            // Decimal Amount = Convert.ToDecimal((row.FindControl("txtSpED_Amt") as TextBox).Text);
            // string Payable = (row.FindControl("txtSpED_Payable") as TextBox).Text;
            // DateTime ProcessDate = Convert.ToDateTime((row.FindControl("txtSpED_ProcessDt") as TextBox).Text);

            // // Optionally, you can add code here to refresh the data or show a success message
            

        }


        private void BindGridView()
        {

            try
            {

                int empNo;
                if (int.TryParse(txtEmp.Text, out empNo))
                {
                    DataTable dataTable = emp_Special_Earn_And_Ded_DA.FetchSpecialEarningsDeductions(empNo);


                    if (dataTable.Rows.Count > 0)
                    {
                        
                        gvSpecialEarningsDeductions.DataSource = dataTable;
                        gvSpecialEarningsDeductions.DataBind();

                        
                        lblMessage.Text = "";
                    }
                    else
                    {
                       
                        lblMessage.Text = "No data found for the specified employee number.";
                    }
                }
                else
                {
                   
                    lblMessage.Text = "Please enter a valid employee number.";
                }
            }
            catch (Exception ex)
            {
                
                lblMessage.Text = "An error occurred: " + ex.Message;
               
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                int empNo;
                if (int.TryParse(txtEmp.Text, out empNo))
                {
                    DataTable dataTable = emp_Special_Earn_And_Ded_DA.FetchSpecialEarningsDeductions(empNo);

                   
                    if (dataTable.Rows.Count > 0)
                    {
                        // Bind the data table to the GridView
                        gvSpecialEarningsDeductions.DataSource = dataTable;
                        gvSpecialEarningsDeductions.DataBind();

                        // Clear any previous error messages
                        lblMessage.Text = "";
                    }
                    else
                    {
                        // If no rows found, display a message
                        lblMessage.Text = "No data found for the specified employee number.";
                    }
                }
                else
                {
                    // If invalid employee number entered, display a message
                    lblMessage.Text = "Please enter a valid employee number.";
                }
            }
            catch (Exception ex)
            {
                // If an error occurs, display the error message
                lblMessage.Text = "An error occurred: " + ex.Message;
                // Log the exception for further investigation
            }
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            GridViewRow row = gvSpecialEarningsDeductions.Rows[e.RowIndex];
            int SpED_EmpNo = Convert.ToInt32(gvSpecialEarningsDeductions.DataKeys[e.RowIndex].Values[0]);
            //int empNo = int.Parse(txtEmp.Text);
            //string ind = txtEarningDeduction.Text;
            //decimal amount = int.Parse(txtAmount.Text);
            //string payable = TextBox2.Text;
            //int code = int.Parse(txtEarnDedCode.Text);
            //DateTime processDate = DateTime.Now;
            Special_Earn_And_DedBO Special_Earn_And_DedBO = new Special_Earn_And_DedBO();
            string SpEDInd = (row.FindControl("txtSpED_Ind") as TextBox).Text;
            // string CodeDesc = (row.FindControl("txtCodeDesc") as TextBox).Text;
            int Code = Convert.ToInt32((row.FindControl("txtCodeDesc") as TextBox).Text);
            Decimal Amount = Convert.ToDecimal((row.FindControl("txtSpED_Amt") as TextBox).Text);
            string Payable = (row.FindControl("txtSpED_Payable") as TextBox).Text;
            DateTime ProcessDate = Convert.ToDateTime((row.FindControl("txtSpED_ProcessDt") as TextBox).Text);



            // Call the UpdateSpecialEarnDed method
            emp_Special_Earn_And_Ded_DA.UpdateSpecialEarnDed(SpED_EmpNo, SpEDInd, Amount, Payable, Code, ProcessDate);
            gvSpecialEarningsDeductions.EditIndex = -1;
            this.btnView_Click(sender,e);
        }
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            gvSpecialEarningsDeductions.EditIndex = e.NewEditIndex;
            this.BindGridView() ;
        }


        private void ShowMessage(string message)
        {
            lblMessage.Text = message;
        }
        protected void btnApply_Click(object sender, EventArgs e)
        {
            Special_Earn_And_DedBO.EmpNo = Convert.ToInt32(txtEmp.Text);
            Special_Earn_And_DedBO.Code = Convert.ToInt32(txtEarnDedCode.Text);
            //Special_Earn_And_DedBO.ED = txtEarningDeduction.Text;
            //Special_Earn_And_DedBO.Amount = Convert.ToDecimal(txtAmount.Text);

            if (string.IsNullOrEmpty(txtEarningDeduction.Text))
            {
                string message = "Earning / Deduction not entered";
                ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "showMessage('" + message + "');", true);
            }
            if (string.IsNullOrEmpty(txtEarnDedCode.Text))
            {
                string message = " Code not selected / entered";
                ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "showMessage('" + message + "');", true);

            }
            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                string message = "cannot be 0";
                ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "showMessage('" + message + "');", true);

            }



            emp_Special_Earn_And_Ded_DA.CheckEntryExists(Special_Earn_And_DedBO);
            //if (checkEntryExit)
            //{
            //    emp_Special_Earn_And_Ded_DA.CheckEntryExists(Special_Earn_And_DedBO);


            //}
            //else
            //{
            //    string message = "Entry on Code " + int.Parse(txtEarnDedCode.Text) + " - " +
            //          "already available in Special Earnings / Deduction ";
            //    ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "showMessage('" + message + "');", true);
            //}


        }

       

    }
}