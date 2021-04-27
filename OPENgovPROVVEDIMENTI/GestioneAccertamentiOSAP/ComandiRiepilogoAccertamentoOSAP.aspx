<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiRiepilogoAccertamentoOSAP.aspx.vb" Inherits="Provvedimenti.ComandiRiepilogoAccertamentoOSAP"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
    <title>ComandiRiepilogoAccertamentoOSAP</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
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
		<script>
			function AbilitaPulsanti(sRETTIFICATO){
				if (sRETTIFICATO=="1"){
					document.getElementById("btnSearch").style.display="none";
					document.getElementById("btnClose").style.display="";
				}else{
					document.getElementById("btnSearch").style.display="";
					document.getElementById("btnClose").style.display="none";
				}
			}
		</script>
	</head>
	<body class="SfondoGenerale">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td>
						<span id="infoEnte"><%=Session("DESCRIZIONE_ENTE").ToString()%></span>
						<br>
						<span id="info"><%=Session("DESC_TIPO_PROC_SERV") %>  -  Riepilogo Accertato</span>
					</td>
					<td align="right" >
						<input class="Bottone BottoneAnnulla" id="btnSearch" runat="server" title="Accerta una nuova posizione" onclick="parent.Visualizza.location.href='GestioneAccertamentiOSAP.aspx';return false;" type="button" name="btnSearch">
						<input class="Bottone BottoneAnnulla" id="btnClose" runat="server" title="Chiudi" onclick="parent.window.close();return false;" type="button" name="btnClose" >
					</td>
				</tr>
			</table>
    </form>
  </body>
</html>
