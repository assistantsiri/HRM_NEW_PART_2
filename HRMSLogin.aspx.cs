using System;
using HRMS.App_Code;
using BO;
using BL;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRMS
{

    public partial class HRMSLogin : System.Web.UI.Page
    {
        UserProfileBO upbo = new UserProfileBO();
        UserProflieBL upbl = new UserProflieBL();
        Encryption PswdEn = new Encryption();
        protected void Page_Load(object sender, EventArgs e)
        {


        }

        protected void LgnBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                upbo.UserId = username.Text.Trim();
                upbo.Pwd = PswdEn.Encrypt(password.Text.Trim());
                upbo.Action = "A";
                dt = upbl.FetchUserMast(upbo);

                
                if ( dt.Rows.Count > 0)
                {
                  
                    Session["StaffNum"] = upbo.UserId;
                    Session["StaffName"] = dt.Rows[0]["up_UserName"];
                    Session["StaffMode"] = dt.Rows[0]["up_UserMode"];
                    if (Convert.ToString(Session["StaffMode"])=="1")
                    {
                        Session["OFYTYP"] = "AD";

                    }
                    else if (Convert.ToString(Session["StaffMode"]) == "2")
                    {
                        Session["OFYTYP"] = "ED";
                    }
                        Response.Redirect("Home.aspx");
                }
                else
                {

                    Globals.ShowError("Invalid Password");
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}