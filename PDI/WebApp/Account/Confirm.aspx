<%@ Page Title="Potvrzení účtu" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Confirm.aspx.cs" Inherits="WebApp.Account.Confirm" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>

    <div>
        <asp:PlaceHolder runat="server" ID="successPanel" ViewStateMode="Disabled" Visible="true">
            <p>
                Děkujeme vám za potvrzení vašeho účtu. Klepnutím <asp:HyperLink ID="login" runat="server" NavigateUrl="~/Account/Login">sem</asp:HyperLink>  se přihlásíte.             
            </p>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="errorPanel" ViewStateMode="Disabled" Visible="false">
            <p class="text-danger">
                Došlo k chybě.
            </p>
        </asp:PlaceHolder>
    </div>
</asp:Content>
