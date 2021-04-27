<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaSemplice.aspx.vb" Inherits="Provvedimenti.RicercaSemplice" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Gestione Lettere</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Utility.js?newversion"></SCRIPT>
		<script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></SCRIPT>
		<script type="text/vbscript" src="../../../_vbs/OperazioniSuCampi.vbs"></SCRIPT>
		<script type="text/vbscript" src="../../../_vbs/ControlliFormali.vbs"></SCRIPT>
		<script type="text/javascript">
	
		//funzione chiamata dal validator

		function VerificaCampi()
		{
			/*sMsg=""
			if(IsBlank(document.getElementById('txtNominativo').value) && IsBlank(document.getElementById('txtNumeroProvvedimento').value) )
			{
				sMsg=  sMsg + "Gestione Atti - Ricerca Manuale\n[Inserire almeno un parametro di ricerca]\n" ; 
			}
		
			if(!IsBlank(document.getElementById('txtNominativo').value) && document.getElementById('txtHiddenCodContribuente').value=="-1")
			{
				sMsg=  sMsg + "Ricerca Manuale\n[il Contribuente non è stato caricato correttamente]\n" ; 
			}
			
			if (!IsBlank(sMsg)) 
			{ 
				strMessage ="Attenzione...\n\n I campi elencati sono obbligatori!\n\n" 
				alert(strMessage + sMsg);
				return false; 
			}		
			*/
			return true;
		
		}

		function Search()
		{ 
			if(VerificaCampi())
			{
					var Parametri=null;
					
					attesaGestioneAtti.style.display='';
					Parametri="";
					Parametri="?COGNOME="+document.getElementById('txtCognome').value;
					Parametri=Parametri+ "&NOME="+document.getElementById('txtNome').value;
				    Parametri=Parametri+ "&CODICEFISCALE="+document.getElementById('txtCodiceFiscale').value;
				    Parametri=Parametri+ "&PARTITAIVA="+document.getElementById('txtPartitaIva').value;
					Parametri=Parametri+ "&NUMEROPROVVEDIMENTO="+document.getElementById('txtNumeroProvvedimento').value;
					Parametri=Parametri+ "&CODTRIBUTO="+document.getElementById('DdlCodTributo').value;
				    
				    document.getElementById('loadGrid').src="SearchAtti.aspx"+Parametri
			
					return true;
				
				
				
			}
			else
			{
	
				return false;
			}
			
		}
		function Massiva()
		{
			
			if(document.getElementById('txtRicercaAttiva').value== "1")
			{	
				location.href ="../AttiAggiornamentoMassivo/AggiornamentoMassivoDate.aspx?TIPORICERCA=SEMPLICE";
			}
			else
			{
			    GestAlert('a', 'warning', '', '', 'Attivare La ricerca prima di aggiornare le date !');	
			}
			
		}	
		
		function AttivaDIV()
		{
			Riepilogativi.style.display='';
		}
		
		
		function ApriRicercaAnagrafe(nomeSessione)
		{ 
			winWidth=980 
			winHeight=680 
			myleft=(screen.width-winWidth)/2 
			mytop=(screen.height-winHeight)/2 - 40 
			Parametri="sessionName=" + nomeSessione 
			WinPopUpRicercaAnagrafica=window.open("../../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?"+Parametri,"","width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no") 
		}
		
		
		function pulisciCampi()
		{
			
			document.getElementById('txtNumeroProvvedimento').value="";
			
			document.getElementById('txtCognome').value="";
			document.getElementById('txtNome').value="";
			document.getElementById('txtCodiceFiscale').value="";
			document.getElementById('txtPartitaIva').value="";
			document.getElementById('txtRicercaAttiva').value="-1";
			
			document.getElementById('loadGrid').src="../../../aspVuota.aspx";
			attesaGestioneAtti.style.display='none';
			//Riepilogativi.style.display='none';
			
		}
		
		function Clear()
		{
			pulisciCampi();
		}
		
		
		function keyPress()
		{
			if(window.event.keyCode==13)
			{
			 if(!Search())
			 {
				
				return false;
			 }
			}
		}	
		</script>
	</HEAD>
	<body class="SfondoVisualizza" leftMargin="20" rightMargin="0" MS_POSITIONING="GridLayout"
		onload="document.getElementById('txtCognome').focus();">
		<form id="Form1" runat="server" method="post">
			<fieldset class="FiledSetRicerca"><legend class="Legend">Modalità di Ricerca Semplice Atti</legend>
				<table cellPadding="0" width="100%" align="center" border="0">
					<tr>
						<td>
							<table id="tablebb" cellPadding="0" width="100%" border="0">
								<tr>
									<td class="Input_Label" width="25%">Cognome</td>
									<td class="Input_Label" width="25%">Nome</td>
								</tr>
								<tr>
									<td><asp:textbox id="txtCognome" onkeydown="keyPress();" runat="server" size="50" maxLength="100"
											cssclass="Input_Text" tabIndex="1"></asp:textbox></td>
									<td><asp:textbox cssclass="Input_Text" onkeydown="keyPress();" runat="server" maxLength="50" size="50"
											id="txtNome" tabIndex="2"></asp:textbox></td>
								</tr>
								<tr>
									<td class="Input_Label" width="25%">Codice Fiscale</td>
									<td class="Input_Label" width="25%">Partita Iva</td>
								</tr>
								<tr>
									<td><asp:textbox id="txtCodiceFiscale" onkeydown="keyPress();" runat="server" size="50" maxLength="16"
											cssclass="Input_Text" tabIndex="3"></asp:textbox></td>
									<td><asp:textbox id="txtPartitaIva" onkeydown="keyPress();" runat="server" size="50" maxLength="11"
											cssclass="Input_Text" tabIndex="4"></asp:textbox></td>
								</tr>
								<tr>
									<TD class="Input_Label" width="25%">
										<asp:label id="lblNumeroProvvedimento" runat="server">Numero Atto</asp:label>
									</TD>
									<td>
										<asp:Label ID="LblCodTributo" Runat="server" CssClass="Input_Label">Cod.Tributo</asp:Label>
									</td>
								</tr>
								<tr>
									<TD>
										<asp:textbox id="txtNumeroProvvedimento" name="txtNumeroProvvedimento" onkeydown="keyPress();"
											tabIndex="4" runat="server" Width="224px" Enabled="true" CssClass="Input_Text" ToolTip="Numero del Provvedimento"></asp:textbox>
									</TD>
									<td>
										<asp:DropDownList ID="DdlCodTributo" Runat="server" CssClass="Input_Text" Width="100px"></asp:DropDownList>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</fieldset>
			<!--
			<br></br>
			<div id="Riepilogativi" style="DISPLAY: none">
				<fieldset class="classeFiledSet"><legend class="Legend">
						Riepilogativi
					</legend>
					<table cellPadding="0" width="100%" align="center" border="0">
						<tr>
							<TD class="Input_Label" width="25%">
								<asp:label id="lblNumeroTotaleAvvisi" runat="server">Numero Totale Provvedimenti
								</asp:label>
							</TD>
							<TD class="Input_Label" width="25%">
								<asp:label id="lbl" runat="server">Importo Totale Provvedimenti
								</asp:label>
							</TD>
						</tr>
						<tr>
							<TD width="25%">
								<asp:textbox CssClass="Input_Text_Enable_Red"   id="txtTotaleAvvisi" runat="server"  ReadOnly="True" Width="215px"></asp:textbox>
							</TD>
							<TD width="25%">
								<asp:textbox  CssClass="Input_Text_Enable_Red" id="txtImportoTotaleAvvisi" runat="server" ReadOnly="True" Width="215px"></asp:textbox>
							</TD>
						</tr>
						<tr>
							<TD class="Input_Label" width="25%" colspan="2">
								<asp:label id="Label1" runat="server">Importo Totale Provvedimenti al netto delle Rettifiche e degli Annulamenti
								</asp:label>
							</TD>
						</tr>
						<tr>
							<TD width="25%" colspan="2">
								<asp:textbox  CssClass="Input_Text_Enable_Red" id="txtTotaleRettifiche" runat="server"  ReadOnly="True" Width="215px"></asp:textbox>
							</TD>
						</tr>
					</table>
				</fieldset>
			</div>-->
			<table cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td>
						<fieldset class="classeFiledSetIframe"><legend class="Legend">Visualizzazione 
								Atti
							</legend>
							<table cellPadding="0" width="100%" border="0">
								<tr>
									<td><iframe id="loadGrid" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="450"> </iframe>
									</td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
			</table>
			<!--TextBox di appoggio testo fisso -->
			<asp:textbox id="hdTestoFisso" style="DISPLAY: none" runat="server" Height="300px" Width="500px"
				CssClass="Input_Text" TextMode="MultiLine"></asp:textbox>
            <div id="attesaGestioneAtti" runat="server" style="z-index: 101; position: absolute;display:none;">
                <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                <div class="BottoneClessidra">&nbsp;</div>
                <div class="Legend">Attendere Prego</div>
            </div>
            <div id="divDialogBox" class="col-md-12">
                <div class="modal-box">
                    <div id="divAlert" class="modal-alert">
                        <span class="closebtnalert" onclick="CloseAlert()">&times;</span>
                        <p id="pAlert">testo di esempio</p>
                        <input type="text" class="prompttxt"/>
                        <button id="btnDialogBoxeOK" class="confirmbtn Bottone BottoneOK" onclick="DialogConfirmOK();"></button>&nbsp;
                        <button id="btnDialogBoxeKO" class="confirmbtn Bottone BottoneKO" onclick="DialogConfirmKO();"></button>
                        <button id="CmdDialogConfirmOK" class="hidden" onclick="RaiseDialogConfirmOK();"></button>
                        <button id="CmdDialogConfirmKO" class="hidden" onclick="RaiseDialogConfirmKO();"></button>
                        <input type="hidden" id="hfCloseAlert" />
                        <input type="hidden" id="hfDialogOK" />
                        <input type="hidden" id="hfDialogKO" />
                    </div>
                </div>
                <input type="hidden" id="cmdHeight" value="0" />
            </div>
			<asp:textbox id="txtRicercaAttiva" style="DISPLAY: none" runat="server"></asp:textbox>
		</form>
	</body>
</HTML>
