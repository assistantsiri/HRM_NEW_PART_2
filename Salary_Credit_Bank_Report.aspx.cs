using BO;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMS
{
    public partial class Salary_Credit_Bank_Report : System.Web.UI.Page
    {
        GnRpByBank_BranchDA gnRpByBank_BranchDA = new GnRpByBank_BranchDA();
        
        SalaryCedit salaryCedit = new SalaryCedit();
        protected void Page_Load(object sender, EventArgs e)
        {
            List<string> branchNames = gnRpByBank_BranchDA.BankBranches();
           
            foreach (string branchName in branchNames)
            {
                branchDropdown.Items.Add(new ListItem(branchName));
            }
            List<string> banknames = gnRpByBank_BranchDA.GetBank_Name();
            foreach (string bankName in banknames)
            {
                bankDropdown.Items.Add(new ListItem(bankName));
            }

            //Bank();
            //BankBranches();
            Session["PageNumber"] = 1;
            Session["TakenBy"] = "ADMIN";
            Session["CurrentDate"] = DateTime.Now;
        }

        public void genBankRep_button(object sender, EventArgs e)
        {
            int empNo = 0;
            int pageNumber = (int)Session["PageNumber"];
            string takenBy = Session["TakenBy"].ToString();
            DateTime currentDate = (DateTime)Session["CurrentDate"];

            string cname = "";
            string selectedBankName = bankDropdown.SelectedValue;
            string selectedBranchName = branchDropdown.SelectedValue;
            string procedureName = "";
            string action = "";

            DateTime nextProcessDate = gnRpByBank_BranchDA.nextProcessDate();




            //  List of Branches---------------------------------------------------------------



            string currentTime = DateTime.Now.ToString("dd-MM-yyyy");
           


            var reportResult = gnRpByBank_BranchDA.GenerateEmployeeBankInfoReport(selectedBankName, selectedBranchName, nextProcessDate, Server.MapPath("~/App_Data/Report"), pageNumber);

            // Get the file path from the result
            string reportFilePath = reportResult.Item2;

            // Set up the response to download the PDF file
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", $"attachment; filename=SalaryCreditReport.pdf");
            Response.TransmitFile(reportFilePath);
            Response.Flush();
            Response.End();
        }

        protected void GenerateTextFile(List<SalaryCedit> salarySlips)
        {
            try
            {
                if (salarySlips.Count > 0)
                {
                    // Create a StringWriter to write data to a string buffer
                    using (StringWriter sw = new StringWriter())
                    {
                        // Write header line
                        

                        // Write employee data to the StringWriter
                        foreach (var salarySlip in salarySlips)
                        {
                            sw.WriteLine("{0,5} | {1,-18} | {2,-13} | {3}",
                                           salarySlip.EmpNo,
                                           salarySlip.EmpName,
                                           salarySlip.AccountNo,
                                           salarySlip.BankName,
                                           salarySlip.BankBranch,
                                           salarySlip.NetSalary.ToString("N2"));
                        }

                        string filePath = Server.MapPath("~/EmployeeData.txt");
                        File.WriteAllText(filePath, sw.ToString());

                        Response.Redirect("~/EmployeeData.txt");
                    }
                }
                else
                {
                    // Handle case when there is no data
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }
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
            //ScriptManager.RegisterStartupScript(this, GetType(), "ConfirmCancel", "confirm('Are you sure  want to cancel?');", true);
             Response.Redirect("~/Home.aspx");//-- it will redirect to homepage
            // if user want to clear any text then , Can apply-----
            // fromDate.Text = "";
            //toDate.Text = "";
        }
        public void bankDropdown_SelectedIndexChanged(object sender, EventArgs e)

        {


        }


        public void branchDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}