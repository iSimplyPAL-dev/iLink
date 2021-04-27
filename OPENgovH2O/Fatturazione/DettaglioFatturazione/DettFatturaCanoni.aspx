<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DettFatturaCanoni.aspx.vb" Inherits="OpenUtenze.DettFatturaCanoni"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>DettFatturaCanoni</title>
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
	</HEAD>
	<body class="Sfondo" MS_POSITIONING="GridLayout" topmargin="0" leftmargin="0">
		<form id="Form1" runat="server" method="post">
			<table width="100%">
				<!--Dati Addizionali-->
				<tr>
					<td>
						<asp:label id="LblResultCanoni" CssClass="Legend" Runat="server">Non sono presenti Canoni</asp:label>
						<Grd:RibesGridView ID="GrdCanoni" runat="server" BorderStyle="None" 
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
								<asp:BoundField DataField="sDescrizione" HeaderText="Descrizione">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="impTariffa" HeaderText="Tariffa " DataFormatString="{0:N}">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="nPercentSulConsumo" HeaderText="Percentuale sul Consumo" DataFormatString="{0:N}">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="nAliquota" HeaderText="Aliquota" DataFormatString="{0:N}">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="impCanone" HeaderText="Importo " DataFormatString="{0:N}">
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="">
									<headerstyle horizontalalign="Center"></headerstyle>
									<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									<itemtemplate>
									<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("Id") %>' alt=""></asp:ImageButton>
									<asp:HiddenField runat="server" ID="hfId" Value='<%# Eval("Id") %>' />
									</itemtemplate>
								</asp:TemplateField>
							</Columns>
							</Grd:RibesGridView>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
