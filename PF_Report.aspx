<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PF_Report.aspx.cs" Inherits="HRMS.PF_Report" MasterPageFile="~/HRMS.Master" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
        <div class="container mt-5">
         <div class="card" style="width:50%">
            <div class="card-body"  style="background-color:gray">
        <center>
            
     <div class="form-container">
             <div class="form-group" id="form">
                       <label for="branchDropdown">
                            <i class="bi bi-bank"></i>Branch</label>
                       <asp:DropDownList ID="branchDropdown" CssClass="colorful-dropdown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="branchDropdown_SelectedIndexChanged" style="width: 80%;">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                         </asp:DropDownList>

                  <div class="form-group text-center">
                           <br />
                           <asp:Button ID="btnPrint" runat="server" Text="Generate PF Reprot" OnClick="btnPrint_Click" CssClass="btn btn-success mr-2" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="btn btn-danger" />
              </div>
             </div>
           </div>
            </center>
                </div>
             </div>
            </div>
   </asp:Content>




