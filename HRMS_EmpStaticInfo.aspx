<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/HRMS.Master" CodeBehind="HRMS_EmpStaticInfo.aspx.cs" Inherits="HRMS.HRMS_EmpStaticInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HRMSContent" runat="server">
    <asp:MultiView runat="server" ActiveViewIndex="0">
        <asp:View ID="View1" runat="server">
            <div id="EmpHeader" class="  m-3" >
            <div class="">
                <asp:Button runat="server" Text="Add" OnClick="AddBtn_Click" CssClass=" btn btn-primary ms-2" />
            <asp:Button runat="server" Text="Delete" CssClass=" btn btn-danger" />
                <div id="searchbar" class=" float-end">
            <%--<i class="material-icons justify-content-center">search </i>--%>
            <asp:TextBox ID="EmpSearch" runat="server" placeholder="Employee Name"></asp:TextBox>
            <asp:Button runat="server" Text="Search" OnClick="EmpMasSearchBtn_Click" CssClass="btn btn-light mx-2" />
        </div>
            </div>
            
        <div class=" justify-content-center mx-2 mt-2">
            <asp:GridView ID="grdUser" PagerStyle-CssClass="GridPager"  PagerStyle-HorizontalAlign="Center" runat="server" CssClass=" HRMSEmptable table-hover  table-responsive table table-bordered border-1 border-black " AllowPaging="true" OnPageIndexChanging="grdUser_PageIndexChanging" AutoGenerateColumns="false" CellSpacing="15" Width="100%" >
                <Columns>
                    <%--<asp:CheckBoxField Visible="true" HeaderText="Select"/>--%>
                    <asp:BoundField DataField="Emp_Name" HeaderText="Emp_Name"/>
                    <asp:BoundField HeaderText="Designation" DataField="Emp_Desig" />
                    <%--<asp:BoundField HeaderText="Qualification" DataField="Emp_Grade" />--%>
                    <asp:BoundField HeaderText="Date of Birth" DataField="Emp_DOB" />
                    <asp:BoundField HeaderText="Date of Joining" DataField="Emp_JoinDt"/>
                    <asp:BoundField HeaderText="Department" DataField="Emp_Dept" />
                    <asp:ButtonField CommandName="Edit" Text="Edit"   ControlStyle-CssClass="btn btn-secondary" ButtonType="Button" />
                    <asp:ButtonField CommandName="View" Text="View" ControlStyle-CssClass="btn btn-success" ButtonType="Button" />
                </Columns>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
                <RowStyle CssClass="table-light  border-black" />
                <AlternatingRowStyle CssClass="table-info border-black" />
                <HeaderStyle CssClass="" />
                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last"  />
            </asp:GridView>
        </div>
        </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>