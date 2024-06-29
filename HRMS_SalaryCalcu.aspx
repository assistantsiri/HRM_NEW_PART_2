<%@ Page Language="C#"  Title="Salary Calculation" MasterPageFile="~/HRMS.Master" AutoEventWireup="true" CodeBehind="HRMS_SalaryCalcu.aspx.cs" Inherits="HRMS.HRMS_SalaryCalcu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
    <div class="HRMSSCWrapper">
        
        
        <div class=" HRMSSCRight d-flex flex-column border border-black justify-content-center align-items-center " style="border-radius:30px;">
             <div class="mb-5" style="color:darkcyan">
                <asp:Label runat="server" ID="TitleLabel" CssClass="fs-3 fw-medium" ></asp:Label>
            </div>
            <div class="d-flex w-100"> <asp:Button runat="server" OnClick="StartBtn_Click" Text="Start" CssClass="btn w-50 m-3 btn-success"/>
            <asp:Button runat="server" ID="BackBtn" OnClick="BackBtn_Click" Text="Back" CssClass="btn w-50 m-3 btn-danger"/></div>
        </div>
    </div>
</asp:Content>