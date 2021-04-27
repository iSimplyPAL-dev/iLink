<%@ Page language="c#" Codebehind="ImmobileDettaglio.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.ImmobileDettaglio" EnableEventValidation="false" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>ImmobileDettaglio</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1"){%> 
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%}%> 
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
        <script type="text/javascript" src="../_js/ControlloData.js?newversion"></script>
		<script type="text/javascript" src="../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript" src="../_js/geocoding.js?newversion"></script>
		<script type="text/javascript">		
			function ControllaValore(){
				var messaggio='';
				var messaggioObbligatori='';
				var campi = '';
				var campiObbligatori = '';
				var fineMessaggio = '';
				var fineMessaggioObbligatori = '';		
				var PercPossesso = 0;
				var MesiPossesso = 0;
				var MesiRiduzione = 0;
				var MesiEsclusione = 0;
				//var NumeroOrdine = 0;
				var Foglio = '';
				var Caratteristica = '';
				var Numero = 0;
				var TipoUtilizzo='0';
				var iDummyDich=<%= this.iHasDummyDich %>;
				
				if (iDummyDich!=0)
				{
				    Percpossesso = document.getElementById('txtPercPossessoDummy').value;
				    PercPossesso =parseInt(document.getElementById('txtPercPossessoDummy').value);
				    MesiPossesso = parseInt(document.getElementById('txtMesiPossessoDummy').value);
				    MesiRiduzione = parseInt(document.getElementById('txtMesiRiduzioneDummy').value);
				    MesiEsclusione = parseInt(document.getElementById('txtMesiEsclusioneDummy').value);
				    //NumeroOrdine = document.getElementById('txtNumOrdine').value;
				    Foglio = document.getElementById('txtFoglioDummy').value;
				    Caratteristica = document.getElementById('ddlCaratteristicaDummy').value;
				    Numero = document.getElementById('txtNumeroDummy').value;
				    TipoUtilizzo=document.getElementById('ddlTipoUtilizzoDummy').value;
				    TipoPossesso = document.getElementById('ddlTipoPossessoDummy').value;
				}
				else{
				    Percpossesso = document.getElementById('txtPercPossessoDummy').value;
				    PercPossesso =parseInt(document.getElementById('txtPercPossessoNoDummy').value);
				    MesiPossesso = parseInt(document.getElementById('txtMesiPossessoNoDummy').value);
				    MesiRiduzione = parseInt(document.getElementById('txtMesiRiduzioneNoDummy').value);
				    MesiEsclusione = parseInt(document.getElementById('txtMesiEsclusioneNoDummy').value);
				    Foglio = document.getElementById('txtFoglioNoDummy').value;
				    Caratteristica = document.getElementById('ddlCaratteristicaNoDummy').value;
				    Numero = document.getElementById('txtNumeroNoDummy').value;
				    TipoUtilizzo=document.getElementById('ddlTipoUtilizzoNoDummy').value;
				    TipoPossesso = document.getElementById('ddlTipoPossessoDummy').value;
				}
				if (PercPossesso > 100){
					campi += '\n- Percentuale Possesso';
				}
				if (MesiPossesso > 12){
					campi += '\n- Mesi Possesso';					
				}
				if (MesiRiduzione > 12){					
					campi += '\n- Mesi Riduzione';
				}
				if (MesiEsclusione > 12){
					campi += '\n- Mesi Escl. o Esenz.';
				}
				/*if (NumeroOrdine == ''){
					campiObbligatori += '\n-Numero Ordine';
				}*/
				if (Foglio == ''){
					campiObbligatori += '\n-Foglio';
				}
				if (Caratteristica == '0'){
					campiObbligatori += '\n-Caratteristica';
				}
				
				if (Numero == ''){
					campiObbligatori += '\n-Numero';
				}
				//*** 20140509 - TASI ***
				if (TipoUtilizzo=='0'){
				    campiObbligatori += '\n-Tipo Utilizzo';
				}				
				if(TipoPossesso == '0')
				{
				    campiObbligatori += '\n-Tipo Possesso';
				}           
				if(Percpossesso == '')
				{
				    campiObbligatori += '\n-Percentuale Possesso';
				}  
				//*** ***
				//alert(campiObbligatori);
				//alert(campi);
				
				if (campi != '' || campiObbligatori != ''){
					if (campiObbligatori != ''){
						messaggioObbligatori = 'I seguenti campi sono obbligatori : ' + campiObbligatori
					}
					if (campi != ''){
						messaggio = '\nI seguenti campi hanno valore errato : ' + campi
					}
					alert(messaggioObbligatori + messaggio);
					return false;
				}
				return true;
			}		
			
			function SettaFlagAbitPrinc(objValore){
				var iDummyDich=<%= this.iHasDummyDich %>;
				
				if (iDummyDich!=0)
				{
				    if ((objValore != '')&&(objValore != '0')){
					    document.getElementById('chkAbitPrincipaleDummy').checked = true;
				    }else{
					    document.getElementById('chkAbitPrincipaleDummy').checked = false;
				    }
				}else{
				    if ((objValore != '')&&(objValore != '0')){
					    document.getElementById('chkAbitprincipaleNoDummy').checked = true;
				    }else{
					    document.getElementById('chkAbitprincipaleNoDummy').checked = false;
				    }
				}
			}
			
			function ApriDatiAggiuntivi(IdImmobile){
				if (IdImmobile != ''){
					//alert(IdImmobile);
					window.open('DatiAggiuntiviImmobile.aspx?IdImmobile='+IdImmobile, 'DatiAggiuntivi', 'top ='+ (screen.height - 400) / 2 +', left='+ (screen.width - 500) / 2 +' width=500,height=400, status=yes, toolbar=no,scrollbar=no, resizable=no');
				}
			}
			
			function ApriPertinenza(){
				var IDContrib=document.getElementById("hdIdContribuente").value
				
				var IdTestata = <% = this.IDTestata %>;
				var IdPertinenza = document.getElementById('txtCodPertinenza').value;
				var finestra = window.open('ImmobilePertinenza.aspx?IdOggetto='+ IdPertinenza +'&IdTestata='+ IdTestata +'&IDContribuente='+IDContrib ,'fPertinenza','top ='+ (screen.height - 400) / 2 +', left='+ (screen.width - 700) / 2 +' width=700,height=400, status=yes, toolbar=no,scrollbar=no, resizable=no');
				finestra.focus();
				return false;
			}
			
			function ApriStradario(){
				var CodEnte = '<% = Session["COD_ENTE"] %>';
                var Parametri = '';
				var TipoStrada = '';
				var CodStrada = document.getElementById('txtCodVia').value;
                var CodTipoStrada = '';
                var Frazione = '';
				var CodFrazione = '';
				var Strada='';
				var iDummyDich=<%= this.iHasDummyDich %>;
				
				if (iDummyDich!=0)
				    Strada = document.getElementById('txtViaDummy').value;
				else
				    Strada = document.getElementById('txtViaNoDummy').value;                
                
                Parametri += 'CodEnte='+CodEnte;
                Parametri += '&TipoStrada='+TipoStrada;
                Parametri += '&Strada='+Strada;
                Parametri += '&CodStrada='+CodStrada;
                Parametri += '&CodTipoStrada='+CodTipoStrada;
                Parametri += '&Frazione='+Frazione;
                Parametri += '&CodFrazione='+CodFrazione;
                Parametri += '&Stile=<% = Session["StileStradario"] %>';
                Parametri += '&FunzioneRitorno=RibaltaStrada'
                
				window.open('<% = this.UrlStradario %>?'+Parametri, 'fStradario', 'top =' + (screen.height - 500) / 2 + ', left=' + (screen.width - 700) / 2 + ' width=700,height=500, status=yes, toolbar=no,scrollbar=no, resizable=no');
			}
			
			function ApriTerritorio()
			{					
				var Parametri = '';
				var iDummyDich=<%= this.iHasDummyDich %>;
	        
				Parametri += 'CodEnte=<% = Session["CodEnte"] %>';
				Parametri += '&Appl=OPENGOVT';
				Parametri += '&User=<% = Session["username"] %>';
				Parametri += '&DescrEnte=<% =Session["DESCRIZIONE_ENTE"] %>';
				Parametri += '&Provenienza=OPENGOV';				
				if (iDummyDich!=0)
				{
				    Parametri += '&Foglio='+document.getElementById('txtFoglioDummy').value;
				    Parametri += '&Numero='+document.getElementById('txtNumeroDummy').value;
				    Parametri += '&Subalterno='+document.getElementById('txtSubalternoDummy').value;
                }else{
				    Parametri += '&Foglio='+document.getElementById('txtFoglioNoDummy').value;
				    Parametri += '&Numero='+document.getElementById('txtNumeroNoDummy').value;
				    Parametri += '&Subalterno='+document.getElementById('txtSubalternoNoDummy').value;
                }                
				winWidth=980
				winHeight=680
				myleft=(screen.width-winWidth)/2 
				mytop=(screen.height-winHeight)/2 - 40
				
				window.open('<% = this.UrlPopUpTerritorio %>?'+Parametri,'Territorio','width='+winWidth+',height='+winHeight+', status=yes, toolbar=no,top='+mytop+',left='+myleft+',Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no');
				return false;
			}
			
			function RibaltaStrada(objStrada)
			{
				// popolo il campo descrizione della via di residenza
				var strada
				var iDummyDich=<%= this.iHasDummyDich %>;
				
				if (objStrada.TipoStrada != '&nbsp;')
				{
					strada= objStrada.TipoStrada;
				}
				if (objStrada.Strada != '&nbsp;')
				{
					strada=strada+ ' ' + objStrada.Strada;
				}
				if (objStrada.Frazione!='CAPOLUOGO')
				{
					strada= strada+ ' ' + objStrada.Frazione;
				}
				/*strada=strada.replace('&#192;','À');*/
				strada = strada.replace(/&#(\d+);/g, function (m, n) { return String.fromCharCode(n); });
				
				document.getElementById('txtCodVia').value = objStrada.CodStrada;
				document.getElementById('TxtViaRibaltata').value = strada;
				if (iDummyDich!=0)
				    document.getElementById('txtViaDummy').value=strada;
				else
				    document.getElementById('txtViaNoDummy').value=strada;
			}
			
			//*** 20130304 - gestione dati da territorio ***
			function ShowRicUIAnater()
			{
				winWidth=850 
				winHeight=650 
				myleft=(screen.width-winWidth)/2 
				mytop=(screen.height-winHeight)/2 - 40 
				var MyParam="?IdContribuente="+document.getElementById('txtIDContrib').value
				MyParam+="&lblCognomeContr="+document.getElementById('lblCognomeContr').innerHTML
				MyParam+="&lblNomeContr="+document.getElementById('lblNomeContr').innerHTML
				MyParam+="&lblDataNascContr="+document.getElementById('lblDataNascContr').innerHTML
				MyParam+="&lblResidContr="+document.getElementById('lblResidContr').innerHTML
				MyParam+="&lblComuneContr="+document.getElementById('lblComuneContr').innerHTML
				WinPopFamiglia=window.open("Anater/RicercaImmobile.aspx"+MyParam,"","width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no") 
			}
			//*** ***

			function CalcolaTariffa(){
				var CampiObbligatori = '';
				var ddlCodRendita = null;
				var txtRendita=null;
				var ddlCategoriaCatastale=null;
				var ddlClasse=null;
				var ddlEstimo=null;
				var txtConsistenza=null;
				var iDummyDich=<%= this.iHasDummyDich %>;
				
				if (iDummyDich!=0)
				{
				    ddlCodRendita=document.getElementById('ddlCodiceRenditaDummy');
				    txtRendita=document.getElementById('txtRenditaDummy');
				    ddlCategoriaCatastale=document.getElementById('ddlCategoriaCatastaleDummy');
				    ddlClasse=document.getElementById('ddlClasseDummy');
				    ddlEstimo=document.getElementById('ddlEstimoDummy');
				    txtConsistenza=document.getElementById('txtConsistenzaDummy');
				}else{
				    ddlCodRendita=document.getElementById('ddlCodiceRenditaNoDummy');
				    txtRendita=document.getElementById('txtRenditaNoDummy');
				    ddlCategoriaCatastale=document.getElementById('ddlCategoriaCatastaleNoDummy');
				    ddlClasse=document.getElementById('ddlClasseNoDummy');
				    ddlEstimo=document.getElementById('ddlEstimoNoDummy');
				    txtConsistenza=document.getElementById('txtConsistenzaNoDummy');
				}
				// controllo i campi obbligatori per il calcolo della rendita
				if ((ddlCodRendita.options[ddlCodRendita.selectedIndex].text) == 'AF'){
					// i campi obbligatori sono ddlCodRendita, ddlEstimo, txtConsistenza
				    GestAlert('a', 'warning', '', '', 'Le aree edificabili prevedono solo il calcolo del valore.');
					txtRendita.value='0';
					return false;
					/*if (document.getElementById('ddlEstimo').value == ''){
						CampiObbligatori += '\n- Zona';
					}
					if (document.getElementById('txtConsistenza').value == ''){
						CampiObbligatori += '\n- Consistenza';
					}*/
				}else if ((ddlCodRendita.options[ddlCodRendita.selectedIndex].text) == 'LC'){
				    GestAlert('a', 'warning', '', '', 'Gli immobili con codice rendita LC prevedono solo il calcolo del valore.');
					txtRendita.value='0';
					return false;
				}else if ((ddlCodRendita.options[ddlCodRendita.selectedIndex].text) == 'TA'){
				    GestAlert('a', 'warning', '', '', 'Inserire manualmente il Reddito Domenicale nel campo rendita!');
					return false;
				}else{
					// controllo la validita dei campi : 
					//alert(document.getElementById('ddlCategoriaCatastale').value);
					if (ddlCategoriaCatastale.value == '0'){
						CampiObbligatori += '\n- Categoria Catastale';
					}
					if (ddlClasse.value == '0,00'){
						CampiObbligatori += '\n- Classe';
					}
					if (ddlEstimo.value == ''){
						CampiObbligatori += '\n- Zona';
					}
					if (txtConsistenza.value == ''){
						CampiObbligatori += '\n- Consistenza';
					}
				}
				
				if (ddlCodRendita.value == '0'){
					CampiObbligatori += '\n- Codice Rendita'
				}
				
				if (CampiObbligatori != ''){
				    GestAlert('a', 'warning', '', '', 'Attenzione,i seguenti campi sono obbligatori\nper il calcolo della rendita.'+CampiObbligatori);
					return false;
				}else{
					return true;
				}				
			}
			
			function CalcolaValoreImmobile(){
				var ddlCategoria = null;
				var Rendita = null;
				var ddlCodRendita =null;
				var Consistenza=null;
				var CampiObbligatori = '';
				var iDummyDich=<%= this.iHasDummyDich %>;
				
				if (iDummyDich!=0)
				{
				    ddlCategoria=document.getElementById('ddlCategoriaCatastaleDummy');
				    Rendita = document.getElementById('txtRenditaDummy').value;
				    ddlCodRendita = document.getElementById('ddlCodiceRenditaDummy');
				    Consistenza=document.getElementById('txtConsistenzaDummy').value;
				}else{
				    ddlCategoria=document.getElementById('ddlCategoriaCatastaleNoDummy');
				    Rendita = document.getElementById('txtRenditaNoDummy').value;
				    ddlCodRendita = document.getElementById('ddlCodiceRenditaNoDummy');
				    Consistenza=document.getElementById('txtConsistenzaNoDummy').value;
				}
				var Categoria = ddlCategoria.value;
				
				if (Categoria.substr(0,1)!= "D"){
					// calcolo il valore senza rivalutazione in base alla categoria
					if (Consistenza == ''){
						CampiObbligatori = '\n- Consistenza';
					}
				}
				if (Rendita == ''){
					CampiObbligatori = '\n- Rendita';
				}
				if (Categoria == '0'){
					CampiObbligatori = '\n- Categoria Catastale';
				}
				
				if (CampiObbligatori != ''){
				    GestAlert('a', 'warning', '', '', 'Attenzione, per il calcolo del valore immobile sono necessari i seguenti dati:' + CampiObbligatori);
					return false;
				}				
				
				if ((ddlCodRendita.options[ddlCodRendita.selectedIndex].text) == 'LC'){
				    GestAlert('a', 'warning', '', '', 'Inserire il valore Immobile manualmente!');
					//document.getElementById('txtValore').value = document.getElementById('txtRendita').value;
					return false;
				}
				
				return true;
				/*if (Categoria == 'A/10'){
					Rendita = Rendita.replace('.', '');
					Rendita = Rendita.replace(',', '.');
					
					//Rendita = parseFloat(Rendita);
					alert('Rendita = ' + Rendita);
					
					ValoreImmobile = Rendita * 50;
					alert(ValoreImmobile);
				}*/		
			}
			
			function RibaltaInAnater(iRetVal)
			{
				if (iRetVal==2)
				{
					//*** 20120704 - IMU ***
					if (!confirm('Si vuole aggiornare la posizione anagrafica di Anater rispetto a quella del verticale ICI/IMU?'))
						{					
							document.getElementById('txtUpdateAnagraficaValue').value="false";
						}
						else
						{	
							document.getElementById('txtUpdateAnagraficaValue').value="true";
						}
						//alert(document.formRicercaAnagrafica.btnRibaltaInAnater);
						document.getElementById('btnRibaltaInAnater').click(); 		
					}
				else if (iRetVal == 0){ // Anagrafe residente
					//*** 20120704 - IMU ***
					if (!confirm('Si vuole aggiornare la posizione anagrafica del verticale ICI/IMU rispetto a quella di Anater?'))
					{
						document.getElementById('txtUpdateAnagraficaValue').value='false';
					}else{
						document.getElementById('txtUpdateAnagraficaValue').value='true verticale';
					}
					document.getElementById('btnRibaltaInAnater').click();
				}else if (iRetVal == 4){
					//alert(iRetVal);
					alert('Attenzione, per il ribaltamento dell\'immobile sulla base dati Anater, \nè necessario che l\'Anagrafica abbia Codice Fiscale o Partita Iva valorizzata.');
				}else{
					document.getElementById('txtUpdateAnagraficaValue').value='false';
					document.getElementById('btnRibaltaInAnater').click(); 				
				}
			}
			
			function OpenPopUpRibaltamento(intControlloAnagrafica, bCambioDati, bAbbinamentoManuale)
			{			
				window.open('Anater/PopUpRibaltamento/OpzioniRibaltamento.aspx?intControlloAnagrafica='+intControlloAnagrafica+'&bCambioDati='+bCambioDati+'&boolAbbinamentoManuale='+bAbbinamentoManuale, 'DatiAggiuntivi', 'top ='+ (screen.height - 300) / 2 +', left='+ (screen.width - 500) / 2 +' width=500,height=300, status=yes, toolbar=no,scrollbar=no, resizable=no');
			}
			
			function DuplicaImmobile(){
				//document.getElementById('btnDuplica.click(); 	
				//alert('Duplica!!');
				document.getElementById('btnDuplica').click();
			}
			
			function nascondi(chiamante, oggetto, label) {
			    if (document.getElementById(oggetto).style.display == "") {
			        document.getElementById(oggetto).style.display = "none"
			        chiamante.title = "Visualizza " + label
			        chiamante.innerText = "Visualizza " + label
			    } else {
			        document.getElementById(oggetto).style.display = ""
			        chiamante.title = "Nascondi " + label
			        chiamante.innerText = "Nascondi " + label
			    }
			}
		</script>
	</head>
	<body class="Sfondo" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
		    <div class="SfondoGenerale" style="WIDTH: 100%; HEIGHT: 45px; OVERFLOW: hidden; padding:0;">
			    <table id="Table1" cellSpacing="0" cellPadding="0" border="0" width ="100%">
				    <tr>
					    <td>
						    <asp:label id="lblTitolo" runat="server"></asp:label><br />
						    <span id="info">
						    ICI/IMU - Dichiarazioni - Dettaglio immobile
						    </span>
					    </td>
					    <td align="right">
						    <input class="Bottone BottoneGIS" id="GIS" title="Visualizza GIS" onclick="document.getElementById('CmdGIS').click()" type="button" name="GIS" />
						    <!--*** 20131018 - DOCFA ***-->
						    <input class="Bottone BottoneHome" id="DOCFADet" title="DOCFA" onclick="document.getElementById('cmdDOCFADet').click();" type="button" name="DOCFADet" />							
						    <!--*** 20131003 - gestione atti compravendita ***-->
						    <input class="Bottone BottoneRibalta" id="Precarica" title="Precarica" onclick="document.getElementById('cmdPrecarica').click();" type="button" name="Precarica" />							
						    <input class="Bottone BottoneTerritorio" id="Territorio" title="Visualizza situazione Territorio" onclick="ApriTerritorio()" type="button" name="Territorio" /> 
						    <input class="Bottone BottoneDuplica" id="Duplica" title="Duplica l'immobile." onclick="DuplicaImmobile();" type="button" name="Duplica" />							
						    <input class="Bottone BottoneDatiAggiuntivi hidden DisableBtn" id="DatiAggiuntivi" title="Visualizza i dati aggiuntivi dell'immobile." onclick="document.getElementById('btnVisualizzaDettagli').click()" type="button" name="DatiAggiuntivi" /> 
						    <input class="Bottone BottoneSoggetti hidden DisableBtn" id="Contitolari" title="Visualizza i contitolari." onclick="document.getElementById('btnAggiungiContitolare').click()" type="button" name="Contitolari" />
						    <input class="Bottone BottoneApri" id="Unlock" onclick="document.getElementById('btnAbilita').click()" type="button" name="Unlock" title="Abilita i contolli per scrivere." /> 
						    <input class="Bottone BottoneSalva" id="Insert" title="Salva i dati dell'immobile." onclick="document.getElementById('btnSalva').click()" type="button" name="Insert" /> 
						    <input class="Bottone BottoneCancella" id="Delete" title="Cancella l'immobile dalla dichiarazione." onclick="document.getElementById('btnElimina').click()" type="button" name="Delete" />
						    <input class="Bottone BottoneAnnulla" id="Cancel" title="Torna alla dichiarazione." onclick="document.getElementById('btnIndietro').click()" type="button" name="Cancel" />
					    </td>
				    </tr>
			    </table>
		    </div>
		    &nbsp;
			<table id="tblCorpo" cellSpacing="1" cellPadding="1" width="100%" border="0">
				<!--blocco dati contribuente--><%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
				<tr id="TRPlainAnag">
				    <td colspan="2">
				        <iframe id="ifrmAnag" runat="server" src="../../aspVuota.aspx" style="height:200px; width:100%" frameborder="0" marginheight="0"></iframe>
				        <asp:HiddenField id="hdIdContribuente" runat="server" Value="-1" />
				    </td>
				</tr>
				<tr id="TRSpecAnag">
				    <td colspan="2">
				        <table width="100%">
					        <tr>
						        <td>
							        <asp:label id="lblDatiContrib" CssClass="lstTabRow" Width="100%" Runat="server">Dati Contribuente</asp:label>
						        </td>
					        </tr>
					        <tr>
						        <td borderColor="darkblue" width="100%">
							        <table id="tblDatiContribuente" cellSpacing="0" cellPadding="0" width="100%" border="1">
								        <tr>
									        <td borderColor="darkblue">
										        <table width="100%" cellSpacing="1" cellPadding="1">
											        <tr>
												        <td class="DettagliContribuente" width="160">Cognome/Rag. Sociale</td>
												        <td class="DettagliContribuente" align="left">
													        <asp:label id="lblCognomeContr" runat="server"></asp:label>
												        </td>
												        <td class="DettagliContribuente" width="110">&nbsp;&nbsp;Nome</td>
												        <td class="DettagliContribuente">
													        <asp:label id="lblNomeContr" runat="server"></asp:label>
												        </td>
											        </tr>
											        <tr>
												        <td class="DettagliContribuente">Data di Nascita</td>
												        <td class="DettagliContribuente" align="left">
													        <asp:label id="lblDataNascContr" runat="server"></asp:label>
												        </td>
												        <td>&nbsp;</td>
												        <td>&nbsp;</td>
											        </tr>
											        <tr>
												        <td class="DettagliContribuente">RESIDENTE IN</td>
												        <td class="DettagliContribuente" align="left">
													        <asp:label id="lblResidContr" runat="server"></asp:label>
												        </td>
												        <td class="DettagliContribuente">&nbsp; Comune (Prov.)</td>
												        <td class="DettagliContribuente" align="left">
													        <asp:label id="lblComuneContr" runat="server"></asp:label>
												        </td>
											        </tr>
											        <tr>
												        <td class="DettagliContribuente">ANNO DICHIARAZIONE</td>
												        <td class="DettagliContribuente" align="left">
													        <asp:label id="lblAnnDichiaraz" runat="server"></asp:label>
												        </td>
												        <td class="DettagliContribuente">&nbsp;</td>
												        <td class="DettagliContribuente" align="left">&nbsp;</td>
											        </tr>
										        </table>
									        </td>
								        </tr>
							        </table>
						        </td>
					        </tr>
				        </table>
				    </td>
				</tr>
				<!--*** 20131003 - gestione atti compravendita ***-->
				<tr>
					<td colspan="2">
						<div id="AttoCompraVendita" style="DISPLAY:none" runat="server">
                            <div class="lstTab">
                                <fieldset class="bordoIFRAME">
								    <legend class="Legend">Compravendita - nota trascrizione</legend>
								    &nbsp;<asp:Label ID="lblNotaTrascrizione" runat="server" Text="" CssClass="Input_Label"></asp:Label>
								    <br />
								    &nbsp;
							    </fieldset>
							    <fieldset class="bordoIFRAME">
                                    <legend class="Legend">Immobile in nota</legend>
								    <p>&nbsp;<asp:Label ID="lblRifNota" runat="server" Text="" CssClass="Input_Label"></asp:Label></p>
								    <p>&nbsp;<asp:Label ID="lblCatNota" runat="server" Text="" CssClass="Input_Label"></asp:Label></p>
								    <p>&nbsp;<asp:Label ID="lblUbicazioneNota" runat="server" Text="" CssClass="Input_Label"></asp:Label>
									    &nbsp;<asp:Label ID="lblUbicazioneCatasto" runat="server" Text="" CssClass="Input_Label"></asp:Label></p>
							    </fieldset>
							    <fieldset class="bordoIFRAME">
                                    <legend class="Legend">Soggetto in nota</legend>
								    &nbsp;<asp:Label ID="lblSoggettoNota" runat="server" Text="" CssClass="Input_Label"></asp:Label>
							    </fieldset>
						    </div>
                        </div>
					</td>
				</tr>
				<!--*** ***-->
				<tr>
				    <td colspan="2">
				        <div id="DummyDich">
				            <table>
							    <!-- Dati Immobile -->
							    <tr>
								    <td colspan="7">
									    <asp:label id="lblDatiImmDummy" runat="server" CssClass="lstTabRow" Width="100%">Dati immobile</asp:label>
								    </td>
							    </tr>
							    <tr>
								    <td>
									    <asp:label id="lblCaratteristicaDummy" runat="server" CssClass="Input_Label">Caratteristica</asp:label>
									    <asp:label id="lblAstCaratteristicaDummy" style="COLOR: red; font-size: 11px" Runat="server">*</asp:label><br />
									    <asp:dropdownlist id="ddlCaratteristicaDummy" runat="server" CssClass="Input_Text" AutoPostBack="True" onselectedindexchanged="ddlCaratteristica_SelectedIndexChanged"></asp:dropdownlist>&nbsp; 
									    &nbsp;&nbsp;
								    </td>
								    <td>
									    <asp:label id="lblCodRenditaDummy" runat="server" CssClass="Input_Label">Codice Rendita</asp:label><br />
									    <asp:dropdownlist id="ddlCodiceRenditaDummy" runat="server" CssClass="Input_Text" AutoPostBack="True" onselectedindexchanged="ddlCodiceRendita_SelectedIndexChanged"></asp:dropdownlist>
								    </td>
								    <td>
									    <asp:label id="lblDataInizioDummy" CssClass="Input_Label" Runat="server">Data Inizio</asp:label><br />
									    <asp:textbox id="txtDataInizioDummy" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);TrackBlurGenerale(this);" CssClass="Input_Text_Right TextDate" Runat="server"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblDataFineDummy" CssClass="Input_Label" Runat="server">Data Fine</asp:label><br />
									    <asp:textbox id="txtDataFineDummy" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);TrackBlurGenerale(this);" CssClass="Input_Text_Right TextDate" Runat="server"></asp:textbox>
								    </td>
							    </tr>
							    <tr>
								    <td colspan="2">
									    <asp:label id="lblCodViaDummy" runat="server" CssClass="Input_Label">Via</asp:label>&nbsp; 
									    <!--*** 20130304 - gestione dati da territorio ***-->
									    <asp:imagebutton id="LnkNewUIAnaterDummy" runat="server" ImageUrl="../images/Bottoni/Listasel.png" CausesValidation="False" imagealign="Bottom"></asp:imagebutton>&nbsp; 
									    <!--*** ***-->
									    <asp:Label ID="lblViaOldDummy" Runat="server" CssClass="Input_Label"></asp:Label>&nbsp;<br />
									    <asp:textbox id="txtViaDummy" style="TEXT-ALIGN: left" runat="server" CssClass="Input_Text" Width="400px" MaxLength="1000" ReadOnly="true"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblNumeroCivicoDummy" runat="server" CssClass="Input_Label">Num. Civico</asp:label><br />
									    <asp:textbox id="txtNumCivDummy" onkeypress="return NumbersOnly(event);" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="65px"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblEspCivicoDummy" runat="server" CssClass="Input_Label">Esp. Civico</asp:label><br />
									    <asp:textbox id="txtEspCivicoDummy" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Text" Width="65px"></asp:textbox>
									    <asp:label id="lblBarraDummy" style="DISPLAY: none" runat="server" CssClass="Input_Label">/</asp:label>&nbsp;
									    <asp:textbox id="txtBarratoDummy" style="DISPLAY: none" runat="server" CssClass="Input_Text" Width="30px" MaxLength="3"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblScalaDummy" runat="server" CssClass="Input_Label">Scala</asp:label><br />
									    <asp:textbox id="txtScalaDummy" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Text" Width="65px" MaxLength="3"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblPianoDummy" runat="server" CssClass="Input_Label">Piano</asp:label><br />
									    <asp:textbox id="txtPianoDummy" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Text" Width="65px" MaxLength="2"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblInternoDummy" runat="server" CssClass="Input_Label">Interno</asp:label><br />
									    <asp:textbox id="txtInternoDummy" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Text" Width="65px" MaxLength="5"></asp:textbox>
								    </td>
							    </tr>
							    <!-- Dati Catastali -->
							    <tr>
								    <td colspan="7">
									    <asp:label id="lblDatiCatDummy" runat="server" CssClass="lstTabRow" Width="100%">Dati Catastali</asp:label>
								    </td>
							    </tr>
							    <tr>
								    <td>
									    <asp:label id="lblSezioneDummy" runat="server" CssClass="Input_Label"> Sezione</asp:label><br />
									    <asp:textbox id="txtSezioneDummy" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Text" Width="65px" MaxLength="3"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblFoglioDummy" runat="server" CssClass="Input_Label">Foglio</asp:label>
									    <asp:label id="lblAstFoglioDummy" style="COLOR: red; FONT-SIZE: 11px" Runat="server">*</asp:label><br />
									    <asp:textbox id="txtFoglioDummy" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Text" Width="65px" MaxLength="5" Text='<%# DataBinder.Eval(Container, "DataItem.Foglio") %>'> </asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblNumeroDummy" runat="server" CssClass="Input_Label">Numero</asp:label>
									    <asp:label id="lblAstNumeroDummy" style="COLOR: red; FONT-SIZE: 11px" runat="server">*</asp:label><br />
									    <asp:textbox id="txtNumeroDummy" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Text" Width="65px" MaxLength="5"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblSubalternoDummy" runat="server" CssClass="Input_Label"> Subalterno</asp:label><br />
									    <asp:textbox id="txtSubalternoDummy" onkeypress="return NumbersOnly(event);" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="65px" MaxLength="4"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblCategoriaCatastaleDummy" runat="server" CssClass="Input_Label">Cat. Catastale</asp:label><br />
									    <asp:dropdownlist id="ddlCategoriaCatastaleDummy" runat="server" CssClass="Input_Text" width="88px"></asp:dropdownlist>
								    </td>
								    <td>
									    <asp:label id="lblClasseDummy" runat="server" CssClass="Input_Label">Classe</asp:label><br />
									    <asp:dropdownlist id="ddlClasseDummy" runat="server" CssClass="Input_Text" width="72px"></asp:dropdownlist>
								    </td>
							    </tr>
							    <tr>
								    <td>
									    <asp:label id="lblComboEstimoDummy" runat="server" CssClass="Input_Label">Zona</asp:label><br />
									    <asp:dropdownlist id="ddlEstimoDummy" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Text" Width="100px"></asp:dropdownlist>
								    </td>
								    <td>
									    <asp:label id="lblConsistenzaDummy" runat="server" CssClass="Input_Label">Consistenza</asp:label><br />
									    <asp:textbox id="txtConsistenzaDummy" onkeypress="return NumbersOnly(event, true, false, 2);" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="100px" MaxLength="20"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblRenditaDummy" runat="server" CssClass="Input_Label">Rendita</asp:label>&nbsp;
									    <asp:linkbutton id="lnkRenditaDummy" Runat="server" CssClass="Input_Label" title="Calcola Rendita" onclick="lnkRendita_Click">&raquo;</asp:linkbutton><br />
									    <asp:textbox id="txtRenditaDummy" onkeypress="return NumbersOnly(event, true, false, 2);" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="100px" MaxLength="20"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblValoreImmobileDummy" runat="server" CssClass="Input_Label">Valore Immobile</asp:label>&nbsp;
									    <asp:linkbutton id="lnkValoreDummy" Runat="server" CssClass="Input_Label" title="Calcola Valore" onclick="lnkValore_Click">&raquo;</asp:linkbutton><br />
									    <asp:textbox id="txtValoreDummy" onkeypress="return NumbersOnly(event, true, false, 2);" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="100px" MaxLength="20"></asp:textbox>
								    </td>
								    <td>
									    <asp:checkbox id="chkValoreProvvisorioDummy" runat="server" CssClass="Input_Label" AutoPostBack="True" text="Valore Provvisorio" oncheckedchanged="chkValoreProvvisorio_CheckedChanged"></asp:checkbox>
								    </td>
								    <td>
									    <asp:checkbox id="chkStoricoDummy" runat="server" CssClass="Input_Label" text="Storico"></asp:checkbox>
								    </td>
								    <td>
									    <asp:checkbox id="chkExRuraleDummy" runat="server" CssClass="Input_Label" text="Ex-rurale"></asp:checkbox>
								    </td>
							    </tr>
							    <!-- Dati Possesso -->
							    <tr>
								    <td colspan="7">
									    <asp:label id="lblDatiPosDummy" runat="server" CssClass="lstTabRow" Width="100%">Dati possesso</asp:label>
								    </td>
							    </tr>
							    <tr>
								    <td>
									    <asp:label id="lblPercPossessoDummy" runat="server" CssClass="Input_Label"> Percentuale Possesso</asp:label>
                                        <span id="lblAstPercPossessoDummy" style="COLOR: red; font-size: 11px">*</span><br/>
									    <asp:textbox id="txtPercPossessoDummy" onkeypress="return NumbersOnly(event, true, false, 2);" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="65px" MaxLength="6"></asp:textbox>
								    </td>
								    <td >
									    <asp:label id="lblMesiPossessoDummy" runat="server" CssClass="Input_Label">Mesi Possesso</asp:label><br />
									    <asp:textbox id="txtMesiPossessoDummy" onkeypress="return NumbersOnly(event);" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="65px" MaxLength="2"></asp:textbox>
								    </td>
                                    <!--*** 20140509 - TASI ***-->
								    <td>
									    <asp:label id="lblTipoUtilizzoDummy" runat="server" CssClass="Input_Label">Tipo Utilizzo</asp:label>
									    <asp:label id="lblAstTipoUtilizzoDummy" style="COLOR: red; FONT-SIZE: 11px" Runat="server">*</asp:label><br />
									    <asp:dropdownlist id="ddlTipoUtilizzoDummy" runat="server" CssClass="Input_Text"></asp:dropdownlist>
								    </td>
								    <td>
									    <asp:label id="lblTipoPossessoDummy" runat="server" CssClass="Input_Label">Tipo Possesso</asp:label>
                                        <span id="lblAstTipoPossessoDummy" style="COLOR: red; font-size: 11px">*</span><br />
									    <asp:dropdownlist id="ddlTipoPossessoDummy" runat="server" CssClass="Input_Text"></asp:dropdownlist>
								    </td>
                                    <!--*** ***-->
								    <td>
									    <asp:CheckBox id="chkColtivatoriDummy" runat="server" CssClass="Input_Label" Text="Coltivatore diretto" TextAlign="Right"></asp:CheckBox>
								    </td>
								    <td>
									    <asp:Label id="lblNumFigliDummy" runat="server" CssClass="Input_Label">Numero figli</asp:Label><br />
									    <asp:TextBox id="txtNumFigliDummy" CssClass="Input_Text_Right OnlyNumber" runat="server" Width="32px" MaxLength="3" AutoPostBack="True" ontextchanged="txtnumfigli_TextChanged"></asp:TextBox>
								    </td>
							    </tr>
							    <tr>
								    <td colspan="7">
									    <div id="DivCaricoFigliDummy" title="Percentuale Carico Figli">
										    <asp:Label id="lblCaricoFigliDummy" runat="server" CssClass="Input_Label">Percentuale Carico Figli</asp:Label>
										    <br />
										    <Grd:RibesGridView ID="GrdCaricoFigliDummy" runat="server" BorderStyle="None" 
                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                                <PagerSettings Position="Bottom"></PagerSettings>
                                                <PagerStyle CssClass="CartListFooter" />
                                                <RowStyle CssClass="CartListItem"></RowStyle>
                                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
											    <Columns>
												    <asp:BoundField DataField="nFiglio" HeaderText="Figlio N.">
													    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
													    <ItemStyle HorizontalAlign="Left"></ItemStyle>
												    </asp:BoundField>
												    <asp:TemplateField HeaderText="Percentuale">
													    <ItemStyle HorizontalAlign="Right"></ItemStyle>
													    <ItemTemplate>
														    <asp:TextBox id="TxtPercentCarico" onblur="VerificaPercentCarico(this);" runat="server" Width="100px" CssClass="Input_Text" style="text-align:right;" Text='<%# DataBinder.Eval(Container, "DataItem.Percentuale") %>'>
														    </asp:TextBox>
													    </ItemTemplate>
												    </asp:TemplateField>
											    </Columns>
										    </Grd:RibesGridView>
									    </div>
								    </td>
							    </tr>
							    <tr>
								    <td >
									    <asp:label id="lblMesiEsclusEsenzDummy" runat="server" CssClass="Input_Label">Mesi Escl. o Esenz. </asp:label><br />
									    <asp:textbox id="txtMesiEsclusioneDummy" onkeypress="return NumbersOnly(event);" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="65px" MaxLength="2"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblMesiRiduzioneDummy" runat="server" CssClass="Input_Label">Mesi Riduzione</asp:label><br />
									    <asp:textbox id="txtMesiRiduzioneDummy" onkeypress="return NumbersOnly(event);" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="65px" MaxLength="2"></asp:textbox>
								    </td>
								    <td valign="bottom">
									    <asp:checkbox id="chkAbitPrincipaleDummy" runat="server" CssClass="Input_Label" AutoPostBack="True" text="Abitaz. Principale" oncheckedchanged="chkAbitprincipale_CheckedChanged"></asp:checkbox>
								    </td>
								    <td valign="bottom">
									    <asp:checkbox id="chkPertinenzaDummy" runat="server" CssClass="Input_Label"></asp:checkbox>
									    <asp:linkbutton id="lnkApriPertinenzaDummy" CssClass="Input_Label" Runat="server" onclick="lnkApriPertinenza_Click">Pertinenza</asp:linkbutton>
									    <!--<a href="javascript:ApriPertinenza();" class="Input_Label_Text">Pertinenza</a>-->
								    </td>
								    <td>
									    <asp:label id="lblNumUtilizzatoriDummy" runat="server" CssClass="Input_Label">Num. Utilizzatori</asp:label><br />
									    <asp:textbox id="txtNumeroUtilizzatoriDummy" onkeypress="return NumbersOnly(event);" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="65px" MaxLength="2"></asp:textbox>
								    </td>
							    </tr>
							    <!-- Blocco Altri Proprietari -->
							    <tr>
								    <td width="100%" colspan="7">
								        <div id="DivDatiAltriProprietari">
								            <br />
					                        <a title="Visualizza Altri Proprietari" onclick="nascondi(this,'divAltriProprietari','Altri Proprietari')" href="#" class="lstTabRow" style="width:100%">Visualizza Altri Proprietari</a>
					                        <div id="divAltriProprietari" runat="server" style="width:100%; display:none">
								                <table width="100%">
										            <tr>
											            <td>
					                                        <Grd:RibesGridView ID="GrdAltriProprietari" runat="server" BorderStyle="None" 
                                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                                                <PagerSettings Position="Bottom"></PagerSettings>
                                                                <PagerStyle CssClass="CartListFooter" />
                                                                <RowStyle CssClass="CartListItem"></RowStyle>
                                                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							                                    <Columns></Columns>
						                                    </Grd:RibesGridView>
											            </td>
										            </tr>
								                </table>
								            </div>
								        </div>
								    </td>
							    </tr>
							    <!-- Blocco Dati Catasto -->
							    <tr>
								    <td width="100%" colspan="7">
								        <div id="DivDatiCatasto">
								            <br />
					                        <a title="Visualizza Dati Catasto" onclick="nascondi(this,'divCatasto','Dati Catasto')" href="#" class="lstTabRow" style="width:100%">Visualizza Dati Catasto</a>
					                        <div id="divCatasto" runat="server" style="width:100%; display:none">
								                <table width="100%">
										            <tr>
											            <td>
					                                        <Grd:RibesGridView ID="GrdCatastoUI" runat="server" BorderStyle="None" 
                                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                                                <PagerSettings Position="Bottom"></PagerSettings>
                                                                <PagerStyle CssClass="CartListFooter" />
                                                                <RowStyle CssClass="CartListItem"></RowStyle>
                                                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							                                    <Columns></Columns>
						                                    </Grd:RibesGridView>
											            </td>
										            </tr>
										            <tr>
											            <td>
					                                        <Grd:RibesGridView ID="GrdCatastoTit" runat="server" BorderStyle="None" 
                                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                                                <PagerSettings Position="Bottom"></PagerSettings>
                                                                <PagerStyle CssClass="CartListFooter" />
                                                                <RowStyle CssClass="CartListItem"></RowStyle>
                                                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							                                    <Columns></Columns>
						                                    </Grd:RibesGridView>
											            </td>
										            </tr>
								                </table>
								            </div>
								        </div>
								    </td>
							    </tr>
							    <!-- Blocco Dati TARSU/TARES -->
							    <tr>
								    <td width="100%" colspan="7">
								        <div id="DivDatiTARSU">
								            <br />
					                        <a title="Visualizza Dati TARI" onclick="nascondi(this,'divTARSU','Dati TARI')" href="#" class="lstTabRow" style="width:100%">Visualizza Dati TARSU/TARES</a>
					                        <asp:imagebutton id="ibNewTARSU" Runat="server" ImageUrl="../images/Bottoni/nuovoinserisci.png" Height="15px" Width="15px" ToolTip="Inserisci in TARI" OnClientClick="return confirm('Si vuole inserire in Dichiarazioni TARI?')"></asp:imagebutton>
								            <div id="divTARSU" runat="server" style="width:100%; display:none">
								                <table width="100%">
										            <tr>
											            <td>
					                                        <Grd:RibesGridView ID="GrdTARSU" runat="server" BorderStyle="None" 
                                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                                                OnRowCommand="GrdRowCommand">
                                                                <PagerSettings Position="Bottom"></PagerSettings>
                                                                <PagerStyle CssClass="CartListFooter" />
                                                                <RowStyle CssClass="CartListItem"></RowStyle>
                                                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							                                    <Columns>
								                                    <asp:BoundField DataField="Nominativo" HeaderText="Proprietario"></asp:BoundField>
								                                    <asp:BoundField DataField="Via" HeaderText="Via"></asp:BoundField>
								                                    <asp:TemplateField HeaderText="Dal">
													                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
													                    <ItemTemplate>
														                    <asp:Label id="lblDalGrid" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.datainizio")) %>'>
														                    </asp:Label>
													                    </ItemTemplate>
												                    </asp:TemplateField>
												                    <asp:TemplateField HeaderText="Al">
													                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
													                    <ItemTemplate>
														                    <asp:Label id="lblAlGrid" runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.datafine")) %>'>
														                    </asp:Label>
													                    </ItemTemplate>
												                    </asp:TemplateField>
								                                    <asp:BoundField DataField="cattares" HeaderText="Cat."></asp:BoundField>
								                                    <asp:BoundField DataField="mq" HeaderText="MQ" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
								                                    <asp:BoundField DataField="mqtassabili" HeaderText="MQ Tassabili" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
								                                    <asp:BoundField DataField="nvani" HeaderText="N.Vani" DataFormatString="{0:0}" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
								                                    <asp:TemplateField HeaderText="">
									                                    <headerstyle horizontalalign="Center"></headerstyle>
									                                    <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									                                    <itemtemplate>
										                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("Id") %>' alt=""></asp:ImageButton>
                                                                            <asp:HiddenField runat="server" ID="hfId" Value='<%# Eval("ID") %>' />
                                                                            <asp:HiddenField runat="server" ID="hfIdContribuente" Value='<%# Eval("IdContribuente") %>' />
                                                                            <asp:HiddenField runat="server" ID="hfIdTessera" Value='<%# Eval("IdTessera") %>' />
                                                                            <asp:HiddenField runat="server" ID="hfIdTestata" Value='<%# Eval("IdTestata") %>' />
									                                    </itemtemplate>
								                                    </asp:TemplateField>
							                                    </Columns>
						                                    </Grd:RibesGridView>
											            </td>
										            </tr>
								                </table>
								            </div>
								        </div>
								    </td>
							    </tr>
							    <!-- Dati Aggiuntivi -->
							    <tr>
							        <td colspan="7">
							            <a title="Visualizza Dati Aggiuntivi" onclick="nascondi(this,'divAltriDati','Dati Aggiuntivi')" href="#" class="lstTabRow" style="width:100%">Visualizza Dati Aggiuntivi</a>
							            <div id="divAltriDati" runat="server" style="width:100%; display:none">
							                <table width="100%">
							                    <tr>
								                    <td>
									                    <asp:label id="lblNumModelloDummy" runat="server" CssClass="Input_Label">Numero Modello</asp:label><br />
									                    <asp:textbox id="txtNumModelloDummy" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Text" Width="120px" MaxLength="9"></asp:textbox>
								                    </td>
								                    <td>
									                    <asp:label id="lblNumOrdineDummy" runat="server" CssClass="Input_Label">Numero Ordine</asp:label><br />
									                    <asp:textbox id="txtNumOrdineDummy" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Text" Width="120px" MaxLength="5"></asp:textbox>
								                    </td>
								                    <td>
									                    <asp:label id="lblNumProtocolloDummy" runat="server" CssClass="Input_Label">Num. Protocollo</asp:label><br />
									                    <asp:textbox id="txtNumProtocolloDummy" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Text" Width="65px" MaxLength="6"></asp:textbox>
								                    </td>
								                    <td>
									                    <asp:label id="lblAnnoDenunciaCatastaleDummy" runat="server" CssClass="Input_Label">Anno Denuncia Catastale</asp:label><br />
									                    <asp:textbox id="txtAnnoDenunciaCatastaleDummy" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="65px" MaxLength="4"></asp:textbox>
								                    </td>
								                </tr>
							                    <tr>
								                    <td class="Input_Label">
									                    <!-- Partita Catastale -->
									                    <asp:label id="lblPartitaCatastaleDummy" runat="server" CssClass="Input_Label">Partita Catastale</asp:label><br />
									                    <asp:textbox id="txtPartitaCatastaleDummy" onkeypress="return NumbersOnly(event);" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="65px" MaxLength="8"></asp:textbox>
								                    </td>
								                    <td>
									                    <asp:label id="lblNumEcograficoDummy" style="TEXT-ALIGN: right" runat="server" CssClass="Input_Label">Num. Ecografico</asp:label><br />
									                    <asp:textbox id="txtNumeroEcograficoDummy" onkeypress="return NumbersOnly(event);" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="60px"></asp:textbox>
								                    </td>
								                    <td>
									                    <asp:label id="lblDescrUffRegistroDummy" runat="server" CssClass="Input_Label">Descrizione Uff. Registro</asp:label><br />
									                    <asp:textbox id="txtDescrizioneUffRegistroDummy" style="TEXT-ALIGN: left" runat="server" CssClass="Input_Text" Width="200px"></asp:textbox>
								                    </td>
								                    <td>
									                    <asp:label id="lblImportoDetrazAbitazPrincipDummy" runat="server" CssClass="Input_Label">Imp. Detraz. Abitaz. Princip.</asp:label><br />
									                    <asp:textbox id="txtImpDetrazioneDummy" onkeypress="return NumbersOnly(event, true, false, 2);" onblur="SettaFlagAbitPrinc(this.value);" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="150px" MaxLength="6"></asp:textbox>
								                    </td>
							                    </tr>
				                                <tr>
					                                <td colspan="7">
						                                <asp:label id="lblAnnoDichiarazioneDummy" runat="server" CssClass="lstTabRow" Width="100%"></asp:label>
					                                </td>
				                                </tr>
							                    <tr>
								                    <td>
									                    <asp:label id="lblPossessoDummy" runat="server" CssClass="Input_Label">Possesso</asp:label><br />
									                    <asp:dropdownlist id="ddlPossessoDummy" runat="server" CssClass="Input_Text" Width="150px">
										                    <asp:listitem Value="2">NON COMPILATO</asp:listitem>
										                    <asp:listitem Value="0">SI</asp:listitem>
										                    <asp:listitem Value="1">NO</asp:listitem>
									                    </asp:dropdownlist>
								                    </td>
								                    <td>
									                    <asp:label id="lblEsclusoEsenteDummy" runat="server" CssClass="Input_Label">Escluso o Esente</asp:label><br />
									                    <asp:dropdownlist id="ddlEsclusoEsenteDummy" runat="server" CssClass="Input_Text" Width="150px">
										                    <asp:listitem Value="2">NON COMPILATO</asp:listitem>
										                    <asp:listitem Value="0">SI</asp:listitem>
										                    <asp:listitem Value="1">NO</asp:listitem>
									                    </asp:dropdownlist>
								                    </td>
								                    <td>
									                    <asp:label id="lblRiduzioneDummy" runat="server" CssClass="Input_Label">Riduzione</asp:label><br />
									                    <asp:dropdownlist id="ddlRiduzioneDummy" runat="server" CssClass="Input_Text" Width="150px">
										                    <asp:listitem Value="2">NON COMPILATO</asp:listitem>
										                    <asp:listitem Value="0">SI</asp:listitem>
										                    <asp:listitem Value="1">NO</asp:listitem>
									                    </asp:dropdownlist>
								                    </td>
								                    <td>
									                    <asp:label id="lblAbitazPrincipaleDummy" runat="server" CssClass="Input_Label">Abitaz. Principale</asp:label><br />
									                    <asp:dropdownlist id="ddlAbitazionePrincipaleDummy" runat="server" CssClass="Input_Text" Width="150px">
										                    <asp:listitem Value="2">NON COMPILATO</asp:listitem>
										                    <asp:listitem Value="0">SI</asp:listitem>
										                    <asp:listitem Value="1">NO</asp:listitem>
									                    </asp:dropdownlist>
								                    </td>
							                    </tr>
				                                <tr>
					                                <td colspan="7">
						                                <asp:label id="lblEstrTitoloDummy" runat="server" CssClass="lstTabRow" Width="100%">Estremi del Titolo</asp:label>
					                                </td>
				                                </tr>
							                    <tr>
								                    <td>
									                    <asp:label id="lblAcquistoDummy" runat="server" CssClass="Input_Label">di acquisto</asp:label><br />
									                    <asp:dropdownlist id="ddlAcquistoDummy" runat="server" CssClass="Input_Text" Width="150px">
										                    <asp:listitem Value="2">NON COMPILATO</asp:listitem>
										                    <asp:listitem Value="0">SI</asp:listitem>
										                    <asp:listitem Value="1">NO</asp:listitem>
									                    </asp:dropdownlist>
								                    </td>
								                    <td>
									                    <asp:label id="lblCessioneDummy" runat="server" CssClass="Input_Label">di cessione </asp:label><br />
									                    <asp:dropdownlist id="ddlCessioneDummy" runat="server" CssClass="Input_Text" Width="150px">
										                    <asp:listitem Value="2">NON COMPILATO</asp:listitem>
										                    <asp:listitem Value="0">SI</asp:listitem>
										                    <asp:listitem Value="1">NO</asp:listitem>
									                    </asp:dropdownlist>
								                    </td>
							                    </tr>
							                    <tr>
								                    <td style="DISPLAY: none">
									                    <asp:label id="lblTipoImmobileDummy" runat="server" CssClass="Input_Label">Tipo Immobile</asp:label>
								                    </td>
								                    <td style="DISPLAY: none">
									                    <asp:dropdownlist id="ddlTipoImmobileDummy" runat="server" CssClass="Input_Text"></asp:dropdownlist>
								                    </td>
								                    <td style="DISPLAY: none">
									                    <asp:label id="lblCodUIDummy" runat="server" CssClass="Input_Label"> Cod. UI</asp:label>
								                    </td>
								                    <td style="DISPLAY: none">
									                    <asp:textbox id="txtCodUIDummy" runat="server" CssClass="Input_Text" Width="40px" MaxLength="4"></asp:textbox>
								                    </td>
							                    </tr>
							                </table>
							            </div>
							        </td>
							    </tr>
				                <tr>
					                <!--*** 20120704 - IMU ***-->
					                <td colspan="7">
						                <asp:label id="lblNoteIciDummy" runat="server" CssClass="lstTabRow" Width="100%">Note ICI/IMU</asp:label><br /><br />
						                <asp:textbox CssClass="Input_Text" ID="txtNoteIciDummy" Runat="server" width="700px" textmode="MultiLine" height="46px" maxlength="2000"></asp:textbox>
					                </td>
				                </tr>
				            </table>
				        </div>
				    </td>
				</tr>
				<tr>
				    <td colspan="2">
				        <div id="NoDummyDich">
				            <table>
							    <!-- Dati Immobile -->
							    <tr>
								    <td colspan="7">
									    <asp:label id="lblDatiImmNoDummy" runat="server" CssClass="lstTabRow" Width="100%">Dati immobile</asp:label>
								    </td>
							    </tr>
							    <tr>
				                    <td>
					                    <asp:label id="lblNumModelloNoDummy" runat="server" CssClass="Input_Label">Numero Modello</asp:label><br />
					                    <asp:textbox id="txtNumModelloNoDummy" runat="server" CssClass="Input_Text_Right" Width="120px" MaxLength="9" Enabled="False"></asp:textbox>
				                    </td>
				                    <td>
					                    <asp:label id="lblNumOrdineNoDummy" runat="server" CssClass="Input_Label">Numero Ordine</asp:label><br />
					                    <asp:textbox id="txtNumOrdineNoDummy" runat="server" CssClass="Input_Text_Right" Width="120px" MaxLength="5" Enabled="False"></asp:textbox>
				                    </td>
								    <td>
									    <asp:label id="lblDataInizioNoDummy" runat="server" CssClass="Input_Label">Data Inizio</asp:label><br />
									    <asp:textbox id="txtDataInizioNoDummy" runat="server" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);TrackBlurGenerale(this);" CssClass="Input_Text_Right TextDate" Enabled="False"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblDataFineNoDummy" runat="server" CssClass="Input_Label">Data Fine</asp:label><br />
									    <asp:textbox id="txtDataFineNoDummy" runat="server" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);TrackBlurGenerale(this);" CssClass="Input_Text_Right TextDate" Enabled="False"></asp:textbox>
								    </td>
							    </tr>
							    <tr>
								    <td>
									    <asp:label id="lblCaratteristicaNoDummy" runat="server" CssClass="Input_Label">Caratteristica</asp:label>
									    <asp:label id="lblAstCaratteristicaNoDummy" runat="server" style="COLOR: red; font-size: 11px">*</asp:label><br />
									    <asp:dropdownlist id="ddlCaratteristicaNoDummy" runat="server" CssClass="Input_Text" Enabled="False" AutoPostBack="True" onselectedindexchanged="ddlCaratteristica_SelectedIndexChanged"></asp:dropdownlist>&nbsp; 
									    &nbsp;&nbsp;
								    </td>
								    <td>
									    <asp:label id="lblCodRenditaNoDummy" runat="server" CssClass="Input_Label">Codice Rendita</asp:label><br />
									    <asp:dropdownlist id="ddlCodiceRenditaNoDummy" runat="server" CssClass="Input_Text" Enabled="False" AutoPostBack="True" onselectedindexchanged="ddlCodiceRendita_SelectedIndexChanged"></asp:dropdownlist>
								    </td>
							    </tr>
							    <tr>
								    <td colspan="2">
									    <asp:label id="lblCodViaNoDummy" runat="server" CssClass="Input_Label">Via</asp:label>&nbsp; 
									    <!--*** 20130304 - gestione dati da territorio ***-->
									    <asp:imagebutton id="LnkNewUIAnaterNoDummy" runat="server" ImageUrl="../images/Bottoni/Listasel.png" CausesValidation="False" imagealign="Bottom" Enabled="False"></asp:imagebutton>&nbsp; 
									    <!--*** ***-->
									    <asp:Label ID="lblViaOldNoDummy" runat="server" CssClass="Input_Label"></asp:Label>&nbsp;<br />
									    <asp:textbox id="txtViaNoDummy" runat="server" style="TEXT-ALIGN: left" CssClass="Input_Text" Width="400px" MaxLength="1000" Enabled="False" ReadOnly="true"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblNumeroCivicoNoDummy" runat="server" CssClass="Input_Label">Num. Civico</asp:label><br />
									    <asp:textbox id="txtNumCivNoDummy" runat="server" onkeypress="return NumbersOnly(event);" CssClass="Input_Text_Right OnlyNumber" Width="65px" Enabled="False"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblEspCivicoNoDummy" runat="server" CssClass="Input_Label">Esp. Civico</asp:label><br />
									    <asp:textbox id="txtEspCivicoNoDummy" runat="server" CssClass="Input_Text_Right" Width="65px" Enabled="False"></asp:textbox>
									    <asp:label id="lblBarraNoDummy" runat="server" style="DISPLAY: none" CssClass="Input_Label">/</asp:label>&nbsp;
									    <asp:textbox id="txtBarratoNoDummy" runat="server" style="DISPLAY: none" CssClass="Input_Text" Width="30px" MaxLength="3" Enabled="False"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblScalaNoDummy" runat="server" CssClass="Input_Label">Scala</asp:label><br />
									    <asp:textbox id="txtScalaNoDummy" runat="server" CssClass="Input_Text_Right" Width="65px" MaxLength="3" Enabled="False"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblPianoNoDummy" runat="server" CssClass="Input_Label">Piano</asp:label><br />
									    <asp:textbox id="txtPianoNoDummy" runat="server" CssClass="Input_Text_Right" Width="65px" MaxLength="2" Enabled="False"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblInternoNoDummy" runat="server" CssClass="Input_Label">Interno</asp:label><br />
									    <asp:textbox id="txtInternoNoDummy" runat="server" CssClass="Input_Text_Right" Width="65px" MaxLength="5" Enabled="False"></asp:textbox>
								    </td>
							    </tr>
							    <!-- Dati Catastali -->
							    <tr>
								    <td colspan="7">
									    <asp:label id="lblDatiCatNoDummy" runat="server" CssClass="lstTabRow" Width="100%">Dati Catastali</asp:label>
								    </td>
							    </tr>
							    <tr>
				                    <td class="Input_Label">
					                    <asp:label id="lblPartitaCatastaleNoDummy" runat="server" CssClass="Input_Label">Partita Catastale</asp:label><br />
					                    <asp:textbox id="txtPartitaCatastaleNoDummy" runat="server" onkeypress="return NumbersOnly(event);" CssClass="Input_Text_Right OnlyNumber" Width="65px" MaxLength="8" Enabled="False"></asp:textbox>
				                    </td>
								    <td>
									    <asp:label id="lblSezioneNoDummy" runat="server" CssClass="Input_Label"> Sezione</asp:label><br />
									    <asp:textbox id="txtSezioneNoDummy" runat="server" CssClass="Input_Text_Right" Width="65px" MaxLength="3" Enabled="False"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblFoglioNoDummy" runat="server" CssClass="Input_Label">Foglio</asp:label>
									    <asp:label id="lblAstFoglioNoDummy" runat="server" style="COLOR: red; FONT-SIZE: 11px">*</asp:label><br />
									    <asp:textbox id="txtFoglioNoDummy" runat="server" CssClass="Input_Text_Right" Width="65px" MaxLength="5" Text='<%# DataBinder.Eval(Container, "DataItem.Foglio") %>' Enabled="False"> </asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblNumeroNoDummy" runat="server" CssClass="Input_Label">Numero</asp:label>
									    <asp:label id="lblAstNumeroNoDummy" runat="server" style="COLOR: red; FONT-SIZE: 11px">*</asp:label><br />
									    <asp:textbox id="txtNumeroNoDummy" runat="server" CssClass="Input_Text_Right" Width="65px" MaxLength="5" Enabled="False"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblSubalternoNoDummy" runat="server" CssClass="Input_Label"> Subalterno</asp:label><br />
									    <asp:textbox id="txtSubalternoNoDummy" runat="server" onkeypress="return NumbersOnly(event);" CssClass="Input_Text_Right OnlyNumber" Width="65px" MaxLength="4" Enabled="False"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblCategoriaCatastaleNoDummy" runat="server" CssClass="Input_Label">Cat. Catastale</asp:label><br />
									    <asp:dropdownlist id="ddlCategoriaCatastaleNoDummy" runat="server" CssClass="Input_Text" Enabled="False" width="88px"></asp:dropdownlist>
								    </td>
								    <td>
									    <asp:label id="lblClasseNoDummy" runat="server" CssClass="Input_Label">Classe</asp:label><br />
									    <asp:dropdownlist id="ddlClasseNoDummy" runat="server" CssClass="Input_Text" Enabled="False" width="72px"></asp:dropdownlist>
								    </td>
							    </tr>
			                    <tr>
				                    <td>
					                    <asp:label id="lblNumProtocolloNoDummy" runat="server" CssClass="Input_Label">Num. Protocollo</asp:label><br />
					                    <asp:textbox id="txtNumProtocolloNoDummy" runat="server" CssClass="Input_Text_Right" Width="65px" MaxLength="6" Enabled="False"></asp:textbox>
				                    </td>
				                    <td>
					                    <asp:label id="lblAnnoDenunciaCatastaleNoDummy" runat="server" CssClass="Input_Label">Anno Denuncia Catastale</asp:label><br />
					                    <asp:textbox id="txtAnnoDenunciaCatastaleNoDummy" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="65px" MaxLength="4" Enabled="False"></asp:textbox>
				                    </td>
				                </tr>
							    <tr>
								    <td>
									    <asp:label id="lblComboEstimoNoDummy" runat="server" CssClass="Input_Label">Zona</asp:label><br />
									    <asp:dropdownlist id="ddlEstimoNoDummy" runat="server" CssClass="Input_Text_Right" Width="100px" MaxLength="20" Enabled="False"></asp:dropdownlist>
								    </td>
								    <td>
									    <asp:label id="lblConsistenzaNoDummy" runat="server" CssClass="Input_Label">Consistenza</asp:label><br />
									    <asp:textbox id="txtConsistenzaNoDummy" runat="server" onkeypress="return NumbersOnly(event, true, false, 2);" CssClass="Input_Text_Right OnlyNumber" Width="100px" MaxLength="20" Enabled="False"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblRenditaNoDummy" runat="server" CssClass="Input_Label">Rendita</asp:label>&nbsp;
									    <asp:linkbutton id="lnkRenditaNoDummy" runat="server" CssClass="Input_Label" title="Calcola Rendita" onclick="lnkRendita_Click">&raquo;</asp:linkbutton><br />
									    <asp:textbox id="txtRenditaNoDummy" runat="server" onkeypress="return NumbersOnly(event, true, false, 2);" CssClass="Input_Text_Right OnlyNumber" Width="100px" MaxLength="20" Enabled="false"></asp:textbox>
								    </td>
								    <td>
									    <asp:label id="lblValoreImmobileNoDummy" runat="server" CssClass="Input_Label">Valore Immobile</asp:label>&nbsp;
									    <asp:linkbutton id="lnkValoreNoDummy" runat="server" CssClass="Input_Label" title="Calcola Valore" onclick="lnkValore_Click">&raquo;</asp:linkbutton><br />
									    <asp:textbox id="txtValoreNoDummy" runat="server" onkeypress="return NumbersOnly(event, true, false, 2);" CssClass="Input_Text_Right OnlyNumber" Width="100px" MaxLength="20" Enabled="false"></asp:textbox>
								    </td>
								    <td>
									    <asp:checkbox id="chkValoreProvvisorioNoDummy" runat="server" CssClass="Input_Label" AutoPostBack="True" text="Valore Provvisorio" oncheckedchanged="chkValoreProvvisorio_CheckedChanged"></asp:checkbox>
								    </td>
								    <td>
									    <asp:checkbox id="chkStoricoNoDummy" runat="server" CssClass="Input_Label" text="Storico"></asp:checkbox>
								    </td>
								    <td>
									    <asp:checkbox id="chkExruraleNoDummy" runat="server" CssClass="Input_Label" text="Ex-rurale"></asp:checkbox>
								    </td>
							    </tr>
			                    <tr>
				                    <td>
					                    <asp:label id="lblNumEcograficoNoDummy" runat="server" style="TEXT-ALIGN: right" CssClass="Input_Label">Num. Ecografico</asp:label><br />
					                    <asp:textbox id="txtNumeroEcograficoNoDummy" runat="server" onkeypress="return NumbersOnly(event);" CssClass="Input_Text_Right OnlyNumber" Width="60px" Enabled="False"></asp:textbox>
				                    </td>
				                    <td>
					                    <asp:label id="lblDescrUffRegistroNoDummy" runat="server" CssClass="Input_Label">Descrizione Uff. Registro</asp:label><br />
					                    <asp:textbox id="txtDescrizioneUffRegistroNoDummy" runat="server" CssClass="Input_Text" Width="200px" Enabled="False"></asp:textbox>
				                    </td>
			                    </tr>
							    <!-- Dati Possesso -->
							    <tr>
								    <td colspan="7">
									    <asp:label id="lblDatiPosNoDummy" runat="server" CssClass="lstTabRow" Width="100%">Dati possesso</asp:label>
								    </td>
							    </tr>
							    <tr>
								    <td>
									    <asp:label id="lblPercPossessoNoDummy" runat="server" CssClass="Input_Label"> Percentuale Possesso</asp:label><br />
									    <asp:textbox id="txtPercPossessoNoDummy" runat="server" onkeypress="return NumbersOnly(event, true, false, 2);" CssClass="Input_Text_Right OnlyNumber" Width="65px" MaxLength="6" Enabled="False"></asp:textbox>
								    </td>
								    <td >
									    <asp:label id="lblMesiPossessoNoDummy" runat="server" CssClass="Input_Label">Mesi Possesso</asp:label><br />
									    <asp:textbox id="txtMesiPossessoNoDummy" runat="server" onkeypress="return NumbersOnly(event);" CssClass="Input_Text_Right OnlyNumber" Width="65px" MaxLength="2" Enabled="False"></asp:textbox>
								    </td>
                                    <!--*** 20140509 - TASI ***-->
								    <td>
									    <asp:label id="lblTipoUtilizzoNoDummy" runat="server" CssClass="Input_Label">Tipo Utilizzo</asp:label>
									    <asp:label id="lblAstTipoUtilizzoNoDummy" runat="server" style="COLOR: red; FONT-SIZE: 11px">*</asp:label><br />
									    <asp:dropdownlist id="ddlTipoUtilizzoNoDummy" runat="server" CssClass="Input_Text" Enabled="False"></asp:dropdownlist>
								    </td>
								    <td>
									    <asp:label id="lblTipoPossessoNoDummy" runat="server" CssClass="Input_Label">Tipo Possesso</asp:label><br />
									    <asp:dropdownlist id="ddlTipoPossessoNoDummy" runat="server" CssClass="Input_Text" Enabled="False"></asp:dropdownlist>
								    </td>
                                    <!--*** ***-->
                                </tr>
                                <tr>
								    <td>
									    <asp:CheckBox id="chkcoltivatoriNoDummy" runat="server" CssClass="Input_Label" Text="Coltivatore diretto" Enabled="False" TextAlign="Right"></asp:CheckBox>
								    </td>
								    <td>
									    <asp:Label id="lblnumfigliNoDummy" runat="server" CssClass="Input_Label">Numero figli</asp:Label><br />
									    <asp:TextBox id="txtnumfigliNoDummy" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="32px" MaxLength="3" AutoPostBack="True" Enabled="False" ontextchanged="txtnumfigli_TextChanged"></asp:TextBox>
								    </td>
							    </tr>
							    <tr>
								    <td colspan="7">
									    <div id="DivCaricoFigli" title="Percentuale Carico Figli">
										    <asp:Label id="lblCaricoFigliNoDummy" runat="server" CssClass="Input_Label">Percentuale Carico Figli</asp:Label>
										    <br />
										    <Grd:RibesGridView ID="GrdCaricoFigliNoDummy" runat="server" BorderStyle="None" 
                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                                OnRowCommand="GrdRowCommand">
                                                <PagerSettings Position="Bottom"></PagerSettings>
                                                <PagerStyle CssClass="CartListFooter" />
                                                <RowStyle CssClass="CartListItem"></RowStyle>
                                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
											    <Columns>
												    <asp:BoundField DataField="nFiglio" HeaderText="Figlio N.">
													    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
													    <ItemStyle HorizontalAlign="Left"></ItemStyle>
												    </asp:BoundField>
												    <asp:TemplateField HeaderText="Percentuale">
													    <ItemStyle HorizontalAlign="Right"></ItemStyle>
													    <ItemTemplate>
														    <asp:TextBox id="TxtPercentCarico" runat="server" onblur="VerificaPercentCarico(this);" Width="100px" CssClass="Input_Text" style="text-align:right;" Text='<%# DataBinder.Eval(Container, "DataItem.Percentuale") %>'>
														    </asp:TextBox>
													    </ItemTemplate>
												    </asp:TemplateField>
												    <asp:TemplateField HeaderText="">
									                    <headerstyle horizontalalign="Center"></headerstyle>
									                    <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									                    <itemtemplate>
										                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>' alt=""></asp:ImageButton>					                                        
									                    </itemtemplate>
								                    </asp:TemplateField>
											    </Columns>
										    </Grd:RibesGridView>
									    </div>
								    </td>
							    </tr>
							    <tr>
								    <td >
									    <asp:label id="lblMesiEsclusEsenzNoDummy" runat="server" CssClass="Input_Label">Mesi Escl. o Esenz. </asp:label><br />
									    <asp:textbox id="txtMesiEsclusioneNoDummy" runat="server" onkeypress="return NumbersOnly(event);" CssClass="Input_Text_Right OnlyNumber" Width="65px" MaxLength="2" Enabled="False"></asp:textbox>
								    </td>
				                    <td>
					                    <asp:label id="lblImportoDetrazAbitazPrincipNoDummy" runat="server" CssClass="Input_Label">Imp. Detraz. Abitaz. Princip.</asp:label><br />
					                    <asp:textbox id="txtImpDetrazioneNoDummy" runat="server" onkeypress="return NumbersOnly(event, true, false, 2);" onblur="SettaFlagAbitPrinc(this.value);" CssClass="Input_Text_Right OnlyNumber" Width="150px" MaxLength="6" Enabled="False"></asp:textbox>
				                    </td>
								    <td>
									    <asp:label id="lblMesiRiduzioneNoDummy" runat="server" CssClass="Input_Label">Mesi Riduzione</asp:label><br />
									    <asp:textbox id="txtMesiRiduzioneNoDummy" runat="server" onkeypress="return NumbersOnly(event);" CssClass="Input_Text_Right OnlyNumber" Width="65px" MaxLength="2" Enabled="False"></asp:textbox>
								    </td>
								    <td valign="bottom">
									    <asp:checkbox id="chkAbitprincipaleNoDummy" runat="server" CssClass="Input_Label" AutoPostBack="True" text="Abitaz. Principale" oncheckedchanged="chkAbitprincipale_CheckedChanged"></asp:checkbox>
								    </td>
								    <td valign="bottom">
									    <asp:checkbox id="chkPertinenzaNoDummy" runat="server" CssClass="Input_Label"></asp:checkbox>
									    <asp:linkbutton id="lnkApriPertinenzaNoDummy" runat="server" CssClass="Input_Label" onclick="lnkApriPertinenza_Click" Enabled="False">Pertinenza</asp:linkbutton>
								    </td>
								    <td>
									    <asp:label id="lblNumUtilizzatoriNoDummy" runat="server" CssClass="Input_Label">Num. Utilizzatori</asp:label><br />
									    <asp:textbox id="txtNumeroUtilizzatoriNoDummy" runat="server" onkeypress="return NumbersOnly(event);" CssClass="Input_Text_Right OnlyNumber" Width="65px" MaxLength="2" Enabled="False"></asp:textbox>
								    </td>
							    </tr>
                                <tr>
	                                <td colspan="7">
		                                <asp:label id="lblAnnoDichiarazioneNoDummy" runat="server" CssClass="lstTabRow" Width="100%"></asp:label>
	                                </td>
                                </tr>
			                    <tr>
				                    <td>
					                    <asp:label id="lblPossessoNoDummy" runat="server" CssClass="Input_Label">Possesso</asp:label><br />
					                    <asp:dropdownlist id="ddlPossessoNoDummy" runat="server" CssClass="Input_Text" Enabled="False" Width="150px">
						                    <asp:listitem Value="2">NON COMPILATO</asp:listitem>
						                    <asp:listitem Value="0">SI</asp:listitem>
						                    <asp:listitem Value="1">NO</asp:listitem>
					                    </asp:dropdownlist>
				                    </td>
				                    <td>
					                    <asp:label id="lblEsclusoEsenteNoDummy" runat="server" CssClass="Input_Label">Escluso o Esente</asp:label><br />
					                    <asp:dropdownlist id="ddlEsclusoEsenteNoDummy" runat="server" CssClass="Input_Text" Enabled="False" Width="150px">
						                    <asp:listitem Value="2">NON COMPILATO</asp:listitem>
						                    <asp:listitem Value="0">SI</asp:listitem>
						                    <asp:listitem Value="1">NO</asp:listitem>
					                    </asp:dropdownlist>
				                    </td>
				                    <td>
					                    <asp:label id="lblRiduzioneNoDummy" runat="server" CssClass="Input_Label">Riduzione</asp:label><br />
					                    <asp:dropdownlist id="ddlRiduzioneNoDummy" runat="server" CssClass="Input_Text" Enabled="False" Width="150px">
						                    <asp:listitem Value="2">NON COMPILATO</asp:listitem>
						                    <asp:listitem Value="0">SI</asp:listitem>
						                    <asp:listitem Value="1">NO</asp:listitem>
					                    </asp:dropdownlist>
				                    </td>
				                    <td>
					                    <asp:label id="lblAbitazPrincipaleNoDummy" runat="server" CssClass="Input_Label">Abitaz. Principale</asp:label><br />
					                    <asp:dropdownlist id="ddlAbitazionePrincipaleNoDummy" runat="server" CssClass="Input_Text" Enabled="False" Width="150px">
						                    <asp:listitem Value="2">NON COMPILATO</asp:listitem>
						                    <asp:listitem Value="0">SI</asp:listitem>
						                    <asp:listitem Value="1">NO</asp:listitem>
					                    </asp:dropdownlist>
				                    </td>
			                    </tr>
                                <tr>
	                                <td colspan="7">
		                                <asp:label id="lblEstrTitoloNoDummy" runat="server" CssClass="lstTabRow" Width="100%">Estremi del Titolo</asp:label>
	                                </td>
                                </tr>
			                    <tr>
				                    <td>
					                    <asp:label id="lblAcquistoNoDummy" runat="server" CssClass="Input_Label">di acquisto</asp:label><br />
					                    <asp:dropdownlist id="ddlAcquistoNoDummy" runat="server" CssClass="Input_Text" Enabled="False" Width="150px">
						                    <asp:listitem Value="2">NON COMPILATO</asp:listitem>
						                    <asp:listitem Value="0">SI</asp:listitem>
						                    <asp:listitem Value="1">NO</asp:listitem>
					                    </asp:dropdownlist>
				                    </td>
				                    <td>
					                    <asp:label id="lblCessioneNoDummy" runat="server" CssClass="Input_Label">di cessione </asp:label><br />
					                    <asp:dropdownlist id="ddlCessioneNoDummy" runat="server" CssClass="Input_Text" Enabled="False" Width="150px">
						                    <asp:listitem Value="2">NON COMPILATO</asp:listitem>
						                    <asp:listitem Value="0">SI</asp:listitem>
						                    <asp:listitem Value="1">NO</asp:listitem>
					                    </asp:dropdownlist>
				                    </td>
			                    </tr>
				                <tr>
					                <!--*** 20120704 - IMU ***-->
					                <td colspan="7">
						                <asp:label id="lblNoteIciNoDummy" runat="server" CssClass="lstTabRow" Width="100%">Note ICI/IMU</asp:label><br /><br />
						                <asp:textbox ID="txtNoteIciNoDummy" runat="server" CssClass="Input_Text" width="700px" textmode="MultiLine" height="46px" maxlength="2000" enabled="False"></asp:textbox>
					                </td>
				                </tr>
							    <!-- Dati Aggiuntivi -->
							    <tr style="display:none">
							        <td colspan="7">
							            <div id="divAltriDatiNoDummy" runat="server" style="width:100%">
							                <table width="100%">
							                    <tr style="DISPLAY: none">
								                    <td>
									                    <asp:label id="lblTipoImmobileNoDummy" runat="server" CssClass="Input_Label">Tipo Immobile</asp:label>
								                    </td>
								                    <td>
									                    <asp:dropdownlist id="ddlTipoImmobileNoDummy" runat="server" CssClass="Input_Text" Enabled="False"></asp:dropdownlist>
								                    </td>
								                    <td>
									                    <asp:label id="lblCodUINoDummy" runat="server" CssClass="Input_Label"> Cod. UI</asp:label>
								                    </td>
								                    <td>
									                    <asp:textbox id="txtCodUINoDummy" runat="server" CssClass="Input_Text" Width="40px" MaxLength="4" Enabled="False"></asp:textbox>
								                    </td>
							                    </tr>
							                </table>
							            </div>
							        </td>
							    </tr>
				            </table>
				        </div>
				    </td>
				</tr>
                <tr style="display:none">
                    <td>
                        <asp:label id="lblDataUltimaModifica" runat="server" CssClass="Input_Label">Data Ultima Modifica</asp:label><br />
                        <asp:textbox id="txtDataUltimaModifica" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);TrackBlurGenerale(this);" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="8"></asp:textbox>
                    </td>
                    <td>
                        <asp:Label runat="server" CssClass="Input_Label">Valuta</asp:Label><br />
                        <asp:dropdownlist id="ddlValuta" runat="server" CssClass="Input_Text" Enabled="true"></asp:dropdownlist>
                    </td>
                </tr>
			</table>
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
			<asp:HiddenField id="hdTypeOperation" runat="server" Value="-1" />
            <asp:linkbutton id="lbtnCodiceComune" runat="server" style="display:none" CommandName="Comune" CausesValidation="False">Comune</asp:linkbutton><br />
            <asp:textbox id="txtCodiceComune" runat="server" style="display:none" CssClass="Input_Text" Width="200px" MaxLength="6"></asp:textbox>
			<asp:TextBox ID="txtUpdateAnagraficaValue" Runat="server" style="DISPLAY:none"></asp:TextBox>
			<asp:TextBox ID="txtCodRicercaAnater" Runat="server" style="DISPLAY:none"></asp:TextBox>
		    <asp:textbox id="TxtViaRibaltata" style="DISPLAY: none" CssClass="Input_Text" Runat="server"></asp:textbox>
		    <asp:TextBox ID="txtCodVia" Runat="server" style="DISPLAY:none"></asp:TextBox>
		    <asp:textbox id="txtCodPertinenza" runat="server" style="DISPLAY:none" CssClass="Input_Text" Width="65px" MaxLength="2"></asp:textbox>
			<input id="txtNameObject" type="hidden" name="txtNameObject" /> 
			<input id="txtIdTerUI" type="hidden" name="txtIdTerUI" runat="server" />
			<input id="txtIdTerProprietario" type="hidden" name="txtIdTerProprietario" runat="server" />
			<input id="txtIdTerProprieta" type="hidden" name="txtIdTerProprieta" runat="server" />
			<asp:button id="btnSalva" style="DISPLAY: none" runat="server" Text="Salva" onclick="btnSalva_Click"></asp:button>
			<asp:button id="btnElimina" style="DISPLAY: none" runat="server" Text="Elimina" CausesValidation="False" onclick="btnElimina_Click"></asp:button>
			<asp:button id="btnIndietro" style="DISPLAY: none" runat="server" Text="Indietro" CausesValidation="False" onclick="btnIndietro_Click"></asp:button>
			<asp:button id="btnAbilita" style="DISPLAY: none" runat="server" Text="Abilita" CausesValidation="False" onclick="btnAbilita_Click"></asp:button>
			<asp:button id="btnAggiungiContitolare" style="DISPLAY: none" runat="server" Text="Aggiungi Contitolare" onclick="btnAggiungiContitolare_Click"></asp:button>
			<asp:button id="btnRibaltaInAnater" runat="server" Text="Ribalta" style="DISPLAY:none" CausesValidation="False" onclick="btnRibaltaInAnater_Click"></asp:button>
			<asp:button id="btnDuplica" style="DISPLAY:none" runat="server" Text="Duplica Immobile" Enabled="true" onclick="btnDuplica_Click"></asp:button>
			<asp:button id="btnVisualizzaDettagli" style="DISPLAY: none" Runat="server" Text="Visualizza Dettagli" Enabled="true" onclick="btnVisualizzaDettagli_Click"></asp:button>
			<!--*** 20131003 - gestione atti compravendita ***-->
			<asp:button id="cmdPrecarica" style="DISPLAY:none" runat="server" Text="Precarica Immobile" Enabled="true" onclick="cmdPrecarica_Click"></asp:button>
			<asp:button id="cmdTrattatoSoggettoCompraVendita" style="DISPLAY:none" runat="server" Text="" Enabled="true" onclick="cmdTrattatoSoggettoCompraVendita_Click"></asp:button>
			<!--*** ***-->
			<!--*** 20131018 - DOCFA ***-->
			<asp:button id="cmdDOCFADet" style="DISPLAY:none" runat="server" Text="cmdDOCFADet" Enabled="true" onclick="cmdDOCFADet_Click"></asp:button>
			<!--*** ***-->
			<asp:button id="CmdGIS" style="DISPLAY: none" Runat="server"></asp:button>
		    <asp:Button id="btnGoogleMaps" runat="server" Width="16px" Cssclass="Bottone BottoneMappa" onclick="btnGoogleMaps_Click"></asp:Button>
		</form>
	</body>
</html>
