<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiRiepilogoAccertamento.aspx.vb" Inherits="Provvedimenti.ComandiRiepilogoAccertamento"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ComandiEnte</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end If%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
	</HEAD>
	<body class="SfondoGenerale" bottomMargin="0" bgColor="#ffcc66"
		leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td style="WIDTH: 464px; HEIGHT: 18px" align="left">
                    <span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
						<asp:Label id="lblTitolo" runat="server"></asp:Label>
					</span>
				</td>
				<td align="right" width="800" colSpan="2" rowSpan="2">
					<INPUT class="Bottone BottoneAnnulla" id="Annulla" title="Torna indietro" onclick="parent.Visualizza.document.getElementById('CmdBack').click();" type="button" name="Annulla">
				</td>
			</tr>
			<TR>
				<TD style="WIDTH: 463px" align="left">
					<span class="NormalBold_title" id="info" runat="server" style="WIDTH: 400px; HEIGHT: 20px;">Accertamenti -  Riepilogo Accertato</span>
				</TD>
			</TR>
		</table>
	</body>
</HTML>
