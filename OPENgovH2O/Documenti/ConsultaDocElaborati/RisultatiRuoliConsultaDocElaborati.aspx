<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RisultatiRuoliConsultaDocElaborati.aspx.vb" Inherits="OpenUtenze.RisultatiRuoliConsultaDocElaborati" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>RisultatiRuoliConsultaDocElaborati</title>
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
        function CaricaDocEffettivi() {
            loadGridDocumenti.location.href = 'ConsultaDocElaborati.aspx'
        }
    </script>
</head>
<body class="Sfondo" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="Form1" runat="server" method="post">
        <table style="z-index: 101; left: 0px; position: absolute; top: 0px" width="100%">
            <tr>
                <td>
                    <asp:Label ID="LblResult" CssClass="Legend" runat="server">Risultati della Ricerca</asp:Label>
                    <Grd:RibesGridView ID="GrdDocumentiElaborati" runat="server" BorderStyle="None"
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
                            <asp:TemplateField HeaderText="Data Emissione Fattura">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="tDataEmissioneFattura" runat="server" Text='<%# FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataEmissioneFattura"))%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Elab. Documenti">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="tDataApprovazioneDOC" runat="server" Text='<%# FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataApprovazioneDOC"))%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="impPositivi" HeaderText="Imp. Positivi " DataFormatString="{0:0.00}">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="impNegativi" HeaderText="Imp. Negativi " DataFormatString="{0:0.00}">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IdFlusso") %>' alt=""></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="hfIdFlusso" Value='<%# Eval("IdFlusso") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </td>
            </tr>
            <tr>
                <td>
                    <iframe id="loadGridDocumenti" name="loadGridDocumenti" src="../../../aspVuota.aspx" frameborder="0" width="100%" height="330px" runat="server"></iframe>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
