<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiRicercaAvanzata.aspx.vb" Inherits="Provvedimenti.ComandiRicercaAvanzata" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ComandiEnte</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
	</HEAD>
	<body class="SfondoGenerale" bottomMargin="0" bgColor="#ffcc66" leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
			<tr>
				<td style="WIDTH: 464px; HEIGHT: 18px" align="left"><asp:label id="lblTitolo" runat="server" Width="400px" CssClass="ContentHead_Title"></asp:label></td>
				<td align="right" width="800" colSpan="2" rowSpan="2">
					<input class="Bottone bottoneCreaFile" id="CreaFile" title="Estrazione 290" onclick="parent.Visualizza.Estrazione290()" type="button" name="CreaFile" style ="display:none">
					<input class="Bottone BottoneWord hidden" id="btnElaboraProvvedimenti" style ="display:none" title="Elaborazione Stampa Documenti" onclick="stampa();"  type="button" name="btnElabora">
					<!--<input class="Bottone BottoneWord" id="btnElaboraBollettini" style ="display:none" title="Elabora Stampa Bollettini Violazione ICI" onclick="parent.Visualizza.frames('loadGrid').frmSearchResults.btnElaboraBollettini.click();"  type="button" name="btnElabora">-->
					<INPUT class="Bottone BottoneExcel" id="Excel" title="Report Excel singoli avvisi" onclick="parent.Visualizza.StampaExcel()" type="button" name="Excel">
					<INPUT class="Bottone BottoneAggDate hidden" id="Massiva" title="Gestione aggiornamento Massivo Date" onclick="parent.Visualizza.Massiva()" type="button" name="Massiva">
					<INPUT class="Bottone BottoneCalendario" id="Date" title="Consente di attivare o disattivare il filtro per Data" onclick="parent.Visualizza.AttivaDIVDate()" type="button" name="Date">
					<INPUT class="Bottone BottoneRicerca" id="Search" title="Ricerca" onclick="parent.Visualizza.Search()" type="button" name="Search"> 
					<INPUT style="display:none" class="Bottone BottonePulisci hidden" id="Annulla" title="Pulisce i filtri utilizzati per la ricerca" onclick="parent.Visualizza.Clear()" type="button" name="Clear">
				</td>
			</tr>
			<tr>
				<td style="WIDTH: 463px" align="left"><asp:label id="info" CssClass="NormalBold_title" Width="524px" runat="server" Height="20px">Provvedimenti - Ricerca Avanzata Atti</asp:label></td>
			</tr>
		</table>
	</body>
</HTML>
