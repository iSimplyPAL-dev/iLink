<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ResultRicercaFatture.aspx.vb" Inherits="OpenUtenze.ResultRicercaFatture" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>ResultRicercaFatture</title>
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
        function DettaglioPagamento(Id, NumeroFattura, DataFattura, Importo, ImportoEmesso, DataPagamento, CodUtente, Operazione, DataAccredito) {
            parent.parent.Comandi.location.href = 'CInserimentoPagamenti.aspx?Selezionato=Si'
            parent.parent.Visualizza.loadInsert.src = 'InserimentoPagamenti.aspx?Id=' + Id + '&NumeroFattura=' + NumeroFattura + '&DataFattura=' + DataFattura + '&Importo=' + Importo + '&ImportoEmesso=' + ImportoEmesso + '&DataPagamento=' + DataPagamento + '&CodUtente=' + CodUtente + '&Operazione=' + Operazione + '&DataAccredito=' + DataAccredito
            //console.log('InserimentoPagamenti.aspx?Id=' + Id + '&NumeroFattura=' + NumeroFattura + '&DataFattura=' + DataFattura + '&Importo=' + Importo + '&ImportoEmesso=' + ImportoEmesso + '&DataPagamento=' + DataPagamento + '&CodUtente=' + CodUtente + '&Operazione=' + Operazione + '&DataAccredito=' + DataAccredito);
        }
    </script>
</head>
<body class="Sfondo">
    <form id="Form1" runat="server" method="post">
        <table style="z-index: 101; left: 0px; position: absolute; top: 0px" width="100%">
            <tr>
                <td>
                    <asp:Label ID="LblResult" CssClass="NormalBold" runat="server">Risultati della Ricerca</asp:Label>
                    <Grd:RibesGridView ID="GrdFatture" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="true" PageSize="10"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <FooterStyle CssClass="CartListFooter"></FooterStyle>
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:TemplateField SortExpression="DataFattura" HeaderText="Data Fattura">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# oReplace.GiraDataFromDB(DataBinder.Eval(Container, "DataItem.sDataFattura")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="sNumeroFattura" SortExpression="NumeroFattura" HeaderText="Numero Fattura">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sCognome" SortExpression="Cognome" HeaderText="Cognome">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sNome" SortExpression="Nome" HeaderText="Nome">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Importo Emesso">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# FncGrd.EuroForGridView(DataBinder.Eval(Container, "DataItem.ImportoEmesso")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Importo Pagato">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# FncGrd.EuroForGridView(DataBinder.Eval(Container, "DataItem.ImportoPagamento")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" CssClass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IdFatturaNota") %>' alt=""></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="hfId" Value='<%# Eval("Id") %>' />
                                    <asp:HiddenField runat="server" ID="hfIdFatturaNota" Value='<%# Eval("IdFatturaNota") %>' />
                                    <asp:HiddenField runat="server" ID="hfIdEnte" Value='<%# Eval("IdEnte") %>' />
                                    <asp:HiddenField runat="server" ID="hfnCodUtente" Value='<%# Eval("nCodUtente") %>' />
                                    <asp:HiddenField runat="server" ID="hfNumeroFattura" Value='<%# Eval("sNumeroFattura") %>' />
                                    <asp:HiddenField runat="server" ID="hfDataFattura" Value='<%# DataBinder.Eval(Container, "DataItem.sDataFattura") %>' />
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
