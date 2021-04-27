<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiRicercaAnagraficaDop.aspx.vb" Inherits="ComandiRicercaAnagraficaDop" %>
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
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script>
		    function VisualizzaLabel(){
			    parent.Comandi.infoEnte.innerText="<%=Session("DESCRIZIONE_ENTE")%>"
			    parent.Comandi.info.innerText="Anagrafica  -  Ricerca"
		    }    		
		</script>
	</HEAD>
	<body class="SfondoGenerale" bottomMargin="0" bgColor="#ffcc66" leftMargin="2" topMargin="6"
		onload="VisualizzaLabel();" rightMargin="2">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td style="WIDTH: 464px; HEIGHT: 18px" align="left">
					<asp:label id="infoEnte" CssClass="ContentHead_Title" Width="400px" runat="server"></asp:label>
				</td>
				<td align="right" width="800" colSpan="2" rowSpan="2">
					<input class="Bottone BottoneExcel" id="Excel" title="Stampa elenco Anagrafiche in formato Excel" onclick="parent.Visualizza.estraiExcel();" type="button" name="Excel" />
					<input class="Bottone BottoneSalva" id="Insert" title="Salva Aggiornamento Anagrafica"  onclick="parent.Visualizza.UpdateContrib()" type="button" name="Insert"> 
					<input class="Bottone Bottonericerca" id="Search" title="Ricerca" onclick="parent.Visualizza.Search(0)" type="button" name="Search">
				</td>
			</tr>
			<tr>
				<td style="WIDTH: 463px" align="left"><asp:label id="info" CssClass="NormalBold_title" Width="400px" runat="server" Height="20px"></asp:label></td>
			</tr>
		</table>
	</body>
</HTML>
