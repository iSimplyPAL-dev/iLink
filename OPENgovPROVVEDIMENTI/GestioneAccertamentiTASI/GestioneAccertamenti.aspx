<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GestioneAccertamenti.aspx.vb" Inherits="Provvedimenti.GestioneAccertamenti1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>GestioneAccertamenti</title>
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
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
        function RibaltaImmobileAccertamento() {
            var myIFrame = document.getElementById('loadGridDichiarato');
            var myContent = myIFrame.contentWindow || myIFrame.contentDocument.defaultView;
            if (myContent.document.getElementById('GrdDichiarato') == null) {
                GestAlert('a', 'warning', '', '', 'Per procedere con il ribaltamento dell\'immobile è necessario ricercare un contribuente e selezionare un anno!!');
            }
            else {
                myContent.RibaltaImmobileAccertamento();
            }
        }
        function EseguiAccertamento() {
            var myIFrame = document.getElementById('loadGridAccertato');
            var myContent = myIFrame.contentWindow || myIFrame.contentDocument.defaultView;
            if (myContent.document.getElementById('GrdAccertato') == null) {
                GestAlert('a', 'warning', '', '', 'Impossibile proseguire con l\'Accertamento della posizione!\n E\' necessario eseguire un inserimento manuale degli Immobili.');
            }
            else {
                if (confirm('Si desidera proseguire con l\'Accertamento della posizione?')) {
                    document.getElementById('attesaElabAccertamento').style.display = '';
                    myContent.document.getElementById('btnAccertamento').click();
                }
            }
            return false;
        }
        function FoundAccDefinitivo() {
            if (confirm('Per contribuente e anno selezionati, è già stato elaborato un Provvedimento di Accertamento definitivo.\nProseguendo con la procedura di accertamento, il sistema provvederà a calcolare un nuovo avviso\nmantenendo comunque l\'avviso definitivo.\nContinuare?')) {
                document.getElementById('btnSearchDichiarazioniEffettivo').click();
            }
            return false;
        }

        function FoundAccNONDefinitivo() {
            if (confirm('Per contribuente e anno selezionati, è già stato elaborato un Provvedimento di Accertamento NON definitivo.\nProseguendo con la procedura di accertamento, il sistema provvederà a ricalcolarlo.\nContinuare?')) {
                document.getElementById('btnSearchDichiarazioniEffettivo').click();
            }
            return false;
        }

        function FoundAccPreAccNONDefinitivi() {
            if (confirm('Per contribuente e anno selezionati, sono già stati elaborati un Provvedimento di Accertamento NON definitivo e un Provvedimanto di Pre Accertamento NON definitivo.\nProseguendo con la procedura di accertamento, il sistema provvederà a ricalcolare l\'ACCERTAMENTO.\nContinuare?')) {
                document.getElementById('btnSearchDichiarazioniEffettivo').click();
            }
            return false;
        }

        function FoundPreAccNONDefinitivo() {
            if (confirm('Per contribuente e anno selezionati, è già stato elaborato un Provvedimento di PRE Accertamento NON definitivo (Atto Potenziale).\nProseguire con la procedura di accertamento?')) {
                document.getElementById('btnSearchDichiarazioniEffettivo').click();
            }
            return false;
        }

        function DeleteContrib() {
            document.getElementById('hdIdContribuente').value = '';
            document.getElementById('txtNominativo').value = '';
            document.getElementById('txtHiddenCodContribuente').value = '-1';
            document.getElementById('txtHiddenIdDataAnagrafica').value = '-1';
        }

        function ApriRicercaAnagrafe(nomeSessione) {
            winWidth = 980
            winHeight = 680
            myleft = (screen.width - winWidth) / 2
            mytop = (screen.height - winHeight) / 2 - 40
            caratteristiche = "toolbar=no,status,left=" + myleft + ",top=" + mytop + ",height =" + winHeight + ",width=" + winWidth + ",resizable"
            Parametri = "sessionName=" + nomeSessione
            WinPopUpRicercaAnagrafica = window.open("../../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?" + Parametri, "", caratteristiche)
            return false;
        }

        function checkDatiSelezionati() {
            if (document.getElementById('hdIdContribuente').value == "") {
                GestAlert('a', 'warning', '', '', 'Selezionare un contribuente');
                return false;
            }
            if (document.getElementById('ddlAnno').value == "-1") {
                GestAlert('a', 'warning', '', '', 'Selezionare l\'anno d\'accertamento!');
                return false;
            }
            return true;
        }


        function cerca() {
            parent.Visualizza.document.getElementById('btnSearchDichiarazioni').click();
            return false;
        }

        function checkDati() {
            if (checkDatiSelezionati() == true) {
                parent.Comandi.btnAccertamento.style.display = 'none'
                parent.Visualizza.document.getElementById('btnSearchDichiarazioni').click();
                return false;
            }
        }

        function msg() {
            GestAlert('a', 'warning', '', '', 'Non implementato')
            return false;
        }
    </script>
</head>
<body class="SfondoVisualizza" topmargin="5" onload="document.formRicercaAnagrafica.focus();" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" runat="server" method="post">
        <fieldset class="classeFiledSetIframe100">
            <legend class="Legend">Modalità di Gestione Accertamenti</legend>
            <table id="tablebb" cellpadding="0" width="100%" border="0">
                <!--blocco dati contribuente-->
                <%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
                <tr id="TRPlainAnag" style="width:100%">
                    <td colspan="3">
                        <iframe id="ifrmAnag" runat="server" src="../../aspVuota.aspx" style="height: 200px; width: 100%" frameborder="0" marginheight="0"></iframe>
                        <asp:HiddenField ID="hdIdContribuente" runat="server" Value="-1" />
                    </td>
                </tr>
                <tr id="TRSpecAnag">
                    <td colspan="3">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblNominativo" runat="server" class="Input_Label">Nominativo</asp:Label>&nbsp;
							            <asp:Button ID="btnFocus" runat="server" Height="1px" Width="1px"></asp:Button>
                                    <asp:ImageButton ID="Imagebutton" runat="server" class="BottoneSel BottoneLista" CausesValidation="False"></asp:ImageButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtNominativo" TabIndex="4" runat="server" Width="492px" Enabled="False" CssClass="Input_Text" ToolTip="Nominativo"></asp:TextBox>
                                    <img id="imageDelete" onmouseover="this.style.cursor='hand'" onclick="DeleteContrib();" alt="Pulisci Nominativo Selezionato" src="../../images/cancel.png" width="10px" height="10px">
                                    <asp:Button ID="btnRibalta" Style="display: none" runat="server" Height="2px" Width="1px" Text="Ribalta"></asp:Button>
                                    <asp:TextBox ID="txtHiddenIdDataAnagrafica" Style="display: none" runat="server" Height="20px" Width="24px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label CssClass="Input_Label" ID="lblAnnoAcc" runat="server">Anno Accertamento</asp:Label>
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlAnno" runat="server" Width="112px" AutoPostBack="True" CssClass="Input_Text"></asp:DropDownList>
                    </td>
                    <td>
                        <div class="col-md-5 Input_Label_Italic"><p><b>Attenzione!</b> Saranno calcolati i soli immobili presenti in banca dati IMU</p></div>&nbsp;&nbsp;
                        <asp:CheckBox ID="chkspese" runat="server" CssClass="Input_CheckBox_NoBorder" Text="Non calcolare spese di spedizione" AutoPostBack="False"></asp:CheckBox>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset class="classeFiledSetIframe100 h230">
            <legend class="Legend">Dichiarato</legend>
            <iframe id="loadGridDichiarato" style="display: none" src="../../aspVuota.aspx" frameborder="0" width="100%" height="230px"></iframe>
        </fieldset>
        <fieldset class="classeFiledSetIframe100 h250">
            <legend class="Legend">Accertato</legend>
            <iframe id="loadGridAccertato" style="display: none" src="../../aspVuota.aspx" frameborder="0" width="100%" height="230px"></iframe>
        </fieldset>
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
        <asp:Button ID="btnSearchDichiarazioni" Style="display: none" runat="server" CausesValidation="False"></asp:Button>
        <asp:Button ID="btnSearchDichiarazioniEffettivo" Style="display: none" runat="server" CausesValidation="False"></asp:Button>
        <div id="attesaCarica" style="z-index: 102; position: absolute;display:none;">
            <div class="Legend" style="margin-top:40px;">Ricerca Dichiarato in Corso...</div>
            <div class="BottoneClessidra">&nbsp;</div>
            <div class="Legend">Attendere Prego</div>
        </div>
        <div id="attesaElabAccertamento" style="z-index: 103; position: absolute; display: none;">
            <div class="Legend" style="margin-top:40px;">Elaborazione Accertamento in Corso...</div>
            <div class="BottoneClessidra">&nbsp;</div>
            <div class="Legend">Attendere Prego</div>
        </div>
        <asp:HiddenField id="hfIdUI" runat="server" />
    </form>
</body>
</html>
