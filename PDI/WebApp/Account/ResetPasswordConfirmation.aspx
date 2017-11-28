<%@ Page Title="Heslo změněno" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetPasswordConfirmation.aspx.cs" Inherits="WebApp.Account.ResetPasswordConfirmation" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>
    <div>
        <p>Heslo bylo změněno. Klepnutím <asp:HyperLink ID="login" runat="server" NavigateUrl="~/Account/Login">sem</asp:HyperLink> se přihlásíte. </p>
    </div>
</asp:Content>
