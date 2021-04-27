<%@ Page Language="vb" AutoEventWireup="false" Codebehind="FormAnagrafica.aspx.vb" Inherits="PAGINA.FormAnagrafica" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Anagrafica</title>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
        <script type="text/javascript" src="../_js/Utility.js?newversion"></script>
		<script type="text/javascript" src="../_js/VerificaCampi.js?newversion"></script>
		<script type="text/vbscript" src="../_js/OperazioniSuCampi.vbs"></script>
		<script type="text/vbscript" src="../_vbs/ControlliFormali.vbs"></script>
		<script type="text/javascript" src="../_js/CODICEFISCALE.js?newversion"></script>
		<script type="text/javascript" src="../_js/jsComuniStrade.js?newversion"></script>
		<script type="text/javascript">
		    function EliminaContatto() {
		        if (!confirm('Confermare la cancellazione del Contatto?')) {
		            return false;
		        }
		        else {
		            if (VerificaDatiContratto()) {
		                document.getElementById('btnEliminaContatti').click();
		            }
		        }
		    }

		    function buttonCancella() {
		        if ((document.getElementById('hdCOD_CONTRIBUENTE').value != '-1') && (document.getElementById('hdIDDATAANAGRAFICA').value != '-1')) {
		            parent.Comandi.document.getElementById('CANCELLA').style.display = '';
		        }
		        else {
		            parent.Comandi.document.getElementById('CANCELLA').style.display = 'none';
		        }
		    }

		    function controlla(max, Max, maxlettere) {
		        if (max.value.length > maxlettere)
		            max.value = max.value.substring(0, maxlettere);
		    }

		    function Conferma() {
		        if (!confirm('Cancellare l\'anagrafica del soggetto?')) {
		            return false;
		        }
		        else {
		            DivAttesa.style.display = '';
		            return true;
		        }
		    }

		    function ModificaContatti(lb, desc, IDRIFERIMENTO, DataValiditaInvioMAIL) {
		        document.getElementById('hdIDRIFERIMENTO').value = unescape(IDRIFERIMENTO);
		        document.getElementById('cboTipoContatto').selectedIndex = unescape(lb);
		        document.getElementById('txtDatiRiferimento').value = unescape(desc);
		        //*** 20140515 - invio mail ***
		        if (unescape(DataValiditaInvioMAIL) != '')
		            document.getElementById('chkInvioInformativeViaMail').checked = true;
		        else
		            document.getElementById('chkInvioInformativeViaMail').checked = false;
		        document.getElementById('txtDataInizioInvio').value = unescape(DataValiditaInvioMAIL);
		    }

		    function MessageNotFound() {
		        GestAlert('a', 'warning', '', '','La ricerca non ha prodotto risultati !!!');
		        return false;
		    }
		    function VerificaDatiContratto() {
		        sMsg = ""

		        var isel = document.getElementById('cboTipoContatto').selectedIndex;

		        if (document.getElementById('cboTipoContatto').value == '-1') {
		            sMsg = sMsg + "[Titolo Soggetto]\n";
		        }
		        if (IsBlank(document.getElementById('txtDatiRiferimento').value)) {
		            sMsg = sMsg + "[Dati del Riferimento]\n";
		        }
		        //*** 20140515 - invio mail ***
		        if (document.getElementById("chkInvioInformativeViaMail").checked) {
		            if (IsBlank(document.getElementById("txtDataInizioInvio").value)) {
		                sMsg = sMsg + "[Data inizio invio]\n";
		                GestAlert('a', 'warning', '', '',"Inserire la Data di Inizio invio!");
		                Setfocus(document.getElementById("txtDataInizioInvio"));
		                return false;
		            }
		            else {
		                if (!isDate(document.getElementById("txtDataInizioInvio").value)) {
		                    GestAlert('a', 'warning', '', '',"Inserire la Data di Inizio invio correttamente in formato: GG/MM/AAAA!");
		                    Setfocus(document.getElementById("txtDataInizioInvio"));
		                    return false;
		                }
		            }
		        }
		        else {
		            if (!IsBlank(document.getElementById("txtDataInizioInvio").value)) {
		                if (!isDate(document.getElementById("txtDataInizioInvio").value)) {
		                    GestAlert('a', 'warning', '', '',"Inserire la Data di Inizio invio correttamente in formato: GG/MM/AAAA!");    
		                    Setfocus(document.getElementById("txtDataInizioInvio"));
		                    return false;
		                }
		                else {
		                    document.getElementById("chkInvioInformativeViaMail").checked = "true";
		                }
		            }
		        }
		        //*** ***
		        if (!IsBlank(sMsg)) {
		            strMessage = "Attenzione...\n\n I campi elencati sono obbligatori!\n\n"
		            GestAlert('a', 'warning', '', '',strMessage + sMsg);
		            Setfocus(document.getElementById('txtDatiRiferimento'))
		            return false;
		        }
		        else {
		            var valueRif = document.getElementById('cboTipoContatto').value;
		            switch (valueRif) {
		                case "1":
		                    if (!CheckPhoneNumber(document.getElementById('txtDatiRiferimento').value)) {
		                        strMessage = "Attenzione...\n\n il numero di Fax inserito non è valido!\n\n"
		                        GestAlert('a', 'warning', '', '', strMessage);
		                        Setfocus(document.getElementById('txtDatiRiferimento'))
		                        return false;
		                    }
		                    break;
		                case "2":
		                    if (!CheckPhoneNumber(document.getElementById('txtDatiRiferimento').value)) {
		                        strMessage = "Attenzione...\n\n il numero di Telefono Ufficio inserito non è valido!\n\n"
		                        GestAlert('a', 'warning', '', '', strMessage);
		                        Setfocus(document.getElementById('txtDatiRiferimento'))
		                        return false;
		                    }
		                    break;
		                case "3":

		                    if (!CheckPhoneNumber(document.getElementById('txtDatiRiferimento').value)) {
		                        strMessage = "Attenzione...\n\n il numero di Telefono Abitazione inserito non è valido!\n\n"
		                        GestAlert('a', 'warning', '', '', strMessage);
		                        Setfocus(document.getElementById('txtDatiRiferimento'))
		                        return false;
		                    }
		                    break;
		                case "4":
		                    if (!echeck(document.getElementById('txtDatiRiferimento').value)) {
		                        strMessage = "Attenzione...\n\n E-mail inserita non valida!\n\n"
		                        GestAlert('a', 'warning', '', '', strMessage);
		                        Setfocus(document.getElementById('txtDatiRiferimento'))
		                        return false;
		                    }
		                    break;
		                case "5":
		                    if (!CheckPhoneNumber(document.getElementById('txtDatiRiferimento').value)) {
		                        strMessage = "Attenzione...\n\n il numero di Cellulare  inserito non è valido!\n\n"
		                        GestAlert('a', 'warning', '', '', strMessage);
		                        Setfocus(document.getElementById('txtDatiRiferimento'))
		                        return false;
		                    }
		                    break;
		                default:
		            }
		        }
		        return true;
		    }

		    function echeck(str) {
		        var at = "@"
		        var dot = "."
		        var lat = str.indexOf(at)
		        var lstr = str.length
		        var ldot = str.indexOf(dot)
		        if (str.indexOf(at) == -1) {
		            return false
		        }

		        if (str.indexOf(at) == -1 || str.indexOf(at) == 0 || str.indexOf(at) == lstr) {
		            return false
		        }

		        if (str.indexOf(dot) == -1 || str.indexOf(dot) == 0 || str.indexOf(dot) == lstr) {
		            return false
		        }

		        if (str.indexOf(at, (lat + 1)) != -1) {
		            return false
		        }

		        if (str.substring(lat - 1, lat) == dot || str.substring(lat + 1, lat + 2) == dot) {
		            return false
		        }

		        if (str.indexOf(dot, (lat + 2)) == -1) {
		            return false
		        }

		        if (str.indexOf(" ") != -1) {
		            return false
		        }

		        return true
		    }


		    function CheckPhoneNumber(TheNumber) {
		        var valid = 1
		        var GoodChars = "0123456789()-+ "
		        var i = 0

		        if (TheNumber == "") {
		            // Return false if number is empty
		            valid = 0;
		        }
		        for (i = 0; i <= TheNumber.length - 1; i++) {
		            if (GoodChars.indexOf(TheNumber.charAt(i)) == -1) {
		                // Note: Remove the comments from the following line to see this
		                // for loop in action.
		                // alert(TheNumber.charAt(i) + " is no good.")
		                valid = 0;
		            } // End if statement
		        } // End for loop
		        return valid
		    }

		    function VerificaPerCodiceFiscale() {
		        sMsg = ""
		        var isel = document.getElementById('cboSesso').selectedIndex;

		        if (IsBlank(document.getElementById('txtCognome').value)) {
		            sMsg = sMsg + "[Cognome]\n";
		        }
		        if (IsBlank(document.getElementById('txtNome').value)) {
		            sMsg = sMsg + "[Nome]\n";
		        }
		        if (document.getElementById('cboSesso').value == '-1') {
		            sMsg = sMsg + "[Sesso]\n";
		        }
		        if (IsBlank(document.getElementById('hdCodComuneNascita').value)) {
		            sMsg = sMsg + "[Luogo di Nascita]\n";
		        }

		        if (IsBlank(document.getElementById('txtDataNascita').value)) {
		            sMsg = sMsg + "[Data di Nascita]\n";
		        }
		        else {
		            if (!isDate(document.getElementById('txtDataNascita').value)) {
		                alert("Inserire la Data di Nascita correttamente in formato: GG/MM/AAAA!");
		                Setfocus(document.getElementById('txtDataNascita'));
		                return false;
		            }
		        }
		        if (!IsBlank(sMsg)) {
		            strMessage = "Attenzione...\n\n I campi elencati sono obbligatori per il calcolo del Codice Fiscale!\n\n"
		            GestAlert('a', 'warning', '', '',strMessage + sMsg);
		            Setfocus(document.getElementById('txtCognome'))
		            return false;
		        }
		        return true;
		    }

		    function VerificaCampi() {
		        sMsg = ""
		        var isel = document.getElementById('cboSesso').selectedIndex;
		        var isels = document.getElementById('cboTitoloSoggetto').selectedIndex;
		        if (document.getElementById('hdIdBancaIntestatario').value == '-1' && !IsBlank(document.getElementById('txtBancaIntestatario').value)) {
		                sMsg = sMsg + "[La Banca deve essere selezionata dalla Lista]\n";
		            }
		        if (IsBlank(document.getElementById('txtCognome').value)) {
		                sMsg = sMsg + "[Cognome]\n";
		            }
		        if (IsBlank(document.getElementById('txtCodiceFiscale').value)) {
		                sMsg = sMsg + "[Codice Fiscale/Partita IVA]\n";
		            }

		            if (document.getElementById('cboSesso').value != 'G') {
		                if (IsBlank(document.getElementById('txtNome').value)) {
		                    sMsg = sMsg + "[Nome]\n";
		                }

		                if (IsBlank(document.getElementById('txtLuogoNascita').value)) {
		                    sMsg = sMsg + "[Luogo di Nascita]\n";
		                }

		                if (IsBlank(document.getElementById('txtDataNascita').value)) {
		                    sMsg = sMsg + "[Data di Nascita]\n";
		                }
		                else {
		                    if (!isDate(document.getElementById('txtDataNascita').value)) {
		                        GestAlert('a', 'warning', '', '',"Inserire la Data di Nascita correttamente in formato: GG/MM/AAAA!");
		                        Setfocus(document.getElementById('txtDataNascita'));
		                        return false;
		                    }
		                }
		            }
		            if (IsBlank(document.getElementById('txtVia').value)) {
		                sMsg = sMsg + "[Via]\n";
		            }

		            if (IsBlank(document.getElementById('txtNCIVICO').value)) {
		                sMsg = sMsg + "[N° Civico]\n";
		            }
		            if (IsBlank(document.getElementById('txtComuneResidenza').value)) {
		                sMsg = sMsg + "[Comune di Residenza]\n";
		            }

		            if (IsBlank(document.getElementById('txtCAP').value)) {
		                sMsg = sMsg + "[C.A.P.]\n";
		            }
		            if (IsBlank(document.getElementById('txtProvinciaResidenza').value)) {
		                sMsg = sMsg + "[Provincia.]\n";
		            }
		            if (IsBlank(document.getElementById('txtTelefono').value)) {
		                sMsg = sMsg + "[Telefono]\n";
		            }

		            if (document.getElementById('cboTitoloSoggetto[isels]').value == '-1') {
		                sMsg = sMsg + "[Titolo Soggetto]\n";
		            }

		            if (!IsBlank(sMsg)) {
		                strMessage = "Attenzione...\n\n I campi elencati sono obbligatori!\n\n"
		                GestAlert('a', 'warning', '', '',strMessage + sMsg);
		                Setfocus(document.getElementById('txtCognome'))
		                return false;
		            }
		        return true;
		    }

		    /*function PulisciDatiUtente() {
		        if (document.getElementById('hdRibaltaCampi').value == 0) {
		            var form = document.frmAnagrafica;
		            form.txtCodiceFiscaleUtente.value = '';
		            form.txtCognomeUtente.value = '';
		            form.txtNomeUtente.value = '';
		            form.cboSessoUtente.selectedIndex = 0
		            form.txtComuneNascitaUtente.value = '';
		            form.txtProvinciaResidenzaNascitaUtente.value = '';
		            form.txtDataNascitaUtente.value = '';
		            form.cboTitoloUtente.selectedIndex = 0
		            form.txtNucleoFamiliareUtente.value = '';

		            form.txtIndirizzoResidenzaUtente.value = '';
		            form.txtCivicoResidenzaUtente.value = '';
		            form.txtFrazioneResidenzaUtente.value = '';
		            form.txtComuneResidenzaUtente.value = '';
		            form.txtCAPResidenzaUtente.value = '';
		            form.txtProvinciaResidenzaUtente.value = '';
		            form.txtTelefonoResidenzaUtente.value = '';
		            form.txtCellulareResidenzaUtente.value = '';
		            form.ResidenzaUtente_0.checked = false;
		            form.ResidenzaUtente_1.checked = false;

		            form.txtCognomeSpedizioneUtente.value = '';
		            form.txtNomeSpedizioneUtente.value = '';
		            form.txtIndirizzoSpedizioneUtente.value = '';
		            form.txtCivicoSpedizioneUtente.value = '';
		            form.txtFrazioneSpedizioneUtente.value = '';
		            form.txtComuneSpedizioneUtente.value = '';
		            form.txtCAPSpedizioneUtente.value = '';
		            form.txtProvinciaResidenzaSpedizioneUtente.value = '';

		            form.hdCodComuneResidenzaUtente.value = '';
		            form.hdCodComuneNascitaUtente.value = '';
		            form.hdCodComuneSpedizioneUtente.value = '';
		            form.hdCodContribuenteUtente.value = '';

		            form.hdIdBancaUtente.value = '';
		            form.hdIdTRBancaUtente.vlue = '';
		            form.txtEMailUtente.vlaue = '';
		            form.txtFaxUtente.vlaue = '';
		            form.txtBancaUtente.value = '';
		            form.txtAgenziaUtente.value = '';
		            form.txtABIUtente.value = '';
		            form.txtCABUtente.value = '';
		            form.txtNumeroCCUtente.value = '';

		            document.getElementById('hdRibaltaCampi').value = '1';
		            document.getElementById('hdModificataAnaUtente').value = '1';
		        }
		    }*/
		    /*function ChangeIndex(tabstrip) {
		        var iIndex;
		        iIndex = tabstrip.selectedIndex;
		        if (iIndex == 1) {
		            if (document.getElementById('hdRibaltaCampi').value == '0') {
		                var form = document.frmAnagrafica;
		                form.txtCodiceFiscaleUtente.value = form.txtCodiceFiscale.value;
		                form.txtCognomeUtente.value = form.txtCognome.value;
		                form.txtNomeUtente.value = form.txtNome.value;
		                form.cboSessoUtente.selectedIndex = form.cboSesso.selectedIndex;
		                form.txtComuneNascitaUtente.value = form.txtLuogoNascita.value;
		                form.txtProvinciaResidenzaNascitaUtente.value = form.txtProvinciaResidenzaNascita.value;
		                form.txtDataNascitaUtente.value = form.txtDataNascita.value;
		                form.cboTitoloUtente.selectedIndex = form.cboTitoloSoggetto.selectedIndex;
		                form.txtNucleoFamiliareUtente.value = form.txtNucleoFamiliare.value;

		                form.txtIndirizzoResidenzaUtente.value = form.txtVia.value;
		                form.txtCivicoResidenzaUtente.value = form.txtNCIVICO.value;
		                form.txtFrazioneResidenzaUtente.value = form.txtFrazione.value;
		                form.txtComuneResidenzaUtente.value = form.txtComuneResidenza.value;
		                form.txtCAPResidenzaUtente.value = form.txtCAP.value;
		                form.txtProvinciaResidenzaUtente.value = form.txtProvinciaResidenza.value;
		                form.txtTelefonoResidenzaUtente.value = form.txtTelefono.value;
		                form.txtCellulareResidenzaUtente.value = form.txtCellulare.value;
		                form.ResidenzaUtente_0.checked = form.Residenza_0.checked;
		                form.ResidenzaUtente_1.checked = form.Residenza_1.checked;

		                form.txtCognomeSpedizioneUtente.value = form.txtCognomeSpedizione.value;
		                form.txtNomeSpedizioneUtente.value = form.txtNomeSp.value;
		                form.txtIndirizzoSpedizioneUtente.value = form.txtIndirizzoSpedizione.value;
		                form.txtCivicoSpedizioneUtente.value = form.txtCivicoSpedizione.value;
		                form.txtFrazioneSpedizioneUtente.value = form.txtFrazioneSpedizione.value;
		                form.txtComuneSpedizioneUtente.value = form.txtComuneSpedizione.value;
		                form.txtCAPSpedizioneUtente.value = form.txtCAPSpedizione.value;
		                form.txtProvinciaResidenzaSpedizioneUtente.value = form.txtProvSpedizione.value;

		                form.hdCodComuneResidenzaUtente.value = form.hdCodComuneResidenza.value;
		                form.hdCodComuneNascitaUtente.value = form.hdCodComuneNascita.value;
		                form.hdCodComuneSpedizioneUtente.value = form.hdCodComuneSpedizione.value;
		                form.hdCodContribuenteUtente.value = form.hdCodContribuenteIntestatario.value;

		                form.hdIdBancaUtente.value = form.hdIdBancaIntestatario.value
		                form.hdIdTRBancaUtente.value = form.hdIdTRBancaIntestatario.value
		                form.txtEMailUtente.value = form.txtEMailIntestatario.value
		                form.txtFaxUtente.value = form.txtFaxIntestatario.value
		                form.txtBancaUtente.value = form.txtBancaIntestatario.value
		                form.txtAgenziaUtente.value = form.txtAgenziaIntestatario.value
		                form.txtABIUtente.value = form.txtABIIntestatario.value
		                form.txtCABUtente.value = form.txtCABIntestatario.value
		                form.txtNumeroCCUtente.value = form.txtNumeroCCIntestatario.value
		            }
		        }
		    }*/
		    function GetDatiAnagrafici(txtCodContribuenteTemp, objFieldCognome, objForm, strPagPrec) {
		        strTxTCognome = objFieldCognome.name;
		        strFormName = objForm.name;

		        if (!IsBlank(objFieldCognome.value) && IsBlank(txtCodContribuenteTemp.value)) {
		            if (document.getElementById('hdNoFinestra').value == '0') {
		                WinPopUp = OpenPopup('OpenTerritorio', '../Selezioni/PopUpAnagrafe.aspx?Surname=' + objFieldCognome.value + '&txtCognome=' + strTxTCognome + '&FORMNAME=' + strFormName + '&PAG_PREC=' + strPagPrec, 'Anagrafe', '770', '500', 0, 0, 'yes', 'no');
		            }
		        }
		    }		    		   
		    function TrackBlur(element) {

		        if (typeof element.id != "undefined")
		            gLastElement = element.id;
		        document.getElementById('txtNameObject').value = gLastElement;
		    }
		    function setComuniFocus(txtWait, txtFocus, txtCodComuneTemp, objFieldComune, objFieldProvincia, objFieldCap, objCodComune, objForm, strComune, strProvincia, strCap) {
		        if (!IsBlank(txtWait.value) && IsBlank(txtCodComuneTemp.value)) {
		            if (document.getElementById('hdNoFinestra').value == '0') {
		                GetDatiComune(objFieldComune, objFieldProvincia, objFieldCap, objCodComune, objForm, strComune, strProvincia, strCap)
		                Setfocus(txtFocus);
		                txtFocus.select();
		            }
		        }
		    }
		    function GetDatiComune(FieldComune, FieldProvincia, FieldCap, CodComune, Form, Comune, Provincia, Cap) {
		        //La Funzione apre il PopUp Per La scelta dei Comuni

		        COMUNE = FieldComune.name;
		        PV = FieldProvincia.name;
		        HIDDEN = CodComune.name;
		        FORM = Form1.name;
		        if (document.getElementById('hdNoFinestra').value == '0') {
		            if (typeof (Cap) == "undefined") {
		                if (IsBlank(Comune) && IsBlank(Provincia)) {
		                    GestAlert('a', 'warning', '', '','Inserire almeno un dato a scelta fra Comune e Provincia!');
		                    Setfocus(FieldComune);
		                    return false;
		                }
		                else {
		                    Parametri = "?cap=&comune=" + Comune + "&provincia=" + Provincia + "&FieldComune=" + COMUNE + "&hidden=" + HIDDEN + "&FieldProvincia=" + PV + "&form=" + FORM
		                    WinPopUp = OpenPopup('Anagrafica', './Selezioni/ListaComuni.aspx' + Parametri, 'W3', '650', '500', 0, 0, 'yes', 'no');
		                }
		            }
		            else {
		                if (IsBlank(Comune) && IsBlank(Provincia) && IsBlank(Cap)) {
		                    GestAlert('a', 'warning', '', '','Inserire almeno un dato a scelta fra Comune e CAP e Provincia!');
		                    Setfocus(FieldComune);
		                    return false;
		                }
		                else {
		                    CAP = FieldCap.name;

		                    Parametri = "?cap=" + Cap + "&comune=" + Comune + "&provincia=" + Provincia + "&FieldComune=" + COMUNE + "&hidden=" + HIDDEN + "&FieldProvincia=" + PV + "&FieldCap=" + CAP + "&form=" + FORM
		                    WinPopUp = OpenPopup('Anagrafica', './Selezioni/ListaComuni.aspx' + Parametri, 'W3', '650', '500', 0, 0, 'yes', 'no');

		                }
		            }
		        }
		    }
		    function Salva() {
		        if (confirm('Si vogliono salvare le modifiche apportate al soggetto?')) {
		            DivAttesa.style.display = '';
		            if (VerificaCampi()) {
		                document.getElementById('btnEvento').click();
		            }
		        }
		    }

		    function CodiceFiscale() {
		        if (VerificaPerCodiceFiscale()) {

		            document.getElementById('btnCodiceFiscaleServer').click();
		        }
		    }
		    function DatiDaCodiceFiscale() {
		        sMsg = ""

		        if (IsBlank(document.getElementById('txtCodiceFiscale').value)) {
		            sMsg = sMsg + "[Codice Fiscale]\n";
		        }

		        if (!IsBlank(sMsg)) {
		            strMessage = "Attenzione...\n\n I campi elencati sono obbligatori per calcolare i dati dal Codice Fiscale!\n\n"
		            GestAlert('a', 'warning', '', '',strMessage + sMsg);
		            Setfocus(document.getElementById('txtCodiceFiscale'))
		            return false;
		        }
		        else {

		            document.getElementById('btnDaCodiceFiscaleServer').click();
		        }
		    }


		    function Title() {
		        if (IsBlank(document.getElementById('txtDatiRiferimento').value)) {
		            document.getElementById('txtDatiRiferimento').title = 'Inserire la Descrizione del contatto';
		        }
		        else {
		            document.getElementById('txtDatiRiferimento').title = document.getElementById('txtDatiRiferimento').value;
		        }

		        //*** 20140515 - invio mail ***
		        if (IsBlank(document.getElementById('txtDataInizioInvio').value)) {
		            document.getElementById('txtDataInizioInvio').title = 'Inserire la Data di inizio invio';
		        }
		        else {
		            document.getElementById('txtDataInizioInvio').title = document.getElementById('txtDataInizioInvio').value;
		        }
		    }

		    function salvaContatto() {
		        if (VerificaDatiContratto()) {
		            document.getElementById('btnConfermaContatti').click();
		        }
		    }

		    function GetListaPersoneStorico(FieldCognome, FieldNome) {
		        //La Funzione apre il PopUp Per La scelta dei Comuni

		        COGNOME = FieldCognome.value;
		        NOME = FieldNome.value;
		        if (document.getElementById('hdCOD_CONTRIBUENTE').value != '-1') {

		            Parametri = "?cognome=" + COGNOME + "&nome=" + NOME
		            WinPopUp = OpenPopup('Anagrafica', '../ListaPersoneStorico.aspx' + Parametri, 'W3', '705', '400', 0, 0, 'yes', 'no');
		        }
		        else {
		            GestAlert('a', 'warning', '', '','Impossibile visualizzare lo storico di un nuovo contribuente !');
		        }

		    }
		    function MessageNotFound() {
		        GestAlert('a', 'warning', '', '','La ricerca non ha prodotto risultati !!!');
		        return false;
		    }

		    function AssegnaFuoco() {

		        if (document.getElementById('txtCognome').disabled == false) {
		            document.getElementById('txtCognome').focus();
		        }
		    }

		    function VerificaCampiObbligatori() {
		        sMsg = ""
		        if (IsBlank(document.getElementById('txtCognome').value)) {
		            sMsg = sMsg + "[Cognome]\n";
		        }
		        if (!IsBlank(sMsg)) {
		            strMessage = "Attenzione...\n\n I campi elencati sono obbligatori!\n\n"
		            GestAlert('a', 'warning', '', '',strMessage + sMsg);
		            Setfocus(document.getElementById('txtCognome'))
		            return false;
		        }
		        else {
		            DivAttesa.style.display = '';
		            return true;
		            /*if (VerificaCampiObbligatoriSpedizione()) {
		            return true;
		            }
		            else {
		            return false;
		            }*/
		        }
		    }

		    function VerificaCampiObbligatoriSpedizione() {
		        sMsg = ""

		        if (document.getElementById('ddlTributo').value == '-1') {
		            sMsg = sMsg + "[Tributo di Spedizione]\n";
		            strMessage = "Attenzione...\n\n I campi elencati sono obbligatori!\n\n"
		        }
		        if (IsBlank(document.getElementById('txtCognomeSpedizione').value)) {
		            sMsg = sMsg + "[Nominativo di Spedizione]\n";
		            strMessage = "Attenzione...\n\n I campi elencati sono obbligatori!\n\n"
		        }
		        if (IsBlank(document.getElementById('txtCAPSpedizione').value)) {
		            sMsg = sMsg + "[CAP di Spedizione]\n";
		            strMessage = "Attenzione...\n\n I campi elencati sono obbligatori!\n\n"
		        }
		        if (IsBlank(document.getElementById('txtComuneSpedizione').value)) {
		            sMsg = sMsg + "[Comune di Spedizione]\n";
		            strMessage = "Attenzione...\n\n I campi elencati sono obbligatori!\n\n"
		        }
		        if (IsBlank(document.getElementById('txtProvinciaSpedizione').value)) {
		            sMsg = sMsg + "[Provincia di Spedizione]\n";
		            strMessage = "Attenzione...\n\n I campi elencati sono obbligatori!\n\n"
		        }
		        if (IsBlank(document.getElementById('txtIndirizzoSpedizione').value)) {
		            sMsg = sMsg + "[Via di Spedizione]\n";
		            strMessage = "Attenzione...\n\n I campi elencati sono obbligatori!\n\n"
		        }
		        if (!IsBlank(sMsg)) {
		            strMessage = "Attenzione...\n\n I campi elencati sono obbligatori!\n\n"
		            GestAlert('a', 'warning', '', '',strMessage + sMsg);
		            Setfocus(document.getElementById('txtCognomeSpedizione'))
		            return false;
		        }
		        else {
		            return true;
		        }
		    }

		    function RibaltaComuneNascita(objComune) {
		        // COD COMUNE NASCITA
		        document.getElementById('hdCodComuneNascita').value = objComune.CodBelfiore;
		        // COD COMUNE RESIDENZA
		        document.getElementById('txtLuogoNascita').value = objComune.Denominazione;
		        // PROVINCIA RESIDENZA
		        document.getElementById('txtProvinciaNascita').value = objComune.Provincia;
		    }
		    function RibaltaComuneResidenza(objComune) {

		        // COD COMUNE RESIDENZA
		        document.getElementById('hdCodComuneResidenza').value = objComune.CodBelfiore;
		        // COD COMUNE RESIDENZA PER UTILIZZO STRADARIO
		        document.getElementById('hdCodComStradaResidenza').value = parseInt(objComune.CodIstat, 10);
		        // COMUNE RESIDENZA
		        document.getElementById('txtComuneResidenza').value = objComune.Denominazione;
		        // CAP RESIDENZA 
		        document.getElementById('txtCAPResidenza').value = objComune.Cap;
		        //PROV RESIDENZA
		        document.getElementById('txtProvinciaResidenza').value = objComune.Provincia;
		        // se il comune gestisce lo stradario obbligo l'utente a selezionare lo stradario
		        if (objComune.HaStradario == "True") {
		            AbilitaRicercaStradario('linkStradaResidenza', '');
		            AbilitaTxtStradario('txtViaResidenza', true);
		        } else {
		            AbilitaRicercaStradario('linkStradaResidenza', 'none');
		            AbilitaTxtStradario('txtViaResidenza', false);
		        }
		        /*for (var i in objComune) {
		        str+=i+"="+obj[i]+"\n";						
		        }
		        alert(str)*/
		    }
		    function RibaltaComuneSpedizione(objComune) {
		        // COD COMUNE SPEDIZIONE
		        document.getElementById('hdCodComuneSpedizione').value = objComune.CodBelfiore;
		        // COD COMUNE RESIDENZA PER UTILIZZO STRADARIO
		        document.getElementById('hdCodComStradaSpedizione').value = parseInt(objComune.CodIstat, 10);
		        // COMUNE SPEDIZIONE
		        document.getElementById('txtComuneSpedizione').value = objComune.Denominazione;
		        //PROV SPEDIZIONE
		        document.getElementById('txtProvinciaSpedizione').value = objComune.Provincia;
		        // CAP SPEDIZIONE
		        document.getElementById('txtCAPSpedizione').value = objComune.Cap;

		        if (objComune.HaStradario == "True") {
		            //document.getElementById('linkStradaSpedizione').style.display = '';
		            AbilitaRicercaStradario('linkStradaSpedizione', '');
		            AbilitaTxtStradario('txtIndirizzoSpedizione', true)
		        } else {
		            AbilitaRicercaStradario('linkStradaSpedizione', 'none');
		            AbilitaTxtStradario('txtIndirizzoSpedizione', false)
		        }
		    }

		    function ApriStradario(FunzioneRitorno, CodEnte) {
		        var TipoStrada = '';
		        var Strada = '';
		        var CodStrada = '';
		        var CodTipoStrada = '';
		        var Frazione = '';
		        var CodFrazione = '';

		        var Parametri = '';

		        Parametri += 'CodEnte=' + CodEnte;
		        Parametri += '&TipoStrada=' + TipoStrada;
		        Parametri += '&Strada=' + Strada;
		        Parametri += '&CodStrada=' + CodStrada;
		        Parametri += '&CodTipoStrada=' + CodTipoStrada;
		        Parametri += '&Frazione=' + Frazione;
		        Parametri += '&CodFrazione=' + CodFrazione;
		        Parametri += '&Stile=<% = Session("StileStradario") %>';
		        Parametri += '&FunzioneRitorno=' + FunzioneRitorno;

		        window.open('<% = UrlStradario %>?' + Parametri, 'fStradario', 'top =' + (screen.height - 500) / 2 + ', left=' + (screen.width - 700) / 2 + ' width=700,height=500, status=yes, toolbar=no,scrollbar=no, resizable=no');
		    }


		    // campi dell'oggetto STRADA
		    /*
		    this.CodiceEnte = CodiceEnte;
		    this.CodStrada = CodStrada;
		    this.Strada = Strada;
		    this.TipoStrada = TipoStrada;
		    this.CodTipoStrada = CodTipoStrada;
		    this.Frazione = Frazione;
		    this.CodFrazione = CodFrazione;
		    */
		    function RibaltaStradaResidenza(objStrada) {

		        // popolo il campo descrizione della via di residenza
		        document.getElementById('txtViaResidenza').value = (objStrada.TipoStrada + ' ' + objStrada.Strada).replace(/&#(\d+);/g, function (m, n) { return String.fromCharCode(n); });
		        // popolo il campo codvia residenza
		        document.getElementById('hdCODViaResidenza').value = objStrada.CodStrada;
		        // popolo il campo frazione della residenza
		        document.getElementById('txtFrazioneResidenza').value = objStrada.Frazione.replace(/&#(\d+);/g, function (m, n) { return String.fromCharCode(n); });
		    }
		    function RibaltaStradaSpedizione(objStrada) {
		        // popolo il campo descrizione della via di residenza
		        document.getElementById('txtIndirizzoSpedizione').value = (objStrada.TipoStrada + ' ' + objStrada.Strada).replace(/&#(\d+);/g, function (m, n) { return String.fromCharCode(n); });
		        // popolo il campo codvia residenza
		        document.getElementById('hdCODViaSpedizione').value = objStrada.CodStrada;
		        // popolo il campo frazione della spedizione
		        document.getElementById('txtFrazioneSpedizione').value = objStrada.Frazione.replace(/&#(\d+);/g, function (m, n) { return String.fromCharCode(n); });
		    }
		    // NomeCollegamento --> collegamento che va abilitato per la selezione dello stradario
		    function AbilitaRicercaStradario(NomeCollegamento, Display) {
		        document.getElementById(NomeCollegamento).style.display = Display;
		    }

		    // NomeControllo --> controllo che va abilitato per l'inserimento della strada
		    function AbilitaTxtStradario(NomeCollegamento, ReadOnly) {
		        document.getElementById(NomeCollegamento).readOnly = ReadOnly;
		    }

		    // Controlla il CodiceFiscale/partitaIva
		    // e poi effettua il salvataggio
		    function ControloCinPiva() {
		        //alert('ControlloCinCFPI.aspx?CF='+document.getElementById('txtCodiceFiscale').value+'&PI='+document.getElementById('txtPartitaIva').value);
		        //alert(document.getElementById('iframenascosto').src);
		        //console.log('ControlloCinCFPI.aspx?CF=' + document.getElementById('txtCodiceFiscale').value + '&PI=' + document.getElementById('txtPartitaIva').value);
		        //document.getElementById('iframenascosto').src = 'ControlloCinCFPI.aspx?CF=' + document.getElementById('txtCodiceFiscale').textContent + '&PI=' + document.getElementById('txtPartitaIva').textContent;
		        //document.getElementById('iframenascosto').location.href = '';
		        document.getElementById('btnControlloCINSalva').click();
		    }

		    function nascondi(oggetto) {
		        if (document.getElementById(oggetto).style.display == "") {
		            document.getElementById(oggetto).style.display = "none"
		        }
		        else {
		            document.getElementById(oggetto).style.display = ""
		        }
		    }

		    function SaveSpedizione(Tipo) {
		        if (Tipo == 'R') {
		            if (confirm('Si vuole ribaltare l\'indirizzo su tutti i tributi sovrascrivendo gli eventuali gia\' presenti?'))
		                document.getElementById('CmdRibaltaSpedizione').click();
		        }
		        else {
		            if (confirm('Si vogliono salvare le modifiche apportate all\'indirizzo?'))
		                document.getElementById('CmdSaveSpedizione').click();
		        }
		    }
		</script>
	    <style type="text/css">
            .style1
            {
                height: 49px;
            }
        </style>
	</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" scroll="yes" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="1" width="100%" align="center" border="0">
				<tr>
					<td>
                        <asp:Label ID="lblEnti" runat="server" CssClass="Input_Label">Ente</asp:Label><br />
                        <asp:DropDownList ID="ddlEnti" runat="server" CssClass="Input_Text"></asp:DropDownList>
                    </td>
                    <td><asp:label id="lblOperation" runat="server" CssClass="AnagraficaRow hidden"></asp:label></td>
				</tr>
				<tr>
					<td colspan="2"><asp:label id="lblConcurrencyMsg" runat="server" CssClass="NormalRed" Width="760px" Visible="False"></asp:label></td>
				</tr>
				<tr><!--Dati Residenti-->
					<td colspan="2" width="100%">
						<a id="GestResidenti" class="lstTabRow" title="NascondiDatiResidenti" onclick="nascondi('DivDatiResidenti')" href="#" runat="server">Dati Residenti</a>
						<div id="DivDatiResidenti" runat="server" style="display:none">
							<table cellSpacing="0" cellPadding="1" width="98%" align="center" border="0">
								<tr>
									<td class="Input_Label">Azione: &nbsp;
										<asp:label id="LblAzione" runat="server" CssClass="Input_Label"></asp:label>
									</td>
								</tr>
								<tr>
									<td class="Input_Label">Cod.Fiscale: &nbsp;
										<asp:label id="LblCodFiscaleRes" runat="server" CssClass="Input_Label"></asp:label>
									</td>
								</tr>
								<tr>
									<td class="Input_Label">Cognome: &nbsp;
										<asp:label id="LblCognomeRes" runat="server" CssClass="Input_Label"></asp:label>
									</td>
									<td class="Input_Label">Nome: &nbsp;
										<asp:label id="LblNomeRes" runat="server" CssClass="Input_Label"></asp:label>
									</td>
									<td class="Input_Label">Sesso: &nbsp;
										<asp:label id="LblSessoRes" runat="server" CssClass="Input_Label"></asp:label>
									</td>
								</tr>
								<tr>
									<td class="Input_Label">Comune nascita: &nbsp;
										<asp:label id="LblComuneNasRes" runat="server" CssClass="Input_Label"></asp:label>
									</td>
									<td class="Input_Label">Data Nascita: &nbsp;
										<asp:label id="LblDataNasRes" runat="server" CssClass="Input_Label"></asp:label>
									</td>
									<td class="Input_Label">Data Morte: &nbsp;
										<asp:label id="LblDataMorteRes" runat="server" CssClass="Input_Label"></asp:label>
									</td>
								</tr>
								<tr>
									<td class="Input_Label">Indirizzo: &nbsp;
										<asp:label id="LblIndirizzoRes" runat="server" CssClass="Input_Label"></asp:label>
									</td>
								</tr>
								<tr>
									<td class="Input_Label">N.Famiglia: &nbsp;
										<asp:label id="LblCodFamigliaRes" runat="server" CssClass="Input_Label"></asp:label>
									</td>
									<td class="Input_Label">Parentela: &nbsp;
										<asp:label id="LblParentelaRes" runat="server" CssClass="Input_Label"></asp:label>
									</td>
								</tr>
							</table>
						</div>
					</td>
				</tr>
				<tr>
					<td colspan="2">
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<table cellSpacing="0" cellPadding="1" width="100%" align="center" border="0">
							<tr>
								<td class="lstTabRow" colspan="5">Dati Generali</td>
							</tr>
							<tr>
								<td class="Input_Label">
								    Codice Fiscale&nbsp;
                                    <div class="tooltip">
                                        <img style="CURSOR: pointer" alt="" onclick="CodiceFiscale();" class="BottoneSel BottoneCFTo" />
                                        <span class="tooltiptext">Determina il Codice Fiscale partendo dai dati di Nascita</span>
                                    </div>&nbsp;&nbsp;&nbsp;
                                    <div class="tooltip"> 
								        <img style="CURSOR: pointer" alt="" onclick="DatiDaCodiceFiscale();" class="BottoneSel BottoneCFFrom" />
                                        <span class="tooltiptext">Determina i dati di nascita partendo dal Codice Fiscale</span>
                                    </div>
								</td>
								<td class="Input_Label">Partita Iva</td>
							</tr>
							<tr>
								<td>
								    <asp:textbox id="txtCodiceFiscale" tabIndex="1" runat="server" Width="180px" cssClass="Input_Text" MaxLength="16" ToolTip="Codice Fiscale" OnChange="javascript:this.value=this.value.toUpperCase();"></asp:textbox>
								</td>
								<td>
								    <asp:textbox id="txtPartitaIva" tabIndex="2" runat="server" Width="120px" cssClass="Input_Text_Right OnlyNumber" MaxLength="11" ToolTip="Partita Iva"></asp:textbox>
								</td>
							</tr>
							<tr>
								<td class="Input_Label">Cognome/R.sociale&nbsp;<font class="NormalRed">*</font></td>
								<td class="Input_Label" colspan="2">Nome</td>
								<td class="Input_Label">Sesso</td>
							</tr>
							<tr>
								<td>
								    <asp:textbox id="txtCognome" tabIndex="3" runat="server" CssClass="Input_Text" Width="400px" MaxLength="100" ToolTip="Cognome"></asp:textbox>
								</td>
								<td colspan="2">
								    <asp:textbox id="txtNome" tabIndex="4" runat="server" CssClass="Input_Text" Width="330px" MaxLength="50" ToolTip="Nome"></asp:textbox>
								</td>
								<td>
								    <asp:dropdownlist id="cboSesso" onfocus="TrackBlur(this);" tabIndex="5" runat="server" CssClass="Input_Text" Width="150px" AutoPostBack="false">
										<asp:listitem Value="-1">...</asp:listitem>
										<asp:listitem Value="M">MASCHIO</asp:listitem>
										<asp:listitem Value="F">FEMMINA</asp:listitem>
										<asp:listitem Value="G">PERSONA GIURIDICA</asp:listitem>
									</asp:dropdownlist>
								</td>
							</tr>
							<tr>
								<td class="Input_Label">Luogo di Nascita 
                                    <div class="tooltip">
									<a id="linkapricomunenascita" onclick="
										<%If Session("SOLA_LETTURA") = "1" Then%> 
											return false;
										<%else%>
											ApriComuni('RibaltaComuneNascita','','','','',document.getElementById('txtLuogoNascita').value,document.getElementById('txtProvinciaNascita').value, '<% = Session("StileStradario") %>', '<% = UrlPopComuni %>') 
										<%end if%>" href="javascript: void(0)" class="nascosto">
                                        <img alt="" class="BottoneSel BottoneLista">
                                        <span class="tooltiptext">Scelta Comuni</span>
									</a>
                                    </div>
								</td>
								<td class="Input_Label">Provincia</td>
								<td class="Input_Label">Data di Nascita</td>
								<td class="Input_Label">Data di Morte</td>
							</tr>
							<tr>
								<td>
									<asp:textbox id="txtLuogoNascita" onblur="if(this.value!='') document.getElementById('linkapricomunenascita').click();" tabIndex="6" runat="server" CssClass="Input_Text" Width="400px" MaxLength="50" ToolTip="Comune Di Nascita"></asp:textbox>
								</td>
								<td>
									<asp:textbox id="txtProvinciaNascita" onfocus="TrackBlur(this);" tabIndex="7" runat="server" CssClass="Input_Text" Width="50PX" MaxLength="2" ToolTip="Provincia Nascita" onblur="if(this.value!='') document.getElementById('linkapricomunenascita').click();"></asp:textbox>
								</td>
								<td>
									<asp:textbox id="txtDataNascita" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);TrackBlur(this);" tabIndex="8" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="10" ToolTip="Data Di Nascita"></asp:textbox>
								</td>
								<td>
									<asp:textbox id="txtDataMorte" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);TrackBlur(this);" tabIndex="8" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="10" ToolTip="Data Di Morte"></asp:textbox>
								</td>
							</tr>
							<tr>
								<td class="Input_Label">Nazionalita' Nascita</td>
								<td class="Input_Label">Nucleo Familiare</td>
								<td colspan="2" rowspan="2">
                                    <fieldset class="classeFieldSetRicerca">
                                        <div class="col-md-12">
                                            <div class="col-md-4">
                                                <asp:CheckBox id="chkSplitPayment" tabIndex="11" runat="server" CssClass="Input_CheckBox_NoBorder" Width="100px" MaxLength="1" ToolTip="Split Payment" Text="Split Payment"></asp:CheckBox>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Label runat="server" CssClass="Input_Label">Codice Univoco</asp:Label><br />
                                                <asp:textbox id="txtSplitCod" tabIndex="12" runat="server" CssClass="Input_Text" Width="200px" MaxLength="30" ToolTip="Codice Univoco"></asp:textbox>
                                            </div>
                                        </div>
                                    </fieldset>
								</td>
							</tr>
							<tr>
								<td>
								    <asp:textbox id="txtNazionalitaNascita" tabIndex="9" runat="server" CssClass="Input_Text" Width="400px" MaxLength="35" ToolTip="Nazionalita' di Nascita"></asp:textbox>
								</td>
								<td>
								    <asp:textbox id="txtNucleoFamiliare" tabIndex="10" runat="server" CssClass="Input_Text" Width="200px" MaxLength="20"></asp:textbox>
								</td>
							</tr>
							<!--=================================== DATI RESIDENZA=====================================================-->
							<tr>
								<td class="lstTabRow" colspan="5">Residenza/Sede Legale </td>
							</tr>
							<tr>
								<td colspan="5" class="modal-alert warning text-warning Input_Label">
									<div>
										In caso di indirizzo estero si deve compilare la nazione e compilare la provincia con la dicitura EE
									</div>	
								</td>
							</tr>
							<tr>
								<td class="Input_Label">
									Comune&nbsp;
                                    <div class="tooltip">
									<a id="linkapricomuneresidenza" onclick="
										<%if Session("SOLA_LETTURA") = "1" Then%> 
											return false;
										<%else%>
											ApriComuni('RibaltaComuneResidenza','',document.getElementById('txtCAPResidenza').value,'','',document.getElementById('txtComuneResidenza').value,document.getElementById('txtProvinciaResidenza').value, '<% = Session("StileStradario") %>', '<% = UrlPopComuni %>');
										<%end if%>" href="javascript: void(0)" class="nascosto">
                                        <img alt="" class="BottoneSel BottoneLista">
                                        <span class="tooltiptext">Scelta Comuni</span>
									</a>
                                    </div>
								</td>
								<td class="Input_Label">Cap</td>
								<td class="Input_Label">Provincia</td>
							</tr>
							<tr>
								<td>
									<asp:textbox id="txtComuneResidenza" onblur="if(this.value!='') document.getElementById('linkapricomuneresidenza').click();" onfocus="TrackBlur(this);" tabIndex="15" runat="server" CssClass="Input_Text" Width="400px" MaxLength="50" ToolTip="Comune di Residenza"></asp:textbox>
								</td>
								<td>
									<asp:textbox id="txtCAPResidenza" onblur="if(this.value!='') document.getElementById('linkapricomuneresidenza').click();" onkeyup="disableLetterChar(this);" onfocus="TrackBlur(this);" tabIndex="16" runat="server" CssClass="Input_Text" Width="130px" MaxLength="10" ToolTip="Codice Avviamento Postale"></asp:textbox>
								</td>
								<td>
									<asp:textbox id="txtProvinciaResidenza" onblur="if(this.value!='') document.getElementById('linkapricomuneresidenza').click();" onfocus="TrackBlur(this);" tabIndex="17" runat="server" CssClass="Input_Text" Width="50px" MaxLength="2" ToolTip="Provincia di Residenza"></asp:textbox>
								</td>
							</tr>
							<tr>
								<td class="Input_Label" colspan="2">
									Via/Piazza/Corso
                                    <div class="tooltip">
									<a onclick="ApriStradario('RibaltaStradaResidenza', document.getElementById('hdCodComStradaResidenza').value);" href="javascript: void(0)" id="linkStradaResidenza" style="display:none" class="nascosto"><IMG alt="" class="BottoneSel BottoneLista">
                                        <span class="tooltiptext">Scelta Strada</span></a>
                                    </div>
								</td>
								<td class="Input_Label" colspan="2">Frazione</td>
							</tr>
							<tr>
								<td colspan="2">
									<asp:textbox id="txtViaResidenza" tabIndex="18" runat="server" CssClass="Input_Text" Width="530px" MaxLength="50" ToolTip="Indirizzo Residenza"></asp:textbox>
								</td>
								<td colspan="3">
								    <asp:textbox id="txtFrazioneResidenza" tabIndex="19" runat="server" CssClass="Input_Text" Width="300px" MaxLength="50" ToolTip="Frazione Residenza"></asp:textbox>
								</td>
							</tr>
							<tr>
								<td class="Input_Label">N.Civico</td>
								<td class="Input_Label">Posizione Civico</td>
								<td class="Input_Label">Esponente Civico</td>
								<td class="Input_Label">Scala</td>
								<td class="Input_Label">Interno</td>
							</tr>
							<tr>
								<td>
								    <asp:textbox id="txtNumeroCivicoResidenza" tabIndex="20" runat="server" CssClass="Input_Text" Width="100px" MaxLength="10" ToolTip="Numero civico Residenza"></asp:textbox>
								</td>
								<td>
								    <asp:textbox id="txtPosizioneResidenza" tabIndex="21" runat="server" CssClass="Input_Text" Width="130px" MaxLength="50" ToolTip="Posizione civico Residenza"></asp:textbox>
								</td>
								<td>
								    <asp:textbox id="txtEsponenteCivicoResidenza" tabIndex="22" runat="server" CssClass="Input_Text" Width="100px" ToolTip="Esponente legato al numero civico di residenza"></asp:textbox>
								</td>
								<td>
								    <asp:textbox id="txtScalaResidenza" tabIndex="23" runat="server" CssClass="Input_Text" Width="100px" MaxLength="20" ToolTip="Numero Scala Residenza"></asp:textbox>
								</td>
								<td>
								    <asp:textbox id="txtInternoResidenza" tabIndex="24" runat="server" CssClass="Input_Text" Width="100px" MaxLength="20" ToolTip="Interno Residenza"></asp:textbox>
								</td>
							</tr>
							<tr>
								<td class="Input_Label" colspan="5">Nazione Residenza</td>
							</tr>
							<tr>
								<td>
								    <asp:textbox id="txtNazionalitaResidenza" tabIndex="25" runat="server" CssClass="Input_Text" Width="400px" MaxLength="50" ToolTip="Nazionlita' residenza"></asp:textbox>
								</td>
							</tr>
							<!--=================================== DATI SPEDIZIONE=====================================================-->
							<tr>
								<td class="lstTabRow" colspan="5">Spedizione</td>
							</tr>
							<tr>
							    <td colspan="5">
                                    <Grd:RibesGridView ID="GrdInvio" runat="server" BorderStyle="None" 
                                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                        AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false"
                                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                        OnRowDataBound="GrdRowDataBound" OnRowCommand="GrdRowCommand">
                                        <PagerSettings Position="Bottom"></PagerSettings>
                                        <PagerStyle CssClass="CartListFooter" />
                                        <RowStyle CssClass="CartListItem"></RowStyle>
                                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							            <Columns>
								            <asp:BoundField HeaderText="Tributo" DataField="descrtributo"></asp:BoundField>
								            <asp:TemplateField HeaderText="Nominativo">
									            <ItemTemplate>
								                    <asp:Label ID="Label1" runat="server" Text='<%# FncGrd.FormattaNominativo(DataBinder.Eval(Container, "DataItem.cognomeinvio"), DataBinder.Eval(Container, "DataItem.nomeinvio")) %>'></asp:Label>
									            </ItemTemplate>
								            </asp:TemplateField>
								            <asp:TemplateField HeaderText="Località">
								                <ItemTemplate>
                								    <asp:Label ID="Label2" runat="server" Text='<%# FncGrd.FormattaComune(DataBinder.Eval(Container, "DataItem.caprcp"), DataBinder.Eval(Container, "DataItem.comunercp"), DataBinder.Eval(Container, "DataItem.provinciarcp")) %>'></asp:Label>
								                </ItemTemplate>
								            </asp:TemplateField>
								            <asp:TemplateField HeaderText="Indirizzo">
								                <ItemTemplate>
									                <asp:Label ID="Label3" runat="server" Text='<%# FncGrd.FormattaVia(DataBinder.Eval(Container, "DataItem.viarcp"), DataBinder.Eval(Container, "DataItem.civicorcp"), DataBinder.Eval(Container, "DataItem.posizionecivicorcp"), DataBinder.Eval(Container, "DataItem.esponentecivicorcp"), DataBinder.Eval(Container, "DataItem.scalacivicorcp"), DataBinder.Eval(Container, "DataItem.internocivicorcp"), DataBinder.Eval(Container, "DataItem.frazionercp")) %>'></asp:Label>
								                </ItemTemplate>
								            </asp:TemplateField>
								            <asp:TemplateField HeaderText="">
									            <headerstyle horizontalalign="Center"></headerstyle>
									            <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									            <itemtemplate>
										            <asp:ImageButton id="imgUpd" runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID_DATA_SPEDIZIONE") %>' alt=""></asp:ImageButton>
									            </itemtemplate>
								            </asp:TemplateField>
								            <asp:TemplateField HeaderText="">
									            <headerstyle horizontalalign="Center"></headerstyle>
									            <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									            <itemtemplate>
										            <asp:ImageButton id="imgDel" runat="server" Cssclass="BottoneGrd BottoneCancellaGrd" CommandName="RowDelete" CommandArgument='<%# Eval("ID_DATA_SPEDIZIONE") %>' alt="" OnClientClick="return confirm('Si vuole eliminare l\'indirizzo per il tributo?')"></asp:ImageButton>
									            </itemtemplate>
								            </asp:TemplateField>
							            </Columns>
						            </Grd:RibesGridView>
							    </td>
							</tr>
							<tr>
							    <td colspan="5">
							    </td>
							</tr>
							<tr>
							    <td colspan="5">
							        <div id="DivIndSped" runat="server" style="display:none">
							            <fieldset>
							                <legend class="Input_Label_title">Indirizzo Spedizione</legend>
					                        <table width="100%">
					                            <tr>
					                                <td colspan="4"></td>
						                            <td align="right">
						                                <asp:HiddenField ID="hdIdSpedizione" runat="server" Value="-1" Visible="false"/>
        											    <div class="tooltip">
                                                            <img style="CURSOR: pointer" onclick="SaveSpedizione('S');" alt="" align="bottom" border="0" src="../images/Bottoni/salva.png" />
                                                            <span class="tooltiptext">Salva indirizzo spedizione</span>&nbsp; 
                                                        </div>
        											    <div class="tooltip">
                                                            <img style="CURSOR: pointer" onclick="SaveSpedizione('R');" alt="" align="bottom" border="0" src="../images/Bottoni/Ribalta.png"/>
                                                            <span class="tooltiptext">Salva e Ribalta indirizzo su altri tributi</span>&nbsp; 
                                                        </div>
        											    <div class="tooltip">
                                                            <img style="CURSOR: pointer" onclick="document.getElementById('CmdUnloadSpedizione').click();" alt="" align="bottom" border="0" src="../images/Bottoni/annulla.png" />
                                                            <span class="tooltiptext">Torna indietro</span>&nbsp; 
                                                        </div>
						                            </td>
					                            </tr>
					                            <tr>
					                                <td class="Input_Label">Tributo</td>
						                            <td colspan="2" class="Input_Label">Cognome&nbsp;<font class="NormalRed">*</font></td>
						                            <td colspan="2" class="Input_Label">Nome</td>
					                            </tr>
					                            <tr>
					                                <td>
					                                    <asp:DropDownList ID="ddlTributo" TabIndex="26" runat="server" CssClass="Input_Text"></asp:DropDownList>
					                                </td>
						                            <td colspan="2">
						                                <asp:textbox id="txtCognomeSpedizione" tabIndex="26" runat="server" CssClass="Input_Text" Width="400px" MaxLength="100" ToolTip="Cognome da Indicare per la spedizione"></asp:textbox>
						                            </td>
						                            <td colspan="2">
						                                <asp:textbox id="txtNomeSpedizione" tabIndex="27" runat="server" CssClass="Input_Text" Width="330px" MaxLength="50" ToolTip="Nome da Indicare per la spedizione"></asp:textbox>
						                            </td>
					                            </tr>
					                            <tr>
						                            <td colspan="2" class="Input_Label">Comune&nbsp;
						                                <div class="tooltip">
															<a id="linkapricomuneresidenza" onclick="
																<%if Session("SOLA_LETTURA") = "1" Then%> 
																	return false;
																<%else%>
																	ApriComuni('RibaltaComuneSpedizione','',document.getElementById('txtCAPSpedizione').value,'','',document.getElementById('txtComuneSpedizione').value,document.getElementById('txtProvinciaSpedizione').value, '<% = Session("StileStradario") %>', '<% = UrlPopcomuni %>');
																<%end if%>" href="javascript: void(0)" class="nascosto">
																<img alt="" class="BottoneSel BottoneLista">
																<span class="tooltiptext">Scelta Comuni</span>
															</a>
														</div>
						                            </td>
						                            <td class="Input_Label">Cap</td>
						                            <td class="Input_Label">Provincia</td>
					                            </tr>
					                            <tr>
						                            <td colspan="2">
						                                <asp:textbox id="txtComuneSpedizione" onblur="if(this.value!='') document.getElementById('linkapricomunespedizione').click();" onfocus="TrackBlur(this);" tabIndex="28" runat="server" CssClass="Input_Text" Width="400px" MaxLength="50" ToolTip="Comune Spedizione" OnChange="Azzera(document.getElementById('txtCAPSpedizione').value,'');Azzera(document.getElementById('hdCodComuneSpedizione').value,'');Azzera(document.getElementById('txtProvinciaSpedizione').value,'');"></asp:textbox>
						                            </td>
						                            <td>
							                            <asp:textbox id="txtCAPSpedizione" onkeyup="disableLetterChar(this);" onfocus="TrackBlur(this);" tabIndex="29" runat="server" onblur="if(this.value!='') document.getElementById('linkapricomunespedizione').click();" CssClass="Input_Text" Width="150px" MaxLength="5" ToolTip="CAP di spedizione"></asp:textbox>
						                            </td>
						                            <td>
						                                <asp:textbox id="txtProvinciaSpedizione" onfocus="TrackBlur(this);" tabIndex="30" runat="server" CssClass="Input_Text" Width="50px" MaxLength="2" ToolTip="Provincia di Spedizione" onblur="if(this.value!='') document.getElementById('linkapricomunespedizione').click();"></asp:textbox>
					                                </td>
					                            </tr>
					                            <tr>
						                            <td class="Input_Label" colspan="3">Via/Piazza/Corso&nbsp; 
							                            <a onclick="ApriStradario('RibaltaStradaSpedizione', document.getElementById('hdCodComStradaSpedizione').value);" href="javascript: void(0)" id="linkStradaSpedizione" style="display:none"><img alt="Scelta Strada" class="BottoneSel BottoneLista" align="absMiddle" border="0" /></a>
						                            </td>
						                            <td colspan="2" class="Input_Label">Frazione</td>
					                            </tr>
					                            <tr>
						                            <td colspan="3">
							                            <asp:textbox id="txtIndirizzoSpedizione" tabIndex="31" runat="server" CssClass="Input_Text" Width="530px" MaxLength="50" ToolTip="Indirizzo di Spedizione"></asp:textbox>
						                            </td>
						                            <td colspan="2">
						                                <asp:textbox id="txtFrazioneSpedizione" tabIndex="32" runat="server" CssClass="Input_Text" Width="350px" MaxLength="20" ToolTip="Frazione di Spedizione"></asp:textbox>
						                            </td>
					                            </tr>
					                            <tr>
						                            <td class="Input_Label">N.Civico</td>
						                            <td class="Input_Label">Posizione Civico</td>
						                            <td class="Input_Label">Esponente Civico</td>
						                            <td class="Input_Label">Scala</td>
						                            <td class="Input_Label">Interno</td>
					                            </tr>
					                            <tr>
						                            <td>
						                                <asp:textbox id="txtNumeroCivicoSpedizione" tabIndex="33" runat="server" CssClass="Input_Text" Width="100px" MaxLength="10" ToolTip="Numero Civico Spedizione"></asp:textbox>
						                            </td>
						                            <td>
						                                <asp:textbox id="txtPosizioneSpedizione" tabIndex="34" runat="server" CssClass="Input_Text" Width="100px" MaxLength="50" ToolTip="Posizione"></asp:textbox>
						                            </td>
						                            <td>
						                                <asp:textbox id="txtEsponenteSpedizione" tabIndex="35" runat="server" CssClass="Input_Text" Width="100px" MaxLength="50" ToolTip="Esponente legato al numero civico di spedizione"></asp:textbox>
						                            </td>
						                            <td>
						                                <asp:textbox id="txtScalaSpedizione" tabIndex="36" runat="server" CssClass="Input_Text" Width="100px" MaxLength="20" ToolTip="Scala"></asp:textbox>
						                            </td>
						                            <td>
						                                <asp:textbox id="txtInternoSpedizione" tabIndex="37" runat="server" CssClass="Input_Text" Width="100px" MaxLength="20" ToolTip="Interno"></asp:textbox>
						                            </td>
					                            </tr>
					                        </table>
							            </fieldset>
							        </div>
							    </td>
							</tr>
							<!-- CONTATTI -->
							<tr>
								<td class="lstTabRow" colspan="5">Contatti</td>
							</tr>
							<tr>
								<td>
									<table cellSpacing="3" cellPadding="0" width="100%" border="0">
										<tr>
											<td width="50%">
												<table cellSpacing="0" cellPadding="0" width="99%" border="0">
													<tr>
														<td colspan="2"><asp:label id="lblInfo" runat="server" CssClass="NormalRed"></asp:label></td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td>
												<div class="DIV_SCROLL">
												    <asp:datagrid id="dgContatti" style="CURSOR: hand" runat="server" Width="100%" OnItemDataBound="dgContatti_OnItemDataBound"
												        onmouseover="this.className='riga_tabella_mouse_over'" onmouseout="this.className='riga_tabella'"
														BorderColor="Gainsboro" BackColor="White" AutoGenerateColumns="False" ShowHeader="False">
							                            <AlternatingItemStyle ForeColor="Black" CssClass="CartListItemAlt"></AlternatingItemStyle>
							                            <PagerStyle CssClass="CartListFooter" />
							                            <ItemStyle ForeColor="Black" CssClass="CartListItem" BackColor="White"></ItemStyle>
														<columns>
															<asp:TemplateColumn Visible="False">
																<itemtemplate>
																	<asp:Label id="lblIDRIFERIMENTO" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IDRIFERIMENTO") %>'>
																	</asp:Label>
																</itemtemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn Visible="False">
																<itemtemplate>
																	<asp:Label id="lblTipoRiferimento" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TipoRiferimento") %>'>
																	</asp:Label>
																</itemtemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn HeaderText="Tipo Riferimento">
																<itemtemplate>
																	<asp:Label id="lblDecrizioneRiferimento" runat="server" Text='<%# DescRiferimento(DataBinder.Eval(Container, "DataItem.TipoRiferimento")) %>'>
																	</asp:Label>
																</itemtemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn>
																<itemtemplate>
																	<asp:Label id="DatiRiferimento" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DatiRiferimento") %>'>
																	</asp:Label>
																</itemtemplate>
															</asp:TemplateColumn>
															<asp:TemplateColumn>
																<headerstyle width="200px"></headerstyle>
																<itemtemplate>
																	<asp:Label id="DataInizioInvio" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DataValiditaInvioMAIL") %>'>
																	</asp:Label>
																</itemtemplate>
															</asp:TemplateColumn>															
														</columns>
													</asp:datagrid>
												</div>
											</td>
										</tr>
									</table>
								</td>
								<td colspan="4">
									<table cellSpacing="1" cellPadding="0" width="100%" border="0">
										<tr>
											<td class="Input_Label">Tipo Riferimento</td>
											<td class="Input_Label">Dati Riferimento</td>
											<td class="Input_Label">Invia Informative Via Mail</td>
											<td class="Input_Label">Data Inizio Invio</td>
											<td></td>
										</tr>
										<tr>
											<td><asp:dropdownlist id="cboTipoContatto" tabIndex="38" runat="server" CssClass="Input_Text" Width="150"></asp:dropdownlist></td>
											<td><asp:textbox id="txtDatiRiferimento" onmouseover="Title();" tabIndex="39" runat="server" CssClass="Input_Text_Normal" MaxLength="50" size="30"></asp:textbox></td>
											<td><asp:checkbox id="chkInvioInformativeViaMail" runat="server" AutoPostBack="false" Checked="false" tabIndex="40"></asp:checkbox></td>			
											<td><asp:textbox id="txtDataInizioInvio" onmouseover="Title();" tabIndex="41" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="10" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:textbox></td>
											<td align="left">
											    <div class="tooltip">
                                                    <img style="CURSOR: pointer; height:24px;width:24px" onclick="salvaContatto()" alt="" src="../images/Bottoni/salva.png" align="bottom" border="0" class="nascosto" />
                                                    <span class="tooltiptext">Salva i dati inseriti relativi al contatto</span>&nbsp; 
                                                </div>
												<div class="tooltip">
                                                    <img style="CURSOR: pointer; height:24px;width:24px" onclick="EliminaContatto()" alt="" src="../images/Bottoni/cestino.png" align="bottom" border="0" class="nascosto" />
                                                    <span class="tooltiptext">Elimina i dati relativi al contatto selezionato</span>
                                                </div>
											</td>
										</tr>
									</table>
								</td>
							</tr>
				            <tr>
					            <td class="lstTabRow" colspan="5">&nbsp;</td>
				            </tr>
				            <!-- note Anagrafica-->
							<tr>
							</tr>
				            <tr>
					            <td class="Input_Label"></td>
					            <td class="Input_Label" colspan="3">Note Anagrafica (Max 1000 caratteri)</td>
					            <td class="Input_Label">Da Ricontrollare</td>
				            </tr>
				            <tr>
							    <td valign="top">
                                    <Grd:RibesGridView ID="GrdTributi" runat="server" BorderStyle="None" 
                                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                        AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false"
                                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                        OnRowCommand="GrdTributiRowCommand">
                                        <PagerSettings Position="Bottom"></PagerSettings>
                                        <PagerStyle CssClass="CartListFooter" />
                                        <RowStyle CssClass="CartListItem"></RowStyle>
                                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							            <Columns>
								            <asp:BoundField HeaderText="Tributi associati" DataField="descrizione"></asp:BoundField>
								            <asp:TemplateField HeaderText="">
									            <headerstyle horizontalalign="Center" Width="40px"></headerstyle>
									            <itemstyle horizontalalign="Center"></itemstyle>
									            <itemtemplate>
										            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("cod_tributo") %>' alt=""></asp:ImageButton>
									            </itemtemplate>
								            </asp:TemplateField>
							            </Columns>
						            </Grd:RibesGridView>
							    </td>
					            <td colspan="3">
					                <asp:textbox id="txtNoteAnagrafica" onkeydown="controlla(this,frmAnagrafica,1000);" onkeyup="controlla(this,frmAnagrafica,1000);" tabIndex="41" runat="server" CssClass="Input_Text" Width="100%" MaxLength="1000" ToolTip="Note angrafica" TextMode="MultiLine" Height="100"></asp:textbox>
					            </td>
					            <td valign="top"><asp:checkbox id="chkDaRicontrollare" tabIndex="40" runat="server" CssClass="Input_Text" ToolTip=""></asp:checkbox></td>
				            </tr>
						</table>
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
			<input id="hdCodComuneResidenzaUtente" type="hidden" name="hdCodComuneResidenzaUtente">
			<!-- hdCodComStradaResidenza e hdCodComStradaSpedizione Servono per quando dobbiamo aprire il popup di selezione delle strade.
				Popolato in ribaltamento comune di residenza da popup comuni-->
			<input id="hdCodComStradaResidenza" type="hidden" name="hdCodComStradaResidenza"> 
			<input id="hdCodComStradaSpedizione" type="hidden" name="hdCodComStradaResidenza">
			<input id="hdCodComuneNascitaUtente" type="hidden" name="hdCodComuneNascitaUtente"> 
			<input id="hdCodComuneSpedizioneUtente" type="hidden" name="hdCodComuneSpedizioneUtente">
			<input id="hdCodContribuenteIntestatario" type="hidden" name="hdCodContribuenteIntestatario">
			<input id="hdCodContribuenteUtente" type="hidden" name="hdCodContribuenteUtente">
			<input id="hdIdBancaIntestatario" type="hidden" name="hdIdBancaIntestatario"> 
			<input id="hdIdBancaUtente" type="hidden" name="hdIdBancaUtente">
			<input id="hdIdTRBancaIntestatario" type="hidden" name="hdIdTRBancaIntestatario">
			<input id="hdIdTRBancaUtente" type="hidden" name="hdIdTRBancaUtente">
			<input id="hdRibaltaCampi" type="hidden" name="hdRibaltaCampi"> 
			<input id="txtNameObject" type="hidden" name="txtNameObject">
			<input id="hdModificataAnaUtente" type="hidden" value="0" name="hdModificataAnaUtente">
			<input id="NewSubActivity" type="hidden" name="NewSubActivity"> 
			<input id="hdNoFinestra" type="hidden" value="0" name="hdNoFinestra">
			<input id="hdCOD_CONTRIBUENTE" type="hidden" value="-1" name="hdCOD_CONTRIBUENTE">
			<input id="hdIDDATAANAGRAFICA" type="hidden" value="-1" name="hdIDDATAANAGRAFICA">
			<input id="hdCOD_TRIBUTO" type="hidden" value="-1" name="hdCOD_TRIBUTO"> 
			<input id="hdIDDATASPEDIZIONE" type="hidden" value="-1" name="hdIDDATASPEDIZIONE">
			<input id="hdIDCONTATTO" type="hidden" value="-1" name="hdIDCONTATTO"> 
			<input id="hdIDRIFERIMENTO" type="hidden" value="-1" name="hdIDRIFERIMENTO">
			<asp:button id="btnEvento" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="btnCodiceFiscaleServer" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="btnDaCodiceFiscaleServer" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="btnAction" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="btnConfermaContatti" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="btnEliminaContatti" style="DISPLAY: none" runat="server"></asp:button>
			<asp:textbox id="CommandAction" style="DISPLAY: none" runat="server"></asp:textbox>
			<asp:button id="btnLock" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="btnDelete" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="btnSalva" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="btnControlloCINSalva" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="btnCancella" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="btnAnnulla" style="DISPLAY: none" runat="server"></asp:button>

			<asp:HiddenField ID="hdIdContribuente" runat="server" Value="-1" Visible="true"/>
			<asp:HiddenField ID="hdIdAnagrafica" runat="server" Value="-1" Visible="true"/>

			<asp:HiddenField id="hdCodComuneNascita" runat="server" Value="" /> 
			<asp:HiddenField id="hdCODRappresentanteLegale" runat="server" value="-1" />
			<asp:HiddenField id="hdCODViaResidenza" runat="server" value="-1" />
			<asp:HiddenField id="hdCodComuneResidenza" runat="server" Value="" />
			<asp:HiddenField id="hdCODViaSpedizione" runat="server" value="-1" />
            <asp:HiddenField id="hdCodComuneSpedizione" runat="server" Value="" />
			<asp:button id="CmdSaveSpedizione" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="CmdUnloadSpedizione" style="DISPLAY: none" runat="server"></asp:button>
			<asp:button id="CmdRibaltaSpedizione" style="DISPLAY: none" runat="server"></asp:button>
			<input id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza">
			<input id="paginacomandi" type="hidden" name="paginacomandi">
			<iframe name="iframenascosto" id="iframenascosto" src="" style="display:none"></iframe>
		</form>
	</body>
</HTML>
