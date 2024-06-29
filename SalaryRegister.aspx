

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalaryRegister.aspx.cs" Inherits="HRMS.SalaryRegister" MasterPageFile="~/HRMS.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
    <center>
        
            <div class="container" id="form">
                <h2>Salary Register</h2>
                <div class="form-group">
                    <label for="ddlBranch">
                        <i class="bi bi-bank"></i> Branch:
                    </label>
                    <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control" AutoPostBack="true"  OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" ></asp:DropDownList>
                    
                </div>
                <div class="form-group">
                    <label for="txtPayableDate">
                        <i class="bi bi-calendar-month"></i> Payable Date:
                    </label>
                    <asp:TextBox ID="txtPayableDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label for="txtFromEmpNo">
                            <i class="bi bi-person-circle"></i> From No:
                        </label>
                         <asp:DropDownList ID="ddlFromEmpNo" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="form-group col-md-6">
                        <label for="txtToEmpNo">
                            <i class="bi bi-person-circle"></i> To No:
                        </label>
                         <asp:DropDownList ID="ddlToEmpNo" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <label for="txtDescription">
                        <i class="bi bi-file-text-fill"></i> Description:
                    </label>
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="d-flex justify-content-center">
                    <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" CssClass="btn btn-success mr-2" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="btn btn-secondary" />
                </div>
                <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>

                <asp:GridView ID="gvEmployees" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="Emp_Name" HeaderText="Employee Name" />
                    <asp:BoundField DataField="Emp_No" HeaderText="Employee Number" />
                    <asp:BoundField DataField="Branch" HeaderText="Branch" />
                </Columns>
            </asp:GridView>
            </div>
        </form>
    </center>
</asp:Content>



