<%@ Page Language="c#" CodeBehind="AvanzamentoElaborazione.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.ElaborazioneDocumenti.AvanzamentoElaborazione" EnableEventValidation="false" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>AvanzamentoElaborazione</title>
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
</head>
<body class="Sfondo" topmargin="0" leftmargin="0" rightmargin="0">
    <form id="Form1" runat="server" method="post">
    <Grd:RibesGridView ID="GrdDaElaborare" runat="server" BorderStyle="None" 
        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red" OnPageIndexChanging="GrdPageIndexChanging">
        <PagerSettings Position="Bottom"></PagerSettings>
        <PagerStyle CssClass="CartListFooter" />
        <RowStyle CssClass="CartListItem"></RowStyle>
        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
        <Columns>
            <asp:BoundField DataField="Cognome" HeaderText="Cognome">
                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                <ItemStyle HorizontalAlign="Left" Width="100px" VerticalAlign="Middle"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="Nome" HeaderText="Nome">
                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                <ItemStyle HorizontalAlign="Left" Width="100px" VerticalAlign="Middle"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField Visible="False" DataField="CodiceFiscale" HeaderText="Codice Fiscale">
                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                <ItemStyle HorizontalAlign="Left" Width="100px" VerticalAlign="Middle"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="DESCRTRIBUTO" HeaderText="Tributo">
	            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
            </asp:BoundField>
            <asp:BoundField DataField="Numero_Fabbricati" HeaderText="N&#176; Fabbricati">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="ICI_DOVUTA_ABITAZIONE_PRINCIPALE_TOTALE" HeaderText="Imp. Abit. Principale €" DataFormatString="{0:N}">
                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="ICI_DOVUTA_ALTRI_FABBRICATI_TOTALE" HeaderText="Imp. Altri Fabbricati €" DataFormatString="{0:N}">
                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="ICI_DOVUTA_AREE_FABBRICABILI_TOTALE" HeaderText="Imp. Aree Fabbricabili €" DataFormatString="{0:N}">
                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="ICI_DOVUTA_TOTALE" HeaderText="Importo Totale €" DataFormatString="{0:N}">
                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
            </asp:BoundField>
            <asp:TemplateField HeaderText="Elabora">
                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                <ItemStyle HorizontalAlign="Center" Width="50px" VerticalAlign="Middle"></ItemStyle>
                <ItemTemplate>
                    <asp:CheckBox ID="chkIncludi" runat="server" Checked='<%# Business.CoreUtility.FormattaGrdCheck(DataBinder.Eval(Container, "DataItem.INCLUDI")) %>' AutoPostBack="true"></asp:CheckBox>
                    <asp:HiddenField runat="server" ID="hfIDContribuente" Value='<%# Eval("CODCONTRIBUENTE") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </Grd:RibesGridView>
    </form>
</body>
</html>
