<%@ Page Language="c#" CodeBehind="GestioneRavvedimento.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.GestioneRavvedimento" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>GestioneRavvedimento</title>
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript" src="../../_js/ControlloData.js?newversion"></script>
    <script type="text/javascript">
        function ConfiguraRavvOper() {
            /*if (document.getElementById('GrdSanzioni') == null) {
                GestAlert('a', 'warning', '', '', 'Per poter proseguire è necessario indicare Sanzioni ed Interessi!');
            }
            else {*/
                ConfiguraRO();
            /*}*/
            return false;
        }

        function CalcolaRO() {
            if (document.getElementById('GrdRavvedimentoOperoso') == null) {
                GestAlert('a', 'warning', '', '', 'Impossibile proseguire con il calcolo.\n Eseguire prima la configurazione del Ravvedimento Operoso.');
            }
            else {
                CalcolaRO();
            }
            return false;
        }

        function SalvaRavvOper() {
            if (document.getElementById('GrdRavvedimentoOperoso') == null) {
                GestAlert('a', 'warning', '', '', 'Impossibile proseguire con il salvataggio.\n Eseguire prima la configurazione e il calcolo del Ravvedimento Operoso.');
            }
            else {
                Salva();
            }
            return false;
        }

        function StampaExcel() {
            if (document.getElementById('GrdRavvedimentoOperoso') == null) {
                GestAlert('a', 'warning', '', '', 'Impossibile proseguire con la stampa.\n Eseguire prima la configurazione del Ravvedimento Operoso.');
            }
            else {
                document.getElementById('DivAttesa').style.display = '';
                document.getElementById('btnExcel').click();
            }
            return false;
        }
        function ApriRicercaAnagrafeCalcoloIci(nomeSessione) {
            var winWidth = 980;
            var winHeight = 680;
            var myleft = (screen.width - winWidth) / 2;
            var mytop = (screen.height - winHeight) / 2 - 40;

            Parametri = "sessionName=" + nomeSessione;
            WinPopUpRicercaAnagrafica = window.open('../../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?' + Parametri, '', 'width=' + winWidth + ',height=' + winHeight + ', status=yes, toolbar=no,top=' + mytop + ',left=' + myleft + ',Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no');
        }
        function AbilitaImporto(UniqueIDchk, UniqueIDtxtData) {

            //alert("check" + UniqueIDchk);
            //alert("data" + UniqueIDtxtData);			
            if (eval("document.getElementById('" + UniqueIDchk + "').checked") == true) {
                //alert("true");
                (eval("document.getElementById('" + UniqueIDtxtData + "').disabled=false"));
                (eval("document.getElementById('" + UniqueIDtxtData + "').focus()"));
            }
            else {
                //alert("false");
                (eval("document.getElementById('" + UniqueIDtxtData + "').disabled=true"));
                (eval("document.getElementById('" + UniqueIDtxtData + "').value=''"));
            }
        }

        function ConfiguraRO() {
            document.getElementById('btnConfiguraRO').click();
        }

        function CalcolaRO() {
            /*
            if(!controllaSelezioneRO())
            {
            return false;
            }
					
			if(!controllaCheckBoxDataScadenza())
            {
            return false;
            }
			
			if (!controllaData())
            {
            return false;
            }
			
			if(!controllaCheckBox())
            {	
            return false;
            }			
            */
            document.getElementById('btnCalcolaRO').click();
        }

        function Salva() {

            if (!controllaSelezioneRO()) {
                return false;
            }

            if (!controllaCheckBoxDataScadenza()) {
                return false;
            }

            if (!controllaData()) {
                return false;
            }

            if (!controllaCheckBox()) {
                return false;
            }

            if (confirm('Si vuole proseguire con il salvataggio degli importi del Ravvedimento Operoso?')) {
                document.getElementById('btnSalva').click();
            }
            else {
                return false;
            }

        }

        function IsNumeric(sText) {
            var ValidChars = "0123456789.";
            var IsNumber = true;
            var Char;


            for (i = 0; i < sText.length && IsNumber == true; i++) {
                Char = sText.charAt(i);
                if (ValidChars.indexOf(Char) == -1) {
                    IsNumber = false;
                }
            }
            return IsNumber;

        }

        function ControlliPerSanzInt() {

            if (document.getElementById('hdIdContribuente').value == '') {
                GestAlert('a', 'warning', '', '', 'Selezionare un contribuente!')
                return false;
            }

            if (document.getElementById('txtAnno').value.length != 4) {
                GestAlert('a', 'warning', '', '', 'Anno non formattato correttamente!')
                return false;
            }

            //return true;															
        }

        function controllaSelezioneRO() {
            if (document.getElementById("dgRavvedimentoOperoso__ctl2_chkSelect").checked == false && document.getElementById("dgRavvedimentoOperoso__ctl3_chkSelect").checked == false && document.getElementById("dgRavvedimentoOperoso__ctl4_chkSelect").checked == false) {
                GestAlert('a', 'warning', '', '', 'Selezionare almeno una tipologia di versamento');
                return false;
            }
            return true;
        }

        function controllaCheckBox() {
            if (document.getElementById("dgRavvedimentoOperoso__ctl2_chkSelect").checked == true && document.getElementById("dgRavvedimentoOperoso__ctl4_chkSelect").checked == true) {
                GestAlert('a', 'warning', '', '', 'Si sta cercando di inserire l\'Unica Soluzione abbinata all\'Acconto.')
                return false;
            }

            if (document.getElementById("dgRavvedimentoOperoso__ctl3_chkSelect").checked == true && document.getElementById("dgRavvedimentoOperoso__ctl4_chkSelect").checked == true) {
                GestAlert('a', 'warning', '', '', 'Si sta cercando di inserire l\'Unica Soluzione abbinata al Saldo.')
                return false;
            }

            return true;
        }

        function dateDiff(dataa, datab) {
            //alert("dataa:" + dataa);
            //alert("datab:" + datab);
            var diffMilli = dataa.getTime() - datab.getTime()
            var divisore = 86400000
            return Math.round(diffMilli / divisore)
        }

        function GiorniDiff(txtdataa, txtdatab) {
            /*var aDataA = dataValida(txtdataa);
            var aDataB = dataValida(txtdatab);*/
            var giorni = ''
            if (txtdataa && txtdatab)
                giorni = dateDiff(txtdataa, txtdatab)
            return giorni
        }

        function Left(str, n) {
            if (n <= 0)
                return "";
            else if (n > String(str).length)
                return str;
            else
                return String(str).substring(0, n);
        }
        function Right(str, n) {
            if (n <= 0)
                return "";
            else if (n > String(str).length)
                return str;
            else {
                var iLen = String(str).length;
                return String(str).substring(iLen, iLen - n);
            }
        }

        function TrasformaInDate(mydate) {
            //alert("mydate: " + mydate);

            giorno = mydate.substring(0, 2)

            //alert(giorno);

            mese = mydate.substring(3, 5)

            //alert(mese);

            anno = mydate.substring(6, 10)

            //alert(anno);

            var mydate1 = new Date()

            mydate1.setFullYear(anno, mese - 1, giorno)

            //alert("mydate1: " + mydate1 )

            return mydate1;

        }

        function DatadiOggi() {
            var mydate = new Date()

            giorno = Right("0" + mydate.getDate(), 2);
            //alert(giorno);						
            mese = Right("0" + mydate.getMonth(), 2);
            //alert(mese);
            anno = mydate.getYear();
            //alert(anno);

            var today = new Date()

            today.setFullYear(anno, mese, giorno)

            //alert("today: " + today )	

            return today;
        }


        function controllaData() {
            var datadioggi = new Date()

            datadioggi = DatadiOggi();

            if (document.getElementById("dgRavvedimentoOperoso__ctl2_chkSelect").checked == true) {
                //alert("dgRavvedimentoOperoso__ctl2_chkSelect " + document.getElementById("dgRavvedimentoOperoso__ctl2_chkSelect").checked);
                var data = new Date()
                data = TrasformaInDate(document.getElementById("dgRavvedimentoOperoso:_ctl2:txtDataScadenza").value);

                giorni = GiorniDiff(data, datadioggi);
                //alert(giorni);

                if (giorni > 0) {
                    aGestAlert('a', 'warning', '', '', 'La data di scadenza per il versamento in ACCONTO è maggiore di oggi!');
                    return false;
                }
            }

            if (document.getElementById("dgRavvedimentoOperoso__ctl3_chkSelect").checked == true) {
                //alert("dgRavvedimentoOperoso__ctl3_chkSelect " + document.getElementById("dgRavvedimentoOperoso__ctl3_chkSelect").checked);
                var data = new Date()
                data = TrasformaInDate(document.getElementById("dgRavvedimentoOperoso:_ctl3:txtDataScadenza").value);

                giorni = GiorniDiff(data, datadioggi);
                //alert(giorni);
                if (giorni > 0) {
                    GestAlert('a', 'warning', '', '', 'La data di scadenza per il versamento in SALDO è maggiore di oggi!');
                    return false;
                }
            }

            if (document.getElementById("dgRavvedimentoOperoso__ctl4_chkSelect").checked == true) {
                //alert("dgRavvedimentoOperoso__ctl4_chkSelect " + document.getElementById("dgRavvedimentoOperoso__ctl4_chkSelect").checked);
                var data = new Date()
                data = TrasformaInDate(document.getElementById("dgRavvedimentoOperoso:_ctl4:txtDataScadenza").value);

                giorni = GiorniDiff(data, datadioggi);
                //alert(giorni);
                if (giorni > 0) {
                    GestAlert('a', 'warning', '', '', 'La data di scadenza per il versamento in UNICA SOLUZIONE è maggiore di oggi!');
                    return false;
                }
            }

            return true;


        }

        function controllaCheckBoxDataScadenza() {
            if (document.getElementById("dgRavvedimentoOperoso__ctl2_chkSelect").checked == true && document.getElementById("dgRavvedimentoOperoso:_ctl2:txtDataScadenza").value == '') {
                GestAlert('a', 'warning', '', '', 'Inserire la data di scadenza per il versamento in ACCONTO!');
                return false;
            }

            if (document.getElementById("dgRavvedimentoOperoso__ctl2_chkSelect").checked == true && document.getElementById("dgRavvedimentoOperoso:_ctl2:txtTotaleNonVersato").value == '') {
                GestAlert('a', 'warning', '', '', 'Inserire l\'importo non versato per il versamento in ACCONTO!');
                return false;
            }

            if (document.getElementById("dgRavvedimentoOperoso__ctl3_chkSelect").checked == true && document.getElementById("dgRavvedimentoOperoso:_ctl3:txtDataScadenza").value == '') {
                GestAlert('a', 'warning', '', '', 'Inserire la data di scadenza per il versamento in SALDO!');
                return false;
            }

            if (document.getElementById("dgRavvedimentoOperoso__ctl3_chkSelect").checked == true && document.getElementById("dgRavvedimentoOperoso:_ctl3:txtTotaleNonVersato").value == '') {
                GestAlert('a', 'warning', '', '', 'Inserire l\'importo non versato per il versamento in SALDO!');
                return false;
            }

            if (document.getElementById("dgRavvedimentoOperoso__ctl4_chkSelect").checked == true && document.getElementById("dgRavvedimentoOperoso:_ctl4:txtDataScadenza").value == '') {
                GestAlert('a', 'warning', '', '', 'Inserire la data di scadenza per il versamento in UNICA SOLUZIONE!');
                return false;
            }

            if (document.getElementById("dgRavvedimentoOperoso__ctl4_chkSelect").checked == true && document.getElementById("dgRavvedimentoOperoso:_ctl4:txtTotaleNonVersato").value == '') {
                GestAlert('a', 'warning', '', '', 'Inserire l\'importo non versato per il versamento in UNICA SOLUZIONE!');
                return false;
            }

            return true;
        }

        function ApriPopUpStampaDocumenti() {
            document.getElementById('DivAttesa').style.display = 'none';
            $('#divStampa').removeClass('hidden');
            document.getElementById('DivCalcolo').style.display = 'none';
            document.getElementById('loadStampa').src = '../ElaborazioneDocumenti/DownloadDocumenti.aspx?Provenienza=C';
        }
        function Clear() {
            if (confirm('Si vogliono ripulire i dati?')) {
                document.getElementById('btnClear').click();
            }
            else {
                return false;
            }
        }
    </script>

</head>
<body class="Sfondo" scroll="yes" ms_positioning="GridLayout">
    <form id="Form1" runat="server" method="post">
        <div id="DivCalcolo" class="col-md-12">
            <table id="tblContrib" class="col-md-12">
                <tr>
                    <td>
                        <div id="ExpAllDB" Class="Input_Label col-md-12">
                            <p class="NormalBold_title">Per poter calcolare il ravvedimento operoso bisogna:</p>
                            <ul>
                                <li><i class="fa fa-check-square-o" aria-hidden="true"></i>selezionare un contribuente;</li>
                                <li><i class="fa fa-check-square-o" aria-hidden="true"></i>selezionare l’anno e cliccare sul pulsante di ricerca in alto a destra;</li>
                                <li><i class="fa fa-check-square-o" aria-hidden="true"></i>selezionare per quale rata si vuole calcolare il ravvedimento ed indicarne l'importo non pagato;</li>
                                <li><i class="fa fa-check-square-o" aria-hidden="true"></i>cliccare sul pulsante di calcolo in altro a destra;</li>
                            </ul>
                            <p>Cliccare sul pulsante di stampa per produrre l’F24 del ravvedimento.</p>
							<p style="font-style:italic;"><i class="fa fa-exclamation"></i>   Nel caso di ravvedimento au acconto o su saldo vengono considerarti esclusivamente i versamenti fatti in tale modalità (acconto su acconto/saldo su saldo).   <i class="fa fa-exclamation"></i></p>
                        </div>
                        <br>
                    </td>
                </tr>
 			    <tr id="TRPlainAnag">
				    <td>
				        <iframe id="ifrmAnag" runat="server" src="../../aspVuota.aspx" style="height:200px; width:100%" frameborder="0" marginheight="0"></iframe>
				        <asp:HiddenField id="hdIdContribuente" runat="server" Value="-1" />
				    </td>
			    </tr>
			    <tr id="TRSpecAnag">
                    <td class="Input_Label" colspan="5">
                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                            <tr>
                                <td width="160">
                                    <asp:Label ID="lblDatiContribuente" runat="server" CssClass="lstTabRow" Font-Bold="True">Dati Contribuente</asp:Label>&nbsp;
                                    <asp:Button ID="btnRibalta" Style="display: none" runat="server" CausesValidation="False" Text="Button" OnClick="btnRibalta_Click"></asp:Button>
                                    <div class="tooltip">
                                        <asp:ImageButton ID="lnkVerificaContribuente" TabIndex="7" runat="server" CausesValidation="False" CssClass="BottoneSel BottoneLista" alt="" ToolTip="Scelta Contribuente" OnClick="lnkVerificaContribuente_Click"></asp:ImageButton>
                                        <span class="tooltiptext">Ricerca Anagrafica da Tributi</span>
                                    </div>
                                    <asp:TextBox ID="txtTypeOperation" TabIndex="28" runat="server" Text="" Visible="False" ReadOnly="True"></asp:TextBox>
                                    <asp:TextBox ID="txtUpdateAnagraficaValue" Style="display: none" TabIndex="28" runat="server" Text="" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblRiemp" CssClass="lstTabRow" Width="100%" runat="server">&nbsp;</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <table>
                                        <tr>
                                            <td style="width: 210px">
                                                <asp:Label ID="lblCodiceFiscaleContr" runat="server" CssClass="Input_Label">Codice Fiscale</asp:Label><br>
                                                <asp:TextBox ID="txtCodFiscaleContr" runat="server" CssClass="Input_Text" Width="185px" Text="" ReadOnly="True" MaxLength="16" Enabled="False"></asp:TextBox>
                                            </td>
                                            <td style="width: 160px">
                                                <asp:Label ID="lblPartitaIVAContr" runat="server" CssClass="Input_Label">Partita IVA</asp:Label><br>
                                                <asp:TextBox ID="txtPIVAContr" runat="server" CssClass="Input_Text" Width="140px" Text="" ReadOnly="True" MaxLength="11" Enabled="False"></asp:TextBox>
                                            </td>
                                            <td>
                                                &nbsp;<asp:Label ID="lblSessoContr" runat="server" CssClass="Input_Label">Sesso</asp:Label><br>
                                                <asp:RadioButton ID="rdbMaschioContr" runat="server" CssClass="Input_Label" Text="M" Enabled="False"></asp:RadioButton>&nbsp;
                                                <asp:RadioButton ID="rdbFemminaContr" runat="server" CssClass="Input_Label" Text="F" Enabled="False"></asp:RadioButton>&nbsp;
                                                <asp:RadioButton ID="rdbGiuridicaContr" runat="server" CssClass="Input_Label" Text="G" Enabled="False"></asp:RadioButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <table>
                                        <tr>
                                            <td style="width: 250px">
                                                <asp:Label ID="lblCognomeContr" runat="server" CssClass="Input_Label">Cognome/Rag.Soc</asp:Label><br>
                                                <asp:TextBox ID="txtCognomeContr" runat="server" CssClass="Input_Text" Width="230px" Text="" ReadOnly="True" MaxLength="70" Enabled="False"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNomeContr" runat="server" CssClass="Input_Label">Nome</asp:Label><br>
                                                <asp:TextBox ID="txtNomeContr" runat="server" CssClass="Input_Text" Width="230px" Text="" ReadOnly="True" MaxLength="30" Enabled="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <table>
                                        <tr>
                                            <td style="width: 100px">
                                                <asp:Label ID="lblDataNascitaContr" runat="server" CssClass="Input_Label">Data Nascita</asp:Label><br>
                                                <asp:TextBox ID="txtDataNascContr" runat="server" CssClass="Input_Text_Right TextDate" ReadOnly="True" MaxLength="10" Enabled="False"></asp:TextBox>
                                            </td>
                                            <td width="250">
                                                <asp:Label ID="lblComuneNascitaContr" runat="server" CssClass="Input_Label">Comune Nascita</asp:Label><br>
                                                <asp:TextBox ID="txtComNascContr" runat="server" CssClass="Input_Text" Width="230px" Text="" ReadOnly="True" MaxLength="30" Enabled="False"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblProvContr" runat="server" CssClass="Input_Label">Prov.</asp:Label><br>
                                                <asp:TextBox ID="txtProvNascContr" runat="server" CssClass="Input_Text" Width="30px" Text="" ReadOnly="True" MaxLength="2" Enabled="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <table>
                                        <tr>
                                            <td style="width: 250px">
                                                <asp:Label ID="lblViaContr" runat="server" CssClass="Input_Label">Via</asp:Label><br>
                                                <asp:TextBox ID="txtViaResContr" runat="server" CssClass="Input_Text" Width="230px" Text="" ReadOnly="True" MaxLength="60" Enabled="False"></asp:TextBox>
                                            </td>
                                            <td style="width: 85px">
                                                <asp:Label ID="lblNumeroCivContr" runat="server" CssClass="Input_Label">Num. Civico</asp:Label><br>
                                                <asp:TextBox ID="txtNumCivResContr" runat="server" CssClass="Input_Text" Width="65px" ReadOnly="True" Enabled="False"></asp:TextBox>
                                            </td>
                                            <td width="85">
                                                <asp:Label ID="lblEsponenteCivico" runat="server" CssClass="Input_Label">Esp. Civico</asp:Label><br>
                                                <asp:TextBox ID="txtEsponenteCivico" runat="server" CssClass="Input_Text" Width="65px" Text="" ReadOnly="True" MaxLength="5" Enabled="False"></asp:TextBox>
                                            </td>
                                            <td width="85">
                                                <asp:Label ID="lblIntContr" runat="server" CssClass="Input_Label">Interno</asp:Label><br>
                                                <asp:TextBox ID="txtIntResContr" runat="server" CssClass="Input_Text" Width="65px" Text="" ReadOnly="True" MaxLength="5" Enabled="False"></asp:TextBox>
                                            </td>
                                            <td width="85">
                                                <asp:Label ID="lblScalaContr" runat="server" CssClass="Input_Label">Scala</asp:Label><br>
                                                <asp:TextBox ID="txtScalaResContr" runat="server" CssClass="Input_Text" Width="65px" Text="" ReadOnly="True" MaxLength="3" Enabled="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="Input_Label" colspan="5">
                                    <table>
                                        <tr>
                                            <td style="width: 250px">
                                                <asp:Label ID="lblComuneResidenzaContr" runat="server" CssClass="Input_Label">Comune Residenza</asp:Label><br>
                                                <asp:TextBox ID="txtComuneResContr" runat="server" CssClass="Input_Text" Width="230px" Text="" ReadOnly="True" MaxLength="30" Enabled="False"></asp:TextBox>
                                            </td>
                                            <td style="width: 85px">
                                                <asp:Label ID="lblCAPContr" runat="server" CssClass="Input_Label">CAP</asp:Label><br>
                                                <asp:TextBox ID="txtCapResContr" runat="server" CssClass="Input_Text" Width="65px" Text="" ReadOnly="True" MaxLength="5" Enabled="False"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblProvinciaContr" runat="server" CssClass="Input_Label">Prov.</asp:Label><br>
                                                <asp:TextBox ID="txtProvResContr" runat="server" CssClass="Input_Text" Width="30px" Text="" ReadOnly="True" MaxLength="2" Enabled="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="lstTabRow">Selezione Anno</td>
                    <td>
					    <asp:CheckBox ID="chkICI" runat="server" CssClass="Input_Label" Text="ICI/IMU" Checked="true"/>
					    <br />
					    <asp:CheckBox ID="chkTASI" runat="server" CssClass="Input_Label" Text="TASI" Checked="false"/>
					</td>
				</tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtAnno" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="88px" Text="" MaxLength="4"></asp:TextBox>&nbsp;
                        <asp:Label ID="lblInfoSanzioni" CssClass="Input_Label" runat="server">Se non sono configurate le sanzioni per l'anno selezionato, verranno visualizzate le sanzioni dell'ultimo anno valido</asp:Label>
                        <br />
                        <asp:Panel ID="PanelSanzInt" runat="server" class="col-md-12">
                            <table>
                                <tr>
                                    <td class="lstTabRow">Selezione Sanzioni</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMessageSanzioni" runat="server" CssClass="Input_Label" Width="696px"></asp:Label>
                                         <Grd:RibesGridView ID="GrdSanzioni" runat="server" BorderStyle="None" 
                                              BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                              AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                              ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                              <PagerSettings Position="Bottom"></PagerSettings>
                                              <PagerStyle CssClass="CartListFooter" />
                                              <RowStyle CssClass="CartListItem"></RowStyle>
                                              <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                               <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sel">
                                                    <HeaderStyle Width="15px"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
	                                                    <asp:HiddenField runat="server" ID="hfCODICEVOCE" Value='<%# Eval("CODICEVOCE") %>' />
                                                        <asp:HiddenField runat="server" ID="hfIDVALOREVOCE" Value='<%# Eval("IDVALOREVOCE") %>' />
                                                        <asp:HiddenField runat="server" ID="hfCODMISURA" Value='<%# Eval("CODMISURA") %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DESCRIZIONE_VOCE_ATTRIBUITA" HeaderText="Sanzione">
                                                    <HeaderStyle Width="250px"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DESCR_MISURA" HeaderText="Misura">
                                                    <HeaderStyle Width="100px"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="VALOREVOCE" HeaderText="Valore">
                                                    <HeaderStyle Width="70px"></HeaderStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ANNOVALOREVOCE" HeaderText="Anno">
                                                    <HeaderStyle Width="80px"></HeaderStyle>
                                                </asp:BoundField>
                                            </Columns>
                                        </Grd:RibesGridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lstTabRow">Seleziona Interessi</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlInteressi" CssClass="Input_Text" Width="440px" runat="server"></asp:DropDownList>
                                        <asp:Label ID="lblMessageInteressi" runat="server" CssClass="Input_Label" Width="696px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="PanelRO" runat="server" class="col-md-12">
                            <table class="col-md-12">
                                <tr>
                                    <td class="lstTabRow">Seleziona le rate per le quali calcolare il ravvedimento operoso</td>
                                </tr>
                                <tr>
                                    <td>
                                        <Grd:RibesGridView ID="GrdRavvedimentoOperoso" runat="server" BorderStyle="None" 
                                              BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                              AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                              ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                              OnRowDataBound="GrdRowDataBound">
                                              <PagerSettings Position="Bottom"></PagerSettings>
                                              <PagerStyle CssClass="CartListFooter" />
                                              <RowStyle CssClass="CartListItem"></RowStyle>
                                              <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                               <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sel">
                                                    <HeaderStyle Width="15px"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="Textbox2" runat="server"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="TIPOLOGIA_VERSAMENTO" HeaderText="Tipo Versamento">
                                                    <HeaderStyle HorizontalAlign="Left" Width="120px"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Scadenza">
                                                    <HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDataScadenza" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate" ReadOnly="true" Text='<%# DataBinder.Eval(Container, "DataItem.DATA_SCADENZA") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="GG Ritardo">
                                                    <HeaderStyle HorizontalAlign="Center" Width="90px"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGGritardo" runat="server" Width="85px" CssClass="Input_Text_Right OnlyNumber disabled" Text='<%# DataBinder.Eval(Container, "DataItem.GIORNI_RITARDO") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Importo pagato €">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTotaleVersato" runat="server" Width="85px" CssClass="Input_Text_Right OnlyNumber" Text='<%# DataBinder.Eval(Container, "DataItem.IMPORTO_PAGATO") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Importo non pagato €">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTotaleNonVersato" runat="server" Width="85px" CssClass="Input_Text_Right OnlyNumber" Text='<%# DataBinder.Eval(Container, "DataItem.IMPORTO_NON_VERSATO") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Importo Sanzioni €">
                                                    <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTotaleSanzioni" runat="server" Width="85px" CssClass="Input_Text_Right OnlyNumber disabled" Text='<%# DataBinder.Eval(Container, "DataItem.IMPORTO_SANZIONI") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Importo Interessi €">
                                                    <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtTotaleInteressi" runat="server" Width="85px" CssClass="Input_Text_Right OnlyNumber disabled" Text='<%# DataBinder.Eval(Container, "DataItem.IMPORTO_INTERESSI") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Importo Totale €">
                                                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate> 
                                                        <asp:TextBox ID="txtTotale" runat="server" Width="85px" CssClass="Input_Text_Right OnlyNumber disabled" Text='<%# DataBinder.Eval(Container, "DataItem.IMPORTO_TOTALE") %>'></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hfNFAB" Value='<%# Eval("NFAB") %>' />
                                                        <asp:HiddenField runat="server" ID="hfAbiPrinc" Value='<%# Eval("AbiPrinc") %>' />
                                                        <asp:HiddenField runat="server" ID="hfTerrAgr" Value='<%# Eval("TerrAgr") %>' />
                                                        <asp:HiddenField runat="server" ID="hfTerrAgrStato" Value='<%# Eval("TerrAgrStato") %>' />
                                                        <asp:HiddenField runat="server" ID="hfAltriFab" Value='<%# Eval("AltriFab") %>' />
                                                        <asp:HiddenField runat="server" ID="hfAltriFabStato" Value='<%# Eval("AltriFabStato") %>' />
                                                        <asp:HiddenField runat="server" ID="hfAreeFab" Value='<%# Eval("AreeFab") %>' />
                                                        <asp:HiddenField runat="server" ID="hfAreeFabStato" Value='<%# Eval("AreeFabStato") %>' />
                                                        <asp:HiddenField runat="server" ID="hfFabRur" Value='<%# Eval("FabRur") %>' />
                                                        <asp:HiddenField runat="server" ID="hfFabRurStato" Value='<%# Eval("FabRurStato") %>' />
                                                        <asp:HiddenField runat="server" ID="hfUsoProdCatD" Value='<%# Eval("UsoProdCatD") %>' />
                                                        <asp:HiddenField runat="server" ID="hfUsoProdCatDStato" Value='<%# Eval("UsoProdCatDStato") %>' />
                                                        <asp:HiddenField runat="server" ID="hfDetraz" Value='<%# Eval("Detraz") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </Grd:RibesGridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="divF24RO"></div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
        <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
            <div class="BottoneClessidra">&nbsp;</div>
            <div class="Legend">Attendere Prego</div>
        </div>
        <div id="divStampa" class="col-md-12 classeFiledSetRicerca hidden">
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
        <asp:Button ID="btnClear" Style="display: none" runat="server" OnClick="btnClear_Click"></asp:Button>        
        <asp:Button ID="btnSalva" Style="display: none" runat="server" OnClick="btnSalva_Click"></asp:Button>
        <asp:Button ID="btnCalcolaRO" Style="display: none" runat="server" OnClick="btnCalcolaRO_Click"></asp:Button>
        <asp:Button ID="btnConfiguraRO" Style="display: none" runat="server" OnClick="btnConfiguraRO_Click"></asp:Button>
        <asp:Button ID="btnExcel" Style="display: none" runat="server" Width="56px" Text="Excel" Height="24px" OnClick="btnExcel_Click"></asp:Button>
    </form>
</body>
</html>
