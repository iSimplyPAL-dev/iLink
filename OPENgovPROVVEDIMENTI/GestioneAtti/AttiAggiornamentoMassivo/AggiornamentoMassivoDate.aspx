<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AggiornamentoMassivoDate.aspx.vb" Inherits="Provvedimenti.AggiornamentoMassivoDate" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>OPENgovProvvedimenti - Aggiornamento Massivo Date</title>
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
		function Back()
		{		
			if(document.getElementById('txtSALVATAGGIOINCORSO').value=="1")
			{
				GestAlert('a', 'warning', '', '', 'Elaborazione in Corso...\nAttendere...');
				return false;
			}		
			window.close(); 		    
		}
		function VerificaCampi()
		{
			    sMsg=""
				
				if(document.getElementById('DataAggiorna_1').checked==false && document.getElementById('DataAggiorna_2').checked==false
				   && document.getElementById('DataAggiorna_2').checked==false && document.getElementById('DataAggiorna_3').checked==false 
				   && document.getElementById('Aggiorna_1').checked==false && document.getElementById('Aggiorna_2').checked==false)
				{					
					sMsg=  sMsg + "[Effettuare almeno una scelta!]\n"; 									
				}
				else
				{
					if(document.getElementById('Aggiorna_1').checked)
					{
						if(IsBlank(document.getElementById('txtAggiornaIN').value))
						{
							GestAlert('a', 'warning', '', '', 'Data di Aggiornamento obbligatoria!'); 
							Setfocus(document.getElementById('txtAggiornaIN'));
							return false;
						}
					}
					else
					{
						if(!document.getElementById('Aggiorna_2').checked)
						{
							GestAlert('a', 'warning', '', '', 'Effettuare la scelta del tipo di aggiornamento [Cancellazione-Modifica]!'); 
							return false;
						}	
					}
					if(document.getElementById('DataAggiorna_1').checked==false && document.getElementById('DataAggiorna_2').checked==false
				       && document.getElementById('DataAggiorna_2').checked==false && document.getElementById('DataAggiorna_3').checked==false)
					{
						sMsg=  sMsg + "[Selezionare il tipo di data che si vuole aggiornare!]\n"; 	
					}						
				}
				if(document.getElementById('DataAggiorna_3').checked && document.getElementById('Aggiorna_2').checked)
				{
					sMsg=  sMsg + "[Non si può eliminare la Data di PERVENUTO IL!]\n"; 	
				}				
				if (!IsBlank(sMsg)) 
				{ 
					strMessage ="Attenzione...\n\n I campi elencati sono obbligatori!\n\n" 
					alert(strMessage + sMsg);
					return false; 
				}			
			return true; 			
		}		
		function pulisciCampi()
		{				
			
		}
	    function Clear()
		{
			pulisciCampi();
		}
		function AttivaDate(Data)
		{
			 IDData=Data.id;
			 
			 document.getElementById(IDData).disabled=false;
			 Setfocus(document.getElementById(IDData));			
		}
		function DissativaDate(Data)
		{			 
			 IDData=Data.id;
			 
			 document.getElementById(IDData).disabled=true;
			 document.getElementById(IDData).value='';			 
		}
		function Salva()
		{			
			if(VerificaCampi())
			{			
				document.getElementById('txtSALVATAGGIOINCORSO').value="1";
				
				attesaGestioneAtti_popup.style.display='';
				document.formRicercaAnagrafica.target ='loadGrid';
				document.formRicercaAnagrafica.action="StatoAggiornamentoDate.aspx"
			
				document.formRicercaAnagrafica.submit();
				document.formRicercaAnagrafica.target ="";
				document.formRicercaAnagrafica.action=""
				return true;
			}
			else
			{
			  return false ;
			}			
		}
		function keyPress()
		{
			if(window.event.keyCode==13)
			{
			 if(!Salva())
			 {
				
				return false;
			 }
			}
		}	
		</script>
		<style>
			BODY { OVERFLOW: hidden }
		</style>
</HEAD>
	<body class="SfondoVisualizza" leftMargin="20" rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">			
			<FIELDSET class="classeFiledSet"><LEGEND class="Legend">Gestione Atti - 
					Modalità di Aggiornamento Massivo Date
				</LEGEND>
				<table cellSpacing="0" cellPadding="2" width="100%" align="left" border="0">
				 <tr>
						<td align="right" colspan="2">
							<INPUT class="Bottone BottoneSalva" id="Salva_date" title="Aggiornamento Massivo Date" onclick="Salva()" type="button" name="Salva_date">
							<INPUT class="Bottone Bottoneannulla" id="Annulla" title="Chiude la finestra" onclick="Back();" type="button" name="Annulla">
						</td>
					</tr>
					<tr  >
					<td colspan="2">
						<hr class="hr" width="98%" SIZE="1">
					</td>
				</tr>
					<tr>
						<td class="Input_Label">					
							<INPUT id="DataAggiorna_1" tabIndex="1" type="radio" value="1" name="Data"> DATA 
							CONSEGNA AVVISO
							<br>
							<br>
							<INPUT id="DataAggiorna_2" tabIndex="2" type="radio" value="2" name="Data"> DATA 
							NOTIFICA AVVISO
							<br>
							<br>
							<INPUT id="DataAggiorna_3" tabIndex="3" type="radio" value="3" name="Data"> DATA 
							PERVENUTO IL
							<br>
							<br>
						</td>
						<td class="Input_Label">
							<FIELDSET class="classeFiledSet AllineaSinistra BackGround">
								<p>
									<INPUT id="Aggiorna_1" tabIndex="4" type="radio" value="1" name="Aggiorna" onclick="AttivaDate(document.getElementById('txtAggiornaIN'));">
									AGGIORNA IN:
									<asp:textbox id="txtAggiornaIN"  onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" tabIndex="5" runat="server" CssClass="Input_Text_Right TextDate" ToolTip="Data Aggiornamento" ></asp:textbox>
								</p>
								<INPUT id="Aggiorna_2" tabIndex="6" type="radio" value="2" name="Aggiorna" onclick="DissativaDate(document.getElementById('txtAggiornaIN'));">
								ELIMINA DATA
							</FIELDSET>
						</td>
					</tr>
				</table>
				<br>
				<br>
			</FIELDSET>
			<table cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td>
						<FIELDSET class="classeFiledSetIframe"><LEGEND class="Legend">Stato 
								Aggiornamento Avvisi
							</LEGEND>
							<table cellPadding="0" width="100%" border="0">
								<tr>
									<td><iframe id="loadGrid" name="loadGrid" src="../../../aspVuota.aspx" frameBorder="0"
											width="100%" height="150"> </iframe>
									</td>
								</tr>
							</table>
						</FIELDSET>
					</td>
				</tr>
			</table>
			<!--TextBox di appoggio testo fisso --><asp:textbox id="hdTestoFisso" style="DISPLAY: none" runat="server" Width="500px" CssClass="Input_Text"
				Height="300px" TextMode="MultiLine"></asp:textbox>
			<div id="attesaGestioneAtti_popup" style="DISPLAY: none;">&nbsp;
				<p>Aggiornamento in Corso....</p>
				<p><IMG alt="" src="../../../images/Clessidra.gif"></p>
				<p>Attendere Prego...</p>
				&nbsp;
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
			<asp:textbox id="txtTIPORICERCA" style="DISPLAY: none" runat="server"></asp:textbox>
			<asp:textbox id="txtSALVATAGGIOINCORSO" style="DISPLAY: none" runat="server"></asp:textbox>
		</form>
	</body>
</HTML>
