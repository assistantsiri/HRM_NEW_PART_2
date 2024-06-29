using DA;
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
    public partial class LEAVE_PROCEEDING_REPORT : System.Web.UI.Page
    {
        LeaveEntryDA leaveEntryDA = new LeaveEntryDA();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                BindEmployeeDropdown();
            }

        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {


            string currentDateTime = DateTime.Now.ToString("yyyy-MM");
            string fileName = $"Leave report{currentDateTime}.pdf";
            string rootDirectoryPath = HttpContext.Current.Server.MapPath("~/App_Data/Report/SalarySlip");
            string filePath = Path.Combine(rootDirectoryPath, fileName);
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
                document.Open();
                Font font = FontFactory.GetFont(FontFactory.COURIER, BaseFont.CP1257, BaseFont.NOT_EMBEDDED, 15);
                Font boldFont = FontFactory.GetFont(FontFactory.COURIER_BOLD, BaseFont.CP1257, BaseFont.NOT_EMBEDDED, 15);


                document.Add(new Paragraph("\n", font));

                //Heading1
                string monthYear = DateTime.Now.ToString("MMMM - yyyy");
                Chunk chunk1 = new Chunk($"CANBANK COMPUTER SERVICES LIMITED #218,I FLOOR, J.P. RPYAL SAMPIGE ROAD , MALLESHWARAM BANGALORE. 56003          {monthYear}", boldFont);
                Paragraph paragraph1 = new Paragraph(chunk1);
                document.Add(paragraph1);
                document.Add(new Paragraph("\n", font));
                //Heading2
                Chunk chunk2 = new Chunk($"LEAVE PROCEEDING          {monthYear}", boldFont);
                Paragraph paragraph2 = new Paragraph(chunk2);
                document.Add(paragraph2);

                document.Add(new Paragraph("\n", font));
                document.Add(new Paragraph("\n", font));
                document.Add(new Paragraph("\n", font));

                document.Add(new Paragraph($"Ref no:  CCSL:LEAVE : 2001 : current Year", font));
                document.Add(new Paragraph($"                                                                                                                Date:", font));
                document.Add(new Paragraph($"To", font));
                document.Add(new Paragraph($"Sri/smt. EmpName(EmpNo)", font));
                document.Add(new Paragraph($"Ref. : Your Leave Application dated :{currentDateTime}", font));
                document.Add(new Paragraph($"With reference to your leave application cited above, you are hereby", font));
                document.Add(new Paragraph($"advised that the following leave has been sanctioned to you", font));
                document.Add(new Paragraph("\n", font));
                document.Add(new Paragraph("\n", font));
                document.Add(new Paragraph($"-----------------------------------------------------------------------------------------------------------------------------------------------------", font));
                document.Add(new Paragraph($"                         LEAVE REQUESTED                                                                LEAVE SANCTIONED                      ", font));
                document.Add(new Paragraph($"--------------------------------------------------------------------------------------------------------------------------------------------------------", font));
                document.Add(new Paragraph($"  FROM         TO            DAYS        CATEGORY OF LEAVE                                   FROM           TO          DAYS        CATEGORY OF LEAVE", font));
                document.Add(new Paragraph($"--------------------------------------------------------------------------------------------------------------------------------------------------------", font));
                document.Add(new Paragraph($"  07-05-2024   09-05-2024     1           Privilege Leave                                      07-05-2024   09-05-2024     1           Privilege Leave   ", font));
                document.Add(new Paragraph($"----------------------------------------------------------------------------------------------------------------------------------------------------------", font));
                document.Add(new Paragraph("\n", font));
                document.Add(new Paragraph($"Details relating to your Leave Record as on date are furnished for information :", font));
                document.Add(new Paragraph("\n", font));
                document.Add(new Paragraph("\n", font));
                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 30, 30, 30, 30 });
                table.AddCell(new PdfPCell(new Phrase("Leave Record", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Data", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Leave Record", font)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Data", font)) { HorizontalAlignment = Element.ALIGN_CENTER });

                table.AddCell(new PdfPCell(new Phrase("PL cum SL Balance ", font)));
                table.AddCell(new PdfPCell(new Phrase("86", font))); //.ToString("0.00")
                table.AddCell(new PdfPCell(new Phrase(" No. of times PL availed during the year", font)));
                table.AddCell(new PdfPCell(new Phrase("1", font)));


                table.AddCell(new PdfPCell(new Phrase("USLHP Balance ", font)));
                table.AddCell(new PdfPCell(new Phrase(" ", font))); //.ToString("0.00")
                table.AddCell(new PdfPCell(new Phrase("No.of days SL availed during the year  ", font)));
                table.AddCell(new PdfPCell(new Phrase("0", font)));


                table.AddCell(new PdfPCell(new Phrase(" No.of days LOP    : ", font)));
                table.AddCell(new PdfPCell(new Phrase(" 0", font))); //.ToString("0.00")
                table.AddCell(new PdfPCell(new Phrase("No.of times LOP availed during the year     :  ", font)));
                table.AddCell(new PdfPCell(new Phrase("0", font)));

                table.AddCell(new PdfPCell(new Phrase(" availed till date    : ", font)));
                table.AddCell(new PdfPCell(new Phrase(" ", font))); //.ToString("0.00")
                table.AddCell(new PdfPCell(new Phrase(" ", font)));
                table.AddCell(new PdfPCell(new Phrase("", font)));


                document.Add(new Paragraph("\n", font));
                document.Add(new Paragraph("\n", font));

                document.Add(new Paragraph($"PL cum SL Next Credit Due Date : 01.01.2025", font)); //NextPLCrDt"

                document.Add(new Paragraph("\n", font));
                document.Add(new Paragraph($"You are permitted to avail LFC for the Block Ending : ", font));
                document.Add(new Paragraph("You are permitted to encash        days of PL for the Block Ending : ", font));
                document.Add(new Paragraph("\n", font));
                document.Add(new Paragraph("Please furnish the leave address while proceeding on leave", font));
                document.Add(new Paragraph("Please furnish Medical / Fitness certificate on joining in case of Sick Leave.", font));
                document.Add(new Paragraph("\n", font));

                document.Add(new Paragraph("For    CanbankComputer Services Limited", font)); //{Cname}

                document.Add(new Paragraph("\n", font));
                document.Add(new Paragraph("\n", font));
                document.Add(new Paragraph("Manager Accounts & Admin", font));
















            }

            catch
            {

            }

        }
        protected void ddlEmpNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void BindEmployeeDropdown()
        {
            DataTable dtEmployees = leaveEntryDA.GetEmployee();
            if (dtEmployees.Rows.Count > 0)
            {
                ddlEmpNo.DataSource = dtEmployees;
                //ddlEmployee.DataTextField = "Emp_Name";
                ddlEmpNo.DataValueField = "Emp_No";
                ddlEmpNo.DataBind();
            }
            ddlEmpNo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Employee--", "0"));
        }


        protected void branchDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedEmpNo = Convert.ToInt32(ddlEmpNo.SelectedValue);
            // txtempNo.Text = selectedEmpNo.ToString();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Home.aspx");
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {

        }
    }
}