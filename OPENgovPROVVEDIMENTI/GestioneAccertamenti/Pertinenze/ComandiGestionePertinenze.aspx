<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiGestionePertinenze.aspx.vb" Inherits="Provvedimenti.ComandiGestionePertinenze" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ComandiEnte</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
	</HEAD>
	<body class="SfondoGenerale" bottomMargin="0" bgColor="#ffcc66"
		leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<table cellSpacing="0" cellPadding="0" width="216" align="left" border="0" style="WIDTH: 216px; HEIGHT: 46px">
			<tr>
				<td align="right" width="300" colSpan="2" rowSpan="2">
					<INPUT class="Bottone BottoneAssocia" id="btnAssocia" title="Associa gli immobili al soggetto"
						
						onclick="parent.Visualizza.document.getElementById('btnCercaImmobile').click()"
						type="button" name="btnAssocia"> <INPUT class="Bottone Bottoneannulla" id="Cancel" title="Esci" 
						onclick="parent.opener.resetPertinenza();parent.window.close()" tabIndex="6" type="button" name="Cancel">
				</td>
			</tr>
		</table>
	</body>
</HTML>
