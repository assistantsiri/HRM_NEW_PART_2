<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EMPLOYEE_INVESTMENT.aspx.cs" Inherits="HRMS.EMPLOYEE_INVESTMENT"  MasterPageFile="~/HRMS.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
    <center>
        <div class="container mt-5">
            <div class="card" style="width:50%">
                <div class="p-3 mb-2 bg-primary text-white">
                    <center>
                        <div class="card-body">
                           
                            <div class="form-group">
                                <label for="ddlEmpNo">Employee No:</label>
                                <asp:DropDownList ID="ddlEmpNo" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="ddsection">Section:</label>
                                <asp:DropDownList ID="ddsection" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="ddsubsection">Sub Section:</label>
                                <asp:DropDownList ID="ddsubsection" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="txtAmount">Amount:</label>
                                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="txtinvestmentDate">Investment Date:</label>
                                <asp:TextBox ID="txtinvestmentDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="txtRemarks">Remarks:</label>
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <br /><br />
                            <asp:Button ID="btnView" runat="server" Text="VIEW" CssClass="btn btn-dark" OnClick="btnView_Click" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnAdd" runat="server" Text="ADD" CssClass="btn btn-success" OnClick="btnAdd_Click" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnEdit" runat="server" Text="EDIT" CssClass="btn btn-info" OnClick="btnEdit_Click" />
                           <%-- &nbsp;&nbsp;
                            <asp:Button ID="btnDelete" runat="server" Text="DELETE" CssClass="btn btn-danger" OnClick="btnDelete_Click" />--%>
                            &nbsp;&nbsp;
                            <asp:Button ID="btnExit" runat="server" Text="EXIT" CssClass="btn btn-warning" OnClick="btnExit_Click" />
                        </div>
                    </center>
                    <div class="table-responsive">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-custom">
                            <Columns>
                                <asp:BoundField DataField="EMPINV_SLNO" HeaderText="Serial No"  ReadOnly="True" />
                                
                                <asp:BoundField DataField="EMPINV_EMPNO" HeaderText="Employee No" />
                                <asp:BoundField DataField="EMP_NAME" HeaderText="Employee Name" />
                                <asp:BoundField DataField="HRM_DESC" HeaderText="Designation" />
                                <asp:BoundField DataField="EMPINV_SLNO" HeaderText="Serial No" />
                                <asp:BoundField DataField="EMPINV_MCODE" HeaderText="Main Code" />
                                <asp:BoundField DataField="EMPINV_SCODE" HeaderText="Sub Code" />
                                <asp:BoundField DataField="EMPINV_REMARKS" HeaderText="Remarks" />
                                <asp:BoundField DataField="EMPINV_AMOUNT" HeaderText="Amount" />
                                <asp:BoundField DataField="EMPINV_YEAR" HeaderText="Year" />
                            </Columns>
                        </asp:GridView>
                       
                        <br />
                    </div>
                </div>
            </div>
        </div>
    </center>
</asp:Content>
