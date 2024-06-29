using System;
using BO;
using BL;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HRMS.App_Code;
using System.ComponentModel.DataAnnotations;
using DA;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using Image = iTextSharp.text.Image;

namespace HRMS
{
    public partial class HRMS_LeaveParticular : System.Web.UI.Page
    {
        LeaveEntryBO bO = new LeaveEntryBO();
        LeaveEntryBL bL = new LeaveEntryBL();
        LeaveEntryDA entryDA = new LeaveEntryDA();
        DataTable dt = new DataTable();
        string msg = "";
        DateTime now = DateTime.Now;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    TempMenuRemoval();
                    FillEmp();
                    FillYear();
                    LP_Grd.Visible = false;
                    HRMS_LP_Panel2.Visible = false;

                    Label1.Text = $"LEAVE DETAILS ";
                }
            }
            catch
            {
                throw;
            }
            Session["PageNumber"] = 1;
            Session["TakenBy"] = "ADMIN";
            Session["CurrentDate"] = DateTime.Now;
           
        }
        protected void FillEmp()
        {
            bO.Action = "A";
            bO.SV= Convert.ToInt16(LP_rbl.SelectedValue);
            dt = bL.FetchDetails(bO);
            if(dt.Rows.Count>0)
            {
                //Employee Number DropDown
                LP_Emp.DataValueField = "Emp_No";
                LP_Emp.DataTextField = "Emp_Name";
                LP_Emp.DataSource = dt;
                LP_Emp.DataBind();
                LP_Emp.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----- Select -----", "0"));

            }
            dt.Clear();

           
        }
        protected void FillYear()
        {
            bO.Action = "C";
            dt = bL.FetchDetails(bO);
            if (dt.Rows.Count > 0)
            {
                LP_Year.DataValueField = "Year";
                LP_Year.DataSource = dt;
                LP_Year.DataBind();
                LP_Year.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----- Select -----", ""));
                LP_Year.Items.Insert(1, new System.Web.UI.WebControls.ListItem("All", "0"));

            }
            dt.Clear();
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

        protected void SubmitBtn_Click(object sender, EventArgs e)
        {
            if (LP_Emp.SelectedIndex==0 || LP_Year.SelectedIndex==0)
            {
                msg = "Select Both Year and Employee for List";
                Globals.Show(msg);
            }
            else
            {
                fillGrd();
                

            }


        }
        protected void fillGrd()
        {
            bO.Emp_No = Convert.ToInt32(LP_Emp.SelectedValue);
            bO.Year = Convert.ToInt32(LP_Year.SelectedValue);
            LP_Grd.Visible = true;
            
            bO.Action = "B";

            dt = bL.FetchDetails(bO);
            if (dt.Rows.Count > 0)
            {
                LP_Grd.DataSource = dt;
                LP_Grd.DataBind();
            }
            else
            {
                string msg = "No Values Present!";
                Globals.Show(msg);
                LP_Grd.Visible = false;
            }
            dt.Clear();
        }

        protected void LP_Grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LP_Grd.PageIndex = e.NewPageIndex;
            fillGrd();
        }
        protected void LP_Grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName=="View")
            {
                HRMS_LP_Panel1.Visible = false;
                HRMS_LP_Panel2.Visible = true;
                int i =(int)LP_Grd.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0];
                View();
                ViewPanel(i);
            }
            else if(e.CommandName=="Delete")
            {
                bO.Sl_No = (int)LP_Grd.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0];
                bO.Action = "Y";
                msg= bL.AUDDetails(bO);
                Globals.Show(msg);
            }
        }
        protected void Clear()
        {
            Panel_EmpName.Text = "";
            Panel_EmpNo.Text = "";
            
            Panel_NoD.Text = "";
            Panel_Reason.Text = "";
            Panel_NoDS.Text = "";
            Panel_AO.Text = "";
            Panel_FrmDt.Text = "";
            Panel_ToDt.Text = "";
            Panel_Sanc.SelectedIndex = -1;
            Panel_LT.SelectedIndex = -1;
            Panel_LvCode.Items.Clear();
            
        }
        
        protected void Enable()
        {
            Panel_EmpName.ReadOnly = true;
            Panel_EmpNo.ReadOnly = true;
            Panel_FrmDt.ReadOnly = false;
            Panel_ToDt.ReadOnly = false;
            Panel_AO.ReadOnly = true;
            Panel_NoD.ReadOnly = false;
            Panel_NoDS.ReadOnly = false;
            Panel_Reason.ReadOnly = false;
            Panel_LvCode.Enabled = true;
            
            Panel_Sanc.Enabled = true;
            LP_BtnSave.Visible = true;
            Panel_LT.Enabled = true;
            DropDown();
        }
        protected void DropDown()
        {
            DataTable cTable = new DataTable();
            bO.Action = "L";
            cTable = bL.FetchDetails(bO);
            try
            {
                if (cTable.Rows.Count > 0)
                {
                    Panel_LvCode.DataSource = cTable;
                    Panel_LvCode.DataTextField = "Leave_Type";
                    Panel_LvCode.DataValueField = "Leave_Code";
                    Panel_LvCode.DataBind();
                    Panel_LvCode.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----- Select -----", "Leave Type"));


                }
            }
            catch
            {
                throw;
            }
            finally
            {
                cTable.Dispose();
            }
            
        }
        protected void View()
        {
            Panel_EmpName.ReadOnly = true;
            Panel_EmpNo.ReadOnly = true;
            Panel_FrmDt.ReadOnly = true;
            Panel_ToDt.ReadOnly = true;
            Panel_AO.ReadOnly = true;
            Panel_NoD.ReadOnly = true;
            Panel_NoDS.ReadOnly = true;
            Panel_Reason.ReadOnly = true;
            Panel_LvCode.Enabled = false;
           
            Panel_Sanc.Enabled = false;
            Panel_LT.Enabled = false;

            LP_BtnSave.Visible = false;
        }
        protected void ViewPanel(int i)
        {
            bO.Action = "F";
            bO.Sl_No = i;
            dt = bL.FetchDetails(bO);
            try
            {
                if (dt.Rows.Count > 0)
                {
                    //Textbox
                    DropDown();
                    Panel_EmpName.Text = dt.Rows[0]["Emp_Name"].ToString();
                    Panel_EmpNo.Text = dt.Rows[0]["LvApp_EmpNo"].ToString();
                    
                    Panel_NoD.Text = dt.Rows[0]["LvApp_Days"].ToString();
                    Panel_Reason.Text = dt.Rows[0]["LvApp_Reason"].ToString();
                    Panel_NoDS.Text = dt.Rows[0]["LvApp_LeaveSanc"].ToString();

                    //Date
                    Panel_AO.Text = Convert.ToDateTime(dt.Rows[0]["LvApp_AppDate"]).ToString("yyyy-MM-dd");
                    Panel_FrmDt.Text = Convert.ToDateTime(dt.Rows[0]["LvApp_FromDt"]).ToString("yyyy-MM-dd");
                    Panel_ToDt.Text = Convert.ToDateTime(dt.Rows[0]["LvApp_ToDt"]).ToString("yyyy-MM-dd");

                    //Radio Buttons
                    Panel_Sanc.SelectedValue = dt.Rows[0]["LvApp_Sanctioned"].ToString();
                    Panel_LT.SelectedValue = dt.Rows[0]["LvApp_PLSL"].ToString();

                    //DropDownList

                    Panel_LvCode.SelectedValue= dt.Rows[0]["Leave_Code"].ToString();




                }
            }
            catch
            {
                throw;
            }
            finally
            {
                bO = null;
                bL = null;
                dt.Dispose();
            }
        }

       
        protected void LP_Grd_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            fillGrd();
        }

        protected void LP_BtnSave_Click(object sender, EventArgs e)
        {
            EA_Grd();
            if (Convert.ToString(Session["Status"]) == "Edit")
            {
                bO.Sl_No = Convert.ToInt32(Session["Sl_No"]);
                bO.Action = "X";
                msg = bL.AUDDetails(bO);
                Globals.Show(msg);
                 

            }
            else if (Convert.ToString(Session["Status"]) == "Add")
            {
                bO.Action = "Z";
                msg = bL.AUDDetails(bO);
                Globals.Show(msg);
               
            }
            HRMS_LP_Panel1.Visible = true;
            HRMS_LP_Panel2.Visible = false;
        }
        protected void EA_Grd()
        {
            bO.Emp_No = Convert.ToInt32(LP_Emp.SelectedValue);
            bO.Code= Convert.ToInt32(Panel_LvCode.SelectedValue);
            bO.AppDt = Convert.ToDateTime(Panel_AO.Text);
            bO.Days= Convert.ToInt32(Panel_NoD.Text);
            bO.Lea_Sanc= Convert.ToInt32(Panel_NoDS.Text);
            bO.Lea_Ty = Panel_LT.SelectedValue.ToString();
            bO.Sanction = Panel_Sanc.SelectedValue.ToString();
            bO.Reason = Panel_Reason.Text;
            bO.ToDt = Convert.ToDateTime(Panel_ToDt.Text);
            bO.FrmDt = Convert.ToDateTime(Panel_FrmDt.Text);
            
            
        }
        protected void LP_BtnCan_Click(object sender, EventArgs e)
        {
            
            HRMS_LP_Panel1.Visible = true;
            HRMS_LP_Panel2.Visible = false;
            Clear();
        }

        protected void LP_BackBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void LP_AddBtn_Click(object sender, EventArgs e)
        {
            if (LP_Emp.SelectedIndex == 0)
            {
                msg = "Please Select an Employee";
                Globals.Show(msg);
            }
            else
            {
                HRMS_LP_Panel2.Visible = true;
                HRMS_LP_Panel1.Visible = false;
                
                Enable();
                Panel_EmpName.Text = LP_Emp.SelectedItem.Text;
                Panel_EmpNo.Text = LP_Emp.SelectedValue;
                Panel_AO.Text = now.ToString("yyyy-MM-dd");
                Session["Status"] = "Add";
            }
        }

        protected void LP_Grd_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Enable();
            int i = (int)LP_Grd.DataKeys[Convert.ToInt32(e.RowIndex)].Values[0];
            Session["Sl_No"] = i;
            ViewPanel(i);
            HRMS_LP_Panel1.Visible = false;
            HRMS_LP_Panel2.Visible = true;
            Session["Status"] = "Edit";
        }

        protected void LP_rbl_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillEmp();
        }

        protected void LP_GenerateRepo_Click(object sender, EventArgs e)
        {
            try
            {
                string branchName = entryDA.GetBranchName();
                int pageNumber = (int)Session["PageNumber"];
                string currentTime = DateTime.Now.ToString("dd-MM-yyyy");
                int selectedEmpNo = int.Parse(LP_Emp.SelectedValue);
                string selectedEmpName = LP_Emp.SelectedItem.Text;

                // Check if an employee is selected
                if (selectedEmpNo == 0)
                {
                    Response.Write("Please select an employee.");
                    return;
                }
                LeaveEntryBO leaveEntry = new LeaveEntryBO
                {
                    Code = Convert.ToInt32(Panel_LvCode.SelectedValue)


                };

                LeaveEntryBO leaveEntryA = new LeaveEntryBO
                {
                    Action = "A"
                };
                DataTable resultA = entryDA.AUDDetail(leaveEntryA);
                if (resultA != null && resultA.Rows.Count > 0)
                {
                    foreach (DataRow row in resultA.Rows)
                    {
                        leaveEntryA.BR_NAME = row["BR_NAME"].ToString();
                        leaveEntryA.BR_ADD1 = row["BR_ADD1"].ToString();
                        leaveEntryA.BR_ADD2 = row["BR_ADD2"].ToString();
                        leaveEntryA.BR_ADD3 = row["BR_ADD3"].ToString();
                        leaveEntryA.BR_CITY = row["BR_CITY"].ToString();
                        leaveEntryA.BR_PIN = Convert.ToInt32(row["BR_PIN"]);

                    }

                }
                LeaveEntryBO leaveEntryB = new LeaveEntryBO
                {
                    Action = "B"
                };
                DataTable resultB = entryDA.AUDDetail(leaveEntryB);
                if (resultB != null && resultB.Rows.Count > 0)
                {
                    foreach (DataRow row in resultB.Rows)
                    {
                        leaveEntryB.LPARAM_REF = row["LPARAM_REF"].ToString();
                        leaveEntryB.REFNO = Convert.ToInt32(row["REFNO"]);
                    }
                }

                LeaveEntryBO leaveEntryC = new LeaveEntryBO
                {
                    Action = "C"
                };
                DataTable resultC = entryDA.AUDDetail(leaveEntryC);
                if (resultC != null && resultC.Rows.Count > 0)
                {
                    foreach (DataRow row in resultC.Rows)
                    {
                        leaveEntryC.STRYR = Convert.ToInt32(row["StYr"]);
                    }
                }

                LeaveEntryBO leaveEntryD = new LeaveEntryBO
                {
                    Action = "D"
                };
                DataTable resultD = entryDA.AUDDetail(leaveEntryD);
                if (resultD != null && resultD.Rows.Count > 0)
                {
                    foreach (DataRow row in resultD.Rows)
                    {
                        leaveEntryC.PLBALANCE = Convert.ToInt32(row["PLBalance"]);
                    }
                }
                LeaveEntryBO leaveEntryZ = new LeaveEntryBO
                {
                    Action = "Z"
                };
                DataTable resultZ = entryDA.AUDDetail(leaveEntryZ);
                if (resultZ != null && resultZ.Rows.Count > 0)
                {
                    foreach (DataRow row in resultZ.Rows)
                    {
                        leaveEntryZ.Sl_No = Convert.ToInt32(row["LvApp_SlNo"]);
                        leaveEntryZ.Emp_No = Convert.ToInt32(row["LvApp_EmpNo"]);
                        leaveEntryZ.Code = Convert.ToInt32(row["LvApp_Code"]);
                        leaveEntryZ.AppDt = Convert.ToDateTime(row["LvApp_AppDate"]);
                        leaveEntryZ.FrmDt = Convert.ToDateTime(row["LvApp_FromDt"]);
                        leaveEntryZ.ToDt = Convert.ToDateTime(row["LvApp_ToDt"]);
                        leaveEntryZ.Days = Convert.ToInt32(row["LvApp_Days"]);
                        leaveEntryZ.Lea_Sanc = Convert.ToInt32(row["LvApp_Sanctioned"]);
                        leaveEntryZ.LvApp_LeaveSanc = Convert.ToInt32(row["LvApp_LeaveSanc"]);
                        leaveEntryZ.LvApp_Reason = row["LvApp_Reason"].ToString();
                        leaveEntryZ.LvApp_PLSL = row["LvApp_PLSL"].ToString();
                        leaveEntryZ.LvApp_Eby = Convert.ToInt32(row["LvApp_Eby"]);                   
                        leaveEntryZ.LvApp_Crdate = Convert.ToDateTime(row["LvApp_Crdate"]);
                       


                    }
                }





















                Document document = new Document();
                string filePath = HttpContext.Current.Server.MapPath("~/App_Data/Report");
                string reportFileName = Path.Combine(filePath, $"LeaveRepo_{currentTime}.pdf");
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(reportFileName, FileMode.Create));
                string logoPath = @"D:\HRMS-\logo.png"; //  path  logo image
                Image logoImage = Image.GetInstance(logoPath);
                logoImage.ScaleAbsolute(90, 50); // Set the size of the logo image

             
                PdfHeaderFooter header = new PdfHeaderFooter();
                header.Logo = logoImage;
                writer.PageEvent = header;

               
                document.Open();
                Font font = FontFactory.GetFont(FontFactory.COURIER, BaseFont.CP1257, BaseFont.NOT_EMBEDDED, 8);
                Font boldFont = FontFactory.GetFont(FontFactory.COURIER_BOLD, BaseFont.CP1257, BaseFont.NOT_EMBEDDED, 8);

                string headingText = $"";
                Paragraph headingParagraph = new Paragraph
                {
                    Alignment = Element.ALIGN_CENTER
                };
                Chunk chunk = new Chunk(headingText, boldFont);
                headingParagraph.Add(chunk);


                document.Add(new Paragraph("\n", font));
                document.Add(new Paragraph("\n", font)); 
                document.Add(new Paragraph("\n", font)); 
                document.Add(new Paragraph("\n", font));
                document.Add(new Paragraph("\n", font));
                document.Add(new Paragraph($"Leave for the month of " + DateTime.Now.ToString("MMMM - yyyy") + " ", font));
                document.Add(new Paragraph($"{branchName}                                                Page No :{pageNumber} ", font));
                document.Add(new Paragraph($"                                                                                 Date :{DateTime.Now.ToString("dd.MM.yyyy")} ", font));
                document.Add(new Paragraph("\n", font));
                document.Add(new Paragraph("\n", font));
                // PdfPTable table = new PdfPTable(7);
                // table.WidthPercentage = 100;
                // table.SetWidths(new float[] { 10, 25, 10, 10, 10, 10, 10 });
               
                document.Add(new Paragraph($"Employee No: {selectedEmpNo}", font));
                document.Add(new Paragraph($"Employee No: {selectedEmpName}", font));
                document.Add(new Paragraph($"Leave Type: {leaveEntry.Code}", font));











                if (document != null)
                {
                    document.Close();
                }
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", $"attachment; filename={Path.GetFileName(reportFileName)}");
                Response.TransmitFile(reportFileName);
                Response.Flush();
                Response.End();


                Response.Write("PF Report generated successfully.");


            }
            catch (Exception ex)
            {
                Response.Write($"Error: {ex.Message}");
            }

           
        }
    }
}