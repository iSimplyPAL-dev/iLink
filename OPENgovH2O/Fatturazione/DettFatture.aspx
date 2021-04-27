<%@ Page Language="vb" AutoEventWireup="False" CodeBehind="DettFatture.aspx.vb" Inherits="OpenUtenze.DettFatture" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Dettaglio Fatture</title>
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <meta content="True" name="vs_showGrid">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../_js/MyUtility.js?newversion"></script>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/vbscript" src="../../_vbs/OperazioniSuCampi.vbs"></script>
    <script type="text/vbscript" src="../../_vbs/ControlliFormali.vbs"></script>
</head>
<body class="Sfondo" bottommargin="0" leftmargin="0" topmargin="0" marginheight="0" marginwidth="0">
    <form id="Form1" runat="server" method="post">
        <asp:Label ID="lblMessage" Style="z-index: 101; left: 0px; position: absolute; top: 5px" runat="server"
            CssClass="NormalRed"></asp:Label>
        <Grd:RibesGridView ID="GrdFatture" runat="server" BorderStyle="None"
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
                <asp:TemplateField SortExpression="DATA_FATTURA" HeaderText="Data Fattura">
                    <HeaderStyle Width="15%"></HeaderStyle>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%#GiraData (DataBinder.Eval(Container, "DataItem.DATA_FATTURA"))%>'>
                        </asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="NUMERO_FATTURA" SortExpression="NUMERO_FATTURA" HeaderText="Numero Fattura"></asp:BoundField>

                <asp:BoundField DataField="TIPO_DOCUMENTO" SortExpression="TIPO_DOCUMENTO" HeaderText="Tipo"></asp:BoundField>

                <asp:BoundField DataField="IMPEMESSO" SortExpression="IMPEMESSO" HeaderText="Importo Totale ">
                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>

                <asp:BoundField DataField="IMPPAG" SortExpression="IMPPAG" HeaderText="Importo Pagato ">
                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                </asp:BoundField>

                <asp:BoundField DataField="MATRICOLA" SortExpression="MATRICOLA" HeaderText="Matricola"></asp:BoundField>
                <asp:TemplateField HeaderText="">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("CODICE_CONTRIBUENTE") %>' alt=""></asp:ImageButton>
                        <asp:HiddenField runat="server" ID="hfIDFATTURANOTA" Value='<%# Eval("IDFATTURANOTA") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </Grd:RibesGridView>
    </form>
</body>
</html>
