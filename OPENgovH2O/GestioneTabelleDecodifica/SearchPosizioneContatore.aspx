<%@ Page Language="vb" CodeBehind="SearchPosizioneContatore.aspx.vb" AutoEventWireup="false" Inherits="OpenUtenze.SearchPosizioneContatore" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<html>
	<head>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
	</head>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" marginwidth="0"
		marginheight="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="1" cellPadding="1" width="98%" align="center" border="0">
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td class="NormalBold">Risultati della ricerca</td>
				</tr>

				<tr>
					<td>
						<asp:label id="lblMessage" runat="server" Cssclass="NormalBold"></asp:label></td>
				</tr>
				<tr>
					<td>
					    <Grd:RibesGridView ID="GrdPosizioneContatore" runat="server" BorderStyle="None" 
							BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
							AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
							ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
							OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
							<PagerSettings Position="Bottom"></PagerSettings>
							<PagerStyle CssClass="CartListFooter" />
							<RowStyle CssClass="CartListItem"></RowStyle>
							<HeaderStyle CssClass="CartListHead"></HeaderStyle>
							<AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							<columns>
								<asp:BoundField DataField="POSIZIONE" HeaderText="Posizione"></asp:BoundField>
								<asp:BoundField DataField="DESCRIZIONE" HeaderText="Descrizione">
									<headerstyle horizontalalign="Left" verticalalign="Middle"></headerstyle>
									<itemstyle horizontalalign="Left" verticalalign="Middle"></itemstyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="">
									<headerstyle horizontalalign="Center"></headerstyle>
									<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									<itemtemplate>
									<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("CODPOSIZIONE") %>' alt=""></asp:ImageButton>
									<asp:HiddenField runat="server" ID="hfCODPOSIZIONE" Value='<%# Eval("CODPOSIZIONE") %>' />
									</itemtemplate>
								</asp:TemplateField>
							</columns>
						</Grd:RibesGridView>
                    </td>
				</tr>
				<tr>
					<td>
						<iframe id="iframetabella" name="iframetabella" width="95%" src="../../aspVuota.aspx" frameborder="0"
							height="290"></iframe>
					</td>
				</tr>
			</table>
			<input id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza"> <input id="paginacomandi" type="hidden" name="paginacomandi">
			<asp:button id="btnNuovo" runat="server" style="DISPLAY: none"></asp:button>
		</form>
	</body>
</html>
