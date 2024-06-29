<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalaryRegister_Employee.aspx.cs" Inherits="HRMS.SalaryRegister_Employee"  MasterPageFile="~/HRMS.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
    <div class="container mt-5">
         <div class="card" style="width:50%">
            <div class="card-body"  style="background-color:gray">
        <center>
          
        <div class="container form-group" id="form">
            <h2>Salary Register-Employee</h2>
            <div class="form-group">
                <label for="empcode">
                    <i class="bi bi-person-circle"></i> EmpNo:</label>
                <%--<asp:TextBox ID="txtempNo" runat="server" CssClass="form-control" Height="50px" Width="100px"></asp:TextBox>--%>
                <label for="ddlEmployee">Employee</label>
                 <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <label for="fromDate">
                <i class="bi bi-calendar-month"></i> From Date:</label>
            <asp:TextBox ID="fromDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
            <label for="toDate">
                <i class="bi bi-calendar-month"></i> To Date:</label>
            <asp:TextBox ID="toDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
            <br />
            <div class="d-flex justify-content-center">
                <asp:Button ID="btnGenRep" runat="server" Text="TEXT_REPO" OnClick="btnGenReport_Click" CssClass="btn btn-success mr-2" />
                 &nbsp;&nbsp;&nbsp;&nbsp;
                 <asp:Button ID="btnPdfRepo" runat="server" Text="PDF_REPO" OnClick="btnGenReport_Click_Pdf" CssClass="btn btn-success mr-2" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="btn btn-danger" />
            </div>
        </div>
    </center>
                </div>
             </div>
        </div>
</asp:Content>
