<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/HRMS.Master" CodeBehind="HRMS_Emp_Investments.aspx.cs" Inherits="HRMS.HRMS_Emp_Investments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
    <div class="PageContent">
        <asp:Panel runat="server" CssClass="" id="HRMS_EmInv_Panel1">
            <div class="">
                <div id="HRMS_EmIn_Selection_Container" class="d-flex m-3 mb-1">
                <div id="EmIn_Selection" class="form-group d-flex align-items-center flex-fill">
                    <label for ="EmIn_Emp">Select an Employee</label><asp:DropDownList CssClass=" me-2 form-control" ID="EmIn_Emp" runat="server"></asp:DropDownList>
                    <asp:Button runat="server" ID="SubmitBtn" OnClick="SubmitBtn_Click" Text="Submit" CssClass="btn btn-success  " />
                </div>
                    <%--<div class="col-3"></div>--%>
                    <%--<div class="col-1"></div>--%>
                <div id="LP_Btn" class="flex-shrink-1 align-items-center d-flex">
                    <asp:Button runat="server" ID="EmIn_AddBtn"  Text="Add" CssClass="btn btn-primary " />
                    <asp:Button runat="server" ID="EmIn_BackBtn" OnClick="EmIn_BackBtn_Click" Text="Back" CssClass="btn btn-danger" />
                </div> 
            </div>
                <div id="HRMS_EmIn_Grid" class="">
                <asp:GridView runat="server" ID="EmIn_Grd" ClientIDMode="Static" DataKeyNames="EmpInv_SlNo" AutoGenerateColumns="false" PagerStyle-CssClass="GridPager" PagerStyle-HorizontalAlign="Center" OnPageIndexChanging="EmIn_Grd_PageIndexChanging"
                   CssClass=" table-responsive table table-bordered border-1 border-black " >
                    <Columns>
                        <asp:BoundField DataField="EmpInv_SlNo" HeaderText="Serial Number" />
                        <asp:BoundField DataField="EmpInv_EmpNo" HeaderText="Emp_No" />
                        <asp:BoundField DataField="Emp_Name" HeaderText="Name" />
                        <asp:BoundField DataField="EmpInv_Year" HeaderText="Year" />
                        <asp:BoundField DataField="EmpInv_Remarks" HeaderText="Decription" />
                        
                        <asp:ButtonField CommandName="Update" Text="Update"    ControlStyle-CssClass="btn btn-secondary" ButtonType="Button" />
                        <asp:ButtonField CommandName="View" Text="View" ControlStyle-CssClass="btn btn-success" ButtonType="Button" />
                        <asp:ButtonField CommandName="Delete" Text="Delete" ControlStyle-CssClass="btn btn-danger" ButtonType="Button" />
                    </Columns>
                </asp:GridView>
                
            </div>
                </div>
        </asp:Panel>
            
        <asp:Panel runat="server" CssClass="" ID="HRMS_EmInv_Panel2">
            <div class="container border border-black p-2 my-4">
            <div class="row mb-4">
                <div class="col-3"></div>
                <div class="col-6">
                    <h4 class="text-center">Employee Investments</h4>
                </div>
                <div class="col-3"></div>
            </div>
            <div class="row my-1">
                <div class="col-3"></div>
                <div class="col-6">
                    <div class="row">
                        <div  class="col-6">
                            <label for="EmIn_EmpNo">Emp No</label>
                        </div>
                        <div class="col-6">
                            <asp:DropDownList runat="server" CssClass="form-control" ID="EmIn_EmpNo"></asp:DropDownList>
                        </div>
                    </div>
                    
                    
                </div>
                <div class="col-3"></div>

            </div>
            <div class="row my-1">
                <div class="col-3"></div>
                <div class="col-6">
                    <div class="row">
                        <div class="col-6">
                            <label for="EmIn_Sec">Section</label>
                        </div>
                        <div class="col-6">
                            <asp:DropDownList runat="server" CssClass="form-control" ID="EmIn_Sec"></asp:DropDownList>
                        </div>
                    </div>
                    
                </div>
                <div class="col-3"></div>
            </div>
            <div class="row my-1">
                <div class="col-3"></div>
                <div class="col-6">
                    <div class="row">
                        <div class="col-6">
                            <label for="EmIn_SubSec">Sub Section</label>
                        </div>
                        <div class="col-6">
                            <asp:DropDownList runat="server" CssClass="form-control" ID="EmIn_SubSec"></asp:DropDownList>
                        </div>
                    </div>
                    
                </div>
                <div class="col-3"></div>
            </div>
            <div class="row my-1">
                <div class="col-3"></div>
                <div class="col-6">
                    <div class="row">
                        <div class="col-6"><label for="EmIn_Amount">Amount</label></div>
                        <div class="col-6"><asp:TextBox  CssClass="form-control" runat="server" ID="EmIn_Amount"></asp:TextBox></div>
                    </div>
                    
                    
                </div>
                <div class="col-3"></div>
            </div>
            <div class="row my-1">
                <div class="col-3"></div>
                <div class="col-6">
                    <div class="row">
                        <div class="col-6"><label for="EmIn_InvDt">Investment Date</label></div>
                        <div class="col-6"><asp:TextBox runat="server"  CssClass="form-control" TextMode="Date" ID="EmIn_InvDt"></asp:TextBox></div>
                    </div>
                    
                   
                </div>
                <div class="col-3"></div>
            </div>
            <div class="row my-1">
                <div class="col-3"></div>
                <div class="col-6">
                    <div class="row">
                        <div class="col-6"><label for="EmIn_Remarks">Remarks</label></div>
                        <div class="col-6"><asp:TextBox runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" ID="EmIn_Remarks"></asp:TextBox></div>
                    </div>
                    
                    
                </div>
                <div class="col-3"></div>
            </div>
        </div>
        </asp:Panel>
        
    </div>
    <script type="text/javascript">
        Pagination("EmIn_Grd","EmIn_GrdPager")
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
