<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiRicerca.aspx.vb" Inherits="OpenGovTerritorio.ComandiRicercaStrade" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
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
		<script>
		function VisualizzaLabel(){
			document.getElementById('infoEnte').innerText="<%=Session("DESCRIZIONE_ENTE")%>"
			parent.Comandi.info.innerText="Configurazione  -  Stradario  -  Ricerca"			
		}
		</script>
	</head>
	<body onload="VisualizzaLabel();" class="SfondoGenerale" bottomMargin="0" 
		leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td style="WIDTH: 464px; HEIGHT: 18px" align="left"><asp:label id="infoEnte" runat="server" Width="400px" CssClass="ContentHead_Title"></asp:label></td>
				<td align="right" width="800" colSpan="2" rowSpan="2">
					<INPUT class="Bottone BottoneNewInsert" id="NewInsert" title="Nuovo Inserimento" onclick="parent.parent.Visualizza.location.href='InsertStrade.aspx';parent.parent.Comandi.location.href='ComandiInserimento.aspx';" type="button" name="NewInsert">
					<INPUT class="Bottone Bottonericerca" id="Search" title="Ricerca" onclick="parent.Visualizza.document.getElementById('Search').click();" type="button" name="Search">&nbsp;
				</td>
			</tr>
			<tr>
				<td style="WIDTH: 463px" align="left"><asp:label id="info" runat="server" Width="400px" CssClass="NormalBold_title" Height="20px"></asp:label></td>
			</tr>
		</table>
	</body>
</html>
