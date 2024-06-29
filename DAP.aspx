<%@ Page Language="C#" Title="DA POINT" AutoEventWireup="true"  CodeBehind="DAP.aspx.cs" Inherits="HRMS.DAP"   MasterPageFile="~/HRMS.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">

    <script type="text/javascript">
        function toggleAddForm() {
            var formDiv = document.getElementById("addFormDiv");
            formDiv.style.display = formDiv.style.display === "none" ? "block" : "none";
        }
    </script>

    <div class="container mt-4" style="background-color:grey">
        <div class="row">
            <div class="col-12 col-md-8">
                <!-- GridView to display data -->
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered gridview" AllowPaging="true" PageSize="10"
                DataKeyNames="DAP_SlNo"
                OnPageIndexChanging="OnPaging"
                OnRowDataBound="OnRowDataBound"
                OnRowEditing="OnRowEditing"
                OnRowCancelingEdit="OnRowCancelingEdit"
                OnRowUpdating="OnRowUpdating"
              
                EmptyDataText="No records have been added.">
                    <Columns>
                        <asp:TemplateField HeaderText="DAPFROM" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblDAPFROM" runat="server" Text='<%# Eval("DAP_From") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFrom" runat="server" Text='<%# Eval("DAP_From") %>' Width="140"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DAPTO" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblDAPTO" runat="server" Text='<%# Eval("DAP_To") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtTo" runat="server" Text='<%# Eval("DAP_To") %>' Width="140"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DAPoint" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblDAPoint" runat="server" Text='<%# Eval("DAP_Points") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPoint" runat="server" Text='<%# Eval("DAP_Points") %>' Width="140"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DAP%" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblDAPPER" runat="server" Text='<%# Eval("DAP_PER") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPer" runat="server" Text='<%# Eval("DAP_PER") %>' Width="140"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DAPCRDATE" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="lblDAPCRDATE" runat="server" Text='<%# Eval("DAP_CrDate") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCrdate" runat="server" Text='<%# Eval("DAP_CrDate") %>' Width="140"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ButtonType="Link" ShowEditButton="true"  ItemStyle-Width="150" />
                    </Columns>
                </asp:GridView>

                <!-- Button to show the add form -->
                <asp:Button ID="btnShowAddForm" runat="server" Text="Add New DA Point" OnClientClick="toggleAddForm(); return false;" CssClass="btn btn-success mt-2" />
                 &nbsp;&nbsp;
                 <asp:button id="btnBack" runat="server" text="Back" onclick="btncancel_click" CssClass="btn btn-danger" />
            </div>
        </div>
    </div>

    <!-- Form to add new DA points -->
    <div class="container" style="width:70%">
        <div id="addFormDiv" style="display: none;">
            <div class="row justify-content-center" style="background-color:gray">
                <div class="col-md-8">
                    <div class="text">Enter DA Points Details</div>
                    <asp:hiddenfield id="hfdap_slno" runat="server" />
                    <div class="form-row">
                        <div class="form-group col-md-4">
                            <label for="txtdapfrom">From Date</label>
                            <asp:textbox id="txtFrom" runat="server" cssclass="form-control" textmode="date"></asp:textbox>
                        </div>
                        <div class="form-group col-md-4">
                            <label for="txtdapto">To Date</label>
                            <asp:textbox id="txtTo" runat="server" cssclass="form-control" textmode="date"></asp:textbox>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-4">
                            <label for="txtdapper">DA%</label>
                            <asp:textbox id="txtPer" runat="server" cssclass="form-control"></asp:textbox>
                        </div>
                    </div>
                    <div class="row justify-content-center">
                        <div class="col-md-8 text-center">
                            <asp:button id="savebutton" runat="server" text="Save"  CssClass="btn btn-primary"  OnClick="Insert" />
                            <asp:button id="cancelbutton" runat="server" text="Cancel" onclick="btncancel_click" CssClass="btn btn-danger" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <link href="Content/DAP.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.2/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .gridview {
            width: 100%;
            border-collapse: collapse;
        }
        .gridheader {
            background-color: #f2f2f2;
            font-weight: bold;
        }
    </style>
</asp:Content>
