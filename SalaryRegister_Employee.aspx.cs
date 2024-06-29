using BO;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRMS.App_Code;
using System.IO;
using System.Data;
using iTextSharp.text;
using Image = iTextSharp.text.Image;
using iTextSharp.text.pdf;

namespace HRMS
{
    public partial class SalaryRegister_Employee : System.Web.UI.Page
    {
        SalaryRegister_EmployeeDA employeeSalaryReport = new SalaryRegister_EmployeeDA();
        SalaryRegisterDA hrmsSalarySlipDA = new SalaryRegisterDA();
        EmployeeBO employeeBO = new EmployeeBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TempMenuRemoval();


                Session["PageNumber"] = 1;
                Session["TakenBy"] = "ADMIN";
                Session["CurrentDate"] = DateTime.Now;

                BindEmployeeDropdown();

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





        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // ScriptManager.RegisterStartupScript(this, GetType(), "ConfirmCancel", "confirm('Are you sure you want to cancel?');", true);
            // Response.Redirect("~/Default.aspx"); -- it will redirect to homepage
            // if user want to clear any text then , Can apply-----
            // fromDate.Text = "";
            //toDate.Text = "";
            Response.Redirect("~/Home.aspx");
        }





        /*protected void ddlEmpCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlTables.SelectedValue == "Employees")
                {
                    EmployeeRepBL employeeRepBL = new EmployeeRepBL();


                    DataTable employeeData = employeeRepBL.FetchEmployeeInformation();





                    foreach (DataRow row in employeeData.Rows)
                    {

                        int empNo = Convert.ToInt32(row["EmpNo"]);
                        string empName = row["EmpName"].ToString();
                        string empDesig = row["EmpDesig"].ToString();


                        ddlTables.Items.Add(new ListItem($"{empNo} - {empName} ({empDesig})", empNo.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
            }
        }*/




        protected void btnGenReport_Click(object sender, EventArgs e)
        {
            EmployeeBO employeeBO = new EmployeeBO();
            int pageNumber = (int)Session["PageNumber"];
            string takenBy = Session["TakenBy"].ToString();
            DateTime currentDate = (DateTime)Session["CurrentDate"];
            int empNo = Convert.ToInt32(ddlEmployee.SelectedValue);
            DateTime fromDateValue = DateTime.ParseExact(fromDate.Text, "yyyy-MM-dd", null);
            DateTime toDateValue = DateTime.ParseExact(toDate.Text, "yyyy-MM-dd", null);
            string fromDateFormatted = fromDateValue.ToString("MMMM-yyyy");
            string toDateFormatted = toDateValue.ToString("MMMM-yyyy");
            // string selectedBranchNames = branchDropdown.SelectedValue;

            SalaryRegister_EmployeeDA employeeReportDA = new SalaryRegister_EmployeeDA();
           
            string EmployeeData = employeeReportDA.GenerateSalarySlip(empNo, pageNumber, takenBy, currentDate, fromDateValue, toDateValue, fromDateFormatted, toDateFormatted);
            string rootDirectoryPath = HttpContext.Current.Server.MapPath("~/App_Data/Report");
            string fileName = "SalaryRegisterEmployee.txt";


            string filePath = Path.Combine(rootDirectoryPath, fileName);

            string reportFileName = Path.Combine(rootDirectoryPath, "SalaryRegisterEmployee.txt");
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "attachment; filename=SalaryRegisterEmployee.txt");
            Response.TransmitFile(reportFileName);
            Response.Flush();
            Response.End();


            Globals.Show("Employee Report generated successfully.");
           

        }
        private void BindEmployeeDropdown()
        {
            DataTable dtEmployees = employeeSalaryReport.GetEmployee();
            if (dtEmployees.Rows.Count > 0)
            {
                ddlEmployee.DataSource = dtEmployees;
                //ddlEmployee.DataTextField = "Emp_Name";
                ddlEmployee.DataValueField = "Emp_No";
                ddlEmployee.DataBind();
            }
            ddlEmployee.Items.Insert(0, new  System.Web.UI.WebControls.ListItem("--Select Employee--", "0"));
        }
        protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedEmpNo = Convert.ToInt32(ddlEmployee.SelectedValue);
           // txtempNo.Text = selectedEmpNo.ToString();
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void btnGenReport_Click_Pdf(object sender, EventArgs e)
        {
            try
            {
                employeeBO.EmpNo = Convert.ToInt32(ddlEmployee.SelectedValue);
                employeeBO.FromDate = DateTime.ParseExact(fromDate.Text, "yyyy-MM-dd", null);
                employeeBO.ToDate = DateTime.ParseExact(toDate.Text, "yyyy-MM-dd", null);
                string fromDateFormatted = employeeBO.FromDate.ToString("MMMM-yyyy");
                string toDateFormatted = employeeBO.ToDate.ToString("MMMM-yyyy");
                int pageNumber = (int)Session["PageNumber"];
                string takenBy = Session["TakenBy"].ToString();
                DateTime currentDate = (DateTime)Session["CurrentDate"];
                DAL.BranchDetails branchDetails = hrmsSalarySlipDA.GetBranchDetails();

                string currentDateTime = DateTime.Now.ToString("yyyy-MM");
                string fileName = $"Salary Register for Employee-{currentDateTime}.pdf";
                Document document = new Document();
                string logoPath = HttpContext.Current.Server.MapPath("~/Images/logo.png");
                iTextSharp.text.Image logoImage = iTextSharp.text.Image.GetInstance(logoPath);
                logoImage.ScaleAbsolute(90, 50);

                using (MemoryStream ms = new MemoryStream())
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    PdfHeaderFooter header = new PdfHeaderFooter { Logo = logoImage };
                    writer.PageEvent = header;

                    document.Open();

                    // Define fonts
                    Font font = FontFactory.GetFont(FontFactory.COURIER, BaseFont.CP1257, BaseFont.NOT_EMBEDDED, 8);
                    Font boldFont = FontFactory.GetFont(FontFactory.COURIER_BOLD, BaseFont.CP1257, BaseFont.NOT_EMBEDDED, 8);

                    string headingText = $"Salary Register for Employee: {fromDateFormatted} to {toDateFormatted}";
                    Paragraph headingParagraph = new Paragraph
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    Chunk chunk = new Chunk(headingText, boldFont);
                    headingParagraph.Add(chunk);

                    document.Add(new Paragraph("\n\n\n", font));
                    document.Add(headingParagraph);
                    document.Add(new Paragraph("\n\n", font));
                    document.Add(new Paragraph($"{branchDetails.BrName}", boldFont));
                    document.Add(new Paragraph($"                                                                                           Page No: {pageNumber}", boldFont));
                    document.Add(new Paragraph($"                                                                                           Taken By: {takenBy}", boldFont));
                    document.Add(new Paragraph($"                                                                                           Date: {DateTime.Now.ToString("dd.MM.yyyy")}", boldFont));
                    document.Add(new Paragraph("\n\n", font));

                    DataTable dt = employeeSalaryReport.SalaryRegisterForemployee(employeeBO);

                    // Table for Employee No and Name
                    PdfPTable empTable = new PdfPTable(2);
                    empTable.WidthPercentage = 30;
                    empTable.SetWidths(new float[] { 5, 5 });
                    AddCellToHeader(empTable, "Emp No", boldFont);
                    AddCellToHeader(empTable, "Emp Name", boldFont);

                    // Add EmpNo and EmpName only once
                    if (dt.Rows.Count > 0)
                    {
                        AddCellToBody(empTable, dt.Rows[0]["Pay_EmpNo"].ToString(), boldFont);
                        AddCellToBody(empTable, dt.Rows[0]["EmpName"].ToString(), boldFont);
                    }

                    PdfPTable firstTable = new PdfPTable(8);
                    firstTable.WidthPercentage = 100;
                    firstTable.SetWidths(new float[] { 5, 5, 7, 7, 7, 7, 7, 7 });

                    AddCellToHeader(firstTable, "Month", boldFont);
                    AddCellToHeader(firstTable, "Year", boldFont);
                    AddCellToHeader(firstTable, "Basic", boldFont);
                    AddCellToHeader(firstTable, "DA", boldFont);
                    AddCellToHeader(firstTable, "HRA", boldFont);
                    AddCellToHeader(firstTable, "CCA", boldFont);
                    AddCellToHeader(firstTable, "Conv.", boldFont);
                    AddCellToHeader(firstTable, "Adhoc", boldFont);

                    PdfPTable secondTable = new PdfPTable(8);
                    secondTable.WidthPercentage = 100;
                    secondTable.SetWidths(new float[] { 7, 7, 7, 7, 10, 10, 10, 10 });

                    AddCellToHeader(secondTable, "Month", boldFont);
                    AddCellToHeader(secondTable, "Year", boldFont);
                    AddCellToHeader(secondTable, "PF", boldFont);
                    AddCellToHeader(secondTable, "PT", boldFont);
                    AddCellToHeader(secondTable, "LIC", boldFont);
                    AddCellToHeader(secondTable, "IT", boldFont);
                    AddCellToHeader(secondTable, "Other Earnings", boldFont);
                    AddCellToHeader(secondTable, "Other Deductions", boldFont);

                    PdfPTable thirdTable = new PdfPTable(5);
                    thirdTable.WidthPercentage = 100;
                    thirdTable.SetWidths(new float[] { 10, 10, 10, 10, 10 });
                    AddCellToHeader(thirdTable, "Month", boldFont);
                    AddCellToHeader(thirdTable, "Year", boldFont);
                    AddCellToHeader(thirdTable, "Gross Earnings", boldFont);
                    AddCellToHeader(thirdTable, "Gross Deductions", boldFont);
                    AddCellToHeader(thirdTable, "Net Salary", boldFont);

                    decimal totalBasic = 0;
                    decimal totalDA = 0;
                    decimal totalHRA = 0;
                    decimal totalCCA = 0;
                    decimal totalConv = 0;
                    decimal totalAdhoc = 0;
                    decimal totalPF = 0;
                    decimal totalPT = 0;
                    decimal totalLIC = 0;
                    decimal totalIT = 0;
                    decimal totalOtherEarn = 0;
                    decimal totalOtherDedut = 0;
                    decimal totalGrossEarn = 0;
                    decimal totalGrossDedut = 0;
                    decimal totalNet = 0;

                    // Add data rows
                    foreach (DataRow row in dt.Rows)
                    {
                        AddCellToBody(firstTable, row["StrMonth"].ToString(), boldFont);
                        AddCellToBody(firstTable, row["Pay_Year"].ToString(), boldFont);
                        AddCellToBody(firstTable, row["Pay_Basic"].ToString(), boldFont);
                        AddCellToBody(firstTable, row["Pay_DA"].ToString(), boldFont);
                        AddCellToBody(firstTable, row["Pay_HRA"].ToString(), boldFont);
                        AddCellToBody(firstTable, row["Pay_CCA"].ToString(), boldFont);
                        AddCellToBody(firstTable, row["Pay_Conveyance"].ToString(), boldFont);
                        AddCellToBody(firstTable, row["Pay_Adhoc"].ToString(), boldFont);

                        AddCellToBody(secondTable, row["StrMonth"].ToString(), boldFont);
                        AddCellToBody(secondTable, row["Pay_Year"].ToString(), boldFont);
                        AddCellToBody(secondTable, row["Pay_PF"].ToString(), boldFont);
                        AddCellToBody(secondTable, row["Pay_PT"].ToString(), boldFont);
                        AddCellToBody(secondTable, row["Pay_LIC"].ToString(), boldFont);
                        AddCellToBody(secondTable, row["Pay_IncomeTax"].ToString(), boldFont);
                        AddCellToBody(secondTable, row["Pay_OtherEarn"].ToString(), boldFont);
                        AddCellToBody(secondTable, row["Pay_OtherDed"].ToString(), boldFont);

                        AddCellToBody(thirdTable, row["StrMonth"].ToString(), boldFont);
                        AddCellToBody(thirdTable, row["Pay_Year"].ToString(), boldFont);
                        AddCellToBody(thirdTable, row["Pay_GrossEarn"].ToString(), boldFont);
                        AddCellToBody(thirdTable, row["Pay_GrossDed"].ToString(), boldFont);
                        AddCellToBody(thirdTable, row["Pay_Net"].ToString(), boldFont);

                        // Accumulate totals
                        totalBasic += Convert.ToDecimal(row["Pay_Basic"]);
                        totalDA += Convert.ToDecimal(row["Pay_DA"]);
                        totalHRA += Convert.ToDecimal(row["Pay_HRA"]);
                        totalCCA += row["Pay_CCA"] != DBNull.Value ? Convert.ToDecimal(row["Pay_CCA"]) : 0;
                        totalConv += row["Pay_Conveyance"] != DBNull.Value ? Convert.ToDecimal(row["Pay_Conveyance"]) : 0;
                        totalAdhoc += row["Pay_Adhoc"] != DBNull.Value ? Convert.ToDecimal(row["Pay_Adhoc"]) : 0;

                        totalPF += Convert.ToDecimal(row["Pay_PF"]);
                        totalPT += Convert.ToDecimal(row["Pay_PT"]);
                        totalLIC += Convert.ToDecimal(row["Pay_LIC"]);
                        totalIT += Convert.ToDecimal(row["Pay_IncomeTax"]);
                        totalOtherEarn += Convert.ToDecimal(row["Pay_OtherEarn"]);
                        totalOtherDedut += Convert.ToDecimal(row["Pay_OtherDed"]);

                        totalGrossEarn += Convert.ToDecimal(row["Pay_GrossEarn"]);
                        totalGrossDedut += Convert.ToDecimal(row["Pay_GrossDed"]);
                        totalNet += Convert.ToDecimal(row["Pay_Net"]);
                    }

                    // Add total row for first table
                    PdfPCell totalCellFirstTable = new PdfPCell(new Phrase("         TOTAL: ", boldFont));
                    totalCellFirstTable.Colspan = 2;
                    firstTable.AddCell(totalCellFirstTable);
                    AddCellToBody(firstTable, totalBasic.ToString(), boldFont);
                    AddCellToBody(firstTable, totalDA.ToString(), boldFont);
                    AddCellToBody(firstTable, totalHRA.ToString(), boldFont);
                    AddCellToBody(firstTable, totalCCA.ToString(), boldFont);
                    AddCellToBody(firstTable, totalConv.ToString(), boldFont);
                    AddCellToBody(firstTable, totalAdhoc.ToString(), boldFont);

                    // Add empty cells to match the columns in first table
                    for (int i = 0; i < 6; i++)
                    {
                        firstTable.AddCell(new PdfPCell(new Phrase("", boldFont)));
                    }

                    // Add total row for second table (if needed)
                    PdfPCell totalCellSecondTable = new PdfPCell(new Phrase("         TOTAL: ", boldFont));
                    totalCellSecondTable.Colspan = 2;
                    secondTable.AddCell(totalCellSecondTable);
                    AddCellToBody(secondTable, totalPF.ToString(), boldFont);
                    AddCellToBody(secondTable, totalPT.ToString(), boldFont);
                    AddCellToBody(secondTable, totalLIC.ToString(), boldFont);
                    AddCellToBody(secondTable, totalIT.ToString(), boldFont);
                    AddCellToBody(secondTable, totalOtherEarn.ToString(), boldFont);
                    AddCellToBody(secondTable, totalOtherDedut.ToString(), boldFont);

                    // Add empty cells to match the columns in second table
                    for (int i = 0; i < 6; i++)
                    {
                        secondTable.AddCell(new PdfPCell(new Phrase("", font)));
                    }

                    // Add total row for third table
                    PdfPCell totalCellThirdTable = new PdfPCell(new Phrase("        TOTAL: ", boldFont));
                    totalCellThirdTable.Colspan = 2;
                    thirdTable.AddCell(totalCellThirdTable);
                    AddCellToBody(thirdTable, totalGrossEarn.ToString(), boldFont);
                    AddCellToBody(thirdTable, totalGrossDedut.ToString(), boldFont);
                    AddCellToBody(thirdTable, totalNet.ToString(), boldFont);

                    // Add empty cells to match the columns in third table
                    for (int i = 0; i < 2; i++)
                    {
                        thirdTable.AddCell(new PdfPCell(new Phrase("", font)));
                    }

                    document.Add(empTable); // Add the employee table first
                    document.Add(new Paragraph("\n\n", font)); // Add some space
                    document.Add(firstTable);
                    document.Add(new Paragraph("\n\n", font)); // Add some space between the tables
                    document.Add(secondTable);
                    document.Add(new Paragraph("\n\n", font)); // Add some space between the tables
                    document.Add(thirdTable);
                    document.Close();

                    byte[] fileBytes = ms.ToArray();
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
                    HttpContext.Current.Response.BinaryWrite(fileBytes);
                    HttpContext.Current.Response.End();
                }

                Response.Write("<script>alert('Salary Register for Employee Generated Successfully!');</script>");
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('An error occurred: {ex.Message}');</script>");
            }
        }

        private void AddCellToHeader(PdfPTable table, string text, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cell);
        }

        private void AddCellToBody(PdfPTable table, string text, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);
        }

    }



}