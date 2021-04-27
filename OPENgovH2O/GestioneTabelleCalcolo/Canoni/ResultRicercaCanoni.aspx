<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ResultRicercaCanoni.aspx.vb" Inherits="OpenUtenze.ResultRicercaCanoni" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ResultRicercaCanoni</title>
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
        function DettaglioCanoni(IdCanoneEnte, IdTC, IdTU, Aliquota, PercentualeSuConsumo, Tariffa, Anno) {
            /*parent.parent.Comandi.location.href='CConfiguraTariffe.aspx'
            parent.parent.Visualizza.location.href='ConfiguraTariffe.aspx?IdTariffa=' + IdTariffa + '&Importo=' + Importo + '&DataInizio=' + DataInizio + '&IdCategoria=' + IdCategoria + '&IdCatCombo=' + IdCatCombo + '&Operazione=modifica'*/
            parent.parent.Comandi.location.href = 'CConfiguraCanoni.aspx'
            parent.parent.Visualizza.loadInsert.src = 'ConfiguraCanoni.aspx?IdCanoneEnte=' + IdCanoneEnte + '&IdTC=' + IdTC + '&IdTU=' + IdTU + '&Anno=' + escape(Anno) + '&Tariffa=' + Tariffa + '&PercentualeSuConsumo=' + PercentualeSuConsumo + '&Aliquota=' + Aliquota + '&Operazione=modifica'
        }
    </script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <table width="100%" style="z-index: 101; left: 0px; position: absolute; top: 0px">
            <tr>
                <td>
                    <asp:Label ID="LblResult" CssClass="NormalBold" runat="server">Risultati della Ricerca</asp:Label>
                    <Grd:RibesGridView ID="GrdCanoneEnte" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="7"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnRowDataBound="GrdRowDataBound" OnPageIndexChanging="GrdPageIndexChanging">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="sAnno" SortExpression="sAnno" HeaderText="Anno">
                                <HeaderStyle ForeColor="White" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sDescrizioneTC" SortExpression="sDescrizioneTC" HeaderText="Tipologia Canone">
                                <HeaderStyle ForeColor="White" Width="20%"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sDescrizioneTU" SortExpression="sDescrizioneTU" HeaderText="Tipologia Utenza">
                                <HeaderStyle ForeColor="White" Width="20%"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="dTariffa" HeaderText="Tariffa ">
                                <HeaderStyle Width="15%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="% Consumo">
                                <HeaderStyle Width="20%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# FncGrd.EuroForGridView(DataBinder.Eval(Container, "DataItem.dPercentualeSuConsumo")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="% IVA">
                                <HeaderStyle Width="15%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# FncGrd.EuroForGridView(DataBinder.Eval(Container, "DataItem.dAliquota")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("id") %>' alt=""></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="hfidTipoCanone" Value='<%# Eval("idTipoCanone") %>' />
                                    <asp:HiddenField runat="server" ID="hfidTipoUtenza" Value='<%# Eval("idTipoUtenza") %>' />
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
