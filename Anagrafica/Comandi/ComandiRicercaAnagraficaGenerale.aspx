<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiRicercaAnagraficaGenerale.aspx.vb" Inherits="ComandiRicercaAnagraficaGenerale" %>
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
		function ControlloAssocia(){
			if (parent.Visualizza.loadGrid.formAnagrafica==undefined){
				alert("Per Associare un Soggetto Anagrafico, effettuare la ricerca, selezionare un Soggetto e premere il pulsante Associa.")
			}
			else{
			    if (parent.Visualizza.loadGrid.document.getElementById('txtCodContrib').value==''){
						alert("Selezionare un Soggetto Anagrafico dalla griglia e premere il pulsante Associa.")
					}
					else{
					parent.Visualizza.loadGrid.document.getElementById('btnAssocia').click()
					}
				
			}
		}
		function ControlloModifica(){
		
			if (parent.Visualizza.loadGrid.formAnagrafica==undefined){
				alert("Per andare in modifica su un Soggetto Anagrafico, effettuare la ricerca, selezionare un Soggetto e premere il pulsante Modifica.")
			}
			else{
			    if (parent.Visualizza.loadGrid.document.getElementById('txtCodContrib').value == '') {
						alert("Selezionare un Soggetto Anagrafico dalla griglia e premere il pulsante Modifica.")
					}
					else{
					parent.Visualizza.loadGrid.document.getElementById('btnModifica').click()
					}
				
			}	
		}
				</script>
		</HEAD>
		<body class="SfondoGenerale" bottomMargin="0" bgColor="#ffcc66" leftMargin="2" topMargin="6" onload="VisualizzaLabel();" rightMargin="2" marginheight="0" marginwidth="0">
				<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
						<tr>
								<td style="WIDTH: 567px; HEIGHT: 18px" align="left"><asp:label id="infoEnte" runat="server" Width="400px" CssClass="ContentHead_Title"></asp:label></td>
								<td align="right" width="800" colSpan="2" rowSpan="2">
										<INPUT class="Bottone Bottonepulisci hidden" id="Clear" title="Pulisci videata per nuova ricerca" onclick="parent.Visualizza.PulisciCampi()" type="button" name="Clear">
										<INPUT class="Bottone BottoneAssocia hidden" id="btnAssocia" title="Associa il Soggetto Anagrafico all'Articolo di Ruolo" onclick="ControlloAssocia();" type="button" name="btnAssocia">
										<INPUT class="Bottone Bottonemodifica" id="btnModifica" title="Vai alla Videata di Modifica del Soggetto Anagrafico" onclick="ControlloModifica();" type="button" name="btnModifica">
										<INPUT class="Bottone BottoneNewInsert" id="NewInsert" title="Inserisci nuovo Soggetto Anagrafico" onclick="parent.Visualizza.document.getElementById('btnNuovo').click()" type="button" name="NewInsert">
										<INPUT class="Bottone Bottonericerca" id="Search" title="Ricerca Soggetto Anagrafico" onclick="parent.Visualizza.Search(1)" type="button" name="Search">
										<INPUT class="Bottone Bottoneannulla" id="Cancel" title="Esci" onclick="parent.window.close()" tabIndex="6" type="button" name="Cancel">
								</td>
						</tr>
						<tr>
								<td style="WIDTH: 567px" align="left"><asp:label id="info" runat="server" Width="400px" CssClass="NormalBold_title" Height="20px"></asp:label></td>
						</tr>
				</table>
		</body>
</HTML>
