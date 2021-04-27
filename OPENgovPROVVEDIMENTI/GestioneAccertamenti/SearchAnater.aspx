<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchAnater.aspx.vb" Inherits="Provvedimenti.SearchAnater" %>

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
		<script type="text/javascript">

		function keyPress()
		{
			if(window.event.keyCode==13)
			{

				document.frmRicerca.btnSearchANATER.click()
			}
		}
			
		</script>
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
					<TD class="Input_Label" style="WIDTH: 123px; HEIGHT: 10px" width="123">Anno 
						Accertamento:</TD>
					<td><asp:label id="lblAnnoAccertamento" runat="server" CssClass="Input_Label"></asp:label></td>
				</TR>
			</table>
			<br>
			<br>
			<br>
			<FIELDSET class="classeFiledSet">
                <legend class="Legend">Inserimento parametri di Ricerca</legend>
				<TABLE style="WIDTH: 296px; HEIGHT: 40px" cellSpacing="1" cellPadding="1" width="296" align="left"
					border="0">
					<TR>
						<td class="Input_Label">Foglio</td>
						<td class="Input_Label">Numero</td>
						<td class="Input_Label">Subalterno</td>
						<td class="Input_Label">Codice Ricerca</td>
					</TR>
					<tr>
						<td><asp:textbox onkeydown="keyPress();" id="txtFoglio" tabIndex="1" runat="server" Width="48px"
								maxLength="100" cssclass="Input_Text"></asp:textbox></td>
						<td><asp:textbox onkeydown="keyPress();" id="txtNumero" tabIndex="2" runat="server" Width="48px"
								maxLength="50" cssclass="Input_Text"></asp:textbox></td>
						<td><asp:textbox onkeydown="keyPress();" id="txtSubalterno" tabIndex="3" runat="server" Width="48px"
								maxLength="50" cssclass="Input_Text"></asp:textbox></td>
						<td><asp:textbox onkeydown="keyPress();" id="txtCodiceRicerca" tabIndex="4" runat="server" Width="88px"
								maxLength="16" cssclass="Input_Text"></asp:textbox></td>
					</tr>
				</TABLE>
			</FIELDSET>
			<TABLE cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td>
						<FIELDSET class="classeFiledSetIframe"><legend class="Legend">Immobili da associare</legend>
							<TABLE cellPadding="0" width="100%" border="0">
								<tr>
									<td><iframe id="loadGrid" src="../../aspVuota.aspx" frameBorder="0" width="100%"
											height="320" style="WIDTH: 100.43%; HEIGHT: 272px"> </iframe>
									</td>
								</tr>
							</TABLE>
						</FIELDSET>
					</td>
				</tr>
			</TABLE>
			<br>
			<asp:button id="btnSearchANATER" runat="server" Text="btnCercaTerritorio" style="DISPLAY: none"></asp:button></TD></TR></TABLE>
		</form>
	</body>
</HTML>
