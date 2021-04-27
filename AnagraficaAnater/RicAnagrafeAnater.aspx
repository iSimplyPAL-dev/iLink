<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicAnagrafeAnater.aspx.vb" Inherits="OPENgov.RicAnagrafeAnater" %>
<html>
	<head>
		<%
	Dim sessionName
	sessionName = Request.item("sessionName")
%>
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
        <script type="text/javascript" src="../_js/Utility.js?newversion"></script>
		<script type="text/javascript" src="../_js/VerificaCampi.js?newversion"></script>
		<script type="text/vbscript" src="../_vbs/OperazioniSuCampi.vbs"></script>
		<script type="text/vbscript" src="../_vbs/ControlliFormali.vbs"></script>
		<script type="text/javascript">
		function Nuovo()
			{
				document.getElementById('btnNuovo').click();
			}
			
		
		function Search()
		{
		
			Parametri="?popup=<% = popup %>&cognome="+document.getElementById('txtCognome').value+"&nome="+document.getElementById('txtNome').value+"&codicefiscale="+document.getElementById('txtCodiceFiscale').value+"&partitaiva="+document.getElementById('txtPartitaIva').value+"&codcontribuente="+document.getElementById('txtCodContribuente').value+"&DARICONTROLLARE="+document.getElementById('chkDaRicontrollare').checked;
			loadGrid.location.href="SearchResultsAnagrafica.aspx"+Parametri
			return true;
		}
		
		function SearchReturn(popUp)
		{
			if (popUp=='1')
			{
				
				Parametri="?sessionName=<%=sessionName%>&cognome="+document.getElementById('txtCognome').value+"&nome="+document.getElementById('txtNome').value+"&codicefiscale="+document.getElementById('txtCodiceFiscale').value+"&partitaiva="+document.getElementById('txtPartitaIva').value+"&codcontribuente="+document.getElementById('txtCodContribuente').value+"&DARICONTROLLARE="+document.getElementById('chkDaRicontrollare').checked; 
				loadGrid.location.href="SearchResultsAnagraficaGenerale.aspx"+Parametri
			
			}
			else
			{
			
				Parametri="?cognome="+document.getElementById('txtCognome').value+"&nome="+document.getElementById('txtNome').value+"&codicefiscale="+document.getElementById('txtCodiceFiscale').value+"&partitaiva="+document.getElementById('txtPartitaIva').value+"&codcontribuente="+document.getElementById('txtCodContribuente').value+"&DARICONTROLLARE="+document.getElementById('chkDaRicontrollare').checked;
				loadGrid.location.href="SearchResultsAnagrafica.aspx"+Parametri
			}
			return true;
		}
		function PulisciCampi()
		{
			document.getElementById('txtCognome').value='';
			document.getElementById('txtNome').value='';
			document.getElementById('txtCodiceFiscale').value='';
			document.getElementById('txtCodiceFiscale').value='';
			document.getElementById('txtCodContribuente').value='';
			document.getElementById('txtPartitaIva').value = '';
			
			loadGrid.location.href="../aspVuota.aspx"
			document.getElementById('txtCognome').focus();
		}

		function keyPress()
		{
			if (window.event.keyCode==13)
			{
				// se è stato premuto il pulsante invio 
				Search();
			}
		}	

		</script>
	</head>
	<body class="Sfondo" bottomMargin="0" leftMargin="10" topMargin="10" onkeypress="keyPress();">
		<form id="Form1" runat="server" method="post">
			<table id="tabEsterna" cellSpacing="1" cellPadding="1" width="100%"
				align="center" border="0">
				<tr>
					<td>
						<fieldset class="classeFiledSetRicerca">
							<legend class="Legend">Inserimento parametri di Ricerca</legend>						
							<table cellSpacing="1" cellPadding="1" width="98%" align="center" border="0">
								<tr>
									<td class="Input_Label" width="25%">
										Cognome<br>
										<asp:textbox id="txtCognome" runat="server" size="50" maxLength="100" cssclass="Input_Text"></asp:textbox>
									</td>
									<td class="Input_Label" width="25%">
										Nome<br>
										<asp:textbox cssclass="Input_Text" runat="server" maxLength="50" size="50" id="txtNome" ></asp:textbox>
									</td>
								</tr>
								<tr>
									<td class="Input_Label" width="25%">
										Codice Fiscale<br>
										<asp:textbox id="txtCodiceFiscale" runat="server" size="50" maxLength="16" cssclass="Input_Text" ></asp:textbox>	
									</td>
									<td class="Input_Label" width="25%">
										Partita Iva<br>
										<asp:textbox id="txtPartitaIva" runat="server" size="50" maxLength="11" cssclass="Input_Text" ></asp:textbox>
									</td>
								</tr>
							<tr style="display:none;">
								<td class="Input_Label" width="25%">Codice Contribuente</td>
								<td class="Input_Label" width="25%">Anagrafiche da Ricontrollare</td>
							</tr>
							<tr style="display:none;">
								<td><asp:textbox id="txtCodContribuente" onkeyup="disableLetterChar(this);" runat="server" size="30"
										maxLength="11" cssclass="Input_Number_Generali" ></asp:textbox></td>
								<td><asp:checkbox id="chkDaRicontrollare" runat="server" Width="16" CssClass="Input_Text" Text=" "></asp:checkbox></td>
							</tr>
						</table>
						</fieldset>
						<br>
						<br>
						<table cellSpacing="0" cellPadding="1" width="98%" align="center" border="0">
							<tr>
								<td class="Input_Label_title" colSpan="4">Visualizzazione Anagrafiche Estratte</td>
							</tr>
							<tr>
								<td style="HEIGHT: 282px"><iframe class="bordoIframe" id="loadGrid" style="WIDTH: 100.13%; HEIGHT: 320px" src="../aspVuota.aspx"
										frameBorder="0" width="100%" height="300"></iframe>
								</td>
							</tr>
						</table>
						<input id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza">
						<asp:button id="btnNuovo" style="DISPLAY: none" runat="server"></asp:button></td>
				</tr>
			</table>
		</form>
	</body>
</html>
