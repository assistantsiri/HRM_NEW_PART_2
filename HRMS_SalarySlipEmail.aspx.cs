using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BA;
using HRMS.App_Code;
using BO;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Globalization;

namespace HRMS
{
    public partial class HRMS_SalarySlipEmail : System.Web.UI.Page
    {
        SalarySlipEmailBO bO = new SalarySlipEmailBO();
        SalarySlipEmailBL bL = new SalarySlipEmailBL();
        
        string rootDirectoryPath = HttpContext.Current.Server.MapPath("~/App_Data/Report/SalarySlip");
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    TempMenuRemoval();
                    FillDD();
                    SSem_GenerateBtn.Visible = false;
                    SSEm_Body.Visible = false;
                    SSEm_Body_Label.Visible = false;

                }
            }
            catch
            {
                throw;
            }

        }
        protected void FillDD()
        {
            try
            {
                bO.Action = "B";
                dt = bL.FetchDetails(bO);
                if (dt.Rows.Count>0)
                {
                    SSEm_Branch.DataSource = dt;
                    SSEm_Branch.DataTextField ="CBr_Name";
                    SSEm_Branch.DataValueField = "CBr_Code";
                    SSEm_Branch.DataBind();
                    SSEm_Branch.Items.Insert(0, new ListItem("----- Select -----"));
                    SSEm_Branch.Items.Insert(1, new ListItem("All", "0"));
                }
               
            }

            catch
            {
                throw;
            }
            finally
            {
                dt.Clear();
            }
        }
  
        protected void FillGrid(short option)
        {
           try
            {
                if (Convert.ToInt16(SSEm_Branch.SelectedValue)==0)
                {
                    bO.Action = "C";
                    dt = bL.FetchDetails(bO);
                    
                    if (dt.Rows.Count > 0)
                    {
                        SSEm_Grid.DataSource = dt;
                        SSEm_Grid.DataBind();
                    }
                }
                else
                {
                    bO.Action = "A";
                    bO.Branches = option;
                    dt = bL.FetchDetails(bO);
                    if (dt.Rows.Count > 0)
                    {
                        SSEm_Grid.DataSource = dt;
                        SSEm_Grid.DataBind();
                    }
                    else
                    {
                        Globals.Show("No Entries for this Value.");
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                dt.Clear();
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
        protected void SubmitBtn_Click(object sender, EventArgs e)
        {
            if (SSEm_Branch.SelectedIndex != 0)
            {
                FillGrid(Convert.ToInt16(SSEm_Branch.SelectedValue));
                SSem_GenerateBtn.Visible = true;
                SSEm_Body.Visible = true;
                SSEm_Body_Label.Visible = true;
                FileExists();
            }
            else
            {
                Globals.Show("Select a Branch!");
            }    
        }

      
        protected void SSEm_BackBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void FileExists()
        {
            foreach (GridViewRow row in SSEm_Grid.Rows)
            {

                // string fileName = $"SalarySlip_Emp{Convert.ToString(SSEm_Grid.DataKeys[row.RowIndex].Values[0])}_{Convert.ToString(SSEm_Grid.DataKeys[row.RowIndex].Values[1]).Replace(" ", "_")}{DateTime.Now:yyyy-mm}.txt";
                string fileName = $"SalarySlip_Emp{Convert.ToString(SSEm_Grid.DataKeys[row.RowIndex].Values[0])}_{Convert.ToString(SSEm_Grid.DataKeys[row.RowIndex].Values[1]).Replace(" ", "_")}{DateTime.Now.ToString("yyyy-MM")}.pdf";
                string filePath = Path.Combine(rootDirectoryPath, fileName);

                if (File.Exists(filePath))
                {
                    (row.FindControl("chkStatus") as CheckBox).Checked = true;
                    Label lblCheckboxText = (Label)row.FindControl("lblCheckboxText");
                    lblCheckboxText.Text = "Present";

                }
                else
                {
                    Label lblCheckboxText = (Label)row.FindControl("lblCheckboxText");
                    lblCheckboxText.Text = "Not Present";
                }

            }
        }

        //protected void SSem_GenerateBtn_Click(object sender, EventArgs e)
        //{
        //    string body = SSEm_Body.Text;
        //    foreach (GridViewRow row in SSEm_Grid.Rows)
        //    {
        //        if ((row.FindControl("chk") as CheckBox).Checked && Convert.ToInt32(SSEm_Grid.DataKeys[row.RowIndex].Values[0]) != 0)
        //        {




        //            // string fileName = $"SalarySlip_Emp{Convert.ToString(SSEm_Grid.DataKeys[row.RowIndex].Values[0])}_{Convert.ToString(SSEm_Grid.DataKeys[row.RowIndex].Values[1]).Replace(" ", "_")}{DateTime.Now.ToString("yyyy-MM")}.txt";
        //            string fileName = $"SalarySlip_Emp{Convert.ToString(SSEm_Grid.DataKeys[row.RowIndex].Values[0])}_{Convert.ToString(SSEm_Grid.DataKeys[row.RowIndex].Values[1]).Replace(" ", "_")}{DateTime.Now.ToString("yyyy-MM")}.pdf";
        //            string filePath = Path.Combine(rootDirectoryPath, fileName);

        //            if (File.Exists(filePath))
        //            {
        //                string mail = Convert.ToString(SSEm_Grid.DataKeys[row.RowIndex].Values[2]);
        //                Email_Body(filePath, mail, body);
        //                Globals.Show("Message has been sent!");

        //            }

        //        }
        //    }


        //}
        protected void SSem_GenerateBtn_Click(object sender, EventArgs e)
        {
            string body = SSEm_Body.Text;
            foreach (GridViewRow row in SSEm_Grid.Rows)
            {
                if ((row.FindControl("chk") as CheckBox).Checked && Convert.ToInt32(SSEm_Grid.DataKeys[row.RowIndex].Values[0]) != 0)
                {
                    // Constructing file name
                    string fileName = $"SalarySlip_Emp{Convert.ToString(SSEm_Grid.DataKeys[row.RowIndex].Values[0])}_{Convert.ToString(SSEm_Grid.DataKeys[row.RowIndex].Values[1]).Replace(" ", "_")}_{DateTime.Now.ToString("yyyy-MM")}.pdf";
                    string filePath = Path.Combine(rootDirectoryPath, fileName);

                    if (File.Exists(filePath))
                    {
                        string mail = Convert.ToString(SSEm_Grid.DataKeys[row.RowIndex].Values[2]);
                        Email_Body(filePath, mail, body);
                        Globals.Show("Message has been sent!");
                    }
                }
            }
        }


        //protected void Email_Body(string filename, string ToAdd, string body)
        //{
        //    string smtpHost = "mail.ccsl.co.in";
        //    int smtpPort = 587;
        //    string smtpUsername = "canbankrta@ccsl.co.in";
        //    string smtpPassword = "can.12345";
        //    string fromEmail = "canbankrta@ccsl.co.in";
        //    string fromName = "Canbank Computer Services Ltd";
        //    string receiverEmail = ToAdd;
        //    MailMessage mail = new MailMessage();
        //    mail.From = new MailAddress(fromEmail, fromName);
        //    mail.To.Add(receiverEmail);
        //    mail.Subject = "Salary Slip";


        //    string attachmentPath = filename;
        //    if (File.Exists(attachmentPath))
        //    {
        //        Attachment attachment = new Attachment(attachmentPath);
        //        mail.Attachments.Add(attachment);
        //    }


        //    mail.Body = body;


        //    SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);
        //    smtpClient.UseDefaultCredentials = true;
        //    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
        //    smtpClient.EnableSsl = true;

        //    try
        //    {

        //        smtpClient.Send(mail);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {

        //        mail.Dispose();
        //    }
        //}
        //protected void Email_Body(string filename, string ToAdd, string body)
        //{
        //    string smtpHost = "mail.ccsl.co.in";
        //    int smtpPort = 587;
        //    string smtpUsername = "canbankrta@ccsl.co.in";
        //    string smtpPassword = "can.123456";
        //    string fromEmail = "canbankrta@ccsl.co.in";
        //    string fromName = "Canbank Computer Services Ltd";
        //    string receiverEmail = ToAdd;

        //    // Get the current month name
        //    string currentMonth = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);

        //    // Create the email subject with the current month
        //    string subject = $"Salary Slip for the Month of {currentMonth}";

        //    // Append additional information to the body
        //    string updatedBody = body + "\n\nPlease find the attached salary slip.";

        //    MailMessage mail = new MailMessage();
        //    mail.From = new MailAddress(fromEmail, fromName);
        //    mail.To.Add(receiverEmail);
        //    mail.Subject = subject;
        //    mail.Body = updatedBody;

        //    // Add attachment if the file exists
        //    string attachmentPath = filename;
        //    if (File.Exists(attachmentPath))
        //    {
        //        Attachment attachment = new Attachment(attachmentPath);
        //        mail.Attachments.Add(attachment);
        //    }

        //    SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);
        //    smtpClient.UseDefaultCredentials = true;
        //    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
        //    smtpClient.EnableSsl = true;

        //    try
        //    {
        //        // Send the email
        //        smtpClient.Send(mail);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        // Dispose the mail object
        //        mail.Dispose();
        //    }
        //}
        protected void Email_Body(string filename, string ToAdd, string body)
        {
            string smtpHost = "mail.ccsl.co.in";
            int smtpPort = 587;
            string smtpUsername = "canbankrta@ccsl.co.in";
            string smtpPassword = "can.123456";
            string fromEmail = "canbankrta@ccsl.co.in";
            string fromName = "Canbank Computer Services Ltd";
            string receiverEmail = ToAdd;

            // Get the current month name
            string currentMonth = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);

            // Create the email subject with the current month
            string subject = $"Salary Slip for the Month of {currentMonth}";

            // Append additional information to the body
            string updatedBody = body + "\n\nPlease find the attached salary slip.";

            // Additional message to be appended at the bottom
            

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail, fromName);
            mail.To.Add(receiverEmail);
            //mail.To.Add("pm148727@gmail.com");
            mail.Subject = subject;
            string footerMessage = "\n\n Dear Sir , Salary Enclosed.";
            updatedBody += footerMessage;
            mail.Body = updatedBody;

            // Add attachment if the file exists
            string attachmentPath = filename;
            if (File.Exists(attachmentPath))
            {
                Attachment attachment = new Attachment(attachmentPath);
                mail.Attachments.Add(attachment);
            }

            SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            smtpClient.EnableSsl = true;

            try
            {
                // Send the email
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // Dispose the mail object
                mail.Dispose();
            }
            
        }

    }

}
