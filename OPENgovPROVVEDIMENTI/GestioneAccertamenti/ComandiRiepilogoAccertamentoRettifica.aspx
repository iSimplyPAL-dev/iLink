<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiRiepilogoAccertamentoRettifica.aspx.vb" Inherits="Provvedimenti.ComandiRiepilogoAccertamentoRettifica"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ComandiEnte</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
	</HEAD>
	<body class="SfondoGenerale" >
		<form id="Form1" runat="server" method="post">
			<table cellspacing="0" cellpadding="0" width="100%" align="right" border="0">
				<tr>
					<td>
						<span id="infoEnte"><%=Session("DESCRIZIONE_ENTE").ToString()%></span>
						<br>
						<span id="info"><%=Session("DESC_TIPO_PROC_SERV") %>  -  Riepilogo Accertato</span>
					</td>
					<td align="right" >
						<input class="Bottone BottoneAnnulla" id="btnClose" title="Chiudi" onclick="parent.window.close();" type="button" name="btnClose" >
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
