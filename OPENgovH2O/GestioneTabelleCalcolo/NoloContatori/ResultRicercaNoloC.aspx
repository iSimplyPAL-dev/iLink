<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ResultRicercaNoloC.aspx.vb" Inherits="OpenUtenze.ResultRicercaNoloC" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ResultRicercaNoloC</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
    <script type="text/javascript">
        function DettaglioNolo(IdNoloEnte, IdTipoContatore, Iva, Tariffa, Anno, IsUnaTantum) {
            /*parent.parent.Comandi.location.href='CConfiguraTariffe.aspx'
            parent.parent.Visualizza.location.href='ConfiguraTariffe.aspx?IdTariffa=' + IdTariffa + '&Importo=' + Importo + '&DataInizio=' + DataInizio + '&IdCategoria=' + IdCategoria + '&IdCatCombo=' + IdCatCombo + '&Operazione=modifica'*/
            parent.parent.Comandi.location.href = 'CConfiguraNoloC.aspx'
            parent.parent.Visualizza.loadInsert.src = 'ConfiguraNoloC.aspx?IdNoloEnte=' + IdNoloEnte + '&IdTipoContatore=' + IdTipoContatore + '&Anno=' + escape(Anno) + '&Tariffa=' + Tariffa + '&Iva=' + Iva + '&Operazione=modifica&IsUnaTantum=' + IsUnaTantum
        }
    </script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <table width="100%" style="z-index: 101; left: 0px; position: absolute; top: 0px">
            <tr>
                <td>
                    <asp:Label ID="LblResult" CssClass="NormalBold" runat="server">Risultati della Ricerca</asp:Label>
                    <Grd:RibesGridView ID="GrdNoliEnte" runat="server" BorderStyle="None"
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
                            <asp:BoundField DataField="sDescrizioneTContatore" SortExpression="sDescrizioneTContatore" HeaderText="Tipologia Contatore">
                                <HeaderStyle ForeColor="White" Width="40%"></HeaderStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Importo ">
                                <HeaderStyle></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# FncGrd.EuroForGridView(DataBinder.Eval(Container, "DataItem.dImporto")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="% IVA">
                                <HeaderStyle></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# FncGrd.EuroForGridView(DataBinder.Eval(Container, "DataItem.dAliquota")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="UNA TANTUM">
                                <HeaderStyle></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox Enabled="False" ID="Label3" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.bisunatantum") %>'></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("id") %>' alt=""></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="hfIdTipoContatore" Value='<%# Eval("IdTipoContatore") %>' />
                                    <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("ID") %>' />
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
