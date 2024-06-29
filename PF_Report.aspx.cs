using DAL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Image = iTextSharp.text.Image;

namespace HRMS
{
    public partial class PF_Report : System.Web.UI.Page
    {
        PF_ReportDA pF_ReportDA = new PF_ReportDA();
        protected void Page_Load(object sender, EventArgs e)
        {
            List<string> branchNames = pF_ReportDA.BindBranches();
            foreach (string branchName in branchNames)
            {
                branchDropdown.Items.Add(new System.Web.UI.WebControls.ListItem(branchName));
            }
            Session["PageNumber"] = 1;
            Session["TakenBy"] = "ADMIN";
            Session["CurrentDate"] = DateTime.Now;
        }
        public void branchDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            string branchName = branchDropdown.SelectedValue;
            string amountType = "";
            // pF_ReportDA.GenerateSalarySlip(branchName, amountType);
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedBranch = branchDropdown.SelectedValue;
                string branchName = pF_ReportDA.GetBranchName();
                int pageNumber = (int)Session["PageNumber"];
                Tuple<decimal, decimal> charges = pF_ReportDA.GetPFandEDLICharges();
                decimal pfAdminCharges = charges.Item1;
                decimal edliCharges = charges.Item2;
                decimal totalWages = 0;
                decimal totalEmployeePF = 0;
                decimal totalEmployerPF = 0;
                decimal totalPension = 0;
                string currentTime = DateTime.Now.ToString("dd-MM-yyyy");
                DataTable pfReportData = pF_ReportDA.GetEmpPFrepoByBranch(selectedBranch);
                if (pfReportData != null && pfReportData.Rows.Count > 0)
                {
                    Document document = new Document();
                    string filePath = HttpContext.Current.Server.MapPath("~/App_Data/Report");
                    string reportFileName = Path.Combine(filePath, $"PfReport_{currentTime}.pdf");

                    try
                    {
                        // Initialize PDF writer
                        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(reportFileName, FileMode.Create));

                        // Load the logo image
                        string logoPath = @"D:\HRMS-\logo.png"; // Provide the path to your logo image
                        Image logoImage = Image.GetInstance(logoPath);
                        logoImage.ScaleAbsolute(90, 50); // Set the size of the logo image

                        // Add header with logo
                        PdfHeaderFooter header = new PdfHeaderFooter();
                        header.Logo = logoImage; // Set the logo image to the header
                        writer.PageEvent = header;

                        // Open the document
                        document.Open();
                        Font font = FontFactory.GetFont(FontFactory.COURIER, BaseFont.CP1252, BaseFont.NOT_EMBEDDED, 8);

                        // Add branch details
                        document.Add(new Paragraph("\n", font));
                        document.Add(new Paragraph("\n", font)); // Add empty line with specified font
                        document.Add(new Paragraph("\n", font)); // Add empty line with specified font
                        document.Add(new Paragraph("\n", font));
                        document.Add(new Paragraph("\n", font));
                        document.Add(new Paragraph($"Salary slip for the month of " + DateTime.Now.ToString("MMMM - yyyy") + " ", font));
                        document.Add(new Paragraph($"{branchName}                                                Page No :{pageNumber} ", font));
                        document.Add(new Paragraph($"{selectedBranch}                                                        Date :{DateTime.Now.ToString("dd.MM.yyyy")} ", font));
                        document.Add(new Paragraph("\n", font));
                        document.Add(new Paragraph("\n", font));
                        PdfPTable table = new PdfPTable(7);
                        table.WidthPercentage = 100;
                        table.SetWidths(new float[] { 10, 25, 10, 10, 10, 10, 10 });

                        // Add header rows to the table

                        table.AddCell(new PdfPCell(new Phrase("Emp NO", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase("Employee Name", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase("Joining Date", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase("Wages", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase("Employee PF", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase("Employer PF", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase("Pension", font)) { HorizontalAlignment = Element.ALIGN_CENTER });


                        // Add employee details
                        foreach (DataRow row in pfReportData.Rows)
                        {

                            table.AddCell(new PdfPCell(new Phrase(row["Emp_No"].ToString(), font)));
                            table.AddCell(new PdfPCell(new Phrase(row["Emp_Name"].ToString(), font)));
                            table.AddCell(new PdfPCell(new Phrase(row["Joining_Date"].ToString(), font)));
                            table.AddCell(new PdfPCell(new Phrase(row["Wages"].ToString(), font)));
                            table.AddCell(new PdfPCell(new Phrase(row["Employee"].ToString(), font)));
                            table.AddCell(new PdfPCell(new Phrase(row["Employer"].ToString(), font)));
                            table.AddCell(new PdfPCell(new Phrase(row["Pension"].ToString(), font)));


                            decimal wages = Convert.ToDecimal(row["Wages"]);
                            string employeeName = row["Emp_Name"].ToString();
                            decimal employer = Convert.ToDecimal(row["Employer"]);
                            decimal pension = Convert.ToDecimal(row["Pension"]);
                            decimal employeePF = Convert.ToDecimal(row["Employee"]);

                            //TotalCalculation 
                            totalWages += wages;
                            totalEmployeePF += employeePF;
                            totalEmployerPF += employer;
                            totalPension += pension;


                        }
                        table.AddCell(new PdfPCell(new Phrase("TOTAL", font)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase(totalWages.ToString("#,##0"), font)));
                        table.AddCell(new PdfPCell(new Phrase(totalEmployeePF.ToString("#,##0"), font)));
                        table.AddCell(new PdfPCell(new Phrase(totalEmployerPF.ToString("#,##0.00"), font)));
                        table.AddCell(new PdfPCell(new Phrase(totalPension.ToString("#,##0.00"), font)));

                        document.Add(table);
                        document.Add(new Paragraph("\n", font));
                        document.Add(new Paragraph("\n", font));

                        PdfPTable chargesTable = new PdfPTable(2);
                        chargesTable.WidthPercentage = 100;
                        chargesTable.SetWidths(new float[] { 70, 30 });

                        // Add rows to the second table
                        decimal pfAdminCharge = totalWages * (pfAdminCharges / 100);
                        pfAdminCharge = Math.Round(pfAdminCharge, 0);
                        decimal EDLIChr = totalWages * (edliCharges / 100);
                        EDLIChr = Math.Round(EDLIChr, 0);

                        chargesTable.AddCell(new PdfPCell(new Phrase("PF Administrative Charges (A/c No.2):", font)));
                        chargesTable.AddCell(new PdfPCell(new Phrase(pfAdminCharge.ToString("#,##0"), font)));

                        chargesTable.AddCell(new PdfPCell(new Phrase("EDLI Administrative Charges (A/c No.22):", font)));
                        chargesTable.AddCell(new PdfPCell(new Phrase(EDLIChr.ToString("#,##0"), font)));

                        chargesTable.AddCell(new PdfPCell(new Phrase("PF A/c Employer + Employee (A/c No.1) :", font)));
                        chargesTable.AddCell(new PdfPCell(new Phrase((totalEmployerPF + totalEmployeePF).ToString("#,##0.00"), font)));

                        chargesTable.AddCell(new PdfPCell(new Phrase("Pension A/c (A/c No.10) :", font)));
                        chargesTable.AddCell(new PdfPCell(new Phrase(totalPension.ToString("#,##0.00"), font)));

                        chargesTable.AddCell(new PdfPCell(new Phrase("TOTAL:", font)));
                        chargesTable.AddCell(new PdfPCell(new Phrase((pfAdminCharges + edliCharges + totalEmployerPF + totalEmployeePF + totalPension).ToString("#,##0.00"), font)));

                        // Add the second table to the document
                        document.Add(chargesTable);

                       
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        throw new Exception("Access to the path is denied.", ex);
                    }
                    finally
                    {
                        // Close the document
                        if (document != null)
                        {
                            document.Close();
                        }
                    }

                    // Serve the PDF document
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", $"attachment; filename={Path.GetFileName(reportFileName)}");
                    Response.TransmitFile(reportFileName);
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    Response.Write("No data available for the selected branch.");
                }
            }
            catch (Exception ex)
            {
                Response.Write($"Error: {ex.Message}");
            }

            Response.Write("PF Report generated successfully.");
        }

        string Tab(int count)
        {
            return new string('\t', count);
        }

        // Define a function for trimming and converting to uppercase
        string TrimAndUppercase(string input)
        {
            return input.Trim().ToUpper();
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
   public class TotalAmount
    {
        public decimal totalWages { get; set; }

    }
}