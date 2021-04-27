<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RicercaDichiarazione.aspx.vb" Inherits="OPENgovTIA.RicercaDichiarazione" %>

<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<html>
<head>
    <title>RicercaDichiarazione</title>
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
    <script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
    <script type="text/javascript">
        function EstraiExcel() {
            if (document.getElementById('loadGrid') == null) {
                GestAlert('a', 'warning', '', '', 'Impossibile accedere alla funzionalità di estrazione elenco in formato excel.\n Eseguire prima la ricerca.');
            }
            else {
                DivAttesa.style.display = '';
                //loadGrid.FrmResult.DivAttesa.Style.display = '';
                loadGrid.document.getElementById('CmdStampa').click()
            }
            return false;
        }

        function fStampaSintetica() {
            //if ( document.getElementById('loadGrid')==null)
            var myIFrame = document.getElementById('loadGrid');
            var myContent = myIFrame.contentWindow || myIFrame.contentDocument.defaultView;
            if (myContent.document.getElementById('GrdUtenti') == null && myContent.document.getElementById('GrdImmobili') == null)//(loadGrid.document.getElementById('GrdUtenti')==null && loadGrid.document.getElementById('GrdImmobili')==null )
            {
                GestAlert('a', 'warning', '', '', 'Impossibile accedere alla funzionalità di estrazione elenco in formato excel.\n Eseguire prima la ricerca.');
            }
            else {
                DivAttesa.style.display = '';
                //loadGrid.FrmResult.DivAttesa.Style.display = '';
                //loadGrid.FrmResult.CmdStampaSintetica.click()
                myContent.document.getElementById('CmdStampaSintetica').click();
            }
            return false;
        }

        function fStampaAnalitica() {
            //if ( document.getElementById('loadGrid')==null)
            var myIFrame = document.getElementById('loadGrid');
            var myContent = myIFrame.contentWindow || myIFrame.contentDocument.defaultView;
            if (myContent.document.getElementById('GrdUtenti') == null && myContent.document.getElementById('GrdImmobili') == null)//(loadGrid.document.getElementById('GrdUtenti')==null && loadGrid.document.getElementById('GrdImmobili')==null )
            {
                GestAlert('a', 'warning', '', '', 'Impossibile accedere alla funzionalità di estrazione elenco in formato excel.\n Eseguire prima la ricerca.');
            }
            else {
                DivAttesa.style.display = '';
                myContent.document.getElementById('CmdStampaAnalitica').click()
            }
            return false;
        }
        function ApriStradario(FunzioneRitorno, CodEnte) {
            var TipoStrada = '';
            var Strada = '';
            var CodStrada = '';
            var CodTipoStrada = '';
            var Frazione = '';
            var CodFrazione = '';

            var Parametri = '';
            if (CodEnte == '') {
                CodEnte = $('#ddlEnti').val();
            }
            Parametri += 'CodEnte=' + CodEnte;
            Parametri += '&TipoStrada=' + TipoStrada;
            Parametri += '&Strada=' + Strada;
            Parametri += '&CodStrada=' + CodStrada;
            Parametri += '&CodTipoStrada=' + CodTipoStrada;
            Parametri += '&Frazione=' + Frazione;
            Parametri += '&CodFrazione=' + CodFrazione;
            Parametri += '&Stile=<% = Session("StileStradario") %>';
			Parametri += '&FunzioneRitorno=' + FunzioneRitorno;
			if (CodEnte != '') {
			    window.open('<% response.write(UrlStradario) %>?' + Parametri, 'fStradario', 'top =' + (screen.height - 500) / 2 + ', left=' + (screen.width - 700) / 2 + ' width=700,height=500, status=yes, toolbar=no,scrollbar=no, resizable=no');
			    return false;
			}
			else {
			    GestAlert('a', 'warning', '', '', 'Impossibile accedere alla ricerca per stradario se non si ha selezionato un comune!');
			}
        }

            function RibaltaStrada(objStrada) {
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
                document.getElementById('TxtCodVia').value = '';
                document.getElementById('TxtVia').value = '';
                document.getElementById('TxtViaRibaltata').value = ' ';
                return false;
            }

            function Search() {
                /*if (document.FrmRicerca.RbSoggetto.checked == true)
				{
					loadGrid.location.href = 'ResultRicDichiarazione.aspx?TipoRicerca=S&TxtCognome='+document.FrmRicerca.TxtCognome.value+'&TxtNome='+document.FrmRicerca.TxtNome.value+'&TxtCodFiscale='+document.FrmRicerca.TxtCodFiscale.value+'&TxtPIva='+document.FrmRicerca.TxtPIva.value+'&TipoDichiarazione='+document.FrmRicerca.DdlDichiarazioni.value+'&TxtNTessera='+document.FrmRicerca.TxtNTessera.value
				}
				else
				{
					var sVia;
					sVia=document.FrmRicerca.TxtVia.value;
					loadGrid.location.href = 'ResultRicDichiarazione.aspx?TipoRicerca=I&TxtVia=' + encodeURIComponent(sVia) + '&TxtCivico='+document.FrmRicerca.TxtCivico.value+'&TxtInterno='+document.FrmRicerca.TxtInterno.value+'&TipoDichiarazione='+document.FrmRicerca.DdlDichiarazioni.value+'&TxtFoglio='+document.FrmRicerca.TxtFoglio.value+'&TxtNumero='+document.FrmRicerca.TxtNumero.value+'&TxtSubalterno='+document.FrmRicerca.TxtSubalterno.value 
				}*/
                //controlli di ricerca
                if (document.getElementById('RbImmobile').checked == true) {
                    if (document.getElementById('TxtNComponenti').value != '' && document.getElementById('TxtNComponenti').value != '-1' && document.getElementById('ChkPF').checked == false && document.getElementById('ChkPV').checked == false) {
                        GestAlert('a', 'warning', '', '', 'E\' obbligatorio indicare la parte di riferimento per i componenti!');
                        return false;
                    }
                    if (document.getElementById('TxtNComponenti').value == '' && (document.getElementById('ChkPF').checked == true || document.getElementById('ChkPV').checked == true)) {
                        GestAlert('a', 'warning', '', '', 'E\' obbligatorio indicare il numero di componenti!');
                        return false;
                    }
                    if (document.getElementById('ChkMoreUI').checked == true && document.getElementById('DdlCatastale').value == '') {
                        GestAlert('a', 'warning', '', '', 'E\' obbligatorio indicare la categoria catastale di ricerca!');
                        return false;
                    }
                }
                DivAttesa.style.display = '';
                document.getElementById('CmdSearch').click();
            }

            function NewDichiarazione() {
                parent.parent.Comandi.location.href = 'ComandiGestDichiarazioni.aspx?sProvenienza=N'
                parent.parent.Visualizza.location.href = 'GestDichiarazione.aspx?IdUniqueTestata=-1&AzioneProv=0'
            }

            function keyPress() {
                if (window.event.keyCode == 13) {
                    Search();
                }
            }
    </script>
</head>
<body class="Sfondo" bottommargin="0" leftmargin="3" rightmargin="3">
    <form id="Form1" runat="server" method="post">
        <table id="TblRicerca" cellspacing="1" cellpadding="1" width="100%" border="0">
            <tr>
                <td>
                    <fieldset class="FiledSetRicerca">
                        <legend class="Legend">Inserimento filtri di ricerca</legend>
                        <table id="TblParametri" cellspacing="1" cellpadding="1" width="100%" border="0">
                            <tr id="trEnte">
                                <td colspan="5">
                                    <asp:Label ID="lblEnti" runat="server" CssClass="Input_Label">Ente</asp:Label><br />
                                    <asp:DropDownList ID="ddlEnti" runat="server" CssClass="Input_Text" AutoPostBack="true"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="Input_Label">
                                    <asp:RadioButton ID="RbSoggetto" runat="server" Text="Per Soggetto" Checked="true" AutoPostBack="true"></asp:RadioButton>
                                    <asp:RadioButton ID="RbImmobile" runat="server" Text="Per Immobile" Checked="False" AutoPostBack="true"></asp:RadioButton>
                                </td>
                                <td>
                                    <asp:Label CssClass="Input_Label" runat="server" ID="LblProv">Provenienza Dichiarazione</asp:Label><br />
                                    <asp:DropDownList ID="DdlProv" CssClass="Input_Label" runat="server"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label runat="server" CssClass="Input_Label" ID="Label17">Immobili Dal</asp:Label><br />
                                    <asp:TextBox runat="server" ID="TxtDal" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" MaxLength="10"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label runat="server" CssClass="Input_Label" ID="Label18">Al</asp:Label><br />
                                    <asp:TextBox runat="server" ID="TxtAl" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" MaxLength="10"></asp:TextBox>
                                </td>
                                <td>&nbsp;
									<asp:DropDownList ID="DdlDichiarazioni" runat="server" CssClass="Input_Text">
                                        <asp:ListItem Value="-1">[TUTTE]</asp:ListItem>
                                        <asp:ListItem Value="0">Aperte</asp:ListItem>
                                        <asp:ListItem Value="1">Cessate</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Panel ID="PanelSoggetto" runat="server">
                                        <table width="100%">
                                            <tr>
                                                <td colspan="6">
                                                    <asp:Label ID="Label1" CssClass="Input_Label" runat="server">Ricerca per Persona</asp:Label><br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <asp:Label ID="Label2" CssClass="Input_Label" runat="server">Cognome</asp:Label><br />
                                                    <asp:TextBox ID="TxtCognome" runat="server" CssClass="Input_Text" Width="376px"></asp:TextBox></td>
                                                <td valign="top">
                                                    <asp:Label ID="Label3" CssClass="Input_Label" runat="server">Nome</asp:Label><br />
                                                    <asp:TextBox ID="TxtNome" runat="server" CssClass="Input_Text" Width="185px"></asp:TextBox></td>
                                                <td valign="top">
                                                    <asp:Label ID="Label4" CssClass="Input_Label" runat="server">Codice Fiscale</asp:Label><br />
                                                    <asp:TextBox ID="TxtCodFiscale" runat="server" CssClass="Input_Text" Width="170px" MaxLength="16"></asp:TextBox></td>
                                                <td valign="top">
                                                    <asp:Label ID="Label5" CssClass="Input_Label" runat="server">Partita IVA</asp:Label><br />
                                                    <asp:TextBox ID="TxtPIva" runat="server" CssClass="Input_Text" Width="160px" MaxLength="11"></asp:TextBox></td>
                                                <td id="TDRes">
                                                    <asp:RadioButton ID="OptAll" GroupName="SoggettiResidenti" Text="Tutti" runat="server" CssClass="Input_Radio" Checked="true" /><br />
                                                    <asp:RadioButton ID="OptRes" GroupName="SoggettiResidenti" Text="Residente" runat="server" CssClass="Input_Radio" /><br />
                                                    <asp:RadioButton ID="OptNoRes" GroupName="SoggettiResidenti" Text="Non Residente" runat="server" CssClass="Input_Radio" />
                                                </td>
                                                <td valign="top">
                                                    <asp:Label ID="LblNTessera" CssClass="Input_Label" runat="server">N.Tessera</asp:Label><br />
                                                    <asp:TextBox ID="TxtNTessera" runat="server" CssClass="Input_Text" Width="120px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="PanelImmobile" runat="server">
                                        <table width="100%">
                                            <tr>
                                                <td colspan="7">
                                                    <asp:Label ID="Label6" CssClass="Input_Label" runat="server">Ricerca per Immobile</asp:Label>
                                                    <span class="Input_Emphasized"> per ricercare tramite stradario/categoria/riduzione/esenzione bisogna prima selezionare un ente</span>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label7" CssClass="Input_Label" runat="server">Via</asp:Label>&nbsp;
													<asp:ImageButton ID="LnkOpenStradario" runat="server" ToolTip="Ubicazione Immobile da Stradario." CausesValidation="False" ImageAlign="Bottom" ImageUrl="../../images/Bottoni/Listasel.png"></asp:ImageButton>&nbsp;
													<asp:ImageButton ID="LnkPulisciStrada" runat="server" ToolTip="Pulisci i campi della Via" CausesValidation="False" ImageAlign="Bottom" ImageUrl="../../images/Bottoni/cancel.png" Width="10px" Height="10px"></asp:ImageButton>
                                                    <br />
                                                    <asp:TextBox ID="TxtVia" runat="server" CssClass="Input_Text" Width="376px" ReadOnly="true"></asp:TextBox>
                                                    <asp:TextBox ID="TxtCodVia" Style="display: none" CssClass="Input_Text" runat="server"></asp:TextBox>
                                                    <asp:TextBox ID="TxtViaRibaltata" Style="display: none" CssClass="Input_Text" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label8" CssClass="Input_Label" runat="server">Civico</asp:Label><br />
                                                    <asp:TextBox ID="TxtCivico" runat="server" CssClass="Input_Text" Width="50px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label11" CssClass="Input_Label" runat="server">Interno</asp:Label><br />
                                                    <asp:TextBox ID="TxtInterno" runat="server" CssClass="Input_Text" Width="50px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label10" runat="server" CssClass="Input_Label">Foglio</asp:Label><br />
                                                    <asp:TextBox ID="TxtFoglio" runat="server" CssClass="Input_Text" Width="50px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label9" runat="server" CssClass="Input_Label">Numero</asp:Label><br />
                                                    <asp:TextBox ID="TxtNumero" runat="server" CssClass="Input_Text" Width="50px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label12" runat="server" CssClass="Input_Label">Subalterno</asp:Label><br />
                                                    <asp:TextBox ID="TxtSubalterno" runat="server" CssClass="Input_Text" Width="50px"></asp:TextBox>
                                                </td>
                                                <td id="TDCatCat">
                                                    <asp:Label CssClass="Input_Label" runat="server" ID="Label14">Cat.Catastale</asp:Label><br />
                                                    <asp:DropDownList ID="DdlCatastale" CssClass="Input_Label" runat="server"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr id="TRCat">
                                                <td>
                                                    <asp:Label CssClass="Input_Label" runat="server" ID="Label35">Cat.</asp:Label><br />
                                                    <asp:DropDownList ID="DDlCatTARES" CssClass="Input_Label" runat="server" Width="376px"></asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label27" runat="server" CssClass="Input_Label">Componenti</asp:Label><br />
                                                    <asp:TextBox ID="TxtNComponenti" runat="server" CssClass="Input_Text_Right" MaxLength="2" Width="50px"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                    <asp:CheckBox ID="ChkPF" runat="server" CssClass="Input_CheckBox_NoBorder" Text="Parte Fissa" /><br />
                                                    <asp:CheckBox ID="ChkPV" runat="server" CssClass="Input_CheckBox_NoBorder" Text="Parte Variabile" />
                                                </td>
                                                <td colspan="3">
                                                    <asp:CheckBox ID="ChkEsente" CssClass="Input_CheckBox_NoBorder" runat="server" Text="Vani Esenti"></asp:CheckBox><br />
                                                    <asp:CheckBox ID="ChkMoreUI" CssClass="Input_CheckBox_NoBorder" runat="server" Text="Più di una UI per Cat.Catastale" />
                                                </td>
                                            </tr>
                                            <tr id="TRRidDet">
                                                <td>
                                                    <asp:Label CssClass="Input_Label" runat="server" ID="Label19">Riduzione</asp:Label><br />
                                                    <asp:DropDownList ID="DdlRid" CssClass="Input_Label" runat="server" Width="376px"></asp:DropDownList>
                                                </td>
                                                <td colspan="3">
                                                    <asp:Label CssClass="Input_Label" runat="server" ID="Label20">Detassazione</asp:Label><br />
                                                    <asp:DropDownList ID="DdlDet" CssClass="Input_Label" runat="server" Width="376px"></asp:DropDownList>
                                                </td>
                                                <td colspan="3">
                                                    <asp:Label CssClass="Input_Label" runat="server" ID="Label15">Stato Occupazione</asp:Label><br />
                                                    <asp:DropDownList ID="DdlStatoOccupazione" CssClass="Input_Label" runat="server"></asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
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
                <td>
                    <iframe id="loadGrid" src="../../aspVuota.aspx" frameborder="0" width="100%" height="350"></iframe>
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
        <asp:Button ID="CmdSearch" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdGIS" Style="display: none" runat="server"></asp:Button>
        <asp:Button ID="CmdAggMassivo" Style="display: none" runat="server"></asp:Button>
    </form>
</body>
</html>
