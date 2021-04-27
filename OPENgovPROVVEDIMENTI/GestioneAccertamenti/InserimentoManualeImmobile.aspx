<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="InserimentoManualeImmobile.aspx.vb" Inherits="Provvedimenti.InserimentoManualeImmobile" EnableEventValidation="false" %>

<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>GestioneAccertamenti</title>
    <meta name="vs_snapToGrid" content="False">
    <meta content="text/html; charset=windows-1252" http-equiv="Content-Type">
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
        function checkPrincipale() {
            if ((document.getElementById('chkPrinc').checked != false) && (document.getElementById('chkPert').checked != false)) {
                GestAlert('a', 'warning', '', '', 'Impossibile che un\'immobile sia definito Principale e Pertinenza!')
                document.getElementById('chkPrinc').checked = false;
                return false;
            }
        }

        function alertPertinenza() {
            GestAlert('a', 'warning', '', '', 'Impossibile che un\'immobile sia definito Principale e Pertinenza!')
        }
        function resetPertinenza() {
            document.getElementById("btnResetPertinenza").click()
        }
        function cercaImmobilePertinenza() {
            if (document.getElementById('chkPert').checked != false) {
                winWidth = 230
                winHeight = 140
                myleft = (screen.width - winWidth) / 2
                mytop = (screen.height - winHeight) / 2 - 40
                WinPopUpSanzioni = window.open("./Pertinenze/FramePertinenze.aspx", "", "width=" + winWidth + ",height=" + winHeight + ", status=yes, toolbar=no,top=" + mytop + ",left=" + myleft + ",Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no")
            }
        }

        function checkDate(annoAccertamento) {
            if (document.getElementById('txtDal').value == '') {
                GestAlert('a', 'warning', '', '', 'Inserire Data Dal!');
                document.getElementById('txtDal').focus();
                return false;
            }
            else {
                var data = new String
                data = document.getElementById('txtDal').value
                data = data.substring(6, 10)
                console.log('data substringata(6,10):  ' + data + '  annoAccertamento con cui confrontare:  ' + document.getElementById('hfAnno').value)
                if (data > annoAccertamento) {
                    GestAlert('a', 'warning', '', '', 'Impossibile inserire una data Dal con anno superiore all\'anno di accertamento ' + document.getElementById('hfAnno').value)
                    return false;
                }
            }
            if (document.getElementById('txtAl').value == '' || document.getElementById('txtAl').value == '31/12/9999' || document.getElementById('txtAl').value == '31/12/9999 23:59:59') {
                document.getElementById('txtAl').value = ''
            }
            else {
                var data = new String
                data = document.getElementById('txtAl').value
                data = data.substring(6, 10)
                if (data < document.getElementById('hfAnno').value) {
                    GestAlert('a', 'warning', '', '', 'Impossibile inserire una data Al con anno inferiore all\'anno di accertamento ' + annoAccertamento)
                    return false;
                }
            }

            if (document.getElementById('txtAl').value != '') {
                if (!doDateCheck(document.getElementById('txtAl'), document.getElementById('txtDal'))) {
                    GestAlert('a', 'warning', '', '', 'Data Dal minore di Data Al!');
                    return false;
                }
            }

            if (document.getElementById('txtFoglio').value == '') {
                document.getElementById('txtFoglio').value = 'ND'
            }
            if (document.getElementById('txtNumero').value == '') {
                document.getElementById('txtNumero').value = 'ND'
            }

            if (document.getElementById('ddlTR').value == '') {
                GestAlert('a', 'warning', '', '', 'Selezionare il Tipo di Rendita!');
                document.getElementById('ddlTR').focus();
                return false;
            }
            var tipoRendita = document.getElementById('ddlTR').text
            if (tipoRendita != 'AF - Aree edificabili' && tipoRendita != 'TA - Terreni agricoli') {
                if (document.getElementById('ddlCategoria').value == '-1') {
                    GestAlert('a', 'warning', '', '', 'Selezionare la Categoria!');
                    document.getElementById('ddlCategoria').focus();
                    return false;
                }
            }

            if (document.getElementById('txtRendita').value == '') {
                GestAlert('a', 'warning', '', '', 'Inserire la Rendita!');
                document.getElementById('txtRendita').focus();
                return false;
            }
            if (document.getElementById('txtValore').value == '') {
                GestAlert('a', 'warning', '', '', 'Inserire il Valore!');
                document.getElementById('txtValore').focus();
                return false;
            }
            if (document.getElementById('txtPercICI').value == '') {
                GestAlert('a', 'warning', '', '', 'Inserire la percentuale di possesso!');
                document.getElementById('txtPercICI').focus();
                return false;
            }
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

        function PulisciCampi() {
            document.getElementById('btnPulisci').click()

        }

        function ApriStradario() {
            var CodEnte = '<% = Session("cod_ente") %>';
            var TipoStrada = '';
            var Strada = document.getElementById('txtUbicazione').value;
            var CodStrada = document.getElementById('txtCodVia').value;
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
                document.getElementById('txtCodVia').value = objStrada.CodStrada;
                document.getElementById('txtUbicazione').value = strada;
                document.getElementById('TxtViaRibaltata').value = strada;
            }

            function PulisciCodVia() {
                document.getElementById('txtCodVia').value = ""
            }

            function CalcolaTariffa() {
                var ddlTR = document.getElementById('ddlTR');
                var CampiObbligatori = '';
                // controllo i campi obbligatori per il calcolo della rendita
                if ((ddlTR.options[ddlTR.selectedIndex].text) == 'AF') {
                    // i campi obbligatori sono ddlTR, ddlZona, txtConsistenza
                    // 
                    GestAlert('a', 'warning', '', '', 'Le aree edificabili prevedono solo il calcolo del valore.');
                    document.getElementById('txtRendita').value = '0';
                    return false;
                } else if ((ddlTR.options[ddlTR.selectedIndex].text) == 'LC') {
                    GestAlert('a', 'warning', '', '', 'Gli immobili con codice rendita LC prevedono solo il calcolo del valore.');
                    document.getElementById('txtRendita').value = '0';
                    return false;
                } else if ((ddlTR.options[ddlTR.selectedIndex].text) == 'TA') {
                    GestAlert('a', 'warning', '', '', 'Inserire manualmente il Reddito Domenicale nel campo rendita!');
                    return false;
                } else {
                    // controllo la validita dei campi : 
                    //alert(document.getElementById('ddlCategoria').value);
                    if (document.getElementById('ddlCategoria').value == '0') {
                        CampiObbligatori += '\n- Categoria Catastale';
                    }
                    if (document.getElementById('ddlClasse').value == '0,00') {
                        CampiObbligatori += '\n- Classe';
                    }
                    if (document.getElementById('ddlZona').value == '0') {
                        CampiObbligatori += '\n- Zona';
                    }
                    if (document.getElementById('txtConsistenza').value == '') {
                        CampiObbligatori += '\n- Consistenza';
                    }
                }

                if (ddlTR.value == '0') {
                    CampiObbligatori += '\n- Codice Rendita'
                }

                if (CampiObbligatori != '') {
                    GestAlert('a', 'warning', '', '', 'Attenzione,i seguenti campi sono obbligatori\nper il calcolo della rendita.' + CampiObbligatori);
                    return false;
                } else {
                    return true;
                }
            }

            function CalcolaValoreImmobile() {
                ddlCategoria = document.getElementById('ddlCategoria');
                var Rendita = document.getElementById('txtRendita').value;
                var Consistenza = document.getElementById('txtConsistenza').value;
                var CampiObbligatori = '';
                var Categoria = ddlCategoria.value;

                // calcolo il valore senza rivalutazione in base alla categoria
                if (Consistenza == '') {
                    CampiObbligatori = '\n- Consistenza';
                }
                var tipoRendita = document.getElementById('ddlTR').text
                if (tipoRendita != 'AF - Aree edificabili' && tipoRendita != 'TA - Terreni agricoli') {

                    if (Rendita == '') {
                        CampiObbligatori = '\n- Rendita';
                    }
                }
                if (Categoria == '0') {
                    CampiObbligatori = '\n- Categoria Catastale';
                }

                if (document.getElementById('txtDal').value == '') {
                    CampiObbligatori = '\n- Data Dal';
                }

                if (CampiObbligatori != '') {
                    GestAlert('a', 'warning', '', '', 'Attenzione, per il calcolo del valore immobile sono necessari i seguenti dati:' + CampiObbligatori);
                    return false;
                }

                var ddlTR = document.getElementById('ddlTR');

                if ((ddlTR.options[ddlTR.selectedIndex].text) == 'LC') {
                    GestAlert('a', 'warning', '', '', 'Inserire il valore Immobile manualmente!');
                    //document.getElementById('txtValore').value = document.getElementById('txtRendita').value;
                    return false;
                }

                return true;
                /*if (Categoria == 'A/10'){
					Rendita = Rendita.replace('.', '');
					Rendita = Rendita.replace(',', '.');
					
					//Rendita = parseFloat(Rendita);
					GestAlert('a', 'warning', '', '', 'Rendita = ' + Rendita);
					
					ValoreImmobile = Rendita * 50;
					alert(ValoreImmobile);
				}*/
            }
    </script>
</head>
<body class="SfondoVisualizza" onload="document.formRicercaAnagrafica.focus();" rightmargin="0"
    topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" runat="server" method="post">
        <fieldset class="classeFiledSet">
            <legend class="Legend">Riferimenti Catastali</legend>
            <table>
                <tr>
                    <td>
                        <table id="TableIndImm" border="0" cellpadding="0" align="left">
                            <tr>
                                <td class="Input_Label">
                                    <asp:Label ID="lblUbicazione" runat="server">Ubicazione</asp:Label></td>
                                <td class="Input_Label">
                                    <asp:Label ID="Label4" runat="server">Civico</asp:Label></td>
                                <td class="Input_Label">
                                    <asp:Label ID="Label5" runat="server">Esp Civico</asp:Label></td>
                                <td class="Input_Label">
                                    <asp:Label ID="Label6" runat="server">Scala</asp:Label></td>
                                <td class="Input_Label">
                                    <asp:Label ID="Label7" runat="server">Interno</asp:Label></td>
                                <td class="Input_Label">
                                    <asp:Label ID="Label8" runat="server">Piano</asp:Label></td>
                                <td class="Input_Label">
                                    <asp:Label ID="Label9" runat="server">Barrato</asp:Label></td>
					            <td rowspan="2">
                                    <div id="divTipoTASI">
					                    <asp:CheckBox ID="chkProp" runat="server" CssClass="Input_CheckBox_NoBorder" Text="proprietario" Checked="true"/>
					                    <br />
					                    <asp:CheckBox ID="chkInqu" runat="server" CssClass="Input_CheckBox_NoBorder" Text="inquilino" Checked="false"/>
                                    </div>
					            </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtUbicazione" runat="server" CssClass="Input_Text" Width="300"></asp:TextBox>
                                    <asp:TextBox ID="TxtViaRibaltata" Style="display: none" CssClass="Input_Text" runat="server"></asp:TextBox>
                                    <asp:TextBox Style="display: none" ID="txtCodVia" runat="server"></asp:TextBox>
                                    <asp:TextBox Style="display: none" ID="txtCodComune" runat="server"></asp:TextBox>
                                    <asp:TextBox Style="display: none" ID="txtComune" runat="server" CssClass="Input_Text"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCivico" runat="server" Width="70" CssClass="Input_Text"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox ID="txtEspCivico" runat="server" Width="70" CssClass="Input_Text"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox ID="txtScala" runat="server" Width="70" CssClass="Input_Text"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox ID="txtInterno" runat="server" Width="70" CssClass="Input_Text"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox ID="txtPiano" runat="server" Width="70" CssClass="Input_Text"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox ID="txtBarrato" runat="server" Width="70" CssClass="Input_Text"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="TableRifCatastali" border="0" cellpadding="0" align="left">
                            <tr>
                                <td style="width: 200px; height: 13px" class="Input_Label">
                                    <asp:Label ID="lblDal" runat="server">Dal</asp:Label></td>
                                <td style="width: 110px; height: 13px" class="Input_Label">
                                    <asp:Label ID="lblAl" runat="server">Al</asp:Label></td>
                                <td style="width: 110px; height: 13px" class="Input_Label" width="88">
                                    <asp:Label ID="lblSezione" runat="server">Sezione</asp:Label></td>
                                <td style="width: 110px; height: 13px" class="Input_Label" width="92">
                                    <asp:Label ID="lblFoglio" runat="server">Foglio</asp:Label></td>
                                <td style="width: 110px; height: 13px" class="Input_Label" width="77">
                                    <asp:Label ID="lblNumero" runat="server">Numero</asp:Label></td>
                                <td style="width: 110px; height: 13px" class="Input_Label" width="84">
                                    <asp:Label ID="lblSub" runat="server">SubAlterno</asp:Label></td>
                                <td style="width: 110px; height: 13px" class="Input_Label" width="65">
                                    <asp:Label ID="lblcat" runat="server">Categoria</asp:Label></td>
                                <td style="width: 110px; height: 13px" class="Input_Label" width="73">
                                    <asp:Label ID="lblclasse" runat="server">Classe</asp:Label></td>
                                <td style="width: 110px; height: 13px" class="Input_Label" width="178">
                                    <asp:Label ID="lblconsistenza" runat="server">Consistenza</asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 50px">
                                    <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this)" ID="txtDal" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                                <td style="width: 49px">
                                    <asp:TextBox onblur="txtDateLostfocus(this);VerificaData(this);ConfrontaDataUguale(this,txtDal,'Dal');" ID="txtAl" onfocus="txtDateGotfocus(this);" runat="server" CssClass="Input_Text_Right TextDate"></asp:TextBox></td>
                                <td style="width: 92px; height: 19px">&nbsp;<asp:TextBox ID="txtSezione" onkeypress="return NumbersOnly(event,false,false,0)" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="60px" MaxLength="4"></asp:TextBox></td>
                                <td style="width: 92px; height: 19px">
                                    <asp:TextBox ID="txtFoglio" onkeypress="return NumbersOnly(event,false,false,0)" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="60px" MaxLength="4"></asp:TextBox></td>
                                <td style="width: 77px; height: 19px">
                                    <asp:TextBox ID="txtNumero" onkeypress="return NumbersOnly(event,false,false,0)" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="60px" MaxLength="4"></asp:TextBox></td>
                                <td style="width: 84px; height: 19px">
                                    <asp:TextBox ID="txtSubalterno" onkeypress="return NumbersOnly(event,false,false,0)" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="60px" MaxLength="4"></asp:TextBox></td>
                                <td style="width: 65px; height: 19px">
                                    <asp:TextBox Style="display: none" ID="txtCategoria" runat="server" CssClass="Input_Text_right"
                                        Width="48px"></asp:TextBox><asp:DropDownList ID="ddlCategoria" runat="server" CssClass="Input_Text" Width="80px"></asp:DropDownList></td>
                                <td style="width: 73px; height: 19px">
                                    <asp:TextBox Style="display: none" ID="txtClasse" runat="server" CssClass="Input_Text_right"
                                        Width="48px"></asp:TextBox><asp:DropDownList ID="ddlClasse" runat="server" CssClass="Input_Text" Width="140px"></asp:DropDownList></td>
                                <td style="width: 178px; height: 19px">
                                    <asp:TextBox ID="txtConsistenza" onkeypress="return NumbersOnly(event,true,false,2)" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="70px" MaxLength="6"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="TableDatiProprietari" border="0" cellpadding="0" align="left">
                            <tr>
                                <td style="width: 260px" class="Input_Label">
                                    <asp:Label ID="lblTr" runat="server">Tipo Rendita</asp:Label></td>
                                <td style="width: 260px" class="Input_Label">
                                    <asp:Label ID="Label2" runat="server">Zona</asp:Label></td>
                                <td style="width: 60px" class="Input_Label">
                                    <asp:Label ID="lblRendita" runat="server">Rendita</asp:Label>&nbsp;<asp:LinkButton ID="lnkRendita" title="Calcola Rendita" runat="server" CssClass="Input_Label">&raquo;</asp:LinkButton></td>
                                <td style="width: 60px" class="Input_Label">
                                    <asp:Label ID="lblValore" runat="server">Valore</asp:Label>&nbsp;<asp:LinkButton ID="lnkValore" title="Calcola Valore" runat="server" CssClass="Input_Label">&raquo;</asp:LinkButton></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlTR" runat="server" CssClass="Input_Text" AutoPostBack="True"></asp:DropDownList></td>
                                <td>
                                    <asp:DropDownList ID="ddlZona" runat="server" CssClass="Input_Text" Width="450px"></asp:DropDownList></td>
                                <td>
                                    <asp:TextBox ID="txtRendita" onkeypress="return NumbersOnly(event,true,false,2)" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="90px"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox ID="txtValore" onkeypress="return NumbersOnly(event,true,false,2)" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="90px"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 100%" id="TableDatiProprietari1" border="0" cellpadding="0" align="left">
                            <tr>
                                <!--*** 20120704 - IMU ***-->
                                <td class="Input_Label">
                                    <asp:Label ID="lblPercICI" runat="server">%ICI/IMU</asp:Label></td>
                                <td class="Input_Label">
                                    <asp:Label ID="lblTitPossesso" runat="server" Width="92px">Tipo Utilizzo</asp:Label></td>
                                <!--*** 20140509 - TASI *** Titolo Possesso-->
                                <td class="Input_Label">
                                    <asp:Label ID="lblPrinc" runat="server">Princ.</asp:Label></td>
                                <td class="Input_Label">
                                    <asp:Label ID="lblPert" runat="server">Pert.</asp:Label></td>
                                <td class="Input_Label">
                                    <asp:Label ID="lblRidotto" runat="server">Rid.</asp:Label></td>
                                <td class="Input_Label">
                                    <asp:Label ID="Label1" runat="server">N° Utiliz.</asp:Label></td>
                                <td class="Input_Label">
                                    <asp:Label ID="Label10" runat="server">Esente</asp:Label></td>
                                <td class="Input_Label">
                                    <asp:Label ID="lblcoltivatore" runat="server">Coltivatore diretto</asp:Label></td>
                                <td class="Input_Label">
                                    <asp:Label ID="lblnumfigli" runat="server">Num. figli</asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox onblur="verificaPercentuale(this)" ID="txtPercICI" onkeypress="return NumbersOnly(event,true,false,2)" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="48px" MaxLength="5"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTitPossesso" runat="server" CssClass="Input_Text"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkPrinc" runat="server" Text=" "></asp:CheckBox>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkPert" runat="server" Text=" " AutoPostBack="True"></asp:CheckBox>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkRidotto" runat="server" Text=" "></asp:CheckBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNumUtiliz" onkeypress="return NumbersOnly(event,false,false,0)" runat="server" CssClass="Input_Text_Right OnlyNumber" Width="48px" MaxLength="3"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEsente" runat="server" CssClass="Input_Text"></asp:DropDownList>
                                    <asp:TextBox Style="display: none" ID="txtMesiEsclusioneEsezione" onkeypress="return NumbersOnly(event,true,false,0)" runat="server" Width="48px" MaxLength="5" CssClass="Input_Text_Right OnlyNumber"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkcoltivatore" runat="server" Text=" " TextAlign="Left"></asp:CheckBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtnumfigli" runat="server" CssClass="Input_Text_Right" Width="60px" MaxLength="3" AutoPostBack="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8"></td>
                                <td colspan="8">
                                    <div id="DivCaricoFigli" title="Percentuale Carico Figli">
                                        <asp:Label ID="Label3" runat="server" CssClass="Input_Label" Enabled="False">Percentuale Carico Figli</asp:Label>
                                        <br>
                                        <Grd:RibesGridView ID="GrdCaricoFigli" runat="server" BorderStyle="None"
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
                                                        <asp:TextBox ID="TxtPercentCarico" onblur="VerificaPercentCarico(this);" runat="server" Width="100px" CssClass="Input_Text_Right" Text='<%# DataBinder.Eval(Container, "DataItem.Percentuale") %>'>
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </Grd:RibesGridView>
                                    </div>
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
            <asp:Button Style="display: none" ID="btnSearchDichiarazioni" runat="server"></asp:Button>
            <asp:Button Style="z-index: 101; position: absolute; display: none; top: 180px; left: 25px" ID="btnRibalta" runat="server" Text="Ribalta Dati Griglia"></asp:Button>
            <asp:Button Style="z-index: 105; position: absolute; display: none; top: 157px; left: 25px" ID="btnFocus" runat="server" Width="1px" Height="1px"></asp:Button>
            <asp:HiddenField ID="hfAnno" runat="server"></asp:HiddenField>
            <asp:Button ID="btnPulisci" Style="z-index: 107; position: absolute; display: none; top: 188px; left: 308px" runat="server" Text="Pulisci"></asp:Button>
            <asp:Button ID="btnResetPertinenza" Style="z-index: 105; position: absolute; display: none; top: 157px; left: 25px" runat="server" Height="1px" Width="1px"></asp:Button>
        </fieldset>
    </form>
</body>
</html>
