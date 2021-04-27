<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ResetPassword.aspx.vb" Inherits="OPENgov.ResetPassword" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
	<link href="../Styles.css" type="text/css" rel="stylesheet">
	<%if Session("SOLA_LETTURA")="1" then%>
	<link href="../solalettura.css" type="text/css" rel="stylesheet">
	<%end if%>
	<script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
    <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
    <script src="../_js/jquery-1.10.2.min.js?newversion" type="text/javascript" ></script>
    <script type="text/javascript">
        function ShowHide(oggetto) {
            console.log('quì->'+oggetto);
            if ($('#' + oggetto).is(':hidden')) {
                console.log('nascosto');
                $('#' + oggetto).removeClass('hidden');
            } else {
                console.log('da nascondere');
                $('#' + oggetto).addClass('hidden');
            }
        }
        function CheckPwd() {
            if (document.getElementById('txtLoginName').value == '') {
                GestAlert('a', 'warning', '', '', 'Il campo Username è obbligatorio.');
                return false;
            }
            if (document.getElementById('txtPassword').value != document.getElementById('txtConfirmPassword').value) {
                GestAlert('a', 'warning', '', '', 'La password e la password di conferma non corrispondono.');
            }
            else {
                if (document.getElementById('txtPassword').value == '') {
                    GestAlert('a', 'warning', '', '', 'Il campo Password è obbligatorio.');
                }
                else {
                    if (document.getElementById('txtConfirmPassword').value == '') {
                        GestAlert('a', 'warning', '', '', 'Il campo Conferma password è obbligatorio.');
                    }
                    else {
                        if (ValidatePwd(document.getElementById('txtPassword').value)) {
                            if ($('#chkAccept').is(':checked')) {
                                document.getElementById("CmdReset").click();
                            }
                            else {
                                GestAlert('a', 'warning', '', '', 'Per procedere bisogna accettare l’Informativa sul trattamento dei dati personali!');
                            }
                        }
                        else {
                            GestAlert('a', 'warning', '', '', 'La password impostata non rispetta le regole di sicurezza!');
                        }
                    }
                }
            }
        }
        function ValidatePwd(myPwd) {
            var ValRet = true;
            //a lower case letter must occur at least once
            var lowerCaseLetters = /[a-z]/g;
            if (myPwd.match(lowerCaseLetters)) {
                RuleLowerCase.classList.remove('text-danger'); RuleLowerCase.classList.remove('invalid');
                RuleLowerCase.classList.add('text-success'); RuleLowerCase.classList.add('valid');
            } else {
                ValRet = false;
                RuleLowerCase.classList.remove('text-success'); RuleLowerCase.classList.remove('valid');
                RuleLowerCase.classList.add('text-danger'); RuleLowerCase.classList.add('invalid');
            }
            // an upper case letter must occur at least once
            var upperCaseLetters = /[A-Z]/g;
            if (myPwd.match(upperCaseLetters)) {
                RuleUpperCase.classList.remove('text-danger'); RuleUpperCase.classList.remove('invalid');
                RuleUpperCase.classList.add('text-success'); RuleUpperCase.classList.add('valid');
            } else {
                ValRet = false;
                RuleUpperCase.classList.remove('text-success'); RuleUpperCase.classList.remove('valid');
                RuleUpperCase.classList.add('text-danger'); RuleUpperCase.classList.add('invalid');
            }
            // a special character must occur at least once
            var specialChr = /[@#$%^&+=!._-]/g;
            if (myPwd.match(specialChr)) {
                console.log('ho speciali->' + specialChr);
                RuleSpecialChr.classList.remove('text-danger'); RuleSpecialChr.classList.remove('invalid');
                RuleSpecialChr.classList.add('text-success'); RuleSpecialChr.classList.add('valid');
            } else {
                ValRet = false;
                RuleSpecialChr.classList.remove('text-success'); RuleSpecialChr.classList.remove('valid');
                RuleSpecialChr.classList.add('text-danger'); RuleSpecialChr.classList.add('invalid');
            }
            //a digit must occur at least once
            var numbers = /[0-9]/g;
            if (myPwd.match(numbers)) {
                RuleNumber.classList.remove('text-danger'); RuleNumber.classList.remove('invalid');
                RuleNumber.classList.add('text-success'); RuleNumber.classList.add('valid');
            } else {
                ValRet = false;
                RuleNumber.classList.remove('text-success'); RuleNumber.classList.remove('valid');
                RuleNumber.classList.add('text-danger'); RuleNumber.classList.add('invalid');
            }
            //no whitespace allowed in the entire string
            var whitespace = / /g;
            if (!myPwd.match(whitespace)) {
                RuleWhiteSpace.classList.remove('text-danger'); RuleWhiteSpace.classList.remove('invalid');
                RuleWhiteSpace.classList.add('text-success'); RuleWhiteSpace.classList.add('valid');
            } else {
                ValRet = false;
                RuleWhiteSpace.classList.remove('text-success'); RuleWhiteSpace.classList.remove('valid');
                RuleWhiteSpace.classList.add('text-danger'); RuleWhiteSpace.classList.add('invalid');
            }
            //at least 8 characters
            if (myPwd.length >= 8) {
                RuleLength.classList.remove('text-danger'); RuleLength.classList.remove('invalid');
                RuleLength.classList.add('text-success'); RuleLength.classList.add('valid');
            } else {
                ValRet = false;
                RuleLength.classList.remove('text-success'); RuleLength.classList.remove('valid');
                RuleLength.classList.add('text-danger'); RuleLength.classList.add('invalid');
            }
            return ValRet;
        }
    </script>
</head>
<body class="Sfondo" MS_POSITIONING="GridLayout">
    <form id="form1" runat="server" method="post" class="FormNoComandi">
        <div class="SfondoGenerale SfondoNoComandi" style="width: 100%; height: 45px; overflow: hidden; padding-left:10px; padding-top: 10px;">
            <div class="col-md-10">
                <span class="NormalBold_title" id="infoEnte" runat="server"></span><br />
                <span class="NormalBold_title" id="info" runat="server" style="width: 400px">Cambio Password</span>
            </div>
            <div class="col-md-offset-11">
                <input id="Reset" class="Bottone BottoneSalva" onclick="CheckPwd();" type="button" title="Reimposta" />
                <asp:Button ID="CmdReset" runat="server" CssClass="hidden" />
            </div>
        </div>
        &nbsp;
        <div class="col-md-12 SfondoNoComandi">
            <div class="col-md-10">
                <h4 class="lstTabRow">Immettere la nuova password</h4>
                <div class="col-md-12">
                    <div class="col-md-5">
                        <div class="col-md-12">
                            <asp:Label runat="server" CssClass="col-md-4 Input_Label">Username</asp:Label>
                            <div class="col-md-7">
                                <asp:TextBox runat="server" ID="txtLoginName" CssClass="Input_Text" />
                                <asp:label id="lblMessage" runat="server" CssClass="col-md-12 text-danger" Enabled="false"></asp:label>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <asp:Label runat="server" CssClass="col-md-4 Input_Label">Nuova password</asp:Label>
                            <div class="col-md-7">
                                <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" CssClass="Input_Text" onkeyup="ValidatePwd(this.value);" AutoCompleteType="Disabled" autocomplete="off"/>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <asp:Label runat="server" CssClass="col-md-4 Input_Label">Conferma nuova password</asp:Label>
                            <div class="col-md-7">
                                <asp:TextBox runat="server" ID="txtConfirmPassword" TextMode="Password" CssClass="Input_Text" AutoCompleteType="Disabled" autocomplete="off"/>
                            </div>
                        </div>
                        <div id="RecapNew" class="col-md-12" style="display: inline-block;margin-top: 80px;">
                            <h2 class="Input_Label text-info">ATTENZIONE</h2>
                            <h2 class="Input_Label text-info">Dalla videata è possibile:</h2>
                            <h2 class="Input_Label text-info"><i class="fa fa-chevron-right" aria-hidden="true"></i> fare il reset della password di un account già presente</h2>
                            <h2 class="Input_Label text-info"><i class="fa fa-chevron-right" aria-hidden="true"></i> inserire un nuovo account</h2>
                            <h2 class="Input_Label text-info"><i class="fa fa-chevron-right" aria-hidden="true"></i> inserire un account già presente su un nuovo ente</h2>
                            <h2 class="Input_Label text-info">Per inserire un account (nuovo e su altro ente) bisogna sempre cambiare lo username e selezionare un'ente</h2>
                        </div>
                        </div>
                    </div>
                    <div class="col-md-5 Input_Label_Italic">
                        <div id="lblPasswordRules" class="col-md-12 Input_Label">
                            <h2 class="Input_Label_title">La password deve:</h2>
                                <p id="RuleWhiteSpace" class="text-success valid"> non contenere spazi</p>
                                <p id="RuleLength" class="text-danger invalid"> essere lunga almeno 8 caratteri</p>
                                <p id="RuleNumber" class="text-danger invalid"> avere almeno un numero (0-9)</p>
                                <p id="RuleLowerCase" class="text-danger invalid"> avere almeno una lettera minuscola (a-z)</p>
                                <p id="RuleUpperCase" class="text-danger invalid"> avere almeno una lettera maiuscola (A-Z)</p>
                                <p id="RuleSpecialChr" class="text-danger invalid"> avere almeno un carattere speciale<br />(@ ! . _ - & + = # $ % ^)</p>
                        </div>
                    </div>
                </div>
                <div id="PrivacyPolicy" class="col-md-12">
                    <div class="col-md-2"></div>
                    <div class="col-md-10 Input_Label"><asp:CheckBox ID="chkAccept" runat="server" Text="" />ho letto e accetto l’<b><i><a href="javascript:ShowHide('Policy');">Informativa sul trattamento dei dati personali</a></i></b>.</div>
                </div>
                <div id="Policy" class="col-md-12 Input_Label hidden">
                    <div class="col-md-2"></div>
                    <div class="col-md-10">
<p class="lead_Emphasized">Informativa sul trattamento dei dati personali ex artt. 13-14 Reg.to UE 2016/679</p>
<p class="Input_Text_Bold">Soggetti Interessati: Contribuenti.</p>
<p>Il Comune di ………., nella qualità di Titolare del trattamento dei Suoi dati personali, ai sensi e per gli effetti del Reg.to UE 2016/679 di seguito 'GDPR', con la presente La informa che la citata normativa prevede la tutela degli interessati rispetto al trattamento dei dati personali e che tale trattamento sarà improntato ai principi di correttezza, liceità, trasparenza e di tutela della Sua riservatezza e dei Suoi diritti.</p>
<p>I Suoi dati personali verranno trattati in accordo alle disposizioni legislative della normativa sopra richiamata e degli obblighi di riservatezza ivi previsti.</p>
<p>Finalità e base giuridica del trattamento: in particolare i Suoi dati saranno utilizzati per le seguenti finalità necessarie per l'esecuzione di un interesse pubblico o connesse all'esercizio di pubblici poteri:</p>
<ul style="background-color: #A0A0A0;"><li>Erogazione di servizi in ambito tributario.</li></ul>
<p>Ai fini dell'indicato trattamento, il Titolare potrà venire a conoscenza di categorie particolari di dati personali ed in dettaglio: Dati personali anche di tipo particolare. I trattamenti di dati personali per queste categorie particolari sono effettuati in osservanza dell'art 9 del GDPR. Nell’ambito della gestione del tributo, possono essere richiesti/utilizzati dati di contatto utili per le comunicazioni da parte dell’Ente finalizzate a chiarire le posizioni tributarie ed a evitare inutili contenziosi.</p>
<p>Modalità del trattamento. I suoi dati personali potranno essere trattati nei seguenti modi:</p>
<ul style="background-color: #A0A0A0;"><li>affidamento a terzi di operazioni di elaborazione;</li>
<li>trattamento a mezzo di calcolatori elettronici;</li>
<li>trattamento manuale a mezzo di archivi cartacei.</li></ul>
<p>Ogni trattamento avviene nel rispetto delle modalità di cui agli artt. 6, 32 del GDPR e mediante l'adozione delle adeguate misure di sicurezza previste.</p>
<p>I suoi dati saranno trattati unicamente da personale espressamente autorizzato dal Titolare ed, in particolare, dalle seguenti categorie di addetti:</p>
<ul style="background-color: #A0A0A0;"><li>Impiegato comunale</li></ul>
<p>Comunicazione: I suoi dati potranno essere comunicati a soggetti esterni per una corretta gestione del rapporto ed in particolare alle seguenti categorie di Destinatari tra cui tutti i Responsabili del Trattamento debitamente nominati:</p>
<ul style="background-color: #A0A0A0;"><li>nell'ambito di soggetti pubblici e/o privati per i quali la comunicazione dei dati è obbligatoria o necessaria in adempimento ad obblighi di legge o sia comunque funzionale all'amministrazione del rapporto;</li>
<li>Fornitori di Servizi per Enti, in particolare la Società iSimply srl (Via Palestro 45 – Ivrea –TO- P.IVA ………..) in qualità di gestore dell’applicazione.</li></ul>
<p></p>
<p>Diffusione. I dati potranno essere diffusi mediante:</p>
<ul style="background-color: #A0A0A0;"><li>Pubblicazione secondo gli obblighi della normativa vigente.</li></ul>
<p>Fermo restando il divieto assoluto di diffondere i dati idonei a rivelare lo stato di salute.</p>
<p>Periodo di Conservazione. Le segnaliamo che, nel rispetto dei principi di liceità, limitazione delle finalità e minimizzazione dei dati, ai sensi dell’art. 5 del GDPR, il periodo di conservazione dei Suoi dati personali è:</p>
<ul style="background-color: #A0A0A0;"><li>stabilito per un arco di tempo non superiore al conseguimento delle finalità per le quali sono raccolti e trattati e nel rispetto dei tempi obbligatori prescritti dalla legge.</li></ul>
<p>Titolare: il Titolare del trattamento dei dati, ai sensi della Legge, è Comune di ……..  (recapiti del Comune reperibili sul Sito istituzionale del Comune) nella persona del suo legale rappresentante pro tempore.</p>
<p>Il responsabile della protezione dei dati (DPO) designato dal titolare ai sensi dell'art.37 del GDPR è identificabile accedendo al sito istituzionale del Comune.</p>
<p>Lei ha diritto di ottenere dal responsabile la cancellazione (diritto all'oblio), la limitazione, l'aggiornamento, la rettificazione, la portabilità, l'opposizione al trattamento dei dati personali che La riguardano, nonché in generale può esercitare tutti i diritti previsti dagli artt. 15, 16, 17, 18, 19, 20, 21, 22 del GDPR.</p>
<p class="lead_Emphasized">COOKIE</p>
<p>Informazioni generali, disattivazione e gestione dei cookie</p>
<p>I cookie sono dati che vengono inviati dal sito web e memorizzati dal browser internet nel computer o in altro dispositivo (ad esempio, tablet o cellulare) dell’utente. Potranno essere installati, dal nostro sito internet o dai relativi sottodomini, cookie tecnici e cookie di terze parti.</p>
<p>Ad ogni modo, l’utente potrà gestire, ovvero richiedere la disattivazione generale o la cancellazione dei cookie, modificando le impostazioni del proprio browser internet. Tale disattivazione, però, potrà rallentare o impedire l’accesso ad alcune parti del sito.</p>
<p>Le impostazioni per gestire o disattivare i cookie possono variare a seconda del browser internet utilizzato, pertanto, per avere maggiori informazioni sulle modalità con le quali compiere tali operazioni, suggeriamo all’Utente di consultare il manuale del proprio dispositivo o la funzione “Aiuto” o “Help” del proprio browser internet.</p>
<p>Di seguito si indicano agli Utenti i link che spiegano come gestire o disabilitare i cookie per i browser internet più diffusi:</p>
<ul style="background-color: #A0A0A0;"><li>Internet Explorer: http://windows.microsoft.com/it-IT/internet-explorer/delete-manage-cookies</li>
<li>Google Chrome: https://support.google.com/chrome/answer/95647</li>
<li>Mozilla Firefox: http://support.mozilla.org/it/kb/Gestione%20dei%20cookie</li>
<li>Opera: http://help.opera.com/Windows/10.00/it/cookies.html</li>
<li>Safari: https://support.apple.com/kb/PH19255</li></ul>
<p></p>
<p>Cookie tecnici</p>
<p>L’uso di cookie tecnici, ossia cookie necessari alla trasmissione di comunicazioni su rete di comunicazione elettronica ovvero cookie strettamente necessari al fornitore per erogare il servizio richiesto dal cliente, consente la fruizione sicura ed efficiente del nostro sito.</p>
<p>Potranno essere installati cookie di sessione al fine di consentire l’accesso e la permanenza nell’area riservata del portale come utente autenticato.</p>
<p>I cookie tecnici sono essenziali per il corretto funzionamento del nostro sito internet e sono utilizzati per permettere agli utenti la normale navigazione e la possibilità di usufruire dei servizi avanzati disponibili sul nostro sito web. I cookie tecnici utilizzati si distinguono in cookie di sessione, che vengono memorizzati esclusivamente per la durata della navigazione fino alla chiusura del browser, e cookie persistenti che vengono salvati nella memoria del dispositivo dell’utente fino alla loro scadenza o cancellazione da parte dell’utente medesimo. Il nostro sito utilizza i seguenti cookie tecnici:</p>
<ul style="background-color: #A0A0A0;"><li>Cookie tecnici di navigazione o di sessione, utilizzati per gestire la normale navigazione e l’autenticazione dell’utente;</li>
<li>Cookie tecnici funzionali, utilizzati per memorizzare personalizzazioni scelte;</li>
<li>Cookie tecnici analytics, utilizzati per conoscere il modo in cui gli utenti utilizzano il nostro sito web così da poter valutare e migliorare il funzionamento.</li></ul>
<p></p>
<p>Cookie di profilazione</p>
<p>Possono essere installati da parte del/dei Titolare/i, mediante software di c.d. web analytics, cookie di profilazione, i quali sono utilizzati per predisporre report di analisi dettagliati e in tempo reale relativi ad informazioni su: visitatori di un sito web, motori di ricerca di provenienza, parole chiave utilizzate, lingua di utilizzo, pagine più visitate.</p>
<p>Gli stessi possono raccogliere informazioni e dati quali indirizzo IP, nazionalità, città, data/orario, dispositivo, browser, sistema operativo, risoluzione dello schermo, provenienza di navigazione, pagine visitate e numero di pagine, durata della visita, numero di visite effettuate. </p>
<p></p>
<p>Reg.to UE 2016/679: Artt. 15, 16, 17, 18, 19, 20, 21, 22 - Diritti dell'Interessato</p>
<p>1. L'interessato ha diritto di ottenere la conferma dell'esistenza o meno di dati personali che lo riguardano, anche se non ancora registrati, la loro comunicazione in forma intelligibile e la possibilità di effettuare reclamo presso l’Autorità di controllo.</p>
<p>2. L'interessato ha diritto di ottenere l'indicazione:</p>
<p>a.	dell'origine dei dati personali;</p>
<p>b.	delle finalità e modalità del trattamento;</p>
<p>c.	della logica applicata in caso di trattamento effettuato con l'ausilio di strumenti elettronici;</p>
<p>d.	degli estremi identificativi del titolare, dei responsabili e del rappresentante designato ai sensi dell'articolo 5, comma 2;</p>
<p>e.	dei soggetti o delle categorie di soggetti ai quali i dati personali possono essere comunicati o che possono venirne a conoscenza in qualità di rappresentante designato nel territorio dello Stato, di responsabili o incaricati.</p>
<p>3. L'interessato, per quanto applicabile, ha diritto di ottenere:</p>
<p>a.	l'aggiornamento, la rettificazione ovvero, quando vi ha interesse, l'integrazione dei dati;</p>
<p>b.	la cancellazione, la trasformazione in forma anonima o il blocco dei dati trattati in violazione di legge, compresi quelli di cui non è necessaria la conservazione in relazione agli scopi per i quali i dati sono stati raccolti o successivamente trattati;</p>
<p>c.	l'attestazione che le operazioni di cui alle lettere a) e b) sono state portate a conoscenza, anche per quanto riguarda il loro contenuto, di coloro ai quali i dati sono stati comunicati o diffusi, eccettuato il caso in cui tale adempimento si rivela impossibile o comporta un impiego di mezzi manifestamente sproporzionato rispetto al diritto tutelato;</p>
<p>d.	la portabilità dei dati.</p>
<p>4. L'interessato, per quanto applicabile, ha diritto di opporsi, in tutto o in parte:</p>
<p>a.	per motivi legittimi al trattamento dei dati personali che lo riguardano, ancorché pertinenti allo scopo della raccolta;</p>
<p>b.	al trattamento di dati personali che lo riguardano per il compimento di ricerche di mercato o di comunicazione commerciale.</p>
                        <p><a href="javascript:ShowHide('Policy');" class="Input_Label_bold">Chiudi</a></p>
                    </div>
                </div>
            </div>
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
    </form>
</body>
</html>
