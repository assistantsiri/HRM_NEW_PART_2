<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LEAVE_PROCEEDING_REPORT.aspx.cs" Inherits="HRMS.LEAVE_PROCEEDING_REPORT" MasterPageFile="~/HRMS.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
    <div class="container mt-5">
        <div class="card" style="width:50%">
            <div class="p-3 mb-2 bg-info text-dark">
                <div class="card-body">
                    <div class="form-group">
                        <label for="ddlEmpNo">Employee Number:</label>
                        <asp:DropDownList ID="ddlEmpNo" runat="server" CssClass="form-control">
                        
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label for="txtFromDate">From Date:</label>  <i class="bi bi-calendar-month">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="txtToDate">To Date:</label>  <i class="bi bi-calendar-month">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div><br /><br />
                    <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btn btn-primary" OnClick="btnGenerate_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnCancel_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
