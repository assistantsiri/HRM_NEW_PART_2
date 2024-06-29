<%@ Page Language="C#"  Title="Login" AutoEventWireup="true" CodeBehind="HRMSLogin.aspx.cs" Inherits="HRMS.HRMSLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="App_Themes/StyleSheet.css" type="text/css" rel="stylesheet"/>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/fontawesome.min.css" integrity="sha512-d0olNN35C6VLiulAobxYHZiXJmq+vl+BGIgAxQtD5+kqudro/xNMvv2yIHAciGHpExsIbKX3iLg+0B6d0k4+ZA==" crossorigin="anonymous">
    <title>HRMSLogin</title>
</head>
<body>
    <div class="HRMSLoginwrapper"> <%--Base Container--%>
        <div class="HRMSLoginrightc"> <%--Right Container for Image--%>

        </div>
        <div class="HRMSLoginleftc" align="center"> <%--Left Container for Form and Button--%>
            <%--<div class="HRMSLoginHeader">
                <%--<asp:Image runat="server" ID="LoginLogo" CssClass="HRMSLoginLogo" ImageUrl="~/Images/CCSLlogo.png" />--%>
                <%--<h1 id="HRMSLoginHeader1" style ="flex-grow:1">Welcome!</h1>--%>
                <%--<h3></h3>--%>
            <%--</div>--%>
            <form id="HRMSLoginform" runat="server">
                <div class="HRMSLoginHeader">
                    <h2 id="HRMSLoginHeader1">Welcome!</h2>
                </div>
                <div>
                    <%--<i class="HRMSloginicon fas fa-user"></i>--%>
                    <asp:TextBox Id="username" runat="server" placeholder="Username" CssClass="HRMSLoginUser"></asp:TextBox>
                    <%--<asp:Label runat="server" CssClass="HRMSLoginUserlbl" Text="Username"></asp:Label>--%>
                </div>
                <div>
                    <%--<i class="HRMSloginicon fas fa-lock"></i>--%>
                    <asp:TextBox Id="password" runat="server" placeholder="Password" TextMode="Password" CssClass="HRMSLoginPSW"></asp:TextBox>
                    <%--<input type="checkbox" onclick="togglePasswordVisibility()" />--%>
                    <%--<asp:Label runat="server" CssClass="HRMSLoginPSWlbl" Text="Password"></asp:Label>--%>
                </div>
               <%-- <div class="HRMSLoginFPlink">
                    <a href="#">Forgot Password?</a>
                </div>--%>
                <div>
                    <asp:Button ID="LgnBtn" runat="server" Text="Login" CssClass="HRMSLoginLgnBtn" OnClick="LgnBtn_Click" />
                </div>
            </form>
        </div>

    </div>
</body>
</html>
