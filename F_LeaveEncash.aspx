<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="F_LeaveEncash.aspx.cs" Inherits="HRMS.F_LeaveEncash" MasterPageFile="~/HRMS.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
    <div class="container mt-5">
        <div class="card p-3 mb-2 bg-secondary text-white" style="width:100%">
            <div >
                  <div class="card-body " >
                <center>
                    <!-- Page Heading -->
                    <h2>Leave Encashment </h2>
                     <div class="card p-3 mb-2 bg-white text-grey" style="width:80%">
                     <div class="container mt-3">
                     <div class="card-body" >
                    <div class="row mb-3" >
                        <div class="col-md-6" ">
                            <div>
                              <asp:Label ID="Label5" runat="server" CssClass="label" Text="Branch Code:"></asp:Label>
                              <asp:DropDownList ID="ddlBranch" runat="server" CssClass="dropdown" style="left: 2px; top: 0px"></asp:DropDownList>
                                &nbsp;
                              <asp:Label ID="Label1" runat="server" CssClass="label" Text="Employee No:"></asp:Label>
                              <asp:DropDownList ID="ddlEmployeeNo" runat="server" CssClass="dropdown"></asp:DropDownList>
                           
                            <br />
                            <br />
                            <br />
                           
                      </div><br />
                        <div>
                              <asp:Label ID="Label2" runat="server" CssClass="label" Text="Emp Name:"></asp:Label>
                              <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="textbox" Width="91px" ></asp:TextBox>
                     
                     &nbsp;&nbsp;&nbsp;&nbsp;
                              <asp:Label ID="Label3" runat="server" CssClass="label" Text="Branch:"></asp:Label>
                              <asp:TextBox ID="txtBranchName" runat="server" CssClass="textbox" Width="91px" ></asp:TextBox>
                              <br />
                              <br />
                              <br />
                              <br />
                              <br />
                    </div>
                            </div>
                        </div>
                         </div>
                         </div>
                         </div>
                    </center>
                      </div>
                </div>
            </div>
        </div>
    </asp:Content>