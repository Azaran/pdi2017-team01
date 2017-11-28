<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OpenAuthProviders.ascx.cs" Inherits="WebApp.Account.OpenAuthProviders" %>

<div id="socialLoginList">
    <h4>Přihlaste se pomocí jiné služby.</h4>
    <hr />
    <asp:ListView runat="server" ID="providerDetails" ItemType="System.String"
        SelectMethod="GetProviderNames" ViewStateMode="Disabled">
        <ItemTemplate>
            <p>
                <button type="submit" class="btn btn-default" name="provider" value="<%#: Item %>"
                    title="Přihlásit pomocí vašeho <%#: Item %> účtu">
                    <%#: Item %>
                </button>
            </p>
        </ItemTemplate>
        <EmptyDataTemplate>
            <div>
                <p>Nejsou nakonfigurované žádné externí ověřovací služby. Podrobné informace o nastavení podpory přihlašování přes externí služby v této aplikaci ASP.NET najdete v <a href="http://go.microsoft.com/fwlink/?LinkId=252803">tomto článku</a>.</p>
            </div>
        </EmptyDataTemplate>
    </asp:ListView>
</div>
