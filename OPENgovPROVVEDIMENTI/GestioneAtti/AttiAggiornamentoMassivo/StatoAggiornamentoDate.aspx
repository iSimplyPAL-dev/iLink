<%@ Page Language="vb" AutoEventWireup="false" enableViewStateMac="false" Codebehind="StatoAggiornamentoDate.aspx.vb" Inherits="Provvedimenti.StatoAggiornamentoDate"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>StatoElaborazione</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Utility.js?newversion"></SCRIPT>
		<script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></SCRIPT>
		<script type="text/vbscript" src="../../../_vbs/OperazioniSuCampi.vbs"></SCRIPT>
		<script type="text/vbscript" src="../../../_vbs/ControlliFormali.vbs"></SCRIPT>
	</HEAD>
	<body class="SfondoVisualizza" leftMargin="20" topMargin="5" rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<div id="ElaborazioneIncorso_1" style="DISPLAY: none; LEFT: 8px; WIDTH: 100%; TOP: 8px; HEIGHT: 120px">
				<table cellSpacing="0" cellPadding="0" width="98%" border="0">
						<tr>
							<td width="50%">
								<table class="SFONDO_TABELLA_TOTALI" cellSpacing="0" cellPadding="5" width="60%" border="0">
									<tr>
										<TD class="riga_menu" align="left" width="200"><asp:label id="Label37" runat="server" Width="195px" Height="12px">TOTALE AVVISI AGGIORNATI:</asp:label></TD>
										<TD class="riga_menu" style="HEIGHT: 10px" align="left"><asp:label id="lblImpTotAvvisiAggironati" runat="server" CssClass="riga_menu" Height="12px"></asp:label></TD>
									</tr>
								</table>
							</td>
						</tr>
                    </table>
                </div>
		</form>
	</body>
</HTML>
