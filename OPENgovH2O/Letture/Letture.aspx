<%@ Page Language="vb" AutoEventWireup="False" CodeBehind="Letture.aspx.vb" Inherits="OpenUtenze.Letture" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Gestione Letture</title>
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <meta content="True" name="vs_showGrid">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%        End If%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    <script type="text/javascript" src="../../_js/MyUtility.js?newversion"></script>
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/vbscript" src="../../_vbs/OperazioniSuCampi.vbs"></script>
    <script type="text/vbscript" src="../../_vbs/ControlliFormali.vbs"></script>
    <script type="text/javascript">
        function Error() {
            GestAlert('a', 'warning', '', '', 'Attenzione: errore durante il salvataggio dei dati del contatore !');
        }

        function SalvaDatiContatore() {
            if (confirm('Si vogliono salvare le modifiche apportate al Contatore selezionato?')) {
                var iselGiro = document.getElementById('cboGiro').selectedIndex;
                var valueGiro = document.getElementById('cboGiro[iselGiro]').value;

                var iselTipoContatore = document.getElementById('cboTipoContatore').selectedIndex;
                var valueTipoContatore = document.getElementById('cboTipoContatore[iselTipoContatore]').value;

                var iselUbicazione = document.getElementById('TxtCodVia').value;
                var valueUbicazione = document.getElementById('TxtVia').value;

                var iselPosizione = document.getElementById('cboPosizione').selectedIndex;
                var valuePosizione = document.getElementById('cboPosizione[iselPosizione]').value

                document.getElementById('hdSalva').value = '1';
                document.getElementById('hdCOD_STRADA').value = document.getElementById('hdCodiceViaLetture').value;
                document.getElementById('hdNumeroCivico').value = document.getElementById('txtNCivico').value;
                document.getElementById('hdIDGiro').value = valueGiro;
                document.getElementById('hdIDPosizione').value = valuePosizione;
                document.getElementById('hdProgressivo').value = document.getElementById('txtProgressivo').value;
                document.getElementById('hdSequenza').value = document.getElementById('txtSequenza').value;
                document.getElementById('hdLatoStrada').value = document.getElementById('txtLatoStrada').value;
                document.getElementById('hdTipoContatore').value = valueTipoContatore
                document.getElementById('hdMATRICOLACONTATORE').value = document.getElementById('txtMatricola').value;
                document.getElementById('hdESPONENTE').value = document.getElementById('txtEsponente').value;
                document.getElementById('hdNOTECONTATORE').value = document.getElementById('txtNoteContatore').value;
                document.getElementById('hdCIFRECONTATORE').value = document.getElementById('txtCifreContatore').value;

                Parametri = "?hdIDContatore=" + document.getElementById('hdIDContatore').value + "&hdSalva=" + document.getElementById('hdSalva').value + "&hdCOD_STRADA=" + document.getElementById('hdCOD_STRADA').value + "&hdNumeroCivico=" + document.getElementById('hdNumeroCivico').value + "&hdIDGiro=" + document.getElementById('hdIDGiro').value + "&hdIDPosizione=" + document.getElementById('hdIDPosizione').value + "&hdProgressivo=" + document.getElementById('hdProgressivo').value + "&hdSequenza=" + document.getElementById('hdSequenza').value + "&hdLatoStrada=" + document.getElementById('hdLatoStrada').value + "&MATRICOLA=" + document.getElementById('hdMATRICOLACONTATORE').value + "&TIPOCONTATORE=" + valueTipoContatore + "&UBICAZIONE=" + iselUbicazione + "&ESPONENTE=" + document.getElementById('hdESPONENTE').value + "&hdNOTECONTATORE=" + document.getElementById('hdNOTECONTATORE').value + "&hdCIFRECONTATORE=" + document.getElementById('hdCIFRECONTATORE').value + "&hdSTRADA=" + valueUbicazione
                
                WinPopUp = OpenPopup('OpenUtenze', '<%=Session("PATH_APPLICAZIONE")%>/Letture/ExecContatori.aspx' + Parametri, 'ExcecContatori', '770', '500', 10000, 10000, 'yes', 'no');
             }
         }

         function MessageNotFound() {
             GestAlert('a', 'warning', '', '', 'La ricerca non ha prodotto risultati !!!');
             return false;
         }

         function NuovoModifica(idPeriodo) {
             if(document.getElementById('PAG_PREC').value == '') {
                 document.getElementById('PAG_PREC').value = 1;
                 document.getElementById('paginacomandi').value = '/OPENutenze/DataEntryLetture/ComandiLetture.aspx';
             }
             if (idPeriodo == 1)
                 window.open('../Letture/FrameInserimentoModifica.aspx?PAG_PREC=' + document.getElementById('PAG_PREC').value + '&IDCONTATORE=' + document.getElementById('txtCodContatore').value + '&IDLettura=0', 'Letture', 'width=800, height=650, toolbar=no,top=0,left=5, menubar=no,status=yes');
			else
			    GestAlert('a', 'warning', '', '', 'Non si possono inserire le letture. E\' necessario impostare un periodo di fatturazione!');
            return false;
        }

        function ModificaLetture(lb, IsFatt) {
            window.open('FrameInserimentoModifica.aspx?PAG_PREC=' + document.getElementById('PAG_PREC').value + '&IDCONTATORE=' + document.getElementById('hdIDContatore').value + '&IDLettura=' + lb + '&IsFatturata=' + IsFatt, 'Letture', 'width=800, height=650, toolbar=no,top=0,left=5, menubar=no,status=yes');
		    return false;
		}

		function GetStradario(objFieldHidden, objFieldUbicazione, CodComune, objForm) {

		    HIDDEN = objFieldHidden.name;
		    FORM = objForm.name;
		    UBICAZIONE = objFieldUbicazione.name;
		    if (document.getElementById('hdSceltaViaLetture').value == '1') {
		        /*WinPopUp=OpenPopup('OpenUtenze','/OpenUtenze/Selezioni/FrameListaSelezioni.aspx?SEARCHVIA=' + objFieldUbicazione').value+'&HIDDEN=' + HIDDEN + '&FORM=' +FORM +'&UBICAZIONE='+ UBICAZIONE +'&CODCOMUNE='+CodComune').value,'Stradario','770','500',0,0,'yes','no');*/
		        WinPopUp = OpenPopup('OpenUtenze', '<%=Session("PATH_APPLICAZIONE")%>/Selezioni/FrameListaSelezioni.aspx?SEARCHVIA=' + UBICAZIONE + '&HIDDEN=' + HIDDEN + '&FORM=' + FORM + '&UBICAZIONE=' + UBICAZIONE + '&CODCOMUNE=' + CodComune, 'Stradario', '770', '500', 0, 0, 'yes', 'no');
        }
        else {
            GestAlert('a', 'warning', '', '', 'Funzione non abilitata per il DE Contatori !!');
        }
    }

    function VerificaData(Data) {
        if (!IsBlank(Data.value)) {
            if (!isDate(Data.value)) {
                alert("Inserire la Data  correttamente in formato: gg/mm/aaaa !");
                Data.value = "";
                Setfocus(Data);
                return false;
            }
        }
    }

    function AttivaAnomalie(oSel) {
        oSel.disabled = false;

        d = new Date();

        s = d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getYear();
        document.getElementById('chkDARicontrollare').checked = false;
        document.getElementById('chkLasciatoAvviso').checked = false;
        document.getElementById('txtDataPassaggio').value = s;
        document.getElementById('chkLasciatoAvviso').disabled = true;
        document.getElementById('chkDARicontrollare').disabled = true;
        document.getElementById('txtIDAnomalia').value = '3';
    }
    function PulisciLista(oSel) {
        var opt, i = 0;
        while (opt = oSel.options[i++]) {
            opt.selected = false;
        }
        oSel.disabled = true;
        document.getElementById('txtDataPassaggio').value = '';
        document.getElementById('chkLasciatoAvviso').disabled = false;
        document.getElementById('chkDARicontrollare').disabled = false;
        document.getElementById('txtIDAnomalia').value = '';
    }

    function limitOptions(oSel, howmany) {

        var opt, i = 0, msg = '', thismany = howmany, toomany = new Array();
        while (opt = oSel.options[i++]) {
            if (opt.selected)--howmany;
            if (howmany < 0) toomany[toomany.length] = opt;
        }
        if (howmany < 0) {
            msg += 'Numero massimo di Anomalie selezionabili  = ' + thismany + '.';
            alert(msg);
            i = 0;
            while (opt = toomany[i++])
                opt.selected = false;
            return false;
        } else { document.getElementById('txtIDAnomalia').value = howmany }
    }


    function AssegnaData() {
        d = new Date();
        s = d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getYear();
        if (document.getElementById('chkLasciatoAvviso').checked == true) {

            document.getElementById('txtDataPassaggio').value = s;
            document.getElementById('chkDARicontrollare').disabled = true;
            document.getElementById('chkDARicontrollare').checked = false;
        }
        else {
            document.getElementById('txtDataPassaggio').value = '';
            document.getElementById('chkDARicontrollare').disabled = false;
        }
    }
    function Disabilita() {
        if (document.getElementById('chkDARicontrollare').checked == true) {


            document.getElementById('chkLasciatoAvviso').disabled = true;
            document.getElementById('chkLasciatoAvviso').checked = false;
        }
        else {

            document.getElementById('chkLasciatoAvviso').disabled = false;
        }
    }

    function ConfermaConsumoNegativo() {
        if (confirm('Attenzione:\nLa lettura inserita risulta essere incongruente\ncontinuare con la registrazione dei dati ? ')) {

            document.getElementById('btnAppoggio').click();
        }
        else {

            return false;
        }

    }
    function ConfermaConsumoNegativoGriglia() {
        if (confirm('Attenzione:\nLa lettura inserita risulta essere incongruente\ncontinuare con la registrazione dei dati ? ')) {
            document.getElementById('txtConfirm').value = '1'
            document.getElementById('btnAppoggioGriglia').click();
        }
        else {
            document.getElementById('txtConfirm').value = ''
            return false;
        }

    }
    function SetPosition() {
        strSetPosition = document.getElementById('txtSetPosition').value;

        if (strSetPosition = '') {

            location.href = '#griglia';
            Setfocus(document.getElementById('txtUbicazione'));
        }
        else {

            lh = '#' + strSetPosition

            location.href = lh
        }
    }
    function PulisciCampi() {

    }

    function VisualizzaGiri(txtTemp, txtIDTemp, txtFocusTemp, objForm) {

        strtxtTemp = txtTemp.name;
        strtxtIDTemp = txtIDTemp.name;
        strtxtFocusTemp = txtFocusTemp.name;
        strFormName = objForm.name;

        /*WinPopUp=OpenPopup('OpenUtenze','/OpenUtenze/DataEntryLetture/VisualizzaGiri.aspx?FIELDMNAME='+strtxtTemp+'&IDFIELDMNAME='+strtxtIDTemp+'&FOCUSFIELDMNAME='+strtxtFocusTemp+'&FORM='+strFormName,'W3','500','500',0,0,'yes','no');*/
        WinPopUp = OpenPopup('OpenUtenze', '<%=Session("PATH_APPLICAZIONE")%>/DataEntryLetture/VisualizzaGiri.aspx?FIELDMNAME=' + strtxtTemp + '&IDFIELDMNAME=' + strtxtIDTemp + '&FOCUSFIELDMNAME=' + strtxtFocusTemp + '&FORM=' + strFormName, 'W3', '500', '500', 0, 0, 'yes', 'no');
            }
            function PulisciCampo(txtTemp, txtIDTemp) {

                document.getElementById('txtTemp').value = ''
                document.getElementById('txtIDTemp').value = ''

            }
            function VisualizzaPosizione(txtTemp, txtIDTemp, txtFocusTemp, objForm) {

                strtxtTemp = txtTemp.name;
                strtxtIDTemp = txtIDTemp.name;
                strtxtFocusTemp = txtFocusTemp.name;
                strFormName = objForm.name;

                /*WinPopUp=OpenPopup('OpenUtenze','/OpenUtenze/DataEntryLetture/VisualizzaPosizione.aspx?FIELDMNAME='+strtxtTemp+'&IDFIELDMNAME='+strtxtIDTemp+'&FOCUSFIELDMNAME='+strtxtFocusTemp+'&FORM='+strFormName,'W3','500','500',0,0,'yes','no');*/
                WinPopUp = OpenPopup('OpenUtenze', '<%=Session("PATH_APPLICAZIONE")%>/DataEntryLetture/VisualizzaPosizione.aspx?FIELDMNAME=' + strtxtTemp + '&IDFIELDMNAME=' + strtxtIDTemp + '&FOCUSFIELDMNAME=' + strtxtFocusTemp + '&FORM=' + strFormName, 'W3', '500', '500', 0, 0, 'yes', 'no');

            }
            function VisualizzaAnomalia(txtTemp, txtIDTemp, txtFocusTemp, objForm) {

                strtxtTemp = txtTemp.name;
                strtxtIDTemp = txtIDTemp.name;
                strtxtFocusTemp = txtFocusTemp.name;
                strFormName = objForm.name;

                /*WinPopUp=OpenPopup('OpenUtenze','/OpenUtenze/DataEntryLetture/VisualizzaAnomalie.aspx?FIELDMNAME='+strtxtTemp+'&IDFIELDMNAME='+strtxtIDTemp+'&FOCUSFIELDMNAME='+strtxtFocusTemp+'&FORM='+strFormName,'W3','500','500',0,0,'yes','no');*/
                WinPopUp = OpenPopup('OpenUtenze', '<%=Session("PATH_APPLICAZIONE")%>/DataEntryLetture/VisualizzaAnomalie.aspx?FIELDMNAME=' + strtxtTemp + '&IDFIELDMNAME=' + strtxtIDTemp + '&FOCUSFIELDMNAME=' + strtxtFocusTemp + '&FORM=' + strFormName, 'W3', '500', '500', 0, 0, 'yes', 'no');
            }

            function VerificaData(txtData) {
                if (!IsBlank(txtData.value)) {
                    if (!isDate(txtData.value)) {
                        alert("Inserire la Data di Lettura correttamente in formato: GG/MM/AAAA!");
                        txtData.value = '';
                        Setfocus(txtData);
                        return false;
                    }
                }
            }
            function VerificaDataDiLettura(txtData) {
                if (!IsBlank(txtData.value)) {
                    if (!isDate(txtData.value)) {
                        alert("Inserire la Data di Lettura correttamente in formato: GG/MM/AAAA!");
                        txtData.value = '';
                        Setfocus(txtData);
                        return false;
                    }
                    else {
                        document.getElementById('btnServ').click();
                    }
                }
            }


            function Salva() {
                
                document.getElementById('btnConferma').click();
            }

            function Annulla() {
                /*if(document.getElementById('PAG_PREC').value=='')
				{
					getElementById('PAG_PREC').value=1;
					getElementById('paginacomandi').value='/OPENutenze/DataEntryLetture/ComandiLetture.aspx';
				}
				getElementById('target ='visualizza';
				getElementById('action = "../DataEntryLetture/RicercaLetture.aspx";
				getElementById('submit();*/
                parent.parent.Comandi.location.href = '../DataEntryLetture/ComandiLetture.aspx';
                //parent.parent.Visualizza.location.href = '..//DataEntryLetture/RicercaLetture.aspx';
                parent.parent.Visualizza.location.href = '../DataEntryLetture/RicercaLetture.aspx';
            }
            function controlla(max, Max, maxlettere) {
                if (max.value.length > maxlettere)
                    max.value = max.value.substring(0, maxlettere);
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

				window.open('<% response.Write(UrlStradario) %>?' + Parametri, 'fStradario', 'top =' + (screen.height - 500) / 2 + ', left=' + (screen.width - 700) / 2 + ' width=700,height=500, status=yes, toolbar=no,scrollbar=no, resizable=no');
				return false;
            }

            function RibaltaStrada(objStrada) {
                // popolo il campo descrizione della via di residenza
                document.getElementById('TxtVia').value = (objStrada.TipoStrada + ' ' + objStrada.Strada).replace(/&#(\d+);/g, function (m, n) { return String.fromCharCode(n); });
                // popolo il campo codvia residenza
                document.getElementById('TxtCodVia').value = objStrada.CodStrada;
                //alert(document.getElementById('TxtCodVia').value);
            }

            function ClearDatiVia() {
                document.getElementById('TxtVia').value = '';
                document.getElementById('TxtCodVia').value = '-1';
                //alert(document.getElementById('TxtCodVia').value);
                return false;
            }

            function ReloadGrdLetture() {
                document.getElementById('btnReloadGrdLetture').click();
            }
    </script>
</head>
<body class="Sfondo" bottommargin="0" leftmargin="0" topmargin="0" marginheight="0" marginwidth="0">
    <form id="Form1" runat="server" method="post">
        <table cellspacing="0" cellpadding="1" width="98%" align="center" border="0">
            <tr>
                <td class="lstTabRow" colspan="6">Dati Contatore&nbsp;
						<asp:Label ID="lblContatore" runat="server" CssClass="NormalBold"></asp:Label></td>
            </tr>
            <tr>
                <td class="Input_Label">Impianto</td>
                <td class="Input_Label" colspan="4">Ubicazione</td>
                <td class="Input_Label">N.Civico/Esponente</td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="cboImpianto" runat="server" Width="170px" CssClass="Input_Text" Enabled="false"></asp:DropDownList></td>
                <td colspan="4">
                    <!--<asp:dropdownlist id="cboUbicazione" runat="server" Width="228px" Cssclass="Input_Text"></asp:dropdownlist>-->
                    <asp:TextBox ID="TxtVia" CssClass="Input_Text" runat="server" Width="400px" Enabled="false"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtNCivico" runat="server" CssClass="Input_Number_Generali" Width="49px" Enabled="False"></asp:TextBox>
                    <asp:TextBox ID="txtEsponente" runat="server" Width="60px" CssClass="Input_Text" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="Input_Label">Giro</td>
                <td class="Input_Label">Sequenza</td>
                <td class="Input_Label">Lato Strada</td>
                <td class="Input_Label" colspan="2">Posizione</td>
                <td class="Input_Label">Progressivo</td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="cboGiro" runat="server" Width="184px" CssClass="Input_Text" Enabled="false"></asp:DropDownList></td>
                <td>
                    <asp:TextBox ID="txtSequenza" runat="server" CssClass="Input_Number_Generali" ToolTip="Sequenza" Width="80px" Enabled="false"></asp:TextBox></td>
                <td>
                    <asp:TextBox ID="txtLatoStrada" runat="server" CssClass="Input_Text" ToolTip="Lato Strada" Width="56px" MaxLength="1" Enabled="false"></asp:TextBox></td>
                <td colspan="2">
                    <asp:DropDownList ID="cboPosizione" runat="server" Width="184px" CssClass="Input_Text" Enabled="false"></asp:DropDownList>
                <td>
                    <asp:TextBox ID="txtProgressivo" runat="server" CssClass="Input_Number_Generali" ToolTip="Posizione Progressiva del contatore" Width="80px" MaxLength="5" Enabled="false"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="Input_Label">Tipo Contatore</td>
                <td class="Input_Label">Cifre Contatore</td>
                <td class="Input_Label">Data Attivazione</td>
                <td class="Input_Label">Data Sostituzione</td>
                <td class="Input_Label">Data Rim. Temporanea</td>
                <td class="Input_Label">Data Cessazione</td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="cboTipoContatore" runat="server" Width="156px" CssClass="Input_Text" Enabled="false"></asp:DropDownList></td>
                <td style="height: 21px">
                    <asp:TextBox ID="txtCifreContatore" runat="server" CssClass="Input_Text" ToolTip="Cifre Contatore" Width="56px" Enabled="false"></asp:TextBox></td>
                <td>
                    <asp:TextBox ID="txtDataAttivazione" runat="server" CssClass="Input_Text" Enabled="False" ToolTip="Data di Attivazione Contatore" Width="90px"></asp:TextBox></td>
                <td>
                    <asp:TextBox ID="txtDataSostituzione" runat="server" CssClass="Input_Text" Enabled="False" ToolTip="Data Sostituzione" Width="90px"></asp:TextBox></td>
                <td>
                    <asp:TextBox ID="txtDataRimTemp" runat="server" CssClass="Input_Text" Enabled="False" ToolTip="Data Rimozione Temporanea" Width="90px"></asp:TextBox></td>
                <td>
                    <asp:TextBox ID="txtDataCessazione" runat="server" CssClass="Input_Text" Enabled="False" ToolTip="Data Cessazione" Width="90px"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="lstTabRow" colspan="6">Dati Utenza</td>
            </tr>
            <tr>
                <td class="Input_Label">Tipo Utenza</td>
                <td class="Input_Label">N.Utenze</td>
                <td class="Input_Label" colspan="2">Acqua Potabile</td>
                <td class="Input_Label">Fognatura</td>
                <td class="Input_Label">Depurazione</td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="cboTipoUtenze" runat="server" Width="250px" CssClass="Input_Text" Enabled="false"></asp:DropDownList></td>
                <td>
                    <asp:TextBox ID="txtNumeroUtenze" TabIndex="3" runat="server" CssClass="Input_Text" ToolTip="Numero Utenze" Width="50px" Enabled="false"></asp:TextBox></td>
                <td colspan="2">
                    <asp:CheckBox ID="chkEsenteAcqua" runat="server" CssClass="Input_Label" Text="Esente" BorderStyle="None" Enabled="false"></asp:CheckBox>
                    <asp:CheckBox ID="chkEsenteAcquaQF" runat="server" CssClass="Input_Label" Text="Esente Quota Fissa" BorderStyle="None" Enabled="false"></asp:CheckBox>
                </td>
                <td>
                    <asp:CheckBox ID="chkEsenteFognatura" runat="server" CssClass="Input_Label" Text="Esente" BorderStyle="None" Enabled="false"></asp:CheckBox>
                    <asp:CheckBox ID="chkEsenteFogQF" runat="server" CssClass="Input_Label" Text="Esente Quota Fissa" BorderStyle="None" Enabled="false"></asp:CheckBox>
                </td>
                <td>
                    <asp:CheckBox ID="chkEsenteDepurazione" runat="server" CssClass="Input_Label" Text="Esente" BorderStyle="None" Enabled="false"></asp:CheckBox>
                    <asp:CheckBox ID="chkEsenteDepQF" runat="server" CssClass="Input_Label" Text="Esente Quota Fissa" BorderStyle="None" Enabled="false"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td class="Input_Label" colspan="6">Note Contatore</td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:TextBox ID="txtNoteContatore" runat="server" CssClass="Input_Text" ToolTip="Note Inerenti al Contatore"
                        Width="100%" TextMode="MultiLine" Height="34px" Enabled="false"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table cellspacing="0" cellpadding="1" width="98%" align="center" border="0">
            <tr>
                <td colspan="5">
                    <asp:Label ID="lblInformation" runat="server" CssClass="NormalBold"></asp:Label></td>
            </tr>
            <tr>
                <td class="lstTabRow" colspan="5">Letture Precedenti (Ultime 5 Disponibili)</td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:Label ID="info" runat="server" CssClass="NormalBold"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="5">
                    <Grd:RibesGridView ID="GrdLetture" runat="server" BorderStyle="None"
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
                            <asp:TemplateField HeaderText="Per.">
                                <HeaderStyle Wrap="False" Width="5%"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label9" runat="server" Text='<%# FncGrd.FormattaPeriodo(DataBinder.Eval(Container, "DataItem.PERIODO")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="D. Lett.">
                                <HeaderStyle Wrap="False" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# FncGrd.FormatGrdData(DataBinder.Eval(Container, "DataItem.DATALETTURA"))%>'>Label</asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Lett.">
                                <HeaderStyle Wrap="False" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LETTURA") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="C.E.">
                                <HeaderStyle Wrap="False" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CONSUMO") %>' ToolTip="Consumo Effettivo">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="G.C.">
                                <HeaderStyle Wrap="False" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label11" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.GIORNIDICONSUMO") %>' ToolTip="Gioni">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="C.T.">
                                <HeaderStyle Wrap="False" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label8" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CONSUMOTEORICO") %>' ToolTip="Consumo Teorico">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="T.L.">
                                <HeaderStyle Wrap="False" Width="5%"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MODALITA") %>' ToolTip="Modalita' Lettura">
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P.L.">
                                <HeaderStyle Wrap="False" HorizontalAlign="Center" Width="5%" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="label5" runat="server" Text='<%#FncGrd.CheckStatus(DataBinder.Eval(Container, "DataItem.PRIMA")) %>' ToolTip="Prima Lettura">
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterStyle Wrap="False"></FooterStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fatt.">
                                <HeaderStyle Wrap="False" HorizontalAlign="Center" Width="5%" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="FATTURATA" runat="server" Text='<%#FncGrd.CheckStatus(DataBinder.Eval(Container, "DataItem.FATTURATA")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterStyle Wrap="False"></FooterStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="D.Pas.">
                                <HeaderStyle Wrap="False" Width="5%"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label10" runat="server" Text='<%# FncGrd.FormatGrdData(DataBinder.Eval(Container, "DataItem.DATADIPASSAGGIO")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="An.1">
                                <HeaderStyle Wrap="False" Width="15%"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label14" runat="server" Text='<%# FncGrd.DescriAnomalia(DataBinder.Eval(Container, "DataItem.COD_ANOMALIA1")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="An.2">
                                <HeaderStyle Wrap="False" Width="15%"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label15" runat="server" Text='<%# FncGrd.DescriAnomalia(DataBinder.Eval(Container, "DataItem.COD_ANOMALIA2")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="An.3">
                                <HeaderStyle Wrap="False" Width="20%"></HeaderStyle>
                                <ItemTemplate>
                                    <asp:Label ID="Label16" runat="server" Text='<%# FncGrd.DescriAnomalia(DataBinder.Eval(Container, "DataItem.COD_ANOMALIA3")) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("CODLETTURA") %>' alt=""></asp:ImageButton>
                                    <asp:HiddenField runat="server" ID="hfIdLettura" Value='<%# Eval("CODLETTURA") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </td>
            </tr>
         </table>
        <asp:Button ID="btnReloadGrdLetture" Style="display: none" runat="server" Text="Button"></asp:Button>
        <asp:Button ID="btnServ" Style="display: none" runat="server" Text="Button"></asp:Button>
        <asp:Button ID="btnAppoggio" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="btnConferma" Style="display: none" runat="server"></asp:Button>
        <asp:TextBox ID="txtCodContatore" Style="display: none" runat="server">-1</asp:TextBox>
        <asp:TextBox ID="txtCodVecchioContatore" Style="display: none" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" Style="display: none" runat="server" ToolTip="Conferma i dati Inseriti"
            Width="72px" Cssclass="Bottone Bottone" Height="24px" Text="Conferma"></asp:Button><asp:Button ID="btnAppoggioGriglia" Style="display: none" runat="server" ToolTip="Conferma i dati Inseriti"
                Width="72px" Cssclass="Bottone Bottone" Height="24px" Text="Conferma"></asp:Button><asp:Button ID="btnClose" Style="display: none" runat="server" ToolTip="Annulla  i dati Inseriti"
                    Width="72px" Cssclass="Bottone Bottone" Height="24px" Text="Annulla "></asp:Button><asp:TextBox ID="txtIDGiro" Style="display: none" runat="server"></asp:TextBox><asp:TextBox ID="txtSetPosition" Style="display: none" runat="server"></asp:TextBox><asp:TextBox ID="txtIDPosizione" Style="display: none" runat="server"></asp:TextBox><asp:TextBox ID="txtIDAnomalia" Style="display: none" runat="server"></asp:TextBox>
        <input id="hdCodiceViaLetture" type="hidden" name="hdCodiceViaLetture">
        <input id="hdEnteAppartenenzaLetture" type="hidden" name="hdEnteAppartenenzaLetture">
        <input id="hdSceltaViaLetture" type="hidden" name="hdSceltaViaLetture" value="0">
        <input id="hdIDContatore" type="hidden" name="hdIDContatore">
        <input id="hdCodiceVia" type="hidden" value="-1" name="hdCodiceVia">
        <input id="hdIntestatario" type="hidden" name="hdIntestatario">
        <input id="hdUtente" type="hidden" name="hdUtente">
        <input id="hdUbicazioneText" type="hidden" name="hdUbicazioneText">
        <input id="hdGiro" type="hidden" value="-1" name="hdGiro">
        <input id="hdNumeroUtente" type="hidden" name="hdNumeroUtente">
        <input id="hdCessati" type="hidden" name="hdCessati">
        <input id="paginacomandi" type="hidden" name="paginacomandi">
        <input id="PAG_PREC" type="hidden" name="PAG_PREC">
        <input id="hdSalva" type="hidden" name="hdSalva">
        <input id="hdCOD_STRADA" type="hidden" name="hdCOD_STRADA">
        <input id="hdNumeroCivico" type="hidden" name="hdNumeroCivico">
        <input id="hdIDGiro" type="hidden" value="-1" name="hdIDGiro">
        <input id="hdIDPosizione" type="hidden" value="-1" name="hdIDPosizione">
        <input id="hdProgressivo" type="hidden" name="hdProgressivo">
        <input id="hdSequenza" type="hidden" name="hdSequenza">
        <input id="hdTipoContatore" type="hidden" name="hdTipoContatore">
        <input id="hdIDUBICAZIONE" type="hidden" name="hdIDUBICAZIONE">
        <input id="hdESPONENTE" type="hidden" name="hdESPONENTE">
        <input id="hdMATRICOLA" type="hidden" name="hdMATRICOLA">
        <input id="hdMATRICOLACONTATORE" type="hidden" name="hdMATRICOLACONTATORE">
        <input id="hdLatoStrada" type="hidden" name="hdLatoStrada">
        <input id="hdNOTECONTATORE" type="hidden" name="hdNOTECONTATORE">
        <input id="hdCIFRECONTATORE" type="hidden" name="hdCIFRECONTATORE">&nbsp;
    </form>
</body>
</html>
