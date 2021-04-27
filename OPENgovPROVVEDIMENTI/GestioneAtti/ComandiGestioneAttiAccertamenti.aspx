<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiGestioneAttiAccertamenti.aspx.vb" Inherits="Provvedimenti.ComandiGestioneAttiAccertamenti" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
		<title>ComandiEnte</title>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%        End If%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
</head>
	<body class="SfondoGenerale">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td style="WIDTH: 464px; HEIGHT: 18px" align="left">
						<asp:label id="infoEnte" runat="server" Width="400px" CssClass="ContentHead_Title"></asp:label>
					</td>
					<td align="right" width="800" colSpan="2" rowSpan="2">
						<input class="Bottone BottoneBonifica" id="RipristinaOrdinario" title="Ripristina ORdinario" onclick="parent.Visualizza.RipristinaOrdinario()" type="button" name="RipristinaOrdinario">
						<input class="Bottone BottonePDF" id="VisualizzaDocumentoPDF" title="Visualizza documento Atto" onclick="parent.Visualizza.VisualizzaDocumentoPDF()" type="button" name="VisualizzaDocumentoPDF">
						<input class="Bottone BottoneForzaDati" id="ForzaDati" title="Forza Dati" onclick="parent.Visualizza.ForzaDati()" type="button" name="ForzaDati">
						<input class="Bottone BottoneRateizzazioni" id="Rateizzazioni" title="Rateizzazioni" onclick="parent.Visualizza.Rateizzazioni()" type="button" name="Rateizzazioni" style="DISPLAY:none"> 
						<input class="Bottone BottoneStampa" id="Stampa" title="Stampa il provvedimento selezionato" onclick="parent.Visualizza.StampaAccertamenti()" type="button" name="Stampa"> 
						<input class="Bottone BottoneBollettino" id="StampaBollettino" title="Stampa i bollettini del provvedimento selezionato" onclick="parent.Visualizza.StampaBollettini()" type="button" name="Stampa">
						<input class="Bottone BottoneCreaFile" id="RettificaAvviso" title="Rettifica Avviso" onclick="parent.Visualizza.RettificaAvviso()" type="button" name="RettificaAvviso">
						<input class="Bottone BottoneSalva" id="Salva" title="Salva Avviso" onclick="parent.Visualizza.SalvaLiquidazioni()" type="button" name="salva"> 
						<input class="Bottone Bottoneannulla" id="return" title="Torna alla videata di Ricerca" onclick="parent.Visualizza.GoBack()" type="button" name="return">
					</td>
				</tr>
				<tr>
					<td style="width: 463px" align="left">
						<asp:label id="info" CssClass="NormalBold_title" Width="524px" runat="server" Height="20px" ></asp:label>
					</td>
				</tr>
			</table>
			<asp:textbox id="txtPagina" style="display: none" runat="server"></asp:textbox>
		</form>
	</body>
</html>
