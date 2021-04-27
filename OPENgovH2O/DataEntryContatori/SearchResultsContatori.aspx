<%@ Page Language="vb" CodeBehind="SearchResultsContatori.aspx.vb" AutoEventWireup="false" Inherits="OpenUtenze.SearchResultsContatori" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<html>
<head>
    <title></title>
    <link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript">
        function ApriContatore(parametro) {
            //alert("Ok, sei in ApriContratto");
            //alert("Parametro: " + parametro);
            link = "DatiGenerali.aspx?IDCONTATORE=" + parametro;
            parent.location.href = link;
        }
    </script>
</head>
<body class="Sfondo" >
    <!-- Gestione del mantenimento dei parametri di ricerca -->
    <form method="post" action="../DataEntryContatori/DatiGenerali.aspx" id="frmHidden" name="frmHidden"
        target="visualizza">
        <input id="IDContatore" type="hidden" name="IDContatore">
        <input id="hdCodiceVia" type="hidden" name="hdCodiceVia">
        <input id="hdIntestatario" type="hidden" name="hdIntestatario">
        <input id="hdUtente" type="hidden" name="hdUtente">
        <input id="hdNumeroUtente" type="hidden" name="hdNumeroUtente">
        <input id="hdMatricola" type="hidden" name="hdMatricola">
        <input id="hdAvviaRicerca" type="hidden" name="hdAvviaRicerca">
    </form>
    <!-- Fine Gestione del mantenimento dei parametri di ricerca -->
    <form runat="server" id="Form1">
        <table border="0">
            <tr>
                <td>
                    <asp:Label ID="lblMessage" runat="server" CssClass="NormalBold"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <Grd:RibesGridView ID="GrdContatori" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnRowDataBound="GrdRowDataBound" OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="Descrizione_Ente" HeaderText="Ente"></asp:BoundField>
                            <asp:TemplateField HeaderText="Nominativo Intestatario">
                                <HeaderStyle></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%# FncGrd.GiraAnagrafiche(DataBinder.Eval(Container, "DataItem.cognome_int")&" "&DataBinder.Eval(Container, "DataItem.nome_int")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="nominativo_utente" HeaderText="Nominativo Utente">
                                <HeaderStyle></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# FncGrd.GiraAnagrafiche(DataBinder.Eval(Container, "DataItem.cognome_ut")&" "&DataBinder.Eval(Container, "DataItem.nome_ut")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="MATRICOLA" HeaderText="Matricola" SortExpression="MATRICOLA">
                                <HeaderStyle HorizontalAlign="LEFT"></HeaderStyle>
                                <ItemStyle HorizontalAlign="LEFT"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Ubicazione" SortExpression="VIA_UBICAZIONE">
                                <HeaderStyle></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.VIA_UBICAZIONE") & "   " &  FncGrd.IntForGridView(DataBinder.Eval(Container, "DataItem.CIVICO_UBICAZIONE")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Contratto Associato" DataField="CODICECONTRATTO"></asp:BoundField>
                            <asp:BoundField DataField="NUMEROUTENTE" HeaderText="N&#176; Utente">
                                <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Data Installazione">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FncGrd.FormatGrdData(DataBinder.Eval(Container, "DataItem.DATAATTIVAZIONE")) %>' ID="Label4">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Cessazione">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FncGrd.FormatGrdData(DataBinder.Eval(Container, "DataItem.DATACESSAZIONE")) %>' ID="Label5">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sub.">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgEdit" runat="server" Height="15px" Width="15px" ImageUrl='<%# FncGrd.FormattaIsSubContatore(DataBinder.Eval(Container, "DataItem.CODCONTATORESUB")) %>' ToolTip='<%# FncGrd.FormattaToolTipSubContatore(DataBinder.Eval(Container, "DataItem.MATRICOLAPRINCIPALE")) %>'></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sel.">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSel" runat="server" AutoPostBack="true" Checked='<%# DataBinder.Eval(Container, "DataItem.bSel") %>' />
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
