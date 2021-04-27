<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ResultRicercaAddizionali.aspx.vb" Inherits="OpenUtenze.ResultRicercaAddizionali" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ResultRicercaAddizionali</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
    <script type="text/javascript">
        function DettaglioAddizionali(IdAddizionaleEnte, IdAddizionale, Tariffa, Anno, Iva) {
            parent.parent.Comandi.location.href = 'CConfiguraAddizionali.aspx'
            parent.parent.Visualizza.loadInsert.src = 'ConfiguraAddizionali.aspx?IdAddizionaleEnte=' + IdAddizionaleEnte + '&IdAddizionale=' + IdAddizionale + '&Anno=' + escape(Anno) + '&Tariffa=' + Tariffa + '&Iva=' + Iva + '&Operazione=modifica'
        }
    </script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <table style="z-index: 101; left: 0px; position: absolute; top: 0px" width="100%">
            <tr>
                <td>
                    <asp:Label ID="LblResult" runat="server" CssClass="NormalBold">Risultati della Ricerca</asp:Label>
                    <Grd:RibesGridView ID="GrdAddizionaliEnte" runat="server" BorderStyle="None"
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
                            <asp:BoundField DataField="sAnno" SortExpression="sAnno" HeaderText="Anno">
                                <HeaderStyle ForeColor="White" Width="20%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sDescrizione" SortExpression="sDescrizione" HeaderText="Capitolo">
                                <HeaderStyle ForeColor="White" Width="40%"></HeaderStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Importo ">
                                <HeaderStyle Width="25%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# EuroForGridView(DataBinder.Eval(Container, "DataItem.dImporto")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Aliquota" HeaderText="% IVA">
                                <HeaderStyle Width="15%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("id") %>' alt=""></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("ID") %>' />
                                    <asp:HiddenField runat="server" ID="hfIDaddizionale" Value='<%# Eval("IDaddizionale") %>' />
                                    <asp:HiddenField runat="server" ID="hfsIdEnte" Value='<%# Eval("sIdEnte") %>' />
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
