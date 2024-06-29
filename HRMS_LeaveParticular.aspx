<%@ Page Language="C#"  Title="Leave Particular" AutoEventWireup="true" MasterPageFile="~/HRMS.Master" CodeBehind="HRMS_LeaveParticular.aspx.cs" Inherits="HRMS.HRMS_LeaveParticular" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
    <div class="PageContent">
        <asp:Panel runat="server" CssClass="" id="HRMS_LP_Panel1">
            <center>
                
                 <div class="mb-5" style="color:darkcyan">
                <asp:Label runat="server" ID="Label1" CssClass="fs-3 fw-medium" ></asp:Label>
            </div>
            </center>
            
                
            <div id="HRMS_LP_Selection_Container" class="d-flex m-3 mb-1">
                <div id="LP_Selection" class=" d-flex align-items-center flex-fill">
                    <label for ="LP_Emp">Select a Year</label><asp:DropDownList CssClass="mx-2" ID="LP_Year" runat="server"></asp:DropDownList>
                    <label for ="LP_Emp">Select an Employee</label><asp:DropDownList CssClass="mx-2" ID="LP_Emp" runat="server"></asp:DropDownList>
                    <asp:Button runat="server" ID="SubmitBtn" CausesValidation="false" OnClick="SubmitBtn_Click" Text="Submit" CssClass="btn btn-success  " />
                </div>
                <div class="d-flex flex-shrink-1 mx-1">
                         <asp:RadioButtonList  RepeatDirection="Horizontal" AutoPostBack="true"  OnSelectedIndexChanged="LP_rbl_SelectedIndexChanged"  style=" display: inline-block; margin-right: 10px;" ID="LP_rbl" runat="server">
                            
                             <asp:ListItem Selected="true" Text="All" Value="1" />
                             <asp:ListItem Text="In Service" Value="2" />
                           <%-- <asp:ListItem Text="Out of Service" Value="3" />--%>
                            
                           
                        </asp:RadioButtonList>
                     </div>
                <div id="LP_Btn" class=" flex-shrink-1 align-items-center d-flex">
                    <asp:Button runat="server" ID="LP_AddBtn" CausesValidation="false"  OnClick="LP_AddBtn_Click" Text="Add" CssClass="btn btn-primary mx-1" />
                    <asp:Button runat="server" ID="LP_BackBtn" CausesValidation="false" OnClick="LP_BackBtn_Click" Text="Back" CssClass="btn btn-danger" />
                </div> 
            </div>
            <div id="HRMS_LP_Grid" class="m-3">
                <asp:GridView runat="server" ID="LP_Grd" ClientIDMode="Static" DataKeyNames="LvApp_SlNo" AutoGenerateColumns="false" PagerStyle-CssClass="GridPager" PagerStyle-HorizontalAlign="Center"
                    OnPageIndexChanging="LP_Grd_PageIndexChanging" CssClass=" table-responsive table table-bordered border-1 border-black "
                    OnRowCommand="LP_Grd_RowCommand" OnRowDeleting="LP_Grd_RowDeleting" OnRowUpdating="LP_Grd_RowUpdating" >
                    <Columns>
                        <asp:BoundField DataField="LvApp_EmpNo" HeaderText="Emp_No" />
                        <asp:BoundField DataField="Emp_Name" HeaderText="Name" />
                        <asp:BoundField DataField="LvApp_FromDt" HeaderText="From" />
                        <asp:BoundField DataField="LVApp_ToDt" HeaderText="To" />
                        <asp:BoundField DataField="LvApp_Days" HeaderText="No. of Days" />
                        <asp:ButtonField CommandName="Update" Text="Update"    ControlStyle-CssClass="btn btn-secondary" ButtonType="Button" />
                        <asp:ButtonField CommandName="View" Text="View" ControlStyle-CssClass="btn btn-success" ButtonType="Button" />
                        <asp:ButtonField CommandName="Delete" Text="Delete" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" />
                    </Columns>
                </asp:GridView>
                
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="HRMS_LP_Panel2" CssClass="d-flex my-4 justify-content-center">
            <div id="HRMS_LP_ViewPanel" class="border border-black"  style=" width: 80%; padding: 20px; border-radius:30px;">
                <div class="row my-1">
                    <div class="col form-group"><label for="Panel_EmpName">Employee Name</label><asp:TextBox CssClass="form-control" ID="Panel_EmpName" runat="server"></asp:TextBox>
                        
                    </div>
                     <div class="col form-group"><label for ="Panel_EmpNo">Employee Number</label><asp:TextBox CssClass="form-control" ID="Panel_EmpNo"  runat="server"></asp:TextBox></div>
                    
                </div>
                <div class="row mb-1">
                    <div class="form-group"><label for="Panel_LvCode">Leave Code</label><asp:DropDownList CssClass="form-control"  ID="Panel_LvCode" runat="server"></asp:DropDownList></div>
                </div>
                <div class="row mb-1">
                    <div class="col form-group"><label for="Panel_FrmDt">From</label><asp:TextBox  CssClass="form-control" TextMode="Date" runat="server" ID="Panel_FrmDt"></asp:TextBox></div>
                    <div class="col form-group"><label for="Panel_ToDt">To</label><asp:TextBox  CssClass="form-control" TextMode="Date" runat="server" ID="Panel_ToDt"></asp:TextBox></div>
                </div>
                <div class="row mb-1">
                    <div class="col form-group"><label for="Panel_NoD">No. of Days</label><asp:TextBox  CssClass="form-control" ID="Panel_NoD" runat="server"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="rfvPanel_NoD" runat="server" ControlToValidate="Panel_NoD"
                            ErrorMessage="No. Of Days Required" ValidationGroup="Scheme"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revPanel_NoD" ValidationGroup="Scheme" Text="*" ForeColor="Red" Display="Dynamic"
                            ValidationExpression="^(\d+)?$" ControlToValidate="Panel_NoD"
                            runat="server"
                            ErrorMessage="Enter Number of Days"></asp:RegularExpressionValidator>--%>
                    </div>
                    <div class="col form-group"><label for="Panel_AO">Applied On</label><asp:TextBox  CssClass="form-control" TextMode="Date" ID="Panel_AO" runat="server"></asp:TextBox></div>
                </div>
                <div class="row mb-1">
                    <div class="col form-group"><label for="Panel_NoDS">No. of Days Sanctioned</label><asp:TextBox  CssClass="form-control" ID="Panel_NoDS" runat="server"></asp:TextBox></div>
                    <div class="col form-group"><label for="Panel_Sanc">Sanctioned?</label><asp:RadioButtonList  CssClass="form-control" RepeatDirection="Horizontal"  ID="Panel_Sanc" runat="server"><asp:ListItem Value="Y">Yes</asp:ListItem>
                        <asp:ListItem Value="N">No</asp:ListItem>
                        </asp:RadioButtonList></div>
                </div>
                <div class="row"> 
                    <div class="col form-group"><label for="Panel_Reason">Reason</label><asp:TextBox  CssClass="form-control" ID="Panel_Reason" runat="server"></asp:TextBox></div>
                    <div class="col form-group"><label for="Panel_LT">Leave Type</label><asp:RadioButtonList  CssClass="form-control" RepeatDirection="Horizontal" ID="Panel_LT" runat="server"><asp:ListItem Value="P">PL</asp:ListItem><asp:ListItem Value="S">SL</asp:ListItem><asp:ListItem Value=" ">None</asp:ListItem></asp:RadioButtonList></div>
                </div>
                <div class=" d-flex mt-4 justify-content-center">
                    <asp:Button runat="server" ID="LP_BtnSave" CausesValidation="true" OnClick="LP_BtnSave_Click" CssClass="btn btn-success mx-2" Text="Save" />
                    <asp:Button runat="server" ID="LP_BtnCan" CausesValidation="false" OnClick="LP_BtnCan_Click" CssClass="btn btn-danger mx-2" Text="Cancel" />
                     <asp:Button runat="server" ID="LP_GenerateRepo" CausesValidation="true" OnClick="LP_GenerateRepo_Click" CssClass="btn btn-secondary" Text="Generate Report" />
                </div>
            </div>
        </asp:Panel>
        
        
    </div>
    <script type="text/javascript">
        Pagination("LP_Grd","LP_GrdPager")
        function Pagination(TableName, PagerName) {

            var TempTableName = 'table#' + TableName;

            $(TempTableName).each(function () {
                var $table = $(this);
                var itemsPerPage = 10;
                var currentPage = 0;
                var pages = Math.ceil($table.find("tr:not(:has(th))").length / itemsPerPage);
                $table.bind('repaginate', function () {
                    if (pages > 1) {
                        var pager;
                        if ($table.next().hasClass(PagerName))
                            pager = $table.next().empty(); else
                            pager = $('<div class="' + PagerName + ' text-center"></div>');

                        $('<button class="pg-goto btn btn-secondary mr-2 me-2" type="button"></button>').text(' « First ').bind('click', function () {
                            currentPage = 0;
                            $table.trigger('repaginate');
                        }).appendTo(pager);

                        $('<button class="pg-goto btn btn-secondary mr-2 me-2" type="button"> « Prev </button>').bind('click', function () {
                            if (currentPage > 0)
                                currentPage--;
                            $table.trigger('repaginate');
                        }).appendTo(pager);

                        var startPager = currentPage > 2 ? currentPage - 2 : 0;
                        var endPager = startPager > 0 ? currentPage + 3 : 5;
                        if (endPager > pages) {
                            endPager = pages;
                            startPager = pages - 5; if (startPager < 0)
                                startPager = 0;
                        }

                        for (var page = startPager; page < endPager; page++) {
                            $('<span id="pg' + page + '" class="mx-2 ' + (page == currentPage ? 'pg-selected' : 'pg-normal') + '"></span>').text(page + 1 + ' ').bind('click', {
                                newPage: page
                            }, function (event) {
                                currentPage = event.data['newPage'];
                                $table.trigger('repaginate');
                            }).appendTo(pager);
                        }

                        $('<button class="pg-goto btn btn-secondary mr-2 me-2" type="button"> Next » </button>').bind('click', function () {
                            if (currentPage < pages - 1)
                                currentPage++;
                            $table.trigger('repaginate');
                        }).appendTo(pager);
                        $('<button class="pg-goto btn btn-secondary" type="button"> Last » </button>').bind('click', function () {
                            currentPage = pages - 1;
                            $table.trigger('repaginate');
                        }).appendTo(pager);

                        if (!$table.next().hasClass(PagerName))
                            pager.insertAfter($table);
                        //pager.insertBefore($table);

                    }// end $table.bind('repaginate', function () { ...

                    $table.find(
                        'tbody tr:not(:has(th))').hide().slice(currentPage * itemsPerPage, (currentPage + 1) * itemsPerPage).show();
                });

                $table.trigger('repaginate');
            });
        }
    </script>
</asp:Content>


