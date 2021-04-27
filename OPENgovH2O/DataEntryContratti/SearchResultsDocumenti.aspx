<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SearchResultsDocumenti.aspx.vb" Inherits="OpenUtenze.SearchResultsDocumenti" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Ricerca Documenti</title>
    <link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <script type="text/javascript">
        //function ApriDocumento(parametro){
        //window.open(parametro,"Documenti","width=100,height=100");
        //}

        function chiudifinestra() {
            window.close();
        }


        function ApriDocumento(parametro) {
            //GestAlert('c', 'question', 'Aprire il documento di tipo "+descrizione+" creato in data "+data+"relativo all'utente "+cognome+" "+nome+"?");

            myleft = ((screen.width) / 2) - 400;
            mytop = ((screen.height) / 2) - 300;
            window.open(parametro, "Documenti", "width=" + (screen.width) - 30 + ", height=" + (screen.height) - 30 + ", menubar=yes, toolbar=yes, resizable=yes");
        }

        function ChiudiFinestraCorrente() {
            //alert("Ok, passa dalla funzione");
            window.close();
        }
    </script>
</head>
<body class="Sfondo" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0"
    marginheight="0" marginwidth="0">
    <form id="Form1" runat="server" method="post">
        <div class="SfondoGenerale" align="right">
            <input class="Bottone BottoneAnnulla" title="Chiudi la finestra corrente" onclick="ChiudiFinestraCorrente();"
                type="button">
        </div>
        <br>
        <table cellspacing="0" cellpadding="0" width="90%" align="center" border="0">
            <tr>
                <td align="right"></td>
            </tr>
            <tr>
                <td class="NormalBold">
                    <asp:Label ID="lbldocumenti">Visualizzazione Documenti Estratti</asp:Label></td>
            </tr>
            <tr>
                <td></td>
            </tr>
            <tr>
                <td></td>
            </tr>
            <tr>
                <td class="bordoIframe">
                    <br>
                    <asp:Label ID="lblMessage" runat="server" CssClass="NormalBold"></asp:Label>
                    <Grd:RibesGridView ID="GrdDocumenti" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnRowDataBound="GrdRowDataBound" OnPageIndexChanging="GrdPageIndexChanging">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField Visible="False" DataField="DESCRIZIONE" HeaderText="Descrizione"></asp:BoundField>
                            <asp:TemplateField HeaderText="Codice Contratto">
                                <HeaderStyle HorizontalAlign="Center" Width="1%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MIOCODICE") %>' ID="Label4">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cognome">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Request.Params("cognome") %>' ID="miaprova">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nome">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Request.Params("nome") %>' ID="Label3">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DESCRIZIONE" SortExpression="Descrizione" HeaderText="Descrizione"></asp:BoundField>
                            <asp:TemplateField HeaderText="Data Creazione">
                                <HeaderStyle HorizontalAlign="Center" Width="1%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="1%"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DATA") %>' ID="Label1">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField Visible="False" DataField="LINK" HeaderText="Linj"></asp:BoundField>
                            <asp:TemplateField HeaderText="Documento">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LINK").substring(65) %>' ID="Label2">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("LINK") %>' alt=""></asp:ImageButton>
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
