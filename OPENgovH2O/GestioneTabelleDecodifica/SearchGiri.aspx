<%@ Page Language="vb" CodeBehind="SearchGiri.aspx.vb" AutoEventWireup="false" Inherits="OpenUtenze.SearchGiri" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<html>
<head>
    <link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
</head>
<body class="Sfondo" bottommargin="0" leftmargin="0" topmargin="0" marginwidth="0"
    marginheight="0">
    <form id="Form1" runat="server" method="post">
        <table cellspacing="1" cellpadding="1" width="98%" align="center" border="0">
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="NormalBold">Risultati della ricerca</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMessage" runat="server" CssClass="NormalBold"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <Grd:RibesGridView ID="GrdGiri" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="COD_GIRO_EST" HeaderText="Codice Giro"></asp:BoundField>
                            <asp:BoundField DataField="DESCRIZIONE" HeaderText="Descrizione">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Ente">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# GetEnte(DataBinder.Eval(Container, "DataItem.CODENTE")) %>' ID="Label1">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IDGIRO") %>' alt=""></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="hfIDGIRO" Value='<%# Eval("IDGIRO") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </td>
            </tr>
            <tr>
                <td>
                    <iframe id="iframetabella" name="iframetabella" width="95%" src="../../aspVuota.aspx" frameborder="0"
                        height="290"></iframe>
                </td>
            </tr>
        </table>
        <input id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza">
        <input id="paginacomandi" type="hidden" name="paginacomandi">
        <asp:Button ID="btnNuovo" runat="server" Style="display: none"></asp:Button>
    </form>
</body>
</html>
