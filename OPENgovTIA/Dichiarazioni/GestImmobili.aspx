<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GestImmobili.aspx.vb" Inherits="OPENgovTIA.GestImmobili" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>GestImmobili</title>
    <script src="../../_js/jquery.min.js?newversion"></script>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%        End If%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript">
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

				window.open('<% response.Write(UrlStradario) %>?' + Parametri, 'fStradario', 'top =' + (screen.height - 500) / 2 + ', left=' + (screen.width - 700) / 2 + ' width=700,height=500, status=yes, toolbar=no,scrollbar=no, resizable=no');
			    //window.open('http://opengov.isimply.it/Stradario/PopUpStradario/popupstradario.aspx?CodEnte=003120&TipoStrada=&Strada=&CodStrada=&CodTipoStrada=&Frazione=&CodFrazione=&Stile=StylesOPENgovTRIBUTI.css&FunzioneRitorno=RibaltaStrada', 'fStradario', 'top =' + (screen.height - 550) / 2 + ', left=' + (screen.width - 500) / 2 + ' width=500,height=550, status=yes, toolbar=no,scrollbar=no, resizable=no');
				return false;
            }

            function ApriTerritorio() {
                var Parametri = '';

                Parametri += 'CodEnte=<% = Session("COD_ENTE") %>';
				Parametri += '&Appl=OPENGOVT';
			    Parametri += '&User=<% = Session("username") %>';
			    Parametri += '&DescrEnte=<% =Session("COD_ENTE") %>';
			    Parametri += '&Provenienza=OPENGOV';
			    Parametri += '&Foglio=' + document.getElementById('TxtFoglio').value;
			    Parametri += '&Numero=' + document.getElementById('TxtNumero').value;
			    Parametri += '&Subalterno=' + document.getElementById('TxtSubalterno').value;

			    winWidth = 980
			    winHeight = 680
			    myleft = (screen.width - winWidth) / 2
			    mytop = (screen.height - winHeight) / 2 - 40

			    window.open('<% response.Write(UrlPopTerritorio) %>?' + Parametri, 'Territorio', 'width=' + winWidth + ',height=' + winHeight + ', status=yes, toolbar=no,top=' + mytop + ',left=' + myleft + ',Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no')
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

            function ShowInsertVani(IdContribuente, IdTestata, IdTessera, IdUniqueUI, IdVano, IdDettaglioTestata, AzioneProv, Provenienza, IdList, IdListUI, ParamRitornoICI) {
                Parametri = 'AzioneProv=' + AzioneProv + '&Provenienza=' + Provenienza + '&IdUniqueVano=' + IdVano + '&IdDettaglioTestata=' + IdDettaglioTestata
                Parametri += '&IdContribuente=' + IdContribuente + "&IdTestata=" + IdTestata + '&IdTessera=' + IdTessera + '&IdUniqueUI=' + IdUniqueUI + '&IdList=' + IdList + '&IdListUI=' + IdListUI
                Parametri += '&IdCategoriaTARES=' + document.getElementById('DDlCatTARES').value + '&txtNC=' + document.getElementById('TxtNComponenti').value + '&txtNCPV=' + document.getElementById('TxtNComponentiPV').value
                Parametri += '&ParamRitornoICI=' + ParamRitornoICI
                parent.Comandi.location.href = '../../aspVuotaRemoveComandi.aspx';
                parent.Basso.location.href = '../../aspVuotaRemoveComandi.aspx';
                parent.Nascosto.location.href = '../../aspVuotaRemoveComandi.aspx';
                document.location.href = 'PopUpInsertVani.aspx?' + Parametri
            }

            function ShowInsertVaniAnater(sProvenienza) {
                winWidth = 1000
                winHeight = 800
                myleft = (screen.width - winWidth) / 2
                mytop = (screen.height - winHeight) / 2 - 40
                Parametri = "sProvenienza=" + sProvenienza + "&TxtCodVia=" + document.getElementById('TxtCodVia').value + "&TxtVia=" + document.getElementById('TxtVia').value + "&TxtCivico=" + document.getElementById('TxtCivico').value + "&TxtInterno=" + document.getElementById('TxtInterno').value + "&TxtFoglio=" + document.getElementById('TxtFoglio').value + "&TxtNumero=" + document.getElementById('TxtNumero').value + "&TxtSubalterno=" + document.getElementById('TxtSubalterno').value
                WinPopVani = window.open("../RicercaAnater/FrmRicercaImmobile.aspx?" + Parametri, "", "width=" + winWidth + ",height=" + winHeight + ", status=yes, toolbar=no,top=" + mytop + ",left=" + myleft + ",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no")
            }

            function ShowInsertRidEse(sTypeShow) {
                winWidth = 690
                winHeight = 200
                myleft = (screen.width - winWidth) / 2
                mytop = (screen.height - winHeight) / 2 - 40
                Parametri = "Provenienza=I&sTypeShow=" + sTypeShow
                WinPopRidEse = window.open("./PopUpInsertRidDet.aspx?" + Parametri, "", "width=" + winWidth + ",height=" + winHeight + ", status=yes, toolbar=no,top=" + mytop + ",left=" + myleft + ",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no")
            }

            function CheckDatiUI() {
                if ($('#hdProvenienza').val() != 'AE') {
                    //devo avere i dati dell'immobile
                   //il civico deve essere numerico
                    if (document.getElementById('TxtCivico').value != '') {
                        sGG = document.getElementById('TxtCivico').value;
                        if (!isNumber(sGG, 0, 0)) {
                            GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI INTERI nel campo Civico!');
                            Setfocus(document.getElementById('TxtCivico'));
                            return false;
                        }
                    }
                    //controllo la validita di N.Componenti
                    if (document.getElementById('TxtNComponenti').value != '') {
                        sCodice = document.getElementById('TxtNComponenti').value;
                        if (!isNumber(sCodice, 0, 0)) {
                            GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI INTERI nel campo Numero Componenti PF!');
                            Setfocus(document.getElementById('TxtNComponenti'));
                            return false;
                        }
                    }
                    //*** 20130128 - gestione numero occupanti per TARES ***
                    if (document.getElementById('TxtNComponentiPV').value != '') {
                        sGG = document.getElementById('TxtNComponentiPV').value;
                        if (!isNumber(sGG, 0, 0)) {
                            GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI INTERI nel campo Componenti PV!');
                            Setfocus(document.getElementById('TxtNComponentiPV'));
                            return false;
                        }
                    }
                    //*** ***
                    //*** 20130201 - gestione mq da catasto per TARES ***
                    //controllo la validità dei mq da castato
                    if (document.getElementById('TxtMQCatasto').value != '') {
                        sMq = document.getElementById('TxtMQCatasto').value;
                        if (!isNumber(sMq)) {
                            GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI nel campo Mq da Catasto!');
                            Setfocus(document.getElementById('TxtMQCatasto'));
                            return false;
                        }
                        else {
                            var sCheck = new String
                            sCheck = document.getElementById('TxtMQCatasto').value
                            sCheck = sCheck.length
                            if (sCheck > 8) {
                                GestAlert('a', 'warning', '', '', 'Il campo Mq da Catasto può valere al massimo 99999!');
                                Setfocus(document.getElementById('TxtMQCatasto'));
                                return false;
                            }
                        }
                    }
                    if (document.getElementById('TxtMQTassabili').value != '') {
                        sMq = document.getElementById('TxtMQTassabili').value;
                        if (!isNumber(sMq)) {
                            GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI nel campo Mq Tassabili!');
                            Setfocus(document.getElementById('TxtMQTassabili'));
                            return false;
                        }
                        else {
                            var sCheck = new String
                            sCheck = document.getElementById('TxtMQTassabili').value
                            sCheck = sCheck.length
                            if (sCheck > 8) {
                                GestAlert('a', 'warning', '', '', 'Il campo Mq Tassabili può valere al massimo 99999!');
                                Setfocus(document.getElementById('TxtMQTassabili'));
                                return false;
                            }
                        }
                    }
                    //*** ***
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
                    if (document.getElementById('TxtPropietario').value != '') {
                        if (!IsValidChar(document.getElementById('TxtPropietario').value)) {
                            GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Propietario!');
                            Setfocus(document.getElementById('TxtPropietario'));
                            return false;
                        }
                    }
                    if (document.getElementById('TxtOccupantePrec').value != '') {
                        if (!IsValidChar(document.getElementById('TxtOccupantePrec').value)) {
                            GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Occupante Precedente!');
                            Setfocus(document.getElementById('TxtOccupantePrec'));
                            return false;
                        }
                    }
                    if (document.getElementById('TxtNoteUI').value != '') {
                        if (!IsValidChar(document.getElementById('TxtNoteUI').value)) {
                            GestAlert('a', 'warning', '', '', 'Inserire solo LETTERE o NUMERI nel campo Note!');
                            Setfocus(document.getElementById('TxtNoteUI'));
                            return false;
                        }
                    }
                    //devo avere almeno un vano per l'immobile
                    if (document.getElementById('TxtCountVani').value == '0') {
                        GestAlert('a', 'warning', '', '', 'E\' necessario inserire il Dettaglio dei locali!');
                        return false;
                    }
                    //devo avere una data di inizio

                    if (document.getElementById('TxtDataInizio').value == '') {
                        GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il campo Data di Inizio!');
                        Setfocus(document.getElementById('TxtDataInizio'));
                        return false;
                    }
                    else {

                        if (!isDate(document.getElementById('TxtDataInizio').value)) {
                            GestAlert('a', 'warning', '', '', 'Inserire la Data di Inizio correttamente in formato: GG/MM/AAAA!');
                            Setfocus(document.getElementById('TxtDataInizio'));
                            return false;
                        } else {
                            //verifico che la data inizio immobile sia maggiore della data dichiarazione
                            //altrimenti do un warnig
                            sInizio = document.getElementById('TxtDataInizio').value
                            sDich = document.getElementById('txtDataDichiarazione').value

                            var fromArray = sInizio.split('/');
                            dInizio = new Date(fromArray[2], fromArray[1] - 1, fromArray[0]);

                            var toArray = sDich.split('/');
                            dDich = new Date(toArray[2], toArray[1] - 1, toArray[0]);

                            /*if (((dInizio-dDich)/86400000)<0){
								alert ("Data inizio occupazione minore di data dichiarazione")							
							}*/
                        }
                    }
                    //se ho la data inizio e fine controllo che siano coerenti
                    if (document.getElementById('TxtDataInizio').value != '' && document.getElementById('TxtDataFine').value != '') {
                        var starttime = document.getElementById('TxtDataInizio').value
                        var endtime = document.getElementById('TxtDataFine').value
                        //Start date split to UK date format and add 31 days for maximum datediff
                        starttime = starttime.split('/');
                        starttime = new Date(starttime[2], starttime[1] - 1, starttime[0]);
                        //End date split to UK date format 
                        endtime = endtime.split('/');
                        endtime = new Date(endtime[2], endtime[1] - 1, endtime[0]);
                        if (endtime <= starttime) {
                            GestAlert('a', 'warning', '', '', 'La Data di Fine e\' minore/uguale alla Data di Inizio!');
                            Setfocus(document.getElementById('TxtDataFine'));
                            return false;
                        }
                    }

                    //se ho tarsu giornaliera devo avere i giorni
                    if (document.getElementById('ChkIsGiornaliera').Checked == true) {
                        if (document.getElementById('TxtGGTarsu').value == '') {
                            GestAlert('a', 'warning', '', '', 'Inserire il numero di giorni per la TARSU giornaliera!');
                            Setfocus(document.getElementById('TxtGGTarsu'));
                            return false;
                        }
                        else {
                            sGG = document.getElementById('TxtGGTarsu').value;
                            if (!isNumber(sGG, 0, 0)) {
                                GestAlert('a', 'warning', '', '', 'Inserire solo NUMERI INTERI nel campo Giorni TARSU!');
                                Setfocus(document.getElementById('TxtGGTarsu'));
                                return false;
                            }
                        }
                    }

                    if (document.getElementById('TxtDataFine').value != '')
                    {
                        if (confirm('Chiudendo l\'immobile il sistema in automatico riaprirà la situazione \ncon data inizio uguale a data fine + 1 giorno.\nConfermare?')) {
                            document.getElementById('TxtIsRibalta').value = '1';
                        }
                        else {
                            document.getElementById('TxtIsRibalta').value = '0';
                        }
                    }
                }
                document.getElementById('CmdSalvaDatiUI').click()
            }

            function ClearDatiUI() {
                if (document.getElementById('TxtIdUI').value == '-1' && document.getElementById('TxtIdTessera').value != '-1') {
                    if (confirm('I dati non sono stati salvati.\nUscire senza salvare?')) {
                        document.getElementById('CmdClearDatiUI').click()
                    }
                }
                else {
                    document.getElementById('CmdClearDatiUI').click()
                }
            }

            function DeleteUI() {
                if (confirm('Si desidera eliminare l\'Immobile?')) {
                    document.getElementById('CmdDeleteUI').click()
                }
                return false;
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
<body class="Sfondo" ms_positioning="GridLayout" bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="Form1" runat="server" method="post">
        <div class="SfondoGenerale" style="width: 100%; height: 45px; overflow: hidden; padding: 0;">
            <table cellspacing="0" cellpadding="0" width="100%" align="right" border="0">
                <tr>
                    <td style="width: 464px; height: 20px" align="left">
                        <span class="ContentHead_Title" id="infoEnte" style="width: 400px">
                            <asp:Label ID="lblTitolo" runat="server"></asp:Label>
                        </span>
                    </td>
                    <td align="right" width="800" colspan="2" rowspan="2">
                        <input class="Bottone BottoneGIS" id="GIS" title="Visualizza GIS" onclick="document.getElementById('CmdGIS').click()" type="button" name="GIS">
                        <input style="display: none" class="Bottone BottoneTerritorio" id="Territorio" title="Visualizza situazione Territorio" onclick="ApriTerritorio()" type="button" name="Territorio">
                        <input class="Bottone BottoneDuplica" id="Duplica" title="Duplica immobile." onclick="document.getElementById('CmdDuplicaUI').click()" type="button" name="Duplica">
                        <input class="Bottone BottoneApri" id="Modifica" title="Modifica immobile." onclick="document.getElementById('CmdModUI').click()" type="button" name="Modifica">
                        <input class="Bottone BottoneCancella" id="Delete" title="Elimina immobile." onclick="DeleteUI()" type="button" name="Delete">
                        <input class="Bottone BottoneSalva" id="Salva" title="Salva immobile." onclick="CheckDatiUI()" type="button" name="Salva">
                        <input class="Bottone BottoneAnnulla" id="Cancel" title="Torna alla dichiarazione." onclick="ClearDatiUI()" type="button" name="Cancel">
                    </td>
                </tr>
                <tr>
                    <td style="width: 463px" align="left">
                        <span class="NormalBold_title" id="info" runat="server" style="width: 600px; height: 20px;">Variabile - Dichiarazioni - Gestione Immobili </span>
                    </td>
                </tr>
            </table>
        </div>
        &nbsp;
			<table id="TblGenDich" style="width: 100%" cellspacing="1" cellpadding="1" border="0">
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
                        <table id="TblDatiContribuente" cellspacing="0" cellpadding="0" width="100%" bgcolor="white"
                            border="1">
                            <tr>
                                <td bordercolor="darkblue">
                                    <table id="Table1" cellspacing="1" cellpadding="1" width="100%" border="0">
                                        <tr>
                                            <td class="Input_Label" colspan="4" height="20"><strong>DATI CONTRIBUENTE</strong></td>
                                        </tr>
                                        <tr>
                                            <td class="DettagliContribuente">
                                                <asp:Label ID="lblNominativo" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                            <td class="DettagliContribuente">
                                                <asp:Label ID="lblCFPIVA" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="DettagliContribuente" colspan="2">
                                                <asp:Label ID="lblDatiNascita" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="DettagliContribuente" colspan="2">
                                                <asp:Label ID="lblResidenza" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <!--blocco dati tessera-->
                <tr>
                    <td>
                        <div id="DivTessera" runat="server">
                            <table id="TblDatiTessera" cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="1">
                                <tr>
                                    <td bordercolor="darkblue">
                                        <table id="Table2" cellspacing="1" cellpadding="1" width="100%" border="0">
                                            <tr>
                                                <td class="Input_Label" colspan="4" height="20"><strong>DATI TESSERA</strong></td>
                                            </tr>
                                            <tr>
                                                <td class="DettagliContribuente">
                                                    <asp:Label ID="LblNTessera" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                                <td class="DettagliContribuente">
                                                    <asp:Label ID="LblCodInterno" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                                <td class="DettagliContribuente">
                                                    <asp:Label ID="LblCodUtente" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                                <td class="DettagliContribuente">
                                                    <asp:Label ID="LblDataRilascio" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                                <td class="DettagliContribuente">
                                                    <asp:Label ID="LblDataCessazione" runat="server" CssClass="DettagliContribuente">Label</asp:Label></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
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
									<asp:ImageButton ID="LnkOpenStradario" runat="server" ToolTip="Ubicazione Immobile da Stradario."
                                        CausesValidation="False" ImageAlign="Bottom" ImageUrl="../../images/Bottoni/Listasel.png"></asp:ImageButton><br />
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
                                    <asp:Label ID="Label8" CssClass="Input_Label" runat="server">Scala</asp:Label><br />
                                    <asp:TextBox ID="TxtScala" CssClass="Input_Text" Width="70px" runat="server"></asp:TextBox></td>
                                <td>
                                    <asp:Label ID="Label6" CssClass="Input_Label" runat="server">Interno</asp:Label><br />
                                    <asp:TextBox ID="TxtInterno" CssClass="Input_Text" Width="70px" runat="server"></asp:TextBox></td>
                                <td colspan="2">
                                    <asp:Label ID="Label16" CssClass="Input_Label" runat="server">Stato Occupazione</asp:Label><br />
                                    <asp:DropDownList ID="DdlStatoOccupazione" CssClass="Input_Text" runat="server"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label11" CssClass="Input_Label" runat="server">Data Inizio</asp:Label><asp:Label ID="Label17" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label><br />
                                    <asp:TextBox ID="TxtDataInizio" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate" runat="server" MaxLength="10"></asp:TextBox></td>
                                <td>
                                    <asp:Label ID="Label12" CssClass="Input_Label" runat="server">Data Fine</asp:Label><br />
                                    <asp:TextBox ID="TxtDataFine" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate" runat="server" MaxLength="10"></asp:TextBox></td>
                                <td>
                                    <asp:Label ID="Label9" CssClass="Input_Label" runat="server">TARSU giornaliera</asp:Label><br />
                                    <asp:CheckBox ID="ChkIsGiornaliera" CssClass="Input_CheckBox_NoBorder" runat="server" AutoPostBack="True"></asp:CheckBox></td>
                                <td>
                                    <asp:Label ID="Label15" CssClass="Input_Label" runat="server">N.GG</asp:Label><br />
                                    <asp:TextBox ID="TxtGGTarsu" CssClass="Input_Text_Right" Width="50px" runat="server"
                                        Enabled="False"></asp:TextBox></td>
                                <!--*** 20130201 - gestione mq da catasto per TARES ***-->
                                <td>
                                    <asp:Label ID="Label13" CssClass="Input_Label" runat="server">MQ da Catasto</asp:Label><br>
                                    <asp:TextBox ID="TxtMQCatasto" CssClass="Input_Text_right" Width="80px" runat="server"></asp:TextBox></td>
                                <!--*** ***-->
                                <!--*** 20130325 - gestione mq tassabili per TARES ***-->
                                <td>
                                    <asp:Label ID="Label32" CssClass="Input_Label" runat="server">MQ Tassabili</asp:Label><br>
                                    <asp:TextBox ID="TxtMQTassabili" CssClass="Input_Text_right" Width="80px" runat="server"></asp:TextBox>
                                </td>
                                <!--*** ***-->
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
                                                <!--<asp:label CssClass="lstTabRow" Width="672px" Runat="server">&nbsp;</asp:label>-->
                                                <!--<a class="Link_Label" id="LnkRicDatiCatastaliAnater" href="" Runat="server">&gt;&gt;</a>-->
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
                            <!-- Blocco Dati ICI/IMU -->
                            <tr>
                                <td width="100%">
                                    <div id="divDatiICI">
                                        <br />
                                        <a title="Visualizza Dati ICI/IMU" onclick="nascondi(this,'divICI','Dati ICI/IMU')" href="#" class="lstTabRow" style="width: 100%">Visualizza Dati ICI/IMU</a>
                                        <asp:ImageButton ID="ibNewICI" runat="server" ImageUrl="../../images/Bottoni/nuovoinserisci.png" Height="15px" Width="15px" ToolTip="Inserisci in ICI/IMU" OnClientClick="return confirm('Si vuole inserire in Dichiarazioni ICI/IMU?')"></asp:ImageButton>
                                        <div id="divICI" runat="server" style="width: 100%; display: none">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <Grd:RibesGridView ID="GrdICI" runat="server" BorderStyle="None"
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
                                                                        <asp:Label ID="lblDalGrid" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.datainizio")) %>'>
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Al">
                                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAlGrid" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.datafine")) %>'>
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="categoria" HeaderText="Cat."></asp:BoundField>
                                                                <asp:BoundField DataField="classe" HeaderText="Classe"></asp:BoundField>
                                                                <asp:BoundField DataField="Valoreimmobile" HeaderText="Valore" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                                <asp:BoundField DataField="Consistenza" HeaderText="Cons." DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                                <asp:BoundField DataField="PercPossesso" HeaderText="Pos." DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                                <asp:TemplateField HeaderText="Caratteristica">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblcaratgrd" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.carat") %>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.descrcarat") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="">
                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IdOggetto") %>' alt=""></asp:ImageButton>
                                                                        <asp:HiddenField runat="server" ID="hfIdOggetto" Value='<%# Eval("IdOggetto") %>' />
                                                                        <asp:HiddenField runat="server" ID="hfIdTestata" Value='<%# Eval("IdTestata") %>' />
                                                                    </ItemTemplate>
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
                            <!--Blocco Dati per Categoria-->
                            <tr>
                                <td>
                                    <br />
                                    <div id="divUIVSCat" runat="server" style="width: 100%;">
                                        <table id="TblUIVSCat" cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="Label41" CssClass="lstTabRow" runat="server" Width="100%">Dati per Categoria</asp:Label></td>
                                            </tr>
                                            <tr>
                                                <!--*** 20130228 - gestione categoria Ateco per TARES ***-->
                                                <td>
                                                    <asp:Label CssClass="Input_Label" runat="server" ID="Label35">Dati di Default - Cat.</asp:Label><br />
                                                    <asp:DropDownList ID="DDlCatTARES" CssClass="Input_Label" runat="server"></asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label27" runat="server" CssClass="Input_Label">Componenti PF</asp:Label><br />
                                                    <asp:TextBox ID="TxtNComponenti" runat="server" CssClass="Input_Text_Right" MaxLength="2" Width="50px">0</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label34" runat="server" CssClass="Input_Label">Componenti PV</asp:Label><br />
                                                    <asp:TextBox ID="TxtNComponentiPV" runat="server" CssClass="Input_Text_Right" MaxLength="2" Width="50px">0</asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="LblForzaPV" CssClass="Input_Label" runat="server">Forza Calcolo PV</asp:Label><br />
                                                    <asp:CheckBox ID="ChkForzaPV" CssClass="Input_CheckBox_NoBorder" runat="server" AutoPostBack="True"></asp:CheckBox></td>
                                                <!--*** ***-->
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <br />
                                                    <asp:Label ID="LblResultUIVSCat" CssClass="Legend" runat="server">Non sono presenti dati</asp:Label>
                                                    <Grd:RibesGridView ID="GrdUIVSCat" runat="server" BorderStyle="None"
                                                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                        AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                                        <PagerSettings Position="Bottom"></PagerSettings>
                                                        <PagerStyle CssClass="CartListFooter" />
                                                        <RowStyle CssClass="CartListItem"></RowStyle>
                                                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                                        <Columns>
                                                            <asp:BoundField DataField="SCATEGORIA" HeaderText="Cat. utilizzata ai fini del calcolo"></asp:BoundField>
                                                            <asp:BoundField DataField="NVANI" HeaderText="N.Vani">
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="NMQ" HeaderText="MQ" DataFormatString="{0:0.00}">
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SOPERATORE" HeaderText="MQ Tassabili" DataFormatString="{0:0.00}">
                                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </Grd:RibesGridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <!--Blocco Dati Vani-->
                            <tr>
                                <td>
                                    <table id="TblVani" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label25" CssClass="lstTabRow" runat="server">Dati Vani</asp:Label><asp:Label ID="Label18" Style="font-family: Verdana; color: red; font-size: 11px" runat="server">*</asp:Label>&nbsp;
												<asp:ImageButton ID="LnkNewVani" runat="server" ToolTip="Nuovo Vano" CausesValidation="False" ImageAlign="Bottom" ImageUrl="../../images/Bottoni/nuovoinserisci.png" Height="15px" Width="15px"></asp:ImageButton>&nbsp;
												<asp:ImageButton ID="LnkNewVaniAnater" runat="server" ToolTip="Nuovo Vano da Territorio" CausesValidation="False" ImageAlign="Bottom" ImageUrl="../../images/Bottoni/Listasel.png"></asp:ImageButton>
                                                <asp:Label ID="Label47" CssClass="lstTabRow" Width="555px" runat="server">&nbsp;</asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblResultVani" CssClass="Legend" runat="server">Non sono presenti vani</asp:Label>
                                                <Grd:RibesGridView ID="GrdVani" runat="server" BorderStyle="None"
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
                                                        <asp:BoundField DataField="SCATEGORIA" HeaderText="Cat.TARSU"></asp:BoundField>
                                                        <asp:BoundField DataField="SDESCRCATTARES" HeaderText="Cat."></asp:BoundField>
                                                        <asp:BoundField DataField="NNC" HeaderText="N.Comp."></asp:BoundField>
                                                        <asp:BoundField DataField="NNCPV" HeaderText="N.Comp.PV"></asp:BoundField>
                                                        <asp:BoundField DataField="STIPOVANO" HeaderText="Descrizione"></asp:BoundField>
                                                        <asp:BoundField DataField="NVANI" HeaderText="N.Vani">
                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="NMQ" HeaderText="MQ">
                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Vani Esenti">
                                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="chkEsente" Checked='<%# (DataBinder.Eval(Container, "DataItem.bIsEsente")) %>' Width="50px" Enabled="false"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="">
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IDOGGETTO") %>' alt=""></asp:ImageButton>
                                                                <asp:HiddenField runat="server" ID="hfIDOGGETTO" Value='<%# Eval("IDOGGETTO") %>' />
                                                                <asp:HiddenField runat="server" ID="hfIDDETTAGLIOTESTATA" Value='<%# Eval("IDDETTAGLIOTESTATA") %>' />
                                                                <asp:HiddenField runat="server" ID="hfIDCATEGORIA" Value='<%# Eval("IDCATEGORIA") %>' />
                                                                <asp:HiddenField runat="server" ID="hfIDTIPOVANO" Value='<%# Eval("IDTIPOVANO") %>' />
                                                                <asp:HiddenField runat="server" ID="hfTDATAINSERIMENTO" Value='<%# Eval("TDATAINSERIMENTO") %>' />
                                                                <asp:HiddenField runat="server" ID="hfTDATAVARIAZIONE" Value='<%# Eval("TDATAVARIAZIONE") %>' />
                                                                <asp:HiddenField runat="server" ID="hfTDATACESSAZIONE" Value='<%# Eval("TDATACESSAZIONE") %>' />
                                                                <asp:HiddenField runat="server" ID="hfSOPERATORE" Value='<%# Eval("SOPERATORE") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </Grd:RibesGridView>
                                                <asp:TextBox ID="TxtCountVani" Style="display: none" runat="server">0</asp:TextBox>
                                            </td>
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
                                                            <asp:ImageButton ID="LnkNewRid" runat="server" ImageUrl="../../images/Bottoni/nuovoinserisci.png" Height="15px" Width="15px"></asp:ImageButton>
                                                            <asp:ImageButton ID="LnkDelRid" runat="server" CssClass="hidden" ImageUrl="../../images/Bottoni/cestino.png" Height="15px" Width="15px"></asp:ImageButton>
                                                            <asp:Label ID="Label1" CssClass="lstTabRow" Width="210px" runat="server">&nbsp;</asp:Label>
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
                                                                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneCancellaGrd" CommandName="RowDelete" CommandArgument='<%# Eval("SCODICE") %>' alt=""></asp:ImageButton>
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
                                                            <asp:ImageButton ID="LnkNewDet" runat="server" ImageUrl="../../images/Bottoni/nuovoinserisci.png" Height="15px" Width="15px"></asp:ImageButton>
                                                            <asp:ImageButton ID="LnkDelDet" runat="server" CssClass="hidden" ImageUrl="../../images/Bottoni/cestino.png" Height="15px" Width="15px"></asp:ImageButton>
                                                            <asp:Label ID="Label2" CssClass="lstTabRow" Width="188px" runat="server">&nbsp;</asp:Label>
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
                                                                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneCancellaGrd" CommandName="RowDelete" CommandArgument='<%# Eval("SCODICE") %>' alt=""></asp:ImageButton>
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
                            <tr>
                                <td style="width: 100%">
                                    <br />
                                </td>
                            </tr>
                            <!--Blocco Dati Agenzia Entrate-->
                            <tr>
                                <td style="width: 100%">
                                    <table id="TblAgenziaEntrate" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td colspan="5">
                                                <asp:Label ID="Label19" CssClass="lstTabRow" Width="100%" runat="server">Dati Agenzia Entrate</asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label24" CssClass="Input_Label" runat="server">Titolo Occupazione</asp:Label><br />
                                                <asp:DropDownList ID="DdlTitOccupaz" CssClass="Input_Text" runat="server" Width="350px"></asp:DropDownList></td>
                                            <td>
                                                <asp:Label ID="Label20" CssClass="Input_Label" runat="server">Natura Occupazione</asp:Label><br />
                                                <asp:DropDownList ID="DdlNatOccupaz" CssClass="Input_Text" runat="server" Width="300px"></asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label21" CssClass="Input_Label" runat="server">Destinazione d'Uso</asp:Label><br />
                                                <asp:DropDownList ID="DdlDestUso" CssClass="Input_Text" runat="server" Width="350px"></asp:DropDownList></td>
                                            <td>
                                                <asp:Label ID="Label22" CssClass="Input_Label" runat="server">Tipo Unita'</asp:Label><br />
                                                <asp:DropDownList ID="DdlTipoUnita" CssClass="Input_Text" runat="server"></asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="Label26" CssClass="Input_Label" runat="server">Assenza Dati Catastali</asp:Label><br />
                                                <asp:DropDownList ID="DdlAssenzaDatiCat" CssClass="Input_Text" runat="server" Width="450px"></asp:DropDownList></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%">
                                    <br />
                                </td>
                            </tr>
                            <!--Blocco Dati Opzionali-->
                            <tr>
                                <td style="width: 100%">
                                    <table id="TblOpzionali" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="Label37" CssClass="lstTabRow" Width="100%" runat="server">Dati Opzionali</asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label43" CssClass="Input_Label" runat="server">Proprietario di Riferimento</asp:Label><br />
                                                <asp:TextBox ID="TxtPropietario" CssClass="Input_Text" Width="300px" runat="server"></asp:TextBox></td>
                                            <td>
                                                <asp:Label ID="Label44" CssClass="Input_Label" runat="server">Occupante Precedente</asp:Label><br />
                                                <asp:TextBox ID="TxtOccupantePrec" CssClass="Input_Text" runat="server" Width="300px"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%">
                                    <br />
                                </td>
                            </tr>
                            <!--Blocco Note UI-->
                            <tr>
                                <td style="width: 100%">
                                    <table id="TblNoteUI" cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label7" CssClass="lstTabRow" Width="100%" runat="server">Note Unità Immobiliare</asp:Label><br />
                                                <asp:TextBox ID="TxtNoteUI" CssClass="Input_Text" runat="server" Height="32px" TextMode="MultiLine"
                                                    Width="100%"></asp:TextBox></td>
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
        <asp:HiddenField ID="hdIdTestata" runat="server" Value="-1" />
        <asp:HiddenField ID="hdProvenienza" runat="server" Value="" />
        <asp:TextBox ID="txtDataDichiarazione" Style="display: none" runat="server"></asp:TextBox>
        <asp:TextBox ID="TxtIsRibalta" Style="display: none" runat="server">0</asp:TextBox>
        <asp:TextBox ID="TxtIdUI" Style="display: none" runat="server">-1</asp:TextBox>
        <asp:TextBox ID="TxtIdDettaglioTestata" Style="display: none" runat="server">-1</asp:TextBox>
        <asp:TextBox ID="TxtIdTessera" Style="display: none" runat="server">-1</asp:TextBox>
        <asp:TextBox ID="TxtIdPadre" Style="display: none" runat="server">-1</asp:TextBox>
        <asp:Button ID="CmdSalvaVani" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdSalvaDatiUI" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdDuplicaUI" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdModUI" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdDeleteUI" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdClearDatiUI" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdRibaltaUIAnater" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdGIS" Style="display: none" runat="server"></asp:Button>
    </form>
</body>
</html>

