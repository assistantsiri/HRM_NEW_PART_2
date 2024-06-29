<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Salary_Credit_Bank_Report.aspx.cs" Inherits="HRMS.Salary_Credit_Bank_Report"  MasterPageFile="~/HRMS.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
    <div class="container mt-5">
         <div class="card" style="width:50%">
            <div class="card-body"  style="background-color:gray">
        <center>
            
    <div class="container form-group"  id="form">
        <center>
            <h2 class="text-center mb-4">Salary-Credit-Report</h2>
            <div class="form-group">
                <label for="bankDropdown">
                    <i class="bi bi-bank2"></i> Bank</label>
                <asp:DropDownList ID="bankDropdown" CssClass="form-control" style="width: 40%;" runat="server" AutoPostBack="true" OnSelectedIndexChanged="bankDropdown_SelectedIndexChanged">
                   <%-- <asp:ListItem Text="" Value=""></asp:ListItem>--%>
                     <asp:ListItem Text="--Select--" Value="" Selected="True" />
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <label for="branchDropdown">
                    <i class="bi bi-bank"></i> Branch</label>
                <asp:DropDownList ID="branchDropdown" CssClass="form-control" style="width: 50%;" runat="server" AutoPostBack="true" OnSelectedIndexChanged="branchDropdown_SelectedIndexChanged">
                    <asp:ListItem Text="" Value=""></asp:ListItem>
                </asp:DropDownList>
                <br />
            </div>
            <div class="form-group">
                <asp:Button ID="btnGenerateReport" runat="server" OnClick="genBankRep_button" Text="Generate Report" CssClass="btn btn-primary" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" CssClass="btn btn-danger" />
            </div>
        </center>
    </div>
            </center>
                </div>
             </div>
</div>
</asp:Content>
