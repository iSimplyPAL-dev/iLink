<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ResultRicercaDescrizioni.aspx.vb" Inherits="OpenUtenze.ResultRicercaDescrizioni" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ResultRicercaDescrizioni</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript">
        function DettaglioDescrizione(Codice, Descrizione, IdServizio, Tabella) {
            var cod = new String(Codice);
            cod = cod.replace(/\+/g, "*pl*")

            var descr = new String(Descrizione)

            descr = descr.replace(/\+/g, "*pl*")
            parent.parent.Comandi.location.href = 'ComandiConfiguraDescrizioni.aspx'
            //*** 20141125 - Componente aggiuntiva sui consumi ***
            parent.parent.Visualizza.loadInsert.src = 'ConfiguraDescrizioni.aspx?Codice=' + encodeURIComponent(cod) + '&Descrizione=' + encodeURIComponent(descr) + '&Applica=' + IdServizio + '&Tabella=' + Tabella + '&Operazione=modifica'
        }
    </script>
</head>
<body class="Sfondo">
    <table cellspacing="0" cellpadding="0" width="100%" border="0" ms_2d_layout="TRUE">
        <tr valign="top">
            <td>
                <form id="Form1" runat="server" method="post">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0" ms_2d_layout="TRUE">
                        <tr valign="top">
                            <td width="100%">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblResult" CssClass="NormalBold" runat="server">Risultati della Ricerca</asp:Label>
                                            <Grd:RibesGridView ID="GrdAddizionali" runat="server" BorderStyle="None"
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
                                                    <asp:BoundField DataField="DESCRIZIONE" SortExpression="DESCRIZIONE" HeaderText="Descrizione">
                                                        <HeaderStyle ForeColor="White" Width="20%"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("id") %>' alt=""></asp:ImageButton>
                                                            <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("ID") %>' />
                                                            <asp:HiddenField runat="server" ID="hfIDSERVIZIO" Value='<%# Eval("IDSERVIZIO") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </Grd:RibesGridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </form>
            </td>
        </tr>
    </table>
</body>
</html>
