<%@ Page Language="vb" AutoEventWireup="false" Codebehind="cRicAnagrafeAnater.aspx.vb" Inherits="OPENgov.cRicAnagrafeAnater" %>
<!doctype HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
	<head>
		<title>ComandiEnte</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
		<script>
		function VisualizzaLabel(){
			parent.Comandi.infoEnte.innerText="<%=Session("DESCRIZIONE_ENTE")%>"
			parent.Comandi.info.innerText="Anagrafica Anater - Ricerca"
		}
		</script>
	</head>
	<body class="SfondoGenerale" bottomMargin="0" bgColor="#ffcc66" leftMargin="2" topMargin="6"
		onload="VisualizzaLabel();" rightMargin="2">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td style="WIDTH: 464px; HEIGHT: 18px" align="left"><asp:label id="infoEnte" CssClass="ContentHead_Title" Width="400px" runat="server"></asp:label></td>
				<td align="right" width="800" colSpan="2" rowSpan="2"><input class="Bottone Bottonericerca" id="Search" title="Ricerca" onclick="parent.Visualizza.Search()" type="button" name="Search">
					<!-- <input class="Bottone BottoneNewInsert" id="NewInsert" title="Nuovo Inserimento" style="BORDER-RIGHT: 1px outset; BORDER-TOP: 1px outset; BORDER-LEFT: 1px outset; WIDTH: 30px; BORDER-BOTTOM: 1px outset; HEIGHT: 30px"
						onclick="parent.Visualizza.Nuovo()" type="button" name="NewInsert">-->
					<input class="Bottone Bottonepulisci hidden" id="Clear" title="Pulisci videata per nuova ricerca" onclick="parent.Visualizza.PulisciCampi()" type="button" name="Clear">
					<input class="Bottone Bottoneannulla" runat="server" id="btnChiudi" type="button" title="Chiudi Finestra" onclick="parent.window.close();">
				</td>
			</tr>
			<tr>
				<td style="WIDTH: 463px" align="left"><asp:label id="info" CssClass="NormalBold_title" Width="400px" style="color:#ffffff;" runat="server" Height="20px"></asp:label></td>
			</tr>
		</table>
	</body>
</html>
