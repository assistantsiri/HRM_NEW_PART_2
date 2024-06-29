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
using HRMS.App_Code;
using iTextSharp.text;
using Image = iTextSharp.text.Image;
using iTextSharp.text.pdf;

namespace HRMS
{
    public partial class Salary_Register : System.Web.UI.Page
    {
        EmployeeBO employeeBO = new EmployeeBO();
        SalaryRegisterDA hrmsSalarySlipDA = new SalaryRegisterDA();
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
            // bO = new EmployeeBO();
            employeeBO.Action = "B";
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
                ddlFromEmpNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----- Select -----", "0"));

                ddlFromEmpNo.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Min", "1"));

                ddlToEmpNo.DataSource = dt;
                ddlToEmpNo.DataTextField = "Emp_Name";
                ddlToEmpNo.DataValueField = "Emp_No";
                ddlToEmpNo.DataBind();
                ddlToEmpNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----- Select -----", "0"));
                ddlToEmpNo.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Max", cnt.ToString()));



               
            }
            else
            {
                ddlFromEmpNo.Items.Clear();
                ddlToEmpNo.Items.Clear();
                ddlFromEmpNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Empty"));
                ddlToEmpNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Empty"));
            }
        }
        //protected void branchDropdown_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Dropdown();

        //}
        protected void btnPrint_Click(object sender, EventArgs e)
        {

            //EmployeeBO employeeBO = new EmployeeBO();
            employeeBO.OtherDeduction = employeeBO.CuTrAmt;

            int pageNumber = (int)Session["PageNumber"];


            string takenBy = Session["TakenBy"].ToString();
            DateTime currentDate = (DateTime)Session["CurrentDate"];
            Session["Description"] = txtDescription.Text;
            Count();
            string description = Session["Description"].ToString();
            string selectedBranchNames = ddlBranch.SelectedItem.ToString();
            employeeBO.BranchCode = Convert.ToInt32(ddlBranch.SelectedItem.Value);
            string currentTime = DateTime.Now.ToString("dd-MM-yyyy");
            DAL.BranchDetails branchDetails = hrmsSalarySlipDA.GetBranchDetails();


           

            if (employeeBO.BranchCode == 0)
            {
                decimal totalDA = 0;
                decimal totalHRA = 0;
                decimal totalCovAllownance = 0;
                decimal totalAdAllownanace = 0;
                decimal totalCurrentBasic = 0;
                decimal totalOriginalBasic = 0;
                decimal totalPf = 0;
                decimal totalProfTax = 0;
                decimal totalIT = 0;
                decimal totalLIC = 0;
                decimal totalOtherEarning = 0;
                decimal totalGrossEarning = 0;
                decimal totalGrossDeduction = 0;
                decimal totalOtherDeduction = 0;
                decimal totalNetSalary = 0;
                string currentDateTime = DateTime.Now.ToString("yyyy-MM");
                string fileName = $"Salary Register{currentDateTime}.pdf";
                Document document = new Document();

                employeeBO.Action = "A";
                DataTable dt = hrmsSalarySlipDA.SalaryRegister(employeeBO);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string logoPath = HttpContext.Current.Server.MapPath("~/Images/logo.png");
                    Image logoImage = Image.GetInstance(logoPath);
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

                        int employeesPerPage = 4;
                        int employeesAdded = 0;

                        foreach (DataRow row in dt.Rows)
                        {
                            if (employeesAdded % employeesPerPage == 0)
                            {
                                if (employeesAdded > 0)
                                {
                                    document.NewPage();
                                }

                                int currentYear = DateTime.Now.Year;
                                int previousYear = currentYear;
                                int nextYear = currentYear + 1;
                                string headingText = $"Salary Register for the Year of ({currentDateTime})";

                                Paragraph headingParagraph = new Paragraph
                                {
                                    Alignment = Element.ALIGN_CENTER
                                };
                                Chunk chunk = new Chunk(headingText, boldFont);
                                headingParagraph.Add(chunk);

                                document.Add(new Paragraph("\n\n\n", font));
                                document.Add(headingParagraph);
                                document.Add(new Paragraph("\n\n", font));
                                document.Add(new Paragraph("\n\n", font));
                                document.Add(new Paragraph($"{branchDetails.BrName}", boldFont));
                                document.Add(new Paragraph($"\t\t\t\t\t\t\t\t\t\t                                                                         Page No :{pageNumber}", boldFont));
                                document.Add(new Paragraph($"Dept:{selectedBranchNames}                                                       Taken By : {takenBy}", boldFont));
                                document.Add(new Paragraph($"\t\t\t\t\t\t\t\t\t\t                                                                         Date :{DateTime.Now.ToString("dd.MM.yyyy")}", boldFont));
                                document.Add(new Paragraph("\n\n", font));
                            }

                            employeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                            employeeBO.EmpName = row["Emp_Name"].ToString();
                            employeeBO.DOj = Convert.ToDateTime(row["Emp_JoinDt"]);
                            employeeBO.Designation = row["Designation"].ToString();

                            EmployeeBO EMPLOYEE_A = new EmployeeBO
                            {
                                Action = "E",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable datatableA = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_A);
                            if (datatableA != null && datatableA.Rows.Count > 0)
                            {
                                foreach (DataRow dataRowA in datatableA.Rows)
                                {
                                    EMPLOYEE_A.ED_Desc = dataRowA["ED_Desc"].ToString();
                                    EMPLOYEE_A.Code = Convert.ToInt32(dataRowA["Cu_Tr_Code"]);
                                    EMPLOYEE_A.CuTrAmt = Convert.ToDecimal(dataRowA["Cu_Tr_Amt"]);
                                    EMPLOYEE_A.OriginalBasic = Convert.ToDecimal(dataRowA["OriBasic"]);

                                    switch (EMPLOYEE_A.ED_Desc)
                                    {
                                        case "BASIC PAY":
                                            EMPLOYEE_A.CurrentBasic = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        case "DA":
                                            EMPLOYEE_A.DA = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        case "HRA":
                                            EMPLOYEE_A.HRA = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        case "CONVEYANCE":
                                            EMPLOYEE_A.CovAllownance = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        case "ADHOC ALLOWANCE":
                                            EMPLOYEE_A.AdAllownance = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        case "OriBasic":
                                            EMPLOYEE_A.OriginalBasic = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        case "OTHER EARNINGS":
                                            EMPLOYEE_A.otherEarning = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        default:
                                            break;
                                    }
                                    //totalCurrentBasic += EMPLOYEE_A.CurrentBasic;
                                    //totalOriginalBasic += EMPLOYEE_A.OriginalBasic;
                                    //totalDA += EMPLOYEE_A.DA;
                                    //totalHRA += EMPLOYEE_A.HRA;
                                    //totalAdAllownanace += EMPLOYEE_A.AdAllownance;
                                    //totalCovAllownance += EMPLOYEE_A.CovAllownance;

                                }
                            }

                            EmployeeBO EMPLOYEE_D = new EmployeeBO
                            {
                                Action = "D",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable deductionData = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_D);
                            if (deductionData != null && deductionData.Rows.Count > 0)
                            {

                                foreach (DataRow deductionRow in deductionData.Rows)
                                {
                                    EMPLOYEE_D.ED_Desc = deductionRow["ED_Desc"].ToString();
                                    EMPLOYEE_D.CuTrAmt = Convert.ToDecimal(deductionRow["Cu_Tr_Amt"]);
                                    EMPLOYEE_D.Cu_Tr_Code = Convert.ToInt32(deductionRow["Cu_Tr_Code"]);
                                    switch (EMPLOYEE_D.ED_Desc)
                                    {
                                        case "PF":
                                            EMPLOYEE_D.PF = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        case "INCOME TAXF":
                                            EMPLOYEE_D.IT = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        case "P.TAX":
                                            EMPLOYEE_D.ProofTax = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        case "INCOME TAX":
                                            EMPLOYEE_D.IT = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        case "FEST ADV RECOVERY 2022":
                                            EMPLOYEE_D.FESTADVRECOVERY2022 = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        case "L I C":
                                            EMPLOYEE_D.LIC = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        case "OTHERS":
                                            EMPLOYEE_D.OtherDeduction = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        default:
                                            break;
                                    }

                                    //totalPf += EMPLOYEE_D.PF;
                                    //totalProfTax += EMPLOYEE_D.ProofTax;
                                    //totalLIC += EMPLOYEE_D.LIC;
                                    //totalIT += EMPLOYEE_D.IT;

                                }

                            }

                            EmployeeBO EMPLOYEE_G = new EmployeeBO
                            {
                                Action = "G",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable totalGrossEarnings = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_G);
                            if (totalGrossEarnings != null && totalGrossEarnings.Rows.Count > 0)
                            {

                                foreach (DataRow totalEarningsRow in totalGrossEarnings.Rows)
                                {
                                    EMPLOYEE_G.GrossEarning = Convert.ToDecimal(totalEarningsRow["EarningsAmount"]);
                                }
                                //totalGrossEarning += EMPLOYEE_G.GrossEarning;
                            }

                            EmployeeBO EMPLOYEE_O = new EmployeeBO
                            {
                                Action = "O",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable OtherEarningData = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_O);
                            if (OtherEarningData != null && OtherEarningData.Rows.Count > 0)
                            {
                                foreach (DataRow otherEarningsRow in OtherEarningData.Rows)
                                {
                                    if (otherEarningsRow["OtherEarningsAmount"] != DBNull.Value)
                                    {
                                        EMPLOYEE_O.otherEarning = Convert.ToDecimal(otherEarningsRow["OtherEarningsAmount"]);
                                    }
                                    else
                                    {
                                        // Handle DBNull case here, such as assigning a default value or logging a message
                                    }
                                    //totalOtherEarning += EMPLOYEE_O.otherEarning;
                                }
                            }

                            EmployeeBO EMPLOYEE_S = new EmployeeBO
                            {
                                Action = "S",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable OtherDeductionData = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_S);
                            if (OtherDeductionData != null && OtherDeductionData.Rows.Count > 0)
                            {
                                foreach (DataRow otherDeductionsRow in OtherDeductionData.Rows)
                                {
                                    if (otherDeductionsRow["otherDeductionsAmount"] != DBNull.Value)
                                    {
                                        EMPLOYEE_S.OtherDeduction = Convert.ToDecimal(otherDeductionsRow["otherDeductionsAmount"]);
                                    }
                                    else
                                    {
                                        // Handle DBNull case here, such as assigning a default value or logging a message
                                    }
                                    //totalOtherDeduction += EMPLOYEE_S.OtherDeduction;
                                }
                            }

                            EmployeeBO EMPLOYEE_N = new EmployeeBO
                            {
                                Action = "N",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };

                            DataTable netSalaryData = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_N);

                            if (netSalaryData != null && netSalaryData.Rows.Count > 0)
                            {

                                foreach (DataRow netSalaryRow in netSalaryData.Rows)
                                {

                                    EMPLOYEE_N.NetSalary = Convert.ToDecimal(netSalaryRow["NetSalary"]);
                                }
                                // totalNetSalary += EMPLOYEE_N.NetSalary;
                            }

                            EmployeeBO EMPLOYEE_L = new EmployeeBO
                            {
                                Action = "L",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable totalDeductionsData = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_L);

                            if (totalDeductionsData != null && totalDeductionsData.Rows.Count > 0)
                            {

                                foreach (DataRow totalDeductionsRow in totalDeductionsData.Rows)
                                {
                                    EMPLOYEE_L.GrossDeduction = Convert.ToDecimal(totalDeductionsRow["Total_Cu_Tr_Amt"]);
                                }
                                //totalGrossDeduction += EMPLOYEE_L.GrossDeduction;
                            }


                            PdfPTable table = new PdfPTable(4)
                            {
                                WidthPercentage = 100
                            };
                            table.SetWidths(new float[] { 70, 70, 70, 70 });
                            PdfPCell CreateCenterAlignedCell(string text, Font font2)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(text, font2));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                return cell;
                            }
                            table.AddCell(new PdfPCell(new Phrase("Emp No", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table.AddCell(new PdfPCell(new Phrase("Emp Name", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table.AddCell(new PdfPCell(new Phrase("Designation", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table.AddCell(new PdfPCell(new Phrase("Date of Joining", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table.AddCell(CreateCenterAlignedCell(employeeBO.EmpNo.ToString(), boldFont));
                            table.AddCell(CreateCenterAlignedCell(employeeBO.EmpName, boldFont));
                            table.AddCell(CreateCenterAlignedCell(employeeBO.Designation, boldFont));
                            table.AddCell(CreateCenterAlignedCell(employeeBO.DOj.ToString("yyyy-MM-dd"), boldFont));
                            document.Add(table);


                            PdfPTable table1 = new PdfPTable(8)
                            {
                                WidthPercentage = 100
                            };
                            table1.SetWidths(new float[] { 50, 50, 50, 50, 50, 50, 50, 50 });
                            PdfPCell CreateRightAlignedCell(string text, Font font1)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(text, font1));
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                return cell;
                            }
                            //Heading
                            // table1.AddCell(new PdfPCell(new Phrase("-----", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("ORIGINAL BASIC", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("CURRENT BASIC", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("DA", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("HRA", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("CON.ALLOWANCE", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("AD.ALLOWANCE", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("OTHER EARNING", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("GROSS EARNING", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.OriginalBasic.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.CurrentBasic.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.DA.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.HRA.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.CovAllownance.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.AdAllownance.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.otherEarning.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_G.GrossEarning.ToString("0.00"), boldFont));





                            document.Add(table1);
                            // document.Add(new Paragraph("\n\n\n", font));

                            PdfPTable table2 = new PdfPTable(8)
                            {
                                WidthPercentage = 100
                            };
                            table2.SetWidths(new float[] { 50, 50, 50, 50, 50, 50, 50, 50 });




                            table2.AddCell(new PdfPCell(new Phrase("P.TAX", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("PF", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("I.TAX", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("LIC", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("OTHER DEDUCTION", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("GROSS DEDUCTION", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("NET SALARY", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("---", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });


                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_D.ProofTax.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_D.PF.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_D.IT.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_D.LIC.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_S.OtherDeduction.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_L.GrossDeduction.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_N.NetSalary.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell("---", boldFont));


                            document.Add(table2);


                            document.Add(new Paragraph("************************************************************************************************************", boldFont));

                            totalCurrentBasic += EMPLOYEE_A.CurrentBasic;
                            totalOriginalBasic += EMPLOYEE_A.OriginalBasic;
                            totalDA += EMPLOYEE_A.DA;
                            totalHRA += EMPLOYEE_A.HRA;
                            totalAdAllownanace += EMPLOYEE_A.AdAllownance;
                            totalCovAllownance += EMPLOYEE_A.CovAllownance;
                            totalOtherEarning += EMPLOYEE_O.otherEarning;
                            totalGrossEarning += EMPLOYEE_G.GrossEarning;
                            totalPf += EMPLOYEE_D.PF;
                            totalProfTax += EMPLOYEE_D.ProofTax;
                            totalLIC += EMPLOYEE_D.LIC;
                            totalIT += EMPLOYEE_D.IT;
                            totalGrossDeduction += EMPLOYEE_L.GrossDeduction;
                            totalNetSalary += EMPLOYEE_N.NetSalary;
                            totalOtherDeduction += EMPLOYEE_S.OtherDeduction;
                            document.Add(new Paragraph("\n\n\n", font));
                            employeesAdded++;



                        }


                        document.Add(new Paragraph("\n\n\n", font));
                        document.Add(new Paragraph("\n\n\n", font));
                        PdfPTable table3 = new PdfPTable(1)
                        {
                            WidthPercentage = 100
                        };
                        table3.SetWidths(new float[] { 50 });
                        table3.AddCell(new PdfPCell(new Phrase("TOTAL", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        document.Add(table3);

                        PdfPTable table4 = new PdfPTable(8)
                        {
                            WidthPercentage = 100
                        };
                        table4.SetWidths(new float[] { 50, 50, 50, 50, 50, 50, 50, 50 });
                        PdfPCell CreateRightAlignedCell1(string text, Font font1)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(text, font1));
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            return cell;
                        }
                        //Heading
                        // table1.AddCell(new PdfPCell(new Phrase("-----", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL ORIGINAL BASIC", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL CURRENT BASIC", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL DA", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL HRA", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL CON.ALLOWANCE", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL AD.ALLOWANCE", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL OTHER EARNING", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL GROSS EARNING", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        table4.AddCell(CreateRightAlignedCell1(totalOriginalBasic.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalOriginalBasic.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalDA.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalHRA.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalCovAllownance.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalAdAllownanace.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalOtherEarning.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalGrossEarning.ToString("0.00"), boldFont));





                        document.Add(table4);

                        PdfPTable table5 = new PdfPTable(8)
                        {
                            WidthPercentage = 100
                        };
                        table5.SetWidths(new float[] { 50, 50, 50, 50, 50, 50, 50, 50 });


                        PdfPCell CreateRightAlignedCell2(string text, Font font1)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(text, font1));
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            return cell;
                        }

                        table5.AddCell(new PdfPCell(new Phrase("TOTAL P.TAX", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("TOTAL PF", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("TOTAL I.TAX", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("TOTAL LIC", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("TOTAL OTHER DEDUCTION", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("TOTAL GROSS DEDUCTION", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("TOTAL NET SALARY", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("---", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });


                        table5.AddCell(CreateRightAlignedCell2(totalProfTax.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2(totalPf.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2(totalIT.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2(totalLIC.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2(totalOtherDeduction.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2(totalGrossDeduction.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2(totalNetSalary.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2("---", boldFont));


                        document.Add(table5);

                        document.Close();

                        byte[] fileBytes = ms.ToArray();
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.ContentType = "application/pdf";
                        HttpContext.Current.Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
                        HttpContext.Current.Response.BinaryWrite(fileBytes);
                        HttpContext.Current.Response.End();
                        Response.Write("<script>alert('IT WorkSheet Generated Successfully!');</script>");
                    }
                }
            }

            else if (employeeBO.BranchCode > 0 && ddlFromEmpNo.SelectedValue != "0" && ddlToEmpNo.SelectedValue != "0" && ddlFromEmpNo.SelectedValue != null && ddlToEmpNo.SelectedValue != null)
            {
                employeeBO.BranchCode = Convert.ToInt32(ddlBranch.SelectedItem.Value);
                employeeBO.fromNo = Convert.ToInt32(ddlFromEmpNo.SelectedItem.Value);
                employeeBO.ToNo = Convert.ToInt32(ddlToEmpNo.SelectedItem.Value);
                decimal totalDA = 0;
                decimal totalHRA = 0;
                decimal totalCovAllownance = 0;
                decimal totalAdAllownanace = 0;
                decimal totalCurrentBasic = 0;
                decimal totalOriginalBasic = 0;
                decimal totalPf = 0;
                decimal totalProfTax = 0;
                decimal totalIT = 0;
                decimal totalLIC = 0;
                decimal totalOtherEarning = 0;
                decimal totalGrossEarning = 0;
                decimal totalGrossDeduction = 0;
                decimal totalOtherDeduction = 0;
                decimal totalNetSalary = 0;
                string currentDateTime = DateTime.Now.ToString("yyyy-MM");
                string fileName = $"Salary Register{currentDateTime}.pdf";
                Document document = new Document();

                employeeBO.Action = "A";
                DataTable dt = hrmsSalarySlipDA.SalaryRegister(employeeBO);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string logoPath = HttpContext.Current.Server.MapPath("~/Images/logo.png");
                    Image logoImage = Image.GetInstance(logoPath);
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

                        int employeesPerPage = 4;
                        int employeesAdded = 0;

                        foreach (DataRow row in dt.Rows)
                        {
                            if (employeesAdded % employeesPerPage == 0)
                            {
                                if (employeesAdded > 0)
                                {
                                    document.NewPage();
                                }

                                int currentYear = DateTime.Now.Year;
                                int previousYear = currentYear;
                                int nextYear = currentYear + 1;
                                string headingText = $"Salary Register for the Year of ({currentDateTime})";

                                Paragraph headingParagraph = new Paragraph
                                {
                                    Alignment = Element.ALIGN_CENTER
                                };
                                Chunk chunk = new Chunk(headingText, boldFont);
                                headingParagraph.Add(chunk);

                                document.Add(new Paragraph("\n\n\n", font));
                                document.Add(headingParagraph);
                                document.Add(new Paragraph("\n\n", font));
                                document.Add(new Paragraph("\n\n", font));
                                document.Add(new Paragraph($"{branchDetails.BrName}", boldFont));
                                document.Add(new Paragraph($"\t\t\t\t\t\t\t\t\t\t                                                                         Page No :{pageNumber}", boldFont));
                                document.Add(new Paragraph($"Dept:{selectedBranchNames}                                                       Taken By : {takenBy}", boldFont));
                                document.Add(new Paragraph($"\t\t\t\t\t\t\t\t\t\t                                                                         Date :{DateTime.Now.ToString("dd.MM.yyyy")}", boldFont));
                                document.Add(new Paragraph("\n\n", font));
                            }

                            employeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                            employeeBO.EmpName = row["Emp_Name"].ToString();
                            employeeBO.DOj = Convert.ToDateTime(row["Emp_JoinDt"]);
                            employeeBO.Designation = row["Designation"].ToString();

                            EmployeeBO EMPLOYEE_A = new EmployeeBO
                            {
                                Action = "E",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable datatableA = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_A);
                            if (datatableA != null && datatableA.Rows.Count > 0)
                            {
                                foreach (DataRow dataRowA in datatableA.Rows)
                                {
                                    EMPLOYEE_A.ED_Desc = dataRowA["ED_Desc"].ToString();
                                    EMPLOYEE_A.Code = Convert.ToInt32(dataRowA["Cu_Tr_Code"]);
                                    EMPLOYEE_A.CuTrAmt = Convert.ToDecimal(dataRowA["Cu_Tr_Amt"]);
                                    EMPLOYEE_A.OriginalBasic = Convert.ToDecimal(dataRowA["OriBasic"]);

                                    switch (EMPLOYEE_A.ED_Desc)
                                    {
                                        case "BASIC PAY":
                                            EMPLOYEE_A.CurrentBasic = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        case "DA":
                                            EMPLOYEE_A.DA = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        case "HRA":
                                            EMPLOYEE_A.HRA = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        case "CONVEYANCE":
                                            EMPLOYEE_A.CovAllownance = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        case "ADHOC ALLOWANCE":
                                            EMPLOYEE_A.AdAllownance = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        case "OriBasic":
                                            EMPLOYEE_A.OriginalBasic = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        case "OTHER EARNINGS":
                                            EMPLOYEE_A.otherEarning = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        default:
                                            break;
                                    }
                                    //totalCurrentBasic += EMPLOYEE_A.CurrentBasic;
                                    //totalOriginalBasic += EMPLOYEE_A.OriginalBasic;
                                    //totalDA += EMPLOYEE_A.DA;
                                    //totalHRA += EMPLOYEE_A.HRA;
                                    //totalAdAllownanace += EMPLOYEE_A.AdAllownance;
                                    //totalCovAllownance += EMPLOYEE_A.CovAllownance;

                                }
                            }

                            EmployeeBO EMPLOYEE_D = new EmployeeBO
                            {
                                Action = "D",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable deductionData = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_D);
                            if (deductionData != null && deductionData.Rows.Count > 0)
                            {

                                foreach (DataRow deductionRow in deductionData.Rows)
                                {
                                    EMPLOYEE_D.ED_Desc = deductionRow["ED_Desc"].ToString();
                                    EMPLOYEE_D.CuTrAmt = Convert.ToDecimal(deductionRow["Cu_Tr_Amt"]);
                                    EMPLOYEE_D.Cu_Tr_Code = Convert.ToInt32(deductionRow["Cu_Tr_Code"]);
                                    switch (EMPLOYEE_D.ED_Desc)
                                    {
                                        case "PF":
                                            EMPLOYEE_D.PF = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        case "INCOME TAXF":
                                            EMPLOYEE_D.IT = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        case "P.TAX":
                                            EMPLOYEE_D.ProofTax = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        case "INCOME TAX":
                                            EMPLOYEE_D.IT = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        case "FEST ADV RECOVERY 2022":
                                            EMPLOYEE_D.FESTADVRECOVERY2022 = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        case "L I C":
                                            EMPLOYEE_D.LIC = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        case "OTHERS":
                                            EMPLOYEE_D.OtherDeduction = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        default:
                                            break;
                                    }

                                    //totalPf += EMPLOYEE_D.PF;
                                    //totalProfTax += EMPLOYEE_D.ProofTax;
                                    //totalLIC += EMPLOYEE_D.LIC;
                                    //totalIT += EMPLOYEE_D.IT;

                                }

                            }

                            EmployeeBO EMPLOYEE_G = new EmployeeBO
                            {
                                Action = "G",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable totalGrossEarnings = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_G);
                            if (totalGrossEarnings != null && totalGrossEarnings.Rows.Count > 0)
                            {

                                foreach (DataRow totalEarningsRow in totalGrossEarnings.Rows)
                                {
                                    EMPLOYEE_G.GrossEarning = Convert.ToDecimal(totalEarningsRow["EarningsAmount"]);
                                }
                                //totalGrossEarning += EMPLOYEE_G.GrossEarning;
                            }

                            EmployeeBO EMPLOYEE_O = new EmployeeBO
                            {
                                Action = "O",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable OtherEarningData = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_O);
                            if (OtherEarningData != null && OtherEarningData.Rows.Count > 0)
                            {
                                foreach (DataRow otherEarningsRow in OtherEarningData.Rows)
                                {
                                    if (otherEarningsRow["OtherEarningsAmount"] != DBNull.Value)
                                    {
                                        EMPLOYEE_O.otherEarning = Convert.ToDecimal(otherEarningsRow["OtherEarningsAmount"]);
                                    }
                                    else
                                    {
                                        // Handle DBNull case here, such as assigning a default value or logging a message
                                    }
                                    //totalOtherEarning += EMPLOYEE_O.otherEarning;
                                }
                            }

                            EmployeeBO EMPLOYEE_S = new EmployeeBO
                            {
                                Action = "S",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable OtherDeductionData = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_S);
                            if (OtherDeductionData != null && OtherDeductionData.Rows.Count > 0)
                            {
                                foreach (DataRow otherDeductionsRow in OtherDeductionData.Rows)
                                {
                                    if (otherDeductionsRow["otherDeductionsAmount"] != DBNull.Value)
                                    {
                                        EMPLOYEE_S.OtherDeduction = Convert.ToDecimal(otherDeductionsRow["otherDeductionsAmount"]);
                                    }
                                    else
                                    {
                                        // Handle DBNull case here, such as assigning a default value or logging a message
                                    }
                                    //totalOtherDeduction += EMPLOYEE_S.OtherDeduction;
                                }
                            }

                            EmployeeBO EMPLOYEE_N = new EmployeeBO
                            {
                                Action = "N",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };

                            DataTable netSalaryData = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_N);

                            if (netSalaryData != null && netSalaryData.Rows.Count > 0)
                            {

                                foreach (DataRow netSalaryRow in netSalaryData.Rows)
                                {

                                    EMPLOYEE_N.NetSalary = Convert.ToDecimal(netSalaryRow["NetSalary"]);
                                }
                                // totalNetSalary += EMPLOYEE_N.NetSalary;
                            }

                            EmployeeBO EMPLOYEE_L = new EmployeeBO
                            {
                                Action = "L",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable totalDeductionsData = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_L);

                            if (totalDeductionsData != null && totalDeductionsData.Rows.Count > 0)
                            {

                                foreach (DataRow totalDeductionsRow in totalDeductionsData.Rows)
                                {
                                    EMPLOYEE_L.GrossDeduction = Convert.ToDecimal(totalDeductionsRow["Total_Cu_Tr_Amt"]);
                                }
                                //totalGrossDeduction += EMPLOYEE_L.GrossDeduction;
                            }


                            PdfPTable table = new PdfPTable(4)
                            {
                                WidthPercentage = 100
                            };
                            table.SetWidths(new float[] { 70, 70, 70, 70 });
                            PdfPCell CreateCenterAlignedCell(string text, Font font2)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(text, font2));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                return cell;
                            }
                            table.AddCell(new PdfPCell(new Phrase("Emp No", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table.AddCell(new PdfPCell(new Phrase("Emp Name", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table.AddCell(new PdfPCell(new Phrase("Designation", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table.AddCell(new PdfPCell(new Phrase("Date of Joining", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table.AddCell(CreateCenterAlignedCell(employeeBO.EmpNo.ToString(), boldFont));
                            table.AddCell(CreateCenterAlignedCell(employeeBO.EmpName, boldFont));
                            table.AddCell(CreateCenterAlignedCell(employeeBO.Designation, boldFont));
                            table.AddCell(CreateCenterAlignedCell(employeeBO.DOj.ToString("yyyy-MM-dd"), boldFont));
                            document.Add(table);


                            PdfPTable table1 = new PdfPTable(8)
                            {
                                WidthPercentage = 100
                            };
                            table1.SetWidths(new float[] { 50, 50, 50, 50, 50, 50, 50, 50 });
                            PdfPCell CreateRightAlignedCell(string text, Font font1)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(text, font1));
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                return cell;
                            }
                            //Heading
                            // table1.AddCell(new PdfPCell(new Phrase("-----", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("ORIGINAL BASIC", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("CURRENT BASIC", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("DA", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("HRA", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("CON.ALLOWANCE", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("AD.ALLOWANCE", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("OTHER EARNING", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("GROSS EARNING", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.OriginalBasic.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.CurrentBasic.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.DA.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.HRA.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.CovAllownance.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.AdAllownance.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.otherEarning.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_G.GrossEarning.ToString("0.00"), boldFont));





                            document.Add(table1);
                            // document.Add(new Paragraph("\n\n\n", font));

                            PdfPTable table2 = new PdfPTable(8)
                            {
                                WidthPercentage = 100
                            };
                            table2.SetWidths(new float[] { 50, 50, 50, 50, 50, 50, 50, 50 });




                            table2.AddCell(new PdfPCell(new Phrase("P.TAX", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("PF", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("I.TAX", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("LIC", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("OTHER DEDUCTION", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("GROSS DEDUCTION", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("NET SALARY", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("---", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });


                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_D.ProofTax.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_D.PF.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_D.IT.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_D.LIC.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_S.OtherDeduction.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_L.GrossDeduction.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_N.NetSalary.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell("---", boldFont));


                            document.Add(table2);


                            document.Add(new Paragraph("************************************************************************************************************", boldFont));

                            totalCurrentBasic += EMPLOYEE_A.CurrentBasic;
                            totalOriginalBasic += EMPLOYEE_A.OriginalBasic;
                            totalDA += EMPLOYEE_A.DA;
                            totalHRA += EMPLOYEE_A.HRA;
                            totalAdAllownanace += EMPLOYEE_A.AdAllownance;
                            totalCovAllownance += EMPLOYEE_A.CovAllownance;
                            totalOtherEarning += EMPLOYEE_O.otherEarning;
                            totalGrossEarning += EMPLOYEE_G.GrossEarning;
                            totalPf += EMPLOYEE_D.PF;
                            totalProfTax += EMPLOYEE_D.ProofTax;
                            totalLIC += EMPLOYEE_D.LIC;
                            totalIT += EMPLOYEE_D.IT;
                            totalGrossDeduction += EMPLOYEE_L.GrossDeduction;
                            totalNetSalary += EMPLOYEE_N.NetSalary;
                            totalOtherDeduction += EMPLOYEE_S.OtherDeduction;
                            document.Add(new Paragraph("\n\n\n", font));
                            employeesAdded++;



                        }


                        document.Add(new Paragraph("\n\n\n", font));
                        document.Add(new Paragraph("\n\n\n", font));
                        PdfPTable table3 = new PdfPTable(1)
                        {
                            WidthPercentage = 100
                        };
                        table3.SetWidths(new float[] { 50 });
                        table3.AddCell(new PdfPCell(new Phrase("TOTAL", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        document.Add(table3);

                        PdfPTable table4 = new PdfPTable(8)
                        {
                            WidthPercentage = 100
                        };
                        table4.SetWidths(new float[] { 50, 50, 50, 50, 50, 50, 50, 50 });
                        PdfPCell CreateRightAlignedCell1(string text, Font font1)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(text, font1));
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            return cell;
                        }
                        //Heading
                        // table1.AddCell(new PdfPCell(new Phrase("-----", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL ORIGINAL BASIC", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL CURRENT BASIC", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL DA", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL HRA", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL CON.ALLOWANCE", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL AD.ALLOWANCE", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL OTHER EARNING", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL GROSS EARNING", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        table4.AddCell(CreateRightAlignedCell1(totalOriginalBasic.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalOriginalBasic.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalDA.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalHRA.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalCovAllownance.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalAdAllownanace.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalOtherEarning.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalGrossEarning.ToString("0.00"), boldFont));





                        document.Add(table4);

                        PdfPTable table5 = new PdfPTable(8)
                        {
                            WidthPercentage = 100
                        };
                        table5.SetWidths(new float[] { 50, 50, 50, 50, 50, 50, 50, 50 });


                        PdfPCell CreateRightAlignedCell2(string text, Font font1)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(text, font1));
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            return cell;
                        }

                        table5.AddCell(new PdfPCell(new Phrase("TOTAL P.TAX", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("TOTAL PF", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("TOTAL I.TAX", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("TOTAL LIC", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("TOTAL OTHER DEDUCTION", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("TOTAL GROSS DEDUCTION", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("TOTAL NET SALARY", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("---", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });


                        table5.AddCell(CreateRightAlignedCell2(totalProfTax.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2(totalPf.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2(totalIT.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2(totalLIC.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2(totalOtherDeduction.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2(totalGrossDeduction.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2(totalNetSalary.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2("---", boldFont));


                        document.Add(table5);

                        document.Close();

                        byte[] fileBytes = ms.ToArray();
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.ContentType = "application/pdf";
                        HttpContext.Current.Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
                        HttpContext.Current.Response.BinaryWrite(fileBytes);
                        HttpContext.Current.Response.End();
                        Response.Write("<script>alert('IT WorkSheet Generated Successfully!');</script>");
                    }
                }
            }
            else if (employeeBO.BranchCode > 0 && ddlFromEmpNo.SelectedValue == "0" && ddlToEmpNo.SelectedValue == "0")
            {
                employeeBO.fromNo = 0;
                int cnt = (int)Session["Count"];
                employeeBO.ToNo = cnt;

                decimal totalDA = 0;
                decimal totalHRA = 0;
                decimal totalCovAllownance = 0;
                decimal totalAdAllownanace = 0;
                decimal totalCurrentBasic = 0;
                decimal totalOriginalBasic = 0;
                decimal totalPf = 0;
                decimal totalProfTax = 0;
                decimal totalIT = 0;
                decimal totalLIC = 0;
                decimal totalOtherEarning = 0;
                decimal totalGrossEarning = 0;
                decimal totalGrossDeduction = 0;
                decimal totalOtherDeduction = 0;
                decimal totalNetSalary = 0;
                string currentDateTime = DateTime.Now.ToString("yyyy-MM");
                string fileName = $"Salary Register{currentDateTime}.pdf";
                Document document = new Document();

                employeeBO.Action = "A";
                DataTable dt = hrmsSalarySlipDA.SalaryRegister(employeeBO);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string logoPath = HttpContext.Current.Server.MapPath("~/Images/logo.png");
                    Image logoImage = Image.GetInstance(logoPath);
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

                        int employeesPerPage = 4;
                        int employeesAdded = 0;

                        foreach (DataRow row in dt.Rows)
                        {
                            if (employeesAdded % employeesPerPage == 0)
                            {
                                if (employeesAdded > 0)
                                {
                                    document.NewPage();
                                }

                                int currentYear = DateTime.Now.Year;
                                int previousYear = currentYear;
                                int nextYear = currentYear + 1;
                                string headingText = $"Salary Register for the Year of ({currentDateTime})";

                                Paragraph headingParagraph = new Paragraph
                                {
                                    Alignment = Element.ALIGN_CENTER
                                };
                                Chunk chunk = new Chunk(headingText, boldFont);
                                headingParagraph.Add(chunk);

                                document.Add(new Paragraph("\n\n\n", font));
                                document.Add(headingParagraph);
                                document.Add(new Paragraph("\n\n", font));
                                document.Add(new Paragraph("\n\n", font));
                                document.Add(new Paragraph($"{branchDetails.BrName}", boldFont));
                                document.Add(new Paragraph($"\t\t\t\t\t\t\t\t\t\t                                                                         Page No :{pageNumber}", boldFont));
                                document.Add(new Paragraph($"Dept:{selectedBranchNames}                                                       Taken By : {takenBy}", boldFont));
                                document.Add(new Paragraph($"\t\t\t\t\t\t\t\t\t\t                                                                         Date :{DateTime.Now.ToString("dd.MM.yyyy")}", boldFont));
                                document.Add(new Paragraph("\n\n", font));
                            }

                            employeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                            employeeBO.EmpName = row["Emp_Name"].ToString();
                            employeeBO.DOj = Convert.ToDateTime(row["Emp_JoinDt"]);
                            employeeBO.Designation = row["Designation"].ToString();

                            EmployeeBO EMPLOYEE_A = new EmployeeBO
                            {
                                Action = "E",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable datatableA = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_A);
                            if (datatableA != null && datatableA.Rows.Count > 0)
                            {
                                foreach (DataRow dataRowA in datatableA.Rows)
                                {
                                    EMPLOYEE_A.ED_Desc = dataRowA["ED_Desc"].ToString();
                                    EMPLOYEE_A.Code = Convert.ToInt32(dataRowA["Cu_Tr_Code"]);
                                    EMPLOYEE_A.CuTrAmt = Convert.ToDecimal(dataRowA["Cu_Tr_Amt"]);
                                    EMPLOYEE_A.OriginalBasic = Convert.ToDecimal(dataRowA["OriBasic"]);

                                    switch (EMPLOYEE_A.ED_Desc)
                                    {
                                        case "BASIC PAY":
                                            EMPLOYEE_A.CurrentBasic = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        case "DA":
                                            EMPLOYEE_A.DA = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        case "HRA":
                                            EMPLOYEE_A.HRA = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        case "CONVEYANCE":
                                            EMPLOYEE_A.CovAllownance = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        case "ADHOC ALLOWANCE":
                                            EMPLOYEE_A.AdAllownance = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        case "OriBasic":
                                            EMPLOYEE_A.OriginalBasic = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        case "OTHER EARNINGS":
                                            EMPLOYEE_A.otherEarning = EMPLOYEE_A.CuTrAmt;
                                            break;
                                        default:
                                            break;
                                    }
                                    //totalCurrentBasic += EMPLOYEE_A.CurrentBasic;
                                    //totalOriginalBasic += EMPLOYEE_A.OriginalBasic;
                                    //totalDA += EMPLOYEE_A.DA;
                                    //totalHRA += EMPLOYEE_A.HRA;
                                    //totalAdAllownanace += EMPLOYEE_A.AdAllownance;
                                    //totalCovAllownance += EMPLOYEE_A.CovAllownance;

                                }
                            }

                            EmployeeBO EMPLOYEE_D = new EmployeeBO
                            {
                                Action = "D",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable deductionData = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_D);
                            if (deductionData != null && deductionData.Rows.Count > 0)
                            {

                                foreach (DataRow deductionRow in deductionData.Rows)
                                {
                                    EMPLOYEE_D.ED_Desc = deductionRow["ED_Desc"].ToString();
                                    EMPLOYEE_D.CuTrAmt = Convert.ToDecimal(deductionRow["Cu_Tr_Amt"]);
                                    EMPLOYEE_D.Cu_Tr_Code = Convert.ToInt32(deductionRow["Cu_Tr_Code"]);
                                    switch (EMPLOYEE_D.ED_Desc)
                                    {
                                        case "PF":
                                            EMPLOYEE_D.PF = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        case "INCOME TAXF":
                                            EMPLOYEE_D.IT = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        case "P.TAX":
                                            EMPLOYEE_D.ProofTax = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        case "INCOME TAX":
                                            EMPLOYEE_D.IT = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        case "FEST ADV RECOVERY 2022":
                                            EMPLOYEE_D.FESTADVRECOVERY2022 = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        case "L I C":
                                            EMPLOYEE_D.LIC = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        case "OTHERS":
                                            EMPLOYEE_D.OtherDeduction = EMPLOYEE_D.CuTrAmt;
                                            break;
                                        default:
                                            break;
                                    }

                                    //totalPf += EMPLOYEE_D.PF;
                                    //totalProfTax += EMPLOYEE_D.ProofTax;
                                    //totalLIC += EMPLOYEE_D.LIC;
                                    //totalIT += EMPLOYEE_D.IT;

                                }

                            }

                            EmployeeBO EMPLOYEE_G = new EmployeeBO
                            {
                                Action = "G",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable totalGrossEarnings = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_G);
                            if (totalGrossEarnings != null && totalGrossEarnings.Rows.Count > 0)
                            {

                                foreach (DataRow totalEarningsRow in totalGrossEarnings.Rows)
                                {
                                    EMPLOYEE_G.GrossEarning = Convert.ToDecimal(totalEarningsRow["EarningsAmount"]);
                                }
                                //totalGrossEarning += EMPLOYEE_G.GrossEarning;
                            }

                            EmployeeBO EMPLOYEE_O = new EmployeeBO
                            {
                                Action = "O",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable OtherEarningData = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_O);
                            if (OtherEarningData != null && OtherEarningData.Rows.Count > 0)
                            {
                                foreach (DataRow otherEarningsRow in OtherEarningData.Rows)
                                {
                                    if (otherEarningsRow["OtherEarningsAmount"] != DBNull.Value)
                                    {
                                        EMPLOYEE_O.otherEarning = Convert.ToDecimal(otherEarningsRow["OtherEarningsAmount"]);
                                    }
                                    else
                                    {
                                        // Handle DBNull case here, such as assigning a default value or logging a message
                                    }
                                    //totalOtherEarning += EMPLOYEE_O.otherEarning;
                                }
                            }

                            EmployeeBO EMPLOYEE_S = new EmployeeBO
                            {
                                Action = "S",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable OtherDeductionData = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_S);
                            if (OtherDeductionData != null && OtherDeductionData.Rows.Count > 0)
                            {
                                foreach (DataRow otherDeductionsRow in OtherDeductionData.Rows)
                                {
                                    if (otherDeductionsRow["otherDeductionsAmount"] != DBNull.Value)
                                    {
                                        EMPLOYEE_S.OtherDeduction = Convert.ToDecimal(otherDeductionsRow["otherDeductionsAmount"]);
                                    }
                                    else
                                    {
                                        // Handle DBNull case here, such as assigning a default value or logging a message
                                    }
                                    //totalOtherDeduction += EMPLOYEE_S.OtherDeduction;
                                }
                            }

                            EmployeeBO EMPLOYEE_N = new EmployeeBO
                            {
                                Action = "N",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };

                            DataTable netSalaryData = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_N);

                            if (netSalaryData != null && netSalaryData.Rows.Count > 0)
                            {

                                foreach (DataRow netSalaryRow in netSalaryData.Rows)
                                {

                                    EMPLOYEE_N.NetSalary = Convert.ToDecimal(netSalaryRow["NetSalary"]);
                                }
                               // totalNetSalary += EMPLOYEE_N.NetSalary;
                            }

                            EmployeeBO EMPLOYEE_L = new EmployeeBO
                            {
                                Action = "L",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable totalDeductionsData = hrmsSalarySlipDA.SalaryRegister(EMPLOYEE_L);

                            if (totalDeductionsData != null && totalDeductionsData.Rows.Count > 0)
                            {

                                foreach (DataRow totalDeductionsRow in totalDeductionsData.Rows)
                                {
                                    EMPLOYEE_L.GrossDeduction = Convert.ToDecimal(totalDeductionsRow["Total_Cu_Tr_Amt"]);
                                }
                                //totalGrossDeduction += EMPLOYEE_L.GrossDeduction;
                            }


                            PdfPTable table = new PdfPTable(4)
                            {
                                WidthPercentage = 100
                            };
                            table.SetWidths(new float[] { 70, 70, 70, 70 });
                            PdfPCell CreateCenterAlignedCell(string text, Font font2)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(text, font2));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                return cell;
                            }
                            table.AddCell(new PdfPCell(new Phrase("Emp No", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table.AddCell(new PdfPCell(new Phrase("Emp Name", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table.AddCell(new PdfPCell(new Phrase("Designation", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table.AddCell(new PdfPCell(new Phrase("Date of Joining", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table.AddCell(CreateCenterAlignedCell(employeeBO.EmpNo.ToString(), boldFont));
                            table.AddCell(CreateCenterAlignedCell(employeeBO.EmpName, boldFont));
                            table.AddCell(CreateCenterAlignedCell(employeeBO.Designation, boldFont));
                            table.AddCell(CreateCenterAlignedCell(employeeBO.DOj.ToString("yyyy-MM-dd"), boldFont));
                            document.Add(table);


                            PdfPTable table1 = new PdfPTable(8)
                            {
                                WidthPercentage = 100
                            };
                            table1.SetWidths(new float[] { 50, 50, 50, 50, 50, 50, 50, 50 });
                            PdfPCell CreateRightAlignedCell(string text, Font font1)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(text, font1));
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                return cell;
                            }
                            //Heading
                            // table1.AddCell(new PdfPCell(new Phrase("-----", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("ORIGINAL BASIC", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("CURRENT BASIC", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("DA", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("HRA", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("CON.ALLOWANCE", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("AD.ALLOWANCE", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("OTHER EARNING", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table1.AddCell(new PdfPCell(new Phrase("GROSS EARNING", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.OriginalBasic.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.CurrentBasic.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.DA.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.HRA.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.CovAllownance.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.AdAllownance.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_A.otherEarning.ToString("0.00"), boldFont));
                            table1.AddCell(CreateRightAlignedCell(EMPLOYEE_G.GrossEarning.ToString("0.00"), boldFont));





                            document.Add(table1);
                            // document.Add(new Paragraph("\n\n\n", font));

                            PdfPTable table2 = new PdfPTable(8)
                            {
                                WidthPercentage = 100
                            };
                            table2.SetWidths(new float[] { 50, 50, 50, 50, 50, 50, 50, 50 });




                            table2.AddCell(new PdfPCell(new Phrase("P.TAX", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("PF", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("I.TAX", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("LIC", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("OTHER DEDUCTION", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("GROSS DEDUCTION", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("NET SALARY", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase("---", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });


                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_D.ProofTax.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_D.PF.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_D.IT.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_D.LIC.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_S.OtherDeduction.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_L.GrossDeduction.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell(EMPLOYEE_N.NetSalary.ToString("0.00"), boldFont));
                            table2.AddCell(CreateRightAlignedCell("---", boldFont));


                            document.Add(table2);


                            document.Add(new Paragraph("************************************************************************************************************", boldFont));

                            totalCurrentBasic += EMPLOYEE_A.CurrentBasic;
                            totalOriginalBasic += EMPLOYEE_A.OriginalBasic;
                            totalDA += EMPLOYEE_A.DA;
                            totalHRA += EMPLOYEE_A.HRA;
                            totalAdAllownanace += EMPLOYEE_A.AdAllownance;
                            totalCovAllownance += EMPLOYEE_A.CovAllownance;
                            totalOtherEarning += EMPLOYEE_O.otherEarning;
                            totalGrossEarning += EMPLOYEE_G.GrossEarning;
                            totalPf += EMPLOYEE_D.PF;
                            totalProfTax += EMPLOYEE_D.ProofTax;
                            totalLIC += EMPLOYEE_D.LIC;
                            totalIT += EMPLOYEE_D.IT;
                            totalGrossDeduction += EMPLOYEE_L.GrossDeduction;
                            totalNetSalary += EMPLOYEE_N.NetSalary;
                            totalOtherDeduction += EMPLOYEE_S.OtherDeduction;
                            document.Add(new Paragraph("\n\n\n", font));
                            employeesAdded++;


                            
                        }
                      

                        document.Add(new Paragraph("\n\n\n", font));
                        document.Add(new Paragraph("\n\n\n", font));
                        PdfPTable table3 = new PdfPTable(1)
                        {
                            WidthPercentage = 100
                        };
                        table3.SetWidths(new float[] { 50 });
                        table3.AddCell(new PdfPCell(new Phrase("TOTAL", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        document.Add(table3);
                        
                        PdfPTable table4 = new PdfPTable(8)
                        {
                            WidthPercentage = 100
                        };
                        table4.SetWidths(new float[] { 50, 50, 50, 50, 50, 50, 50, 50 });
                        PdfPCell CreateRightAlignedCell1(string text, Font font1)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(text, font1));
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            return cell;
                        }
                        //Heading
                        // table1.AddCell(new PdfPCell(new Phrase("-----", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL ORIGINAL BASIC", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL CURRENT BASIC", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL DA", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL HRA", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL CON.ALLOWANCE", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL AD.ALLOWANCE", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL OTHER EARNING", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table4.AddCell(new PdfPCell(new Phrase("TOTAL GROSS EARNING", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                        table4.AddCell(CreateRightAlignedCell1(totalOriginalBasic.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalOriginalBasic.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalDA.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalHRA.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalCovAllownance.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalAdAllownanace.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalOtherEarning.ToString("0.00"), boldFont));
                        table4.AddCell(CreateRightAlignedCell1(totalGrossEarning.ToString("0.00"), boldFont));

                       



                        document.Add(table4);

                        PdfPTable table5 = new PdfPTable(8)
                        {
                            WidthPercentage = 100
                        };
                        table5.SetWidths(new float[] { 50, 50, 50, 50, 50, 50, 50, 50 });


                        PdfPCell CreateRightAlignedCell2(string text, Font font1)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(text, font1));
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            return cell;
                        }

                        table5.AddCell(new PdfPCell(new Phrase("TOTAL P.TAX", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("TOTAL PF", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("TOTAL I.TAX", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("TOTAL LIC", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("TOTAL OTHER DEDUCTION", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("TOTAL GROSS DEDUCTION", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("TOTAL NET SALARY", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table5.AddCell(new PdfPCell(new Phrase("---", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });


                        table5.AddCell(CreateRightAlignedCell2(totalProfTax.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2(totalPf.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2(totalIT.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2(totalLIC.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2(totalOtherDeduction.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2(totalGrossDeduction.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2(totalNetSalary.ToString("0.00"), boldFont));
                        table5.AddCell(CreateRightAlignedCell2("---", boldFont));


                        document.Add(table5);

                        document.Close();

                        byte[] fileBytes = ms.ToArray();
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.ContentType = "application/pdf";
                        HttpContext.Current.Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
                        HttpContext.Current.Response.BinaryWrite(fileBytes);
                        HttpContext.Current.Response.End();
                        Response.Write("<script>alert('IT WorkSheet Generated Successfully!');</script>");
                    }
                }
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
                
                ddlBranch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
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
            Response.Redirect("~/Home.aspx");
           
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dropdown();
        }

        protected void btnPrint_Click_txt(object sender, EventArgs e)
        {
            employeeBO.OtherDeduction = employeeBO.CuTrAmt;

            int pageNumber = (int)Session["PageNumber"];


            string takenBy = Session["TakenBy"].ToString();
            DateTime currentDate = (DateTime)Session["CurrentDate"];
            Session["Description"] = txtDescription.Text;
            Count();
            string description = Session["Description"].ToString();
            string selectedBranchNames = ddlBranch.SelectedItem.ToString();
            employeeBO.BranchCode = Convert.ToInt32(ddlBranch.SelectedItem.Value);
            string currentTime = DateTime.Now.ToString("dd-MM-yyyy");
            DAL.BranchDetails branchDetails = hrmsSalarySlipDA.GetBranchDetails();

            if (employeeBO.BranchCode == 0)
            {
                decimal totalDA = 0;
                decimal totalHRA = 0;
                decimal totalCovAllownance = 0;
                decimal totalAdAllownanace = 0;
                decimal totalCurrentBasic = 0;
                decimal totalOriginalBasic = 0;
                decimal totalPf = 0;
                decimal totalProfTax = 0;
                decimal totalIT = 0;
                decimal totalLIC = 0;
                decimal totalOtherEarning = 0;
                decimal totalGrossEarning = 0;
                decimal totalGrossDeduction = 0;
                decimal totalOtherDeduction = 0;
                decimal totalNetSalary = 0;


                StringBuilder formattedOutput = new StringBuilder();
                employeeBO.Action = "A";
                DataTable salaryRegisterData = hrmsSalarySlipDA.SalaryRegister(employeeBO);
                formattedOutput.AppendLine($"{branchDetails.BrName}                                                                                                       Page No :{pageNumber}");
                formattedOutput.AppendLine($"Salary Register for the month of :{DateTime.Now.ToString("MMMM - yyyy")}                                                                                               Taken By : {takenBy}");
                formattedOutput.AppendLine($"\t\t\t\t\t\t\t\t\t\t                                                          Date :{DateTime.Now.ToString("dd.MM.yyyy")}");
                formattedOutput.AppendLine($"Dept:{selectedBranchNames}");
                formattedOutput.AppendLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                formattedOutput.AppendLine("Emp No   Employee Name / Designation        Date of Joining           Original Basic      Current Basic          DA           HRA           Conv.           Adhoc       Other Earnings      Gross Earnings       Prof. Tax       Provident Fund       Income Tax       L I C        Other Deductions       Gross Deductions             Net Salary");
                formattedOutput.AppendLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");

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
                        DataTable earningsData = hrmsSalarySlipDA.SalaryRegister(earningsEmployeeBO);

                        if (earningsData != null && earningsData.Rows.Count > 0)
                        {

                            foreach (DataRow earningRow in earningsData.Rows)
                            {
                                earningsEmployeeBO.ED_Desc = earningRow["ED_Desc"].ToString();
                                earningsEmployeeBO.CuTrAmt = Convert.ToDecimal(earningRow["Cu_Tr_Amt"]);
                                earningsEmployeeBO.Cu_Tr_Code = Convert.ToInt32(earningRow["Cu_Tr_Code"]);
                                earningsEmployeeBO.OriginalBasic = Convert.ToDecimal(earningRow["OriBasic"]);
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
                                    case "OriBasic":
                                        earningsEmployeeBO.OriginalBasic = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    case "OTHER EARNINGS":
                                        earningsEmployeeBO.otherEarning = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    default:
                                        break;
                                }

                                totalCurrentBasic += earningsEmployeeBO.CurrentBasic;
                                totalOriginalBasic += earningsEmployeeBO.OriginalBasic;
                                totalDA += earningsEmployeeBO.DA;
                                totalHRA += earningsEmployeeBO.HRA;
                                totalAdAllownanace += earningsEmployeeBO.AdAllownance;
                                totalCovAllownance += earningsEmployeeBO.CovAllownance;

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
                        DataTable deductionData = hrmsSalarySlipDA.SalaryRegister(deductionEmployeeBO);

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
                                    case "INCOME TAXF":
                                        deductionEmployeeBO.IT = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "P.TAX":
                                        deductionEmployeeBO.ProofTax = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "INCOME TAX":
                                        deductionEmployeeBO.IT = deductionEmployeeBO.CuTrAmt;
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

                                totalPf += deductionEmployeeBO.PF;
                                totalProfTax += deductionEmployeeBO.ProofTax;
                                totalLIC += deductionEmployeeBO.LIC;
                                totalIT += deductionEmployeeBO.IT;

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
                        DataTable netSalaryData = hrmsSalarySlipDA.SalaryRegister(netSalaryEmployeeBO);

                        if (netSalaryData != null && netSalaryData.Rows.Count > 0)
                        {

                            foreach (DataRow netSalaryRow in netSalaryData.Rows)
                            {

                                netSalaryEmployeeBO.NetSalary = Convert.ToDecimal(netSalaryRow["NetSalary"]);
                            }
                            totalNetSalary += netSalaryEmployeeBO.NetSalary;
                        }
                        else
                        {
                            formattedOutput.AppendLine("No net salary data found.");
                        }


                        EmployeeBO totalEarningsEmployeeBO = new EmployeeBO();
                        totalEarningsEmployeeBO.Action = "G";
                        totalEarningsEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable totalEarningsData = hrmsSalarySlipDA.SalaryRegister(totalEarningsEmployeeBO);

                        if (totalEarningsData != null && totalEarningsData.Rows.Count > 0)
                        {

                            foreach (DataRow totalEarningsRow in totalEarningsData.Rows)
                            {
                                totalEarningsEmployeeBO.GrossEarning = Convert.ToDecimal(totalEarningsRow["EarningsAmount"]);
                            }
                            totalGrossEarning += totalEarningsEmployeeBO.GrossEarning;
                        }
                        else
                        {
                            formattedOutput.AppendLine("No total earnings data found.");
                        }


                        // Fetch Other Earnings Amount (Action "O")
                        EmployeeBO OtherEarningemployeeBO = new EmployeeBO();
                        OtherEarningemployeeBO.Action = "O";
                        OtherEarningemployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable OtherEarningData = hrmsSalarySlipDA.SalaryRegister(OtherEarningemployeeBO);
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
                                totalOtherEarning += OtherEarningemployeeBO.otherEarning;
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
                        DataTable OtherDeductionData = hrmsSalarySlipDA.SalaryRegister(OtherdeductionemployeeBO);
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
                                totalOtherDeduction += OtherdeductionemployeeBO.OtherDeduction;
                            }
                        }
                        else
                        {
                            formattedOutput.AppendLine("No Other Deductions Amount data found.");
                        }


                        EmployeeBO totalDeductionsEmployeeBO = new EmployeeBO();
                        totalDeductionsEmployeeBO.Action = "L";
                        totalDeductionsEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable totalDeductionsData = hrmsSalarySlipDA.SalaryRegister(totalDeductionsEmployeeBO);

                        if (totalDeductionsData != null && totalDeductionsData.Rows.Count > 0)
                        {

                            foreach (DataRow totalDeductionsRow in totalDeductionsData.Rows)
                            {
                                totalDeductionsEmployeeBO.GrossDeduction = Convert.ToDecimal(totalDeductionsRow["Total_Cu_Tr_Amt"]);
                            }
                            totalGrossDeduction += totalDeductionsEmployeeBO.GrossDeduction;
                        }
                        else
                        {
                            formattedOutput.AppendLine("No total deductions data found.");
                        }


                        // string firstLine = $"{employeeBO.EmpNo,-8} {employeeBO.EmpName,-40} {employeeBO.DOj.ToString("dd-MM-yyyy"),-20}  {earningsEmployeeBO.CurrentBasic}          { earningsEmployeeBO.CurrentBasic}              {earningsEmployeeBO.DA}      {earningsEmployeeBO.HRA}        {earningsEmployeeBO.DA}          {earningsEmployeeBO.AdAllownance}        { OtherEarningemployeeBO.otherEarning,-5}                                 {totalEarningsEmployeeBO.GrossEarning,-10}              {deductionEmployeeBO.ProofTax}             {deductionEmployeeBO.PF}               {deductionEmployeeBO.IT}                {deductionEmployeeBO.LIC}            { OtherdeductionemployeeBO.OtherDeduction}                          {totalDeductionsEmployeeBO.GrossDeduction}               {netSalaryEmployeeBO.NetSalary}"; // Adjust width as needed
                        string firstLine = $"{employeeBO.EmpNo,-8} {employeeBO.EmpName,-40} {employeeBO.DOj.ToString("dd-MM-yyyy"),-20}  { earningsEmployeeBO.OriginalBasic,-20} {earningsEmployeeBO.CurrentBasic,-20} {earningsEmployeeBO.DA,-15} {earningsEmployeeBO.HRA,-15} {earningsEmployeeBO.CovAllownance,-15} {earningsEmployeeBO.AdAllownance,-15} {OtherEarningemployeeBO.otherEarning,-15} {totalEarningsEmployeeBO.GrossEarning,-20} {deductionEmployeeBO.ProofTax,-15} {deductionEmployeeBO.PF,-15} { deductionEmployeeBO.IT,-15} {deductionEmployeeBO.LIC,-15} {OtherdeductionemployeeBO.OtherDeduction,-25} {totalDeductionsEmployeeBO.GrossDeduction,-20} {netSalaryEmployeeBO.NetSalary}"; // Adjust width as needed

                        string secondLine = $"{"",8} /{employeeBO.Designation}";

                        formattedOutput.AppendLine(firstLine);
                        formattedOutput.AppendLine(secondLine);






                        formattedOutput.AppendLine();
                    }


                    formattedOutput.AppendLine("============================================================================================================================================================================================================================================================================================================================================================================================================================================================================================");
                    formattedOutput.AppendLine($"Total :                                                                { totalOriginalBasic}            {totalCurrentBasic}          { totalDA }      {totalHRA}        {totalCovAllownance}           {totalAdAllownanace}              { totalOtherEarning }        {totalGrossEarning}           {totalProfTax}         {totalPf}       {totalIT}               {totalLIC}               {totalOtherDeduction}                  {totalGrossDeduction}            {totalNetSalary}");
                    formattedOutput.AppendLine("=========================================================================================================================================================================================================================================================================================================================================================================================================================================================================================== ");
                    formattedOutput.AppendLine($"No of Employees : {salaryRegisterData.Rows.Count,-2}          DA points for this month : ");

                    string folderPath = HttpContext.Current.Server.MapPath("~/App_Data/Report");


                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }


                    string currentDateTime = DateTime.Now.ToString("yyyy-MM");


                    string fileName = $"SalaryRegisterReport_{currentDateTime}.txt";


                    File.WriteAllText(fileName, formattedOutput.ToString());


                    Console.WriteLine($"Report saved to: {fileName}");

                }
                else
                {
                    Console.WriteLine("No data found.");
                }

            }
            else if (employeeBO.BranchCode > 0 && ddlFromEmpNo.SelectedValue != "0" && ddlToEmpNo.SelectedValue != "0" && ddlFromEmpNo.SelectedValue != null && ddlToEmpNo.SelectedValue != null)
            {
                decimal totalDA = 0;
                decimal totalHRA = 0;
                decimal totalCovAllownance = 0;
                decimal totalAdAllownanace = 0;
                decimal totalCurrentBasic = 0;
                decimal totalOriginalBasic = 0;
                decimal totalPf = 0;
                decimal totalProfTax = 0;
                decimal totalIT = 0;
                decimal totalLIC = 0;
                decimal totalOtherEarning = 0;
                decimal totalGrossEarning = 0;
                decimal totalGrossDeduction = 0;
                decimal totalOtherDeduction = 0;
                decimal totalNetSalary = 0;
                employeeBO.BranchCode = Convert.ToInt32(ddlBranch.SelectedItem.Value);
                employeeBO.fromNo = Convert.ToInt32(ddlFromEmpNo.SelectedItem.Value);
                employeeBO.ToNo = Convert.ToInt32(ddlToEmpNo.SelectedItem.Value);


                StringBuilder formattedOutput = new StringBuilder();
                employeeBO.Action = "A";
                DataTable salaryRegisterData = hrmsSalarySlipDA.SalaryRegister(employeeBO);
                formattedOutput.AppendLine($"{branchDetails.BrName}                                                                                                       Page No :{pageNumber}");
                formattedOutput.AppendLine($"Salary Register for the month of :{DateTime.Now.ToString("MMMM - yyyy")}                                                                                               Taken By : {takenBy}");
                formattedOutput.AppendLine($"\t\t\t\t\t\t\t\t\t\t                                                          Date :{DateTime.Now.ToString("dd.MM.yyyy")}");
                formattedOutput.AppendLine($"Dept:{selectedBranchNames}");
                formattedOutput.AppendLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                formattedOutput.AppendLine("Emp No   Employee Name / Designation        Date of Joining           Original Basic      Current Basic          DA           HRA           Conv.           Adhoc       Other Earnings      Gross Earnings       Prof. Tax       Provident Fund       Income Tax       L I C        Other Deductions       Gross Deductions             Net Salary");
                formattedOutput.AppendLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");

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
                        DataTable earningsData = hrmsSalarySlipDA.SalaryRegister(earningsEmployeeBO);

                        decimal employeeDA = 0;
                        decimal employeeCurrentBasic = 0;
                        decimal employeeOriginalBasic = 0;

                        if (earningsData != null && earningsData.Rows.Count > 0)
                        {

                            foreach (DataRow earningRow in earningsData.Rows)
                            {
                                earningsEmployeeBO.ED_Desc = earningRow["ED_Desc"].ToString();
                                earningsEmployeeBO.CuTrAmt = Convert.ToDecimal(earningRow["Cu_Tr_Amt"]);
                                earningsEmployeeBO.Cu_Tr_Code = Convert.ToInt32(earningRow["Cu_Tr_Code"]);
                                earningsEmployeeBO.OriginalBasic = Convert.ToDecimal(earningRow["OriBasic"]);
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
                                    case "OriBasic":
                                        earningsEmployeeBO.OriginalBasic = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    case "OTHER EARNINGS":
                                        earningsEmployeeBO.otherEarning = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    default:
                                        break;
                                }

                                //totalCurrentBasic += earningsEmployeeBO.CurrentBasic;
                                //totalOriginalBasic += earningsEmployeeBO.OriginalBasic;
                                //totalDA += earningsEmployeeBO.DA;
                                //totalHRA += earningsEmployeeBO.HRA;
                                //totalAdAllownanace += earningsEmployeeBO.AdAllownance;
                                //totalCovAllownance += earningsEmployeeBO.CovAllownance;

                            }
                            totalCurrentBasic += earningsEmployeeBO.CurrentBasic;
                            totalOriginalBasic += earningsEmployeeBO.OriginalBasic;
                            totalDA += earningsEmployeeBO.DA;
                            totalHRA += earningsEmployeeBO.HRA;
                            totalAdAllownanace += earningsEmployeeBO.AdAllownance;
                            totalCovAllownance += earningsEmployeeBO.CovAllownance;
                        }
                        else
                        {
                            formattedOutput.AppendLine("No earnings data found.");
                        }



                        // Fetch deduction details (Action "D")
                        EmployeeBO deductionEmployeeBO = new EmployeeBO();
                        deductionEmployeeBO.Action = "D"; // Set action to fetch deduction data
                        deductionEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable deductionData = hrmsSalarySlipDA.SalaryRegister(deductionEmployeeBO);

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
                                    case "INCOME TAXF":
                                        deductionEmployeeBO.IT = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "P.TAX":
                                        deductionEmployeeBO.ProofTax = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "INCOME TAX":
                                        deductionEmployeeBO.IT = deductionEmployeeBO.CuTrAmt;
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

                                totalPf += deductionEmployeeBO.PF;
                                totalProfTax += deductionEmployeeBO.ProofTax;
                                totalLIC += deductionEmployeeBO.LIC;
                                totalIT += deductionEmployeeBO.IT;

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
                        DataTable netSalaryData = hrmsSalarySlipDA.SalaryRegister(netSalaryEmployeeBO);

                        if (netSalaryData != null && netSalaryData.Rows.Count > 0)
                        {

                            foreach (DataRow netSalaryRow in netSalaryData.Rows)
                            {

                                netSalaryEmployeeBO.NetSalary = Convert.ToDecimal(netSalaryRow["NetSalary"]);
                            }
                            totalNetSalary += netSalaryEmployeeBO.NetSalary;
                        }
                        else
                        {
                            formattedOutput.AppendLine("No net salary data found.");
                        }


                        EmployeeBO totalEarningsEmployeeBO = new EmployeeBO();
                        totalEarningsEmployeeBO.Action = "G";
                        totalEarningsEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable totalEarningsData = hrmsSalarySlipDA.SalaryRegister(totalEarningsEmployeeBO);

                        if (totalEarningsData != null && totalEarningsData.Rows.Count > 0)
                        {

                            foreach (DataRow totalEarningsRow in totalEarningsData.Rows)
                            {
                                totalEarningsEmployeeBO.GrossEarning = Convert.ToDecimal(totalEarningsRow["EarningsAmount"]);
                            }
                            totalGrossEarning += totalEarningsEmployeeBO.GrossEarning;
                        }
                        else
                        {
                            formattedOutput.AppendLine("No total earnings data found.");
                        }


                        // Fetch Other Earnings Amount (Action "O")
                        EmployeeBO OtherEarningemployeeBO = new EmployeeBO();
                        OtherEarningemployeeBO.Action = "O";
                        OtherEarningemployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable OtherEarningData = hrmsSalarySlipDA.SalaryRegister(OtherEarningemployeeBO);
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
                                totalOtherEarning += OtherEarningemployeeBO.otherEarning;
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
                        DataTable OtherDeductionData = hrmsSalarySlipDA.SalaryRegister(OtherdeductionemployeeBO);
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
                                totalOtherDeduction += OtherdeductionemployeeBO.OtherDeduction;
                            }
                        }
                        else
                        {
                            formattedOutput.AppendLine("No Other Deductions Amount data found.");
                        }


                        EmployeeBO totalDeductionsEmployeeBO = new EmployeeBO();
                        totalDeductionsEmployeeBO.Action = "L";
                        totalDeductionsEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable totalDeductionsData = hrmsSalarySlipDA.SalaryRegister(totalDeductionsEmployeeBO);

                        if (totalDeductionsData != null && totalDeductionsData.Rows.Count > 0)
                        {

                            foreach (DataRow totalDeductionsRow in totalDeductionsData.Rows)
                            {
                                totalDeductionsEmployeeBO.GrossDeduction = Convert.ToDecimal(totalDeductionsRow["DeductionsAmount"]);
                            }
                            totalGrossDeduction += totalDeductionsEmployeeBO.GrossDeduction;
                        }
                        else
                        {
                            formattedOutput.AppendLine("No total deductions data found.");
                        }


                        // string firstLine = $"{employeeBO.EmpNo,-8} {employeeBO.EmpName,-40} {employeeBO.DOj.ToString("dd-MM-yyyy"),-20}  {earningsEmployeeBO.CurrentBasic}          { earningsEmployeeBO.CurrentBasic}              {earningsEmployeeBO.DA}      {earningsEmployeeBO.HRA}        {earningsEmployeeBO.DA}          {earningsEmployeeBO.AdAllownance}        { OtherEarningemployeeBO.otherEarning,-5}                                 {totalEarningsEmployeeBO.GrossEarning,-10}              {deductionEmployeeBO.ProofTax}             {deductionEmployeeBO.PF}               {deductionEmployeeBO.IT}                {deductionEmployeeBO.LIC}            { OtherdeductionemployeeBO.OtherDeduction}                          {totalDeductionsEmployeeBO.GrossDeduction}               {netSalaryEmployeeBO.NetSalary}"; // Adjust width as needed
                        string firstLine = $"{employeeBO.EmpNo,-8} {employeeBO.EmpName,-40} {employeeBO.DOj.ToString("dd-MM-yyyy"),-20}  { earningsEmployeeBO.OriginalBasic,-20} {earningsEmployeeBO.CurrentBasic,-20} {earningsEmployeeBO.DA,-15} {earningsEmployeeBO.HRA,-15} {earningsEmployeeBO.CovAllownance,-15} {earningsEmployeeBO.AdAllownance,-15} {OtherEarningemployeeBO.otherEarning,-15} {totalEarningsEmployeeBO.GrossEarning,-20} {deductionEmployeeBO.ProofTax,-15} {deductionEmployeeBO.PF,-15} { deductionEmployeeBO.IT,-15} {deductionEmployeeBO.LIC,-15} {OtherdeductionemployeeBO.OtherDeduction,-25} {totalDeductionsEmployeeBO.GrossDeduction,-20} {netSalaryEmployeeBO.NetSalary}"; // Adjust width as needed

                        string secondLine = $"{"",8} /{employeeBO.Designation}";

                        formattedOutput.AppendLine(firstLine);
                        formattedOutput.AppendLine(secondLine);






                        formattedOutput.AppendLine();
                    }


                    formattedOutput.AppendLine("============================================================================================================================================================================================================================================================================================================================================================================================================================================================================================");
                    formattedOutput.AppendLine($"Total :                                                                { totalOriginalBasic}            {totalCurrentBasic}          { totalDA }      {totalHRA}        {totalCovAllownance}           {totalAdAllownanace}              { totalOtherEarning }        {totalGrossEarning}           {totalProfTax}         {totalPf}       {totalIT}               {totalLIC}               {totalOtherDeduction}                  {totalGrossDeduction}            {totalNetSalary}");
                    formattedOutput.AppendLine("=========================================================================================================================================================================================================================================================================================================================================================================================================================================================================================== ");
                    formattedOutput.AppendLine($"No of Employees : {salaryRegisterData.Rows.Count,-2}          DA points for this month : ");

                    string folderPath = HttpContext.Current.Server.MapPath("~/App_Data/Report");


                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }


                    string currentDateTime = DateTime.Now.ToString("yyyy-MM");


                    string fileName = $"SalaryRegisterReport_{currentDateTime}.txt";


                    File.WriteAllText(fileName, formattedOutput.ToString());


                    Console.WriteLine($"Report saved to: {fileName}");
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Text Report generated successfully for selected Branch');", true);
                }
                else
                {
                    Console.WriteLine("No data found.");
                }
            }

            else if (employeeBO.BranchCode > 0 && ddlFromEmpNo.SelectedValue == "0" && ddlToEmpNo.SelectedValue == "0")
            {
                decimal totalDA = 0;
                decimal totalHRA = 0;
                decimal totalCovAllownance = 0;
                decimal totalAdAllownanace = 0;
                decimal totalCurrentBasic = 0;
                decimal totalOriginalBasic = 0;
                decimal totalPf = 0;
                decimal totalProfTax = 0;
                decimal totalIT = 0;
                decimal totalLIC = 0;
                decimal totalOtherEarning = 0;
                decimal totalGrossEarning = 0;
                decimal totalGrossDeduction = 0;
                decimal totalOtherDeduction = 0;
                decimal totalNetSalary = 0;
                employeeBO.fromNo = 0;
                int cnt = (int)Session["Count"];
                employeeBO.ToNo = cnt;

                StringBuilder formattedOutput = new StringBuilder();
                employeeBO.Action = "A";
                DataTable salaryRegisterData = hrmsSalarySlipDA.SalaryRegister(employeeBO);
                formattedOutput.AppendLine($"{branchDetails.BrName}                                                                                                       Page No :{pageNumber}");
                formattedOutput.AppendLine($"Salary Register for the month of :{DateTime.Now.ToString("MMMM - yyyy")}                                                                                               Taken By : {takenBy}");
                formattedOutput.AppendLine($"\t\t\t\t\t\t\t\t\t\t                                                          Date :{DateTime.Now.ToString("dd.MM.yyyy")}");
                formattedOutput.AppendLine($"Dept:{selectedBranchNames}");
                formattedOutput.AppendLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                formattedOutput.AppendLine("Emp No   Employee Name / Designation        Date of Joining           Original Basic      Current Basic          DA           HRA           Conv.           Adhoc       Other Earnings      Gross Earnings       Prof. Tax       Provident Fund       Income Tax       L I C        Other Deductions       Gross Deductions             Net Salary");
                formattedOutput.AppendLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");

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
                        DataTable earningsData = hrmsSalarySlipDA.SalaryRegister(earningsEmployeeBO);

                        if (earningsData != null && earningsData.Rows.Count > 0)
                        {

                            foreach (DataRow earningRow in earningsData.Rows)
                            {
                                earningsEmployeeBO.ED_Desc = earningRow["ED_Desc"].ToString();
                                earningsEmployeeBO.CuTrAmt = Convert.ToDecimal(earningRow["Cu_Tr_Amt"]);
                                earningsEmployeeBO.Cu_Tr_Code = Convert.ToInt32(earningRow["Cu_Tr_Code"]);
                                earningsEmployeeBO.OriginalBasic = Convert.ToDecimal(earningRow["OriBasic"]);

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
                                    //case "OriBasic":
                                    //    earningsEmployeeBO.OriginalBasic = earningsEmployeeBO.CuTrAmt;
                                    //    break;
                                    case "OTHER EARNINGS":
                                        earningsEmployeeBO.otherEarning = earningsEmployeeBO.CuTrAmt;
                                        break;
                                    default:
                                        break;
                                }



                            }
                            totalCurrentBasic += earningsEmployeeBO.CurrentBasic;
                            totalOriginalBasic += earningsEmployeeBO.OriginalBasic;
                            totalDA += earningsEmployeeBO.DA;
                            totalHRA += earningsEmployeeBO.HRA;
                            totalAdAllownanace += earningsEmployeeBO.AdAllownance;
                            totalCovAllownance += earningsEmployeeBO.CovAllownance;
                        }
                        else
                        {
                            formattedOutput.AppendLine("No earnings data found.");
                        }

                        // Fetch deduction details (Action "D")
                        EmployeeBO deductionEmployeeBO = new EmployeeBO();
                        deductionEmployeeBO.Action = "D"; // Set action to fetch deduction data
                        deductionEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable deductionData = hrmsSalarySlipDA.SalaryRegister(deductionEmployeeBO);

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
                                    case "INCOME TAXF":
                                        deductionEmployeeBO.IT = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "P.TAX":
                                        deductionEmployeeBO.ProofTax = deductionEmployeeBO.CuTrAmt;
                                        break;
                                    case "INCOME TAX":
                                        deductionEmployeeBO.IT = deductionEmployeeBO.CuTrAmt;
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
                            totalPf += deductionEmployeeBO.PF;
                            totalProfTax += deductionEmployeeBO.ProofTax;
                            totalLIC += deductionEmployeeBO.LIC;
                            totalIT += deductionEmployeeBO.IT;

                        }
                        else
                        {
                            formattedOutput.AppendLine("No deduction data found.");
                        }

                        // Fetch Net Salary (Action "N")
                        EmployeeBO netSalaryEmployeeBO = new EmployeeBO();
                        netSalaryEmployeeBO.Action = "N"; // Set action to fetch net salary data
                        netSalaryEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable netSalaryData = hrmsSalarySlipDA.SalaryRegister(netSalaryEmployeeBO);

                        if (netSalaryData != null && netSalaryData.Rows.Count > 0)
                        {

                            foreach (DataRow netSalaryRow in netSalaryData.Rows)
                            {

                                netSalaryEmployeeBO.NetSalary = Convert.ToDecimal(netSalaryRow["NetSalary"]);
                            }
                            totalNetSalary += netSalaryEmployeeBO.NetSalary;
                        }
                        else
                        {
                            formattedOutput.AppendLine("No net salary data found.");
                        }


                        EmployeeBO totalEarningsEmployeeBO = new EmployeeBO();
                        totalEarningsEmployeeBO.Action = "G";
                        totalEarningsEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable totalEarningsData = hrmsSalarySlipDA.SalaryRegister(totalEarningsEmployeeBO);

                        if (totalEarningsData != null && totalEarningsData.Rows.Count > 0)
                        {

                            foreach (DataRow totalEarningsRow in totalEarningsData.Rows)
                            {
                                totalEarningsEmployeeBO.GrossEarning = Convert.ToDecimal(totalEarningsRow["EarningsAmount"]);
                            }
                            totalGrossEarning += totalEarningsEmployeeBO.GrossEarning;
                        }
                        else
                        {
                            formattedOutput.AppendLine("No total earnings data found.");
                        }


                        // Fetch Other Earnings Amount (Action "O")
                        EmployeeBO OtherEarningemployeeBO = new EmployeeBO();
                        OtherEarningemployeeBO.Action = "O";
                        OtherEarningemployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable OtherEarningData = hrmsSalarySlipDA.SalaryRegister(OtherEarningemployeeBO);
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
                                totalOtherEarning += OtherEarningemployeeBO.otherEarning;
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
                        DataTable OtherDeductionData = hrmsSalarySlipDA.SalaryRegister(OtherdeductionemployeeBO);
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
                                totalOtherDeduction += OtherdeductionemployeeBO.OtherDeduction;
                            }
                        }
                        else
                        {
                            formattedOutput.AppendLine("No Other Deductions Amount data found.");
                        }


                        EmployeeBO totalDeductionsEmployeeBO = new EmployeeBO();
                        totalDeductionsEmployeeBO.Action = "L";
                        totalDeductionsEmployeeBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                        DataTable totalDeductionsData = hrmsSalarySlipDA.SalaryRegister(totalDeductionsEmployeeBO);

                        if (totalDeductionsData != null && totalDeductionsData.Rows.Count > 0)
                        {

                            foreach (DataRow totalDeductionsRow in totalDeductionsData.Rows)
                            {
                                totalDeductionsEmployeeBO.GrossDeduction = Convert.ToDecimal(totalDeductionsRow["Total_Cu_Tr_Amt"]);
                            }
                            totalGrossDeduction += totalDeductionsEmployeeBO.GrossDeduction;
                        }
                        else
                        {
                            formattedOutput.AppendLine("No total deductions data found.");
                        }



                        // string firstLine = $"{employeeBO.EmpNo,-8} {employeeBO.EmpName,-40} {employeeBO.DOj.ToString("dd-MM-yyyy"),-20}  {earningsEmployeeBO.CurrentBasic}          { earningsEmployeeBO.CurrentBasic}              {earningsEmployeeBO.DA}      {earningsEmployeeBO.HRA}        {earningsEmployeeBO.DA}          {earningsEmployeeBO.AdAllownance}        { OtherEarningemployeeBO.otherEarning,-5}                                 {totalEarningsEmployeeBO.GrossEarning,-10}              {deductionEmployeeBO.ProofTax}             {deductionEmployeeBO.PF}               {deductionEmployeeBO.IT}                {deductionEmployeeBO.LIC}            { OtherdeductionemployeeBO.OtherDeduction}                          {totalDeductionsEmployeeBO.GrossDeduction}               {netSalaryEmployeeBO.NetSalary}"; // Adjust width as needed
                        string firstLine = $"{employeeBO.EmpNo,-8} {employeeBO.EmpName,-40} {employeeBO.DOj.ToString("dd-MM-yyyy"),-20}  {earningsEmployeeBO.OriginalBasic,-20} {earningsEmployeeBO.CurrentBasic,-20} {earningsEmployeeBO.DA,-15} {earningsEmployeeBO.HRA,-15} {earningsEmployeeBO.CovAllownance,-15} {earningsEmployeeBO.AdAllownance,-15} {OtherEarningemployeeBO.otherEarning,-15} {totalEarningsEmployeeBO.GrossEarning,-20} {deductionEmployeeBO.ProofTax,-15} {deductionEmployeeBO.PF,-15} { deductionEmployeeBO.IT,-15} {deductionEmployeeBO.LIC,-15} {OtherdeductionemployeeBO.OtherDeduction,-25} {totalDeductionsEmployeeBO.GrossDeduction,-20} {netSalaryEmployeeBO.NetSalary}"; // Adjust width as needed

                        string secondLine = $"{"",8} /{employeeBO.Designation}";

                        formattedOutput.AppendLine(firstLine);
                        formattedOutput.AppendLine(secondLine);






                        formattedOutput.AppendLine();
                    }




                    formattedOutput.AppendLine("============================================================================================================================================================================================================================================================================================================================================================================================================================================================================================");
                    formattedOutput.AppendLine($"Total :                                                                { totalOriginalBasic}            {totalCurrentBasic}           {totalDA }      {totalHRA}        {totalCovAllownance}           {totalAdAllownanace}              { totalOtherEarning }        {totalGrossEarning}           {totalProfTax}         {totalPf}       {totalIT}               {totalLIC}               {totalOtherDeduction}                  {totalGrossDeduction}            {totalNetSalary}");
                    formattedOutput.AppendLine("=========================================================================================================================================================================================================================================================================================================================================================================================================================================================================================== ");
                    formattedOutput.AppendLine($"No of Employees : {salaryRegisterData.Rows.Count,-2}          DA points for this month : ");

                    string folderPath = HttpContext.Current.Server.MapPath("~/App_Data/Report");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string currentDateTime = DateTime.Now.ToString("yyyy-MM");
                    string fileName = $"SalaryRegisterReport_{currentDateTime}.txt";
                    string filePath = Path.Combine(folderPath, fileName);

                    File.WriteAllText(filePath, formattedOutput.ToString());

                    Console.WriteLine($"Report saved to: {filePath}");

                    Response.Write("Succesfully generated");
                }
                else
                {
                    Console.WriteLine("No data found.");
                }
            }


        }
    }
}