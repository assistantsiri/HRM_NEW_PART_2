<%@ Page Language="C#" MasterPageFile="~/HRMS.Master" AutoEventWireup="true" CodeBehind="HRMS_SalarySlip.aspx.cs" Inherits="HRMS.HRMS_SalarySlip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
    <div class="HRMSSCWrapper">
        
        <div class =" HRMSSCLeft d-flex justify-content-center align-items-center border border-black bg-dark" >
            
            <asp:Button runat="server" OnClick="StartBtn_Click" Text="Start" CssClass="btn w-50 m-3 btn-light"/>
            <asp:Button runat="server" ID="BackBtn" OnClick="BackBtn_Click" Text="Back" CssClass="btn w-50 m-3 btn-danger"/>
            
        </div>
        <div class="  HRMSSCRight border border-black">

        </div>
    </div>
</asp:Content>
