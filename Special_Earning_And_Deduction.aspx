<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Special_Earning_And_Deduction.aspx.cs" Inherits="HRMS.Special_Earning_And_Deduction" MasterPageFile="~/HRMS.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
    <h2>Employees Special Deductions</h2>

    <div>
       <asp:DropDownList ID="ddlEmployee" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged" EnableViewState="true">
</asp:DropDownList>


    </div>
    <br />

    <div>
        <asp:Label ID="lblEarningDeductions" runat="server" Text="Earning/Deductions" AssociatedControlID="ddlEarningDeductions"></asp:Label>
        <asp:DropDownList ID="ddlEarningDeductions" runat="server" AppendDataBoundItems="true">
            <asp:ListItem Text="Please Select Any One" Value="" />
            <asp:ListItem Text="E" Value="E" />
            <asp:ListItem Text="D" Value="D" />
           
        </asp:DropDownList>
    </div>
    <br />

    <div>
        <asp:Label ID="lblEarningDeductionsCode" runat="server" Text="Earning/Deductions Code" AssociatedControlID="txtEarningDeductionsCode"></asp:Label>
        <asp:TextBox ID="txtEarningDeductionsCode" runat="server" Placeholder="Please Enter the value.."></asp:TextBox>
    </div>
    <br />

    <div>
        <asp:Label ID="lblPayableNonPayable" runat="server" Text="Payable/Non Payable" AssociatedControlID="ddlPayableNonPayable"></asp:Label>
        <asp:DropDownList ID="ddlPayableNonPayable" runat="server" AppendDataBoundItems="true">
            <asp:ListItem Text="Please Select Any one" Value="" />
            <asp:ListItem Text="P" Value="P" />
            <asp:ListItem Text="N" Value="N" />
        </asp:DropDownList>
    </div>
    <br />

    <div>
        <asp:Label ID="lblAmount" runat="server" Text="Amount" AssociatedControlID="txtAmount"></asp:Label>
        <asp:TextBox ID="txtAmount" runat="server" Placeholder="Please Enter the value.." TextMode="Number"></asp:TextBox>
    </div>

    <br />
    <div>
    <asp:Label ID="lblPayableDate" runat="server" Text="Payable Date" AssociatedControlID="txtDate"></asp:Label>
    <asp:TextBox ID="txtDate" runat="server" TextMode="Date"></asp:TextBox>
</div>
     <div>
        <asp:Label ID="lblNoOfDays" runat="server" Text="NoOfDays"  AssociatedControlID="txtNoOfDays" ></asp:Label>
        <asp:TextBox ID="txtNoOfDays" runat="server"  TextMode="Number"></asp:TextBox>
    </div>


    <br />

    <div>
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
         &nbsp;&nbsp;
         <asp:Button ID="btnAdd" runat="server" Text="ADD" OnClick="btnAdd_Click" />

    </div>

    <asp:GridView ID="gvEmployeeDetails" runat="server" AutoGenerateColumns="true" EnableViewState="true" CssClass="table table-bordered gridview" AllowPaging="true" PageSize="10"
               
  OnPageIndexChanging="OnPaging">
</asp:GridView>
</asp:Content>
