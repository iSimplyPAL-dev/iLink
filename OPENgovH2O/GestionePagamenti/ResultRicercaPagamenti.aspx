<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ResultRicercaPagamenti.aspx.vb" Inherits="OpenUtenze.ResultRicercaPagamenti" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ResultRicercaPagamenti</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript">
        function DettaglioPagamento(Id, NumeroFattura, DataFattura, Anno, ImportoPagamento, ImportoEmesso, CodUtente, DataAccredito) {
            parent.parent.Comandi.location.href = 'Inserimento/CInserimentoPagamenti.aspx'
            parent.parent.Visualizza.location.href = 'Inserimento/RicercaFatture.aspx?Id=' + Id + '&NumeroFattura=' + NumeroFattura + '&DataFattura=' + DataFattura + '&Anno=' + escape(Anno) + '&ImportoPagamento=' + ImportoPagamento + '&ImportoEmesso=' + ImportoEmesso + '&CodUtente=' + CodUtente + '&Operazione=modifica' + '&DataAccredito=' + DataAccredito
        }
    </script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <table style="z-index: 101; left: 0px; position: absolute; top: 0px" width="100%">
            <tr>
                <td>
                    <asp:Label ID="LblResult" CssClass="NormalBold" runat="server">Risultati della Ricerca</asp:Label>
                    <Grd:RibesGridView ID="GrdPagamenti" runat="server" BorderStyle="None"
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
                            <asp:BoundField DataField="sCognome" SortExpression="Cognome" HeaderText="Cognome">
                                <HeaderStyle ForeColor="White" Width="20%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sNome" SortExpression="Nome" HeaderText="Nome">
                                <HeaderStyle ForeColor="White" Width="20%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sCodFiscalePIva" HeaderText="Cod.Fiscale/P.IVA">
                                <ItemStyle Width="100px"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField SortExpression="DataFattura" HeaderText="Data Fattura">
                                <HeaderStyle ForeColor="White" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.sDataFattura") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="sNumeroFattura" SortExpression="sNumeroFattura" HeaderText="N.Fattura">
                                <HeaderStyle ForeColor="White" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField SortExpression="DataPagamento" HeaderText="Data Pagamento">
                                <HeaderStyle ForeColor="White" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.sDataPagamento") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Riversamento">
                                <HeaderStyle ForeColor="White" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblDataRiversamento" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.sDataAccredito") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ImportoPagamento" HeaderText="Imp. Pagato" DataFormatString="{0:0.00}">
                                <ItemStyle HorizontalAlign="Right" Width="120px"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("id") %>' alt=""></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="hfId" Value='<%# Eval("Id") %>' />
                                    <asp:HiddenField runat="server" ID="hfIdEnte" Value='<%# Eval("IdEnte") %>' />
                                    <asp:HiddenField runat="server" ID="hfsProvenienza" Value='<%# Eval("sProvenienza") %>' />
                                    <asp:HiddenField runat="server" ID="hfImportoEmesso" Value='<%# Eval("ImportoEmesso") %>' />
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
