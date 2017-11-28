<%@ Page Title="Registrovat externí přihlášení" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegisterExternalLogin.aspx.cs" Inherits="WebApp.Account.RegisterExternalLogin" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
<h3>Registrovat pomocí účtu <%: ProviderName %></h3>

    <asp:PlaceHolder runat="server">
        <div class="form-horizontal">
            <h4>Přidružovací formulář</h4>
            <hr />
            <asp:ValidationSummary runat="server" ShowModelStateErrors="true" CssClass="text-danger" />
            <p class="text-info">
                Došlo k ověření pomocí <strong><%: ProviderName %></strong>. Zadejte níže e-mail pro aktuální web
                a klikněte na tlačítko Přihlásit.
            </p>

            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="email" CssClass="col-md-2 control-label">E-mail</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="email" CssClass="form-control" TextMode="Email" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="email"
                        Display="Dynamic" CssClass="text-danger" ErrorMessage="Pole E-mail je povinné." />
                    <asp:ModelErrorMessage runat="server" ModelStateKey="email" CssClass="text-error" />
                </div>
            </div>

            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button runat="server" Text="Přihlásit" CssClass="btn btn-default" OnClick="LogIn_Click" />
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
</asp:Content>
