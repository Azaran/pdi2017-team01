<%@ Page Title="Správa účtu" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="WebApp.Account.Manage" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>

    <div>
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="text-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="form-horizontal">
                <h4>Změňte nastavení účtu.</h4>
                <hr />
                <dl class="dl-horizontal">
                    <dt>Heslo:</dt>
                    <dd>
                        <asp:HyperLink NavigateUrl="/Account/ManagePassword" Text="[Změnit]" Visible="false" ID="ChangePassword" runat="server" />
                        <asp:HyperLink NavigateUrl="/Account/ManagePassword" Text="[Vytvořit]" Visible="false" ID="CreatePassword" runat="server" />
                    </dd>
                    <dt>Externí přihlášení:</dt>
                    <dd><%: LoginsCount %>
                        <asp:HyperLink NavigateUrl="/Account/ManageLogins" Text="[Spravovat]" runat="server" />

                    </dd>
                    <%--
                        Telefonní číslo je možné použít jako druhou úroveň při dvojúrovňovém ověřování.
                        V <a href="https://go.microsoft.com/fwlink/?LinkId=403804">tomto článku</a>
                        najdete podrobnosti o nastavení podpory dvojúrovňového ověřování pomocí SMS v této aplikaci ASP.NET.
                        Po nastavení dvojúrovňového ověřování odkomentujte následující bloky.
                    --%>
                    <%--
                    <dt>Telefonní číslo:</dt>
                    <% if (HasPhoneNumber)
                       { %>
                    <dd>
                        <asp:HyperLink NavigateUrl="/Account/AddPhoneNumber" runat="server" Text="[Přidat]" />
                    </dd>
                    <% }
                       else
                       { %>
                    <dd>
                        <asp:Label Text="" ID="PhoneNumber" runat="server" />
                        <asp:HyperLink NavigateUrl="/Account/AddPhoneNumber" runat="server" Text="[Změnit]" /> &nbsp;|&nbsp;
                        <asp:LinkButton Text="[Odebrat]" OnClick="RemovePhone_Click" runat="server" />
                    </dd>
                    <% } %>
                    --%>

                    <dt>Dvoufaktorové ověřování:</dt>
                    <dd>
                        <p>
                            Nejsou nakonfigurovaní žádní poskytovatelé dvojúrovňového ověřování. Podrobnosti o nastavení podpory dvojúrovňového ověřování v této aplikaci ASP.NET
                            najdete v <a href="https://go.microsoft.com/fwlink/?LinkId=403804">tomto článku</a>.
                        </p>
                        <% if (TwoFactorEnabled)
                          { %> 
                        <%--
                        Povoleno
                        <asp:LinkButton Text="[Zakázat]" runat="server" CommandArgument="false" OnClick="TwoFactorDisable_Click" />
                        --%>
                        <% }
                          else
                          { %> 
                        <%--
                        Zakázáno
                        <asp:LinkButton Text="[Povolit]" CommandArgument="true" OnClick="TwoFactorEnable_Click" runat="server" />
                        --%>
                        <% } %>
                    </dd>
                </dl>
            </div>
        </div>
    </div>

</asp:Content>
