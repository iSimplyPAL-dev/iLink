<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GestioneAtti.aspx.vb" Inherits="Provvedimenti.GestioneAtti" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Gestione Lettere</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../_js/Utility.js?newversion"></script>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/vbscript" src="../../_vbs/OperazioniSuCampi.vbs"></script>
    <script type="text/vbscript" src="../../_vbs/ControlliFormali.vbs"></script>
    <script type="text/javascript">
        var myWin = null, isBlocked = false;

        function checkFocus() {
            if (myWin && !myWin.closed) {
                myWin.focus();
                return false;
            } else if (isBlocked) {
                document.releaseEvents(Event.MOUSEDOWN);
                document.onmousedown = null;
                isBlocked = false;
            }
        }

        function openModalDialog(url, t, l, w, h) {
            if (document.layers) {
                document.captureEvents(Event.MOUSEDOWN);
                document.onmousedown = checkFocus;
                isBlocked = true;

                myWin = window.open(url, "newWin", "top=" + t + ",left=" + l + ",width=" + w + ",height=" + h);
                myWin.focus();
            }
            if (document.all)
                window.showModalDialog(url, window, "dialogTop:" + t + "px;dialogLeft:" + l + "px;dialogWidth:" + w + "px;dialogHeight:" + h + "px");
        }

        function GoBack() {
            document.getElementById('CmdGoBack').click();
        }
        function GestRettifica() {
            var WinPar = "top =60, left=120, width=1200,height=800, status=yes, toolbar=no,scrollbar=no, resizable=yes"
            //alert(document.getElementById('txtCOD_TRIBUTO').value);
            if (document.getElementById('txtOPEN_RETTIFICA').value == "1") {
                var Parametri = null;

                Parametri = "";
                Parametri = "?COD_CONTRIBUENTE=" + document.getElementById('hdIdContribuente').value;
                Parametri = Parametri + "&ID_PROVVEDIMENTO=" + document.getElementById('hfIdProvvedimento').value;
                Parametri = Parametri + "&ANNO=" + document.getElementById('txtANNO_RETTIFICA').value;
                Parametri = Parametri + "&DATA_ELABORAZIONE=" + document.getElementById('txtDATA_ELABORAZIONE').value;
                Parametri = Parametri + "&COD_TRIBUTO=" + document.getElementById('txtCOD_TRIBUTO').value;
                Parametri = Parametri + "&TIPO_OPERAZIONE=" + document.getElementById('txtTIPO_OPERAZIONE').value;
                Parametri = Parametri + "&TIPO_RICERCA=" + document.getElementById('txtTIPO_RICERCA').value;
                Parametri = Parametri + "&DATADAAGGIORNARE=" + document.getElementById('txtNOME_OGGETTO').value;
                Parametri = Parametri + "&NOMINATIVO=" + escape(document.getElementById('txtNOMINATIVO_RETTIFICA').value);
                Parametri = Parametri + "&TIPO_PROCEDIMENTO=" + document.getElementById('txtTIPO_PROCEDIMENTO').value;

                //window.open ("../GestioneLiquidazioni/StampaLiquidazioni/frmRistampaL.aspx?TIPODOC=B","","width=600,height=500,top=20,status=yes,toolbar=no")
                // ALESSIO COMMENTATO
                //openModalDialog('FrameRettifica.aspx'+ Parametri ,60,120,'800','650'); 
                window.open('FrameRettifica.aspx' + Parametri, '', WinPar);


                //WinPopUp=OpenPopup('OPENgovProvvedimenti','/OPENgovProvvedimenti/GestioneAtti/FrameRettifica.aspx'+ Parametri ,'Rettifica','800','650',0,0,'yes','no');			
                return false;
            }
            if (document.getElementById('txtOPEN_RETTIFICA').value == "2") {
                var Parametri = null;

                Parametri = "";
                Parametri = "?COD_CONTRIBUENTE=" + document.getElementById('hdIdContribuente').value;
                Parametri = Parametri + "&ID_PROVVEDIMENTO=" + document.getElementById('hfIdProvvedimento').value;
                Parametri = Parametri + "&ANNO=" + document.getElementById('txtANNO_RETTIFICA').value;
                Parametri = Parametri + "&DATA_ELABORAZIONE=" + document.getElementById('txtDATA_ELABORAZIONE').value;
                Parametri = Parametri + "&COD_TRIBUTO=" + document.getElementById('txtCOD_TRIBUTO').value;
                Parametri = Parametri + "&TIPO_OPERAZIONE=" + document.getElementById('txtTIPO_OPERAZIONE').value;
                Parametri = Parametri + "&TIPO_RICERCA=" + document.getElementById('txtTIPO_RICERCA').value;
                //alert("data:", document.getElementById('txtNOME_OGGETTO').value);
                Parametri = Parametri + "&DATADAAGGIORNARE=" + document.getElementById('txtNOME_OGGETTO').value;
                Parametri = Parametri + "&NOMINATIVO=" + escape(document.getElementById('txtNOMINATIVO_RETTIFICA').value);
                Parametri = Parametri + "&TIPO_PROCEDIMENTO=" + document.getElementById('txtTIPO_PROCEDIMENTO').value;

                console.log("eccoli-->", Parametri)

                //alert(Parametri);

                //openModalDialog('FrameRettifica.aspx'+ Parametri ,60,120,'800','650'); 
                window.open('FrameRettifica.aspx' + Parametri, '', WinPar);

                /*Parametri="";
                Parametri="?COD_CONTRIBUENTE="+document.getElementById('hdIdContribuente').value;
                Parametri=Parametri+ "&ID_PROVVEDIMENTO="+document.getElementById('txtID_PROVVEDIMENTO').value;
                Parametri=Parametri+ "&ANNO="+document.getElementById('txtANNO_RETTIFICA').value;
                Parametri=Parametri+ "&DATA_ELABORAZIONE="+document.getElementById('txtDATA_ELABORAZIONE').value;
                Parametri=Parametri+ "&COD_TRIBUTO="+document.getElementById('txtCOD_TRIBUTO').value;
                Parametri=Parametri+ "&TIPO_OPERAZIONE="+document.getElementById('txtTIPO_OPERAZIONE').value;
                Parametri=Parametri+ "&TIPO_RICERCA="+document.getElementById('txtTIPO_RICERCA').value;
                Parametri=Parametri+ "&DATADAAGGIORNARE="+document.getElementById('txtNOME_OGGETTO').value;
                Parametri=Parametri+ "&NOMINATIVO="+escape(document.getElementById('txtNOMINATIVO_RETTIFICA').value);					
                
                WinPopUp=OpenPopup('OPENgovProvvedimenti','/OPENgovProvvedimenti/GestioneAtti/FrameRettifica.aspx'+ Parametri ,'Rettifica','800','650',0,0,'yes','no');			
                return false;*/
            }
        }
        //funzione chiamata dal validator

        function VerificaCampiQuestionari() {
            sMsg = ""



            if (!IsBlank(document.getElementById('txtDataConsegna').value)) {
                if (IsBlank(document.getElementById('txtDataStampa').value)) {
                    sMsg = sMsg + "[La Data di Consegna puo' essere inserita solo se presente la Data di Stampa !]\n";
                }
                else {
                    if (!doDateCheck(document.getElementById('txtDataConsegna'), document.getElementById('txtDataStampa'))) {
                        sMsg = sMsg + "[Data di Consegna Avviso minore di Data di Stampa Avviso!]\n";
                    }
                }
            }
            if (!IsBlank(document.getElementById('txtDataNotifica').value)) {
                if (IsBlank(document.getElementById('txtDataConsegna').value)) {
                    sMsg = sMsg + "[La Data di Notifica puo' essere inserita solo se presente la Data di Consegna !]\n";
                }
                else {
                    if (!doDateCheck(document.getElementById('txtDataNotifica'), document.getElementById('txtDataConsegna'))) {
                        sMsg = sMsg + "[Data di Notifica  minore di Data di Consegna Avviso !]\n";
                    }
                }


            }
            if (!IsBlank(document.getElementById('txtPervenutoQuestionari').value)) {
                if (IsBlank(document.getElementById('txtDataNotifica').value)) {
                    sMsg = sMsg + "[La Data di Pervenuto il puo' essere inserita solo se presente la Data di Notifica !]\n";
                }
                else {
                    if (!doDateCheck(document.getElementById('txtPervenutoQuestionari'), document.getElementById('txtDataNotifica'))) {
                        sMsg = sMsg + "[Data di Pervenuto Il  minore di Data di Notifica Avviso !]\n";
                    }
                }
            }


            if (!IsBlank(sMsg)) {
                strMessage = "Attenzione...\n\n Gestione Date Questionari:\n\n"
                GestAlert('a', 'warning', '', '', strMessage + sMsg);
                return false;
            }

            return true;

        }
        function VerificaCampiLiquidazioni() {
            sMsg = ""

            if (!IsBlank(document.getElementById('txtDataConsegnaAvviso').value)) {
                if (IsBlank(document.getElementById('txtDataStampaAvviso').value)) {
                    sMsg = sMsg + "[La Data di Consegna puo' essere inserita solo se presente la Data di Stampa !]\n";
                }
                else {
                    if (!doDateCheck(document.getElementById('txtDataConsegnaAvviso'), document.getElementById('txtDataStampaAvviso'))) {
                        sMsg = sMsg + "[Data di Consegna Avviso minore di Data di Stampa Avviso!]\n";
                    }
                }
            }
            if (!IsBlank(document.getElementById('txtDataNotificaAvviso').value)) {
                if (IsBlank(document.getElementById('txtDataConsegnaAvviso').value)) {
                    sMsg = sMsg + "[La Data di Notifica puo' essere inserita solo se presente la Data di Consegna !]\n";
                }
                else {
                    if (!doDateCheck(document.getElementById('txtDataNotificaAvviso'), document.getElementById('txtDataConsegnaAvviso'))) {
                        sMsg = sMsg + "[Data di Notifica Avviso minore di Data di Consegna Avviso!]\n";
                    }
                }
            }

            if (!IsBlank(document.getElementById('txtDataSospensioneAvvisoAutotutela').value)) {
                if (IsBlank(document.getElementById('txtDataNotificaAvviso').value)) {
                    sMsg = sMsg + "[La Data di Sospensione Avviso puo' essere inserita solo se presente la Data di Notifica !]\n";
                }
                else {
                    if (!doDateCheck(document.getElementById('txtDataSospensioneAvvisoAutotutela'), document.getElementById('txtDataNotificaAvviso'))) {
                        sMsg = sMsg + "[Data di Sospensione Avviso minore di Data di Notifica Avviso!]\n";
                    }
                }
            }
            /*GESTIONE RICORSO PROVINCIALE/*
            if (!IsBlank(document.getElementById('txtDataRicorsoProvinciale').value)) {
                if (IsBlank(document.getElementById('txtDataNotificaAvviso').value)) {
                    sMsg = sMsg + "[La Data Ricorso Provinciale  puo' essere inserita solo se presente la Data di Notifica Avviso !]\n";
                }
                else {
                    if (!doDateCheck(document.getElementById('txtDataRicorsoProvinciale'), document.getElementById('txtDataNotificaAvviso'))) {
                        sMsg = sMsg + "[Data di Ricorso Provinciale Avviso minore di Data di Notifica Avviso!]\n";
                    }
                }
            }
            if (!IsBlank(document.getElementById('txtSospensioneProvinciale').value)) {
                if (IsBlank(document.getElementById('txtDataRicorsoProvinciale').value)) {
                    sMsg = sMsg + "[La Data di Sospensione Comm. Tributaria Provinciale  puo' essere inserita solo se presente la Data di Ricorso Provinciale !]\n";
                }
                else {
                    if (!doDateCheck(document.getElementById('txtSospensioneProvinciale'), document.getElementById('txtDataRicorsoProvinciale'))) {
                        sMsg = sMsg + "[Data di Sospensione Provinciale Avviso minore di Data di Ricorso Provinciale Avviso!]\n";
                    }
                }
            }
            if (!IsBlank(document.getElementById('txtSentenzaProvinciale').value)) {
                if (!IsBlank(document.getElementById('txtSospensioneProvinciale').value)) {
                    if (!doDateCheck(document.getElementById('txtSentenzaProvinciale'), document.getElementById('txtSospensioneProvinciale'))) {
                        sMsg = sMsg + "[Data di Sentenza Provinciale Avviso minore di Data di Sospensione Provinciale Avviso!]\n";
                    }
                }
                else {
                    if (IsBlank(document.getElementById('txtDataRicorsoProvinciale').value)) {
                        sMsg = sMsg + "[La Data di Sentenza Provinciale puo' essere inserita solo se presente la Data di Ricorso Provinciale !]\n";
                    }
                    else {
                        if (!doDateCheck(document.getElementById('txtSentenzaProvinciale'), document.getElementById('txtDataRicorsoProvinciale'))) {

                            sMsg = sMsg + "[Data di Sentenza Provinciale Avviso minore di Data di Ricorso Provinciale Avviso!]\n";
                        }
                    }
                }
            }

            if (!IsBlank(document.getElementById('txtNoteProvinciale').value)) {
                if ((IsBlank(document.getElementById('txtDataRicorsoProvinciale').value)) && (IsBlank(document.getElementById('txtSospensioneProvinciale').value)) && (IsBlank(document.getElementById('txtSentenzaProvinciale').value))) {
                    sMsg = sMsg + "[Le Note/Motivazioni Provinciali possono essere inserite solamente se una delle date Provinciali è valorizzata !]\n";
                }

            }

            /*GESTIONE RICORSO PROVINCIALE*/
            /*GESTIONE RICORSO REGIONALE*/
            if (!IsBlank(document.getElementById('txtDataRicorsoRegionale').value)) {
                if (IsBlank(document.getElementById('txtSentenzaProvinciale').value)) {
                    sMsg = sMsg + "[La Data di presentazione Ricorso regionale puo' essere inserita solo se presente la Data di Sentenza Provinciale !]\n";
                }
                else {
                    if (!doDateCheck(document.getElementById('txtDataRicorsoRegionale'), document.getElementById('txtSentenzaProvinciale'))) {
                        sMsg = sMsg + "[Data di Ricorso Regionale Avviso minore di Data di Sentenza Provinciale Avviso!]\n";
                    }
                }
            }
            if (!IsBlank(document.getElementById('txtSospensioneRegionale').value)) {
                if (IsBlank(document.getElementById('txtDataRicorsoRegionale').value)) {
                    sMsg = sMsg + "[La Data di Sospensione Comm. Tributaria Regionale  puo' essere inserita solo se presente la Data di Ricorso Regionale !]\n";
                }
                else {
                    if (!doDateCheck(document.getElementById('txtSospensioneRegionale'), document.getElementById('txtDataRicorsoRegionale'))) {
                        sMsg = sMsg + "[Data di Sospensione Regionale Avviso minore di Data di Ricorso Regionale Avviso!]\n";
                    }
                }

            }
            if (!IsBlank(document.getElementById('txtSentenzaRegionale').value)) {
                if (!IsBlank(document.getElementById('txtSospensioneRegionale').value)) {
                    if (!doDateCheck(document.getElementById('txtSentenzaRegionale'), document.getElementById('txtSospensioneRegionale'))) {
                        sMsg = sMsg + "[Data di Sentenza Regionale Avviso minore di Data di Sospensione Regionale Avviso!]\n";
                    }
                }
                else {
                    if (IsBlank(document.getElementById('txtDataRicorsoRegionale').value)) {
                        sMsg = sMsg + "[La Data di Sentenza Regionale puo' essere inserita solo se presente la Data di Ricorso Regionale !]\n";
                    }
                    else {
                        if (!doDateCheck(document.getElementById('txtSentenzaRegionale'), document.getElementById('txtDataRicorsoRegionale'))) {
                            sMsg = sMsg + "[Data di Sentenza Regionale Avviso minore di Data di Ricorso Regionale Avviso!]\n";
                        }
                    }
                }
            }

            if (!IsBlank(document.getElementById('txtNoteRegionale').value)) {
                if ((IsBlank(document.getElementById('txtDataRicorsoRegionale').value)) && (IsBlank(document.getElementById('txtSospensioneRegionale').value)) && (IsBlank(document.getElementById('txtSentenzaRegionale').value))) {
                    sMsg = sMsg + "[Le Note/Motivazioni Regionali possono essere inserite solamente se una delle date Regionali è valorizzata !]\n";
                }

            }
            /*GESTIONE RICORSO REGIONALE*/
            /*GESTIONE INGIUNZIONE*/
            /*if (!IsBlank(document.getElementById('txtDataRicorsoCassazione').value)) {
                if (IsBlank(document.getElementById('txtSentenzaRegionale').value)) {
                    sMsg = sMsg + "[La Data di presentazione Ricorso Cassazione puo' essere inserita solo se presente la Data di Sentenza Regionale !]\n";
                }
                else {
                    if (!doDateCheck(document.getElementById('txtDataRicorsoCassazione'), document.getElementById('txtSentenzaRegionale'))) {
                        sMsg = sMsg + "[Data di Ricorso Cassazione Avviso minore di Data di Sentenza Regionale Avviso!]\n";
                    }
                }
            }
            if (!IsBlank(document.getElementById('txtSospensioneCassazione').value)) {
                if (IsBlank(document.getElementById('txtDataRicorsoCassazione').value)) {
                    sMsg = sMsg + "[La Data di Sospensione Comm. Tributaria Cassazione  puo' essere inserita solo se presente la Data di Ricorso Cassazione !]\n";
                }
                else {
                    if (!doDateCheck(document.getElementById('txtSospensioneCassazione'), document.getElementById('txtDataRicorsoCassazione'))) {
                        sMsg = sMsg + "[Data di Sospensione Cassazione Avviso minore di Data di Ricorso Cassazione Avviso!]\n";
                    }
                }
            }
            if (!IsBlank(document.getElementById('txtSentenzaCassazione').value)) {
                if (!IsBlank(document.getElementById('txtSospensioneCassazione').value)) {
                    if (!doDateCheck(document.getElementById('txtSentenzaCassazione'), document.getElementById('txtSospensioneCassazione'))) {
                        sMsg = sMsg + "[Data di Sentenza Cassazione Avviso minore di Data di Sospensione Cassazione Avviso!]\n";
                    }
                }
                else {

                    if (IsBlank(document.getElementById('txtDataRicorsoCassazione').value)) {
                        sMsg = sMsg + "[La Data di Sentenza Cassazione puo' essere inserita solo se presente la Data di Ricorso Cassazione !]\n";
                    }
                    else {
                        if (!doDateCheck(document.getElementById('txtSentenzaCassazione'), document.getElementById('txtDataRicorsoCassazione'))) {
                            sMsg = sMsg + "[Data di Sentenza Cassazione Avviso minore di Data di Ricorso Cassazione Avviso!]\n";
                        }
                    }
                }
            }
            if (!IsBlank(document.getElementById('txtNoteCassazione').value)) {
                if ((IsBlank(document.getElementById('txtDataRicorsoCassazione').value)) && (IsBlank(document.getElementById('txtSospensioneCassazione').value)) && (IsBlank(document.getElementById('txtSentenzaCassazione').value))) {
                    sMsg = sMsg + "[Le Note/Motivazioni Cassazione possono essere inserite solamente se una delle date Cassazione è valorizzata !]\n";
                }

            }*/
            if (!IsBlank(document.getElementById('txtNoteCassazione').value)) {
                if ((IsBlank(document.getElementById('txtSospensioneCassazione').value))) {
                    sMsg = sMsg + "[Il Numero Ingiunzione può essere inserito solamente se la data Ingiunzione è valorizzata !]\n";
                }
            }
            /*GESTIONE INGIUNZIONE*/
            /*GESTIONE CONCILIAZIONE GIUDIZIALE*/
            if (!IsBlank(document.getElementById('txtNoteConcGiudiz').value)) {
                if ((document.getElementById('ckConcGiudiz').checked == false)) {
                    sMsg = sMsg + "[Le Note/Motivazioni Conciliazione possono essere inserite solamente se il flag Conciliazione Giudiziale è selezionato !]\n";
                }
            }
            /*GESTIONE CONCILIAZIONE GIUDIZIALE*/
            /*GESTIONE ACCERTAMENTI CON ADESIONE*/
            if (!IsBlank(document.getElementById('txtNoteAccertamenti').value)) {
                if ((document.getElementById('ckAccertamento').checked == false)) {
                    sMsg = sMsg + "[Le Note/Motivazioni Accertamento possono essere inserite solamente se il flag Accertamenti con Adesione è selezionato !]\n";
                }
            }
            if (!IsBlank(document.getElementById('txtTermineRicorso').value)) {
                if ((document.getElementById('ckAccertamento').checked == false) && ((document.getElementById('ddlEsitoAccertamenti').value != 0) || (document.getElementById('ddlEsitoAccertamenti').value != 1))) {
                    sMsg = sMsg + "[Il Nuovo Termine di Ricorso Accertamento può essere inserito solamente se il flag Accertamenti con Adesione è selezionato e se Esito ha valore Conferma oppure Rettifica !]\n";
                }
            }
            if (document.getElementById('ddlEsitoAccertamenti').value != -1) {
                if ((document.getElementById('ckAccertamento').checked == false)) {
                    sMsg = sMsg + "[L'Esito Accertamento può essere inserito solamente se il flag Accertamenti con Adesione è selezionato !]\n";
                }
            }
            if ((document.getElementById('ckAccertamento').checked == true)) {
                if (document.getElementById('ddlEsitoAccertamenti').value == -1) {
                    sMsg = sMsg + "[Se il flag Accertamenti con Adesione è selezionato è obbligatorio inserire il tipo Esito !]\n";
                }
            }
            if (!IsBlank(document.getElementById('txtTermineRicorso').value)) {
                //alert('esito accertamento: ' + document.getElementById('ddlEsitoAccertamenti').value)
                if ((document.getElementById('ddlEsitoAccertamenti').value == -1) || (document.getElementById('ddlEsitoAccertamenti').value == 2)) {
                    sMsg = sMsg + "[E' possibile inserire il Termine di Ricorso solamente se Esito ha valore Conferma o Rettifica !]\n";
                }
            }

            /*GESTIONE ACCERTAMENTI CON ADESIONE*/

            if (!IsBlank(document.getElementById('txtDataConcessioneRateizzazione').value)) {

                if (!IsBlank(document.getElementById('txtDataVersamentoUnicaSoluzione').value)) {
                    sMsg = sMsg + "[Data di Versamento unica soluzione valorizzata impossibile rateizzare!]\n";
                }
                else {
                    if (IsBlank(document.getElementById('txtDataNotificaAvviso').value)) {
                        sMsg = sMsg + "[La Data Concessione Rateizzazione puo' essere inserita solo se presente la Data di Notifica Avviso!]\n";
                    }
                    else {
                        if (!doDateCheck(document.getElementById('txtDataConcessioneRateizzazione'), document.getElementById('txtDataNotificaAvviso'))) {
                            sMsg = sMsg + "[Data Concessione Rateizzazione minore di Data di Notifica Avviso!]\n";
                        }
                    }
                }
            }

            if (!IsBlank(document.getElementById('txtDataVersamentoUnicaSoluzione').value)) {

                if (!IsBlank(document.getElementById('txtDataConcessioneRateizzazione').value)) {
                    sMsg = sMsg + "[Data di concessione rateizzazione valorizzata impossibile versare in unica soluzione!]\n";
                }
                else {
                    if (IsBlank(document.getElementById('txtDataNotificaAvviso').value)) {
                        sMsg = sMsg + "[La Data Pagamento puo' essere inserita solo se presente la Data di Notifica Avviso!]\n";
                    }
                    else {
                        if (!doDateCheck(document.getElementById('txtDataVersamentoUnicaSoluzione'), document.getElementById('txtDataNotificaAvviso'))) {
                            sMsg = sMsg + "[Data Pagamento minore di Data di Notifica Avviso!]\n";
                        }
                    }
                }
            }


            if (!IsBlank(sMsg)) {
                strMessage = "Attenzione...\n\n Gestione Date PreAccertamento/Accertamenti:\n\n"
                GestAlert('a', 'warning', '', '', strMessage + sMsg);
                return false;
            }

            return true;

        }
        function AttivaPervenutoIl() {
            if (confirm('Si vuole modificare la Data di Pervenuto il ?')) {
                document.getElementById('txtPervenutoQuestionari').disabled = false;
                document.getElementById('txtHiddenPERVENUTOIL').value = '';
                Setfocus(document.getElementById('txtPervenutoQuestionari'));
            }
        }
        function SalvaLiquidazioni() {
            if (VerificaCampiLiquidazioni()) {
                if (confirm('Si vogliono salvare le modifiche apportate all\'atto?')) {
                    attesaGestioneAtti_ATTI.style.display = '';
                    document.getElementById('btnSalvaLiquidazioni').click();
                }
            }
        }

        function Stampa() {
            if (VerificaCampiLiquidazioni()) {

                if (IsBlank(document.getElementById('txtDataConfermaAvviso').value)) {
                    if (confirm('Si desidera stampare la Bozza del Provvedimento?')) {
                        if (document.getElementById("txtIDTIPOPROVVEDIMENTO").value == "5") {
                            ApriStampaLiquidazioneBozzaRimborso();
                        } else {
                            ApriStampaLiquidazioneBozza();
                        }
                    }
                    else {
                        if (confirm('Si desidera stampare e rendere definitivo il Provvedimento?')) {
                            //attesaGestioneAtti_ATTI.style.display ='';
                            document.getElementById('btnStampa').click();
                        }
                    }
                }
                else {

                    //alert('La data di conferma avviso è già presente.\nNon è possibile ristampare.');

                    if (confirm('La data di conferma avviso è già presente.\nSi desidera ristampare la Bozza del Provvedimento?')) {
                        if (document.getElementById("txtIDTIPOPROVVEDIMENTO").value == "5") {
                            ApriStampaLiquidazioneBozzaRimborso();
                        } else {
                            ApriStampaLiquidazioneBozza();
                        }
                    }
                    else {
                        if (confirm('La data di conferma avviso è già presente.\nSi desidera ristampare il Provvedimento?')) {
                            document.getElementById('btnStampa').click();
                        }
                    }
                }
            }
        }

        function StampaBollettini() {
            if (VerificaCampiLiquidazioni()) {
                if(document.getElementById('hfBollettinoF24').value=="1")
                    ApriStampaAccertamento('BOLLACC', 1);
                else
                    document.getElementById('btnStampaBollettini').click();
            }
        }


        function StampaQuestionari() {
            if (VerificaCampiQuestionari()) {
                if (IsBlank(document.getElementById('txtDataStampa').value)) {

                    if (confirm('Si desidera stampare e rendere definitivo il Provvedimento?')) {
                        //attesaGestioneAtti_ATTI.style.display =''; 
                        document.getElementById('btnStampaQuestionari').click();
                    }

                }
                else {
                    //alert('La data di stampa avviso è già presente.\nNon è possibile ristampare.');
                    if (confirm('La data di stampa avviso è già presente.\nSi desidera comunque ristampare il Provvedimento?')) {
                        //attesaGestioneAtti_ATTI.style.display =''; 
                        document.getElementById('btnStampaQuestionari').click();
                    }

                }
            }
        }

        function StampaAccertamenti() {
            //alert("eccomi");
            console.log("eccomi");
            if (VerificaCampiLiquidazioni()) {
                if (IsBlank(document.getElementById('txtDataConfermaAvviso').value)) {

                    if (confirm('Si desidera stampare la Bozza del Provvedimento?')) {
                        if (document.getElementById("txtIDTIPOPROVVEDIMENTO").value == "5") {
                            ApriStampaAccertamento('DOCRIMBORSO_ICI', 1);
                        } else {
                            if (document.getElementById("txtIDTIPOPROVVEDIMENTO").value == "7") {
                                console.log("eccomi2");
                                ApriStampaAccertamento('DOCANNULLAMENTO', 1);
                            } else {
                                console.log("eccomi3");
                                ApriStampaAccertamento('DOCACC', 1);
                            }
                        }
                    }
                    else {
                        if (confirm('Si desidera stampare e rendere definitivo il Provvedimento?')) {
                            //attesaGestioneAtti_ATTI.style.display ='';
                            document.getElementById('btnStampaAccertamenti').click();
                        }
                    }
                }
                else {
                    //alert('La data di conferma avviso è già presente.\nNon è possibile ristampare.');								
                    if (confirm('La data di conferma avviso è già presente.\nSi desidera ristampare la Bozza del Provvedimento?')) {
                        if (document.getElementById("txtIDTIPOPROVVEDIMENTO").value == "5") {
                            ApriStampaAccertamento('DOCRIMBORSO_ICI', 1);
                        } else {
                            if (document.getElementById("txtIDTIPOPROVVEDIMENTO").value == "7") {
                                console.log("eccomi4");
                                ApriStampaAccertamento('DOCANNULLAMENTO', 1);
                            } else {
                                console.log("eccomi5");
                                ApriStampaAccertamento('DOCACC', 1);
                            }
                        }
                    }
                    else {
                        if (confirm('La data di conferma avviso è già presente.\nSi desidera ristampare il Provvedimento?')) {
                            document.getElementById('btnStampaAccertamenti').click();
                        }
                    }
                }
            }
        }

        function ApriStampaLiquidazioneBozza() {
            //alert("ApriStampaLiquidazioneBozza");
            myleft = (screen.width - 600) / 2;
            window.open("../GestioneLiquidazioni/StampaLiquidazioni/frmRistampaL.aspx?CODTRIBUTO=" + document.getElementById('txtCOD_TRIBUTO').value + "&TIPODOC=DOCPREACCBOZZA", "", "width=600,height=500,top=20,left=" + myleft + ",status=yes,toolbar=no")
        }

        function ApriStampaLiquidazioneBozzaRimborso() {
            //alert("ApriStampaLiquidazioneBozza");
            myleft = (screen.width - 600) / 2;
            window.open("../GestioneLiquidazioni/StampaLiquidazioni/frmRistampaL.aspx?CODTRIBUTO=" + document.getElementById('txtCOD_TRIBUTO').value + "&TIPODOC=DOCRIMBORSO_ICI_BOZZA", "", "width=600,height=500,top=20,left=" + myleft + ",status=yes,toolbar=no")
        }

        function ApriStampaLiquidazione() {
            //alert("ApriStampaLiquidazione");
            myleft = (screen.width - 600) / 2;
            window.open("../GestioneLiquidazioni/StampaLiquidazioni/frmRistampaL.aspx?CODTRIBUTO=" + document.getElementById('txtCOD_TRIBUTO').value + "&TIPODOC=DOCPREACC", "", "width=600,height=500,top=20left=" + myleft + ",status=yes,toolbar=no")
        }

        function ApriStampaQuestionario() {
            //alert("ApriStampaQuestionario");
            myleft = (screen.width - 600) / 2;
            window.open("../GestioneQuestionari/StampaQuestionari/frmRistampa.aspx", "", "width=600,height=500,top=20left=" + myleft + ",status=yes,toolbar=no")
        }

        function ApriStampaAccertamento(sTipoDoc, IsBozza) {
            myleft = (screen.width - 600) / 2;
            console.log("../GestioneAccertamenti/StampaAccertamenti/frmRistampaA.aspx?CODTRIBUTO=" + document.getElementById('txtCOD_TRIBUTO').value + "&TIPODOC=" + sTipoDoc + "&IsBozza=" + IsBozza + "&Anno=" + document.getElementById('txtAnno').value, "", "width=600,height=500,top=20,left=" + myleft + ",status=yes,toolbar=no")
            //window.open("../GestioneAccertamenti/StampaAccertamenti/frmRistampaA.aspx?CODTRIBUTO=" + document.getElementById('txtCOD_TRIBUTO.value + "&TIPODOC=" + sTipoDoc + "&IsBozza=" + IsBozza, "", "width=600,height=500,top=20,left=" + myleft + ",status=yes,toolbar=no")            
            document.getElementById('DivAttesa').style.display = '';
            document.getElementById('divStampa').style.display = '';
            document.getElementById('divAtto').style.display = 'none';
            document.getElementById('loadStampa').src = '../GestioneAccertamenti/StampaAccertamenti/frmRistampaA.aspx?CODTRIBUTO=' + document.getElementById('txtCOD_TRIBUTO').value + '&TIPODOC=' + sTipoDoc + '&IsBozza=' + IsBozza;
        }
        function ApriStampaBollettiniViolazione() {
            myleft = (screen.width - 600) / 2;
            //window.open("../GestioneLiquidazioni/StampaLiquidazioni/frmRistampaL.aspx?CODTRIBUTO=" + document.getElementById('txtCOD_TRIBUTO.value + "&TIPODOC=BOLLACC", "", "width=600,height=500,top=20left=" + myleft + ",status=yes,toolbar=no")
            document.getElementById('DivAttesa').style.display = '';
            document.getElementById('divStampa').style.display = '';
            document.getElementById('divAtto').style.display = 'none';
            document.getElementById('loadStampa').src = '../GestioneLiquidazioni/StampaLiquidazioni/frmRistampaL.aspx?CODTRIBUTO=' + document.getElementById('txtCOD_TRIBUTO').value + '&TIPODOC=BOLLACC';
        }
        function ApriStampaBollettiniViolazioneRate() {
            myleft = (screen.width - 600) / 2;
            //window.open("../GestioneLiquidazioni/StampaLiquidazioni/frmRistampaL.aspx?CODTRIBUTO=" + document.getElementById('txtCOD_TRIBUTO.value + "&TIPODOC=BOLLACC_RATE", "", "width=600,height=500,top=20left=" + myleft + ",status=yes,toolbar=no")
            document.getElementById('DivAttesa').style.display = '';
            document.getElementById('divStampa').style.display = '';
            document.getElementById('divAtto').style.display = 'none';
            document.getElementById('loadStampa').src = '../GestioneLiquidazioni/StampaLiquidazioni/frmRistampaL.aspx?CODTRIBUTO=' + document.getElementById('txtCOD_TRIBUTO').value + '&TIPODOC=BOLLACC_RATE';
        }

        function ApriStampa() {
            /*winWidth=980 
			winHeight=680 
			myleft=(screen.width-winWidth)/2 
			mytop=(screen.height-winHeight)/2 - 40 
			Parametri="IDPROVVEDIMENTO=" + document.getElementById('ID_PROVVEDIMENTO').value
			WinPopUpRicercaAnagrafica=window.open("../GestioneQuestionari/StampaQuestionari/frmRistampa.aspx?"+Parametri,"","width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no") */
        }
        function RettificaAvviso() {
            if (IsBlank(document.getElementById('txtDataStampaAvviso').value) || IsBlank(document.getElementById('txtDataNotificaAvviso').value)) {
                GestAlert('a', 'warning', '', '', 'Impossibile eseguire la Rettifica del provvedimento.\nData Stampa avviso o Data Notifica Avviso non presenti.');
            }
            else {
                /*if(!IsBlank(document.getElementById('txtDataRettificaAvviso').value) || !IsBlank(document.getElementById('txtDataAnnullamentoAvviso').value))
				{
					alert('Impossibile eseguire la Rettifica del provvedimento.\nIl provvedimento è già stato Rettificato o Annullato.');
				}
				else
				{*/
                if (confirm('Si desidera eseguire la Rettifica del provvedimento?')) {
                    GestRettifica()
                }
                //}	
            }
        }
        function Rateizzazioni() {

            if (document.getElementById('txtImporto').value < 0) {
                GestAlert('a', 'warning', '', '', 'Impossibile proseguire con la procedura di rateizzazione: Importo Provvedimento Negativo!');
                return false;
            }
            document.getElementById('btnRateizzazioni').click();
            //parent.Comandi.location.href="ComandiRateizzazioni.aspx";
        }

        function ForzaDati() {
            document.getElementById('btnForzaDati').click();
        }

        function getDateNow() {
            var d, s
            d = new Date();
            s = "";

            if (d.getDate() < 10) {
                s += "0" + d.getDate() + "/";
            }
            else {
                s += d.getDate() + "/";
            }
            if ((d.getMonth() + 1) < 10) {
                s += "0" + (d.getMonth() + 1) + "/";
            }
            else {
                s += (d.getMonth() + 1) + "/";
            }
            s += d.getYear();

            return (s);
        }
        function getDate(cDate) {
            var aTmp = cDate.split('/');
            var nDay = aTmp[0]; // `01'
            var nMonth = aTmp[1]; // `01'
            var nYear = aTmp[2]; // `2001'

            return parseInt(nYear + nMonth + nDay); // diventa `20010101'

        }
        function doDateCheck(from, to) {
            if (getDate(from.value) >= getDate(to.value)) {

                return true;
            }
            else {
                return false;
            }
        }

        function RiepilogoAtto() {

            if (document.getElementById("divDettaglioAtto").className == "divDettaglioAttoh") {
                document.getElementById("divDettaglioAtto").className = "divDettaglioAtto"
                document.getElementById("lblViewDettaglio").innerText = "Nascondi Riepilogo Atto"
            } else {
                document.getElementById("divDettaglioAtto").className = "divDettaglioAttoh"
                document.getElementById("lblViewDettaglio").innerText = "Visualizza Riepilogo Atto"
                document.getElementById("divMotivazione").className = "divMotivazioneh"
            }
        }
        function ApriDettaglioSanzioni(idLegame, idCheck, idSanzioni, bloccaCheck, id_provvedimento) {
            if (eval("idCheck".checked) == false) {
                return false;
            }
            winWidth = 700
            winHeight = 600
            myleft = (screen.width - winWidth) / 2
            mytop = (screen.height - winHeight) / 2 - 40
            caratteristiche = "toolbar=no,status,left=" + myleft + ",top=" + mytop + ",height =" + winHeight + ",width=" + winWidth + ",resizable"
            Parametri = "idLegame=" + idLegame + "&strSanzioni=" + idSanzioni + "&bloccaCheck=" + bloccaCheck + "&id_provvedimento=" + id_provvedimento
            WinPopUpSanzioni = window.open("../gestioneaccertamenti/Sanzioni/FrameSanzioni.aspx?" + Parametri, "", caratteristiche)
            //"width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no") 
            return false;
        }
        function VisualizzaDocumentoPDF() {
            document.getElementById('btnVisualizzaDocPDF').click()
        }
        function ApriDocumentoPDF(percorso) {

            winWidth = 1000
            winHeight = 700
            myleft = (screen.width - winWidth) / 2
            mytop = (screen.height - winHeight) / 2 - 40
            caratteristiche = "toolbar=no,status,left=" + myleft + ",top=" + mytop + ",height =" + winHeight + ",width=" + winWidth + ",resizable"
            window.open(percorso, "", caratteristiche);

        }
        function VisualizzaMotivazione(DescSanzione, Motivazione) {

            document.getElementById("divMotivazione").className = "divMotivazionev"

            testo = "<p>&Egrave; stata applicata la sanzione<br><b>" + DescSanzione + "</b></p>"
            if (Motivazione != "") {
                testo += "con la seguente motivazione<p class='descMotivazione'>" + Motivazione + "</p>"
            } else {
                testo += "<p class='descMotivazione'>Nessuna motivazione Presente</p>"
            }

            document.getElementById("motivazione").innerHTML = testo
            return false
        }

        function VisualizzaRiduzione(Valore) {
            document.getElementById("divMotivazione").className = "divMotivazionev"
            testo = "<p><b>Riduzioni applicate all'immobile</b></p>"
            if (Valore != "") {
                arrval = Valore.split("§")
                //testo+=Valore

                var i = 0;
                testo += "<table id='tblRiduzioni' >"
                testo += "<colgroup><col width=260><col width=50><col width=50><colgroup>"
                testo += "<thead><tr><td>Descrizione</td><td>Tipo</td><td>Valore</td></tr></thead><tbody>"
                while (i < arrval.length) {
                    var rec = arrval[i].split("#");
                    testo += "<tr>"
                    testo += "<td>" + rec[0] + "</td>";
                    testo += "<td class='Centrato'>" + rec[1] + "</td>";
                    testo += "<td class='Centrato'>" + rec[2] + "</td>";
                    testo += "</tr>"
                    i++;
                }
                testo += "</tbody></table>"
            } else {
                testo += "Nessuna Riduzione applicata"
            }
            document.getElementById("motivazione").innerHTML = testo
            return false
        }
        function RipristinaOrdinario() {
            if (document.getElementById("txtIDTIPOPROVVEDIMENTO").value == "2") {
                GestAlert('c', 'question', 'CmdRipristinaOrdinario', '', 'Si vuole eliminare l\'ingiunzione e ripristinare l\'avviso bonario?\r\nEventuali pagamenti saranno spostati sul bonario.')
            }
            else {
                GestAlert('a', 'warning', '', '', 'E\' possibile ripristinare solo un accertamento di tipo Ingiunzione!');
            }
        }
    </script>
</head>
<body class="SfondoVisualizza overflow_x_hidden" rightmargin="0">
    <form id="Form1" runat="server" method="post">
        <table width="100%">
            <!--blocco dati contribuente-->
            <%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
            <tr id="TRPlainAnag">
                <td>
                    <iframe id="ifrmAnag" runat="server" src="../../aspVuota.aspx" style="height: 200px; width: 100%" frameborder="0" marginheight="0"></iframe>
                    <asp:HiddenField ID="hdIdContribuente" runat="server" Value="-1" />
                </td>
            </tr>
            <tr id="TRSpecAnag">
                <td>
                    <!--DATI ANAGRAFICI-->
                    <table class="dati_anagrafe_tarsu_blu" border="0" cellspacing="0" cellpadding="2" width="98%" height="102">
                        <tr>
                            <td style="height: 44px">
                                <asp:Label ID="lblCognomeNome" runat="server" Width="243px" Height="12px"></asp:Label></td>
                            <td style="height: 44px">
                                <asp:Label ID="Label32" runat="server" Width="32px" Height="12px">CF/P.IVA:</asp:Label><asp:Label ID="lblCfPiva" runat="server"></asp:Label></td>
                            <td style="height: 44px">
                                <asp:Label ID="Label33" runat="server" Width="32px" Height="12px">SESSO:</asp:Label><asp:Label ID="lblSesso" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label34" runat="server" Width="131px" Height="12px">DATA DI NASCITA:</asp:Label><asp:Label ID="lblDataNascita" runat="server"></asp:Label></td>
                            <td colspan="2">
                                <asp:Label ID="Label35" runat="server" Width="159px" Height="12px">COMUNE DI NASCITA:</asp:Label><asp:Label ID="lblComuneNascita" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="Label36" runat="server" Width="116px" Height="12px">RESIDENTE IN:</asp:Label><asp:Label ID="lblResidenza" runat="server" Width="550px"></asp:Label></td>
                        </tr>
                    </table>
                    <!--FINE DATI ANAGRAFICI-->
                </td>
            </tr>
        </table>
        <hr class="hr" size="1" width="98%">
        <div id="divAtto">
            <div style="display: none" id="DISABILITA_IMPORTI">
                <table class="bordo_tabelle" border="0" cellspacing="0" cellpadding="0" width="98%">
                    <tr>
                        <td width="50%">
                            <table class="SFONDO_TABELLA_TOTALI" border="0" cellspacing="0" cellpadding="5" width="300">
                                <tr>
                                    <td class="riga_menu" width="200" align="left">
                                        <asp:Label ID="Label68" runat="server" Width="195px" Height="12px">TOTALE AVVISO:</asp:Label></td>
                                    <td style="height: 10px" class="riga_menu" align="left">
                                        <asp:Label ID="lblImpTotAvvisoPieno" runat="server" Height="12px" CssClass="riga_menu"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="riga_menu" width="200" align="left">
                                        <asp:Label ID="Label37" runat="server" Width="195px" Height="12px">TOTALE AVVISO RIDOTTO:</asp:Label></td>
                                    <td style="height: 10px" class="riga_menu" align="left">
                                        <asp:Label ID="lblImpTotAvvisoRidotto" runat="server" Height="12px" CssClass="riga_menu"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="riga_menu" width="200">
                                        <asp:Label ID="Label38" runat="server" Height="12px" Width="195px">TOTALE PAGATO:</asp:Label></td>
                                    <td class="riga_menu" align="left">
                                        <asp:Label ID="lblTotPagato" runat="server" Height="12px"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="riga_menu" width="200" align="left">
                                        <asp:Label ID="Label39" runat="server" Width="195px" Height="12px">RESIDUO:</asp:Label></td>
                                    <td class="riga_menu" align="left">
                                        <asp:Label ID="lblResiduo" runat="server" Width="121px" Height="12px">0</asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="riga_menu" width="200" align="left">
                                        <asp:Label ID="Label40" runat="server" Width="195px" Height="12px">DATA SOLLECITO:</asp:Label></td>
                                    <td class="riga_menu" align="left">
                                        <asp:Label ID="lblSollecito" runat="server" Height="12px"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="riga_menu" width="200" align="left">
                                        <asp:Label ID="Label41" runat="server" Width="195px" Height="12px">DATA COATTIVO:</asp:Label></td>
                                    <td class="riga_menu" align="left">
                                        <asp:Label ID="lblCoattivo" runat="server" Height="12px"></asp:Label></td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" colspan="2" align="left">
                            <table class="bordo_tabelle" cellspacing="1" cellpadding="1" width="100%">
                                <tr class="intest_riga_tabella">
                                    <td colspan="4" align="center">
                                       <div id="attesaGestioneAtti_ATTI" runat="server" style="z-index: 101; position: absolute;display:none;">
                                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                                            <div class="BottoneClessidra">&nbsp;</div>
                                            <div class="Legend">Attendere Prego</div>
                                        </div>
                                        DETTAGLIO PAGAMENTI
                                    </td>
                                </tr>
                                <!--<tr class="RIGA_TABELLA_WHITE" borderColor="red">
									    <td align="center">Rata
									    </td>
									    <td align="center">data pagamento
									    </td>
									    <td align="center">importo pagato €
									    </td>
									    <td align="center">Provenienza
									    </td>
								    </tr>-->
                            </table>
                            <!--<tr><td>-->
                            <Grd:RibesGridView ID="grdPagamenti" runat="server" BorderStyle="None"
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <PagerStyle CssClass="CartListFooter" />
                                <RowStyle CssClass="CartListItem"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="Data">
                                        <HeaderStyle></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# FncGrd.GiraData(DataBinder.Eval(Container, "DataItem.Data_Pagamento")) %>' ID="Label44" NAME="Label44"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Importo">
                                        <HeaderStyle></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# FncGrd.ConvertABS(DataBinder.Eval(Container, "DataItem.Importo_Pagato")) %>' ID="Label45" NAME="Label45"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Provenienza">
                                        <HeaderStyle></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DESCRIZIONEPROVENIENZA") %>' ID="Label46"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </Grd:RibesGridView>
                            <!--</td>
								    </tr>
							    </table>-->
                        </td>
                    </tr>
                </table>
                <hr class="hr" size="1" width="98%">
            </div>
            <table class="bordo_tabelle" border="0" cellspacing="1" cellpadding="1" width="98%">
                <tr class="intest_riga_tabella" bordercolor="red">
                    <td colspan="9" align="center">
                        <asp:Label ID="lblTributo" runat="server" CssClass="riga_menu_11"></asp:Label>- 
						    N°
						    <asp:Label ID="lblNumeroAvviso" runat="server" CssClass="riga_menu_11"></asp:Label>- 
						    ANNO
						    <asp:Label ID="lblAnnoAvviso" runat="server" CssClass="riga_menu_11"></asp:Label>- 
						    Elaborato il:
						    <asp:Label ID="lblDATAGENERAZIONE" runat="server" CssClass="riga_menu_11"></asp:Label></td>
                </tr>
                <!--<tr class="RIGA_TABELLA_WHITE" id="RIGA_IMPORTI" style="DISPLAY: none">
					    <td align="center">Differenza Imposta €
					    </td>
					    <td align="center" width="13%">Sanzioni €
					    </td>
					    <td align="center" width="13%">Interessi €
					    </td>
					    <td align="center" width="11%">Addizionali €
					    </td>
					    <td align="center" width="11%">Spese €
					    </td>
					    <td align="center" width="16%">Importo Arrot. €
					    </td>
					    <td align="center" width="16%">Totale €
					    </td>
				    </tr>-->
            </table>
            <Grd:RibesGridView ID="GrdDettaglioAvviso" runat="server" BorderStyle="None"
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="98%"
                AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                <Columns>
                    <asp:TemplateField HeaderText="Diff. d'Imposta">
                        <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="right" VerticalAlign="Middle"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# FncGrd.ConvertABS(DataBinder.Eval(Container, "DataItem.IMPORTO_DIFFERENZA_IMPOSTA")) %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Totale Sanzioni">
                        <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# FncGrd.ConvertABS(DataBinder.Eval(Container, "DataItem.IMPORTO_SANZIONI")) %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sanzioni Non Riducibili">
                        <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# FncGrd.ConvertABS(DataBinder.Eval(Container, "DataItem.IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI")) %>' ID="Label48">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sanzioni Ridotte">
                        <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# FncGrd.ConvertABS(DataBinder.Eval(Container, "DataItem.IMPORTO_SANZIONI_RIDOTTO")) %>' ID="Label47">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Interessi">
                        <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# FncGrd.ConvertABS(DataBinder.Eval(Container, "DataItem.IMPORTO_INTERESSI")) %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Addizionali">
                        <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# FncGrd.ConvertABS(DataBinder.Eval(Container, "DataItem.IMPORTO_ALTRO")) %>' ID="Label43">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Arr.">
                        <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# FncGrd.ConvertABS(DataBinder.Eval(Container, "DataItem.IMPORTO_ARROTONDAMENTO")) %>' ID="Label42">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Spese">
                        <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# FncGrd.ConvertABS(DataBinder.Eval(Container, "DataItem.IMPORTO_SPESE")) %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Totale">
                        <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# FncGrd.ConvertABS(DataBinder.Eval(Container, "DataItem.IMPORTO_TOTALE")) %>' ID="Label51">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Arr. Rid.">
                        <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# FncGrd.ConvertABS(DataBinder.Eval(Container, "DataItem.IMPORTO_ARROTONDAMENTO_RIDOTTO")) %>' ID="Label50">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Totale Ridotto">
                        <HeaderStyle HorizontalAlign="center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# FncGrd.ConvertABS(DataBinder.Eval(Container, "DataItem.IMPORTO_TOTALE_RIDOTTO")) %>' ID="Label49">
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Grd:RibesGridView>
            <asp:Label ID="lblRateizzato" runat="server" CssClass="Input_Label bold"></asp:Label><asp:TextBox Style="display: none" ID="txtIDTIPOPROVVEDIMENTO" runat="server"></asp:TextBox>
            <hr class="hr" size="1" width="98%">
            <!-- dettaglio atto-->
            <asp:Label ID="lblViewDettaglio" title="Riepilogo Atto" runat="server" CssClass="Input_Label pointer">Visualizza Riepilogo Atto</asp:Label>
            <div id="divDettaglioAtto" class="divDettaglioAttoh col-md-12">
                <div id="divMotivazione" class="divMotivazioneh">
                    <div class="chiudi">
                        <input id="Annulla" class="Bottone Bottoneannulla" title="Nascondi" onclick="document.getElementById('divMotivazione').className = 'divMotivazioneh'"
                            type="button" name="Annulla">
                    </div>
                    <div id="motivazione" class="cmotivazione"></div>
                </div>
                <fieldset id="fldDicICI" runat="server" class="col-md-11">
                    <!-- dichiarato ICI -->
                    <legend class="Legend">Riepilogo dati 
						    Dichiarato</legend>
                    <asp:Label ID="lblNFDichICI" runat="server" CssClass="NormalRed AllineaSinistra">Nessun Immobile Dichiarato</asp:Label>
                    <!--*** 20120704 - IMU ***-->
                    <Grd:RibesGridView ID="GrdDichiaratoICI" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="Progressivo" ReadOnly="True" HeaderText="Prg">
                                <HeaderStyle Width="10px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField Visible="true" HeaderText="Dal">
                                <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FncGrd.ParseDate(DataBinder.Eval(Container, "DataItem.daL")) %>' ID="Label52">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="true" HeaderText="Al">
                                <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FncGrd.ParseDate(DataBinder.Eval(Container, "DataItem.AL")) %>' ID="Label53">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Foglio" ReadOnly="True" HeaderText="Fg">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Numero" ReadOnly="True" HeaderText="N&#176;">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Sub">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FncGrd.IntForGridView(DataBinder.Eval(Container, "DataItem.Subalterno")) %>' ID="Label54">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="categoria" ReadOnly="True" HeaderText="Cat">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="classe" ReadOnly="True" HeaderText="Cl">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField Visible="true" HeaderText="Cons">
                                <HeaderStyle Width="10px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FncGrd.IntForGridView(DataBinder.Eval(Container, "DataItem.Consistenza")) %>' ID="Label55" name="Label5">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField Visible="true" DataField="TIPORENDITA" ReadOnly="True" HeaderText="TR">
                                <HeaderStyle Width="10px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="valore" ReadOnly="True" HeaderText="Rend/Val" DataFormatString="{0:#,##0.00}">
                                <HeaderStyle Width="25px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="percPossesso" ReadOnly="True" HeaderText="% Poss">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
						    <asp:BoundField DataField="TITPOSSESSO" ReadOnly="True" HeaderText="Tit.Poss">
							    <headerstyle width="15px"></HeaderStyle>
							    <itemstyle horizontalalign="Left"></ItemStyle>
						    </asp:BoundField>
                            <asp:BoundField DataField="TotDovuto" ReadOnly="True" HeaderText="Importo ICI/IMU" DataFormatString="{0:#,##0.00}">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="idlegame" ReadOnly="True" HeaderText="Leg">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>
                    <br>
                    <asp:Label ID="lblDichICI" runat="server" Width="553px" CssClass="Input_Label_14 bold"></asp:Label>
                </fieldset>
                <fieldset id="fldAccICI" runat="server" class="col-md-11">
                    <!-- accertato ICI-->
                    <legend class="Legend">Riepilogo dati 
						    Accertato</legend>
                    <asp:Label ID="lblNFAccICI" runat="server" CssClass="NormalRed AllineaSinistra">Nessun Immobile Accertato</asp:Label>
                    <Grd:RibesGridView ID="GrdAccertatoICI" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="Progressivo" ReadOnly="True" HeaderText="Prg">
                                <HeaderStyle Width="10px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField Visible="true" HeaderText="Dal">
                                <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FncGrd.ParseDate(DataBinder.Eval(Container, "DataItem.DAL")) %>' ID="lblDal">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="true" HeaderText="Al">
                                <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FncGrd.ParseDate(DataBinder.Eval(Container, "DataItem.AL")) %>' ID="lblAl">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Foglio" ReadOnly="True" HeaderText="Fg">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Numero" ReadOnly="True" HeaderText="N&#176;">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Sub">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FncGrd.IntForGridView(DataBinder.Eval(Container, "DataItem.Subalterno")) %>' ID="Label56" name="Label3">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Categoria" ReadOnly="True" HeaderText="Cat">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CLASSE" ReadOnly="True" HeaderText="Cl">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Cons">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FncGrd.IntForGridView(DataBinder.Eval(Container, "DataItem.Consistenza")) %>' ID="Label57" name="Label3">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField Visible="true" DataField="TIPORENDITA" ReadOnly="True" HeaderText="TR">
                                <HeaderStyle Width="10px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Valore" ReadOnly="True" HeaderText="Rend/Val">
                                <HeaderStyle Width="25px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="percPossesso" ReadOnly="True" HeaderText="% Poss">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
						    <asp:BoundField DataField="TITPOSSESSO" ReadOnly="True" HeaderText="Tit.Poss">
							    <headerstyle width="15px"></HeaderStyle>
							    <itemstyle horizontalalign="Left"></ItemStyle>
						    </asp:BoundField>
                            <asp:BoundField Visible="False" DataField="IDSANZIONI" ReadOnly="True" HeaderText="IDSanzioni"></asp:BoundField>
                            <asp:BoundField Visible="False" DataField="IDLEGAME" ReadOnly="True" HeaderText="IDLegame"></asp:BoundField>
                            <asp:BoundField DataField="TotDovuto" ReadOnly="True" HeaderText="Importo ICI/IMU">
                                <HeaderStyle Width="25px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DIFFIMPOSTA" ReadOnly="True" HeaderText="Diff. Imposta">
                                <HeaderStyle Width="25px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Imp. Sanzioni">
                                <HeaderStyle Width="25px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FncGrd.IntForGridView(DataBinder.Eval(Container, "DataItem.IMPSanzioni")) %>' ID="Label62" name="Label3">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Imp. Interessi">
                                <HeaderStyle Width="25px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FncGrd.IntForGridView(DataBinder.Eval(Container, "DataItem.IMPInteressi")) %>' ID="Label67" name="Label3">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField Visible="False" DataField="percPossesso" ReadOnly="True" HeaderText="% Poss">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField Visible="False" HeaderText="Princ">
                                <HeaderStyle Width="25px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox Enabled="False" ID="chkPrinc" runat="server"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False" HeaderText="Pert">
                                <HeaderStyle Width="25px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox Enabled="False" ID="chkPert" runat="server"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False" HeaderText="Rid">
                                <HeaderStyle Width="25px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox Enabled="False" ID="chkRidotto" runat="server"></asp:CheckBox>
                                    <asp:HiddenField runat="server" ID="hfDESC_SANZIONE" Value='<%# Eval("DescrSanzioni") %>' />
                                    <asp:HiddenField runat="server" ID="hfTITPOSSESSO" Value='<%# Eval("TITPOSSESSO") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sanz">
                                <HeaderStyle Width="20px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox Enabled="true" ID="chkSanzioni" runat="server"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="IDLegame" ReadOnly="True" HeaderText="Leg">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>
                    <br>
                </fieldset>
                <fieldset id="fldDicTARSU" runat="server" class="col-md-11">
                    <!-- dichiarato TARSU -->
                    <legend class="Legend">Riepilogo dati Dichiarato</legend>
                    <asp:Label ID="lblNFDicTARSU" runat="server" CssClass="NormalRed AllineaSinistra">Nessun Immobile Dichiarato</asp:Label>
                    <Grd:RibesGridView ID="GrdDichiaratoTARSU" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Dal">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataInizio")) %>' ID="Label1">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Al">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataFine")) %>' ID="Label2">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ubicazione">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.svia") & " " & FncGrd.FormattaNumeriGrd(DataBinder.Eval(Container, "DataItem.scivico")) & " " & DataBinder.Eval(Container, "DataItem.sinterno")%>' ID="Label29">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="sfoglio" ReadOnly="True" HeaderText="Foglio">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="snumero" ReadOnly="True" HeaderText="Numero">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ssubalterno" ReadOnly="True" HeaderText="Sub.">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sdescrCategoria" ReadOnly="True" HeaderText="Cat.">
                                <HeaderStyle Width="10px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="nComponenti" ReadOnly="True" HeaderText="NC">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="nComponentiPV" ReadOnly="True" HeaderText="NC PV">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Forza PV">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkForzaPV" runat="server" ReadOnly="True" Checked='<%# DataBinder.Eval(Container, "DataItem.bForzaPV")%>'></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ImpTariffa" ReadOnly="True" HeaderText="Tariffa">
                                <HeaderStyle Width="10px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="nMQ" ReadOnly="True" HeaderText="MQ">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="nBimestri" ReadOnly="True" HeaderText="Bim/GG">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ImpNetto" ReadOnly="True" HeaderText="Imp. Ruolo" DataFormatString="{0:#,##0.00}">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Rid">
                                <HeaderStyle Width="20px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox Enabled="true" ID="chkRiduzioniTarsuDich" runat="server"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Det">
                                <HeaderStyle Width="20px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox Enabled="true" ID="chkDetassazioniTarsuDich" runat="server"></asp:CheckBox>
                                    <asp:HiddenField runat="server" ID="hfImpRiduzione" Value='<%# Eval("ImpRiduzione") %>' />
                                    <asp:HiddenField runat="server" ID="hfImpDetassazione" Value='<%# Eval("ImpDetassazione") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="IdLegame" ReadOnly="True" HeaderText="Leg"></asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>
                    <br>
                    <asp:Label ID="lblDichTARSU" runat="server" Width="553px" CssClass="Input_Label"></asp:Label>
                </fieldset>
                <fieldset id="fldAccTARSU" runat="server" class="col-md-11">
                    <!-- accertato TARSU-->
                    <legend class="Legend">Riepilogo dati Accertato</legend>
                    <asp:Label ID="lblNFAccTARSU" runat="server" CssClass="NormalRed AllineaSinistra">Nessun Immobile Accertato</asp:Label>
                    <Grd:RibesGridView ID="GrdAccertatoTARSU" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Dal">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataInizio")) %>' ID="Label1">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Al">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataFine")) %>' ID="Label2">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ubicazione">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.svia") & " " & FncGrd.FormattaNumeriGrd(DataBinder.Eval(Container, "DataItem.scivico")) & " " & DataBinder.Eval(Container, "DataItem.sinterno")%>' ID="Label29">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="sfoglio" ReadOnly="True" HeaderText="Foglio">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="snumero" ReadOnly="True" HeaderText="Numero">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ssubalterno" ReadOnly="True" HeaderText="Sub.">
                                <HeaderStyle Width="15px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="sdescrCategoria" ReadOnly="True" HeaderText="Cat.">
                                <HeaderStyle Width="10px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="nComponenti" ReadOnly="True" HeaderText="NC">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="nComponentiPV" ReadOnly="True" HeaderText="NC PV">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Forza PV">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkForzaPV" runat="server" ReadOnly="True" Checked='<%# DataBinder.Eval(Container, "DataItem.bForzaPV")%>'></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ImpTariffa" ReadOnly="True" HeaderText="Tariffa">
                                <HeaderStyle Width="10px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="nMQ" ReadOnly="True" HeaderText="MQ">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="nBimestri" ReadOnly="True" HeaderText="Bim/GG">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ImpRuolo" ReadOnly="True" HeaderText="Imp. Ruolo" DataFormatString="{0:#,##0.00}">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ImpNetto" ReadOnly="True" HeaderText="Imp. Netto" DataFormatString="{0:#,##0.00}">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ImpSanzioni" ReadOnly="True" HeaderText="Imp. Sanzioni" DataFormatString="{0:#,##0.00}">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ImpInteressi" ReadOnly="True" HeaderText="Imp. Interessi" DataFormatString="{0:#,##0.00}">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Imp. Totale">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FncForGrd.CalcolaTotaliGrd(DataBinder.Eval(Container, "DataItem.ImpNetto"), DataBinder.Eval(Container, "DataItem.ImpInteressi"), DataBinder.Eval(Container, "DataItem.ImpSanzioni"))%>' ID="Label66">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sanz">
                                <HeaderStyle Width="20px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox Enabled="true" ID="chkSanzioniTarsu" runat="server"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rid">
                                <HeaderStyle Width="20px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox Enabled="true" ID="chkRiduzioniTarsu" runat="server"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Det">
                                <HeaderStyle Width="20px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox Enabled="true" ID="chkDetassazioniTarsu" runat="server"></asp:CheckBox>
                                    <asp:HiddenField runat="server" ID="hfImpRiduzione" Value='<%# Eval("ImpRiduzione") %>' />
                                    <asp:HiddenField runat="server" ID="hfImpDetassazione" Value='<%# Eval("ImpDetassazione") %>' />
                                    <asp:HiddenField runat="server" ID="hfsDescrSanzioni" Value='<%# Eval("sDescrSanzioni") %>' />
                                    <asp:HiddenField runat="server" ID="hfSanzioni" Value='<%# Eval("Sanzioni") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="IdLegame" ReadOnly="True" HeaderText="Leg"></asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>
                </fieldset>
                <!--*** 20130801 - accertamento OSAP ***-->
                <fieldset id="fldDicOSAP" runat="server" class="col-md-11">
                    <!-- dichiarato OSAP -->
                    <legend class="Legend">Riepilogo dati 
						    Dichiarato</legend>
                    <asp:Label ID="lblNFDicOSAP" runat="server" CssClass="NormalRed AllineaSinistra">Nessun Immobile Dichiarato</asp:Label>
                    <Grd:RibesGridView ID="GrdDichiaratoOSAP" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Ubicazione (Rif.Catastali)">
                                <ItemStyle Width="300px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label69" runat="server" Text='<%# OPENgovTOCO.SharedFunction.FormattaVia(DataBinder.Eval(Container, "DataItem.sVia"), DataBinder.Eval(Container, "DataItem.Civico"), DataBinder.Eval(Container, "DataItem.Interno"), DataBinder.Eval(Container, "DataItem.Esponente"), DataBinder.Eval(Container, "DataItem.Scala")) %>'>Label</asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Inizio">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label70" runat="server" Text='<%# OPENgovTOCO.SharedFunction.FormattaData(DataBinder.Eval(Container, "DataItem.DataInizioOccupazione"))%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Fine">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label71" runat="server" Text='<%# OPENgovTOCO.SharedFunction.FormattaData(DataBinder.Eval(Container, "DataItem.DataFineOccupazione"))%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Durata">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle Width="120px" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label72" runat="server" Text='<%# FncForGrd.FormattaDurCons(DataBinder.Eval(Container, "DataItem.DurataOccupazione"), DataBinder.Eval(Container, "DataItem.TipoDurata.Descrizione"))%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo Occup.">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle Width="300px" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label73" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.TipologiaOccupazione.Descrizione"))%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cat.">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle Width="300px" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label74" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.Categoria.Descrizione"))%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cons.">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label75" runat="server" Text='<%# FncForGrd.FormattaDurCons(DataBinder.Eval(Container, "DataItem.Consistenza"), DataBinder.Eval(Container, "DataItem.TipoConsistenzaTOCO.Descrizione"))%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tariffa">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label76" runat="server" Text='<%# FncForGrd.FormattaCalcolo("T", DataBinder.Eval(Container, "DataItem.Calcolo"))%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Imp.">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label77" runat="server" Text='<%# FncForGrd.FormattaCalcolo("I", DataBinder.Eval(Container, "DataItem.Calcolo"))%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="IdLegame" ReadOnly="True" HeaderText="Leg"></asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>
                    <br>
                    <asp:Label ID="lblDichOSAP" runat="server" Width="553px" CssClass="Input_Label"></asp:Label>
                </fieldset>
                <fieldset id="fldAccOSAP" runat="server" class="col-md-11">
                    <!-- accertato OSAP-->
                    <legend class="Legend">Riepilogo dati 
						    Accertato</legend>
                    <asp:Label ID="lblNFAccOSAP" runat="server" CssClass="NormalRed AllineaSinistra">Nessun Immobile Accertato</asp:Label>
                    <Grd:RibesGridView ID="GrdAccertatoOSAP" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Ubicazione (Rif.Catastali)">
                                <ItemStyle Width="300px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label78" runat="server" Text='<%# OPENgovTOCO.SharedFunction.FormattaVia(DataBinder.Eval(Container, "DataItem.sVia"), DataBinder.Eval(Container, "DataItem.Civico"), DataBinder.Eval(Container, "DataItem.Interno"), DataBinder.Eval(Container, "DataItem.Esponente"), DataBinder.Eval(Container, "DataItem.Scala")) %>'>Label</asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Inizio">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label79" runat="server" Text='<%# OPENgovTOCO.SharedFunction.FormattaData(DataBinder.Eval(Container, "DataItem.DataInizioOccupazione"))%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Fine">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label80" runat="server" Text='<%# OPENgovTOCO.SharedFunction.FormattaData(DataBinder.Eval(Container, "DataItem.DataFineOccupazione"))%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Durata">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle Width="120px" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label81" runat="server" Text='<%# FncForGrd.FormattaDurCons(DataBinder.Eval(Container, "DataItem.DurataOccupazione"), DataBinder.Eval(Container, "DataItem.TipoDurata.Descrizione"))%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo Occup.">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle Width="300px" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label82" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.TipologiaOccupazione.Descrizione"))%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cat.">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle Width="300px" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label83" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.Categoria.Descrizione"))%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cons.">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label84" runat="server" Text='<%# FncForGrd.FormattaDurCons(DataBinder.Eval(Container, "DataItem.Consistenza"), DataBinder.Eval(Container, "DataItem.TipoConsistenzaTOCO.Descrizione"))%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tariffa">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label85" runat="server" Text='<%# FncForGrd.FormattaCalcolo("T", DataBinder.Eval(Container, "DataItem.Calcolo"))%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Imp.">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label86" runat="server" Text='<%# FncForGrd.FormattaCalcolo("I", DataBinder.Eval(Container, "DataItem.Calcolo"))%>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ImpDiffImposta" ReadOnly="True" HeaderText="Diff. Imposta" DataFormatString="{0:#,##0.00}">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ImpSanzioni" ReadOnly="True" HeaderText="Imp. Sanzioni" DataFormatString="{0:#,##0.00}">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ImpInteressi" ReadOnly="True" HeaderText="Imp. Interessi" DataFormatString="{0:#,##0.00}">
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Imp. Totale">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# FncForGrd.CalcolaTotaliGrd(DataBinder.Eval(Container, "DataItem.ImpDiffImposta"), DataBinder.Eval(Container, "DataItem.ImpInteressi"), DataBinder.Eval(Container, "DataItem.ImpSanzioni"))%>' ID="Label87">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sanz">
                                <HeaderStyle Width="20px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox Enabled="true" ID="chkSanzioniOSAP" runat="server"></asp:CheckBox>
                                    <asp:HiddenField runat="server" ID="hfSanzioni" Value='<%# Eval("Sanzioni") %>' />
                                    <asp:HiddenField runat="server" ID="hfDescrSanzioni" Value='<%# Eval("DescrSanzioni") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="IdLegame" ReadOnly="True" HeaderText="Leg"></asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>
                </fieldset>
                <!--*** ***-->
                <fieldset id="fldPreAcc" runat="server" class="col-md-11">
                    <legend class="Legend">Riepilogo Dati PreAccertamento</legend>
                    <br>
                    <table class="Input_Label" border="0" cellspacing="0" cellpadding="2" width="580" align="left">
                        <colgroup>
                            <col width="80">
                            <col width="80">
                            <col width="80">
                            <col width="80">
                            <col width="160">
                            <col width="80">
                        </colgroup>
                        <tr align="right">
                            <td>Dichiarato</td>
                            <td>
                                <asp:Label ID="LblImpDichPreAcc" runat="server"></asp:Label></td>
                            <td>Versato</td>
                            <td>
                                <asp:Label ID="LblImpVersPreAcc" runat="server"></asp:Label></td>
                            <td>Differenza di imposta</td>
                            <td>
                                <asp:Label ID="LblImpDifImpPreAcc" runat="server"></asp:Label></td>
                        </tr>
                        <tr align="right">
                            <td colspan="4">&nbsp;</td>
                            <td>Sanzioni</td>
                            <td>
                                <asp:Label ID="LblImpSanzPreAcc" runat="server"></asp:Label></td>
                        </tr>
                        <tr align="right">
                            <td colspan="4">&nbsp;</td>
                            <td>Interessi</td>
                            <td>
                                <asp:Label ID="LblImpIntPreAcc" runat="server"></asp:Label></td>
                        </tr>
                        <tr align="right">
                            <td colspan="4">&nbsp;</td>
                            <td>Totale</td>
                            <td>
                                <asp:Label ID="LblImpTotPreAcc" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset id="fldAcc" runat="server" class="col-md-11">
                    <legend class="Legend">Riepilogo Dati Fase Accertamento</legend>
                    <br>
                    <table class="Input_Label" border="0" cellspacing="0" cellpadding="2" width="580" align="left">
                        <colgroup>
                            <col width="80">
                            <col width="80">
                            <col width="80">
                            <col width="80">
                            <col width="160">
                            <col width="80">
                        </colgroup>
                        <tr align="right">
                            <td>Dichiarato</td>
                            <td>
                                <asp:Label ID="LblImpDich" runat="server"></asp:Label></td>
                            <td>Accertato</td>
                            <td>
                                <asp:Label ID="LblImpAcc" runat="server"></asp:Label></td>
                            <td>Differenza di imposta</td>
                            <td>
                                <asp:Label ID="LblImpDifImp" runat="server"></asp:Label></td>
                        </tr>
                        <tr align="right">
                            <td colspan="4">&nbsp;</td>
                            <td>Sanzioni</td>
                            <td>
                                <asp:Label ID="LblImpSanz" runat="server"></asp:Label></td>
                        </tr>
                        <tr align="right">
                            <td colspan="4">&nbsp;</td>
                            <td>Sanzioni Ridotte</td>
                            <td>
                                <asp:Label ID="LblImpSanzRid" runat="server"></asp:Label></td>
                        </tr>
                        <tr align="right">
                            <td colspan="4">&nbsp;</td>
                            <td>Interessi</td>
                            <td>
                                <asp:Label ID="LblImpInt" runat="server"></asp:Label></td>
                        </tr>
                        <tr align="right">
                            <td colspan="4">&nbsp;</td>
                            <td>Totale</td>
                            <td>
                                <asp:Label ID="LblImpTot" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset class="col-md-11">
                    <legend class="Legend">Riepilogo Totali</legend>
                    <br>
                    <table id="tblRiepTotali" class="Input_Label" border="0" cellspacing="0" cellpadding="2"
                        width="580" runat="server">
                        <colgroup>
                            <col width="80">
                            <col width="80">
                            <col width="80">
                            <col width="80">
                            <col width="160">
                            <col width="80">
                        </colgroup>
                        <tr align="right">
                            <td colspan="4">&nbsp;</td>
                            <td>Differenza di imposta</td>
                            <td>
                                <asp:Label ID="LblImpDifImpAvviso" runat="server"></asp:Label></td>
                        </tr>
                        <tr align="right">
                            <td colspan="4">&nbsp;</td>
                            <td>Sanzioni</td>
                            <td>
                                <asp:Label ID="LblImpSanzAvviso" runat="server"></asp:Label></td>
                        </tr>
                        <tr align="right">
                            <td colspan="4">&nbsp;</td>
                            <td>Sanzioni Ridotte</td>
                            <td>
                                <asp:Label ID="LblImpSanzRidAvviso" runat="server"></asp:Label></td>
                        </tr>
                        <tr align="right">
                            <td colspan="4">&nbsp;</td>
                            <td>Interessi</td>
                            <td>
                                <asp:Label ID="LblImpIntAvviso" runat="server"></asp:Label></td>
                        </tr>
                        <tr align="right">
                            <td colspan="4">&nbsp;</td>
                            <td>Totale</td>
                            <td>
                                <asp:Label ID="LblImpTotAvviso" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <hr class="hr" size="1" width="98%">
            <div style="display: none" id="DISABILITA">
                <fieldset class="classeFiledSet">
                    <legend class="Legend">Dati 
						    Contribuente
                    </legend>
                    <table id="Tablebb_1" border="0" cellpadding="0" width="98%">
                        <tr>
                            <td>
                                <asp:Label ID="lblCognome" runat="server" CssClass="Input_Label">Cognome</asp:Label></td>
                            <td>
                                <asp:Label ID="Label28" runat="server" CssClass="Input_Label">Nome</asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtCognome" runat="server" Width="215px" CssClass="Input_Text_Enable_Red_Left"
                                    ReadOnly="True"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtNome" runat="server" Width="215px" CssClass="Input_Text_Enable_Red_Left"
                                    ReadOnly="True"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="Input_Label" width="50%">
                                <asp:Label ID="lblCodiceFIscale" runat="server">Codice Fiscale</asp:Label></td>
                            <td class="Input_Label" width="50%">
                                <asp:Label ID="lblPArtitaIVA" runat="server">Partita Iva</asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtCodiceFiscale" runat="server" Width="215px" CssClass="Input_Text_Enable_Red_Left"
                                    ReadOnly="True"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtPartitaIVA" runat="server" Width="215px" CssClass="Input_Text_Enable_Red_Left"
                                    ReadOnly="True"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="Input_Label">
                                <asp:Label ID="Label26" runat="server">Indirizzo</asp:Label></td>
                            <td class="Input_Label">
                                <asp:Label ID="Label27" runat="server">Comune Residenza</asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtIndirizzo" runat="server" Width="300px" CssClass="Input_Text_Enable_Red_Left"
                                    ReadOnly="True"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtComuneResidenza" runat="server" Width="215px" CssClass="Input_Text_Enable_Red_Left"
                                    ReadOnly="True"></asp:TextBox></td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset class="classeFiledSet">
                    <legend class="Legend">Estremi Atto
                    </legend>
                    <table id="Tablebb_2" border="0" cellpadding="0" width="98%">
                        <tr>
                            <td class="Input_Label" width="25%">
                                <asp:Label ID="lblNumeroProvvedimento" runat="server">Numero Provvedimento</asp:Label></td>
                            <td class="Input_Label" width="25%">
                                <asp:Label ID="lblTipoTributo" runat="server">Tipo Provvedimento - Tributo</asp:Label></td>
                            <td class="Input_Label" width="25%">
                                <asp:Label ID="lblAnno" runat="server">Anno</asp:Label></td>
                        </tr>
                        <tr>
                            <td width="25%">
                                <asp:TextBox ID="txtNumeroProvvedimento" runat="server" Width="120px" CssClass="Input_Text_Enable_Red_right"
                                    ReadOnly="True"></asp:TextBox></td>
                            <td width="25%">
                                <asp:TextBox ID="txtTipoTributo" runat="server" Width="230px" CssClass="Input_Text_Enable_Red_left"
                                    ReadOnly="True"></asp:TextBox></td>
                            <td width="25%">
                                <asp:TextBox ID="txtAnno" runat="server" Width="90px" CssClass="Input_Text_Enable_Red_right OnlyNumber" ReadOnly="True"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="Input_Label" width="25%" colspan="3">
                                <asp:Label ID="Label11" runat="server">Data Generazione Avviso</asp:Label></td>
                        </tr>
                        <tr>
                            <td width="25%" colspan="3">
                                <asp:TextBox ID="txtDataGenerazione" runat="server" CssClass="Input_Text_Enable_Red_Left TextDate" ReadOnly="True"></asp:TextBox></td>
                        </tr>
                        <!--<TR>
						    <TD class="Input_Label" width="25%"><asp:label id="lblTipoProvvedimento" runat="server">Tipo Provvedimento</asp:label></TD>
						
					    </TR>-->
                        <!--<tr>
						    <TD width="25%"><asp:textbox id="txtTipoProvvedimento" runat="server" CssClass="Input_Text_Enable_Red_left" Width="215px"
								    ReadOnly="True"></asp:textbox></TD>
						
					    </tr>-->
                    </table>
                </fieldset>
                <div style="display: none" id="IMPORTI">
                    <fieldset class="classeFiledSet">
                        <legend class="Legend">Dettaglio 
							    Importi €
                        </legend>
                        <table id="Tablebb_3" border="0" cellpadding="0" width="98%">
                            <tr>
                                <td class="Input_Label" width="25%">
                                    <asp:Label ID="lblImportoDifferenzaImposta" runat="server">Importo Differenza Imposta</asp:Label></td>
                                <td class="Input_Label" width="25%">
                                    <asp:Label ID="lblImportoSanzioni" runat="server">Importo Sanzioni</asp:Label></td>
                                <td class="Input_Label" width="25%">
                                    <asp:Label ID="lblImportoInteressi" runat="server">Importo Interessi</asp:Label></td>
                            </tr>
                            <tr>
                                <td width="25%">
                                    <asp:TextBox ID="txtImportoDifferenzaImposta" runat="server" Width="215px" CssClass="Input_Text_Enable_Red_right OnlyNumber" ReadOnly="True"></asp:TextBox></td>
                                <td width="25%">
                                    <asp:TextBox ID="txtImportoSanzioni" runat="server" Width="215px" CssClass="Input_Text_Enable_Red_right OnlyNumber" ReadOnly="True"></asp:TextBox></td>
                                <td width="25%">
                                    <asp:TextBox ID="txtImportoInteressi" runat="server" Width="215px" CssClass="Input_Text_Enable_Red_right OnlyNumber" ReadOnly="True"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="Input_Label" width="25%">
                                    <asp:Label ID="lblImportoSpese" runat="server">Importo Spese</asp:Label></td>
                                <td class="Input_Label" width="25%">
                                    <asp:Label ID="lblAltroImporto" runat="server">Altro Importo</asp:Label></td>
                                <td class="Input_Label" width="25%">
                                    <asp:Label ID="lblImportoTotale" runat="server">Importo Totale</asp:Label></td>
                            </tr>
                            <tr>
                                <td width="25%">
                                    <asp:TextBox ID="txtImportoSpese" runat="server" Width="215px" CssClass="Input_Text_Enable_Red_right OnlyNumber" ReadOnly="True"></asp:TextBox></td>
                                <td width="25%">
                                    <asp:TextBox ID="txtAltroImporto" runat="server" Width="215px" CssClass="Input_Text_Enable_Red_right OnlyNumber" ReadOnly="True"></asp:TextBox></td>
                                <td width="25%">
                                    <asp:TextBox ID="txtImportoTotale" runat="server" Width="215px" CssClass="Input_Text_Enable_Red_right OnlyNumber" ReadOnly="True"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="Input_Label" width="25%" colspan="3">
                                    <asp:Label ID="lblImportoPagato" runat="server">Importo Pagato</asp:Label></td>
                            </tr>
                            <tr>
                                <td width="25%">
                                    <asp:TextBox ID="txtImportoPagato" runat="server" Width="215px" CssClass="Input_Text_Enable_Red_right" ReadOnly="True"></asp:TextBox></td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </div>
            <div style="display: none" id="QUESTIONARIO">
                <fieldset class="classeFiledSet">
                    <legend class="Legend">Gestione Date
                    </legend>
                    <table id="Tablebb_5" border="0" cellpadding="0" width="98%">
                        <tr>
                            <td class="Input_Label" width="25%">
                                <asp:Label ID="Label4" runat="server">Data Stampa</asp:Label></td>
                            <td class="Input_Label" width="25%">
                                <asp:Label ID="Label5" runat="server">Data Consegna Spedizione</asp:Label></td>
                        </tr>
                        <tr>
                            <td width="25%">
                                <asp:TextBox ID="txtDataStampa" runat="server" CssClass="Input_Text_Enable_Red_left TextDate" ReadOnly="True"></asp:TextBox></td>
                            <td width="25%">
                                <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);" ID="txtDataConsegna" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="Input_Label" width="25%">
                                <asp:Label ID="Label14" runat="server">Data Notifica</asp:Label></td>
                            <td class="Input_Label" width="25%">
                                <asp:Label ID="Label20" runat="server">Data Pervenuto il</asp:Label>&nbsp;
								<div class="tooltip">
                                    <a onclick="AttivaPervenutoIl();" href="javascript: void(0)">
                                        <img border="0" alt="" src="../../images/cancel.png" width="10px" height="10px">
                                        <span class="tooltiptext">Consente di modifcare Data di Pervenuto</span>
                                    </a>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td width="25%">
                                <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);" ID="txtDataNotifica" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                            <td width="25%">
                                <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);" ID="txtPervenutoQuestionari" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                        </tr>
                    </table>
                </fieldset>
            </div>
            <div style="display: none" id="LIQUIDAZIONE">
                <fieldset class="classeFiledSet">
                    <legend class="Legend">Gestione Date
                    </legend>
                    <table id="Tablebb_4" border="0" cellpadding="0" width="100%">
                        <br>
                        <tr>
                            <td class="Input_Label" width="25%">
                                <asp:Label ID="Label18" runat="server">Data Conferma Avviso</asp:Label></td>
                            <td class="Input_Label" width="25%">
                                <asp:Label ID="Label19" runat="server">Data Stampa Avviso</asp:Label></td>
                            <td class="Input_Label" width="25%">
                                <asp:Label ID="lblDataConsegnaAvviso" runat="server">Data Consegna Avviso</asp:Label></td>
                            <td class="Input_Label" width="25%">
                                <asp:Label ID="Label6" runat="server">Data Notifica Avviso</asp:Label></td>
                            <tr>
                                <td width="25%">
                                    <asp:TextBox ID="txtDataConfermaAvviso" runat="server" CssClass="Input_Text_Enable_Red_Left TextDate" ReadOnly="True"></asp:TextBox></td>
                                <td width="25%">
                                    <asp:TextBox ID="txtDataStampaAvviso" runat="server" CssClass="Input_Text_Enable_Red_Left TextDate" ReadOnly="True"></asp:TextBox></td>
                                <td width="25%">
                                    <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);" ID="txtDataConsegnaAvviso" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                                <td width="25%">
                                    <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);" ID="txtDataNotificaAvviso" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                            </tr>
                        <tr>
                            <td class="Input_Label" width="25%">
                                <asp:Label ID="Label1" runat="server">Data Rettifica Avviso</asp:Label></td>
                            <td class="Input_Label" width="25%">
                                <asp:Label ID="Label2" runat="server">Data Annullamento Avviso</asp:Label></td>
                            <td class="Input_Label" width="25%">
                                <asp:Label ID="Label3" runat="server">Data Sosp. Avviso Autotutela</asp:Label></td>
                            <td class="Input_Label" width="25%">
                                <asp:Label ID="Label88" runat="server">Data Irreperibilità</asp:Label></td>
                        </tr>
                        <tr>
                            <td width="25%">
                                <asp:TextBox ID="txtDataRettificaAvviso" runat="server" CssClass="Input_Text_Enable_Red_Left TextDate" ReadOnly="True"></asp:TextBox></td>
                            <td width="25%">
                                <asp:TextBox ID="txtDataAnnullamentoAvviso" runat="server" CssClass="Input_Text_Enable_Red_Left TextDate" ReadOnly="True"></asp:TextBox></td>
                            <td width="25%">
                                <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);" ID="txtDataSospensioneAvvisoAutotutela" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                            <td width="25%">
                                <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);" ID="txtDataIrreperibile" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <br>
                                <fieldset class="classeFiledSet">
                                    <legend class="Legend">1° Grado di 
										    Giudizio&nbsp;
                                    </legend>
                                    <table border="0" width="100%">
                                        <tr>
                                            <td style="width: 20%" class="Input_Label" width="20%">
                                                <asp:Label ID="Label8" runat="server">Data Presentazione<br>Ricorso</asp:Label></td>
                                            <td style="width: 20%" class="Input_Label" width="20%">
                                                <asp:Label ID="Label9" runat="server">Data Sosp.Comm.<br>Tributaria</asp:Label></td>
                                            <td style="width: 20%" class="Input_Label" width="20%">
                                                <asp:Label ID="Label10" runat="server">Data Sentenza</asp:Label></td>
                                            <td style="width: 40%" class="Input_Label" width="40%">
                                                <asp:Label ID="lblNoteProvinciale" runat="server">Note / Motivazioni</asp:Label></td>
                                        </tr>
                                        <tr valign="top">
                                            <td style="width: 20%" width="20%">
                                                <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);" ID="txtDataRicorsoProvinciale" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                                            <td style="width: 20%" width="20%">
                                                <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);" ID="txtSospensioneProvinciale" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                                            <td style="width: 20%" width="20%">
                                                <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);" ID="txtSentenzaProvinciale" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                                            <td style="width: 40%" width="40%">
                                                <asp:TextBox ID="txtNoteProvinciale" runat="server" Width="320px" Height="46px" CssClass="Input_Text"
                                                    MaxLength="2000" TextMode="MultiLine"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <fieldset class="classeFiledSet">
                                    <legend class="Legend">Conciliazione Giudiziale</legend>
                                    <table border="0" width="100%" align="center">
                                        <tr>
                                            <td class="Input_Label" width="25%">
                                                <asp:Label ID="lblFlagConcGiudiz" runat="server">&nbsp;</asp:Label></td>
                                            <td class="Input_Label" width="25%">
                                                <asp:Label ID="lblNoteConcGiudiz" runat="server">Note / Motivazioni</asp:Label></td>
                                        </tr>
                                        <tr valign="top">
                                            <td class="Input_Label" width="25%">
                                                <asp:CheckBox ID="ckConcGiudiz" runat="server" Text="Conciliazione Giudiziale"></asp:CheckBox></td>
                                            <td class="Input_Label" width="50%">
                                                <asp:TextBox ID="txtNoteConcGiudiz" runat="server" Width="400px" Height="40px" CssClass="Input_Text" MaxLength="2000" TextMode="MultiLine"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <fieldset class="classeFiledSet">
                                    <legend class="Legend">2° Grado di Giudizio</legend>
                                    <table border="0" width="100%" align="left">
                                        <tr>
                                            <td style="width: 20%" class="Input_Label" width="20%">
                                                <asp:Label ID="Label7" runat="server">Data Presentazione<br>Ricorso</asp:Label></td>
                                            <td style="width: 20%" class="Input_Label" width="20%">
                                                <asp:Label ID="Label21" runat="server">Data Sosp.Comm.<br>Tributaria</asp:Label></td>
                                            <td style="width: 20%" class="Input_Label" width="20%">
                                                <asp:Label ID="Label22" runat="server">Data Sentenza</asp:Label></td>
                                            <td style="width: 40%" class="Input_Label" width="40%">
                                                <asp:Label ID="lblNoteRegionale" runat="server">Note / Motivazioni</asp:Label></td>
                                        </tr>
                                        <tr valign="top">
                                            <td style="width: 20%" width="20%">
                                                <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);" ID="txtDataRicorsoRegionale" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                                            <td style="width: 20%" width="20%">
                                                <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);" ID="txtSospensioneRegionale" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                                            <td style="width: 20%" width="20%">
                                                <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);" ID="txtSentenzaRegionale" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                                            <td style="width: 40%" width="40%">
                                                <asp:TextBox ID="txtNoteRegionale" runat="server" Width="320px" Height="46px" CssClass="Input_Text"
                                                    MaxLength="2000" TextMode="MultiLine"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <fieldset class="classeFiledSet">
                                    <legend class="Legend">Accertamenti con Adesione</legend>
                                    <table id="tabella" border="0" cellpadding="0" width="100%">
                                        <tr>
                                            <td style="width: 25%" class="Input_Label" width="25%">
                                                <asp:Label ID="lblFlagAcc" runat="server">&nbsp;</asp:Label></td>
                                            <td style="width: 25%" class="Input_Label" width="25%">
                                                <asp:Label ID="lblEsitoAcc" runat="server">Esito</asp:Label></td>
                                            <td style="width: 25%" class="Input_Label" width="25%">
                                                <asp:Label ID="lblTermineRicorso" runat="server">Nuovo Termine di Ricorso</asp:Label></td>
                                            <td style="width: 25%" class="Input_Label" width="25%">
                                                <asp:Label ID="lblNoteAccertamenti" runat="server">Note / Motivazioni</asp:Label></td>
                                        </tr>
                                        <tr valign="top">
                                            <td class="Input_Label" width="25%">
                                                <asp:CheckBox ID="ckAccertamento" runat="server" Text="Accertamenti con Adesione"></asp:CheckBox></td>
                                            <td class="Input_Label" width="15%">
                                                <asp:DropDownList ID="ddlEsitoAccertamenti" runat="server" CssClass="Input_Text">
                                                    <asp:ListItem Selected="True" Value="-1">...</asp:ListItem>
                                                    <asp:ListItem Value="0">Conferma</asp:ListItem>
                                                    <asp:ListItem Value="1">Rettifica</asp:ListItem>
                                                    <asp:ListItem Value="2">Annullamento</asp:ListItem>
                                                </asp:DropDownList></td>
                                            <td class="Input_Label" width="25%">
                                                <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);" ID="txtTermineRicorso" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="50"></asp:TextBox></td>
                                            <td style="width: 35%" class="Input_Label" width="35%">
                                                <asp:TextBox ID="txtNoteAccertamenti" runat="server" Width="300px" Height="46px" CssClass="Input_Text" MaxLength="2000" TextMode="MultiLine"></asp:TextBox></td>
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
                                            <td style="display:none" class="Input_Label" width="20%">
                                                <asp:Label ID="Label23" runat="server">Data Presentazione<br>Ricorso</asp:Label></td>
                                            <td class="Input_Label" width="20%">
                                                <asp:Label ID="Label24" runat="server">Data</asp:Label></td>
                                            <td style="display:none" class="Input_Label" width="20%">
                                                <asp:Label ID="Label25" runat="server">Data Sentenza</asp:Label></td>
                                            <td style="width: 40%" class="Input_Label" width="40%">
                                                <asp:Label ID="lblNoteCassazione" runat="server">Numero</asp:Label></td>
                                        </tr>
                                        <tr valign="top">
                                            <td style="display:none" width="20%">
                                                <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);" ID="txtDataRicorsoCassazione" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                                            <td width="20%">
                                                <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);" ID="txtSospensioneCassazione" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                                            <td style="display:none" width="20%">
                                                <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);" ID="txtSentenzaCassazione" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                                            <td style="width: 40%" width="40%">
                                                <asp:TextBox ID="txtNoteCassazione" runat="server" Width="320px" Height="46px" CssClass="Input_Text" MaxLength="2000" TextMode="MultiLine"></asp:TextBox></td>
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
                                            <td class="Input_Label"><asp:Label ID="Label29" runat="server">Data Atto Definitivo</asp:Label></td>
                                            <td class="Input_Label"><asp:Label ID="Label16" runat="server">Data Ruolo (TARSU)</asp:Label></td>
                                            <td class="Input_Label"><asp:Label ID="Label12" runat="server">Data Pagamento</asp:Label></td>
                                            <td class="Input_Label"><asp:Label ID="Label13" runat="server">Data Concessione Rate</asp:Label></td>
                                            <td class="Input_Label"><asp:Label ID="Label15" runat="server">Data Ruolo Coattivo</asp:Label></td>
                                            <td class="Input_Label"><asp:Label ID="Label17" runat="server">Importo Ruolo Coattivo</asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:TextBox ID="txtDataAttoDefinitivo" runat="server" CssClass="Input_Text_Enable_Red_Left TextDate" ReadOnly="True"></asp:TextBox></td>
                                            <td><asp:TextBox ID="txtDataRuoloOrdinario" runat="server" CssClass="Input_Text_Enable_Red_Left TextDate" ReadOnly="True"></asp:TextBox></td>
                                            <td><asp:TextBox ID="txtDataVersamentoUnicaSoluzione" runat="server" CssClass="Input_Text_Enable_Red_Left TextDate" ReadOnly="True"></asp:TextBox></td>
                                            <td><asp:TextBox ID="txtDataConcessioneRateizzazione" runat="server" CssClass="Input_Text_Enable_Red_Left TextDate" ReadOnly="True"></asp:TextBox></td>
                                            <td><asp:TextBox ID="txtDataRuoloCoattivoICI" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                                            <td><asp:TextBox ID="txtImportoRuoloCoattivoICI" runat="server" Width="130px" CssClass="Input_Text_Enable_Red_Right OnlyNumber" ReadOnly="true">0</asp:TextBox></td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                    <div style="float:left;">
                        <asp:Label ID="lblNoteGenerali" runat="server" CssClass="Legend">Note Generali Atto</asp:Label><br>
                        <asp:TextBox ID="txtNoteGenerali" runat="server" Width="797px" Height="46px" CssClass="Input_Text" MaxLength="2000" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </fieldset>
                <br>
                <br>
            </div>
        </div>
        <div id="DivAttesa" runat="server" style="z-index: 103; position: absolute;display:none;">
            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
            <div class="BottoneClessidra">&nbsp;</div>
            <div class="Legend">Attendere Prego...<asp:Label ID="LblAvanzamento" runat="server"></asp:Label></div>
        </div>
        <div id="divStampa" class="col-md-12 classeFiledSetRicerca" style="display:none;">
            <iframe runat="server" id="loadStampa" name="loadStampa" frameborder="0" width="100%" height="250px" src="../../aspvuota.aspx"></iframe>
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
        <asp:HiddenField ID="hfBollettinoF24" runat="server" Value="0" />
        <asp:HiddenField ID="hfIdProvvedimento" runat="server" Value="0" />
        <asp:TextBox CssClass="hidden" ID="hdTestoFisso" runat="server" TextMode="MultiLine"></asp:TextBox>
        <asp:TextBox CssClass="hidden" ID="txtDateNow" runat="server"></asp:TextBox>
        <asp:TextBox CssClass="hidden" ID="txtCOD_TRIBUTO" runat="server"></asp:TextBox>
        <asp:TextBox CssClass="hidden" ID="txtANNO_RETTIFICA" runat="server"></asp:TextBox>
        <asp:TextBox CssClass="hidden" ID="txtDATA_ELABORAZIONE" runat="server"></asp:TextBox>
        <asp:TextBox CssClass="hidden" ID="txtTIPO_OPERAZIONE" runat="server"></asp:TextBox>
        <asp:TextBox CssClass="hidden" ID="txtTIPO_PROCEDIMENTO" runat="server"></asp:TextBox>
        <asp:TextBox CssClass="hidden" ID="txtTIPO_RICERCA" runat="server"></asp:TextBox>
        <asp:TextBox CssClass="hidden" ID="txtNOME_OGGETTO" runat="server"></asp:TextBox>
        <asp:TextBox CssClass="hidden" ID="txtNOMINATIVO_RETTIFICA" runat="server"></asp:TextBox>
        <asp:TextBox CssClass="hidden" ID="txtOPEN_RETTIFICA" runat="server"></asp:TextBox>
        <asp:TextBox CssClass="hidden" ID="txtHiddenPERVENUTOIL" runat="server"></asp:TextBox>
        <asp:TextBox CssClass="hidden" ID="txtImporto" runat="server"></asp:TextBox>
        <asp:TextBox CssClass="hidden" ID="TxtNomePDF" runat="server"></asp:TextBox>
        <asp:Button CssClass="hidden" ID="btnSalvaLiquidazioni" runat="server"></asp:Button>
        <asp:Button CssClass="hidden" ID="btnStampaBozza" runat="server"></asp:Button>
        <asp:Button CssClass="hidden" ID="btnStampa" runat="server"></asp:Button>
        <asp:Button CssClass="hidden" ID="btnStampaBollettini" runat="server"></asp:Button>
        <asp:Button CssClass="hidden" ID="btnStampaQuestionari" runat="server"></asp:Button>
        <asp:Button CssClass="hidden" ID="btnStampaAccertamenti" runat="server"></asp:Button>
        <asp:Button CssClass="hidden" ID="btnRateizzazioni" runat="server"></asp:Button>
        <asp:Button CssClass="hidden" ID="btnForzaDati" runat="server"></asp:Button>
        <asp:Button CssClass="hidden" ID="btnVisualizzaDocPDF" runat="server"></asp:Button>
        <asp:Button CssClass="hidden" ID="btnRicarica" runat="server"></asp:Button>
        <asp:Button CssClass="hidden" ID="CmdGoBack" runat="server"></asp:Button>
        <asp:Button CssClass="hidden" ID="CmdRipristinaOrdinario" runat="server"></asp:Button>
    </form>
</body>
</html>
