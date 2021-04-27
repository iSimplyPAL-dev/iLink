<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RicercaAvanzata.aspx.vb" Inherits="Provvedimenti.RicercaAvanzata" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
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
    <script type="text/javascript" src="../../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/vbscript" src="../../../_vbs/OperazioniSuCampi.vbs"></script>
    <script type="text/vbscript" src="../../../_vbs/ControlliFormali.vbs"></script>
    <script type="text/javascript">	
        function stampa() {
            if (frames('loadGrid').document.getElementById('RibesGridAnagrafica') == null) {
                GestAlert('a', 'warning', '', '', 'Per effettuare la stampa massiva dei Provvedimenti è necessario effettuare la ricerca!');
                return false;
            }
            else {
                frames('loadGrid').StampaDocumenti()
            }
        }
        //funzione chiamata dal validator
		function AttivaDIVDate()
		{
			if(document.getElementById('txtDIVDate').value =='-1')
			{
				document.getElementById('Date').style.display ='';
				document.getElementById('txtDIVDate').value ='1';			
			}
			else
			{
				document.getElementById('Date').style.display ='none';
				document.getElementById('txtDIVDate').value ='-1';			
			}			
		}
		function VerificaCampi()
		{
			sMsg=""
			
			/*checkbox_checker();
			
			if(document.getElementById('txtCheckBox').value=="-1")
			{
				alert('[Selezionare almeno una Data per la ricerca !]') ; 
				document.getElementById('Date').style.display ='';
				document.getElementById('txtDIVDate').value ='1';
			
				return false;
				
			}*/
			if(document.getElementById('chkDataGenerazione').checked)
			{
				//'''''''''Data Generazione''''''''''''''''''''''''''''''//
				if(document.getElementById('optGenerazione').checked)
				{
					if(IsBlank(document.getElementById('txtDataGenerazioneDal').value) && IsBlank(document.getElementById('txtDataGenerazioneAL').value)) 
					{
						sMsg=  sMsg + "[Date di Elaborazione obbligatorie!]\n" ; 
					}
					/*if (IsBlank(document.getElementById('txtDataGenerazioneDal').value) && !IsBlank(document.getElementById('txtDataGenerazioneAL').value)) 
					{
						sMsg=  sMsg + "[Date di Elaborazione DAL obbligatoria!]\n" ; 
					}*/
					if (!IsBlank(document.getElementById('txtDataGenerazioneDal').value) && !IsBlank(document.getElementById('txtDataGenerazioneAL').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataGenerazioneAL'),document.getElementById('txtDataGenerazioneDal')))
						{
							sMsg=  sMsg + "[Data di Elaborazione AL minore di Data di Elaborazione DAL !]\n" ; 
						}
					}
				}
				else
				{
					sMsg=  sMsg + "[Inserire le Date di Elaborazione per la ricerca !]\n" ; 
				}
			 }	
				//'''''''''Fine Data Generazione''''''''''''''''''''''''''''''//
				//'''''''''Data ConfermaA vviso''''''''''''''''''''''''''''''//
			 if(document.getElementById('optConfermaAvviso').checked)
			 {
				if(IsBlank(document.getElementById('txtDataConfermaAvvisoDal').value) && IsBlank(document.getElementById('txtDataConfermaAvvisoAl').value)) 
				{
					
					sMsg=  sMsg + "[Date di Conferma Avviso obbligatorie!]\n" ; 
				}
				/*if (IsBlank(document.getElementById('txtDataConfermaAvvisoDal').value) && !IsBlank(document.getElementById('txtDataConfermaAvvisoAl').value)) 
				{
					sMsg=  sMsg + "[Date di Conferma Avviso DAL obbligatoria!]\n" ; 
				}*/
				if (!IsBlank(document.getElementById('txtDataConfermaAvvisoDal').value) && !IsBlank(document.getElementById('txtDataConfermaAvvisoAl').value)) 
				{
					if (!doDateCheck(document.getElementById('txtDataConfermaAvvisoAl'),document.getElementById('txtDataConfermaAvvisoDal')))
					{
						sMsg=  sMsg + "[Data di Conferma Avviso AL minore di Data di Conferma Avviso DAL!]\n" ; 
					}
				}
			
			}
			//'''''''''Fine Data  Conferma Avviso''''''''''''''''''''''''''''''//
				//'''''''''Data  stampa Avviso''''''''''''''''''''''''''''''''''''//
			 if (document.getElementById('optStampaAvviso').checked)
			{
				if(IsBlank(document.getElementById('txtDataStampaAvvisoDal').value) && IsBlank(document.getElementById('txtDataStampaAvvisoAl').value)) 
				{
					
					sMsg=  sMsg + "[Date di Stampa Avviso obbligatorie!]\n" ; 
				}
				/*if (IsBlank(document.getElementById('txtDataStampaAvvisoDal').value) && !IsBlank(document.getElementById('txtDataStampaAvvisoAl').value)) 
				{
					sMsg=  sMsg + "[Date di Stampa Avviso DAL obbligatoria!]\n" ; 
				}*/
				if (!IsBlank(document.getElementById('txtDataStampaAvvisoDal').value) && !IsBlank(document.getElementById('txtDataStampaAvvisoAl').value)) 
				{
					if (!doDateCheck(document.getElementById('txtDataStampaAvvisoAl'),document.getElementById('txtDataStampaAvvisoDal')))
					{
						sMsg=  sMsg + "[Data di Stampa Avviso AL minore di Data di Stampa Avviso DAL!]\n" ; 
					}
				}
			
			}
			//'''''''''Fine Data  stampa Avviso'''''''''''''''''''''''''''''''''/
			//'''''''''Data  Consegna  Avviso'''''''''''''''''''''''''''''''''/
			if(document.getElementById('optConsegnaAvviso').checked)
			{
				if(IsBlank(document.getElementById('txtDataConsegnaAvvisoDal').value) && IsBlank(document.getElementById('txtDataConsegnaAvvisoAl').value)) 
				{
					sMsg=  sMsg + "[Date di Consegna Avviso obbligatorie!]\n" ; 
				}
				/*if (IsBlank(document.getElementById('txtDataConsegnaAvvisoDal').value) && !IsBlank(document.getElementById('txtDataConsegnaAvvisoAl').value)) 
				{
					sMsg=  sMsg + "[Date di Consegna Avviso DAL obbligatoria!]\n" ; 
				}*/
				if (!IsBlank(document.getElementById('txtDataConsegnaAvvisoDal').value) && !IsBlank(document.getElementById('txtDataConsegnaAvvisoAl').value)) 
				{
					if (!doDateCheck(document.getElementById('txtDataConsegnaAvvisoAl'),document.getElementById('txtDataConsegnaAvvisoDal')))
					{
						sMsg=  sMsg + "[Data di Consegna Avviso AL minore di Data di Consegna Avviso DAL!]\n" ; 
	
					}
				}
			}
			//'''''''''Fine Data  Consegna  Avviso'''''''''''''''''''''''''''''''''/

			if(document.getElementById('optNotificaAvviso').checked)
			{
				if(IsBlank(document.getElementById('txtDataNotificaAvvisoDal').value) && IsBlank(document.getElementById('txtDataNotificaAvvisoAl').value)) 
				{
							
				sMsg=  sMsg + "[Date di Notifica Avviso obbligatorie!]\n" ; 
				}
				/*if (IsBlank(document.getElementById('txtDataConsegnaAvvisoDal').value) && !IsBlank(document.getElementById('txtDataNotificaAvvisoAl').value)) 
				{
					sMsg=  sMsg + "[Date di Notifica Avviso DAL obbligatoria!]\n" ; 
				
				}*/
				if (!IsBlank(document.getElementById('txtDataNotificaAvvisoDal').value) && !IsBlank(document.getElementById('txtDataConsegnaAvvisoAl').value)) 
				{
					if (!doDateCheck(document.getElementById('txtDataNotificaAvvisoAl'),document.getElementById('txtDataNotificaAvvisoDal')))
					{
					
						sMsg=  sMsg + "[Data di Notifica Avviso AL minore di Data di Notifica Avviso DAL!]\n" ; 
					
					}
				
				}
			}
			//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
				if(document.getElementById('optRettificaAvviso').checked)
				{
					if(IsBlank(document.getElementById('txtDataRettificaAvvisoAl').value) && IsBlank(document.getElementById('txtDataRettificaAvvisoAl').value)) 
					{
								
					sMsg=  sMsg + "[Date di Rettifica Avviso obbligatorie!]\n" ; 
					}
				/*	if (IsBlank(document.getElementById('txtDataRettificaAvvisoDal').value) && !IsBlank(document.getElementById('txtDataRettificaAvvisoAl').value)) 
					{
					sMsg=  sMsg + "[Date di Rettifica Avviso DAL obbligatoria!]\n" ; 
					
						}*/
					if (!IsBlank(document.getElementById('txtDataRettificaAvvisoDal').value) && !IsBlank(document.getElementById('txtDataRettificaAvvisoAl').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataRettificaAvvisoAl'),document.getElementById('txtDataRettificaAvvisoDal')))
						{
						
							sMsg=  sMsg + "[Data di Rettifica Avviso AL minore di Data di Rettifica Avviso DAL!]\n" ; 
						
						}
					
					}
				}
				//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
				if(document.getElementById('optAnnulamentoAvviso').checked)
				{
					if(IsBlank(document.getElementById('txtDataAnnulamentoAvvisoDal').value) && IsBlank(document.getElementById('txtDataAnnulamentoAvvisoAl').value)) 
					{
								
					sMsg=  sMsg + "[Date di Annulamento Avviso obbligatorie!]\n" ; 
					}
					/*if (IsBlank(document.getElementById('txtDataAnnulamentoAvvisoDal').value) && !IsBlank(document.getElementById('txtDataRettificaAvvisoAl').value)) 
					{
						sMsg=  sMsg + "[Date di Annulamento Avviso DAL obbligatoria!]\n" ; 
					
					}*/
					if (!IsBlank(document.getElementById('txtDataAnnulamentoAvvisoDal').value) && !IsBlank(document.getElementById('txtDataAnnulamentoAvvisoAl').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataAnnulamentoAvvisoAl'),document.getElementById('txtDataAnnulamentoAvvisoDal')))
						{
						
							sMsg=  sMsg + "[Data di Annulamento Avviso AL minore di Data di Annulamento Avviso DAL!]\n" ; 
						
						}
					
					}
				}
				//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
				if(document.getElementById('optSopensioneAutotutela').checked)
				{
					if(IsBlank(document.getElementById('txtDataSopensioneAutotutelaDal').value) && IsBlank(document.getElementById('txtDataSopensioneAutotutelaAl').value)) 
					{
								
					sMsg=  sMsg + "[Date di Sopensione Autotutela Avviso obbligatorie!]\n" ; 
					}
					/*if (IsBlank(document.getElementById('txtDataSopensioneAutotutelaDal').value) && !IsBlank(document.getElementById('txtDataSopensioneAutotutelaAl').value)) 
					{
					sMsg=  sMsg + "[Date di Sopensione Autotutela Avviso DAL obbligatoria!]\n" ; 
					
						}*/
					if (!IsBlank(document.getElementById('txtDataSopensioneAutotutelaDal').value) && !IsBlank(document.getElementById('txtDataSopensioneAutotutelaAl').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataSopensioneAutotutelaAl'),document.getElementById('txtDataSopensioneAutotutelaDal')))
						{
						
							sMsg=  sMsg + "[Data di Sopensione Autotutela Avviso AL minore di Data di Sopensione Autotutela Avviso DAL!]\n" ; 
						
						}
					
					}
				}
				if(document.getElementById('optIrreperibile').checked)
				{
					if(IsBlank(document.getElementById('txtDataIrreperibileDal').value) && IsBlank(document.getElementById('txtDataIrreperibileAl').value)) 
					{
								
					sMsg=  sMsg + "[Date di Irreperibilità obbligatorie!]\n" ; 
					}
					/*if (IsBlank(document.getElementById('txtDataConsegnaAvvisoDal').value) && !IsBlank(document.getElementById('txtDataIrreperibileAl').value)) 
					{
						sMsg=  sMsg + "[Data di Irreperibilità DAL obbligatoria!]\n" ; 
					
					}*/
					if (!IsBlank(document.getElementById('txtDataIrreperibileDal').value) && !IsBlank(document.getElementById('txtDataConsegnaAvvisoAl').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataIrreperibileAl'),document.getElementById('txtDataIrreperibileDal')))
						{
						
							sMsg=  sMsg + "[Data di Irreperibilità AL minore di Data di Notifica Avviso DAL!]\n" ; 
						
						}
					
					}
				}
				//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
				if(document.getElementById('optRicorsoProvinciale').checked)
				{
					if(IsBlank(document.getElementById('txtDataRicorsoProvincialeDal').value) && IsBlank(document.getElementById('txtDataRicorsoProvincialeAl').value)) 
					{
								
					sMsg=  sMsg + "[Date di Ricorso Avviso 1° Grado di Giudizio obbligatorie!]\n" ; 
					}
					/*if (IsBlank(document.getElementById('txtDataRicorsoProvincialeDal').value) && !IsBlank(document.getElementById('txtDataRicorsoProvincialeAl').value)) 
					{
					sMsg=  sMsg + "[Date di Ricorso Provinciale Avviso DAL obbligatoria!]\n" ; 
					
						}*/
					if (!IsBlank(document.getElementById('txtDataRicorsoProvincialeDal').value) && !IsBlank(document.getElementById('txtDataRicorsoProvincialeAl').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataRicorsoProvincialeAl'),document.getElementById('txtDataRicorsoProvincialeDal')))
						{
						
							sMsg=  sMsg + "[Data di Ricorso Avviso 1° Grado di Giudizio -AL- minore di Data di Ricorso Avviso 1° Grado di Giudizio -DAL- !]\n" ; 
						
						}
					
					}
				}
				if(document.getElementById('optSopensioneProvinciale').checked)
				{
					if(IsBlank(document.getElementById('txtDataSopensioneProvincialeDal').value) && IsBlank(document.getElementById('txtDataSopensioneProvincialeAl').value)) 
					{
								
					sMsg=  sMsg + "[Date di Sopensione Avviso 1° Grado di Giudizio obbligatorie!]\n" ; 
					}
					/*if (IsBlank(document.getElementById('txtDataSopensioneProvincialeDal').value) && !IsBlank(document.getElementById('txtDataSopensioneProvincialeAl').value)) 
					{
						sMsg=  sMsg + "[Date di Sopensione Provinciale Avviso DAL obbligatoria!]\n" ; 
					
					}*/
					if (!IsBlank(document.getElementById('txtDataSopensioneProvincialeDal').value) && !IsBlank(document.getElementById('txtDataSopensioneProvincialeAl').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataSopensioneProvincialeAl'),document.getElementById('txtDataSopensioneProvincialeDal')))
						{
						
							sMsg=  sMsg + "[Data di Sopensione Avviso 1° Grado di Giudizio -AL- minore di Data di Sopensione Avviso 1° Grado di Giudizio -DAL- !]\n" ; 
						
						}
					
					}
				}
				if(document.getElementById('optSentenzaProvinciale').checked)
				{
					if(IsBlank(document.getElementById('txtDataSentenzaProvincialeDal').value) && IsBlank(document.getElementById('txtDataSentenzaProvincialeAl').value)) 
					{
								
					sMsg=  sMsg + "[Date di Sentenza Avviso 1° Grado di Giudizio obbligatorie!]\n" ; 
					}
					/*if (IsBlank(document.getElementById('txtDataSentenzaProvincialeDal').value) && !IsBlank(document.getElementById('txtDataSentenzaProvincialeAl').value)) 
					{
						sMsg=  sMsg + "[Date di Sentenza Provinciale Avviso DAL obbligatoria!]\n" ; 
					
					}*/
					if (!IsBlank(document.getElementById('txtDataSentenzaProvincialeDal').value) && !IsBlank(document.getElementById('txtDataSentenzaProvincialeAl').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataSentenzaProvincialeAl'),document.getElementById('txtDataSentenzaProvincialeDal')))
						{
						
							sMsg=  sMsg + "[Data di Sentenza Avviso 1° Grado di Giudizio -AL- minore di Data di Sentenza Avviso 1° Grado di Giudizio -DAL- !]\n" ; 
						
						}
					
					}
				}
				//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
				if(document.getElementById('optRicorsoRegionale').checked)
				{
					if(IsBlank(document.getElementById('txtDataRicorsoRegionaleDal').value) && IsBlank(document.getElementById('txtDataRicorsoRegionaleAl').value)) 
					{
								
					sMsg=  sMsg + "[Date di Ricorso Avviso 2° Grado di Giudizio obbligatorie!]\n" ; 
					}
					/*if (IsBlank(document.getElementById('txtDataRicorsoRegionaleDal').value) && !IsBlank(document.getElementById('txtDataRicorsoRegionaleAl').value)) 
					{
						sMsg=  sMsg + "[Date di Ricorso Regionale Avviso DAL obbligatoria!]\n" ; 
					
					}*/
					if (!IsBlank(document.getElementById('txtDataRicorsoRegionaleDal').value) && !IsBlank(document.getElementById('txtDataRicorsoRegionaleAl').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataRicorsoRegionaleAl'),document.getElementById('txtDataRicorsoRegionaleDal')))
						{
						
							sMsg=  sMsg + "[Data di Ricorso Avviso 2° Grado di Giudizio -AL- minore di Data di Ricorso Avviso 2° Grado di Giudizio -DAL- !]\n" ; 
						
						}
					
					}
				}
				if(document.getElementById('optSopensioneRegionale').checked)
				{
					if(IsBlank(document.getElementById('txtDataSopensioneRegionaleDal').value) && IsBlank(document.getElementById('txtDataSopensioneRegionaleAl').value)) 
					{
								
					sMsg=  sMsg + "[Date di Sopensione Avviso 2° Grado di Giudizio obbligatorie!]\n" ; 
					}
					/*if (IsBlank(document.getElementById('txtDataSopensioneRegionaleDal').value) && !IsBlank(document.getElementById('txtDataSopensioneRegionaleAl').value)) 
					{
					sMsg=  sMsg + "[Date di Sopensione Regionale Avviso DAL obbligatoria!]\n" ; 
					
						}*/
					if (!IsBlank(document.getElementById('txtDataSopensioneRegionaleDal').value) && !IsBlank(document.getElementById('txtDataSopensioneRegionaleAl').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataSopensioneRegionaleAl'),document.getElementById('txtDataSopensioneRegionaleDal')))
						{
						
							sMsg=  sMsg + "[Data di Sopensione Avviso 2° Grado di Giudizio -AL- minore di Data di Sopensione Avviso 2° Grado di Giudizio -DAL- !]\n" ; 
						
						}
					
					}
				}
				if(document.getElementById('optSentenzaRegionale').checked)
				{
					if(IsBlank(document.getElementById('txtDataSentenzaRegionaleDal').value) && IsBlank(document.getElementById('txtDataSentenzaRegionaleAl').value)) 
					{
								
					sMsg=  sMsg + "[Date di Sentenza Avviso 2° Grado di Giudizio obbligatorie!]\n" ; 
					}
					/*if (IsBlank(document.getElementById('txtDataSentenzaRegionaleDal').value) && !IsBlank(document.getElementById('txtDataSentenzaRegionaleAl').value)) 
					{
					sMsg=  sMsg + "[Date di Sentenza Regionale Avviso DAL obbligatoria!]\n" ; 
					
						}*/
					if (!IsBlank(document.getElementById('txtDataSentenzaRegionaleDal').value) && !IsBlank(document.getElementById('txtDataSentenzaRegionaleAl').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataSentenzaRegionaleAl'),document.getElementById('txtDataSentenzaRegionaleDal')))
						{
						
							sMsg=  sMsg + "[Data di Sentenza Avviso 2° Grado di Giudizio -AL- minore di Data di Sentenza Avviso 2° Grado di Giudizio -DAL- !]\n" ; 
						
						}
					
					}
				
				
				}
		
				//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
				if(document.getElementById('optRicorsoCassazione').checked)
				{
					if(IsBlank(document.getElementById('txtDataRicorsoCassazioneDal').value) && IsBlank(document.getElementById('txtDataRicorsoCassazioneAl').value)) 
					{
								
					sMsg=  sMsg + "[Date di Ricorso Avviso 3° Grado di Giudizio obbligatorie!]\n" ; 
					}
					/*if (IsBlank(document.getElementById('txtDataRicorsoCassazioneDal').value) && !IsBlank(document.getElementById('txtDataRicorsoCassazioneAl').value)) 
					{
						sMsg=  sMsg + "[Date di Ricorso Cassazione Avviso DAL obbligatoria!]\n" ; 					
					}*/
					if (!IsBlank(document.getElementById('txtDataRicorsoCassazioneDal').value) && !IsBlank(document.getElementById('txtDataRicorsoCassazioneAl').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataRicorsoCassazioneAl'),document.getElementById('txtDataRicorsoCassazioneDal')))
						{
						
							sMsg=  sMsg + "[Data di Ricorso Avviso 3° Grado di Giudizio -AL- minore di Data di Ricorso Avviso 3° Grado di Giudizio -DAL- !]\n" ; 
						
						}
					
					}
				}
				if(document.getElementById('optSopensioneCassazione').checked)
				{
					if(IsBlank(document.getElementById('txtDataSopensioneCassazioneDal').value) && IsBlank(document.getElementById('txtDataSopensioneCassazioneAl').value)) 
					{
								
					sMsg=  sMsg + "[Date di Sopensione Avviso 3° Grado di Giudizio obbligatorie!]\n" ; 
					}
					/*if (IsBlank(document.getElementById('txtDataSopensioneCassazioneDal').value) && !IsBlank(document.getElementById('txtDataSopensioneCassazioneAl').value)) 
					{
					  sMsg=  sMsg + "[Date di Sopensione Cassazione Avviso DAL obbligatoria!]\n" ; 					
					}*/
					if (!IsBlank(document.getElementById('txtDataSopensioneCassazioneDal').value) && !IsBlank(document.getElementById('txtDataSopensioneCassazioneAl').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataSopensioneCassazioneAl'),document.getElementById('txtDataSopensioneCassazioneDal')))
						{
						
							sMsg=  sMsg + "[Data di Sopensione Avviso 3° Grado di Giudizio -AL- minore di Data di Sopensione Avviso 3° Grado di Giudizio DAL- !]\n" ; 
						
						}
					
					}
				}
				if(document.getElementById('optSentenzaCassazione').checked)
				{
					if(IsBlank(document.getElementById('txtDataSentenzaCassazioneDal').value) && IsBlank(document.getElementById('txtDataSentenzaCassazioneAl').value)) 
					{
								
					sMsg=  sMsg + "[Date di Sentenza Avviso 3° Grado di Giudizio obbligatorie!]\n" ; 
					}
					/*if (IsBlank(document.getElementById('txtDataSentenzaCassazioneDal').value) && !IsBlank(document.getElementById('txtDataSentenzaCassazioneAl').value)) 
					{
						sMsg=  sMsg + "[Date di Sentenza Cassazione  Avviso DAL obbligatoria!]\n" ; 
					
					}*/
					if (!IsBlank(document.getElementById('txtDataSentenzaCassazioneDal').value) && !IsBlank(document.getElementById('txtDataSentenzaCassazioneAl').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataSentenzaCassazioneAl'),document.getElementById('txtDataSentenzaCassazioneDal')))
						{
						
							sMsg=  sMsg + "[Data di Sentenza Avviso 3° Grado di Giudizio -AL- minore di Data di Sentenza Avviso 3° Grado di Giudizio -DAL- !]\n" ; 
						
						}
					
					}
				}
				
				//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
				
				if(document.getElementById('optAttoDefinitivo').checked)
				{
					if(IsBlank(document.getElementById('txtDataAttoDefinitivoDal').value) && IsBlank(document.getElementById('txtDataAttoDefinitivoAl').value)) 
					{
								
					sMsg=  sMsg + "[Date di Atto Definitivo Avviso obbligatorie!]\n" ; 
					}
					/*if (IsBlank(document.getElementById('txtDataAttoDefinitivoDal').value) && !IsBlank(document.getElementById('txtDataAttoDefinitivoAl').value)) 
					{
					  sMsg=  sMsg + "[Date di Atto Definitivo Avviso DAL obbligatoria!]\n" ; 
					
					}*/
					if (!IsBlank(document.getElementById('txtDataAttoDefinitivoDal').value) && !IsBlank(document.getElementById('txtDataAttoDefinitivoAl').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataAttoDefinitivoAl'),document.getElementById('txtDataAttoDefinitivoDal')))
						{
						
							sMsg=  sMsg + "[Data di Atto Definitivo Avviso AL minore di Data di Atto Definitivo Avviso DAL!]\n" ; 
						
						}
					
					}
				}
				if(document.getElementById('optPagamento').checked)
				{
					if(IsBlank(document.getElementById('txtDataPagamentoDal').value) && IsBlank(document.getElementById('txtDataPagamentoAl').value)) 
					{
								
					sMsg=  sMsg + "[Date di Data Pagamento Avviso obbligatorie!]\n" ; 
					}
					/*if (IsBlank(document.getElementById('txtDataPagamentoDal').value) && !IsBlank(document.getElementById('txtDataPagamentoAl').value)) 
					{
					sMsg=  sMsg + "[Date di Data Pagamento Avviso DAL obbligatoria!]\n" ; 
					
						}*/
					if (!IsBlank(document.getElementById('txtDataPagamentoDal').value) && !IsBlank(document.getElementById('txtDataPagamentoAl').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataPagamentoAl'),document.getElementById('txtDataPagamentoDal')))
						{
						
							sMsg=  sMsg + "[Data di Data Pagamento Avviso AL minore di Data di Data Pagamento Avviso DAL!]\n" ; 
						
						}
					
					}
				}
				
				//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/			
				if(document.getElementById('optSollecitoBonario').checked)
				{
					if(IsBlank(document.getElementById('txtDataSollecitoBonarioDal').value) && IsBlank(document.getElementById('txtDataSollecitoBonarioAl').value)) 
					{
								
					sMsg=  sMsg + "[Date di Sollecito Bonario Avviso obbligatorie!]\n" ; 
					}
					/*if (IsBlank(document.getElementById('txtDataSollecitoBonarioDal').value) && !IsBlank(document.getElementById('txtDataSollecitoBonarioAl').value)) 
					{
					 sMsg=  sMsg + "[Date di Sollecito Bonario Avviso DAL obbligatoria!]\n" ; 
					
					}*/
					if (!IsBlank(document.getElementById('txtDataSollecitoBonarioDal').value) && !IsBlank(document.getElementById('txtDataSollecitoBonarioAl').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataSollecitoBonarioAl'),document.getElementById('txtDataSollecitoBonarioDal')))
						{
						
							sMsg=  sMsg + "[Data di Sollecito Bonario Avviso AL minore di Data di Sollecito Bonario Avviso DAL!]\n" ; 
						
						}
					
					}
				}
				if(document.getElementById('optRuoloOrdinario').checked)
				{
					if(IsBlank(document.getElementById('txtDataRuoloOrdinarioDal').value) && IsBlank(document.getElementById('txtDataPagamentoAl').value)) 
					{
								
					sMsg=  sMsg + "[Date di Ruolo Ordinario Avviso obbligatorie!]\n" ; 
					}
					/*if (IsBlank(document.getElementById('txtDataRuoloOrdinarioDal').value) && !IsBlank(document.getElementById('txtDataPagamentoAl').value)) 
					{
						sMsg=  sMsg + "[Date di Ruolo Ordinario Avviso DAL obbligatoria!]\n" ; 
					
					}*/
					if (!IsBlank(document.getElementById('txtDataRuoloOrdinarioDal').value) && !IsBlank(document.getElementById('txtDataPagamentoAl').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataPagamentoAl'),document.getElementById('txtDataRuoloOrdinarioDal')))
						{
						
							sMsg=  sMsg + "[Data di Ruolo Ordinario Avviso AL minore di Data di Ruolo Ordinario Avviso DAL!]\n" ; 
						
						}
					
					}
				}
				if(document.getElementById('optCoattivo').checked)
				{
					if(IsBlank(document.getElementById('txtDataCoattivoDal').value) && IsBlank(document.getElementById('txtDataCoattivoAl').value)) 
					{
								
					sMsg=  sMsg + "[Date di Coattivo obbligatorie!]\n" ; 
					}
					/*if (IsBlank(document.getElementById('txtDataCoattivoDal').value) && !IsBlank(document.getElementById('txtDataCoattivoAl').value)) 
					{
						sMsg=  sMsg + "[Date Coattivo DAL obbligatoria!]\n" ; 
					
					}*/
					if (!IsBlank(document.getElementById('txtDataCoattivoDal').value) && !IsBlank(document.getElementById('txtDataCoattivoAl').value)) 
					{
						if (!doDateCheck(document.getElementById('txtDataCoattivoAl'),document.getElementById('txtDataCoattivoDal')))
						{
						
							sMsg=  sMsg + "[Data Coattivo  Avviso AL minore di Data  Coattivo DAL!]\n" ; 
						
						}
					
					}
				}
				
				if (!IsBlank(sMsg)) 
				{ 
						strMessage ="Attenzione...\n\n I campi elencati sono obbligatori!\n\n" 
						GestAlert('a', 'warning', '', '', strMessage + sMsg);
						return false; 
				}		
				
		
				return true; 
			
		}
		function Search()
		{			
			if(VerificaCampi())
			{
				attesaGestioneAtti.style.display='';
				document.getElementById('CmdSearch').click();				
				document.getElementById('Date').style.display ='none';
				document.getElementById('txtDIVDate').value ='-1'
				return true;
			}
			else
			{			
				return false;
			}			
		}
		function pulisciCampi()
		{
				
			elm=Form1.elements;
	
			for(i=0;i<elm.length;i++)
			{	
				if(elm[i].type=="checkbox")
				{
					elm[i].checked=false;
				}
				if(elm[i].type=="radio")
				{
					
					elm[i].checked=false;
					elm[i].disabled=true;
				}
				if(elm[i].type=="text")
				{
					
					elm[i].value='';
					elm[i].disabled=true;
				}
			}		
				
			document.getElementById('ddlAnno').value='-1';
			document.getElementById('ddlTributo').value='-1';
			document.getElementById('ddlTipologiaTributo').value='-1';
				
			document.getElementById('txtCheckBox').value='-1';
			document.getElementById('loadGrid').src="../../../aspVuota.aspx";
			document.getElementById('Date').style.display ='none';
			document.getElementById('txtDIVDate').value ='-1'
			attesaGestioneAtti.style.display='none';
			parent.Comandi.Excel.style.display="none";
			
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
		function AttivaOption(nameckeck,opt1,DataDal,DataAl,opt2)
		{
		    IDckeck=nameckeck.id;
			IDopt1=opt1.id;
			IDopt2=opt2.id;
			IDDataDal=DataDal.id;
			IDDataAl=DataAl.id;
			if(document.getElementById(IDckeck).checked==true)
			{
			    document.getElementById(IDopt1).disabled = false;
			    document.getElementById(IDopt2).disabled = false;
			    document.getElementById(IDopt2).checked = true;
			    /**** 20171219 - estrazione 290 non ancora da rilasciare perché non ancora comprata da nessuno ***
                if (IDckeck == 'chkDataCoattivo') {
			      document.getElementById('LblIntest290').style.display = '';
			      document.getElementById('LblFile290').style.display = '';
			      parent.Comandi.document.getElementById('CreaFile').style.display = '';
			    }*/
			}
			else
			{
			  document.getElementById(IDDataDal).disabled=true;
			  document.getElementById(IDDataAl).disabled=true;
			  document.getElementById(IDopt1).disabled=true;
			  document.getElementById(IDopt2).disabled=true;
			  document.getElementById(IDDataDal).value='';
			  document.getElementById(IDDataAl).value='';
			  document.getElementById(IDopt1).checked=false;
			  document.getElementById(IDopt2).checked = false;
			  if (IDckeck == 'chkDataCoattivo') {
			      document.getElementById('LblIntest290').style.display = 'none';
			      document.getElementById('LblFile290').style.display = 'none';
			      parent.Comandi.document.getElementById('CreaFile').style.display = 'none';
			  }
			}	
		}
		function AttivaOptionElaborazione(nameckeck,opt1,DataDal,DataAl)
		{			
			IDckeck=nameckeck.id;
			IDopt1=opt1.id;
			
			IDDataDal=DataDal.id;
			IDDataAl=DataAl.id;
			if(document.getElementById(IDckeck).checked==true)
			{
			  document.getElementById(IDopt1).disabled=false;
			  document.getElementById(IDopt1).checked=true;
			    document.getElementById(IDDataDal).disabled=false;
			  document.getElementById(IDDataAl).disabled=false;
			}
			else
			{
			  document.getElementById(IDDataDal).disabled=true;
			  document.getElementById(IDDataAl).disabled=true;
			  document.getElementById(IDopt1).disabled=true;
			 
			  document.getElementById(IDDataDal).value='';
			  document.getElementById(IDDataAl).value='';
			  document.getElementById(IDopt1).checked=false;		  
			  
			}				
		}
		function AttivaDate(DataDal,DataAl)
		{
			 IDDataDal=DataDal.id;
			 IDDataAl=DataAl.id;
			 document.getElementById(IDDataDal).disabled=false;
			 document.getElementById(IDDataAl).disabled=false;
			
		}
		function DissativaDate(DataDal,DataAl)
		{
			 IDDataDal=DataDal.id;
			 IDDataAl=DataAl.id;
			 document.getElementById(IDDataDal).disabled=true;
			 document.getElementById(IDDataAl).disabled=true;
			 document.getElementById(IDDataDal).value='';
			 document.getElementById(IDDataAl).value='';
			
		}		
        function doDateCheck(from, to) 
        {
	        var ndfrom=new Date (dateITtoEN(from.value))
	        var ndto=new Date (dateITtoEN(to.value))
	        if (Date.parse(ndfrom) >= Date.parse(ndto)) 
	        {
	          return true;
	        }
	        else 
	        {
	          return false;
           }
        }
        function dateITtoEN(value){
	        var data=new String(value)
	        gg=data.substr(0,2)
	        mm=data.substr(3,2)
	        yy=data.substr(6,4)
	        newdate=mm+"/"+gg+"/"+yy
	        return newdate 
        }
        function AttivaDateRicorso()
        {	
	        if(document.getElementById('txtDateRicorso').value =='1')
	        {
		        document.getElementById('provinciale_1').style.display ='none';
		        document.getElementById('provinciale_2').style.display ='none';
		        document.getElementById('provinciale_3').style.display ='none';
	
		        document.getElementById('regionale_1').style.display ='none';
		        document.getElementById('regionale_2').style.display ='none';
		        document.getElementById('regionale_3').style.display ='none';
	
		        document.getElementById('cassazione_1').style.display ='none';
		        document.getElementById('cassazione_2').style.display ='none';
		        document.getElementById('cassazione_3').style.display ='none';
		        document.getElementById('txtDateRicorso').value ='-1';
		
		        document.getElementById('btnDateRicorso_1').style.display ='none';
		        document.getElementById('btnDateRicorso').value='Visualizzazione date Ricorso';
	        }
	        else
	        {		
		        document.getElementById('provinciale_1').style.display ='';
		        document.getElementById('provinciale_2').style.display ='';
		        document.getElementById('provinciale_3').style.display ='';
	
		        document.getElementById('regionale_1').style.display ='';
		        document.getElementById('regionale_2').style.display ='';
		        document.getElementById('regionale_3').style.display ='';
	
		        document.getElementById('cassazione_1').style.display ='';
		        document.getElementById('cassazione_2').style.display ='';
		        document.getElementById('cassazione_3').style.display ='';
		
		        document.getElementById('txtDateRicorso').value ='1';
		        document.getElementById('btnDateRicorso_1').style.display ='';
		        document.getElementById('btnDateRicorso').value='Nascondi Date Ricorso';		
	        }
        }
        function VerificaDIV()
        {

         if(document.getElementById('txtDIVDate').value =='1')
         {
	        document.getElementById('Date').style.display ='';
         }
         else
         {
	        document.getElementById('Date').style.display ='none';
         }

        }
        function Massiva()
        {	
	        if(document.getElementById('txtRicercaAttiva').value== "1")
	        {	
		        document.getElementById('Date').style.display = '';
	            document.getElementById('divAggMassivo').style.display = '';
	        }
	        else
	        {
	            GestAlert('a', 'warning', '', '', 'Eseguire la ricerca prima di fare l\'aggiornamento massivo!');	
	        }	
        }	
	    function StampaExcel(){
		    document.getElementById("btnStampaExcel").click();
	    }
	    function Estrazione290() {
	        document.getElementById("CmdEstrazione290").click();
	    }
    </script>
</head>
<body class="SfondoVisualizza" leftmargin="20" rightmargin="0" ms_positioning="GridLayout" onload="Form1.ddlAnno.focus();">
    <form id="Form1" runat="server" method="post">
        <fieldset class="FiledSetRicerca">
            <legend class="Legend">Modalità di Ricerca Avanzata Atti</legend>
            <div class="col-md-12">
                <div id="divEnti" class="col-md-2">
                    <asp:Label ID="lblEnti" runat="server" CssClass="Input_Label">Ente</asp:Label><br />
                    <asp:DropDownList ID="ddlEnti" runat="server" CssClass="Input_Text" DataTextField="string" DataValueField="string"></asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblAnno" runat="server" CssClass="Input_Label">Anno</asp:Label><br />
                    <asp:DropDownList ID="ddlAnno" onkeydown="keyPress();" TabIndex="0" runat="server" AutoPostBack="True" CssClass="Input_Text" Width="100px"> </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <asp:Label ID="lblTributo" runat="server" CssClass="Input_Label">Tributo</asp:Label><br />
                    <asp:DropDownList ID="ddlTributo" onkeydown="keyPress();" TabIndex="1" runat="server" AutoPostBack="True" CssClass="Input_Text" Width="200px"></asp:DropDownList>
                </div>
                <div class="col-md-4">
                    <asp:Label ID="lblTipologiaAtto" runat="server" CssClass="Input_Label">Tipologia Atto</asp:Label><br />
                    <asp:DropDownList ID="ddlTipologiaTributo" onkeydown="keyPress();" TabIndex="2" runat="server" CssClass="Input_Text" Width="300px"></asp:DropDownList>
                </div>
            </div>
        </fieldset>
        <div id="Date" style="display: none">
            <fieldset class="classeFiledSet">
                <legend class="Legend">Filtri per Data</legend>
                <table cellspacing="0" cellpadding="1" width="100%" align="left" border="0">
                    <tr>
                        <td colspan="6">
                            <input class="btnList_botton" id="btnDateRicorso_1" style="display: none; width: 184px; height: 15px" onclick="AttivaDateRicorso();" type="button" value="Nascondi Date Ricorso">
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td class="Input_Label">Dal</td>
                        <td class="Input_Label">Al</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <!-- Data Generazione Avviso-->
                    <tr>
                        <td class="Input_Label">
                            <asp:CheckBox ID="chkDataGenerazione" onclick="AttivaOptionElaborazione(document.getElementById('chkDataGenerazione'),document.getElementById('optGenerazione'),document.getElementById('txtDataGenerazioneDal'),document.getElementById('txtDataGenerazioneAL'));" TabIndex="3" runat="server" Text="Elaborazione"></asp:CheckBox>
                        </td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optGenerazione" runat="server" onclick="AttivaDate(document.getElementById('txtDataGenerazioneDal'),document.getElementById('txtDataGenerazioneAL'));" tabindex="4" Text="" GroupName="DataGenerazione" />
						</td>
                        <td>
                            <asp:TextBox ID="txtDataGenerazioneDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="5" runat="server" name="txtDataGenerazioneDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDataGenerazioneAL" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="6" runat="server" name="txtDataGenerazioneAL" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label">&nbsp;</td>
                    </tr>
                    <!-- Fine Data Generazione Avviso-->
                    <!-- Data Conferma Avviso-->
                    <tr>
                        <td class="Input_Label">
                            <asp:CheckBox ID="chkDataConfermaAvviso" onclick="AttivaOption(document.getElementById('chkDataConfermaAvviso'),document.getElementById('optConfermaAvviso'),document.getElementById('txtDataConfermaAvvisoDal'),document.getElementById('txtDataConfermaAvvisoAl'),document.getElementById('optConfermaAvvisoNoDate'));"
                                TabIndex="8" runat="server" Text="Conferma Avviso"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optConfermaAvviso" runat="server" onclick="AttivaDate(document.getElementById('txtDataConfermaAvvisoDal'),document.getElementById('txtDataConfermaAvvisoAl'));" tabindex="9" Text="" GroupName="DataConfermaAvviso" />
						</td>
                        <td>
                            <asp:TextBox ID="txtDataConfermaAvvisoDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="10" runat="server" name="txtDataConfermaAvvisoDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataConfermaAvvisoAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="11" runat="server" name="txtDataConfermaAvvisoAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optConfermaAvvisoNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataConfermaAvvisoDal'),document.getElementById('txtDataConfermaAvvisoAl'));" Text="Nessuna"  GroupName="DataConfermaAvviso"/>
						</td>
                    </tr>
                    <!-- Fine Data Conferma Avviso-->
                    <!-- Fine Data Stampa Avviso-->
                    <tr>
                        <td class="Input_Label">
                            <asp:CheckBox ID="chkDataStampaAvviso" onclick="AttivaOption(document.getElementById('chkDataStampaAvviso'),document.getElementById('optStampaAvviso'),document.getElementById('txtDataStampaAvvisoDal'),document.getElementById('txtDataStampaAvvisoAl'),document.getElementById('optStampaAvvisoNoDate'));"
                                TabIndex="13" runat="server" Text="Stampa Avviso"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optStampaAvviso" runat="server" onclick="AttivaDate(document.getElementById('txtDataStampaAvvisoDal'),document.getElementById('txtDataStampaAvvisoAl'));" Text="" GroupName="DataStampa" />

						</td>
                        <td>
                            <asp:TextBox ID="txtDataStampaAvvisoDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="15" runat="server" name="txtDataStampaAvvisoDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataStampaAvvisoAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="16" runat="server" name="txtDataStampaAvvisoAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optStampaAvvisoNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataStampaAvvisoDal'),document.getElementById('txtDataStampaAvvisoAl'));" Text="Nessuna" GroupName="DataStampa" />
                    </tr>
                    <!-- Fine Data Stampa Avviso-->
                    <!-- Data Consegna Avviso-->
                    <tr>
                        <td class="Input_Label">
                            <asp:CheckBox ID="chkDataConsegnaAvviso" onclick="AttivaOption(document.getElementById('chkDataConsegnaAvviso'),document.getElementById('optConsegnaAvviso'),document.getElementById('txtDataConsegnaAvvisoDal'),document.getElementById('txtDataConsegnaAvvisoAl'),document.getElementById('optConsegnaAvvisoNoDate'));"
                                TabIndex="18" runat="server" Text="Consegna Avviso"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optConsegnaAvviso" runat="server" onclick="AttivaDate(document.getElementById('txtDataConsegnaAvvisoDal'),document.getElementById('txtDataConsegnaAvvisoAl'));" tabindex="19" Text="" GroupName="DataConsegnaAvviso"/></td>
                        <td>
                            <asp:TextBox ID="txtDataConsegnaAvvisoDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="20" runat="server" name="txtDataConsegnaAvvisoDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataConsegnaAvvisoAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="21" runat="server" name="txtDataConsegnaAvvisoAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optConsegnaAvvisoNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataConsegnaAvvisoDal'),document.getElementById('txtDataConsegnaAvvisoAl'));" Text="Nessuna" GroupName="DataConsegnaAvviso" />

						</td>
                    </tr>
                    <!-- Fine Data Consegna Avviso-->
                    <!--  Data Notifica Avviso-->
                    <!--  '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''//-->
                    <tr>
                        <td class="Input_Label">
                            <asp:CheckBox ID="chkDataNotificaAvviso" onclick="AttivaOption(document.getElementById('chkDataNotificaAvviso'),document.getElementById('optNotificaAvviso'),document.getElementById('txtDataNotificaAvvisoDal'),document.getElementById('txtDataNotificaAvvisoAl'),document.getElementById('optNotificaAvvisoNoDate'));"
                                TabIndex="23" runat="server" Text="Notifica Avviso"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optNotificaAvviso" runat="server" onclick="AttivaDate(document.getElementById('txtDataNotificaAvvisoDal'),document.getElementById('txtDataNotificaAvvisoAl'));"
                                tabindex="24" Text="" GroupName="DataNotificaAvviso" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDataNotificaAvvisoDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="25" runat="server" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataNotificaAvvisoAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="26" runat="server" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton  id="optNotificaAvvisoNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataNotificaAvvisoDal'),document.getElementById('txtDataNotificaAvvisoAl'));" Text="Nessuna" GroupName="DataNotificaAvviso" />
                        </td>
                    </tr>
                    <!--  Data Notifica Avviso-->
                    <!--  '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''//-->
                    <tr>
                        <td class="Input_Label">
                            <asp:CheckBox ID="chkDataRettificaAvviso" onclick="AttivaOption(document.getElementById('chkDataRettificaAvviso'),document.getElementById('optRettificaAvviso'),document.getElementById('txtDataRettificaAvvisoDal'),document.getElementById('txtDataRettificaAvvisoAl'),document.getElementById('optRettificaAvvisoNoDate'));"
                                TabIndex="28" runat="server" Text="Rettifica Avviso"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optRettificaAvviso" runat="server" onclick="AttivaDate(document.getElementById('txtDataRettificaAvvisoDal'),document.getElementById('txtDataRettificaAvvisoAl'));"
                                tabindex="29" Text="" GroupName="DataRettificaAvviso" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDataRettificaAvvisoDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="30" runat="server" name="txtDataRettificaAvvisoDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataRettificaAvvisoAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="31" runat="server" name="txtDataRettificaAvvisoAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optRettificaAvvisoNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataRettificaAvvisoDal'),document.getElementById('txtDataRettificaAvvisoAl'));" Text="Nessuna" GroupName="DataRettificaAvviso" />
                        </td>
                    </tr>
                    <tr>
                        <td class="Input_Label">
                            <asp:CheckBox ID="chkDataAnnulamentoAvviso" onclick="AttivaOption(document.getElementById('chkDataAnnulamentoAvviso'),document.getElementById('optAnnulamentoAvviso'),document.getElementById('txtDataAnnulamentoAvvisoDal'),document.getElementById('txtDataAnnulamentoAvvisoAl'),document.getElementById('optAnnullamentoAvvisoNoDate'));"
                                TabIndex="33" runat="server" Text="Annulamento Avviso"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optAnnulamentoAvviso" runat="server" onclick="AttivaDate(document.getElementById('txtDataAnnulamentoAvvisoDal'),document.getElementById('txtDataAnnulamentoAvvisoAl'));"
                                tabindex="34" Text="" GroupName="DataAnnulamentoAvviso"/></td>
                        <td>
                            <asp:TextBox ID="txtDataAnnulamentoAvvisoDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="35" runat="server" name="txtDataAnnulamentoAvvisoDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataAnnulamentoAvvisoAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="36" runat="server" name="txtDataAnnulamentoAvvisoAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton  id="optAnnullamentoAvvisoNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataAnnulamentoAvvisoDal'),document.getElementById('txtDataAnnulamentoAvvisoAl'));" Text="Nessuna" GroupName="DataAnnulamentoAvviso"/>

						</td>
                    </tr>
                    <tr>
                        <td class="Input_Label">
                            <asp:CheckBox ID="chkDataSopensioneAutotutela" onclick="AttivaOption(document.getElementById('chkDataSopensioneAutotutela'),document.getElementById('optSopensioneAutotutela'),document.getElementById('txtDataSopensioneAutotutelaDal'),document.getElementById('txtDataSopensioneAutotutelaAl'),document.getElementById('optSospensioneAutotutelaNoDate'));"
                                TabIndex="38" runat="server" Text="Sopensione Autotutela"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optSopensioneAutotutela" runat="server" onclick="AttivaDate(document.getElementById('txtDataSopensioneAutotutelaDal'),document.getElementById('txtDataSopensioneAutotutelaAl'));"
                                tabindex="39" Text="" GroupName="DataSopensioneAutotutela"/></td>
                        <td>
                            <asp:TextBox ID="txtDataSopensioneAutotutelaDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="40" runat="server" name="txtDataSopensioneAutotutelaDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataSopensioneAutotutelaAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="41" runat="server" name="txtDataSopensioneAutotutelaAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optSospensioneAutotutelaNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataSopensioneAutotutelaDal'),document.getElementById('txtDataSopensioneAutotutelaAl'));" Text="Nessuna" GroupName="DataSopensioneAutotutela"/>

						</td>
                    </tr>
                    <!--  Data Irreperibilità-->
                    <!--  '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''//-->
                    <tr class="hidden">
                        <td class="Input_Label">
                            <asp:CheckBox ID="chkDataIrreperibile" onclick="AttivaOption(document.getElementById('chkDataIrreperibile'),document.getElementById('optIrreperibile'),document.getElementById('txtDataIrreperibileDal'),document.getElementById('txtDataIrreperibileAl'),document.getElementById('optIrreperibileNoDate'));"
                                TabIndex="23" runat="server" Text="Irreperibilità"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optIrreperibile" runat="server"  onclick="AttivaDate(document.getElementById('txtDataIrreperibileDal'),document.getElementById('txtDataIrreperibileAl'));"
                                tabindex="24" Text="" GroupName="DataIrreperibile" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDataIrreperibileDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="25" runat="server" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataIrreperibileAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="26" runat="server" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optIrreperibileNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataIrreperibileDal'),document.getElementById('txtDataIrreperibileAl'));"
                                tabindex="27" Text="Nessuna" GroupName="DataIrreperibile"/></td>
                    </tr>
                    <!--  Data Irreperibilità-->
                    <!--'''''''''''''''''''''''''''''''''''''''''''''//RICORSO'''''''''''''''''''''''''''//-->
                    <!--'''''''''''''''''''''''''''''''''''''''''''''//PROVINCIALE'''''''''''''''''''''''''''//-->
                    <tr id="provinciale_1" style="display: none">
                        <td class="Input_Label tdt tdl">
                            <asp:CheckBox ID="chkDataRicorsoProvinciale" onclick="AttivaOption(document.getElementById('chkDataRicorsoProvinciale'),document.getElementById('optRicorsoProvinciale'),document.getElementById('txtDataRicorsoProvincialeDal'),document.getElementById('txtDataRicorsoProvincialeAl'),document.getElementById('optRicorsoProvincialeNoDate'));"
                                TabIndex="43" runat="server" Text="Ricorso 1° Grado di Giudizio"></asp:CheckBox></td>
                        <td class="Input_Label tdt">
                            <asp:RadioButton id="optRicorsoProvinciale" runat="server" onclick="AttivaDate(document.getElementById('txtDataRicorsoProvincialeDal'),document.getElementById('txtDataRicorsoProvincialeAl'));"
                                tabindex="44" Text="" GroupName="DataRicorsoProvinciale"/></td>
                        <td class="tdt">
                            <asp:TextBox ID="txtDataRicorsoProvincialeDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="45" runat="server" name="txtDataRicorsoProvincialeDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="tdt">
                            <asp:TextBox ID="txtDataRicorsoProvincialeAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="46" runat="server" name="txtDataRicorsoProvincialeAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label tdt tdr">
                            <asp:RadioButton id="optRicorsoProvincialeNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataRicorsoProvincialeAl'),document.getElementById('txtDataRicorsoProvincialeAl);"
                                tabindex="47" Text="Nessuna" GroupName="DataRicorsoProvinciale"/></td>
                    </tr>
                    <tr id="provinciale_2" style="display: none">
                        <td class="Input_Label tdl">
                            <asp:CheckBox ID="chkDataSopensioneProvinciale" onclick="AttivaOption(document.getElementById('chkDataSopensioneProvinciale'),document.getElementById('optSopensioneProvinciale'),document.getElementById('txtDataSopensioneProvincialeDal'),document.getElementById('txtDataSopensioneProvincialeAl'),document.getElementById('optSopensioneProvincialeNoDate'));"
                                TabIndex="48" runat="server" Text="Sopensione Ricorso 1° Grado di Giudizio"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optSopensioneProvinciale" runat="server" onclick="AttivaDate(document.getElementById('txtDataSopensioneProvincialeDal'),document.getElementById('txtDataSopensioneProvincialeAl'));"
                                tabindex="49" Text="" GroupName="DataSopensioneProvinciale"/></td>
                        <td>
                            <asp:TextBox ID="txtDataSopensioneProvincialeDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="50" runat="server" name="txtDataSopensioneProvincialeDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataSopensioneProvincialeAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="51" runat="server" name="txtDataSopensioneProvincialeAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label tdr">
                            <asp:RadioButton id="optSopensioneProvincialeNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataSopensioneProvincialeDal'),document.getElementById('txtDataSopensioneProvincialeAl'));"
                                tabindex="52" Text="Nessuna" GroupName="DataSopensioneProvinciale"/></td>
                    </tr>
                    <tr id="provinciale_3" style="display: none">
                        <td class="Input_Label tdl">
                            <asp:CheckBox ID="chkDataSentenzaProvinciale" onclick="AttivaOption(document.getElementById('chkDataSentenzaProvinciale'),document.getElementById('optSentenzaProvinciale'),document.getElementById('txtDataSentenzaProvincialeDal'),document.getElementById('txtDataSentenzaProvincialeAl'),document.getElementById('optSentenzaProvincialeNoDate'));"
                                TabIndex="53" runat="server" Text="Sentenza 1° Grado di Giudizio"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optSentenzaProvinciale" runat="server" onclick="AttivaDate(document.getElementById('txtDataSentenzaProvincialeDal'),document.getElementById('txtDataSentenzaProvincialeAl'));"
                                tabindex="54" Text="" GroupName="DataSentenzaProvinciale"/></td>
                        <td>
                            <asp:TextBox ID="txtDataSentenzaProvincialeDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="55" runat="server" name="txtDataSentenzaProvincialeDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataSentenzaProvincialeAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="56" runat="server" name="txtDataSentenzaProvincialeAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label tdr">
                            <asp:RadioButton id="optSentenzaProvincialeNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataSentenzaProvincialeDal'),document.getElementById('txtDataSentenzaProvincialeAl);"
                                tabindex="57" Text="Nessuna" GroupName="DataSentenzaProvinciale"/></td>
                    </tr>
                    <!--'''''''''''''''''''''''''''''''''''''''''''''//FINE PROVINCIALE'''''''''''''''''''''''''''//-->
                    <tr id="regionale_1" style="display: none">
                        <td class="Input_Label tdl">
                            <asp:CheckBox ID="chkDataRicorsoRegionale" onclick="AttivaOption(document.getElementById('chkDataRicorsoRegionale'),document.getElementById('optRicorsoRegionale'),document.getElementById('txtDataRicorsoRegionaleDal'),document.getElementById('txtDataRicorsoRegionaleAl'),document.getElementById('optRicorsoRegionaleNoDate'));"
                                TabIndex="58" runat="server" Text="Ricorso 2° Grado di Giudizio"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optRicorsoRegionale" runat="server" onclick="AttivaDate(document.getElementById('txtDataRicorsoRegionaleDal'),document.getElementById('txtDataRicorsoRegionaleAl'));"
                                tabindex="59" Text="" GroupName="DataRicorsoRegionale"/></td>
                        <td>
                            <asp:TextBox ID="txtDataRicorsoRegionaleDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="60" runat="server" name="txtDataRicorsoRegionaleDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataRicorsoRegionaleAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="61" runat="server" name="txtDataRicorsoRegionaleAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label tdr">
                            <asp:RadioButton id="optRicorsoRegionaleNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataRicorsoRegionaleDal'),document.getElementById('txtDataRicorsoRegionaleAl'));"
                                tabindex="62" Text="Nessuna" GroupName="DataRicorsoRegionale"/></td>
                    </tr>
                    <tr id="regionale_2" style="display: none">
                        <td class="Input_Label tdl">
                            <asp:CheckBox ID="chkDataSopensioneRegionale" onclick="AttivaOption(document.getElementById('chkDataSopensioneRegionale'),document.getElementById('optSopensioneRegionale'),document.getElementById('txtDataSopensioneRegionaleDal'),document.getElementById('txtDataSopensioneRegionaleAl'),document.getElementById('optSopensioneRegionaleNoDate'));"
                                TabIndex="63" runat="server" Text="Sopensione 2° Grado di Giudizio"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optSopensioneRegionale" runat="server" onclick="AttivaDate(document.getElementById('txtDataSopensioneRegionaleDal'),document.getElementById('txtDataSopensioneRegionaleAl'));"
                                tabindex="64" Text="" GroupName="DataSopensioneRegionale"/></td>
                        <td>
                            <asp:TextBox ID="txtDataSopensioneRegionaleDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="65" runat="server" name="txtDataSopensioneRegionaleDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataSopensioneRegionaleAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="66" runat="server" name="txtDataSopensioneRegionaleAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label tdr">
                            <asp:RadioButton id="optSopensioneRegionaleNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataSopensioneRegionaleDal'),document.getElementById('txtDataSopensioneRegionaleAl'));"
                                tabindex="67" Text="Nessuna" GroupName="DataSopensioneRegionale"/></td>
                    </tr>
                    <tr id="regionale_3" style="display: none">
                        <td class="Input_Label tdl">
                            <asp:CheckBox ID="chkDataSentenzaRegionale" onclick="AttivaOption(document.getElementById('chkDataSentenzaRegionale'),document.getElementById('optSentenzaRegionale'),document.getElementById('txtDataSentenzaRegionaleDal'),document.getElementById('txtDataSentenzaRegionaleAl'),document.getElementById('optSentenzaRegionaleNoDate'));"
                                TabIndex="68" runat="server" Text="Sentenza 2° Grado di Giudizio"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optSentenzaRegionale" runat="server" onclick="AttivaDate(document.getElementById('txtDataSentenzaRegionaleDal'),document.getElementById('txtDataSentenzaRegionaleAl);"
                                tabindex="69" Text="" GroupName="DataSentenzaRegionale"/></td>
                        <td>
                            <asp:TextBox ID="txtDataSentenzaRegionaleDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="70" runat="server" name="txtDataSentenzaRegionaleDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataSentenzaRegionaleAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="71" runat="server" name="txtDataSentenzaRegionaleAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label tdr">
                            <asp:RadioButton id="optSentenzaRegionaleNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataSentenzaRegionaleDal'),document.getElementById('txtDataSentenzaRegionaleAl'));"
                                tabindex="72" Text="Nessuna" GroupName="DataSentenzaRegionale"/></td>
                    </tr>
                    <tr id="cassazione_1" style="display: none">
                        <td class="Input_Label tdl">
                            <asp:CheckBox ID="chkDataRicorsoCassazione" onclick="AttivaOption(document.getElementById('chkDataRicorsoCassazione'),document.getElementById('optRicorsoCassazione'),document.getElementById('txtDataRicorsoCassazioneDal'),document.getElementById('txtDataRicorsoCassazioneAl'),document.getElementById('optRicorsoCassazioneNoDate'));"
                                TabIndex="73" runat="server" Text="Ingiunzione"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optRicorsoCassazione" runat="server" onclick="AttivaDate(document.getElementById('txtDataRicorsoCassazioneDal'),document.getElementById('txtDataRicorsoCassazioneAl'));"
                                tabindex="74" Text="" GroupName="DataRicorsoCassazione"/></td>
                        <td>
                            <asp:TextBox ID="txtDataRicorsoCassazioneDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="75" runat="server" name="txtDataRicorsoCassazioneDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataRicorsoCassazioneAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="76" runat="server" name="txtDataRicorsoCassazioneAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label tdr">
                            <asp:RadioButton id="optRicorsoCassazioneNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataRicorsoCassazioneDal'),document.getElementById('txtDataRicorsoCassazioneAl'));"
                                tabindex="77" Text="Nessuna" GroupName="DataRicorsoCassazione"/></td>
                    </tr>
                    <tr id="cassazione_2" style="display: none">
                        <td class="Input_Label tdl">
                            <asp:CheckBox ID="chkDataSopensioneCassazione" onclick="AttivaOption(document.getElementById('chkDataSopensioneCassazione'),document.getElementById('optSopensioneCassazione'),document.getElementById('txtDataSopensioneCassazioneDal'),document.getElementById('txtDataSopensioneCassazioneAl'),document.getElementById('optSopensioneCassazioneNoDate'));"
                                TabIndex="78" runat="server" Text="Sopensione 3° Grado di Giudizio"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optSopensioneCassazione" runat="server" onclick="AttivaDate(document.getElementById('txtDataSopensioneCassazioneDal'),document.getElementById('txtDataSopensioneCassazioneAl'));"
                                tabindex="79" Text="" GroupName="DataSopensioneCassazione"/></td>
                        <td>
                            <asp:TextBox ID="txtDataSopensioneCassazioneDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="80" runat="server" name="txtDataSopensioneCassazioneDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataSopensioneCassazioneAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="81" runat="server" name="txtDataSopensioneCassazioneAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label tdr">
                            <asp:RadioButton id="optSopensioneCassazioneNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataSopensioneCassazioneDal'),document.getElementById('txtDataSopensioneCassazioneAl'));"
                                tabindex="82" Text="Nessuna" GroupName="DataSopensioneCassazione"/></td>
                    </tr>
                    <tr id="cassazione_3" style="display: none">
                        <td class="Input_Label tdl tdb">
                            <asp:CheckBox ID="chkDataSentenzaCassazione" onclick="AttivaOption(document.getElementById('chkDataSentenzaCassazione'),document.getElementById('optSentenzaCassazione'),document.getElementById('txtDataSentenzaCassazioneDal'),document.getElementById('txtDataSentenzaCassazioneAl'),document.getElementById('optSentenzaCassazioneNoDate'));"
                                TabIndex="83" runat="server" Text="Sentenza 3° Grado di Giudizio"></asp:CheckBox></td>
                        <td class="Input_Label tdb">
                            <asp:RadioButton id="optSentenzaCassazione" runat="server" onclick="AttivaDate(document.getElementById('txtDataSentenzaCassazioneDal'),document.getElementById('txtDataSentenzaCassazioneAl'));"
                                tabindex="84" Text="" GroupName="DataSentenzaCassazione"/></td>
                        <td class=" tdb">
                            <asp:TextBox ID="txtDataSentenzaCassazioneDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="85" runat="server" name="txtDataSentenzaCassazioneDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class=" tdb">
                            <asp:TextBox ID="txtDataSentenzaCassazioneAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="86" runat="server" name="txtDataSentenzaCassazioneAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label tdb tdr">
                            <asp:RadioButton id="optSentenzaCassazioneNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataSentenzaCassazioneDal'),document.getElementById('txtDataSentenzaCassazioneAl'));"
                                tabindex="87" Text="Nessuna" GroupName="DataSentenzaCassazione"/></td>
                    </tr>
                    <!--'''''''''''''''''''''''''''''''''''''''''''''//FINE RICORSO'''''''''''''''''''''''''''//-->
                    <tr>
                        <td class="Input_Label">
                            <asp:CheckBox ID="chkDataAttoDefinitivo" onclick="AttivaOption(document.getElementById('chkDataAttoDefinitivo'),document.getElementById('optAttoDefinitivo'),document.getElementById('txtDataAttoDefinitivoDal'),document.getElementById('txtDataAttoDefinitivoAl'),document.getElementById('optAttoDefinitivoNoDate'));"
                                TabIndex="88" runat="server" Text="Atto Definitivo"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optAttoDefinitivo" runat="server" onclick="AttivaDate(document.getElementById('txtDataAttoDefinitivoDal'),document.getElementById('txtDataAttoDefinitivoAl'));"
                                tabindex="89" Text="" GroupName="DataAttoDefinitivo"/></td>
                        <td>
                            <asp:TextBox ID="txtDataAttoDefinitivoDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="90" runat="server" name="txtDataAttoDefinitivoDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataAttoDefinitivoAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="91" runat="server" name="txtDataAttoDefinitivoAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton  id="optAttoDefinitivoNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataAttoDefinitivoDal'),document.getElementById('txtDataAttoDefinitivoAl'));" Text="Nessuna"  GroupName="DataAttoDefinitivo"/>

						</td>
                    </tr>
                    <tr>
                        <td class="Input_Label">
                            <asp:CheckBox ID="chkDataPagamento" onclick="AttivaOption(document.getElementById('chkDataPagamento'),document.getElementById('optPagamento'),document.getElementById('txtDataPagamentoDal'),document.getElementById('txtDataPagamentoAl'),document.getElementById('optPagamentoNoDate'));"
                                TabIndex="93" runat="server" Text="Pagamento"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optPagamento" runat="server" onclick="AttivaDate(document.getElementById('txtDataPagamentoDal'),document.getElementById('txtDataPagamentoAl'));"
                                tabindex="94" Text="" GroupName="DataPagamento"/></td>
                        <td>
                            <asp:TextBox ID="txtDataPagamentoDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="95" runat="server" name="txtDataPagamentoDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataPagamentoAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="96" runat="server" name="txtDataPagamentoAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton  id="optPagamentoNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataPagamentoDal'),document.getElementById('txtDataPagamentoAl'));" Text="Nessuna" GroupName="DataPagamento" />

						</td>
                    </tr>
                    <tr>
                        <td class="Input_Label">
                            <asp:CheckBox ID="chkDataSollecitoBonario" onclick="AttivaOption(document.getElementById('chkDataSollecitoBonario'),document.getElementById('optSollecitoBonario'),document.getElementById('txtDataSollecitoBonarioDal'),document.getElementById('txtDataSollecitoBonarioAl'),document.getElementById('optSollecitoBonarioNoDate'));"
                                TabIndex="103" runat="server" Text="Sollecito Bonario"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optSollecitoBonario" runat="server" onclick="AttivaDate(document.getElementById('txtDataSollecitoBonarioDal'),document.getElementById('txtDataSollecitoBonarioAl'));"
                                tabindex="104" Text="" GroupName="DataSollecitoBonario"/></td>
                        <td>
                            <asp:TextBox ID="txtDataSollecitoBonarioDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="105" runat="server" name="txtDataSollecitoBonarioDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataSollecitoBonarioAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="106" runat="server" name="txtDataSollecitoBonarioAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optSollecitoBonarioNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataSollecitoBonarioDal'),document.getElementById('txtDataSollecitoBonarioAl'));" Text="Nessuna" GroupName="DataSollecitoBonario" />

						</td>
                    </tr>
                    <tr>
                        <td class="Input_Label">
                            <asp:CheckBox ID="chkDataRuoloOrdinario" onclick="AttivaOption(document.getElementById('chkDataRuoloOrdinario'),document.getElementById('optRuoloOrdinario'),document.getElementById('txtDataRuoloOrdinarioDal'),document.getElementById('txtDataRuoloOrdinarioAl'),document.getElementById('optRuoloOrdinarioNoDate'));"
                                TabIndex="108" runat="server" Text="Ruolo Ordinario(Tarsu)"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optRuoloOrdinario" runat="server" onclick="AttivaDate(document.getElementById('txtDataRuoloOrdinarioDal'),document.getElementById('txtDataRuoloOrdinarioAl'));"
                                tabindex="109" Text="" GroupName="DataRuoloOrdinario"/></td>
                        <td>
                            <asp:TextBox ID="txtDataRuoloOrdinarioDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="110" runat="server" name="txtDataRuoloOrdinarioDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataRuoloOrdinarioAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="111" runat="server" name="txtDataRuoloOrdinarioAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optRuoloOrdinarioNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataRuoloOrdinarioDal'),document.getElementById('txtDataRuoloOrdinarioAl'));"
                                tabindex="112" Text="Nessuna" GroupName="DataRuoloOrdinario"/></td>
                    </tr>
                    <tr>
                        <!--*** 20120704 - IMU ***-->
                        <td class="Input_Label">
                            <asp:CheckBox ID="chkDataCoattivo" onclick="AttivaOption(document.getElementById('chkDataCoattivo'),document.getElementById('optCoattivo'),document.getElementById('txtDataCoattivoDal'),document.getElementById('txtDataCoattivoAl'),document.getElementById('optCoattivoNoDate'));"
                                TabIndex="113" runat="server" Text="Coattivo"></asp:CheckBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optCoattivo" runat="server" onclick="AttivaDate(document.getElementById('txtDataCoattivoDal'),document.getElementById('txtDataCoattivoAl'));"
                                tabindex="114" Text="" GroupName="DataCoattivo"/></td>
                        <td>
                            <asp:TextBox ID="txtDataCoattivoDal" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="115" runat="server" name="txtDataCoattivoDal" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtDataCoattivoAl" onkeydown="keyPress();" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" TabIndex="116" runat="server" name="txtDataCoattivoAl" CssClass="Input_Text_Right TextDate" ToolTip=""></asp:TextBox></td>
                        <td class="Input_Label">
                            <asp:RadioButton id="optCoattivoNoDate" runat="server" onclick="DissativaDate(document.getElementById('txtDataCoattivoDal'),document.getElementById('txtDataCoattivoAl'));"
                                tabindex="117" Text="Nessuna" GroupName="DataCoattivo"/></td>
                    </tr>
					<tr>
						<td>
							<asp:label id="LblIntest290" CssClass="Input_Label" Runat="server" style ="display:none">File 290 </asp:label>
						</td>
						<td>
							<asp:label ID="LblFile290" Runat="server" CssClass="Input_Label" Font-Underline="True" style ="display:none"></asp:label>
						</td>
					</tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <input class="btnList_botton" id="btnDateRicorso" style="width: 184px; height: 15px" onclick="AttivaDateRicorso();" type="button" value="Visualizzazione date Ricorso">
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div id="divAggMassivo" style="display: none">
            <fieldset class="classeFiledSetIframe">
                <legend class="Legend">Aggiornamento Massivo Date</legend>
                <div class="col-md-12">
                    <div class="col-md-3">
                        <asp:RadioButton ID="optAMConsegna" runat="server" GroupName="optAggMassivo" CssClass="Input_Label" Text="Consegna" /><br />
                        <asp:RadioButton ID="optAMNotifica" runat="server" GroupName="optAggMassivo" CssClass="Input_Label" Text="Notifica" /><br />
                        <asp:RadioButton ID="optAMSollecito" runat="server" GroupName="optAggMassivo" CssClass="Input_Label" Text="Sollecito" /><br />
                        <asp:RadioButton ID="optAMCoattivo" runat="server" GroupName="optAggMassivo" CssClass="Input_Label" Text="Coattivo" /><br />
                        <asp:RadioButton ID="optAMIrreperibile" runat="server" GroupName="optAggMassivo" CssClass="Input_Label" Text="Irreperibilità" /><br />
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblAMData" runat="server" CssClass="Input_Label">Data</asp:Label><br />
                        <asp:TextBox ID="txtAMData" runat="server" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                    </div>
                    <div class="col-md-1">
                        <input class="Bottone BottoneSalva" id="AggMassivoDate" title="Aggiorna Data" onclick="attesaGestioneAtti.style.display='';document.getElementById('CmdAggMassivoDate.click();" type="button" name="AggMassivoDate">
                    </div>
                </div>
            </fieldset>
        </div>
        <table cellpadding="0" width="100%" align="left" border="0">
            <tr>
                <td>
                    <fieldset class="classeFiledSetIframe">
                        <legend class="Legend">Visualizzazione Atti</legend>
                        <table cellpadding="0" width="100%" border="0">
                            <tr>
                                <td>
                                    <iframe id="loadGrid" src="SearchAttiRicercaAvanzata.aspx" frameBorder="0" width="100%" height="600px"></iframe>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td>
                   <div id="attesaGestioneAtti" style="z-index: 103; position: absolute;display:none;">
                        <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                        <div class="BottoneClessidra">&nbsp;</div>
                        <div class="Legend">Attendere Prego</div>
                    </div>
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
        <asp:TextBox ID="txtDIVDate" Style="display: none" runat="server" Height="10"></asp:TextBox>
        <asp:TextBox ID="txtDateRicorso" Style="display: none" runat="server" Height="10"></asp:TextBox>
        <asp:TextBox ID="txtCheckBox" Style="display: none" runat="server" Height="10"></asp:TextBox>
        <asp:TextBox ID="txtRicercaAttiva" Style="display: none" runat="server"></asp:TextBox>
        <asp:button id="CmdEstrazione290" style="DISPLAY: none" runat="server"></asp:button>
		<asp:button id="CmdScarica" style="DISPLAY: none" runat="server"></asp:button>
		<asp:Button ID="CmdSearch" Style="display: none" runat="server"></asp:Button>
		<asp:Button ID="CmdAggMassivoDate" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="btnStampaExcel" Style="display: none" runat="server"></asp:Button>
    </form>
</body>
</html>
