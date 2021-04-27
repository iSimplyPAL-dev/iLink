<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaConfigTipoVoci.aspx.vb" Inherits="Provvedimenti.RicercaConfigTipoVoci" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>RicercaConfigTipoVoci</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../../_js/Custom.js?newversion"></script>
	</HEAD>
	<body class="SfondoVisualizza">
		<form id="Form1" runat="server" method="post">
			<fieldset class="classeFiledSetIframe"><legend class="Legend">Visualizzazione Voci</legend></fieldset>
			<br>
			<asp:label id="lblMessage" runat="server" CssClass="NormalRed" Visible="False">lblMessage</asp:label>
			 <Grd:RibesGridView ID="GrdVoci" runat="server" BorderStyle="None" 
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
					<asp:BoundField DataField="descTributo" HeaderText="Tributo">
						<HeaderStyle></HeaderStyle>
					</asp:BoundField>
					<asp:BoundField DataField="descCapitolo" HeaderText="Capitolo">
						<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="DescTipoProvvedimento" HeaderText="Tipo Provvedimento">
						<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="VOCE" HeaderText="Voce">
						<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="DESCRIZIONE_VOCE_ATTRIBUITA" HeaderText="Descrizione Voce"></asp:BoundField>
					<asp:BoundField DataField="descMisura" HeaderText="Misura"></asp:BoundField>
					<asp:BoundField DataField="DESCRIZIONE_FASE" HeaderText="Fase"></asp:BoundField>
					<asp:TemplateField HeaderText="">
						<headerstyle horizontalalign="Center"></headerstyle>
						<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
						<itemtemplate>
						<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID_TIPO_VOCE") %>' alt=""></asp:ImageButton>
						    <asp:HiddenField runat="server" ID="hfCOD_TRIBUTO" Value='<%# Eval("COD_TRIBUTO") %>' />
                            <asp:HiddenField runat="server" ID="hfCOD_CAPITOLO" Value='<%# Eval("COD_CAPITOLO") %>' />
                            <asp:HiddenField runat="server" ID="hfCOD_TIPO_PROVVEDIMENTO" Value='<%# Eval("COD_TIPO_PROVVEDIMENTO") %>' />
                            <asp:HiddenField runat="server" ID="hfMISURA" Value='<%# Eval("MISURA") %>' />
                            <asp:HiddenField runat="server" ID="hfCOD_FASE" Value='<%# Eval("COD_FASE") %>' />
                            <asp:HiddenField runat="server" ID="hfCOD_VOCE" Value='<%# Eval("COD_VOCE") %>' />
                            <asp:HiddenField runat="server" ID="hfID_TIPO_VOCE" Value='<%# Eval("ID_TIPO_VOCE") %>' />
						</itemtemplate>
					</asp:TemplateField>
				</Columns>
			</Grd:RibesGridView>&nbsp;
		</form>
	</body>
</HTML>
