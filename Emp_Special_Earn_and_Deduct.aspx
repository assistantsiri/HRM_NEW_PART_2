<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Emp_Special_Earn_and_Deduct.aspx.cs" Inherits="HRMS.Emp_Special_Earn_and_Deduct"  MasterPageFile="~/HRMS.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">

    <div class="container mt-5">
        <div class="card" style="width:100%" >
            <div class="card-body"  style="background-color:ghostwhite" >
                <center>
                    <!-- Page Heading -->
                    <h2  class="text-primary"> Employee Special Earning  and  Deduction</h2>
                    </center>
                </div>
            </div>
        </div>
     <div class="container mt-5" CssClass="p-3 mb-2 bg-secondary text-white">
        <div class="card" style="width:100%">
            <div class="card-body"  style="background-color:ghostwhite" >
                <center>
                   <div class="container mt-5">
    <div class="card" style="width:50%" >
        <div class="card-body" style="background-color:ghostwhite">
            <center>
                <div class="row mb-3">
                    <div class="col-md-6">
                        <label for="txtEmp">Employee No:</label>
                        <asp:TextBox ID="txtEmp" runat="server" TextMode="Number" CssClass="text-primary"></asp:TextBox>

                    </div>
                </div>
                </center>
            </div>
        </div>
                       <br />
      <div class="card" style="width:50%">
    <div class="card-body" style="background-color:ghostwhite">
        <center>
            <!-- Earning/Deduction Row -->
            <div class="row mb-3">
                <div class="col-md-3">
                    <label for="txtEarningDeduction">Earning/Deduction:</label>
                    <asp:TextBox ID="txtEarningDeduction" runat="server" MaxLength="1" CssClass="text-primary"></asp:TextBox>

                </div>
            </div>

            <!-- Earn/Ded Code Row -->
          <div class="row mb-3">
    <div class="col-md-3">
        <label for="txtEarnDedCode">Earn/Ded Code:</label>
        <asp:TextBox ID="txtEarnDedCode" runat="server" TextMode="Number" CssClass="text-primary"></asp:TextBox>
    
    
        <label for="txtAmount">Amount:</label>
        <asp:TextBox ID="txtAmount" runat="server" TextMode="Number" CssClass="text-primary"></asp:TextBox>

         <label for="txtPaybale">Process Dt:</label>
        <asp:TextBox ID="TextBox1" runat="server" TextMode="Date" CssClass="text-primary"></asp:TextBox>
         <label for="txt">Payable:</label>
                     <asp:TextBox ID="TextBox2" runat="server"  CssClass="text-primary"></asp:TextBox>
    </div>
</div>
                
                  

                  
                      </center>
                     </div>
                    </div>
                  </div>
               </div>


            </div><br />
         <center>

                     <asp:Button ID="btnApply" runat="server" Text="APPLY" CssClass="btn btn-outline-dark ml-2" OnClick="btnApply_Click"  />
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-outline-success" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-outline-secondary ml-2" OnClick="btnCancel_Click" />
                    <asp:Button ID="btnEdit" runat="server" Text="EDIT" CssClass="btn btn-outline-primary" OnClick="btnEdit_Click" />
                   
                    <asp:Button ID="btnAdd" runat="server" Text="ADD" CssClass="btn btn-outline-warning ml-2" OnClick="btnAdd_Click" />
                   <asp:Button ID="btnView" runat="server" Text="VIEW" CssClass="btn btn-outline-info ml-2" OnClick="btnView_Click" /> 
                     <br />
                     <br />
                 <asp:GridView ID="gvSpecialEarningsDeductions" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="false" AllowPaging="true" PageSize="10"  OnRowEditing="OnRowEditing"

                OnRowUpdating="OnRowUpdating"  >
                <Columns>
                        <asp:TemplateField HeaderText="E/D" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblSpED_Ind" runat="server" Text='<%# Eval("SpED_Ind") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSpED_Ind" runat="server" Text='<%# Eval("SpED_Ind") %>' Width="140"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CodeDesc" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblCodeDesc" runat="server" Text='<%# Eval("CodeDesc") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCodeDesc" runat="server" Text='<%# Eval("CodeDesc") %>' Width="140"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblSpED_Amt" runat="server" Text='<%# Eval("SpED_Amt") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSpED_Amt" runat="server" Text='<%# Eval("SpED_Amt") %>' Width="140"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Payable" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblSpED_Payable" runat="server" Text='<%# Eval("SpED_Payable") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSpED_Payable" runat="server" Text='<%# Eval("SpED_Payable") %>' Width="140"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SpED_ProcessDt" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblSpED_ProcessDt" runat="server" Text='<%# Eval("SpED_ProcessDt") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtSpED_ProcessDt" runat="server" Text='<%# Eval("SpED_ProcessDt") %>' Width="140"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                         <asp:CommandField ButtonType="Link" ShowEditButton="true"  ItemStyle-Width="150" />
                    </Columns>
</asp:GridView>
<asp:Label ID="Label1" runat="server" CssClass="text-danger"></asp:Label>
              </center>
         </div>
   <div class="container mt-5">
        <div class="card" style="width:100%">
            <div class="card-body" style="background-color:ghostwhite">
                <center>
                    <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
                </center>
            </div>
        </div>
    </div>
    <%--<div class="row mb-3">
            <div class="col-md-12">
                <asp:GridView ID="gvSpecialEarningsDeductions" runat="server" CssClass="table table-bordered table-striped">
                    <Columns>
                        <asp:BoundField DataField="SpEDInd" HeaderText="Index" />
                        <asp:BoundField DataField="CodeDesc" HeaderText="Code Description" />
                        <asp:BoundField DataField="SpEDAmt" HeaderText="Amount" DataFormatString="{0:C}" />
                        <asp:BoundField DataField="SpEDPayable" HeaderText="Payable" DataFormatString="{0:C}" />
                        <asp:BoundField DataField="SpEDNoDays" HeaderText="Number of Days" />
                        <asp:BoundField DataField="SpEDProcessDt" HeaderText="Process Date" DataFormatString="{0:yyyy-MM-dd}" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>--%>
    <script>
    function showMessage(message) {
        alert(message);
    }
    </script>

</asp:Content>