using BO;
using DAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Image = iTextSharp.text.Image;

namespace HRMS
{
    public partial class IT_Calculation : System.Web.UI.Page
    {
        IT_CalculationDA iT_CalculationDA = new IT_CalculationDA();
        IT_WORKSHEETBO iT_WORKSHEETBO = new IT_WORKSHEETBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["PageNumber"] = 1;
                Session["TakenBy"] = "ADMIN";
                Session["CurrentDate"] = DateTime.Now;

                FillGrid();
            }
        }
        private void FillGrid()
        {

            try
            {
                DataTable dt = new DataTable();
                dt = iT_CalculationDA.GetEmployeeTaxDetails();
                if (dt.Rows.Count > 0)
                {
                    gvEmployeeTaxDetails.DataSource = dt;
                    gvEmployeeTaxDetails.DataBind();

                }

            }
            catch
            {
                throw;
            }
            finally
            {

            }
        }
        protected void gvEmployeeTaxDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                // Example: You can set the checkbox state based on some condition
                // chkSelect.Checked = ... ;
            }
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvEmployeeTaxDetails.PageIndex = e.NewPageIndex;
            this.FillGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();

            // Check if the search text is empty
            if (!string.IsNullOrEmpty(searchText))
            {
                DataTable dt = new DataTable(); // Assuming dt is populated properly

                // Check if Emp_No column exists in the DataTable
                if (dt.Columns.Contains("Emp_No"))
                {
                    // Filter the DataTable based on Emp_No
                    DataTable filteredTable = dt.AsEnumerable()
                        .Where(row => row.Field<string>("Emp_No").Contains(searchText))
                        .CopyToDataTable();

                    // Bind the filtered DataTable to the GridView
                    gvEmployeeTaxDetails.DataSource = filteredTable;
                    gvEmployeeTaxDetails.DataBind();
                }
                else
                {
                    // Handle the case where Emp_No column doesn't exist
                    // You can show an error message or take appropriate action
                }
            }
            else
            {

            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {

        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            
        }



       
        protected void CalculateIncomeTax_Click(object sender, EventArgs e)
        {
            DateTime salDate;
            string dateFormat = "yyyy-MM-dd"; // Expected date format

            // Attempt to parse the date from the TextBox
            if (DateTime.TryParseExact(txtPayableDate.Text, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out salDate))
            {
                // Iterate through GridView rows
                foreach (GridViewRow row in gvEmployeeTaxDetails.Rows)
                {
                    CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;

                    if (chkSelect != null && chkSelect.Checked)
                    {
                        int empNo = Convert.ToInt32(row.Cells[1].Text);

                        // Call the method with the employee number and the parsed date
                        iT_CalculationDA.ExecuteITCalculation(empNo, salDate);
                    }
                }
            }
            else
            {
                // Display an alert if the date format is invalid
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid date format. Please enter the date in yyyy-MM-dd format.');", true);
            }
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            DateTime salDate;
            string dateFormat = "yyyy-MM-dd"; // Expected date format

            // Attempt to parse the date from the TextBox
            if (DateTime.TryParseExact(txtPayableDate.Text, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out salDate))
            {
                bool isCalculationSuccessful = false;

                // Iterate through GridView rows
                foreach (GridViewRow row in gvEmployeeTaxDetails.Rows)
                {
                    CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;

                    if (chkSelect != null && chkSelect.Checked)
                    {
                        int empNo = Convert.ToInt32(row.Cells[1].Text);

                        // Call the method with the employee number and the parsed date
                        iT_CalculationDA.ExecuteITCalculation(empNo, salDate);
                        isCalculationSuccessful = true;
                    }
                }

                // Show a success message if at least one calculation was performed
                if (isCalculationSuccessful)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Income tax calculation successful for selected employees.');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('No employees selected for tax calculation.');", true);
                }
            }
            else
            {
                // Display an alert if the date format is invalid
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid date format. Please enter the date in yyyy-MM-dd format.');", true);
            }
        }

        protected void btn_click(object sender, EventArgs e)
        {
            try
            {
                // Retrieve session variables
                int pageNumber = (int)Session["PageNumber"];
                string takenBy = Session["TakenBy"].ToString();
                DateTime currentDate = (DateTime)Session["CurrentDate"];

                // Initialize document and file path
                string currentDateTime = DateTime.Now.ToString("yyyy-MM");
                string fileName = $"Emp_ITWorkSheet{currentDateTime}.pdf";
                Document document = new Document();

                iT_WORKSHEETBO.Action = "Y";
                DataTable dt = iT_CalculationDA.ITWorkSheetData(iT_WORKSHEETBO);

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

                        foreach (DataRow row in dt.Rows)
                        {
                            iT_WORKSHEETBO.EmpNo = Convert.ToInt32(row["Emp_No"]);
                            iT_WORKSHEETBO.EmpName = Convert.ToString(row["Emp_Name"]);

                            IT_WORKSHEETBO WORKSHEETBO_A = new IT_WORKSHEETBO
                            {
                                Action = "A",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };

                            DataTable datatableA = iT_CalculationDA.ITWorkSheetData(WORKSHEETBO_A);

                            if (datatableA != null && datatableA.Rows.Count > 0)
                            {
                                foreach (DataRow dataRowA in datatableA.Rows)
                                {
                                    WORKSHEETBO_A.ED_Desc = dataRowA["ITE_Desc"].ToString();
                                    WORKSHEETBO_A.Code = Convert.ToInt32(dataRowA["Calc_IT_Code"]);
                                    WORKSHEETBO_A.CuTrAmt = Convert.ToDecimal(dataRowA["Calc_IT_Amount"]);

                                    switch (WORKSHEETBO_A.ED_Desc)
                                    {
                                        case "BASIC PAY":
                                            WORKSHEETBO_A.BASIC_PAY = WORKSHEETBO_A.CuTrAmt;
                                            break;
                                        case "DA":
                                            WORKSHEETBO_A.DA = WORKSHEETBO_A.CuTrAmt;
                                            break;
                                        case "HRA":
                                            WORKSHEETBO_A.HRA = WORKSHEETBO_A.CuTrAmt;
                                            break;
                                        case "TECHNICAL ALLOWANCE":
                                            WORKSHEETBO_A.TECHNICALALLOWANCE = WORKSHEETBO_A.CuTrAmt;
                                            break;
                                        case "LFC":
                                            WORKSHEETBO_A.LFC = WORKSHEETBO_A.CuTrAmt;
                                            break;
                                        case "PL ENCASHMENT":
                                            WORKSHEETBO_A.PLENCASHMENT = WORKSHEETBO_A.CuTrAmt;
                                            break;
                                        case "CONVEYANCE":
                                            WORKSHEETBO_A.CONVEYANCE = WORKSHEETBO_A.CuTrAmt;
                                            break;
                                    }
                                }
                            }

                           





                            IT_WORKSHEETBO WORKSHEETBO_B = new IT_WORKSHEETBO
                            {
                                Action = "B",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };

                            DataTable datatableB = iT_CalculationDA.ITWorkSheetData(WORKSHEETBO_B);
                            if (datatableB != null && datatableB.Rows.Count > 0)
                            {
                                foreach (DataRow dataRowB in datatableB.Rows)
                                {
                                    WORKSHEETBO_B.TOTAL_SALARY = Convert.ToDecimal(dataRowB["TotalAmount"]);
                                }
                            }

                            IT_WORKSHEETBO WORKSHEETBO_CD = new IT_WORKSHEETBO
                            {
                                Action = "C",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };

                            DataTable datatableCD = iT_CalculationDA.ITWorkSheetData(WORKSHEETBO_CD);

                            if (datatableCD != null && datatableCD.Rows.Count > 0)
                            {
                                foreach (DataRow dataRowA in datatableCD.Rows)
                                {
                                    WORKSHEETBO_CD.ED_Desc = dataRowA["ITE_Desc"].ToString();
                                    //WORKSHEETBO_CD.Code = Convert.ToInt32(dataRowA["Calc_IT_Code"]);
                                    WORKSHEETBO_CD.CuTrAmt = Convert.ToDecimal(dataRowA["Calc_IT_Amount"]);

                                    switch (WORKSHEETBO_CD.ED_Desc)
                                    {
                                        case "CONVEYANCE DEDUCTION":
                                            WORKSHEETBO_CD.CONVEYANCEDEDUCTION = WORKSHEETBO_CD.CuTrAmt;
                                            break;
                                        case "HRA EXEMPTION":
                                            WORKSHEETBO_CD.HRAEXEMPTION = WORKSHEETBO_CD.CuTrAmt;
                                            break;


                                    }
                                }
                            }


                            IT_WORKSHEETBO WORKSHEETBO_C = new IT_WORKSHEETBO
                            {
                                Action = "E",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };

                            DataTable datatableE = iT_CalculationDA.ITWorkSheetData(WORKSHEETBO_C);
                            if (datatableE != null && datatableE.Rows.Count > 0)
                            {
                                foreach (DataRow dataRowE in datatableE.Rows)
                                {
                                    WORKSHEETBO_C.GROSS_SALARY = Convert.ToDecimal(dataRowE["GROSS_SALARY"]);
                                }
                            }

                            IT_WORKSHEETBO WORKSHEETBO_F = new IT_WORKSHEETBO
                            {
                                Action = "F",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };

                            DataTable datatableF = iT_CalculationDA.ITWorkSheetData(WORKSHEETBO_F);
                            if (datatableF != null && datatableF.Rows.Count > 0)
                            {
                                foreach (DataRow dataRowF in datatableF.Rows)
                                {
                                    WORKSHEETBO_F.ED_Desc = dataRowF["ITE_Desc"].ToString();
                                    WORKSHEETBO_F.CuTrAmt = Convert.ToDecimal(dataRowF["Calc_IT_Amount"]);
                                    switch (WORKSHEETBO_F.ED_Desc)
                                    {
                                        case "P.TAX EXEMPTION":
                                            WORKSHEETBO_F.P_TAX_EXEMPTION = WORKSHEETBO_F.CuTrAmt;
                                            break;
                                    }
                                }
                            }

                            IT_WORKSHEETBO WORKSHEETBO_I = new IT_WORKSHEETBO
                            {
                                Action = "I",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };

                            DataTable datatableI = iT_CalculationDA.ITWorkSheetData(WORKSHEETBO_I);
                            if (datatableI != null && datatableI.Rows.Count > 0)
                            {
                                foreach (DataRow dataRowI in datatableI.Rows)
                                {
                                    WORKSHEETBO_I.Gross_Total_Income = Convert.ToDecimal(dataRowI["Gross_Total_Income"]);
                                }
                            }

                            IT_WORKSHEETBO WORKSHEETBO_O = new IT_WORKSHEETBO
                            {
                                Action = "O",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };

                            DataTable datatableO = iT_CalculationDA.ITWorkSheetData(WORKSHEETBO_O);
                            if (datatableO != null && datatableO.Rows.Count > 0)
                            {
                                foreach (DataRow dataRowO in datatableO.Rows)
                                {
                                    WORKSHEETBO_O._80CPROVIDENDFUND = Convert.ToDecimal(dataRowO["Sum"]);
                                }
                            }

                            IT_WORKSHEETBO WORKSHEETBO_P = new IT_WORKSHEETBO
                            {
                                Action = "P",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };

                            DataTable datatableP = iT_CalculationDA.ITWorkSheetData(WORKSHEETBO_P);
                            if (datatableP != null && datatableP.Rows.Count > 0)
                            {
                                foreach (DataRow dataRowP in datatableP.Rows)
                                {
                                    //WORKSHEETBO_P.ED_Desc = dataRowP["ITE_Desc"].ToString();
                                    //WORKSHEETBO_P.CuTrAmt = Convert.ToDecimal(dataRowP["Calc_IT_Amount"]);
                                    //switch (WORKSHEETBO_P.ED_Desc)
                                    //{
                                    //    case "NET TAXABLE INCOME":
                                    //        WORKSHEETBO_P.NET_TAXABLE_INCOME = WORKSHEETBO_P.CuTrAmt;
                                    //        break;
                                    //}
                                    WORKSHEETBO_P.NET_TAXABLE_INCOME = Convert.ToDecimal(dataRowP["NETTAXABLEINCOME"]);
                                }
                            }

                            IT_WORKSHEETBO WORKSHEETBO_Q = new IT_WORKSHEETBO
                            {
                                Action = "Q",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };

                            DataTable datatableQ = iT_CalculationDA.ITWorkSheetData(WORKSHEETBO_Q);
                            if (datatableQ != null && datatableQ.Rows.Count > 0)
                            {
                                foreach (DataRow dataRowQ in datatableQ.Rows)
                                {
                                    //WORKSHEETBO_Q.ED_Desc = dataRowQ["ITE_Desc"].ToString();
                                    //WORKSHEETBO_Q.CuTrAmt = Convert.ToDecimal(dataRowQ["Calc_IT_Amount"]);
                                    //switch (WORKSHEETBO_Q.ED_Desc)
                                    //{
                                    //    case "TAXABLE INCOME U/S 88D":
                                    //        WORKSHEETBO_Q.TAXABLEINCOMEUS80C = WORKSHEETBO_Q.CuTrAmt;
                                    //        break;
                                    //} TAXABLE INCOME U/S 88D
                                    WORKSHEETBO_Q.TAXABLEINCOMEUS80C = Convert.ToDecimal(dataRowQ["TAXABLEINCOMEUS88D"]);

                                }
                            }

                            IT_WORKSHEETBO WORKSHEETBO_R = new IT_WORKSHEETBO
                            {
                                Action = "R",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };

                            DataTable datatableR = iT_CalculationDA.ITWorkSheetData(WORKSHEETBO_R);
                            if (datatableR != null && datatableR.Rows.Count > 0)
                            {
                                foreach (DataRow dataRowR in datatableR.Rows)
                                {
                                    WORKSHEETBO_R.ED_Desc = dataRowR["ITE_Desc"].ToString();
                                    WORKSHEETBO_R.CuTrAmt = Convert.ToDecimal(dataRowR["Calc_IT_Amount"]);
                                    switch (WORKSHEETBO_R.ED_Desc)
                                    {
                                        case "TAX BEFORE REBATE U/S 88D":
                                            WORKSHEETBO_R.TAXAFTERREBATEUS88D = WORKSHEETBO_R.CuTrAmt;
                                            break;
                                    }
                                }
                            }

                            IT_WORKSHEETBO WORKSHEETBO_S = new IT_WORKSHEETBO
                            {
                                Action = "S",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable datatableS = iT_CalculationDA.ITWorkSheetData(WORKSHEETBO_S);
                            if (datatableR != null && datatableR.Rows.Count > 0)
                            {
                                foreach (DataRow dataRowS in datatableS.Rows)
                                {
                                    WORKSHEETBO_S.ED_Desc = dataRowS["ITE_Desc"].ToString();
                                    WORKSHEETBO_S.CuTrAmt = Convert.ToDecimal(dataRowS["Calc_IT_Amount"]);
                                    switch (WORKSHEETBO_S.ED_Desc)
                                    {
                                        case "ADD SURCHARGE":
                                            WORKSHEETBO_S.ADDSURCHARGE = WORKSHEETBO_S.CuTrAmt;
                                            break;
                                    }
                                }
                            }

                            IT_WORKSHEETBO WORKSHEETBO_T = new IT_WORKSHEETBO
                            {
                                Action = "T",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable datatableT = iT_CalculationDA.ITWorkSheetData(WORKSHEETBO_T);
                            if (datatableT != null && datatableT.Rows.Count > 0)
                            {
                                foreach (DataRow dataRowT in datatableT.Rows)
                                {
                                    WORKSHEETBO_T.ED_Desc = dataRowT["ITE_Desc"].ToString();
                                    WORKSHEETBO_T.CuTrAmt = Convert.ToDecimal(dataRowT["Calc_IT_Amount"]);
                                    switch (WORKSHEETBO_T.ED_Desc)
                                    {
                                        case "TOTAL TAX PAYABLE":
                                            WORKSHEETBO_T.GTOTALTAXPAYABLE = WORKSHEETBO_T.CuTrAmt;
                                            break;
                                    }
                                }
                            }

                            IT_WORKSHEETBO WORKSHEETBO_U = new IT_WORKSHEETBO
                            {
                                Action = "U",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable datatableU = iT_CalculationDA.ITWorkSheetData(WORKSHEETBO_U);
                            if (datatableU != null && datatableU.Rows.Count > 0)
                            {
                                foreach (DataRow dataRowU in datatableU.Rows)
                                {
                                    WORKSHEETBO_U.ED_Desc = dataRowU["ITE_Desc"].ToString();
                                    WORKSHEETBO_U.CuTrAmt = Convert.ToDecimal(dataRowU["Calc_IT_Amount"]);
                                    switch (WORKSHEETBO_U.ED_Desc)
                                    {
                                        case "LESS TAX ALREADY PAID":
                                            WORKSHEETBO_U.GLESSTAXALREADYPAID = WORKSHEETBO_U.CuTrAmt;
                                            break;
                                    }
                                }
                            }


                            IT_WORKSHEETBO WORKSHEETBO_V = new IT_WORKSHEETBO
                            {
                                Action = "V",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable datatableV = iT_CalculationDA.ITWorkSheetData(WORKSHEETBO_V);
                            if (datatableU != null && datatableU.Rows.Count > 0)
                            {
                                foreach (DataRow dataRowV in datatableV.Rows)
                                {
                                    WORKSHEETBO_V.ED_Desc = dataRowV["ITE_Desc"].ToString();
                                    WORKSHEETBO_V.CuTrAmt = Convert.ToDecimal(dataRowV["Calc_IT_Amount"]);
                                    switch (WORKSHEETBO_V.ED_Desc)
                                    {
                                        case "NET TAX PAYABLE":
                                            WORKSHEETBO_V.GNETTAXPAYABLE = WORKSHEETBO_V.CuTrAmt;
                                            break;
                                    }
                                }
                            }


                            IT_WORKSHEETBO WORKSHEETBO_W = new IT_WORKSHEETBO
                            {
                                Action = "W",
                                EmpNo = Convert.ToInt32(row["Emp_No"])
                            };
                            DataTable datatableW = iT_CalculationDA.ITWorkSheetData(WORKSHEETBO_W);
                            if (datatableW != null && datatableW.Rows.Count > 0)
                            {
                                foreach (DataRow dataRowW in datatableW.Rows)
                                {
                                    WORKSHEETBO_W.ED_Desc = dataRowW["ITE_Desc"].ToString();
                                    WORKSHEETBO_W.CuTrAmt = Convert.ToDecimal(dataRowW["RoundedAmount"]);
                                    switch (WORKSHEETBO_V.ED_Desc)
                                    {
                                        case "NET TAX PAYABLE":
                                            WORKSHEETBO_W.TaxPayablepermonth = WORKSHEETBO_W.CuTrAmt;
                                            break;
                                    }
                                }
                            }


                            document.NewPage();


                            int currentYear = DateTime.Now.Year;
                            int previousYear = currentYear;
                            int nextYear = currentYear + 1;
                            //string currentMonth = DateTime.Now.ToString("MMMM"); 
                            string currentMonth = "May";
                            string headingText = $"Income Tax Work Sheet ( {previousYear} - {nextYear}) :({currentMonth} - {previousYear})";

                            // Example output: "Income Tax Work Sheet (June - 2023 - 2024)"


                            Paragraph headingParagraph = new Paragraph
                            {
                                Alignment = Element.ALIGN_CENTER
                            };
                            Chunk chunk = new Chunk(headingText, boldFont);
                            headingParagraph.Add(chunk);

                            document.Add(new Paragraph("\n\n\n", font));
                            document.Add(headingParagraph);
                            document.Add(new Paragraph($"EmpNo      : {iT_WORKSHEETBO.EmpNo}", boldFont));
                            document.Add(new Paragraph($"EmpName    : {iT_WORKSHEETBO.EmpName}", boldFont));
                            document.Add(new Paragraph("\n\n", font));

                            PdfPTable table = new PdfPTable(2)
                            {
                                WidthPercentage = 70
                            };
                            table.SetWidths(new float[] { 70, 70 });
                            table.AddCell(new PdfPCell(new Phrase(" BASIC_PAY : DA : HRA ", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table.AddCell(new PdfPCell(new Phrase(" Amount", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                            table.AddCell(new PdfPCell(new Phrase(" BasicPay", boldFont)));
                            table.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_A.BASIC_PAY.ToString("0.00"), boldFont)));
                            table.AddCell(new PdfPCell(new Phrase(" DA", boldFont)));
                            table.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_A.DA.ToString("0.00"), boldFont)));
                            table.AddCell(new PdfPCell(new Phrase(" HRA", boldFont)));
                            table.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_A.HRA.ToString("0.00"), boldFont)));
                            table.AddCell(new PdfPCell(new Phrase("TECHNICALALLOWANCE ", boldFont)));
                            table.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_A.TECHNICALALLOWANCE.ToString("0.00"), boldFont)));
                            table.AddCell(new PdfPCell(new Phrase("LFC ", boldFont)));
                            table.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_A.LFC.ToString("0.00"), boldFont)));
                            table.AddCell(new PdfPCell(new Phrase(" PLENCASHMENT", boldFont)));
                            table.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_A.PLENCASHMENT.ToString("0.00"), boldFont)));
                            table.AddCell(new PdfPCell(new Phrase(" CONVEYANCE", boldFont)));
                            table.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_A.CONVEYANCE.ToString("0.00"), boldFont)));
                           

                            table.AddCell(new PdfPCell(new Phrase(" Total Salary", boldFont)));
                            table.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_B.TOTAL_SALARY.ToString("0.00"), boldFont)));

                            document.Add(table);
                            document.Add(new Paragraph("\n\n\n", font));

                            PdfPTable table2 = new PdfPTable(2)
                            {
                                WidthPercentage = 70
                            };
                            table2.SetWidths(new float[] { 70, 70 });
                            table2.AddCell(new PdfPCell(new Phrase(" Exemption", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table2.AddCell(new PdfPCell(new Phrase(" Amount", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                            table2.AddCell(new PdfPCell(new Phrase(" HRAEXEMPTION", boldFont)));
                            table2.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_CD.HRAEXEMPTION.ToString("0.00"), boldFont)));

                            table2.AddCell(new PdfPCell(new Phrase("CONVEYANCE DEDUCTION", boldFont)));
                            table2.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_CD.CONVEYANCEDEDUCTION.ToString("0.00"), boldFont)));

                            table2.AddCell(new PdfPCell(new Phrase(" GROSS SALARY", boldFont)));
                            table2.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_C.GROSS_SALARY.ToString("0.00"), boldFont)));

                            table2.AddCell(new PdfPCell(new Phrase(" P.TAX EXEMPTION", boldFont)));
                            table2.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_F.P_TAX_EXEMPTION.ToString("0.00"), boldFont)));
                            table2.AddCell(new PdfPCell(new Phrase(" Gross Total Income", boldFont)));
                            table2.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_I.Gross_Total_Income.ToString("0.00"), boldFont)));

                            document.Add(table2);
                            document.Add(new Paragraph("\n\n\n", font));

                            PdfPTable table3 = new PdfPTable(2)
                            {
                                WidthPercentage = 70
                            };
                            table3.SetWidths(new float[] { 70, 70 });
                            table3.AddCell(new PdfPCell(new Phrase(" Less Deductions : ", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table3.AddCell(new PdfPCell(new Phrase(" Amount", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table3.AddCell(new PdfPCell(new Phrase(" 80 C PROVIDEND FUND ", boldFont)));
                            table3.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_O._80CPROVIDENDFUND.ToString("0.00"), boldFont)));

                            table3.AddCell(new PdfPCell(new Phrase(" 80 C Total   ", boldFont)));
                            table3.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_O._80CPROVIDENDFUND.ToString("0.00"), boldFont)));
                            table3.AddCell(new PdfPCell(new Phrase(" Rebate U/S 80 C", boldFont)));
                            table3.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_O._80CPROVIDENDFUND.ToString("0.00"), boldFont)));
                            document.Add(table3);
                            document.Add(new Paragraph("\n\n\n", font));

                            PdfPTable table4 = new PdfPTable(2)
                            {
                                WidthPercentage = 70
                            };

                            table4.SetWidths(new float[] { 70, 70 });
                            table4.AddCell(new PdfPCell(new Phrase(" TAXABLE : ", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table4.AddCell(new PdfPCell(new Phrase(" Amount", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                            table4.AddCell(new PdfPCell(new Phrase("NET TAXABLE INCOME", boldFont)));
                            table4.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_P.NET_TAXABLE_INCOME.ToString("0.00"), boldFont)));
                            table4.AddCell(new PdfPCell(new Phrase("TAXABLE INCOME U/S 80 C  ", boldFont)));
                            table4.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_Q.TAXABLEINCOMEUS80C.ToString("0.00"), boldFont)));
                            table4.AddCell(new PdfPCell(new Phrase(" TAX AFTER REBATE U/S 80 C ", boldFont)));
                            table4.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_R.TAXAFTERREBATEUS88D.ToString("0.00"), boldFont)));
                            document.Add(table4);


                            document.Add(new Paragraph("\n\n\n", font));

                            PdfPTable table5 = new PdfPTable(2)
                            {
                                WidthPercentage = 70
                            };
                            table5.SetWidths(new float[] { 70, 70 });
                            table5.AddCell(new PdfPCell(new Phrase(" ADD SURCHARGE:GTOTAL TAX PAYABLEH :GLESS TAX ALREADY PAIDH :GNET TAX PAYABLEH : ", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            table5.AddCell(new PdfPCell(new Phrase(" Amount", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                            table5.AddCell(new PdfPCell(new Phrase("ADD SURCHARGE", boldFont)));
                            table5.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_S.ADDSURCHARGE.ToString("0.00"), boldFont))); 
                            table5.AddCell(new PdfPCell(new Phrase("TOTAL TAX PAYABLE", boldFont)));
                            table5.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_T.GTOTALTAXPAYABLE.ToString("0.00"), boldFont))); 
                            table5.AddCell(new PdfPCell(new Phrase("LESS TAX ALREADY PAID", boldFont)));
                            table5.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_U.GLESSTAXALREADYPAID.ToString("0.00"), boldFont)));
                            table5.AddCell(new PdfPCell(new Phrase("NET TAX PAYABLE", boldFont)));
                            table5.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_V.GNETTAXPAYABLE.ToString("0.00"), boldFont))); // WORKSHEETBO_W.GNETTAXPAYABLE
                            table5.AddCell(new PdfPCell(new Phrase("Tax Payable per month  (for 11 month)", boldFont)));
                            table5.AddCell(new PdfPCell(new Phrase(WORKSHEETBO_W.TaxPayablepermonth.ToString("0.00"), boldFont))); // WORKSHEETBO_W.GNETTAXPAYABLE




                            document.Add(table5);
                        }

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
                else
                {
                    Response.Write("<script>alert('No data found');</script>");
                }
            }
            catch (Exception ex)
            {
                // Log exception and notify user
                // Consider logging the exception details to a file or logging system
                Response.Write($"<script>alert('Error: {ex.Message}');</script>");
            }
        }





    }
}