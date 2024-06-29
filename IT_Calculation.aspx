<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IT_Calculation.aspx.cs" Inherits="HRMS.IT_Calculation" MasterPageFile="~/HRMS.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
    <div class="container mt-5">
        <div class="card" style="width:100%">
            <div class="p-3 mb-2 bg-info text-dark">
                  <div class="card-body" >
                <center>
                    <!-- Page Heading -->
                    <h2>Employee Income Tax Calculation</h2>

                    <!-- Payable Date and Search Box Row -->
                    <div class="row mb-3" >
                        <div class="col-md-6" ">
                            <label for="txtPayableDate">
                                <i class="bi bi-calendar-month"></i> Payable Date:</label>
                            <asp:TextBox ID="txtPayableDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-6 text-right">
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search Emp No..." ></asp:TextBox>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary mt-2" OnClick="btnSearch_Click" />
                        </div>
                    </div>

                    <!-- Employee Tax Details Table -->
                    <div>
                        <table class="table table-bordered">
                            <tbody>
                                <asp:GridView ID="gvEmployeeTaxDetails" runat="server" AutoGenerateColumns="False" CssClass="table" AllowPaging="true" PageSize="10"  OnPageIndexChanging="OnPaging" >
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Calculate
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" Value='<%# Eval("Emp_No") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Emp_No" HeaderText="Emp No" />
                                        <asp:BoundField DataField="Emp_Name" HeaderText="Emp Name" />
                                        <asp:BoundField DataField="Designation" HeaderText="Designation" />
                                        <asp:BoundField DataField="Income Tax" HeaderText="Income Tax" />
                                    </Columns>
                                </asp:GridView>
                            </tbody>
                        </table>
                        <asp:Button ID="btnCheckAll" runat="server" Text="Check All" OnClientClick="checkAllCheckboxes(true); return false;" CssClass="btn btn-primary" />
                       <asp:Button ID="btnCalculate" runat="server" Text="Calculate" OnClick="btnCalculate_Click" CssClass="btn btn-success" />
                       <asp:Button ID="btnView" runat="server" Text="View" OnClick="btnView_Click" CssClass="btn btn-secondary" />
                      <asp:button id="btnprint" runat="server" text="print"  onclick="btn_click"  cssclass="btn btn btn-warning" />
                         

                    </div>
                </center>
            </div>
            </div>
          
        </div>
    </div>

    <script type="text/javascript">
        function checkAllCheckboxes(checkAll) {
            var grid = document.getElementById('<%= gvEmployeeTaxDetails.ClientID %>');
            var checkboxes = grid.getElementsByTagName('input');
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].type == 'checkbox') {
                    checkboxes[i].checked = checkAll;
                }
            }
        }

        function checkCheckboxes() {
            var grid = document.getElementById('<%= gvEmployeeTaxDetails.ClientID %>');
            var checkboxes = grid.getElementsByTagName('input');
            var allChecked = true;

            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].type == 'checkbox' && !checkboxes[i].checked) {
                    allChecked = false;
                    break;
                }
            }

            if (!allChecked) {
                alert('Please check all checkboxes before submitting.');
                return false;
            }

            return true;
        }
    </script>
   <%-- <script type="text/javascript">
        function printDiv() {
            var divToPrint = document.getElementById('<%= gvEmployeeTaxDetails.ClientID %>');
            var newWin = window.open('', 'Print-Window');
            newWin.document.open();
            newWin.document.write('<html><body onload="window.print()">' + divToPrint.outerHTML + '</body></html>');
            newWin.document.close();
            setTimeout(function () { newWin.close(); }, 10);
        }
    </script>--%>
    <style>
        .form-control {
            width: 50%;
        }
    </style>

   
</asp:Content>
