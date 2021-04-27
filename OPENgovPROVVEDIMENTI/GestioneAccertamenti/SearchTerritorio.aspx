<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchTerritorio.aspx.vb" Inherits="Provvedimenti.SearchTerritorio" %>
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
	</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="20" topMargin="5" rightMargin="0">
		<form id="Form1" runat="server" method="post">
			<table style="WIDTH: 728px; HEIGHT: 50px" cellSpacing="1" cellPadding="1" width="728" align="left"
				border="0">
				<TR>
					<TD class="Input_Label" style="WIDTH: 123px; HEIGHT: 10px" width="123">Contribuente:</TD>
					<td><asp:label id="lblContribuente" runat="server" CssClass="Input_Label"></asp:label></td>
				</TR>
				<TR>
					<TD class="Input_Label" style="WIDTH: 123px; HEIGHT: 10px" width="123">Anno Accertamento:</TD>
					<td><asp:label id="lblAnnoAccertamento" runat="server" CssClass="Input_Label"></asp:label></td>
				</TR>
			</table>
			<br>
			<br>
			<br>
			<FIELDSET class="classeFiledSet"><LEGEND class="Legend">Inserimento parametri di Ricerca
				</LEGEND>
				<TABLE style="WIDTH: 296px; HEIGHT: 40px" cellSpacing="1" cellPadding="1" width="296" align="left"
					border="0">
					<TR>
						<td class="Input_Label" style="WIDTH: 332px; HEIGHT: 10px" width="332">Foglio</td>
						<td class="Input_Label" style="WIDTH: 58px; HEIGHT: 10px" width="58">Numero</td>
						<td class="Input_Label" style="WIDTH: 60px; HEIGHT: 10px" width="60">Subalterno</td>
						<td class="Input_Label" style="WIDTH: 332px" width="332">Classe</td>
						<td class="Input_Label" style="WIDTH: 337px" width="337">Categoria</td>
					</TR>
					<tr>
						<td style="WIDTH: 332px; HEIGHT: 16px"><asp:textbox id="txtFoglio" tabIndex="1" runat="server" Width="48px" maxLength="100" cssclass="Input_Text"></asp:textbox></td>
						<td style="WIDTH: 58px; HEIGHT: 16px"><asp:textbox id="txtNumero" tabIndex="2" runat="server" Width="48px" maxLength="50" cssclass="Input_Text"></asp:textbox></td>
						<td style="WIDTH: 60px; HEIGHT: 16px" align="center"><asp:textbox id="txtSubalterno" tabIndex="3" runat="server" Width="48px" maxLength="50" cssclass="Input_Text"></asp:textbox></td>
						<td style="WIDTH: 332px"><asp:textbox id="txtClasse" tabIndex="4" runat="server" Width="48px" maxLength="16" cssclass="Input_Text"></asp:textbox></td>
						<td style="WIDTH: 337px"><asp:textbox id="txtCategoria" tabIndex="5" runat="server" Width="56px" maxLength="11" cssclass="Input_Text"></asp:textbox></td>
					</tr>
				</TABLE>
			</FIELDSET>
			<TABLE cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td>
						<FIELDSET class="classeFiledSetIframe"><LEGEND class="Legend">Immobili da associare</LEGEND>
							<TABLE cellPadding="0" width="100%" border="0">
								<tr>
									<td><iframe id="loadGrid" src="../../aspVuota.aspx" frameBorder="0" width="100%" height="320">
										</iframe>
									</td>
								</tr>
							</TABLE>
						</FIELDSET>
					</td>
				</tr>
			</TABLE>
			<br>
			<asp:button id="btnSearchTerritorio" runat="server" Text="btnCercaTerritorio" style="DISPLAY: none"></asp:button></TD></TR></TABLE>
		</form>
	</body>
</HTML>
