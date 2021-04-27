<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InserimentoManualeImmobileTARSU.aspx.vb" Inherits="Provvedimenti.InserimentoManualeImmobileTARSU" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>InserimentoManualeImmobileTARSU</title>
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
        function Controlli() {
            //devo avere il contribuente
            //devo avere l'anno
            if (document.getElementById('TxtAnno').value == '') {
                GestAlert('a', 'warning', '', '', 'E\' necessario inserire l\'Anno!');
                Setfocus(document.getElementById('TxtAnno'));
                return false;
            }
            //devo avere i dati dell'immobile
            if (document.getElementById('TxtVia').value == '') {
                GestAlert('a', 'warning', '', '', 'E\' necessario inserire la Via!');
                return false;
            }
            //devo avere una data di inizio o i bimestri
            if (document.getElementById('TxtDataInizio').value == '' && document.getElementById('TxtTempo').value == '') {
                GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il campo Data di Inizio o i Bimestri!');
                Setfocus(document.getElementById('TxtDataInizio'));
                return false;
            }
            //devo avere la tariffa
            if (document.getElementById('TxtTariffa').value == '') {
                GestAlert('a', 'warning', '', '', 'E\' necessario inserire la Categoria!');
                Setfocus(document.getElementById('TxtTariffa'));
                return false;
            }
            //devo avere i mq
            if (document.getElementById('TxtMQTassabili').value == '') {
                GestAlert('a', 'warning', '', '', 'E\' necessario inserire i Metri!');
                Setfocus(document.getElementById('TxtMQTassabili'));
                return false;
            }
            else {
                sGG = document.getElementById('TxtMQTassabili').value;
                if (!isNumber(sGG)) {
                    GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI nel campo MQ!');
                    Setfocus(document.getElementById('TxtMQTassabili'));
                    return false;
                }
            }
            //se forzo l'importo devo inserire l'importo articolo
            if (document.getElementById('ChkImpForzato').checked == true) {
                if (document.getElementById('TxtImpArticolo').value == '') {
                    GestAlert('a', 'warning', '', '', 'E\' necessario inserire l\'Importo Articolo!');
                    Setfocus(document.getElementById('TxtImpArticolo'));
                    return false;
                }
                else {
                    sGG = document.getElementById('TxtImpArticolo').value;
                    if (!isNumber(sGG)) {
                        GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI nel campo Importo Articolo!');
                        Setfocus(document.getElementById('TxtImpArticolo'));
                        return false;
                    }
                }
            }
            //se ho tarsu giornaliera devo avere i bimestri
        if (document.getElementById('ChkIsGiornaliera').checked == true) {
                if (document.getElementById('TxtTempo').value == '') {
                    GestAlert('a', 'warning', '', '', 'Inserire il numero di giorni per la TARSU giornaliera!');
                    Setfocus(document.getElementById('TxtTempo'));
                    return false;
                }
                else {
                    sGG = document.getElementById('TxtTempo').value;
                    if (!isNumber(sGG, 0, 0)) {
                        GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI INTERI nel campo Giorni TARSU!');
                        Setfocus(document.getElementById('TxtTempo'));
                        return false;
                    }
                }
            }
            else {
                //se ho i bimestri non devono essere maggiori di 6 e interi
                if (document.getElementById('TxtTempo').value != '') {
                    if (document.getElementById('TxtTempo').value > 6) {
                        GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI inferiori a 6 nel campo Bimestri!');
                        Setfocus(document.getElementById('TxtTempo'));
                        return false;
                    }
                    else {
                        sGG = document.getElementById('TxtTempo').value;
                        if (!isNumber(sGG, 0, 0)) {
                            GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI INTERI nel campo Bimestri!');
                            Setfocus(document.getElementById('TxtTempo'));
                            return false;
                        }
                    }
                }
            }
            //il civico deve essere numerico
            if (document.getElementById('TxtCivico').value != '') {
                sGG = document.getElementById('TxtCivico').value;
                if (!isNumber(sGG, 0, 0)) {
                    GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI INTERI nel campo Civico!');
                    Setfocus(document.getElementById('TxtCivico'));
                    return false;
                }
            }
            //il numero componenti deve essere numerico
            if (document.getElementById('TxtNComponenti').value != '') {
                sGG = document.getElementById('TxtNComponenti').value;
                if (!isNumber(sGG, 0, 0)) {
                    GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI INTERI nel campo Componenti!');
                    Setfocus(document.getElementById('TxtNComponenti'));
                    return false;
                }
            }
            //controllo la presenza di stringhe valide
            if (document.getElementById('TxtEsponente').value != '') {
                if (!IsValidChar(document.getElementById('TxtEsponente').value)) {
                    GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Esponente!');
                    Setfocus(document.getElementById('TxtEsponente'));
                    return false;
                }
            }
            if (document.getElementById('TxtInterno').value != '') {
                if (!IsValidChar(document.getElementById('TxtInterno').value)) {
                    GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Interno!');
                    Setfocus(document.getElementById('TxtInterno'));
                    return false;
                }
            }
            if (document.getElementById('TxtScala').value != '') {
                if (!IsValidChar(document.getElementById('TxtScala').value)) {
                    GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Scala!');
                    Setfocus(document.getElementById('TxtScala'));
                    return false;
                }
            }

            if (document.getElementById('TxtFoglio').value != '') {
                if (!IsValidChar(document.getElementById('TxtFoglio').value)) {
                    GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Foglio!');
                    Setfocus(document.getElementById('TxtFoglio'));
                    return false;
                }
            }
            if (document.getElementById('TxtNumero').value != '') {
                if (!IsValidChar(document.getElementById('TxtNumero').value)) {
                    GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Numero!');
                    Setfocus(document.getElementById('TxtNumero'));
                    return false;
                }
            }
            if (document.getElementById('TxtSubalterno').value != '') {
                if (!IsValidChar(document.getElementById('TxtSubalterno').value)) {
                    GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Subalterno!');
                    Setfocus(document.getElementById('TxtSubalterno'));
                    return false;
                }
            }
            //se ho la data inizio controllo che sia valida e coerente con l'anno
            if (document.getElementById('TxtDataInizio').value != '') {
                if (!isDate(document.getElementById('TxtDataInizio').value)) {
                    GestAlert('a', 'warning', '', '', 'Inserire la Data di Inizio correttamente in formato: GG/MM/AAAA!');
                    Setfocus(document.getElementById('TxtDataInizio'));
                    return false;
                }
                else {
                    var sAnno = new String
                    sAnno = document.getElementById('TxtDataInizio').value
                    sAnno = sAnno.substring(6, 10)
                    if (document.getElementById('TxtAnno').value < sAnno) {
                        GestAlert('a', 'warning', '', '', 'La Data di Inizio non e\' coerente con l\'Anno!');
                        Setfocus(document.getElementById('TxtDataInizio'));
                        return false;
                    }
                }
            }
            //se ho la data fine controllo che sia valida e coerente con l'anno
            if (document.getElementById('TxtDataFine').value != '') {
                if (!isDate(document.getElementById('TxtDataFine').value)) {
                    GestAlert('a', 'warning', '', '', 'Inserire la Data di Fine correttamente in formato: GG/MM/AAAA!');
                    Setfocus(document.getElementById('TxtDataFine'));
                    return false;
                }
                else {
                    var sAnno = new String
                    sAnno = document.getElementById('TxtDataFine').value
                    sAnno = sAnno.substring(6, 10)
                    if (document.getElementById('TxtAnno').value > sAnno) {
                        GestAlert('a', 'warning', '', '', 'La Data di Fine non e\' coerente con l\'Anno!');
                        Setfocus(document.getElementById('TxtDataFine'));
                        return false;
                    }
                }
            }
            //se ho la data inizio e fine controllo che siano coerenti
            if (document.getElementById('TxtDataInizio').value != '' && document.getElementById('TxtDataFine').value != '') {
                var starttime = document.getElementById('TxtDataInizio').value
                var endtime = document.getElementById('TxtDataFine').value
                //Start date split to UK date format and add 31 days for maximum datediff
                starttime = starttime.split("/");
                starttime = new Date(starttime[2], starttime[1] - 1, starttime[0]);
                //End date split to UK date format 
                endtime = endtime.split("/");
                endtime = new Date(endtime[2], endtime[1] - 1, endtime[0]);
                if (endtime <= starttime) {
                    GestAlert('a', 'warning', '', '', 'La Data di Fine e\' minore/uguale alla Data di Inizio!');
                    Setfocus(document.getElementById('TxtDataFine);
                    return false;
                }
            }
            return true;
        }

        function ShowInsertRidDet(sTypeShow) {
            if (document.getElementById('TxtAnno').value == '') {
                GestAlert('a', 'warning', '', '', 'E\' necessario inserire l\'Anno!');
                Setfocus(document.getElementById('TxtAnno);
                return false;
            }
            winWidth = 690
            winHeight = 200
            myleft = (screen.width - winWidth) / 2
            mytop = (screen.height - winHeight) / 2 - 40
            var myProv = "S"
            if (document.getElementById('TxtTipoPartita').value == 'PC')
                myProv = "T"
            Parametri = "Provenienza=S&sTypeShow=" + sTypeShow + "&Anno=" + document.getElementById('TxtAnno').value
            WinPopRidDet = window.open("../../OPENgovPROVVEDIMENTI/Dichiarazioni/PopUpInsertRidDet.aspx?" + Parametri, "", "width=" + winWidth + ",height=" + winHeight + ", status=yes, toolbar=no,top=" + mytop + ",left=" + myleft + ",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no")
        }

        function Test() {
            parent.parent.opener.parent.document.getElementById('loadGridAccertato').src = 'SearchAccertatiTARSU.aspx';
        }

        function ApriStradario() {
            var CodEnte = '<% = session("cod_ente") %>';
			    var TipoStrada = '';
			    var Strada = '';
			    var CodStrada = document.getElementById('TxtCodVia').value;
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
			    Parametri += '&Stile=<% = StileStradario %>';
			    Parametri += '&FunzioneRitorno=RibaltaStrada'

                window.open('<% = UrlStradario %>?' + Parametri, 'fStradario', 'top =' + (screen.height - 500) / 2 + ', left=' + (screen.width - 700) / 2 + ' width=700,height=500, status=yes, toolbar=no,scrollbar=no, resizable=no');

			    // l'istruzione seguente ritorna false per non fare il postback della pagina.
                return false;
            }

            function RibaltaStrada(objStrada) {
                // popolo il campo descrizione della via di residenza
                var strada
                if (objStrada.TipoStrada != '&nbsp;') {
                    strada = objStrada.TipoStrada;
                }
                if (objStrada.Strada != '&nbsp;') {
                    strada = strada + ' ' + objStrada.Strada;
                }
                if (objStrada.Frazione != 'CAPOLUOGO') {
                    strada = strada + ' ' + objStrada.Frazione;
                }
                strada = strada.replace(/&#(\d+);/g, function (m, n) { return String.fromCharCode(n); });
                document.getElementById('TxtCodVia').value = objStrada.CodStrada;
                document.getElementById('TxtVia').value = strada;
                document.getElementById('TxtViaRibaltata').value = strada;
            }

            function ClearDatiVia() {
                document.getElementById('TxtVia').value = '';
                document.getElementById('TxtCodVia').value = '';
                return false;
            }

            function PopolaDataFine() {
                document.getElementById('TxtDataFine').value = '31/12/' + document.getElementById('TxtAnno').value;
                document.getElementById('btnGestioneTariffa').click()

            }
    </script>
</head>
<body class="SfondoVisualizza" ms_positioning="GridLayout">
    <form id="Form1" runat="server" method="post">
		<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" class="SfondoGenerale">
			<tr>
				<td style="WIDTH: 464px; HEIGHT: 18px" align="left"><asp:label id="infoEnte" runat="server" Width="400px" CssClass="ContentHead_Title"></asp:label></td>
				<td align="right" width="800" colSpan="2" rowSpan="2">
					<INPUT class="Bottone BottoneAssocia" id="btnAssocia" title="Associa gli immobili al soggetto" onclick="document.getElementById('btnRibalta').click()" type="button" name="btnAssocia"> 
                    <INPUT class="Bottone BottonePulisci hidden hidden" id="Clear" title="Pulisci videata per nuova Associazione" onclick="PulisciCampi()" type="button" name="Clear">
					<INPUT class="Bottone Bottoneannulla" id="Cancel" title="Esci" onclick="window.close()" tabIndex="6" type="button" name="Cancel">
				</td>
			</tr>
			<TR>
				<TD style="WIDTH: 463px" align="left">
					<asp:label id="info" CssClass="NormalBold_title" Width="524px" runat="server" Height="20px" >Accertamenti TARES/TARI - Inserimento Manuale Immobile</asp:label>
				</TD>
			</TR>
		</table>
        <table id="TblGenDich" style="width: 100%" cellspacing="1" cellpadding="1" border="0">
            <tr>
                <td>
                    <asp:Label ID="Label13" CssClass="lstTabRow" runat="server">Tipologia Calcolo</asp:Label><br />
                    <fieldset id="Fieldset1" class="classeFieldSetRicerca" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label16" runat="server" CssClass="Input_Label">Tipo Calcolo</asp:Label>&nbsp;
				                        <asp:Label runat="server" CssClass="Input_Label" ID="LblTipoCalcolo"></asp:Label>
                                </td>
                                <td class="hidden">
                                    <asp:CheckBox ID="ChkConferimenti" runat="server" CssClass="Input_Label" Text="presenza Conferimenti" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkMaggiorazione" runat="server" CssClass="Input_Label" Text="presenza Maggiorazione" />
                                </td>
                                <td>
                                    <asp:Label ID="Label18" runat="server" CssClass="Input_Label">Tipo Superfici</asp:Label>&nbsp;
				                        <asp:Label runat="server" CssClass="Input_Label" ID="LblTipoMQ"></asp:Label>
                                </td>
                                <td class="hidden">
                                    <label class="Input_Label">Data Inizio Conferimenti</label>&nbsp;
                                        <asp:TextBox runat="server" ID="txtInizioConf" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
            <!--Blocco Dati UI-->
            <tr>
                <td>
                    <table id="TblDati" width="100%">
                        <tr>
                            <td colspan="8">
                                <asp:Label ID="Label10" CssClass="lstTabRow" Width="100%" runat="server">Dati Immobile</asp:Label></td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <asp:Label ID="Label3" CssClass="Input_Label" runat="server">Via</asp:Label><asp:Label ID="Label14" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label>&nbsp;
									<asp:ImageButton ID="LnkOpenStradario" runat="server" ToolTip="Ubicazione Immobile da Stradario." CausesValidation="False" ImageAlign="Bottom" ImageUrl="../../images/Bottoni/Listasel.png"></asp:ImageButton>&nbsp;
									<asp:ImageButton ID="LnkPulisciStrada" runat="server" ToolTip="Pulisci i campi della Via" CausesValidation="False" ImageAlign="Bottom" ImageUrl="../../images/Bottoni/cancel.png"></asp:ImageButton>
                                <br />
                                <asp:TextBox ID="TxtVia" CssClass="Input_Text" Width="500px" runat="server" ReadOnly="True"></asp:TextBox><asp:TextBox ID="TxtCodVia" Style="display: none" CssClass="Input_Text" runat="server"></asp:TextBox>
                                <asp:TextBox ID="TxtViaRibaltata" Style="display: none" CssClass="Input_Text" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" CssClass="Input_Label" runat="server">Civico</asp:Label><br />
                                <asp:TextBox ID="TxtCivico" CssClass="Input_Text" Width="70px" runat="server"></asp:TextBox></td>
                            <td>
                                <asp:Label ID="Label5" CssClass="Input_Label" runat="server">Esponente</asp:Label><br />
                                <asp:TextBox ID="TxtEsponente" CssClass="Input_Text" Width="70px" runat="server"></asp:TextBox></td>
                            <td>
                                <asp:Label ID="Label6" CssClass="Input_Label" runat="server">Interno</asp:Label><br />
                                <asp:TextBox ID="TxtInterno" CssClass="Input_Text" Width="70px" runat="server"></asp:TextBox></td>
                            <td>
                                <asp:Label ID="Label8" CssClass="Input_Label" runat="server">Scala</asp:Label><br />
                                <asp:TextBox ID="TxtScala" CssClass="Input_Text" Width="70px" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label11" CssClass="Input_Label" runat="server">Data Inizio</asp:Label><asp:Label ID="Label17" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label><br />
                                <asp:TextBox ID="TxtDataInizio" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate" runat="server" MaxLength="10"></asp:TextBox></td>
                            <td>
                                <asp:Label ID="Label12" CssClass="Input_Label" runat="server">Data Fine</asp:Label><br />
                                <asp:TextBox ID="TxtDataFine" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate" runat="server" MaxLength="10"></asp:TextBox></td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server">Tempo</asp:Label><br />
                                <asp:TextBox ID="TxtTempo" CssClass="Input_Text_Right" Width="50px" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label9" CssClass="Input_Label" runat="server">TARSU giornaliera</asp:Label><br />
                                <asp:CheckBox ID="ChkIsGiornaliera" CssClass="Input_CheckBox" runat="server" AutoPostBack="True"></asp:CheckBox></td>
                            <td>
                                <asp:Label ID="Label15" CssClass="Input_Label" runat="server">N.GG</asp:Label><br />
                                <asp:TextBox ID="TxtGGTarsu" CssClass="Input_Text_Right" Width="50px" runat="server"
                                    Enabled="False"></asp:TextBox></td>
                            <td>
                                <asp:Label ID="Label32" CssClass="Input_Label" runat="server">MQ Tassabili</asp:Label><br>
                                <asp:TextBox ID="TxtMQTassabili" CssClass="Input_Text_right" Width="80px" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label27" runat="server" CssClass="Input_Label">Componenti PF</asp:Label><br />
                                <asp:TextBox ID="TxtNComponenti" runat="server" CssClass="Input_Text" Style="text-align: right"
                                    MaxLength="2" Width="50px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label34" runat="server" CssClass="Input_Label">Componenti PV</asp:Label><br />
                                <asp:TextBox ID="TxtNComponentiPV" runat="server" CssClass="Input_Text" Style="text-align: right"
                                    MaxLength="2" Width="50px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <!--*** 20130228 - gestione categoria Ateco per TARES ***-->
                            <td colspan="7">
                                <asp:Label CssClass="Input_Label" runat="server" ID="Label35">Cat.Ateco</asp:Label><br>
                                <asp:DropDownList ID="DDlCatAteco" CssClass="Input_Label" runat="server" Width="500px"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label36" CssClass="Input_Label" runat="server">Forza Calcolo PV</asp:Label><br />
                                <asp:CheckBox ID="ChkForzaPV" CssClass="Input_CheckBox" runat="server" AutoPostBack="True"></asp:CheckBox></td>
                            <!--*** ***-->
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" CssClass="Input_Label" runat="server">Anno</asp:Label><br />
                                <asp:TextBox ID="TxtAnno" CssClass="Input_Text_Right OnlyNumber" runat="server" Width="90px" Enabled="False"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label1" CssClass="Input_Label" runat="server">Tariffa €</asp:Label><br />
                                <asp:TextBox ID="TxtTariffa" CssClass="Input_Text_Right" runat="server" Width="90px" Enabled="False"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server">Imp.Articolo €</asp:Label><br />
                                <asp:TextBox ID="TxtImpArticolo" CssClass="Input_Text_Right OnlyNumber" runat="server" Width="90px" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server">Imp.Netto €</asp:Label><br />
                                <asp:TextBox ID="TxtImpNetto" CssClass="Input_Text_Right OnlyNumber" runat="server" Width="90px" Enabled="False" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label CssClass="Input_Label" runat="server"></asp:Label><br />
                                <asp:CheckBox ID="ChkImpForzato" CssClass="Input_Label" runat="server" Text="Forza l'importo" TextAlign="Right"></asp:CheckBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="TblUnitaImmo" cellspacing="0" cellpadding="0" border="0" width="100%">
                        <!--Blocco Dati Catastali-->
                        <tr>
                            <td style="width: 100%">
                                <table id="TblCatastali" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td colspan="6">
                                            <asp:Label ID="Label30" CssClass="lstTabRow" Width="100%" runat="server">Dati Catastali</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label38" CssClass="Input_Label" runat="server">Foglio</asp:Label><br />
                                            <asp:TextBox ID="TxtFoglio" CssClass="Input_Text" Width="90px" runat="server"></asp:TextBox></td>
                                        <td>
                                            <asp:Label ID="Label39" CssClass="Input_Label" runat="server">Numero</asp:Label><br />
                                            <asp:TextBox ID="TxtNumero" CssClass="Input_Text" Width="90px" runat="server"></asp:TextBox></td>
                                        <td>
                                            <asp:Label ID="Label40" CssClass="Input_Label" runat="server">Subalterno</asp:Label><br />
                                            <asp:TextBox ID="TxtSubalterno" CssClass="Input_Text" Width="90px" runat="server"></asp:TextBox></td>
                                        <td>
                                            <asp:Label ID="Label28" runat="server" CssClass="Input_Label">Est.Particella</asp:Label><br />
                                            <asp:TextBox ID="TxtEstParticella" TabIndex="10" runat="server" CssClass="Input_Text" Width="90px">0</asp:TextBox></td>
                                        <td>
                                            <asp:Label ID="Label29" runat="server" CssClass="Input_Label">Tipo Particella</asp:Label><br />
                                            <asp:DropDownList ID="DdlTipoParticella" CssClass="Input_Text" Width="100px" runat="server"></asp:DropDownList></td>
                                        <td>
                                            <asp:Label ID="Label23" runat="server" CssClass="Input_Label">Sezione</asp:Label><br />
                                            <asp:TextBox ID="TxtSezione" runat="server" CssClass="Input_Text" Width="90px"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <!--Blocco Dati Riduzioni/Detassazioni-->
                        <tr>
                            <td>
                                <table id="TblRidEse" cellspacing="0" cellpadding="0" border="0" width="100%">
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td width="45%">
                                                        <asp:Label ID="Label31" CssClass="lstTabRow" runat="server">Dati Riduzioni</asp:Label>
                                                        <asp:ImageButton ID="LnkNewRid" runat="server" ImageUrl="../../images/Bottoni/nuovoinserisci.png"></asp:ImageButton>
                                                        <asp:ImageButton ID="LnkDelRid" runat="server" ImageUrl="../../images/Bottoni/cestino.png"></asp:ImageButton>
                                                        <asp:Label ID="Label7" CssClass="lstTabRow" Width="210px" runat="server">&nbsp;</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="45%">
                                                        <asp:Label ID="LblResultRid" CssClass="Legend" runat="server">Non sono presenti riduzioni</asp:Label>
                                                        <Grd:RibesGridView ID="GrdRiduzioni" runat="server" BorderStyle="None"
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
                                                                <asp:BoundField DataField="SCODICE" HeaderText="Codice"></asp:BoundField>
                                                                <asp:BoundField DataField="SDESCRIZIONE" HeaderText="Descrizione"></asp:BoundField>
                                                                <asp:TemplateField HeaderText="">
                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton runat="server" CssClass="BottoneGrd BottoneCancella" CommandName="RowDelete" CommandArgument='<%# Eval("SCODICE") %>' alt=""></asp:ImageButton>
                                                                        <asp:HiddenField runat="server" ID="hfIDRIFERIMENTO" Value='<%# Eval("IDRIFERIMENTO") %>' />
                                                                        <asp:HiddenField runat="server" ID="hfSTIPOVALORE" Value='<%# Eval("STIPOVALORE") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </Grd:RibesGridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td style="height: 30px" width="45%">
                                                        <asp:Label ID="Label33" CssClass="lstTabRow" runat="server">Dati Esenzioni</asp:Label>
                                                        <asp:ImageButton ID="LnkNewDet" CssClass="nascosto" runat="server" ImageUrl="../../images/Bottoni/nuovoinserisci.png"></asp:ImageButton>
                                                        <asp:ImageButton ID="LnkDelDet" CssClass="nascosto" runat="server" ImageUrl="../../images/Bottoni/cestino.png"></asp:ImageButton>
                                                        <asp:Label CssClass="lstTabRow" Width="188px" runat="server">&nbsp;</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="45%">
                                                        <asp:Label ID="LblResultDet" CssClass="Legend" runat="server">Non sono presenti Esenzioni</asp:Label>
                                                        <Grd:RibesGridView ID="GrdDetassazioni" runat="server" BorderStyle="None"
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
                                                                <asp:BoundField DataField="SCODICE" HeaderText="Codice"></asp:BoundField>
                                                                <asp:BoundField DataField="SDESCRIZIONE" HeaderText="Descrizione"></asp:BoundField>
                                                                <asp:TemplateField HeaderText="">
                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton runat="server" CssClass="BottoneGrd BottoneCancella" CommandName="RowDelete" CommandArgument='<%# Eval("SCODICE") %>' alt=""></asp:ImageButton>
                                                                        <asp:HiddenField runat="server" ID="hfIDRIFERIMENTO" Value='<%# Eval("IDRIFERIMENTO") %>' />
                                                                        <asp:HiddenField runat="server" ID="hfSTIPOVALORE" Value='<%# Eval("STIPOVALORE") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </Grd:RibesGridView>
                                                    </td>
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
        <asp:TextBox ID="TxtId" Style="display: none" runat="server">-1</asp:TextBox>
        <asp:TextBox ID="TxtIdArticolo" Style="display: none" runat="server">-1</asp:TextBox>
        <asp:TextBox ID="TxtTipoPartita" Style="display: none" runat="server">PF</asp:TextBox>
        <asp:TextBox ID="txtIdDettaglioTestata" runat="server" Style="display: none"></asp:TextBox>
        <asp:Button ID="btnRibalta" Style="display: none" runat="server" Text="Avvia Accertamento"></asp:Button><asp:TextBox ID="txtAnnoAccertamento" runat="server" Visible="False"></asp:TextBox><asp:TextBox ID="txtCodContribuente" runat="server" Visible="False" DESIGNTIMEDRAGDROP="17"></asp:TextBox><asp:TextBox ID="txtRiaccerta" runat="server" Visible="False"></asp:TextBox><asp:Button ID="btnRiaccerta" Style="display: none" runat="server" Text="Button"></asp:Button><asp:Button ID="CmdSalvaDati" Style="display: none" runat="server"></asp:Button><asp:TextBox ID="TxtIdTariffa" Style="display: none" runat="server">-1</asp:TextBox>
        <asp:TextBox ID="TxtProgGriglia" Style="display: none" runat="server">-1</asp:TextBox>
        <asp:TextBox ID="TxtIdLegame" Style="display: none" runat="server">-1</asp:TextBox>
        <asp:Button ID="btnGestioneTariffa" Style="display: none" runat="server" Text="btnGestioneTariffa"></asp:Button>
        <asp:Button ID="CmdSalvaRidEse" Style="display: none" runat="server"></asp:Button>
    </form>
</body>
</html>
