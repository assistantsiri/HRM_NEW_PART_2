<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Salary_Register.aspx.cs" Inherits="HRMS.Salary_Register" MasterPageFile="~/HRMS.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
    <div class="container mt-5">
         <div class="card" style="width:50%">
            <div class="card-body" style="background-color:gray">
        <center>
            
        <h2 class="text-center mb-4">Salary Register</h2>
        
                <div class="form-group">
                    <label for="ddlBranch">
                        <i class="bi bi-bank"></i> Branch:
                    </label>
                    <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" style="width:50%"></asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="txtPayableDate">
                        <i class="bi bi-calendar-month"></i> Payable Date:
                    </label>
                    <asp:TextBox ID="txtPayableDate" runat="server" TextMode="Date" CssClass="form-control" style="width:30%"></asp:TextBox>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label for="txtFromEmpNo">
                            <i class="bi bi-person-circle"></i> From No:
                        </label>
                        <asp:DropDownList ID="ddlFromEmpNo" runat="server" CssClass="form-control" style="width:40%"></asp:DropDownList>
                        
                        <label for="txtToEmpNo">
                            <i class="bi bi-person-circle"></i> To No:
                        </label>
                        <asp:DropDownList ID="ddlToEmpNo" runat="server" CssClass="form-control" style="width:40%"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label for="txtDescription">
                        <i class="bi bi-file-text-fill"></i> Description:
                    </label>
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" style="width:50%"></asp:TextBox>
                    <br />
                </div>
                <div class="d-flex justify-content-center">
                    <asp:Button ID="btnPrint" runat="server" Text="Print_PDF_Repo" OnClick="btnPrint_Click" CssClass="btn btn-success mr-2" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnPrinttext" runat="server" Text="Print_Text_Repo" OnClick="btnPrint_Click_txt" CssClass="btn btn-success mr-2" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="btn btn-danger" />
                </div>
                <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
                
                &nbsp;<asp:GridView ID="gvEmployees" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="Emp_Name" HeaderText="Employee Name" />
                        <asp:BoundField DataField="Emp_No" HeaderText="Employee Number" />
                        <asp:BoundField DataField="Branch" HeaderText="Branch" />
                    </Columns>
                </asp:GridView>
            </div>
            </div>
        </center>
    </div>
</asp:Content>
