using System;
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
    public partial class HRMS : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                DateTime now = DateTime.Now;

                HRMSDandT.Text = now.ToString();
                //HRMSlblStaffName.Text = Session["StaffName"].ToString();
                MenuMastBO MenuMast = new MenuMastBO();
                MenuMastBL p1 = new MenuMastBL();
                DataTable dTable = new DataTable();
                MenuMast.HMM_OFFICE_TYP = Convert.ToString(Session["OFYTYP"]);
                MenuMast.Action = "M";
                dTable = p1.FetchMenuMast(MenuMast);
                Byte L1 = 0;
                Byte L2 = 0;
                Byte L3 = 0;
                Byte L4 = 0;
                Byte L5 = 0;
                foreach (DataRow drow in dTable.Rows)
                {
                    System.Web.UI.WebControls.MenuItem mnuitem = new System.Web.UI.WebControls.MenuItem();
                    L1 = Convert.ToByte(drow["HMM_MENU_LEVEL1"]);
                    L2 = Convert.ToByte(drow["HMM_MENU_LEVEL2"]);
                    L3 = Convert.ToByte(drow["HMM_MENU_LEVEL3"]);
                    L4 = Convert.ToByte(drow["HMM_MENU_LEVEL4"]);
                    L5 = Convert.ToByte(drow["HMM_MENU_LEVEL5"]);
                    mnuitem.Text = Convert.ToString(drow["HMM_MENU_TITLE"]);
                    mnuitem.NavigateUrl = Convert.ToString(drow["HMM_MENU_URL"]);
                    if (string.IsNullOrEmpty(mnuitem.NavigateUrl))
                    {
                        mnuitem.NavigateUrl = "javascript:void(0)";
                        mnuitem.Target = "";
                        
                    }
                    if (L2 == 0 && L3 == 0 && L4 == 0 && L5 == 0)
                    {
                        HRMSMMenu.Items.Add(mnuitem);
                    }
                    else if (L2 > 0 && L3 == 0 && L4 == 0 && L5 == 0)
                    {
                        HRMSMMenu.Items[L1 - 1].ChildItems.Add(mnuitem);
                    }
                    else if (L2 > 0 && L3 > 0 && L4 == 0 && L5 == 0)
                    {
                        HRMSMMenu.Items[L1 - 1].ChildItems[L2 - 1].ChildItems.Add(mnuitem);
                    }
                    else if (L2 > 0 && L3 > 0 && L4 > 0 && L5 == 0)
                    {
                        HRMSMMenu.Items[L1 - 1].ChildItems[L2 - 1].ChildItems[L3 - 1].ChildItems.Add(mnuitem);
                    }
                    else if (L2 > 0 && L3 > 0 && L4 > 0 && L5 > 0)
                    {
                        HRMSMMenu.Items[L1 - 1].ChildItems[L2 - 1].ChildItems[L3 - 1].ChildItems[L4 - 1].ChildItems.Add(mnuitem);
                    }
                    p1 = null;
                    MenuMast = null;
                    dTable.Dispose();
                }
            }
        }

        protected void LogoffBtn_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("HRMSLogin.aspx?Message=Thanks for using HRMS Software!", false);
        }

        protected void HomeBtn_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void HRMSMasterMenu_MenuItemClick(object sender, MenuEventArgs e)
        {

        }

    }
}