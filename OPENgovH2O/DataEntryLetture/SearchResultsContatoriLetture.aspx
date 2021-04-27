<%@ Page Language="vb" CodeBehind="SearchResultsContatoriLetture.aspx.vb" AutoEventWireup="false" Inherits="OpenUtenze.SearchResultsContatoriLetture" EnableEventValidation="false" %>

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
<body class="Sfondo" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0"
    marginwidth="0" marginheight="0">
    <form method="post" action="../Letture/Letture.aspx" id="frmHidden" name="frmHidden" target="visualizza">
        <input id="bIsInVar" type="hidden" name="bIsInVar"><input id="hdIDContatore" type="hidden" name="hdIDContatore">
        <input id="hdCodiceVia" type="hidden" name="hdCodiceVia">
        <input id="hdUbicazioneText" type="hidden" name="hdUbicazioneText">
        <input id="hdIntestatario" type="hidden" name="hdIntestatario">
        <input id="hdUtente" type="hidden" name="hdUtente">
        <input id="hdGiro" type="hidden" name="hdGiro">
        <input id="hdNumeroUtente" type="hidden" name="hdNumeroUtente">
        <input id="hdCessati" type="hidden" name="hdCessati">
        <input id="hdMatricola" type="hidden" name="hdMatricola">
        <input id="paginacomandi" type="hidden" name="paginacomandi">
        <input id="PAG_PREC" type="hidden" name="PAG_PREC">
    </form>
    <form runat="server">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td>
                    <asp:Label ID="lblMessage" runat="server" CssClass="NormalBold"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <Grd:RibesGridView ID="GrdContatoriLetture" runat="server" BorderStyle="None"
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
                            <asp:TemplateField HeaderText="Nominativo Intestatario">
                                <HeaderStyle></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%# FncGrd.GiraAnagrafiche(DataBinder.Eval(Container, "DataItem.cognome_int")&" "&DataBinder.Eval(Container, "DataItem.nome_int")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nominativo Utente">
                                <HeaderStyle></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# FncGrd.GiraAnagrafiche(DataBinder.Eval(Container, "DataItem.cognome_ut")&" "&DataBinder.Eval(Container, "DataItem.nome_ut")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Matricola">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MATRICOLA") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ubicazione">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.VIA_UBICAZIONE") & "  " & DataBinder.Eval(Container, "DataItem.CIVICO_UBICAZIONE")  %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="NUMEROUTENTE" HeaderText="N&#176; Utente"></asp:BoundField>
                            <asp:TemplateField HeaderText="Let.">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgEdit" runat="server" Height="15px" Width="15px" CommandName="Edit" ImageUrl='<%# FncGrd.FormattaLettura(DataBinder.Eval(Container, "DataItem.LETTURA")) %>'></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("CODCONTATORE") %>' alt=""></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="hfCODCONTATORE" Value='<%# Eval("CODCONTATORE") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
