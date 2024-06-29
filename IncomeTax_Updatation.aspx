<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IncomeTax_Updatation.aspx.cs" Inherits="HRMS.IncomeTax_Updatation" MasterPageFile="~/HRMS.Master" %>


   <asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
         <div class="container mt-4 p-3 mb-2 bg-info text-dark">
        <div class="row">
            <div class="col-12 col-md-8">
                <!-- GridView to display data -->
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered gridview" AllowPaging="true" PageSize="10"
               
                OnPageIndexChanging="OnPaging"
               
                OnRowEditing="OnRowEditing"
                OnRowCancelingEdit="OnRowCancelingEdit"
                OnRowUpdating="OnRowUpdating"
                
              
                EmptyDataText="No records have been added.">
                       <Columns>
                        <asp:TemplateField HeaderText="Emp No" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblEmpNo" runat="server" Text='<%# Eval("Calc_IT_EmpNo") %>'></asp:Label>
                            </ItemTemplate>
                            <%--<EditItemTemplate>
                                <asp:TextBox ID="txtEmpNO" runat="server" Text='<%# Eval("Calc_IT_EmpNo") %>' Width="140"></asp:TextBox>
                            </EditItemTemplate>--%>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Emp Name" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("Emp_Name") %>'></asp:Label>
                            </ItemTemplate>
                            <%--<EditItemTemplate>
                                <asp:TextBox ID="txtEmpName" runat="server" Text='<%# Eval("Emp_Name") %>' Width="140"></asp:TextBox>
                            </EditItemTemplate>--%>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IT Amount " ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblITAmount" runat="server" Text='<%# Eval("Calc_IT_Amount") %>'></asp:Label>
                            </ItemTemplate>
                           <%-- <EditItemTemplate>
                                <asp:TextBox ID="txtITAmount" runat="server" Text='<%# Eval("Calc_IT_Amount") %>' Width="140"></asp:TextBox>
                            </EditItemTemplate>--%>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Existing Mthly Deduction" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblExistingMthDeduction" runat="server" Text='<%# Eval("EmpEDAmt") %>'></asp:Label>
                            </ItemTemplate>
                           <%-- <EditItemTemplate>
                                <asp:TextBox ID="txtExistingMthDeduction" runat="server" Text='<%# Eval("EmpEDAmt") %>' Width="140"></asp:TextBox>
                            </EditItemTemplate>--%>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mthly Deduction" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblMthDeduction" runat="server" Text='<%# Eval("MthAmt") %>'></asp:Label>
                            </ItemTemplate>
                            <%--<EditItemTemplate>
                                <asp:TextBox ID="txtMthDeduction" runat="server" Text='<%# Eval("MthAmt") %>' Width="140"></asp:TextBox>
                            </EditItemTemplate>--%>
                        </asp:TemplateField>
                           <asp:TemplateField HeaderText="Existing current Month Deduction" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblExCuMthDed" runat="server" Text='<%# Eval("SpED") %>'></asp:Label>
                            </ItemTemplate>
                           <%-- <EditItemTemplate>
                                <asp:TextBox ID="txtExCuMthDed" runat="server" Text='<%# Eval("SpED") %>' Width="140"></asp:TextBox>
                            </EditItemTemplate>--%>
                        </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mthly Deduction" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblMthDed" runat="server" Text='<%# Eval("SpED") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtMthDed" runat="server" Text='<%# Eval("SpED") %>' Width="140"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ButtonType="Link" ShowEditButton="true"  ItemStyle-Width="150" />
                    </Columns>
                   
                </asp:GridView>

                 <asp:button id="btnBack" runat="server" text="Back" onclick="btncancel_click" CssClass="btn btn-danger" />
            </div>
        </div>
    </div>

    </asp:Content>

