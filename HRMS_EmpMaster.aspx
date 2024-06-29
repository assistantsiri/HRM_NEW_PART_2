<%@ Page Language="C#" Title="Employee Master" AutoEventWireup="true" MasterPageFile="~/HRMS.Master" CodeBehind="HRMS_EmpMaster.aspx.cs" Inherits="HRMS.HRMS_EmpMaster" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
    <script type="text/javascript">
       
    </script>
   
     <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
         <asp:View ID="view1" runat="server">
             <div id="EmpHeader" class="  m-3" >
                 <center>
                     
                 <div class="mb-5" style="color:darkcyan">
                <asp:Label runat="server" ID="TitleLabel" CssClass="fs-3 fw-medium" ></asp:Label>
            </div>
                 </center>
           
               
                
                <asp:Button runat="server" Text="Add" OnClick="AddBtn_Click" CssClass=" btn btn-primary ms-2" />
            <asp:Button runat="server" OnClick="DelBtn_Click" Text="Disable" CssClass=" btn btn-danger " />
                <asp:Button runat="server" ID="BackBtn" OnClick="BackBtn_Click" Text="Back" CssClass=" btn btn-secondary" />
              
                <div id="searchbar" class=" d-flex float-end">
            <%--<i for="grdInput" class="fas fa-search justify-content-center"></i>--%>
            <input id="grdInput" placeholder="Search Emp Name/No." class="form-control mx-2" onkeyup="SearchTable('grdUser','grdInput',1)" type="text"/>
            <%--<asp:TextBox ID="EmpSearch" runat="server" placeholder="Employee Name"></asp:TextBox>
            <asp:Button runat="server" Text="Search" OnClick="EmpMasSearchBtn_Click" CssClass="btn btn-secondary ms-2" />--%>
           <%-- <asp:Button runat="server" ID="EmpMast_BtnReset" OnClick="EmpMast_BtnReset_Click" Text="Reset" CssClass="btn btn-danger mx-2" />--%>
        </div>
            </div>
                 <div class="">
                     <div class="d-flex float-end mx-1">
                         <asp:RadioButtonList  RepeatDirection="Horizontal" AutoPostBack="true"  OnSelectedIndexChanged="rbl_SelectedIndexChanged" style=" display: inline-block; margin-right: 10px;" ID="rbl" runat="server">
                            <asp:ListItem Text="In Service" Value="1" />
                            <asp:ListItem Text="Out of Service" Value="2" />
                            <asp:ListItem Selected="true" Text="All" Value="3" />
                           
                        </asp:RadioButtonList>
                     </div>
                 </div>
        <div class=" justify-content-center mx-2 mt-2">
            <asp:GridView ID="grdUser" DataKeyNames="Emp_No" ClientIDMode="Static" PagerStyle-CssClass="GridPager" OnRowUpdating="grdUser_RowUpdating" PagerStyle-HorizontalAlign="Center" 
                runat="server" CssClass=" HRMSEmptable table-hover  table-responsive table table-bordered border-1 border-black " 
                OnPageIndexChanging="grdUser_PageIndexChanging" AutoGenerateColumns="false"
                CellSpacing="15" Width="100%" OnRowCommand="grdUser_RowCommand" OnSelectedIndexChanged="grdUser_SelectedIndexChanged" >
                <Columns>
                    
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="chk" />
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Serial No.">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="Serial_No" Text="<%# Container.DataItemIndex + 1 %>"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:CheckBoxField Visible="true" HeaderText="Select"/>--%>
                    <asp:BoundField DataField="Emp_No" HeaderText="Emp_No" />
                    <asp:BoundField DataField="Emp_Name" HeaderText="Emp_Name"/>
                    <asp:BoundField HeaderText="Designation" DataField="Designation" />
                    <%--<asp:BoundField HeaderText="Qualification" DataField="Emp_Grade" />--%>
                    <asp:BoundField HeaderText="Date of Birth" DataFormatString="{0:dd-M-yyyy}"  DataField="Emp_DOB" />
                    <asp:BoundField HeaderText="Date of Joining" DataFormatString="{0:dd-M-yyyy}" DataField="Emp_JoinDt"/>
                    <asp:BoundField HeaderText="Department" DataField="Department" />
                    <asp:BoundField DataField="Emp_Status" HeaderText="Status" />
                    <asp:ButtonField CommandName="Update" Text="Update"    ControlStyle-CssClass="btn btn-secondary" ButtonType="Button" />
                    <asp:ButtonField CommandName="View" Text="View" ControlStyle-CssClass="btn btn-success" ButtonType="Button" />
                </Columns>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
                <RowStyle CssClass="table-light  border-black" />
                <AlternatingRowStyle CssClass="table-info border-black" />
                <HeaderStyle CssClass="" />
                <%--<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last"  />--%>
            </asp:GridView>
            
        </div>
        </div>
         </asp:View>
         <asp:View ID="view2" runat="server">
             <div id="EmpMastContainer" class="  d-flex my-4  justify-content-center">
            <div id="EmpMastFields" class=" overflow-y-auto border border-black Popup card">
            <div class="EmpMastTabbed card-header">
                <ul class="nav nav-tabs nav-fill card-header-tabs" id="myTab" role="tablist">
                    <li class="nav-item">
                        <a class="nav-link active" id="tab1-tab" data-toggle="tab" href="#DD" role="tab" aria-controls="tab1" aria-selected="true"><i class="fas fa-info-circle"></i>Designation Details</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" id="tab2-tab" data-toggle="tab" href="#PD" role="tab" aria-controls="tab2" aria-selected="false"><i class="fas fa-user-astronaut"></i>Personal Details</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" id="tab3-tab" data-toggle="tab" href="#RD" role="tab" aria-controls="tab3" aria-selected="false"><i class="fas fa-home"></i>Residential Details</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" id="tab4-tab" data-toggle="tab" href="#ED" role="tab" aria-controls="tab4" aria-selected="false"><i class="fas fa-rupee-sign"></i>Earnings and Deductions</a>
                    </li>
                </ul>
                <%--<asp:LinkButton runat="server"  CssClass="Anchor nav-item nav-link active" href="#DD" datatoggle="tab" role="tab"><i class="fas fa-info-circle"></i>Designation Details</asp:LinkButton>
                <asp:LinkButton runat="server" CssClass="Anchor nav-item nav-link" datatoggle="tab" href="#PD" role="tab"><i class="fas fa-user-astronaut"></i>Personal Details</asp:LinkButton>
                <asp:LinkButton runat="server" CssClass="Anchor nav-item nav-link" datatoggle="tab" href="#RD" role="tab"><i class="fas fa-home"></i>Residential Details</asp:LinkButton>
                <asp:LinkButton runat="server" CssClass="Anchor nav-item nav-link" datatoggle="tab" href="#ED" role="tab"><i class="fas fa-dollar-sign"></i>Earnings and Deduction</asp:LinkButton>--%>
            </div>
            <div class="EmpMastTabs  card-body tab-content">
                <div id="DD" class=" tab-pane active" role="tabpanel">
                    <div class=" ">
                        <div class="row py-2">
                            <div class=" col form-group">
                                <label for="">Employee Name</label><asp:TextBox CssClass="form-control" ID="Emp_Name" runat="server"></asp:TextBox>

                            </div>
                            <div class="col form-group"><label for="Emp_FatherName">Father Name</label><asp:TextBox CssClass="form-control" ID="Emp_FatherName" runat="server"></asp:TextBox></div>
                            <div class="col form-group"><label for="Emp_SpouseName">Spouse Name</label><asp:TextBox CssClass="form-control" ID="Emp_SpouseName" runat="server"></asp:TextBox></div>
                            
                        </div>

                        <div class="row py-2">
                            <div class="col form-group">
                                <label for="Emp_Gen">Gender</label><asp:RadioButtonList CssClass="form-control" RepeatDirection="Horizontal" ID="Emp_Gen" runat="server"><asp:ListItem Value="M">Male</asp:ListItem>
                                    <asp:ListItem Value="F">Female</asp:ListItem>
                                                                   </asp:RadioButtonList>
                            </div>
                            <div class="col form-group"><label for="Emp_MS">Marital Status</label><asp:RadioButtonList CssClass="form-control" RepeatDirection="Horizontal" ID="Emp_MS" runat="server">
                                <asp:ListItem Value="M">Married</asp:ListItem><asp:ListItem Value="S">Single</asp:ListItem>
                                                                                       </asp:RadioButtonList></div>
                            <div class="col form-group" ><label for="Emp_Qual">Qualification</label><asp:TextBox CssClass="form-control" ID="Emp_Qual" runat="server"></asp:TextBox></div>
                        </div>
                        <div class="row py-2">
                             
                            <div class="col form-group"><label for="Emp_Desig">Designation</label><asp:DropDownList CssClass="form-control" ID="Emp_Desig" runat="server"></asp:DropDownList></div>
                             
                            <div class="col form-group"><label for="Emp_Dept">Department</label><asp:DropDownList CssClass="form-control" ID="Emp_Dept" runat="server"></asp:DropDownList></div>
                        </div>
                        <div class="row py-2">
                            <div class="col form-group"><label for="Emp_Grade">Grade</label><asp:DropDownList CssClass="form-control" ID="Emp_Grade" runat="server"></asp:DropDownList></div>
                           <div class="col form-group" ><label for="Emp_Branch">Branch</label><asp:DropDownList CssClass="form-control" ID="Emp_Branch" runat="server"></asp:DropDownList></div>
                            <div class="col form-group"><label for="Emp_State">State</label><asp:DropDownList CssClass="form-control" ID="Emp_State" runat="server"></asp:DropDownList></div>
                            
                        </div>
                        <div class="row py-2">
                            <div class="col  form-group"><label for="Emp_DOB">Date of Birth</label><asp:TextBox  CssClass="form-control" ID="Emp_DOB" TextMode="Date" runat="server"></asp:TextBox></div>
                            <div class="col  form-group"><label for="Emp_DOJ">Date of Joining</label><asp:TextBox  CssClass="form-control" ID="Emp_DOJ" TextMode="Date" runat="server"></asp:TextBox></div>
                           
                        </div>
                        <div class="row py-2">
                            <div class="col  form-group"><label for="Emp_DOC">Date of Confirmation</label><asp:Textbox  CssClass="form-control" TextMode="Date" ID="Emp_DOC" runat="server"></asp:Textbox></div>
                            <div class="col  form-group"><label for="Emp_DOI">Date of Increment</label><asp:Textbox  CssClass="form-control" TextMode="Date" ID="Emp_DOI" runat="server"></asp:Textbox></div>
                            <div class="col  form-group"><label for="Emp_DOP">Date of Promotion</label><asp:Textbox  CssClass="form-control" TextMode="Date" ID="Emp_DOP" runat="server"></asp:Textbox></div>
                            <%--<div class="col"> <label for="Emp_ESIA">ESI Applicable</label><asp:DropDownList ID="Emp_ESIA" runat="server"></asp:DropDownList></div>--%>
                        </div>
                        
 
                    </div>
                </div>
                <div id="PD" class=" tab-pane" role="tabpanel">
                    <div class=" ">
                        <div class="row py-2">
                            <div class="col  form-group"><label for="Emp_PFNom">PF Nominee</label><asp:TextBox  CssClass="form-control" ID="Emp_PFNom" runat="server"></asp:TextBox></div>
                            <div class="col  form-group"><label for="Emp_PFNo">PF No.</label><asp:TextBox  CssClass="form-control" ID="Emp_PFNo" runat="server"></asp:TextBox></div>
                            <div class="col  form-group"><label for="Emp_EPSNo">EPS No.</label><asp:TextBox  CssClass="form-control" ID="Emp_EPSNo" runat="server"></asp:TextBox></div>
                        </div>
                        <div class="row py-2">
                            <div class="col form-group"><label for="Emp_PAN">PAN/GIR No.</label><asp:TextBox CssClass="form-control" ID="Emp_PAN" runat="server"></asp:TextBox></div>
                             <div class="col form-group"><label for="Emp_Aadhar">Aadhar No.</label><asp:TextBox CssClass="form-control" ID="Emp_Aadhar" runat="server"></asp:TextBox>
                               <%--  <asp:RegularExpressionValidator ID="rgxAadhaar" runat="server" ControlToValidate="Emp_Aadhar" Enabled="true"
                                    ValidationExpression="([0-9]{4}[0-9]{4}[0-9]{4}$)|([0-9]{4}\s[0-9]{4}\s[0-9]{4}$)|([0-9]{4}-[0-9]{4}-[0-9]{4}$)"
                                    ErrorMessage="Invalid Aadhaar Number" Text="*" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                             </div>
                            <div class="col  form-group"><label for="Emp_UAN">UAN No.</label><asp:TextBox  CssClass="form-control" ID="Emp_UAN" runat="server"></asp:TextBox></div>
                        </div>
                        <div class="row py-2">
                            <div class="col  form-group"><label for="Emp_CAdd1">Address</label><asp:TextBox CssClass="form-control" ID="Emp_CAdd1" runat="server"></asp:TextBox><asp:TextBox CssClass="form-control my-1" ID="Emp_CAdd2" runat="server"></asp:TextBox><asp:TextBox CssClass="form-control" ID="Emp_CAdd3" runat="server"></asp:TextBox></div>
                        </div>
                        <div class ="row py-2">
                             <div class="col  form-group"><label for="Emp_CCity">City</label><asp:TextBox CssClass="form-control" ID="Emp_CCity" runat="server"></asp:TextBox></div>
                            <div class="col  form-group"><label for="Emp_CState">State</label><asp:DropDownList CssClass="form-control" ID="Emp_CState" runat="server"></asp:DropDownList></div>
                            <div class="col  form-group"><label for="Emp_CPin">Pin</label><asp:TextBox CssClass="form-control" ID="Emp_CPin" runat="server"></asp:TextBox></div>
                        </div>
                        <div class="row py-2">
                            <div class="col  form-group"><label for="Emp_SCBank">Salary Credited Bank</label><asp:DropDownList  CssClass="form-control" ID="Emp_SCBank" runat="server"></asp:DropDownList></div>
                            <div class="col  form-group"><label for="Emp_SCBranch">Salary Credited Branch</label><asp:DropDownList  CssClass="form-control" ID="Emp_SCBranch" runat="server"></asp:DropDownList></div>
                            <div class="col  form-group"><label for="Emp_ACNo">A/C No.</label><asp:TextBox  CssClass="form-control" ID="Emp_ACNo" runat="server"></asp:TextBox></div>
                            
                        </div>
                        <div class="row py-2">
                            <div class="col form-group"><label for="Emp_OT">OverTime</label><asp:RadioButtonList  CssClass="form-control" RepeatDirection="Horizontal" ID="Emp_OT" runat="server">
                                <asp:ListItem Value="Y">Yes</asp:ListItem><asp:ListItem Value="N">No</asp:ListItem>
                                                                                 </asp:RadioButtonList></div>
                          
                            <div class="col form-group"><label for="Emp_SP">Stop Payment</label><asp:RadioButtonList  CssClass="form-control" RepeatDirection="Horizontal" ID="Emp_SP" runat="server">
                                <asp:ListItem Value="Y">Yes</asp:ListItem><asp:ListItem  Value="N">No</asp:ListItem>
                                                                                     </asp:RadioButtonList></div>
                            
                            <div class="col form-group"><label for="Emp_Depu">Deputation</label><asp:RadioButtonList  CssClass="form-control"  RepeatDirection="Horizontal" ID="Emp_Depu" runat="server">
                                <asp:ListItem Value="Y">Yes</asp:ListItem><asp:ListItem  Value="N">No</asp:ListItem>
                                                                                     </asp:RadioButtonList></div>
                        </div>
                        <div class="row py-2">
                            <div class="col form-group"><label for="Emp_AG">Accomodation Given</label><asp:RadioButtonList  CssClass="form-control"  RepeatDirection="Horizontal" ID="Emp_AG" runat="server">
                                 <asp:ListItem Value="Y">Yes</asp:ListItem><asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                                                                                           </asp:RadioButtonList></div>
                            <div class="col form-group"><label for="Emp_HRP">House Rent Paid</label><asp:TextBox  CssClass="form-control"  ID="Emp_HRP" runat="server" ></asp:TextBox></div>
                        </div>
                        <div class="row py-2">
                             <div class="col form-group"><label for="Emp_CC">City Class</label><asp:DropDownList  CssClass="form-control"  ID="Emp_CC" runat="server"></asp:DropDownList></div>
                            <div class="col form-group"><label for="Emp_Metro">Metro</label><asp:RadioButtonList  CssClass="form-control"  RepeatDirection="Horizontal" ID="Emp_Metro" runat="server">
                                <asp:ListItem Value="Y">Metro</asp:ListItem><asp:ListItem Value="N">Non Metro</asp:ListItem>
                                                                                 </asp:RadioButtonList></div>
                        </diV>
                        <div class="row py-2">
                            <div class="col form-group"><label for="Emp_LTC">LTC Availed</label><asp:TextBox  CssClass="form-control"  ID="Emp_LTC" runat="server"></asp:TextBox></div>
                             <div class="col form-group"><label for="Emp_VPF">VPF Amount</label><asp:TextBox  CssClass="form-control" ID="Emp_VPF" runat="server"></asp:TextBox></div>
                             <div class="col form-group"><label for="Emp_Vehi">Vehicle</label><asp:DropDownList  CssClass="form-control"  ID="Emp_Vehi" runat="server"></asp:DropDownList></div>
                            
                            <%--<div class="col"><label for="Emp_Status">Status</label><asp:DropDownList ID="Emp_Status" runat="server"></asp:DropDownList></div>--%>
                            
                        </div>
                        <%--<div class="row py-3">
                            
                            <div class="col"><label for="Emp_DOR">Date of Resignation</label><asp:TextBox ID="Emp_DOR" runat="server"></asp:TextBox></div>
                            <div class="col"><label for="Emp_ROR">Reason for Resign</label><asp:DropDownList ID="Emp_ROR" runat="server"></asp:DropDownList></div>
                        </div>--%>

                    </div>
                </div>
                <div id="RD"  class="tab-pane" role="tabpanel">
                    <div class=" d-flex justify-content-center">
                        <div>
                            <div class="row  py-2"><asp:Button OnClick="Cpy_PAdd_Click" ID="Cpy_PAdd" CssClass="btn btn-light border-black border" runat="server" Text="Copy the Permanent Address" /></div>
                            <div class="row form-group  py-2"><label for="Emp_PAdd">Address</label><asp:TextBox ID="Emp_PAdd"  CssClass="form-control"  runat="server"></asp:TextBox> <asp:TextBox  CssClass="form-control my-1"  ID="Emp_PAdd1" runat="server"></asp:TextBox> <asp:TextBox  CssClass="form-control" ID="Emp_PAdd2" runat="server"></asp:TextBox> </div>
                            <div class="row  form-group py-2"><label for="Emp_PCity">City</label><asp:TextBox ID="Emp_PCity"  CssClass="form-control"  runat="server"></asp:TextBox></div>
                            <div class="row form-group py-2"> <label for="Emp_PState">State</label><asp:DropDownList ID="Emp_PState"  CssClass="form-control"  runat="server"></asp:DropDownList></div>
                            <div class="row form-group py-2"><label for="Emp_PPin">Pin</label><asp:TextBox ID="Emp_PPin"  CssClass="form-control"  runat="server"></asp:TextBox></div>
                            <div class="row form-group py-2"><label for="Emp_Email">Email Id</label><asp:TextBox ID="Emp_Email"  CssClass="form-control"  runat="server"></asp:TextBox></div>
                        </div>
                        
                    
                   
                    
                  
                   
                   
                    
                  
                    
                  
                    </div>
                </div>
                <div id="ED" class="tab-pane" role="tabpanel">
                    <div>
                        <%--<asp:Label ID="" runat="server"></asp:Label>--%>
                        <asp:GridView ID="EDGrd" runat="server" AutoGenerateColumns="false" CssClass=" table-hover  table-responsive table table-bordered border-1 border-black">
                        <Columns>
                            
                            <asp:BoundField HeaderText="Code" DataField="ED_Code" />
                            <asp:BoundField HeaderText="Description" DataField="ED_Desc"/>
                            <asp:BoundField HeaderText="Amount" DataField="ED_Amount" />
                            
                    
                        </Columns>
                    </asp:GridView>
                    </div>
                </div>

        </div>
                <div class=" d-flex justify-content-center">
                    <asp:Button runat="server" ID="BtnSave" OnClick="BtnSave_Click" CausesValidation="true" CssClass="btn btn-success mx-2" Text="Save" />
                    <asp:Button runat="server" OnClick="HRMSEmpCan_Click" CssClass="btn btn-danger mx-2" Text="Cancel" />
                </div>

                </div>
            </div>
         </asp:View>
     </asp:MultiView>
    <script type="text/javascript">

        Pagination("grdUser","grdUserPager");

        function SearchTable(TableName, InputID, StartPoint) {
            var input, filter, found, table, tr, td, i, j;
            input = document.getElementById(InputID);
            filter = input.value.toUpperCase();
            table = document.getElementById(TableName);
            tr = table.getElementsByTagName("tr");
            for (i = StartPoint; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td");
                for (j = 0; j < td.length; j++) {
                    if (td[j].innerHTML.toUpperCase().indexOf(filter) > -1) {
                        found = true;
                    }
                }
                if (found) {
                    tr[i].style.display = "";
                    found = false;
                } else {
                    tr[i].style.display = "none";
                }
            }
        }

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