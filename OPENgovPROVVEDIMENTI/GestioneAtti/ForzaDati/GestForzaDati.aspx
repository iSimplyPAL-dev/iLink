<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GestForzaDati.aspx.vb" Inherits="Provvedimenti.GestForzaDati" %>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>GestForzaDati</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/vbscript" src="../../../_vbs/OperazioniSuCampi.vbs"></script>
		<script type="text/vbscript" src="../../../_vbs/ControlliFormali.vbs"></script>
		<script type="text/javascript">
		function SaveDati()
		{
			if(VerificaCampi())
			{
				if (confirm('Si vogliono salvare le modifiche apportate all\'atto?'))
				{
				    document.getElementById('CmdSalva').click(); 
				}
			}				
		}

		function DeleteDati()
		{
			if (confirm('Si vuole eliminare il Provvedimento?'))
			{
			    document.getElementById('CmdDelete').click();
			}				
		}

		function VerificaCampi()
		{
			sMsg=""
			
			if (!IsBlank(document.getElementById('txtDataConfermaAvviso').value)) {
			    if (IsBlank(document.getElementById('txtDataGenerazione').value)) {
			        sMsg = sMsg + "[La Data di Conferma puo\' essere inserita solo se presente la Data di Generazione!]<br>";
			    }
			    else {
			        if (!doDateCheck(document.getElementById('txtDataConfermaAvviso'), document.getElementById('txtDataGenerazione'))) {
			            sMsg = sMsg + "[Data di Conferma Avviso minore di Data di Generazione Avviso!]<br>";
			        }
			    }
			}
			if (!IsBlank(document.getElementById('txtDataStampaAvviso').value)) {
			    if (IsBlank(document.getElementById('txtDataConfermaAvviso').value)) {
			        sMsg = sMsg + "[La Data di Stampa puo\' essere inserita solo se presente la Data di Conferma!]<br>";
			    }
			    else {
			        if (!doDateCheck(document.getElementById('txtDataStampaAvviso'), document.getElementById('txtDataConfermaAvviso'))) {
			            sMsg = sMsg + "[Data di Stampa Avviso minore di Data di Conferma Avviso!]<br>";
			        }
			    }
			}
			if (!IsBlank(document.getElementById('txtDataConsegnaAvviso').value))
			{
				if(IsBlank(document.getElementById('txtDataStampaAvviso').value))
				{
					sMsg=  sMsg + "[La Data di Consegna puo\' essere inserita solo se presente la Data di Stampa!]<br>" ; 
				}
				else
				{
				if (!doDateCheck(document.getElementById('txtDataConsegnaAvviso'),document.getElementById('txtDataStampaAvviso')))
					{
						sMsg=  sMsg + "[Data di Consegna Avviso minore di Data di Stampa Avviso!]<br>" ; 
					}
				}
			}
			if(!IsBlank(document.getElementById('txtDataNotificaAvviso').value))
			{
				if(IsBlank(document.getElementById('txtDataConsegnaAvviso').value))
				{
					sMsg=  sMsg + "[La Data di Notifica puo\' essere inserita solo se presente la Data di Consegna!]<br>" ; 
				}
				else
				{
					if (!doDateCheck(document.getElementById('txtDataNotificaAvviso'),document.getElementById('txtDataConsegnaAvviso')))
					{
						sMsg=  sMsg + "[Data di Notifica Avviso minore di Data di Consegna Avviso!]<br>" ; 
					}
				}
			}
			if(!IsBlank(document.getElementById('txtDataIrreperibile').value))
			{
				if(IsBlank(document.getElementById('txtDataNotifica').value))
				{
					sMsg=  sMsg + "[La Data di Irreperibililta\' il puo\' essere inserita solo se presente la Data di Notifica!]<br>" ; 
				}
				else
				{
				    if (!doDateCheck(document.getElementById('txtDataIrreperibile'), document.getElementById('txtDataNotifica')))
				  {
				        sMsg = sMsg + "[Data di Irreperibililta\' minore di Data di Notifica Avviso!]<br>";
				  }
				}
			}
			if(!IsBlank(document.getElementById('txtDataSospensioneAvvisoAutotutela').value))
			{
				if(IsBlank(document.getElementById('txtDataNotificaAvviso').value))
				{
					sMsg=  sMsg + "[La Data di Sospensione Avviso puo' essere inserita solo se presente la Data di Notifica!]<br>" ; 
				}
				else
				{
					if (!doDateCheck(document.getElementById('txtDataSospensioneAvvisoAutotutela'),document.getElementById('txtDataNotificaAvviso')))
					{
						sMsg=  sMsg + "[Data di Sospensione Avviso minore di Data di Notifica Avviso!]<br>" ; 
					}
				}
			}
			//''''''''''''''''''''''''''''''''''''''''''GESTIONE RICORSO PROVINCIALE'''''''''''''''''''''''''''''''''''''''''''''//
			if(!IsBlank(document.getElementById('txtDataRicorsoProvinciale').value))
			{
				if(IsBlank(document.getElementById('txtDataNotificaAvviso').value))
				{
					sMsg=  sMsg + "[La Data Ricorso Provinciale  puo' essere inserita solo se presente la Data di Notifica Avviso!]<br>" ; 
				}
				else
				{
					if (!doDateCheck(document.getElementById('txtDataRicorsoProvinciale'),document.getElementById('txtDataNotificaAvviso')))
					{
						sMsg=  sMsg + "[Data di Ricorso Provinciale Avviso minore di Data di Notifica Avviso!]<br>" ; 
					}
				}
			}
			if(!IsBlank(document.getElementById('txtSospensioneProvinciale').value))
			{
				if(IsBlank(document.getElementById('txtDataRicorsoProvinciale').value))
				{
					sMsg=  sMsg + "[La Data di Sospensione Comm. Tributaria Provinciale  puo' essere inserita solo se presente la Data di Ricorso Provinciale!]<br>" ; 
				}
				else
				{
					if (!doDateCheck(document.getElementById('txtSospensioneProvinciale'),document.getElementById('txtDataRicorsoProvinciale')))
					{
						sMsg=  sMsg + "[Data di Sospensione Provinciale Avviso minore di Data di Ricorso Provinciale Avviso!]<br>" ; 
					}
				}
			}
			if(!IsBlank(document.getElementById('txtSentenzaProvinciale').value))
			{
				if(!IsBlank(document.getElementById('txtSospensioneProvinciale').value))
				{
					if (!doDateCheck(document.getElementById('txtSentenzaProvinciale'),document.getElementById('txtSospensioneProvinciale')))
					{
						sMsg=  sMsg + "[Data di Sentenza Provinciale Avviso minore di Data di Sospensione Provinciale Avviso!]<br>" ; 
					}
				}
				else
				{
					if(IsBlank(document.getElementById('txtDataRicorsoProvinciale').value))
					{
						sMsg=  sMsg + "[La Data di Sentenza Provinciale puo' essere inserita solo se presente la Data di Ricorso Provinciale!]<br>" ; 
					}
					else
					{
						if(!doDateCheck(document.getElementById('txtSentenzaProvinciale'),document.getElementById('txtDataRicorsoProvinciale')))
						{
							sMsg=  sMsg + "[Data di Sentenza Provinciale Avviso minore di Data di Ricorso Provinciale Avviso!]<br>" ; 
						}
					}
				}											
			 }
			 if(!IsBlank(document.getElementById('txtNoteProvinciale').value)){
				if((IsBlank(document.getElementById('txtDataRicorsoProvinciale').value)) && (IsBlank(document.getElementById('txtSospensioneProvinciale').value)) &&(IsBlank(document.getElementById('txtSentenzaProvinciale').value)))
				{
					sMsg=  sMsg + "[Le Note/Motivazioni Provinciali possono essere inserite solamente se una delle date Provinciali è valorizzata!]<br>" ; 
				}
			 }
			//''''''''''''''''''''''''''''''''''''''''''GESTIONE RICORSO REGIONALE'''''''''''''''''''''''''''''''''''''''''''''//
			if(!IsBlank(document.getElementById('txtDataRicorsoRegionale').value))
			{
				if(IsBlank(document.getElementById('txtSentenzaProvinciale').value))
				{
					sMsg=  sMsg + "[La Data di presentazione Ricorso regionale puo' essere inserita solo se presente la Data di Sentenza Provinciale!]<br>" ; 
				}
				else
				{
					if (!doDateCheck(document.getElementById('txtDataRicorsoRegionale'),document.getElementById('txtSentenzaProvinciale')))
					{
						sMsg=  sMsg + "[Data di Ricorso Regionale Avviso minore di Data di Sentenza Provinciale Avviso!]<br>" ; 
					}
				}
			}
			if(!IsBlank(document.getElementById('txtSospensioneRegionale').value))
			{
				if(IsBlank(document.getElementById('txtDataRicorsoRegionale').value))
				{
					sMsg=  sMsg + "[La Data di Sospensione Comm. Tributaria Regionale  puo' essere inserita solo se presente la Data di Ricorso Regionale!]<br>" ; 
				}
				else
				{
					if (!doDateCheck(document.getElementById('txtSospensioneRegionale'),document.getElementById('txtDataRicorsoRegionale')))
					{
						sMsg=  sMsg + "[Data di Sospensione Regionale Avviso minore di Data di Ricorso Regionale Avviso!]<br>" ; 
					}
				}
			}
			if(!IsBlank(document.getElementById('txtSentenzaRegionale').value))
			{
				if(!IsBlank(document.getElementById('txtSospensioneRegionale').value))
				{
					if (!doDateCheck(document.getElementById('txtSentenzaRegionale'),document.getElementById('txtSospensioneRegionale')))
					{
						sMsg=  sMsg + "[Data di Sentenza Regionale Avviso minore di Data di Sospensione Regionale Avviso!]<br>" ; 
					}
				}
				else
				{
					if(IsBlank(document.getElementById('txtDataRicorsoRegionale').value))
					{
						sMsg=  sMsg + "[La Data di Sentenza Regionale puo' essere inserita solo se presente la Data di Ricorso Regionale!]<br>" ; 
					}
					else
					{
						if(!doDateCheck(document.getElementById('txtSentenzaRegionale'),document.getElementById('txtDataRicorsoRegionale')))
						{
							sMsg=  sMsg + "[Data di Sentenza Regionale Avviso minore di Data di Ricorso Regionale Avviso!]<br>" ; 
						}
					}
				}				
			}
			if(!IsBlank(document.getElementById('txtNoteRegionale').value)){
				if((IsBlank(document.getElementById('txtDataRicorsoRegionale').value)) && (IsBlank(document.getElementById('txtSospensioneRegionale').value)) &&(IsBlank(document.getElementById('txtSentenzaRegionale').value)))
				{    
					sMsg=  sMsg + "[Le Note/Motivazioni Regionali possono essere inserite solamente se una delle date Regionali è valorizzata!]<br>" ; 
				}
			 }
			//''''''''''''''''''''''''''''''''''''''''''GESTIONE RICORSO CASSAZIONE'''''''''''''''''''''''''''''''''''''''''''''//
			if(!IsBlank(document.getElementById('txtSospensioneCassazione').value))
			{
			    if (IsBlank(document.getElementById('txtSospensioneCassazione').value))
				{
				    sMsg = sMsg + "[La Data Ingiunzione puo\' essere inserita solo se presente la Data di Notifica!]<br>";
				}
				else
				{
				    if (!doDateCheck(document.getElementById('txtSospensioneCassazione'), document.getElementById('txtDataNotificaAvviso')))
					{
					    sMsg = sMsg + "[Data di Ingiunzione minore di Data di Notifica!]<br>";
					}
				}
			}
			if (!IsBlank(document.getElementById('txtNoteCassazione').value)) {
			    if ((IsBlank(document.getElementById('txtSospensioneCassazione').value))) {
			        sMsg = sMsg + "[Il Numero Ingiunzione può essere inserito solamente se la Data Ingiunzione è valorizzata!]<br>";
			    }
			}
			//''''''''''''''''''''''''''''''''''''''''''GESTIONE CONCILIAZIONE GIUDIZIALE'''''''''''''''''''''''''''''''''''''''/
			if(!IsBlank(document.getElementById('txtNoteConcGiudiz').value)){
				if((document.getElementById('ckConcGiudiz').checked ==false))
				{    
					sMsg=  sMsg + "[Le Note/Motivazioni Conciliazione possono essere inserite solamente se il flag Conciliazione Giudiziale è selezionato!]<br>" ; 
				}
			 }
			//''''''''''''''''''''''''''''''''''''''''''GESTIONE ACCERTAMENTI CON ADESIONE'''''''''''''''''''''''''''''''''''''''/
			 if(!IsBlank(document.getElementById('txtNoteAccertamenti').value)){
				if((document.getElementById('ckAccertamento').checked ==false))
				{    			    
					sMsg=  sMsg + "[Le Note/Motivazioni Accertamento possono essere inserite solamente se il flag Accertamenti con Adesione è selezionato!]<br>" ; 
				}
			 }
			 if(!IsBlank(document.getElementById('txtTermineRicorso').value)){
				if((document.getElementById('ckAccertamento').checked ==false) && ((document.getElementById('ddlEsitoAccertamenti').value!= 0) || (document.getElementById('ddlEsitoAccertamenti').value!= 1 )))
				{    			    
					sMsg=  sMsg + "[Il Nuovo Termine di Ricorso Accertamento può essere inserito solamente se il flag Accertamenti con Adesione è selezionato e se Esito ha valore Conferma oppure Rettifica!]<br>" ; 
				}
			 }
			 if(document.getElementById('ddlEsitoAccertamenti').value!= -1){
				if((document.getElementById('ckAccertamento').checked ==false))
				{    			    
					sMsg=  sMsg + "[L'Esito Accertamento può essere inserito solamente se il flag Accertamenti con Adesione è selezionato!]<br>" ; 
				}
			 }
			if((document.getElementById('ckAccertamento').checked ==true))
			{    			    
				if(document.getElementById('ddlEsitoAccertamenti').value == -1){
					sMsg=  sMsg + "[Se il flag Accertamenti con Adesione è selezionato è obbligatorio inserire il tipo Esito!]<br>" ; 
				}
			}
			if(!IsBlank(document.getElementById('txtTermineRicorso').value)){
				if ((document.getElementById('ddlEsitoAccertamenti').value == -1) || (document.getElementById('ddlEsitoAccertamenti').value == 2)){
					sMsg=  sMsg + "[E' possibile inserire il Termine di Ricorso solamente se Esito ha valore Conferma o Rettifica!]<br>" ; 
				}
			}
			//''''''''''''''''''''''''''''''''''''''''''GESTIONE ACCERTAMENTI CON ADESIONE'''''''''''''''''''''''''''''''''''''''/
			if (!IsBlank(sMsg)) 
			{ 
				strMessage ="Attenzione...<br>" 
				GestAlert('a', 'warning', '', '', strMessage + sMsg);
				return false; 
			}		
			return true; 
		}

		function getDateNow()
		{
			var d,s
			d= new Date();
			s="";
			
			if(d.getDate() <10)
			{
				s += "0" + d.getDate() + "/";
			}
			else
			{
				s += d.getDate() + "/";
			}
			if( (d.getMonth() +1)<10 )
			{
				s += "0" + (d.getMonth() +1)  + "/";	
			} 
			else
			{
				s += (d.getMonth() +1)  + "/";
			}
			s += d.getYear(); 
			
			return (s);
		}
		
		function getDate(cDate) 
		{
			var aTmp = cDate.split('/');
			var nDay = aTmp[0]; // `01'
			var nMonth = aTmp[1]; // `01'
			var nYear = aTmp[2]; // `2001'
			
			return parseInt( nYear+nMonth+nDay ); // diventa `20010101'
		}

		function doDateCheck(from, to) 
		{	
			if(getDate(from.value) >= getDate(to.value)) 
			{
				return true;
			}
			else 
			{
			  return false;
			}
		}
		</script>
	</HEAD>
	<body class="SfondoVisualizza" rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
            <table width="100%">
                <!--blocco dati contribuente-->
                <%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
                <tr id="TRPlainAnag">
                    <td>
                        <iframe id="ifrmAnag" runat="server" src="../../../aspVuota.aspx" style="height: 200px; width: 100%" frameborder="0" marginheight="0"></iframe>
                        <asp:HiddenField ID="hdIdContribuente" runat="server" Value="-1" />
                    </td>
                </tr>
                <tr id="TRSpecAnag">
                    <td>
			            <table class="dati_anagrafe_tarsu_blu" border="0" cellSpacing="0" cellPadding="2" width="98%"
				            height="102">
				            <tr>
					            <td style="HEIGHT: 44px"><asp:label id="lblCognomeNome" runat="server" Height="12px" Width="243px"></asp:label></td>
					            <td style="HEIGHT: 44px"><asp:label id="Label32" runat="server" Height="12px" Width="32px">CF/P.IVA:</asp:label><asp:label id="lblCfPiva" runat="server"></asp:label></td>
					            <td style="HEIGHT: 44px"><asp:label id="Label33" runat="server" Height="12px" Width="32px">SESSO:</asp:label><asp:label id="lblSesso" runat="server"></asp:label></td>
				            </tr>
				            <tr>
					            <td><asp:label id="Label34" runat="server" Height="12px" Width="131px">DATA DI NASCITA:</asp:label><asp:label id="lblDataNascita" runat="server"></asp:label></td>
					            <td colSpan="2"><asp:label id="Label35" runat="server" Height="12px" Width="159px">COMUNE DI NASCITA:</asp:label><asp:label id="lblComuneNascita" runat="server"></asp:label></td>
				            </tr>
				            <tr>
					            <td colSpan="3"><asp:label id="Label36" runat="server" Height="12px" Width="116px">RESIDENTE IN:</asp:label><asp:label id="lblResidenza" runat="server" Width="550px"></asp:label></td>
				            </tr>
			            </table>
                    </td>
                </tr>
			</table>
            <table border="0" cellSpacing="0" cellPadding="0" width="98%">
				<tr>
					<td>
						<hr class="hr" SIZE="1" width="100%">
					</td>
				</tr>
			</table>
			<div id="DISABILITA">
				<fieldset class="classeFiledSet"><legend class="Legend">Estremi Atto</legend>
					<table id="tablebb_2" border="0" cellPadding="0" width="98%">
						<tr>
							<td class="Input_Label"><asp:label id="lblNumeroProvvedimento" runat="server">N&#176;Provvedimento</asp:label></td>
							<td class="Input_Label"><asp:label id="lblAnno" runat="server">Anno</asp:label></td>
							<td class="Input_Label"><asp:label id="Label4" runat="server">Tributo</asp:label></td>
							<td class="Input_Label"><asp:label id="lblTipoTributo" runat="server">Tipo Provvedimento</asp:label></td>
							<td class="Input_Label" colSpan="3"><asp:label id="Label11" runat="server">Data Generazione</asp:label></td>
						</tr>
						<tr>
							<td><asp:textbox id="txtNumeroProvvedimento" runat="server" Width="100px" ReadOnly="True" CssClass="Input_Text_Enable_Red_Right"></asp:textbox></td>
							<td><asp:textbox id="txtAnno" runat="server" Width="50px" ReadOnly="True" CssClass="Input_Text_Enable_Red_Right OnlyNumber"></asp:textbox></td>
							<td><asp:textbox id="txtTributo" runat="server" Width="150px" ReadOnly="True" CssClass="Input_Text_Enable_Red_Left"></asp:textbox></td>
							<td><asp:dropdownlist id="DdlTipoProvvedimento" Width="400px" CssClass="Input_Text" Runat="server"></asp:dropdownlist></td>
							<td><asp:textbox id="txtDataGenerazione" runat="server" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:textbox></td>
						</tr>
					</table>
				</fieldset>
				<br>
				<div id="IMPORTI">
					<fieldset class="classeFiledSet"><legend class="Legend">Dettaglio Importi</legend>
						<table id="tablebb_3" border="0" cellPadding="0" width="98%">
							<tr>
								<td class="Input_Label"><asp:label id="lblImportoDifferenzaImposta" runat="server">Differenza Imposta</asp:label></td>
								<td class="Input_Label"><asp:label id="lblImportoSanzioni" runat="server">Sanzioni</asp:label></td>
								<td class="Input_Label"><asp:label id="lblImportoSanzRid" runat="server">Sanzioni Ridotto</asp:label></td>
								<td class="Input_Label"><asp:label id="lblImportoSanzNonRid" runat="server">Sanzioni Non Riducibili</asp:label></td>
							</tr>
							<tr>
								<td><asp:textbox id="txtImportoDifferenzaImposta" runat="server" Width="100px" CssClass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:textbox></td>
								<td><asp:textbox id="txtImportoSanzioni" runat="server" Width="100px" CssClass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:textbox></td>
								<td><asp:textbox id="txtImportoSanzRid" runat="server" Width="100px" CssClass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:textbox></td>
								<td><asp:textbox id="txtImportoSanzNonRid" runat="server" Width="100px" CssClass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:textbox></td>
							</tr>
							<tr>
								<td class="Input_Label"><asp:label id="lblImportoInteressi" runat="server">Interessi</asp:label></td>
								<td class="Input_Label"><asp:label id="lblAltroImporto" runat="server">Addizionali</asp:label></td>
								<td class="Input_Label"><asp:label id="lblImportoSpese" runat="server">Spese</asp:label></td>
							</tr>
							<tr>
								<td><asp:textbox id="txtImportoInteressi" runat="server" Width="100px" CssClass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:textbox></td>
								<td><asp:textbox id="txtAltroImporto" runat="server" Width="100px" CssClass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:textbox></td>
								<td><asp:textbox id="txtImportoSpese" runat="server" Width="100px" CssClass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:textbox></td>
							</tr>
							<tr>
								<td class="Input_Label"><asp:label id="Label1" runat="server">Dichiarato</asp:label></td>
								<td class="Input_Label"><asp:label id="Label2" runat="server">Versato</asp:label></td>
								<td class="Input_Label"><asp:label id="Label3" runat="server">Accertato</asp:label></td>
							</tr>
							<tr>
								<td><asp:textbox id="txtDichiarato" runat="server" Width="100px" CssClass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:textbox></td>
								<td><asp:textbox id="txtVersato" runat="server" Width="100px" CssClass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:textbox></td>
								<td><asp:textbox id="txtAccertato" runat="server" Width="100px" CssClass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:textbox></td>
							</tr>
							<tr>
								<td class="Input_Label"><asp:label id="lblImportoArrotondRid" runat="server">Arrotondamento Ridotto</asp:label></td>
								<td class="Input_Label"><asp:label id="lblImportoTotRid" runat="server">Totale Ridotto</asp:label></td>
								<td class="Input_Label"><asp:label id="lblImportoArrotond" runat="server">Arrotondamento</asp:label></td>
								<td class="Input_Label"><asp:label id="lblImportoTotale" runat="server">Totale</asp:label></td>
							    <td class="Input_Label"><asp:label id="Label51" runat="server">Ruolo Coattivo</asp:label></td>
							</tr>
							<tr>
								<td><asp:textbox id="txtImportoArrotondRid" runat="server" cssclass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 2);" width="130px"></asp:textbox></td>
								<td><asp:textbox id="txtImportoTotRid" runat="server" cssclass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 2);" width="130px"></asp:textbox></td>
								<td><asp:textbox id="txtImportoArrotond" runat="server" Width="100px" CssClass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:textbox></td>
								<td><asp:textbox id="txtImportoTotale" runat="server" Width="100px" CssClass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:textbox></td>
							    <td><asp:textbox id="txtImportoRuoloCoattivoICI" runat="server" Width="100px" CssClass="Input_Text_Right OnlyNumber" onkeypress="return NumbersOnly(event, true, false, 2);" ReadOnly="true"></asp:textbox></td>
							</tr>
						</table>
					</fieldset>
				</div>
			</div>
			<br>
			<div id="QUESTIONARIO">
				<fieldset class="classeFiledSet"><legend class="Legend">Gestione Date</legend>
					<table id="tablebb_5" border="0" cellPadding="0" width="98%">
						<tr>
							<td class="Input_Label"><asp:label id="Label18" runat="server">Data Conferma Avviso</asp:label></td>
							<td class="Input_Label"><asp:label id="Label19" runat="server">Data Stampa Avviso</asp:label></td>
							<td class="Input_Label"><asp:label id="lblDataConsegnaAvviso" runat="server">Data Consegna Avviso</asp:label></td>
							<td class="Input_Label"><asp:label id="Label8" runat="server">Data Notifica Avviso</asp:label></td>
						</tr>
						<tr>
							<td><asp:textbox onblur="txtDateLostfocus(this);VerificaData(this);" id="txtDataConfermaAvviso" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:textbox></td>
							<td><asp:textbox onblur="txtDateLostfocus(this);VerificaData(this);" id="txtDataStampaAvviso" runat="server" CssClass="Input_Text_Right TextDate"></asp:textbox></td>
							<td><asp:textbox onblur="txtDateLostfocus(this);VerificaData(this);" id="txtDataConsegnaAvviso" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:textbox></td>
							<td><asp:textbox onblur="txtDateLostfocus(this);VerificaData(this);" id="txtDataNotificaAvviso" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:textbox></td>
						</tr>
						<tr>
							<td class="Input_Label"><asp:label id="Label9" runat="server">Data Rettifica Avviso</asp:label></td>
							<td class="Input_Label"><asp:label id="Label10" runat="server">Data Annullamento Avviso</asp:label></td>
							<td class="Input_Label"><asp:label id="Label12" runat="server">Data Sosp. Avviso Autotutela</asp:label></td>
							<td class="Input_Label"><asp:label id="Label20" runat="server">Data Irreperibilità</asp:label></td>
						</tr>
						<tr>
							<td><asp:textbox id="txtDataRettificaAvviso" runat="server" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:textbox></td>
							<td><asp:textbox id="txtDataAnnullamentoAvviso" runat="server" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:textbox></td>
							<td><asp:textbox onblur="txtDateLostfocus(this);VerificaData(this);" id="txtDataSospensioneAvvisoAutotutela" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:textbox></td>
							<td><asp:textbox onblur="txtDateLostfocus(this);VerificaData(this);" id="txtDataIrreperibile" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:textbox></td>
						</tr>
						<tr>
							<td colSpan="4"><br>
								<fieldset class="classeFiledSet"><legend class="Legend">1° Grado di Giudizio</legend>
									<table border="0" width="100%">
										<tr>
											<td style="WIDTH: 20%" class="Input_Label" width="20%"><asp:label id="Label13" runat="server">Data Presentazione<br>Ricorso</asp:label></td>
											<td style="WIDTH: 20%" class="Input_Label" width="20%"><asp:label id="Label15" runat="server">Data Sosp.Comm.<br>Tributaria</asp:label></td>
											<td style="WIDTH: 20%" class="Input_Label" width="20%"><asp:label id="Label16" runat="server">Data Sentenza</asp:label></td>
											<td style="WIDTH: 40%" class="Input_Label" width="40%"><asp:label id="lblNoteProvinciale" runat="server">Note / Motivazioni</asp:label></td>
										</tr>
										<tr vAlign="top">
											<td style="WIDTH: 20%" width="20%"><asp:textbox onblur="txtDateLostfocus(this);VerificaData(this);" id="txtDataRicorsoProvinciale" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:textbox></td>
											<td style="WIDTH: 20%" width="20%"><asp:textbox onblur="txtDateLostfocus(this);VerificaData(this);" id="txtSospensioneProvinciale" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:textbox></td>
											<td style="WIDTH: 20%" width="20%"><asp:textbox onblur="txtDateLostfocus(this);VerificaData(this);" id="txtSentenzaProvinciale" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:textbox></td>
											<td style="WIDTH: 40%" width="40%"><asp:textbox id="txtNoteProvinciale" runat="server" Height="46px" Width="320px" CssClass="Input_Text" TextMode="MultiLine" MaxLength="2000"></asp:textbox></td>
										</tr>
									</table>
								</fieldset>
							</td>
						</tr>
						<tr>
							<td colSpan="4">
								<fieldset class="classeFiledSet"><legend class="Legend">Conciliazione Giudiziale</legend>
									<table border="0" width="100%" align="center">
										<tr>
											<td class="Input_Label"><asp:label id="lblFlagConcGiudiz" runat="server">&nbsp;</asp:label></td>
											<td class="Input_Label"><asp:label id="lblNoteConcGiudiz" runat="server">Note / Motivazioni</asp:label></td>
										</tr>
										<tr vAlign="top">
											<td class="Input_Label" width="15%"><asp:checkbox id="ckConcGiudiz" Text="Conciliazione Giudiziale" Runat="server"></asp:checkbox></td>
											<td class="Input_Label" width="50%"><asp:textbox id="txtNoteConcGiudiz" runat="server" Height="40px" Width="400px" CssClass="Input_Text" TextMode="MultiLine" MaxLength="2000"></asp:textbox></td>
										</tr>
									</table>
								</fieldset>
							</td>
						</tr>
						<tr>
							<td colSpan="4">
								<fieldset class="classeFiledSet"><legend class="Legend">2° Grado di Giudizio</legend>
									<table border="0" width="100%" align="left">
										<tr>
											<td style="WIDTH: 20%" class="Input_Label" width="20%"><asp:label id="Label17" runat="server">Data Presentazione<br>Ricorso</asp:label></td>
											<td style="WIDTH: 20%" class="Input_Label" width="20%"><asp:label id="Label21" runat="server">Data Sosp.Comm.<br>Tributaria</asp:label></td>
											<td style="WIDTH: 20%" class="Input_Label" width="20%"><asp:label id="Label22" runat="server">Data Sentenza</asp:label></td>
											<td style="WIDTH: 40%" class="Input_Label" width="40%"><asp:label id="lblNoteRegionale" runat="server">Note / Motivazioni</asp:label></td>
										</tr>
										<tr vAlign="top">
											<td style="WIDTH: 20%" width="20%"><asp:textbox onblur="txtDateLostfocus(this);VerificaData(this);" id="txtDataRicorsoRegionale" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:textbox></td>
											<td style="WIDTH: 20%" width="20%"><asp:textbox onblur="txtDateLostfocus(this);VerificaData(this);" id="txtSospensioneRegionale" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:textbox></td>
											<td style="WIDTH: 20%" width="20%"><asp:textbox onblur="txtDateLostfocus(this);VerificaData(this);" id="txtSentenzaRegionale" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:textbox></td>
											<td style="WIDTH: 40%" width="40%"><asp:textbox id="txtNoteRegionale" runat="server" Height="46px" Width="320px" CssClass="Input_Text" TextMode="MultiLine" MaxLength="2000"></asp:textbox></td>
										</tr>
									</table>
								</fieldset>
							</td>
						</tr>
						<tr>
				            <td colspan="4">
                                <fieldset class="classeFiledSet"><legend class="Legend">Accertamenti con Adesione</legend>
					                <table id="tabella" border="0" cellPadding="0" width="100%">
						                <tr>
							                <td style="WIDTH: 25%" class="Input_Label"><asp:label id="lblFlagAcc" runat="server">&nbsp;</asp:label></td>
							                <td style="WIDTH: 25%" class="Input_Label"><asp:label id="lblEsitoAcc" runat="server">Esito</asp:label></td>
							                <td style="WIDTH: 25%" class="Input_Label"><asp:label id="lblTermineRicorso" runat="server">Nuovo Termine di Ricorso</asp:label></td>
							                <td style="WIDTH: 25%" class="Input_Label"><asp:label id="lblNoteAccertamenti" runat="server">Note / Motivazioni</asp:label></td>
						                </tr>
						                <tr vAlign="top">
							                <td style="WIDTH: 15%" class="Input_Label" width="15%"><asp:checkbox id="ckAccertamento" Text="Accertamenti con Adesione" Runat="server"></asp:checkbox></td>
							                <td style="WIDTH: 15%" class="Input_Label" width="15%"><asp:dropdownlist id="ddlEsitoAccertamenti" Runat="server" CssClass="Input_Text">
									                <asp:ListItem Selected="True" Value="-1">...</asp:ListItem>
									                <asp:ListItem Value="0">Conferma</asp:ListItem>
									                <asp:ListItem Value="1">Rettifica</asp:ListItem>
									                <asp:ListItem Value="2">Annullamento</asp:ListItem>
								                </asp:dropdownlist></td>
							                <td style="WIDTH: 35%" class="Input_Label" width="35%"><asp:textbox onblur="txtDateLostfocus(this);VerificaData(this);" id="txtTermineRicorso" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="50"></asp:textbox></td>
							                <td style="WIDTH: 35%" class="Input_Label" width="35%"><asp:textbox id="txtNoteAccertamenti" runat="server" Height="46px" Width="300px" CssClass="Input_Text" TextMode="MultiLine" MaxLength="2000"></asp:textbox></td>
						                </tr>
					                </table>
					                <br>
				                </fieldset>
							</td>
						</tr>
						<tr>
                            <td colspan="4">
                                <fieldset class="classeFiledSet">
                                    <legend class="Legend">Ingiunzione</legend>
                                    <table border="0" width="100%" align="left">
                                        <tr>
                                            <td class="Input_Label" width="20%"><asp:Label ID="Label24" runat="server">Data</asp:Label></td>
                                            <td style="width: 40%" class="Input_Label" width="40%"><asp:Label ID="lblNoteCassazione" runat="server">Numero</asp:Label></td>
                                        </tr>
                                        <tr valign="top">
                                            <td width="20%"><asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);" ID="txtSospensioneCassazione" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                                            <td style="width: 40%" width="40%"><asp:TextBox ID="txtNoteCassazione" runat="server" Width="320px" Height="46px" CssClass="Input_Text" axLength="2000" TextMode="MultiLine"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
						<tr>
                            <td colspan="4">
                                <fieldset class="classeFiledSet">
                                    <legend class="Legend"></legend>
                                    <table border="0" width="100%" align="left">
                                        <tr>
							                <td class="Input_Label"><asp:label id="Label29" runat="server">Data Atto Definitivo</asp:label></td>
							                <td class="Input_Label"><asp:label id="Label49" runat="server">Data Ruolo (TARSU)</asp:label></td>
							                <td class="Input_Label"><asp:label id="Label50" runat="server">Data Ruolo Coattivo</asp:label></td>
						                </tr>
						                <tr>
							                <td><asp:textbox id="txtDataAttoDefinitivo" runat="server" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:textbox></td>
							                <td><asp:textbox id="txtDataRuoloOrdinario" runat="server" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:textbox></td>
							                <td><asp:textbox id="txtDataRuoloCoattivoICI" runat="server" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:textbox></td>
						                </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
					</table>
			        <div style="float:left;">
				        <asp:label id="lblNoteGenerali" runat="server" CssClass="Legend">Note Generali Atto</asp:label><br>
				        <asp:textbox id="txtNoteGenerali" runat="server" Height="46px" Width="700px" CssClass="Input_Text" TextMode="MultiLine" MaxLength="2000"></asp:textbox>
			        </div>
				</fieldset>
				<br>
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
			<!--TextBox di appoggio testo fisso -->
			<asp:textbox style="DISPLAY: none" id="txtCOD_CONTRIBUENTE" runat="server"></asp:textbox>
			<asp:textbox style="DISPLAY: none" id="txtID_PROVVEDIMENTO" runat="server"></asp:textbox>
			<asp:textbox style="DISPLAY: none" id="txtCOD_TRIBUTO" runat="server"></asp:textbox>
			<asp:textbox style="DISPLAY: none" id="txtTIPO_OPERAZIONE" runat="server"></asp:textbox>
			<asp:textbox style="DISPLAY: none" id="txtTIPO_PROCEDIMENTO" runat="server"></asp:textbox>
			<asp:textbox style="DISPLAY: none" id="txtTIPO_RICERCA" runat="server"></asp:textbox>
			<asp:textbox id="ID_PROVVEDIMENTO" style="DISPLAY: none" runat="server"></asp:textbox>
			<asp:button id="CmdSalva" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="CmdDelete" style="DISPLAY: none" runat="server"></asp:button>
		</form>
	</body>
</HTML>
