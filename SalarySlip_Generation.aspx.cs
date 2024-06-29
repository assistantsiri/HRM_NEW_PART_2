using BL;
using BO;
using DAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Image = iTextSharp.text.Image;

namespace HRMS
{
    public partial class SalarySlip_Generation : System.Web.UI.Page
    {
        EmployeeBO employeeBO = new EmployeeBO();
        SalaryPaySlipBL salaryPaySlipBL = new SalaryPaySlipBL();
        Salary_Slip_Generation_DA salaryPaySlipDA = new Salary_Slip_Generation_DA();
        BranchDetails branchDetails = new BranchDetails();
        //BranchDetails branchDetails =


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Session["PageNumber"] = 1;
                Session["TakenBy"] = "ADMIN";
                Session["CurrentDate"] = DateTime.Now;

                FillBanks();
               
            }

            Label1.Text = $"SALARY SLIP GENERATION ";

        }


        private void FillBanks()
        {
            DataTable dt = new DataTable();
            EmployeeBO employeeBO = new EmployeeBO();
            employeeBO.Action = "M";
            dt = salaryPaySlipDA.SalaryRegister(employeeBO);
            if (dt.Rows.Count > 0)
            {
                branchDropdown.DataSource = dt;
                branchDropdown.DataTextField = "CBr_Name";
                branchDropdown.DataValueField = "CBr_Code";
                branchDropdown.DataBind();
                //branchDropdown.Items.Insert(0, new ListItem("----- Select -----",""));
                branchDropdown.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
            }
        }

        protected void branchDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dropdown();

        }

        private void Dropdown()
        {
            DataTable dt = new DataTable();
            EmployeeBO employeeBO = new EmployeeBO();
            Count();
            int cnt = (int)Session["Count"];
            employeeBO.BranchCode = Convert.ToInt32(branchDropdown.SelectedValue);
            employeeBO.Action = "Q";
            dt = salaryPaySlipDA.SalaryRegister(employeeBO);
            if (dt.Rows.Count > 0)
            {
                ddlFromEmpNo.DataSource = dt;
                ddlFromEmpNo.DataTextField = "Emp_Name";
                ddlFromEmpNo.DataValueField = "Emp_No";
                ddlFromEmpNo.DataBind();
                ddlFromEmpNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----- Select -----", "0"));
                ddlFromEmpNo.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Min", "1"));


                ddlToEmpNo.DataSource = dt;
                ddlToEmpNo.DataTextField = "Emp_Name";
                ddlToEmpNo.DataValueField = "Emp_No";
                ddlToEmpNo.DataBind();
                ddlToEmpNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----- Select -----", "0"));
                ddlToEmpNo.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Max", cnt.ToString()));



                //ddlFromEmpNo.SelectedValue = dt.Rows[0]["Emp_No"].ToString(); 
                //ddlToEmpNo.SelectedValue = dt.Rows[dt.Rows.Count - 1]["Emp_No"].ToString();
            }
            else
            {
                ddlFromEmpNo.Items.Clear();
                ddlToEmpNo.Items.Clear();
                ddlFromEmpNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Empty"));
                ddlToEmpNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Empty"));

            }
        }


        protected void Count()
        {
            // bO = new EmployeeBO();
            employeeBO.Action = "B";
            DataTable dataTable = salaryPaySlipDA.SalaryRegister(employeeBO);
            if (dataTable.Rows.Count > 0)
            {
                Session["Count"] = dataTable.Rows[0]["Cnt"];
            }
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            int pageNumber = (int)Session["PageNumber"];
            string takenBy = Session["TakenBy"].ToString();
            DateTime currentDate = (DateTime)Session["CurrentDate"];
            Session["Description"] = txtDescription.Text;
            Count();
            //Dropdown();
            DAL.BranchDetails dalBranchDetails = salaryPaySlipDA.GetBranchDetails();


            StringBuilder salarySlip = new StringBuilder();


            string description = Session["Description"].ToString();
            string selectedBranchNames = branchDropdown.SelectedItem.ToString();
            employeeBO.BranchCode = Convert.ToInt32(branchDropdown.SelectedItem.Value);
            string currentTime = DateTime.Now.ToString("dd-MM-yyyy");
            if (employeeBO.BranchCode == 0)
            {



                employeeBO.Action = "A";
                DataTable salaryRegisterData = salaryPaySlipDA.SalaryRegister(employeeBO);

                if (salaryRegisterData != null && salaryRegisterData.Rows.Count > 0)
                {



                    foreach (DataRow row in salaryRegisterData.Rows)
                    {
                        employeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        employeeBO.EmpName = row["Emp_Name"].ToString();
                        employeeBO.DOj = Convert.ToDateTime(row["Emp_JoinDt"]);

                        employeeBO.Designation = row["Designation"].ToString();




                        // Fetch earnings details
                        EmployeeBO earningsEmployeeBO = new EmployeeBO();
                        earningsEmployeeBO.Action = "E"; // Set action to fetch earnings data
                        earningsEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable earningsData = salaryPaySlipDA.SalaryRegister(earningsEmployeeBO);

                        if (earningsData != null && earningsData.Rows.Count > 0)
                        {

                            foreach (DataRow earningRow in earningsData.Rows)
                            {
                                earningsEmployeeBO.ED_Desc = earningRow["ED_Desc"].ToString();
                                earningsEmployeeBO.CuTrAmt = Convert.ToDecimal(earningRow["Cu_Tr_Amt"]);
                                earningsEmployeeBO.Cu_Tr_Code = Convert.ToInt32(earningRow["Cu_Tr_Code"]);

                                switch (earningsEmployeeBO.ED_Desc)
                                {
                                    case "BASIC PAY": //1001
                                        earningsEmployeeBO.CurrentBasic = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    case "DA": //1002
                                        earningsEmployeeBO.DA = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    case "HRA"://1003
                                        earningsEmployeeBO.HRA = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    case "CONVEYANCE"://1005
                                        earningsEmployeeBO.CovAllownance = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    case "ADHOC ALLOWANCE": //1006
                                        earningsEmployeeBO.AdAllownance = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    case "TECHNICAL ALLOWANCE"://2010
                                        earningsEmployeeBO.TECHNICALAllownace = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    //case "LFC"://2002
                                    //    earningsEmployeeBO.TECHNICALAllownace = earningsEmployeeBO.CuTrAmt;
                                    //    break;

                                    case "OTHER EARNINGS": //2001
                                        earningsEmployeeBO.otherEarning = earningsEmployeeBO.CuTrAmt;
                                   
                                        break;
                                    default:
                                        break;
                                }


                            }
                        }
                        else
                        {
                            salarySlip.AppendLine("No earnings data found.");
                        }

                        // Fetch deduction details (Action "D")
                        EmployeeBO deductionEmployeeBO = new EmployeeBO();
                        deductionEmployeeBO.Action = "D"; // Set action to fetch deduction data
                        deductionEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable deductionData = salaryPaySlipDA.SalaryRegister(deductionEmployeeBO);

                        if (deductionData != null && deductionData.Rows.Count > 0)
                        {

                            foreach (DataRow deductionRow in deductionData.Rows)
                            {
                                deductionEmployeeBO.ED_Desc = deductionRow["ED_Desc"].ToString();
                                deductionEmployeeBO.CuTrAmt = Convert.ToDecimal(deductionRow["Cu_Tr_Amt"]);
                                deductionEmployeeBO.Cu_Tr_Code = Convert.ToInt32(deductionRow["Cu_Tr_Code"]);
                                switch (deductionEmployeeBO.ED_Desc)
                                {
                                    case "PF"://5001
                                        deductionEmployeeBO.PF = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "EPF"://5003
                                        deductionEmployeeBO.IT = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "P.TAX": //5002
                                        deductionEmployeeBO.ProofTax = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "FEST ADV RECOVERY 2022"://6031
                                        deductionEmployeeBO.FESTADVRECOVERY2022 = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "L I C":
                                        deductionEmployeeBO.LIC = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "OTHERS":
                                        deductionEmployeeBO.OtherDeduction = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    default:
                                        break;
                                }



                            }

                        }
                        else
                        {
                            salarySlip.AppendLine("No deduction data found.");
                        }

                        // Fetch Net Salary (Action "N")
                        EmployeeBO netSalaryEmployeeBO = new EmployeeBO();
                        netSalaryEmployeeBO.Action = "N"; // Set action to fetch net salary data
                        netSalaryEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable netSalaryData = salaryPaySlipDA.SalaryRegister(netSalaryEmployeeBO);

                        if (netSalaryData != null && netSalaryData.Rows.Count > 0)
                        {

                            foreach (DataRow netSalaryRow in netSalaryData.Rows)
                            {

                                netSalaryEmployeeBO.NetSalary = Convert.ToDecimal(netSalaryRow["NetSalary"]);
                            }

                        }
                        else
                        {
                            salarySlip.AppendLine("No net salary data found.");
                        }


                        EmployeeBO totalEarningsEmployeeBO = new EmployeeBO();
                        totalEarningsEmployeeBO.Action = "G";
                        totalEarningsEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable totalEarningsData = salaryPaySlipDA.SalaryRegister(totalEarningsEmployeeBO);

                        if (totalEarningsData != null && totalEarningsData.Rows.Count > 0)
                        {

                            foreach (DataRow totalEarningsRow in totalEarningsData.Rows)
                            {
                                totalEarningsEmployeeBO.GrossEarning = Convert.ToDecimal(totalEarningsRow["EarningsAmount"]);
                            }

                        }
                        else
                        {
                            salarySlip.AppendLine("No total earnings data found.");
                        }


                        // Fetch Other Earnings Amount (Action "O")
                        EmployeeBO OtherEarningemployeeBO = new EmployeeBO();
                        OtherEarningemployeeBO.Action = "O";
                        OtherEarningemployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable OtherEarningData = salaryPaySlipDA.SalaryRegister(OtherEarningemployeeBO);
                        if (OtherEarningData != null && OtherEarningData.Rows.Count > 0)
                        {
                            foreach (DataRow otherEarningsRow in OtherEarningData.Rows)
                            {
                                if (otherEarningsRow["OtherEarningsAmount"] != DBNull.Value)
                                {
                                    OtherEarningemployeeBO.otherEarning = Convert.ToDecimal(otherEarningsRow["OtherEarningsAmount"]);
                                }
                                else
                                {
                                    // Handle DBNull case here, such as assigning a default value or logging a message
                                }

                            }
                        }
                        else
                        {
                            salarySlip.AppendLine("No Other Earnings Amount data found.");
                        }

                        // Fetch Other Deductions Amount (Action "S")
                        EmployeeBO OtherdeductionemployeeBO = new EmployeeBO();
                        OtherdeductionemployeeBO.Action = "S";
                        OtherdeductionemployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable OtherDeductionData = salaryPaySlipDA.SalaryRegister(OtherdeductionemployeeBO);
                        if (OtherDeductionData != null && OtherDeductionData.Rows.Count > 0)
                        {
                            foreach (DataRow otherDeductionsRow in OtherDeductionData.Rows)
                            {
                                if (otherDeductionsRow["otherDeductionsAmount"] != DBNull.Value)
                                {
                                    OtherdeductionemployeeBO.OtherDeduction = Convert.ToDecimal(otherDeductionsRow["otherDeductionsAmount"]);
                                }
                                else
                                {
                                    // Handle DBNull case here, such as assigning a default value or logging a message
                                }

                            }
                        }
                        else
                        {
                            salarySlip.AppendLine("No Other Deductions Amount data found.");
                        }


                        EmployeeBO totalDeductionsEmployeeBO = new EmployeeBO();
                        totalDeductionsEmployeeBO.Action = "L";
                        totalDeductionsEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable totalDeductionsData = salaryPaySlipDA.SalaryRegister(totalDeductionsEmployeeBO);

                        if (totalDeductionsData != null && totalDeductionsData.Rows.Count > 0)
                        {

                            foreach (DataRow totalDeductionsRow in totalDeductionsData.Rows)
                            {
                                totalDeductionsEmployeeBO.GrossDeduction = Convert.ToDecimal(totalDeductionsRow["Total_Cu_Tr_Amt"]);
                            }

                        }
                        else
                        {
                            salarySlip.AppendLine("No total deductions data found.");
                        }
                       
                        if (employeeBO != null && employeeBO.EmpNo != null && employeeBO.EmpName != null)
                        {
                            string currentDateTime = DateTime.Now.ToString("yyyy-MM");
                            string fileName = $"SalarySlip_Emp{employeeBO.EmpNo}_{employeeBO.EmpName.Replace(" ", "_")}_{currentDateTime}.pdf";

                            // Map file path
                            string rootDirectoryPath = HttpContext.Current.Server.MapPath("~/App_Data/Report/SalarySlip");
                            string filePath = Path.Combine(rootDirectoryPath, fileName);

                            // Create PDF document
                            Document document = new Document();

                            try
                            {
                                string logoPath = @"D:\HRMS-\logo.png"; // Provide the path to your logo image
                                Image logoImage = Image.GetInstance(logoPath);
                                logoImage.ScaleAbsolute(90, 50); // Set the size of the logo image

                                // Initialize PDF writer
                                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                                PdfHeaderFooter header = new PdfHeaderFooter();
                                header.Logo = logoImage; // Set the logo image to the header
                                writer.PageEvent = header;
                                // Add header with logo
                                writer.PageEvent = new PdfHeaderFooter();

                                // Open the document
                                document.Open();
                                Font font = FontFactory.GetFont(FontFactory.COURIER, BaseFont.CP1257, BaseFont.NOT_EMBEDDED, 8);
                                Font boldFont = FontFactory.GetFont(FontFactory.COURIER_BOLD, BaseFont.CP1257, BaseFont.NOT_EMBEDDED, 8);


                                document.Add(new Paragraph("\n", font));
                                // Add branch details

                                string monthYear = DateTime.Now.ToString("MMMM - yyyy");
                                Chunk chunk = new Chunk($"                                        Salary slip for the month of {monthYear}", boldFont);
                                Paragraph paragraph = new Paragraph(chunk);

                                // Add the paragraph to the document
                                document.Add(paragraph);
                                document.Add(new Paragraph("\n", font));
                                document.Add(new Paragraph("\n", font));
                                document.Add(new Paragraph("\n", font));
                                document.Add(new Paragraph($"{dalBranchDetails.BrName}", font));
                                document.Add(new Paragraph($"{dalBranchDetails.BrAdd1}", font));
                                document.Add(new Paragraph($"{dalBranchDetails.BrAdd2}", font));
                                document.Add(new Paragraph($"{dalBranchDetails.BrPin}", font));
                                document.Add(new Paragraph($"{dalBranchDetails.BrCity}", font));
                                // Add empty lines for separation
                                document.Add(new Paragraph("\n", font));
                                document.Add(new Paragraph("\n", font));

                                // Add employee details
                              //  document.Add(new Paragraph($"                                                                           EmpNo      : {employeeBO.EmpNo}", font));
                                document.Add(new Paragraph($"                                                                    EmpNo      : {employeeBO.EmpNo}", font));
                                document.Add(new Paragraph($"                                                                    EmpName    : {employeeBO.EmpName}", font));
                                document.Add(new Paragraph($"                                                                    Designation: {employeeBO.Designation}", font));
                                document.Add(new Paragraph($"                                                                    Dept       : {selectedBranchNames}", font));
                                document.Add(new Paragraph("\n", font));
                                document.Add(new Paragraph("\n", font));
                                PdfPTable table = new PdfPTable(4);
                                table.WidthPercentage = 100;
                                table.SetWidths(new float[] { 70, 70, 70, 70 });

                                // Add table header
                                table.AddCell(new PdfPCell(new Phrase("Earnings", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                table.AddCell(new PdfPCell(new Phrase("Amount", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                table.AddCell(new PdfPCell(new Phrase("Deductions", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                table.AddCell(new PdfPCell(new Phrase("Amount", font)) { HorizontalAlignment = Element.ALIGN_CENTER });

                                // Add table content with specified font
                                table.AddCell(new PdfPCell(new Phrase("BasicPay", font)));
                                table.AddCell(new PdfPCell(new Phrase(earningsEmployeeBO.CurrentBasic.ToString("0.00"), font)));
                                table.AddCell(new PdfPCell(new Phrase("PF", font)));
                                table.AddCell(new PdfPCell(new Phrase(deductionEmployeeBO.PF.ToString("0.00"), font)));

                                table.AddCell(new PdfPCell(new Phrase("DA", font)));
                                table.AddCell(new PdfPCell(new Phrase(earningsEmployeeBO.DA.ToString("0.00"), font)));
                                table.AddCell(new PdfPCell(new Phrase("P.Tax", font)));
                                table.AddCell(new PdfPCell(new Phrase(deductionEmployeeBO.ProofTax.ToString("0.00"), font)));

                                table.AddCell(new PdfPCell(new Phrase("HRA", font)));
                                table.AddCell(new PdfPCell(new Phrase(earningsEmployeeBO.HRA.ToString("0.00"), font)));
                                table.AddCell(new PdfPCell(new Phrase("Fest ADV Recovery", font)));
                                table.AddCell(new PdfPCell(new Phrase(deductionEmployeeBO.FESTADVRECOVERY2022.ToString("0.00"), font)));

                                table.AddCell(new PdfPCell(new Phrase("CovAllownance", font)));
                                table.AddCell(new PdfPCell(new Phrase(earningsEmployeeBO.CovAllownance.ToString("0.00"), font)));
                                table.AddCell(new PdfPCell(new Phrase("EPF", font)));
                                table.AddCell(new PdfPCell(new Phrase(deductionEmployeeBO.IT.ToString("0.00"), font)));

                                table.AddCell(new PdfPCell(new Phrase("TECHNICAL ALLOWANCE", font)));
                                table.AddCell(new PdfPCell(new Phrase(earningsEmployeeBO.TECHNICALAllownace.ToString("0.00"), font)));
                                table.AddCell(new PdfPCell(new Phrase(" ", font)));
                                table.AddCell(new PdfPCell(new Phrase(" ", font)));

                                table.AddCell(new PdfPCell(new Phrase("GrossEarning", font)));
                                table.AddCell(new PdfPCell(new Phrase(totalEarningsEmployeeBO.GrossEarning.ToString("0.00"), font)));
                                table.AddCell(new PdfPCell(new Phrase("GrossDeduction", font)));
                                table.AddCell(new PdfPCell(new Phrase(totalDeductionsEmployeeBO.GrossDeduction.ToString("0.00"), font)));
                                document.Add(table);

                                // Add net salary
                                document.Add(new Paragraph($"NetSalary Rs: {netSalaryEmployeeBO.NetSalary.ToString("0.00")}", font));
                                document.Add(new Paragraph($"Payable on : {DateTime.Now.ToString("dd-MM-yyyy")}", font));


                                // Add footer
                                document.Add(new Paragraph("---------------------------------*****" + description + "********---------------------------------------------------"));
                            }
                            finally
                            {
                                // Close the document
                                if (document != null)
                                {
                                    document.Close();
                                }
                            }
                        }

                       
                        else
                        {
                            // Handle the case where employee details are not available
                        }




                    }

                }
                else
                {
                    Console.WriteLine("No data found.");
                }


            }
            else if (employeeBO.BranchCode > 0 && ddlFromEmpNo.SelectedValue != "0" && ddlToEmpNo.SelectedValue != "0" && ddlFromEmpNo.SelectedValue != null && ddlToEmpNo.SelectedValue != null)
            {
                employeeBO.BranchCode = Convert.ToInt32(branchDropdown.SelectedItem.Value);
                employeeBO.fromNo = Convert.ToInt32(ddlFromEmpNo.SelectedItem.Value);
                employeeBO.ToNo = Convert.ToInt32(ddlToEmpNo.SelectedItem.Value);






                StringBuilder formattedOutput = new StringBuilder();
                employeeBO.Action = "A";
                DataTable salaryRegisterData = salaryPaySlipDA.SalaryRegister(employeeBO);

                if (salaryRegisterData != null && salaryRegisterData.Rows.Count > 0)
                {



                    foreach (DataRow row in salaryRegisterData.Rows)
                    {
                        employeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        employeeBO.EmpName = row["Emp_Name"].ToString();
                        employeeBO.DOj = Convert.ToDateTime(row["Emp_JoinDt"]);

                        employeeBO.Designation = row["Designation"].ToString();




                        // Fetch earnings details
                        EmployeeBO earningsEmployeeBO = new EmployeeBO();
                        earningsEmployeeBO.Action = "E"; // Set action to fetch earnings data
                        earningsEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable earningsData = salaryPaySlipDA.SalaryRegister(earningsEmployeeBO);

                        if (earningsData != null && earningsData.Rows.Count > 0)
                        {

                            foreach (DataRow earningRow in earningsData.Rows)
                            {
                                earningsEmployeeBO.ED_Desc = earningRow["ED_Desc"].ToString();
                                earningsEmployeeBO.CuTrAmt = Convert.ToDecimal(earningRow["Cu_Tr_Amt"]);
                                earningsEmployeeBO.Cu_Tr_Code = Convert.ToInt32(earningRow["Cu_Tr_Code"]);

                                switch (earningsEmployeeBO.ED_Desc)
                                {
                                    case "BASIC PAY":
                                        earningsEmployeeBO.CurrentBasic = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    case "DA":
                                        earningsEmployeeBO.DA = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    case "HRA":
                                        earningsEmployeeBO.HRA = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    case "CONVEYANCE":
                                        earningsEmployeeBO.CovAllownance = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    case "ADHOC ALLOWANCE":
                                        earningsEmployeeBO.AdAllownance = earningsEmployeeBO.CuTrAmt;
                                        break;
                                        
                                    case "TECHNICAL ALLOWANCE"://2010
                                        earningsEmployeeBO.TECHNICALAllownace = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    case "OTHER EARNINGS":
                                        earningsEmployeeBO.otherEarning = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    default:
                                        break;
                                }



                            }
                        }
                        else
                        {
                            formattedOutput.AppendLine("No earnings data found.");
                        }

                        // Fetch deduction details (Action "D")
                        EmployeeBO deductionEmployeeBO = new EmployeeBO();
                        deductionEmployeeBO.Action = "D"; // Set action to fetch deduction data
                        deductionEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable deductionData = salaryPaySlipDA.SalaryRegister(deductionEmployeeBO);

                        if (deductionData != null && deductionData.Rows.Count > 0)
                        {

                            foreach (DataRow deductionRow in deductionData.Rows)
                            {
                                deductionEmployeeBO.ED_Desc = deductionRow["ED_Desc"].ToString();
                                deductionEmployeeBO.CuTrAmt = Convert.ToDecimal(deductionRow["Cu_Tr_Amt"]);
                                deductionEmployeeBO.Cu_Tr_Code = Convert.ToInt32(deductionRow["Cu_Tr_Code"]);
                                switch (deductionEmployeeBO.ED_Desc)
                                {
                                    case "PF":
                                        deductionEmployeeBO.PF = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "INCOME TAX":
                                        deductionEmployeeBO.IT = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "P.TAX":
                                        deductionEmployeeBO.ProofTax = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "FEST ADV RECOVERY 2022":
                                        deductionEmployeeBO.FESTADVRECOVERY2022 = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "L I C":
                                        deductionEmployeeBO.LIC = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "OTHERS":
                                        deductionEmployeeBO.OtherDeduction = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    default:
                                        break;
                                }



                            }

                        }
                        else
                        {
                            formattedOutput.AppendLine("No deduction data found.");
                        }

                        // Fetch Net Salary (Action "N")
                        EmployeeBO netSalaryEmployeeBO = new EmployeeBO();
                        netSalaryEmployeeBO.Action = "N"; // Set action to fetch net salary data
                        netSalaryEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable netSalaryData = salaryPaySlipDA.SalaryRegister(netSalaryEmployeeBO);

                        if (netSalaryData != null && netSalaryData.Rows.Count > 0)
                        {

                            foreach (DataRow netSalaryRow in netSalaryData.Rows)
                            {

                                netSalaryEmployeeBO.NetSalary = Convert.ToDecimal(netSalaryRow["NetSalary"]);
                            }

                        }
                        else
                        {
                            formattedOutput.AppendLine("No net salary data found.");
                        }


                        EmployeeBO totalEarningsEmployeeBO = new EmployeeBO();
                        totalEarningsEmployeeBO.Action = "G";
                        totalEarningsEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable totalEarningsData = salaryPaySlipDA.SalaryRegister(totalEarningsEmployeeBO);

                        if (totalEarningsData != null && totalEarningsData.Rows.Count > 0)
                        {

                            foreach (DataRow totalEarningsRow in totalEarningsData.Rows)
                            {
                                totalEarningsEmployeeBO.GrossEarning = Convert.ToDecimal(totalEarningsRow["EarningsAmount"]);
                            }

                        }
                        else
                        {
                            formattedOutput.AppendLine("No total earnings data found.");
                        }


                        // Fetch Other Earnings Amount (Action "O")
                        EmployeeBO OtherEarningemployeeBO = new EmployeeBO();
                        OtherEarningemployeeBO.Action = "O";
                        OtherEarningemployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable OtherEarningData = salaryPaySlipDA.SalaryRegister(OtherEarningemployeeBO);
                        if (OtherEarningData != null && OtherEarningData.Rows.Count > 0)
                        {
                            foreach (DataRow otherEarningsRow in OtherEarningData.Rows)
                            {
                                if (otherEarningsRow["OtherEarningsAmount"] != DBNull.Value)
                                {
                                    OtherEarningemployeeBO.otherEarning = Convert.ToDecimal(otherEarningsRow["OtherEarningsAmount"]);
                                }
                                else
                                {
                                    // Handle DBNull case here, such as assigning a default value or logging a message
                                }

                            }
                        }
                        else
                        {
                            formattedOutput.AppendLine("No Other Earnings Amount data found.");
                        }

                        // Fetch Other Deductions Amount (Action "S")
                        EmployeeBO OtherdeductionemployeeBO = new EmployeeBO();
                        OtherdeductionemployeeBO.Action = "S";
                        OtherdeductionemployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable OtherDeductionData = salaryPaySlipDA.SalaryRegister(OtherdeductionemployeeBO);
                        if (OtherDeductionData != null && OtherDeductionData.Rows.Count > 0)
                        {
                            foreach (DataRow otherDeductionsRow in OtherDeductionData.Rows)
                            {
                                if (otherDeductionsRow["otherDeductionsAmount"] != DBNull.Value)
                                {
                                    OtherdeductionemployeeBO.OtherDeduction = Convert.ToDecimal(otherDeductionsRow["otherDeductionsAmount"]);
                                }
                                else
                                {
                                    // Handle DBNull case here, such as assigning a default value or logging a message
                                }

                            }
                        }
                        else
                        {
                            formattedOutput.AppendLine("No Other Deductions Amount data found.");
                        }


                        EmployeeBO totalDeductionsEmployeeBO = new EmployeeBO();
                        totalDeductionsEmployeeBO.Action = "L";
                        totalDeductionsEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable totalDeductionsData = salaryPaySlipDA.SalaryRegister(totalDeductionsEmployeeBO);

                        if (totalDeductionsData != null && totalDeductionsData.Rows.Count > 0)
                        {

                            foreach (DataRow totalDeductionsRow in totalDeductionsData.Rows)
                            {
                                totalDeductionsEmployeeBO.GrossDeduction = Convert.ToDecimal(totalDeductionsRow["DeductionsAmount"]);
                            }

                        }
                        else
                        {

                        }              
                        if (employeeBO != null && employeeBO.EmpNo != null && employeeBO.EmpName != null)
                        {
                            string currentDateTime = DateTime.Now.ToString("yyyy-MM");
                            string fileName = $"SalarySlip_Emp{employeeBO.EmpNo}_{employeeBO.EmpName.Replace(" ", "_")}_{currentDateTime}.pdf";

                            // Map file path
                            string rootDirectoryPath = HttpContext.Current.Server.MapPath("~/App_Data/Report/SalarySlip");
                            string filePath = Path.Combine(rootDirectoryPath, fileName);
                            // totalDeductionsEmployeeBO.GrossDeduction   totalEarningsEmployeeBO.GrossEarning
                            
                            // Create PDF document
                            Document document = new Document();

                            try
                            {
                                string logoPath = @"D:\HRMS-\logo.png"; // Provide the path to your logo image
                                Image logoImage = Image.GetInstance(logoPath);
                                logoImage.ScaleAbsolute(90, 50); // Set the size of the logo image

                                // Initialize PDF writer
                                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                                PdfHeaderFooter header = new PdfHeaderFooter();
                                header.Logo = logoImage; // Set the logo image to the header
                                writer.PageEvent = header;
                                // Add header with logo
                                writer.PageEvent = new PdfHeaderFooter();

                                // Open the document
                                document.Open();
                                Font font = FontFactory.GetFont(FontFactory.COURIER, BaseFont.CP1257, BaseFont.NOT_EMBEDDED, 15);
                                Font boldFont = FontFactory.GetFont(FontFactory.COURIER_BOLD, BaseFont.CP1257, BaseFont.NOT_EMBEDDED, 15);


                                document.Add(new Paragraph("\n", font));
                                // Add branch details

                                string monthYear = DateTime.Now.ToString("MMMM - yyyy");
                                Chunk chunk = new Chunk($"                                        Salary slip for the month of {monthYear}", boldFont);
                                Paragraph paragraph = new Paragraph(chunk);

                                // Add the paragraph to the document
                                document.Add(paragraph);
                                document.Add(new Paragraph("\n", font));
                                document.Add(new Paragraph("\n", font));
                                document.Add(new Paragraph("\n", font));
                                document.Add(new Paragraph($"{dalBranchDetails.BrName}", font));
                                document.Add(new Paragraph($"{dalBranchDetails.BrAdd1}", font));
                                document.Add(new Paragraph($"{dalBranchDetails.BrAdd2}", font));
                                document.Add(new Paragraph($"{dalBranchDetails.BrPin},BANGALORE.", font));
                                document.Add(new Paragraph($"{dalBranchDetails.BrCity}", font));
                                
                                // Add empty lines for separation
                                document.Add(new Paragraph("\n", font));
                                document.Add(new Paragraph("\n", font));

                                // Add employee details
                                //  document.Add(new Paragraph($"                                                                           EmpNo      : {employeeBO.EmpNo}", font));
                                document.Add(new Paragraph($"                                                                    EmpNo      : {employeeBO.EmpNo}", font));
                                document.Add(new Paragraph($"                                                                    EmpName    : {employeeBO.EmpName}", font));
                                document.Add(new Paragraph($"                                                                    Designation: {employeeBO.Designation}", font));
                                document.Add(new Paragraph($"                                                                    Dept: {selectedBranchNames}", font));
                                document.Add(new Paragraph("\n", font));
                                document.Add(new Paragraph("\n", font));
                                PdfPTable table = new PdfPTable(4);
                                table.WidthPercentage = 100;
                                table.SetWidths(new float[] { 30, 30, 30, 30 });

                                // Add table header
                                table.AddCell(new PdfPCell(new Phrase("Earnings", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                table.AddCell(new PdfPCell(new Phrase("Amount", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                table.AddCell(new PdfPCell(new Phrase("Deductions", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                table.AddCell(new PdfPCell(new Phrase("Amount", font)) { HorizontalAlignment = Element.ALIGN_CENTER });

                                // Add table content with specified font
                                table.AddCell(new PdfPCell(new Phrase("Basic Pay", font)));
                                table.AddCell(new PdfPCell(new Phrase(earningsEmployeeBO.CurrentBasic.ToString("0.00"), font)));
                                table.AddCell(new PdfPCell(new Phrase("PF", font)));
                                table.AddCell(new PdfPCell(new Phrase(deductionEmployeeBO.PF.ToString("0.00"), font)));

                                table.AddCell(new PdfPCell(new Phrase("DA", font)));
                                table.AddCell(new PdfPCell(new Phrase(earningsEmployeeBO.DA.ToString("0.00"), font)));
                                table.AddCell(new PdfPCell(new Phrase("P.Tax", font)));
                                table.AddCell(new PdfPCell(new Phrase(deductionEmployeeBO.ProofTax.ToString("0.00"), font)));

                                table.AddCell(new PdfPCell(new Phrase("HRA", font)));
                                table.AddCell(new PdfPCell(new Phrase(earningsEmployeeBO.HRA.ToString("0.00"), font)));
                                table.AddCell(new PdfPCell(new Phrase("Fest ADV Recovery", font)));
                                table.AddCell(new PdfPCell(new Phrase(deductionEmployeeBO.FESTADVRECOVERY2022.ToString("0.00"), font)));

                                table.AddCell(new PdfPCell(new Phrase("CovAllownance", font)));
                                table.AddCell(new PdfPCell(new Phrase(earningsEmployeeBO.CovAllownance.ToString("0.00"), font)));
                                table.AddCell(new PdfPCell(new Phrase("INCOME TAX", font)));
                                table.AddCell(new PdfPCell(new Phrase(deductionEmployeeBO.IT.ToString("0.00"), font)));

                                table.AddCell(new PdfPCell(new Phrase("TECHNICAL ALLOWANCE", font)));
                                table.AddCell(new PdfPCell(new Phrase(earningsEmployeeBO.TECHNICALAllownace.ToString("0.00"), font)));
                                table.AddCell(new PdfPCell(new Phrase(" ", font)));
                                table.AddCell(new PdfPCell(new Phrase(" ", font)));

                                table.AddCell(new PdfPCell(new Phrase("Gross Earning", font)));
                                table.AddCell(new PdfPCell(new Phrase(totalEarningsEmployeeBO.GrossEarning.ToString("0.00"), font)));
                                table.AddCell(new PdfPCell(new Phrase("Gross Deduction", font)));
                                table.AddCell(new PdfPCell(new Phrase(totalDeductionsEmployeeBO.GrossDeduction.ToString("0.00"), font)));
                                document.Add(table);

                                // Add net salary
                                document.Add(new Paragraph($"Net Salary Rs: {netSalaryEmployeeBO.NetSalary.ToString("0.00")}", font));
                                document.Add(new Paragraph($"Payable on : {DateTime.Now.ToString("dd-MM-yyyy")}", font));



                                // Add footer
                                document.Add(new Paragraph("---------------------------------*****" + description + "********---------------------------------------------------"));
                            }
                            finally
                            {
                                // Close the document
                                if (document != null)
                                {
                                    document.Close();
                                }
                            }
                        }
                        else
                        {
                            // Handle the case where employee details are not available
                        }

                        Response.Write($"Salary Slip generated Succesfully");

                    }

                }
                else
                {
                    Console.WriteLine("No data found.");
                }
            

        }
            else if (employeeBO.BranchCode > 0 && ddlFromEmpNo.SelectedValue == "0" && ddlToEmpNo.SelectedValue == "0")
            {
                employeeBO.fromNo = 0;
                int cnt = (int)Session["Count"];
                employeeBO.ToNo = cnt;

                StringBuilder formattedOutput = new StringBuilder();
                employeeBO.Action = "A";
                DataTable salaryRegisterData = salaryPaySlipDA.SalaryRegister(employeeBO);

                if (salaryRegisterData != null && salaryRegisterData.Rows.Count > 0)
                {



                    foreach (DataRow row in salaryRegisterData.Rows)
                    {
                        employeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        employeeBO.EmpName = row["Emp_Name"].ToString();
                        employeeBO.DOj = Convert.ToDateTime(row["Emp_JoinDt"]);

                        employeeBO.Designation = row["Designation"].ToString();




                        // Fetch earnings details
                        EmployeeBO earningsEmployeeBO = new EmployeeBO();
                        earningsEmployeeBO.Action = "E"; // Set action to fetch earnings data
                        earningsEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable earningsData = salaryPaySlipDA.SalaryRegister(earningsEmployeeBO);

                        if (earningsData != null && earningsData.Rows.Count > 0)
                        {

                            foreach (DataRow earningRow in earningsData.Rows)
                            {
                                earningsEmployeeBO.ED_Desc = earningRow["ED_Desc"].ToString();
                                earningsEmployeeBO.CuTrAmt = Convert.ToDecimal(earningRow["Cu_Tr_Amt"]);
                                earningsEmployeeBO.Cu_Tr_Code = Convert.ToInt32(earningRow["Cu_Tr_Code"]);

                                switch (earningsEmployeeBO.ED_Desc)
                                {
                                    case "BASIC PAY":
                                        earningsEmployeeBO.CurrentBasic = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    case "DA":
                                        earningsEmployeeBO.DA = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    case "HRA":
                                        earningsEmployeeBO.HRA = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    case "CONVEYANCE":
                                        earningsEmployeeBO.CovAllownance = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    case "ADHOC ALLOWANCE":
                                        earningsEmployeeBO.AdAllownance = earningsEmployeeBO.CuTrAmt;
                                        break;
                                       
                                    case "TECHNICAL ALLOWANCE"://2010
                                        earningsEmployeeBO.TECHNICALAllownace = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    case "OTHER EARNINGS":
                                        earningsEmployeeBO.otherEarning = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    default:
                                        break;
                                }



                            }
                        }
                        else
                        {
                            formattedOutput.AppendLine("No earnings data found.");
                        }

                        // Fetch deduction details (Action "D")
                        EmployeeBO deductionEmployeeBO = new EmployeeBO();
                        deductionEmployeeBO.Action = "D"; // Set action to fetch deduction data
                        deductionEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable deductionData = salaryPaySlipDA.SalaryRegister(deductionEmployeeBO);

                        if (deductionData != null && deductionData.Rows.Count > 0)
                        {

                            foreach (DataRow deductionRow in deductionData.Rows)
                            {
                                deductionEmployeeBO.ED_Desc = deductionRow["ED_Desc"].ToString();
                                deductionEmployeeBO.CuTrAmt = Convert.ToDecimal(deductionRow["Cu_Tr_Amt"]);
                                deductionEmployeeBO.Cu_Tr_Code = Convert.ToInt32(deductionRow["Cu_Tr_Code"]);
                                switch (deductionEmployeeBO.ED_Desc)
                                {
                                    case "PF":
                                        deductionEmployeeBO.PF = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "INCOME TAX":
                                        deductionEmployeeBO.IT = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "P.TAX":
                                        deductionEmployeeBO.ProofTax = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "FEST ADV RECOVERY 2022":
                                        deductionEmployeeBO.FESTADVRECOVERY2022 = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "L I C":
                                        deductionEmployeeBO.LIC = deductionEmployeeBO.CuTrAmt;
                                        break;
                                       
                                    case "OTHERS":
                                        deductionEmployeeBO.OtherDeduction = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    default:
                                        break;
                                }



                            }

                        }
                        else
                        {
                            formattedOutput.AppendLine("No deduction data found.");
                        }

                        // Fetch Net Salary (Action "N")
                        EmployeeBO netSalaryEmployeeBO = new EmployeeBO();
                        netSalaryEmployeeBO.Action = "N"; // Set action to fetch net salary data
                        netSalaryEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable netSalaryData = salaryPaySlipDA.SalaryRegister(netSalaryEmployeeBO);

                        if (netSalaryData != null && netSalaryData.Rows.Count > 0)
                        {

                            foreach (DataRow netSalaryRow in netSalaryData.Rows)
                            {

                                netSalaryEmployeeBO.NetSalary = Convert.ToDecimal(netSalaryRow["NetSalary"]);
                            }

                        }
                        else
                        {
                            formattedOutput.AppendLine("No net salary data found.");
                        }


                        EmployeeBO totalEarningsEmployeeBO = new EmployeeBO();
                        totalEarningsEmployeeBO.Action = "G";
                        totalEarningsEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable totalEarningsData = salaryPaySlipDA.SalaryRegister(totalEarningsEmployeeBO);

                        if (totalEarningsData != null && totalEarningsData.Rows.Count > 0)
                        {

                            foreach (DataRow totalEarningsRow in totalEarningsData.Rows)
                            {
                                totalEarningsEmployeeBO.GrossEarning = Convert.ToDecimal(totalEarningsRow["EarningsAmount"]);
                            }

                        }
                        else
                        {
                            formattedOutput.AppendLine("No total earnings data found.");
                        }


                        // Fetch Other Earnings Amount (Action "O")
                        EmployeeBO OtherEarningemployeeBO = new EmployeeBO();
                        OtherEarningemployeeBO.Action = "O";
                        OtherEarningemployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable OtherEarningData = salaryPaySlipDA.SalaryRegister(OtherEarningemployeeBO);
                        if (OtherEarningData != null && OtherEarningData.Rows.Count > 0)
                        {
                            foreach (DataRow otherEarningsRow in OtherEarningData.Rows)
                            {
                                if (otherEarningsRow["OtherEarningsAmount"] != DBNull.Value)
                                {
                                    OtherEarningemployeeBO.otherEarning = Convert.ToDecimal(otherEarningsRow["OtherEarningsAmount"]);
                                }
                                else
                                {
                                    // Handle DBNull case here, such as assigning a default value or logging a message
                                }

                            }
                        }
                        else
                        {
                            formattedOutput.AppendLine("No Other Earnings Amount data found.");
                        }

                        // Fetch Other Deductions Amount (Action "S")
                        EmployeeBO OtherdeductionemployeeBO = new EmployeeBO();
                        OtherdeductionemployeeBO.Action = "S";
                        OtherdeductionemployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable OtherDeductionData = salaryPaySlipDA.SalaryRegister(OtherdeductionemployeeBO);
                        if (OtherDeductionData != null && OtherDeductionData.Rows.Count > 0)
                        {
                            foreach (DataRow otherDeductionsRow in OtherDeductionData.Rows)
                            {
                                if (otherDeductionsRow["otherDeductionsAmount"] != DBNull.Value)
                                {
                                    OtherdeductionemployeeBO.OtherDeduction = Convert.ToDecimal(otherDeductionsRow["otherDeductionsAmount"]);
                                }
                                else
                                {
                                    // Handle DBNull case here, such as assigning a default value or logging a message
                                }

                            }
                        }
                        else
                        {
                            formattedOutput.AppendLine("No Other Deductions Amount data found.");
                        }


                        EmployeeBO totalDeductionsEmployeeBO = new EmployeeBO();
                        totalDeductionsEmployeeBO.Action = "L";
                        totalDeductionsEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable totalDeductionsData = salaryPaySlipDA.SalaryRegister(totalDeductionsEmployeeBO);

                        if (totalDeductionsData != null && totalDeductionsData.Rows.Count > 0)
                        {

                            foreach (DataRow totalDeductionsRow in totalDeductionsData.Rows)
                            {
                                totalDeductionsEmployeeBO.GrossDeduction = Convert.ToDecimal(totalDeductionsRow["Total_Cu_Tr_Amt"]);
                            }

                        }
                        else
                        {

                        }

                       
                        if (employeeBO != null && employeeBO.EmpNo != null && employeeBO.EmpName != null)
                        {
                            string currentDateTime = DateTime.Now.ToString("yyyy-MM");
                            string fileName = $"SalarySlip_Emp{employeeBO.EmpNo}_{employeeBO.EmpName.Replace(" ", "_")}_{currentDateTime}.pdf";

                            // Map file path
                            string rootDirectoryPath = HttpContext.Current.Server.MapPath("~/App_Data/Report/SalarySlip");
                            string filePath = Path.Combine(rootDirectoryPath, fileName);

                            // Create PDF document
                            Document document = new Document();

                            try
                            {
                                string logoPath = @"D:\HRMS-\logo.png"; // Provide the path to your logo image
                                Image logoImage = Image.GetInstance(logoPath);
                                logoImage.ScaleAbsolute(90, 50); // Set the size of the logo image

                                // Initialize PDF writer
                                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                                PdfHeaderFooter header = new PdfHeaderFooter();
                                header.Logo = logoImage; // Set the logo image to the header
                                writer.PageEvent = header;
                                // Add header with logo
                                writer.PageEvent = new PdfHeaderFooter();

                                // Open the document
                                document.Open();
                                Font font = FontFactory.GetFont(FontFactory.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED, 8);
                                Font boldFont = FontFactory.GetFont(FontFactory.COURIER_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED, 8);


                                document.Add(new Paragraph("\n", font));
                                // Add branch details

                                string monthYear = "MAY";
                                Chunk chunk = new Chunk($"                                        Salary slip for the month of {monthYear}", boldFont);
                                Paragraph paragraph = new Paragraph(chunk);

                                // Add the paragraph to the document
                                document.Add(paragraph);
                                document.Add(new Paragraph("\n", font));
                                document.Add(new Paragraph("\n", font));
                                document.Add(new Paragraph("\n", font));
                                document.Add(new Paragraph($"{dalBranchDetails.BrName}", font));
                                document.Add(new Paragraph($"{dalBranchDetails.BrAdd1}", font));
                                document.Add(new Paragraph($"{dalBranchDetails.BrAdd2}", font));
                                document.Add(new Paragraph($"{dalBranchDetails.BrPin},BANGALORE.", font));
                                document.Add(new Paragraph($"{dalBranchDetails.BrCity}", font));
                                // Add empty lines for separation
                                document.Add(new Paragraph("\n", font));
                                document.Add(new Paragraph("\n", font));

                                // Add employee details
                                //  document.Add(new Paragraph($"                                                                           EmpNo      : {employeeBO.EmpNo}", font));
                                document.Add(new Paragraph($"                                                                    EmpNo      : {employeeBO.EmpNo}", font));
                                document.Add(new Paragraph($"                                                                    EmpName    : {employeeBO.EmpName}", font));
                                document.Add(new Paragraph($"                                                                    Designation: {employeeBO.Designation}", font));
                                document.Add(new Paragraph($"                                                                    Dept: {selectedBranchNames}", font));
                                document.Add(new Paragraph("\n", font));
                                document.Add(new Paragraph("\n", font));
                                PdfPTable table = new PdfPTable(4);
                                table.WidthPercentage = 100;
                                table.SetWidths(new float[] { 30, 30, 30, 30 });

                                // Add table header
                                table.AddCell(new PdfPCell(new Phrase("Earnings", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                table.AddCell(new PdfPCell(new Phrase("Amount", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                table.AddCell(new PdfPCell(new Phrase("Deductions", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                table.AddCell(new PdfPCell(new Phrase("Amount", font)) { HorizontalAlignment = Element.ALIGN_CENTER });

                                // Add table content with specified font
                                table.AddCell(new PdfPCell(new Phrase("Basic Pay", font)));
                                table.AddCell(new PdfPCell(new Phrase(earningsEmployeeBO.CurrentBasic.ToString("0.00"), font)));
                                table.AddCell(new PdfPCell(new Phrase("PF", font)));
                                table.AddCell(new PdfPCell(new Phrase(deductionEmployeeBO.PF.ToString("0.00"), font)));

                                table.AddCell(new PdfPCell(new Phrase("DA", font)));
                                table.AddCell(new PdfPCell(new Phrase(earningsEmployeeBO.DA.ToString("0.00"), font)));
                                table.AddCell(new PdfPCell(new Phrase("P.Tax", font)));
                                table.AddCell(new PdfPCell(new Phrase(deductionEmployeeBO.ProofTax.ToString("0.00"), font)));

                                table.AddCell(new PdfPCell(new Phrase("HRA", font)));
                                table.AddCell(new PdfPCell(new Phrase(earningsEmployeeBO.HRA.ToString("0.00"), font)));
                                table.AddCell(new PdfPCell(new Phrase("Fest ADV Recovery", font)));
                                table.AddCell(new PdfPCell(new Phrase(deductionEmployeeBO.FESTADVRECOVERY2022.ToString("0.00"), font)));

                                table.AddCell(new PdfPCell(new Phrase("CovAllownance", font)));
                                table.AddCell(new PdfPCell(new Phrase(earningsEmployeeBO.CovAllownance.ToString("0.00"), font)));
                                table.AddCell(new PdfPCell(new Phrase("INCOME TAX", font)));
                                table.AddCell(new PdfPCell(new Phrase(deductionEmployeeBO.IT.ToString("0.00"), font)));

                                table.AddCell(new PdfPCell(new Phrase("TECHNICAL ALLOWANCE", font)));
                                table.AddCell(new PdfPCell(new Phrase(earningsEmployeeBO.TECHNICALAllownace.ToString("0.00"), font)));
                                table.AddCell(new PdfPCell(new Phrase(" ", font)));
                                table.AddCell(new PdfPCell(new Phrase(" ", font)));

                                table.AddCell(new PdfPCell(new Phrase("Gross Earning", font)));
                                table.AddCell(new PdfPCell(new Phrase(totalEarningsEmployeeBO.GrossEarning.ToString("0.00"), font)));
                                table.AddCell(new PdfPCell(new Phrase("Gross Deduction", font)));
                                table.AddCell(new PdfPCell(new Phrase(totalDeductionsEmployeeBO.GrossDeduction.ToString("0.00"), font)));
                                document.Add(table);

                                // Add net salary
                                document.Add(new Paragraph($"Net Salary Rs: {netSalaryEmployeeBO.NetSalary.ToString("0.00")}", font));
                                //document.Add(new Paragraph($"Payable on : {DateTime.Now.ToString("dd-MM-yyyy")}", font));
                                DateTime date = new DateTime(2024, 5, 31);
                                document.Add(new Paragraph($"Payable on: {date.ToString("dd-MM-yyyy")}", font));



                                // Add footer
                                document.Add(new Paragraph("---------------------------------*****" + description + "********---------------------------------------------------"));
                            }
                            finally
                            {
                                // Close the document
                                if (document != null)
                                {
                                    document.Close();
                                }
                            }
                            //Response.Write($"Salary Slip generated Succesfully");
                        }
                        else
                        {
                            // Handle the case where employee details are not available
                        }



                    }

                }
                else
                {
                    Console.WriteLine("No data found.");
                }
            }

         }

            string Tab(int count)
        {
            return new string('\t', count);
        }

        // Define a function for trimming and converting to uppercase
        //public string TrimAndUppercase(string input)
        //{


        //    return input.Trim().ToUpper();
        //}
        public static string AddSpace(string input, int desiredLength, string alignment)
        {
            if (input.Length >= desiredLength)
                return input;

            int spacesToAdd = desiredLength - input.Length;
            string spaces = new string(' ', spacesToAdd);

            if (alignment == "L")
                return input + spaces;
            else if (alignment == "R")
                return spaces + input;
            else // Default to left alignment if alignment is not specified or incorrect
                return input + spaces;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "ConfirmCancel", "confirm('Are you sure  want to cancel?');", true);
            // Response.Redirect("~/Default.aspx"); -- it will redirect to homepage
            // if user want to clear any text then , Can apply-----
            // fromDate.Text = "";
            //toDate.Text = "";
            Response.Redirect("~/Home.aspx");
        }



    }

    public class PdfHeaderFooter : PdfPageEventHelper
    {
        public Image Logo { get; set; } // Logo image property

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            PdfPTable headerTable = new PdfPTable(1);
            headerTable.TotalWidth = document.PageSize.Width;
            headerTable.DefaultCell.Border = 0;

            // Add logo to the header
            if (Logo != null)
            {
                PdfPCell cell = new PdfPCell(Logo);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                headerTable.AddCell(cell);
            }

            headerTable.WriteSelectedRows(0, -1, 0, document.Top + 10, writer.DirectContent);
        }
    }
    public class BranchDetails
    {
        public string BrName { get; set; }
        public string BrAdd1 { get; set; }
        public string BrAdd2 { get; set; }
        public string BrAdd3 { get; set; }
        public string BrCity { get; set; }
        public string BrPin { get; set; }
    }
}