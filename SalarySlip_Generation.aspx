<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalarySlip_Generation.aspx.cs" Inherits="HRMS.SalarySlip_Generation" MasterPageFile="~/HRMS.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
     
                
    <div class="container mt-5">
        
         <div class="card" style="width:50%">
            <div class="card-body"  style="background-color:bisque">
        <center>
            
        <div class="container" id="form">
                         <div class="mb-5" style="color:darkcyan">
                <asp:Label runat="server" ID="Label1" CssClass="fs-3 fw-medium" ></asp:Label>
                     </div>    
                    <div class="form-group">
                        <div class="row">
                            <div class="col">
                                <label for="branchDropdown">
                                    <i class="bi bi-bank"></i> Branch</label>
                                <asp:DropDownList ID="branchDropdown" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="branchDropdown_SelectedIndexChanged" style="width: 100%;">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col">
                                <label for="txtPayableDate">
                                    <i class="bi bi-calendar-month"></i> Payable Date:</label>
                                <asp:TextBox ID="txtPayableDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="row">
                            <div class="col">
                                <label for="txtFromEmpNo">
                                    <i class="bi bi-person-circle"></i> From No:</label>
                               <asp:DropDownList ID="ddlFromEmpNo" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="col">
                                <label for="txtToEmpNo">
                                    <i class="bi bi-person-circle"></i> To No:</label>
                               <asp:DropDownList ID="ddlToEmpNo" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="txtDescription">
                            <i class="bi bi-file-text-fill"></i> Description:</label>
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                        <br />
                    </div>
                    <div class="form-group text-center">
                        <asp:Button ID="btnPrint" runat="server" Text="Generate Salary Slip" OnClick="btnPrint_Click" CssClass="btn btn-success mr-2" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="btn btn-danger" />
                    </div>
                

            </div>
   </center>
                </div>
             </div>
      </div>
           
</asp:Content>
