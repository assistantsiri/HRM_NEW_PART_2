<%@ Page Language="C#"  Title="Email Salary Slip" MasterPageFile="~/HRMS.Master" AutoEventWireup="true" CodeBehind="HRMS_SalarySlipEmail.aspx.cs" Inherits="HRMS.HRMS_SalarySlipEmail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
    
    <div class="PageContent">
        <div id="HRMS_SSEm_Header_Container" class="d-flex m-3 mb-1">
                <div id="SSEm_Selection" class=" d-flex align-items-center flex-fill">
                    <label for ="SSEm_Branch">Select a Branch</label><asp:DropDownList CssClass="mx-2" ID="SSEm_Branch" runat="server"></asp:DropDownList>
                    <asp:Button runat="server" ID="SubmitBtn" OnClick="SubmitBtn_Click" Text="Submit" CssClass="btn btn-success  " />
                </div>
                <div id="SSEm_Btn" class=" flex-shrink-1 align-items-center d-flex">
                    <asp:Button runat="server" ID="SSem_GenerateBtn"  OnClick="SSem_GenerateBtn_Click" Text="Generate Email" CssClass="btn btn-primary mx-1" />
                    <asp:Button runat="server" ID="SSEm_BackBtn" OnClick="SSEm_BackBtn_Click" Text="Back" CssClass="btn btn-danger" />
                </div> 
            </div>
        <div id="HRMS_SSEm_Body" class="w-100 m-3 mb-2 d-flex flex-column" style="height:150px;">
            <asp:label runat="server" id="SSEm_Body_Label" for ="SSEm_Body">Description</asp:label>
            <asp:TextBox ID="SSEm_Body" TextMode="MultiLine" Rows="3" runat="server" CssClass="w-75" style="height:70px;" placeholder="Please Make Sure to Select Employees for whom Files Exists! No mail is sent, if not present."></asp:TextBox>

        </div>
        <div id="HRMS_SSEm_Grid" class="m-3" style="max-height: 550px; overflow-y: auto;">
                <asp:GridView runat="server" ID="SSEm_Grid" AllowPaging="false" DataKeyNames="Emp_No,Emp_Name,Emp_EMailID" AutoGenerateColumns="false"
                     CssClass=" table-responsive table table-bordered border-1 border-black"  >
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox runat="server" ID="chkAll"  onclick="SelectAll(this)"  />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chk" onclick="SelectOne(this)" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Emp_No" HeaderText="Emp_No" />
                        <asp:BoundField DataField="Emp_Name" HeaderText="Name" />
                        <asp:BoundField DataField="Emp_EMailID" HeaderText="Email ID" />
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label runat="server" Text="File Exists/Not"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chkStatus" style="display:none;"  />
                                <asp:Label runat="server" ID="lblCheckboxText" AssociatedControlID="chkStatus"></asp:Label>
                             </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                
            </div>
        
    </div>
   

    <script type="text/javascript">
        function SelectAll(headerCheckBox) {
            //Get the reference of GridView.
            var GridView = headerCheckBox.parentNode.parentNode.parentNode;

            //Loop through all GridView Rows except first row.
            for (var i = 1; i < GridView.rows.length; i++) {
                //Reference the CheckBox.
                var checkBox = GridView.rows[i].cells[0].getElementsByTagName("input")[0];

                //If CheckBox is checked, change background color the GridView Row.
                if (headerCheckBox.checked) {
                    checkBox.checked = true;
                    GridView.rows[i].className = "selected";
                } else {
                    checkBox.checked = false;
                    GridView.rows[i].className = "";
                }
            }
        }
        function SelectOne(chk) {
            //Reference the GridView Row.
            var row = chk.parentNode.parentNode;

            //Get the reference of GridView.
            var GridView = row.parentNode;

            //Reference the Header CheckBox.
            var headerCheckBox = GridView.rows[0].cells[0].getElementsByTagName("input")[0];

            //If CheckBox is checked, change background color the GridView Row.
            if (chk.checked) {
                row.className = "selected";
            } else {
                row.className = "";
            }

            var checked = true;

            //Loop through all GridView Rows.
            for (var i = 1; i < GridView.rows.length; i++) {
                //Reference the CheckBox.
                var checkBox = GridView.rows[i].cells[0].getElementsByTagName("input")[0];
                if (!checkBox.checked) {
                    checked = false;
                    break;
                }
            }

            headerCheckBox.checked = checked;
        };

        document.addEventListener('DOMContentLoaded', function () {
            const myModal = document.getElementById('myModal');
            const myInput = document.getElementById('myInput');

            myModal.addEventListener('shown.bs.modal', function () {
                myInput.focus();
            });
        });
    </script>
   
</asp:Content>
