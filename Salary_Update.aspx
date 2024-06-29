<%@ Page Language="C#" AutoEventWireup="true"   MasterPageFile="~/HRMS.Master" CodeBehind="Salary_Update.aspx.cs" Inherits="HRMS.Salary_Update" %>

<%--<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <asp:Button ID="btnStart" runat="server" Text="Start" CssClass="btn btn-primary mr-2" OnClick="btnStart_Click" />
            <asp:Button ID="btnEnd" runat="server" Text="End" CssClass="btn btn-danger" OnClick="btnEnd_Click" />
        </div>
    </form>
</body>
</html>--%>


<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
    
    <div class="HRMSSCWrapper" style="color:cornflowerblue">
        <div class="HRMSSCRight d-flex flex-column border border-black justify-content-center align-items-center" style="border-radius:70px; ">
            <div class="mb-5" style="color:cornflowerblue">
                <asp:Label runat="server" ID="TitleLabel" CssClass="fs-3 fw-medium"></asp:Label>
          <center>
         <h2 style="color:lightsalmon"> Salary Updatation-----</h2>
    </center>
    
            <div >
               
                <asp:Button ID="btnStart" runat="server" Text="Start" CssClass="btn btn-primary mr-2" OnClick="btnStart_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnEnd" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="btnEnd_Click" />
            </div>
        </div>
              </div>
    </div>
</asp:Content>


