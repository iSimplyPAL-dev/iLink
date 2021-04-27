<%@ Page Language="vb" CodeBehind="SearchResultsContratti.aspx.vb" AutoEventWireup="false" Inherits="OpenUtenze.SearchResultContratti" EnableEventValidation="false" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<html>
<head>
    <link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript">
        function stampaRiassuntiva() {
            alert("Funzione stampa riassuntiva");
        }

        function ApriDocumenti(parametro, parametro2, parametro3) {
            //alert("Ok, sei in ApriDocumenti");
            //alert("\nStai passando i seguenti parametri via URL:\n\n\n[Codice Contratto] : " + parametro + "\n\n[Cognome] : " + parametro2 + "\n\n[Nome] : " + parametro3);
            myleft = ((screen.width) / 2) - 350;
            mytop = ((screen.height) / 2) - 250;
            window.open("SearchResultsDocumenti.aspx?codcontratto=" + parametro + "&cognome=" + parametro2 + "&nome=" + parametro3, "RicercaDocumenti", "width=700, height=500, toolbar=no,top=" + mytop + ",left=" + myleft + ", menubar=no");
        }

        function ApriContratto(parametro, DataScaricoPDA) {
            //alert("Ok, sei in ApriContratto");
            //alert("Parametro: " + parametro);
            link = "../DataEntryContratti/DatiGeneraliContr.aspx?IDCONTRATTO=" + parametro + "&DataScaricoPDA=" + DataScaricoPDA;
            parent.location.href = link;
        }
    </script>
</head>
<body class="Sfondo" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0"
    marginwidth="0" marginheight="0">
    <form id="Form1" runat="server" method="post">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td>
                    <asp:Label ID="lblMessage" runat="server" CssClass="NormalBold"></asp:Label>
                    <Grd:RibesGridView ID="GrdContratti" runat="server" BorderStyle="None"
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
                                    <asp:Label ID="Label6" runat="server" Text='<%# FncGrd.GiraAnagrafiche(DataBinder.Eval(Container, "DataItem.cognomeint")&" "&DataBinder.Eval(Container, "DataItem.nomeint")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nominativo Utente">
                                <HeaderStyle></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# FncGrd.GiraAnagrafiche(DataBinder.Eval(Container, "DataItem.cognomeut")&" "&DataBinder.Eval(Container, "DataItem.nomeut")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ubicazione">
                                <ItemTemplate>
                                    <asp:Label ID="Label8" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.via_ubicazione") &amp; "   " &amp;  DataBinder.Eval(Container, "DataItem.civico_ubicazione") &amp; "   " &amp;  DataBinder.Eval(Container, "DataItem.esponente_civico")  %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Codice Contratto">
                                <ItemTemplate>
                                    <asp:Label ID="Label9" Text='<%# DataBinder.Eval(Container, "DataItem.CODICECONTRATTO") %>' runat="server">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Sottoscrizione">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label10" runat="server" Text='<%# FncGrd.FormatGrdData(DataBinder.Eval(Container, "DataItem.datasottoscrizione")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Cessazione">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label11" runat="server" Text='<%# FncGrd.FormatGrdData(DataBinder.Eval(Container, "DataItem.DATACESSAZIONE")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("CODCONTRATTO") %>' alt=""></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="hfCODCONTRATTO" Value='<%# Eval("CODCONTRATTO") %>' />
                                    <asp:HiddenField runat="server" ID="hfCOGNOMEUT" Value='<%# Eval("COGNOMEUT") %>' />
                                    <asp:HiddenField runat="server" ID="hfNOMEUT" Value='<%# Eval("NOMEUT") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </td>
            </tr>
        </table>
        <!-- Gestione del mantenimento dei parametri di ricerca -->
        <input id="IDContratto" type="hidden" name="IDContratto">
        <input id="hdCodiceVia" type="hidden" name="hdCodiceVia">
        <input id="hdIntestatario" type="hidden" name="hdIntestatario">
        <input id="hdUtente" type="hidden" name="hdUtente">
        <input id="hdMatricola" type="hidden" name="hdMatricola">
        <input id="hdAvviaRicerca" type="hidden" name="hdAvviaRicerca">
    </form>
</body>
</html>
