using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRMS.App_Code;
using BL;
using BO;
using System.Collections;
using System.Text;
using System.Windows;
using System.Threading;

namespace HRMS
{
    public partial class HRMS_EmpMaster : System.Web.UI.Page
    {
        EmpDetailsBO EmpMast = new EmpDetailsBO();
        EmpDetailsBL bL = new EmpDetailsBL();
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    FillgrdUser();
                    TempMenuRemoval();

                }
            }
            catch
            {
                throw;
            }
            TitleLabel.Text = $"EMPLOYEE DETAILS";
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
        protected void FillgrdUser()
        {

            try
            {
                EmpMast.Action = "E";
                dt = bL.FetchEmpDetails(EmpMast);
                if (dt.Rows.Count > 0)
                {
                    grdUser.DataSource = dt;
                    grdUser.DataBind();
                    
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

   

        //protected void FillDD()
        //{

        //    //Desig
        //    Emp_Desig.DataValueField = "Designation";

        //    Emp_Desig.DataSource = dt;
        //    Emp_Desig.DataBind();
        //    //Grade
        //    Emp_Grade.DataValueField = "Grade";

        //    Emp_Grade.DataSource = dt;
        //    Emp_Grade.DataBind();
        //    //Dept
        //    Emp_Dept.DataValueField = "Department";

        //    Emp_Dept.DataSource = dt;
        //    Emp_Dept.DataBind();
        //    //Branch
        //    Emp_Branch.DataValueField = "Branch";

        //    Emp_Branch.DataSource = dt;
        //    Emp_Branch.DataBind();
        //    //State
        //    Emp_State.DataValueField = "StateCode";

        //    Emp_State.DataSource = dt;
        //    Emp_State.DataBind();
        //    ////ConfDt
        //    //Emp_DOC.DataValueField = "Emp_ConfDt";
        //    //Emp_DOC.DataSource = dt;
        //    //Emp_DOC.DataBind();
        //    ////IncrDt
        //    //Emp_DOI.DataValueField = "Emp_IncrementDt";
        //    //Emp_DOI.DataSource = dt;
        //    //Emp_DOI.DataBind();
        //    ////PromDt
        //    //Emp_DOP.DataValueField = "Emp_PromotionDt";
        //    //Emp_DOP.DataSource = dt;
        //    //Emp_DOP.DataBind();
        //    //Martial Status
        //    //Emp_MS.DataValueField = "Emp_Marital_Status";
        //    //Emp_MS.DataSource = dt;
        //    //Emp_MS.DataBind();
        //    //Emp_CState
        //    Emp_CState.DataValueField = "CState";

        //    Emp_CState.DataSource = dt;
        //    Emp_CState.DataBind();
        //    //Emp_SCBank
        //    Emp_SCBank.DataValueField = "SalBank";

        //    Emp_SCBank.DataSource = dt;
        //    Emp_SCBank.DataBind();
        //    //Emp_SCBranch
        //    Emp_SCBranch.DataValueField = "SalBranch";

        //    Emp_SCBranch.DataSource = dt;
        //    Emp_SCBranch.DataBind();
        //    //Emp_OT
        //    //Emp_OT.DataValueField = "Emp_OT_Ind";
        //    //Emp_OT.DataSource = dt;
        //    //Emp_OT.DataBind();
        //    //Emp_SP
        //    //Emp_SP.DataValueField = "Emp_Stop_Pay";
        //    //Emp_SP.DataSource = dt;
        //    //Emp_SP.DataBind();
        //    //Emp_Depu
        //    //Emp_Depu.DataValueField = "Emp_Deput_Indi";
        //    //Emp_Depu.DataSource = dt;
        //    //Emp_Depu.DataBind();
        //    //Emp_AG
        //    //Emp_AG.DataValueField = "Emp_Accomadation";
        //    //Emp_AG.DataSource = dt;
        //    //Emp_AG.DataBind();
        //    //Emp_CC
        //    Emp_CC.DataValueField = "City_Class";

        //    Emp_CC.DataSource = dt;
        //    Emp_CC.DataBind();
        //    //Emp_Metro
        //    //Emp_Metro.DataValueField = "Emp_Metro";
        //    //Emp_Metro.DataSource = dt;
        //    //Emp_Metro.DataBind();
        //    //Emp_Vehi
        //    Emp_Vehi.DataValueField = "Vehicle";

        //    Emp_Vehi.DataSource = dt;
        //    Emp_Vehi.DataBind();

        //    //Emp_PState
        //    Emp_PState.DataValueField = "PState";

        //    Emp_PState.DataSource = dt;
        //    Emp_PState.DataBind();
        //}

        protected void FillDD(int i)
        {
            DataTable et = new DataTable();

            EmpDetailsBO BOEmployeeMast = new EmpDetailsBO();
            EmpDetailsBL BLEmployeeMast = new EmpDetailsBL();
            BOEmployeeMast.Action = "M";
            BOEmployeeMast.Emp_No = i;
            et = BLEmployeeMast.FetchEmpDetails(BOEmployeeMast);
            if(et.Rows.Count>0)
            {
                EDGrd.DataSource = et;
                EDGrd.DataBind();
            }
        }

        protected void EmpDetails(int i)
        {
            EmpMast.Action = "V";
            EmpMast.Emp_No = i;
            dt = bL.FetchEmpDetails(EmpMast);
            if(dt.Rows.Count>0)
            {
                //Textbox
                FillDDrp();
                Emp_Name.Text = dt.Rows[0]["Emp_Name"].ToString();
                Emp_FatherName.Text = dt.Rows[0]["Emp_Father_name"].ToString();
                Emp_SpouseName.Text = dt.Rows[0]["Emp_Spouse_name"].ToString();
                Emp_DOB.Text = Convert.ToDateTime(dt.Rows[0]["Emp_DOB"]).ToString("yyyy-MM-dd");
                Emp_DOJ.Text = Convert.ToDateTime (dt.Rows[0]["Emp_JoinDt"]).ToString("yyyy-MM-dd");
                Emp_DOC.Text = Convert.ToDateTime(dt.Rows[0]["Emp_JoinDt"]).ToString("yyyy-MM-dd");
                Emp_DOI.Text = Convert.ToDateTime(dt.Rows[0]["Emp_JoinDt"]).ToString("yyyy-MM-dd");
                Emp_DOP.Text = Convert.ToDateTime(dt.Rows[0]["Emp_JoinDt"]).ToString("yyyy-MM-dd");
                
                Emp_Qual.Text = dt.Rows[0]["Emp_qualification"].ToString();
                Emp_PFNom.Text = dt.Rows[0]["Emp_PF_Nom"].ToString();
                Emp_PFNo.Text = dt.Rows[0]["Emp_PF_No"].ToString();
                Emp_EPSNo.Text = dt.Rows[0]["Emp_EPS_No"].ToString();
                Emp_UAN.Text = dt.Rows[0]["Emp_UAN"].ToString();
                Emp_CAdd1.Text = dt.Rows[0]["Emp_Res_Add1"].ToString();
                Emp_CAdd2.Text = dt.Rows[0]["Emp_Res_Add2"].ToString();
                Emp_CAdd3.Text = dt.Rows[0]["Emp_Res_Add3"].ToString();
                Emp_CCity.Text = dt.Rows[0]["Emp_Res_City"].ToString();
                
                Emp_CPin.Text = dt.Rows[0]["Emp_Res_pin"].ToString();
                Emp_ACNo.Text = dt.Rows[0]["Emp_AccNo"].ToString();
                Emp_VPF.Text = dt.Rows[0]["Emp_VPF_Contri"].ToString();
                Emp_HRP.Text = dt.Rows[0]["Emp_House_Rpaid"].ToString();
                Emp_LTC.Text = dt.Rows[0]["Emp_LTC_Availed"].ToString();
                //Emp_DOR.Text = dt.Rows[0]["Emp_ResigDt"].ToString();
                Emp_PAN.Text = dt.Rows[0]["Emp_PAN"].ToString();
               // Emp_Aadhar.Text = dt.Rows[0]["Emp_AadharNo"].ToString();
                Emp_PAdd.Text = dt.Rows[0]["Emp_PAdd1"].ToString();
                Emp_PAdd1.Text = dt.Rows[0]["Emp_PAdd2"].ToString();
                Emp_PAdd2.Text = dt.Rows[0]["Emp_PAdd3"].ToString();
                Emp_PCity.Text = dt.Rows[0]["Emp_PCity"].ToString();
                
                Emp_PPin.Text = dt.Rows[0]["Emp_Ppin"].ToString();
                Emp_Email.Text = dt.Rows[0]["Emp_EMailID"].ToString();
                if (dt.Rows[0]["Emp_Accomadation"].ToString() != "")
                {
                    Emp_AG.SelectedValue = dt.Rows[0]["Emp_Accomadation"].ToString();

                }
                if(dt.Rows[0]["Emp_Sex"].ToString() != "")
                {
                    Emp_Gen.SelectedValue = dt.Rows[0]["Emp_Sex"].ToString();
                }
                if(dt.Rows[0]["Emp_Marital_Status"].ToString() != "")
                {
                    Emp_MS.SelectedValue = dt.Rows[0]["Emp_Marital_Status"].ToString();
                }
                if(dt.Rows[0]["Emp_OT_Ind"].ToString() != "")
                {
                    Emp_OT.SelectedValue = dt.Rows[0]["Emp_OT_Ind"].ToString();
                }
                if(dt.Rows[0]["Emp_Stop_Pay"].ToString() != "")
                {
                    Emp_SP.SelectedValue = dt.Rows[0]["Emp_Stop_Pay"].ToString();
                }
                if(dt.Rows[0]["Emp_Deput_Indi"].ToString() != "")
                {
                    Emp_Depu.SelectedValue = dt.Rows[0]["Emp_Deput_Indi"].ToString();
                }
                if(dt.Rows[0]["Emp_Metro"].ToString() != "")
                {
                    Emp_Metro.SelectedValue = dt.Rows[0]["Emp_Metro"].ToString();
                }
                
                
                
               
               
               


                //DropDown
                Emp_Desig.SelectedValue = dt.Rows[0]["Emp_Desig"].ToString();
                Emp_Dept.SelectedValue = dt.Rows[0]["Emp_Dept"].ToString();
                Emp_CC.SelectedValue = dt.Rows[0]["Emp_City_Class"].ToString();
                Emp_Grade.SelectedValue= dt.Rows[0]["Emp_Grade"].ToString();
                Emp_State.SelectedValue= dt.Rows[0]["Emp_StateCode"].ToString();
                Emp_CState.SelectedValue = dt.Rows[0]["Emp_Res_State"].ToString();
                Emp_PState.SelectedValue = dt.Rows[0]["Emp_PState"].ToString();
                Emp_Vehi.SelectedValue= dt.Rows[0]["Emp_Vehicle"].ToString();
                Emp_Branch.SelectedValue = dt.Rows[0]["Emp_Branch"].ToString();
                if(dt.Rows[0]["Emp_SalBank"].ToString()=="1")
                {
                    Emp_SCBank.SelectedValue = dt.Rows[0]["Emp_SalBank"].ToString();
                }
                
                Emp_SCBranch.SelectedValue = dt.Rows[0]["Emp_SalBranch"].ToString();
            }

        }

        protected void AddDetails()
        {
            EmpMast.Emp_Name = Emp_Name.Text;
            EmpMast.Emp_Father_name = Emp_FatherName.Text;
            EmpMast.Emp_Spouse_name = Emp_SpouseName.Text;
            EmpMast.Emp_DOB = Convert.ToDateTime(Emp_DOB.Text);
            EmpMast.Emp_JoinDt= Convert.ToDateTime(Emp_DOJ.Text);
            EmpMast.Emp_IncrementDt= Convert.ToDateTime(Emp_DOI.Text);
            EmpMast.Emp_ConfDt= Convert.ToDateTime(Emp_DOC.Text);
            EmpMast.Emp_PromotionDt= Convert.ToDateTime(Emp_DOP.Text);
            EmpMast.Emp_AccNo = Emp_ACNo.Text;
            EmpMast.Emp_EMailID = Emp_Email.Text;
            EmpMast.Emp_EPS_No = Emp_EPSNo.Text;
            EmpMast.Emp_House_Rpaid = (float)Convert.ToDecimal(Emp_HRP.Text);
            EmpMast.Emp_LTC_Availed= Convert.ToInt16(Emp_LTC.Text);
            EmpMast.Emp_PAdd1 = Emp_PAdd.Text;
            EmpMast.Emp_PAdd2 = Emp_PAdd1.Text;
            EmpMast.Emp_PAdd3 = Emp_PAdd2.Text;
            EmpMast.Emp_PAN = Emp_PAN.Text;
            EmpMast.Emp_AadharNo = Emp_Aadhar.Text;
            EmpMast.Emp_PCity = Emp_PCity.Text;
            EmpMast.Emp_PF_No = Emp_PFNo.Text;
            EmpMast.Emp_PF_Nom = Emp_PFNom.Text;
            EmpMast.Emp_Ppin = Emp_PPin.Text;
            EmpMast.Emp_qualification = Emp_Qual.Text;
            EmpMast.Emp_Res_Add1 = Emp_CAdd1.Text;
            EmpMast.Emp_Res_Add2 = Emp_CAdd2.Text;
            EmpMast.Emp_Res_Add3 = Emp_CAdd3.Text;
            EmpMast.Emp_Res_City = Emp_CCity.Text;
            EmpMast.Emp_Res_pin = Emp_CPin.Text;
            EmpMast.Emp_UAN = Emp_UAN.Text;
            EmpMast.Emp_VPF_Contri = (float)Convert.ToDecimal( Emp_VPF.Text);
            EmpMast.Emp_Sex = Convert.ToChar(Emp_Gen.SelectedValue.ToString());
            EmpMast.Emp_Marital_Status= Convert.ToChar(Emp_MS.SelectedValue.ToString());
            EmpMast.Emp_OT_Ind= Convert.ToChar(Emp_OT.SelectedValue.ToString());
            EmpMast.Emp_Stop_Pay= Convert.ToChar(Emp_SP.SelectedValue.ToString());
            EmpMast.Emp_Deput_Indi= Convert.ToChar(Emp_Depu.SelectedValue.ToString());
            EmpMast.Emp_Accomadation= Convert.ToChar(Emp_AG.SelectedValue.ToString());
            EmpMast.Emp_Metro= Convert.ToChar(Emp_Metro.SelectedValue.ToString());
            EmpMast.Emp_Desig = Convert.ToInt16(Emp_Desig.SelectedValue);
            EmpMast.Emp_Dept= Convert.ToInt16(Emp_Dept.SelectedValue);
            EmpMast.Emp_City_Class= Convert.ToInt16(Emp_CC.SelectedValue);
            EmpMast.Emp_Grade= Convert.ToInt16(Emp_Grade.SelectedValue);
            EmpMast.Emp_StateCode= Convert.ToInt16(Emp_State.SelectedValue);
            EmpMast.Emp_Res_State= Convert.ToInt16(Emp_CState.SelectedValue);
            EmpMast.Emp_PState= Convert.ToInt16(Emp_PState.SelectedValue);
            EmpMast.Emp_Vehicle = Convert.ToInt16(Emp_Vehi.SelectedValue);
            EmpMast.Emp_Branch= Convert.ToInt16(Emp_Branch.SelectedValue);
            EmpMast.Emp_SalBranch= Convert.ToInt16(Emp_SCBranch.SelectedValue);
            EmpMast.Emp_SalBank= Convert.ToInt16(Emp_SCBank.SelectedValue);
        }

        protected void enableView()
        {
            Emp_Name.ReadOnly = true;
            Emp_FatherName.ReadOnly = true;
            Emp_SpouseName.ReadOnly = true;
            Emp_Gen.Enabled = false;
            Emp_Qual.ReadOnly = true;
            Emp_PFNom.ReadOnly = true;
            Emp_PFNo.ReadOnly = true;
            Emp_EPSNo.ReadOnly = true;
            Emp_UAN.ReadOnly = true;
            Emp_CAdd1.ReadOnly = true;
            Emp_CAdd2.ReadOnly = true;
            Emp_CAdd3.ReadOnly = true;
            Emp_CCity.ReadOnly = true;
            Emp_CPin.ReadOnly = true;
            Emp_ACNo.ReadOnly = true;
            Emp_VPF.ReadOnly = true;
            Emp_HRP.ReadOnly = true;
            Emp_LTC.ReadOnly = true;
            Emp_PAN.ReadOnly = true;
            Emp_PAdd.ReadOnly = true;
            Emp_PAdd1.ReadOnly = true;
            Emp_PAdd2.ReadOnly = true;
            Emp_PCity.ReadOnly = true;
            Emp_PPin.ReadOnly = true;
            Emp_Email.ReadOnly = true;
            Emp_MS.Enabled = false;
            BtnSave.Visible = false;
            EDGrd.Visible = true;

        }

        protected void EnableEdit()
        {
            
            Emp_Name.ReadOnly = false;
            Emp_FatherName.ReadOnly = false;
            Emp_SpouseName.ReadOnly = false;
            Emp_Gen.Enabled = true;
            Emp_Qual.ReadOnly = false;
            Emp_PFNom.ReadOnly = false;
            Emp_PFNo.ReadOnly = false;
            Emp_EPSNo.ReadOnly = false;
            Emp_UAN.ReadOnly = false;
            Emp_CAdd1.ReadOnly = false;
            Emp_CAdd2.ReadOnly = false;
            Emp_CAdd3.ReadOnly = false;
            Emp_CCity.ReadOnly = false;
            Emp_CPin.ReadOnly = false;
            Emp_ACNo.ReadOnly = false;
            Emp_VPF.ReadOnly = false;
            Emp_HRP.ReadOnly = false;
            Emp_LTC.ReadOnly = false;
            Emp_PAN.ReadOnly = false;
            Emp_PAdd.ReadOnly = false;
            Emp_PAdd1.ReadOnly = false;
            Emp_PAdd2.ReadOnly = false;
            Emp_PCity.ReadOnly = false;
            Emp_PPin.ReadOnly = false;
            Emp_Email.ReadOnly = false;
            Emp_MS.Enabled = true;
            BtnSave.Visible = true;
            
        }
        protected void Clear()
        {
            Emp_Name.Text = "";
            Emp_FatherName.Text = "";
            Emp_SpouseName.Text = "";
            Emp_Gen.SelectedIndex = -1;
            Emp_Qual.Text = "";
            Emp_DOB.Text = "";
            Emp_DOJ.Text = "";
            Emp_DOC.Text = "";
            Emp_DOI.Text = "";
            Emp_DOP.Text = "";
            Emp_MS.SelectedIndex = -1;
            Emp_PFNom.Text = "";
            Emp_PFNo.Text = "";
            Emp_EPSNo.Text = "";
            Emp_UAN.Text = "";
            Emp_CAdd1.Text = "";
            Emp_CAdd2.Text = "";
            Emp_CAdd3.Text = "";
            Emp_CCity.Text = "";
            Emp_CPin.Text = "";
            Emp_ACNo.Text = "";
            Emp_VPF.Text = "";
            Emp_HRP.Text = "";
            Emp_LTC.Text = "";
            Emp_PAN.Text = "";
            Emp_PAdd.Text = "";
            Emp_PAdd1.Text = "";
            Emp_PAdd2.Text = "";
            Emp_PCity.Text = "";
            Emp_PPin.Text = "";
            Emp_Email.Text = "";
            Emp_OT.SelectedIndex = -1;
            Emp_SP.SelectedIndex = -1;
            Emp_Depu.SelectedIndex = -1;
            Emp_Metro.SelectedIndex = -1;
            EDGrd.Visible = false;
        }
        protected void grdUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            
                grdUser.PageIndex = e.NewPageIndex;
                FillgrdUser();
            
            
        }

        protected void grdUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName=="View")
            {
                Session["userID"] = (int)grdUser.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0];
                EmpDetails(Convert.ToInt32(Session["userID"]));
                FillDD(Convert.ToInt32(Session["userID"]));
                MultiView1.ActiveViewIndex = 1;
                enableView();
                
            }
            else if (e.CommandName=="Edit")
            {
               
            }
        }
        //protected void EmpMasSearchBtn_Click(object sender, EventArgs e)
        //{
        //    if(EmpSearch.Text !="")
        //    {
        //        EmpMast.Action = "S";
        //        EmpMast.Emp_Name = EmpSearch.Text;
        //        dt = bL.FetchEmpDetails(EmpMast);
        //        if (dt.Rows.Count > 0)
        //        {
        //            grdUser.DataSource = dt;
        //            grdUser.DataBind();
        //        }
        //        else if(dt.Rows.Count==0)
        //        {
                    
        //            Globals.Show("Enter the Full Name or Employee Name not Available/Active");
        //        }
        //    }
        //}

        protected void AddBtn_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 1;
            EnableEdit();
            FillDDrp();
            Session["Status"] = "Add";

        }

        protected void HRMSEmpCan_Click(object sender, EventArgs e)
        {
            Clear();
            MultiView1.ActiveViewIndex = 0;
        }
        
        protected void DelBtn_Click(object sender, EventArgs e)
         {
            foreach(GridViewRow row in grdUser.Rows)
            {
                if ((row.FindControl("chk") as CheckBox).Checked && Convert.ToInt32(grdUser.DataKeys[row.RowIndex].Value) != 0)
                {
                    
                    EmpMast.Emp_No = Convert.ToInt32(grdUser.DataKeys[row.RowIndex].Value);
                    EmpMast.Action = "D";
                    string msg = bL.AUDEmpDetails(EmpMast);
                    Globals.Show(msg);
                }
                else
                {
                    string msg = "Please select the checkbox to highlight an Employee!";
                    Globals.Show(msg);
                }
            }
            this.FillgrdUser();
        }
        
        //protected void EmpMast_BtnReset_Click(object sender, EventArgs e)
        //{

        //    FillgrdUser();
        //}

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if(Session["Status"].ToString()=="Add")
            {
                EmpMast.Action = "A";
                AddDetails();
                string msg = bL.AUDEmpDetails(EmpMast);
                Globals.Show(msg);
            }
            else if (Session["Status"].ToString() == "Edit")
            {
                EmpMast.Action = "U";
                EmpMast.Emp_No = Convert.ToInt32(Session["userID"]);
                AddDetails();
                string msg = bL.AUDEmpDetails(EmpMast);
                Globals.Show(msg);
                MultiView1.ActiveViewIndex = 0;
            }
        }

        protected void grdUser_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Session["userID"] = (int)grdUser.DataKeys[Convert.ToInt32(e.RowIndex)].Values[0];
            EmpDetails(Convert.ToInt32(Session["userID"]));
            MultiView1.ActiveViewIndex = 1;
            EnableEdit();
            Session["Status"] = "Edit";
        }
        private void FillDDrp()
        {
           
            DataTable cTable = new DataTable();
            try
            {
                EmpMast.Action = "B";
                EmpMast.MCode = 10;
                if(EmpMast.MCode==10)
                {
                    cTable = bL.FetchEmpDetails(EmpMast);
                    Emp_Desig.DataSource = cTable;
                    Emp_Desig.DataValueField = "Hrm_Scode";
                    Emp_Desig.DataTextField = "Hrm_Desc";
                    Emp_Desig.DataBind();
                    Emp_Desig.Items.Insert(0, new ListItem("----- Select/No Value -----", ""));
                }
                cTable.Clear();
                EmpMast.MCode = 3;
                if (EmpMast.MCode == 3)
                {
                    cTable = bL.FetchEmpDetails(EmpMast);
                    Emp_Dept.DataSource = cTable;
                    Emp_Dept.DataValueField = "Hrm_Scode";
                    Emp_Dept.DataTextField = "Hrm_Desc";
                    Emp_Dept.DataBind();
                    Emp_Dept.Items.Insert(0, new ListItem("----- Select/No Value -----", ""));
                }
                cTable.Clear();
                EmpMast.MCode = 5;
                if (EmpMast.MCode == 5)
                {
                    cTable = bL.FetchEmpDetails(EmpMast);
                    Emp_CC.DataSource = cTable;
                    Emp_CC.DataValueField = "Hrm_Scode";
                    Emp_CC.DataTextField = "Hrm_Desc";
                    Emp_CC.DataBind();
                    Emp_CC.Items.Insert(0, new ListItem("----- Select/No Value -----", ""));
                }
                cTable.Clear();
                EmpMast.MCode = 15;
                if (EmpMast.MCode == 15)
                {
                    cTable = bL.FetchEmpDetails(EmpMast);
                    Emp_Grade.DataSource = cTable;
                    Emp_Grade.DataValueField = "Hrm_Scode";
                    Emp_Grade.DataTextField = "Hrm_Desc";
                    Emp_Grade.DataBind();
                    Emp_Grade.Items.Insert(0, new ListItem("----- Select/No Value -----", ""));
                }

                cTable.Clear();
                EmpMast.MCode = 12;
                if (EmpMast.MCode == 12)
                {
                    cTable = bL.FetchEmpDetails(EmpMast);
                    Emp_State.DataSource = cTable;
                    Emp_State.DataValueField = "Hrm_Scode";
                    Emp_State.DataTextField = "Hrm_Desc";
                    Emp_State.DataBind();
                    Emp_State.Items.Insert(0, new ListItem("----- Select/No Value -----", ""));

                    Emp_CState.DataSource = cTable;
                    Emp_CState.DataValueField = "Hrm_Scode";
                    Emp_CState.DataTextField = "Hrm_Desc";
                    Emp_CState.DataBind();
                    Emp_CState.Items.Insert(0, new ListItem("----- Select/No Value -----", ""));

                    Emp_PState.DataSource = cTable;
                    Emp_PState.DataValueField = "Hrm_Scode";
                    Emp_PState.DataTextField = "Hrm_Desc";
                    Emp_PState.DataBind();
                    Emp_PState.Items.Insert(0, new ListItem("----- Select/No Value -----", ""));

                }
                cTable.Clear();
                EmpMast.MCode = 2;
                if (EmpMast.MCode == 2)
                {
                    cTable = bL.FetchEmpDetails(EmpMast);
                    Emp_Vehi.DataSource = cTable;
                    Emp_Vehi.DataValueField = "Hrm_Scode";
                    Emp_Vehi.DataTextField = "Hrm_Desc";
                    Emp_Vehi.DataBind();
                    Emp_Vehi.Items.Insert(0, new ListItem("----- Select/No Value -----", ""));
                }
                EmpMast.Action = "C";
                cTable.Clear();
                
                if (EmpMast.Action == "C")
                {
                    cTable = bL.FetchEmpDetails(EmpMast);
                    Emp_Branch.DataSource = cTable;
                    Emp_Branch.DataValueField = "CBr_Code";
                    Emp_Branch.DataTextField = "CBr_Name";
                    Emp_Branch.DataBind();
                    Emp_Branch.Items.Insert(0, new ListItem("----- Select/No Value -----", ""));
                }
                EmpMast.Action = "D";
                cTable.Clear();
                if (EmpMast.Action == "D")
                {
                    cTable = bL.FetchEmpDetails(EmpMast);
                    Emp_SCBank.DataSource = cTable;
                    Emp_SCBank.DataValueField = "Bank_Code";
                    Emp_SCBank.DataTextField = "Bank_Name";
                    Emp_SCBank.DataBind();
                    Emp_SCBank.Items.Insert(0, new ListItem("----- Select/No Value -----", ""));
                }
                EmpMast.Action = "R";
                cTable.Clear();
                
                if (EmpMast.Action == "R")
                {
                    cTable = bL.FetchEmpDetails(EmpMast);
                    Emp_SCBranch.DataSource = cTable;
                    Emp_SCBranch.DataValueField = "Bank_BrCode";
                    Emp_SCBranch.DataTextField = "Bank_BrName";
                    Emp_SCBranch.DataBind();
                    Emp_SCBranch.Items.Insert(0, new ListItem("----- Select/No Value -----", ""));
                }
                cTable.Clear();

                // Emp_Desig.Items.Insert(0, new ListItem("---- Select ----", "0"));
                // pnlNew.Visible = true;
            }
            catch (Exception ee)
            {
                Globals.Show(ee.Message.ToString());
            }
            finally
            {
                bL = null;
                EmpMast = null;
                cTable.Dispose();
            }
        }

        protected void BackBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void Cpy_PAdd_Click(object sender, EventArgs e)
        {
            string text = String.Join(",", Emp_PAdd.Text, Emp_PAdd1.Text, Emp_PAdd2.Text,Emp_PCity.Text,Emp_PState.Text,Emp_PPin.Text);
            Thread thread = new Thread(() => Clipboard.SetText(text));
            thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread.Start();
            thread.Join();
            
        }

        protected void rbl_SelectedIndexChanged(object sender, EventArgs e)
        {
            EmpMast.SV = Convert.ToInt16(rbl.SelectedValue);
            EmpMast.Action = "S";
            try
            {
                dt = bL.FetchEmpDetails(EmpMast);
                if (dt.Rows.Count > 0)
                {
                    grdUser.DataSource = dt;
                    grdUser.DataBind();
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
    }
}

