<%@ Page Title="Log in" Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Log in</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row" style="margin-top:60px;">
                <div class="col-md-4 col-md-offset-4">
                    <h2 class="text-center">Log in</h2>
                    <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="false" />
                    <asp:Panel runat="server" DefaultButton="LoginButton">
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="UserEmail" CssClass="control-label">Email</asp:Label>
                            <asp:TextBox runat="server" ID="UserEmail" CssClass="form-control" />
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="Password" CssClass="control-label">Password</asp:Label>
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control"/>
                        </div>
                        <asp:Button runat="server" ID="LoginButton" Text="Log in" CssClass="btn btn-primary btn-block" OnClick="LoginButton_Click" />
                    </asp:Panel>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
