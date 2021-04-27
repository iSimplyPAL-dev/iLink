<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RicercaAnagrafica.aspx.vb" Inherits="RicercaAnagrafica" %>

<html>
<head>
    <%
        Dim sessionName
        sessionName = Request.Item("sessionName")
    %>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
        <script type="text/javascript" src="../_js/Utility.js?newversion"></script>
        <script type="text/javascript" src="../_js/VerificaCampi.js?newversion"></script>
        <script type="text/vbscript" src="../_vbs/OperazioniSuCampi.vbs"></script>
        <script type="text/vbscript" src="../_vbs/ControlliFormali.vbs"></script>
        <script type="text/javascript" src="../_js/jsComuniStrade.js?newversion"></script>
        <script type="text/javascript">
        function estraiExcel() {
            if (document.getElementById('loadGrid') == null) {
                GestAlert('a', 'warning', '', '', 'Impossibile accedere alla funzionalità di estrazione elenco in formato excel.\n Eseguire la ricerca.');
            }
            else {
                DivAttesa.style.display = '';
                document.getElementById('btnStampaExcel').click();
            }												
        }
        function Nuovo() {
            if ($('#hdEnteAppartenenza').val() == '') {
                GestAlert('a', 'warning', '', '', 'Impossibile accedere alla funzionalità di inserimento dalla funzione sovracomunale.');
            }
            else {
                document.getElementById('btnNuovo').click();
            }
        }

        function Search(popUp) {
            Parametri = "?cognome=" + document.getElementById('txtCognome').value + "&nome=" + document.getElementById('txtNome').value + "&codicefiscale=" + document.getElementById('txtCodiceFiscale').value + "&partitaiva=" + document.getElementById('txtPartitaIva').value + "&codcontribuente=" + document.getElementById('txtCodContribuente').value + "&DARICONTROLLARE=" + document.getElementById('chkDaRicontrollare').checked + "&Via=" + document.getElementById('txtViaResidenza').value + "&CodVia=" + document.getElementById('txtCodViaResidenza').value;
            Parametri = Parametri + "&NONAGGANCIATE=" + document.getElementById('chkNonAgganciate').checked
            Parametri = Parametri + "&comuneresidenza=" + document.getElementById('txtComuneResidenza').value
            Parametri = Parametri + "&provinciaresidenza=" + document.getElementById('txtProvinciaResidenza').value
            Parametri = Parametri + "&datanascita=" + document.getElementById('txtDataNascita').value
            Parametri = Parametri + "&datamorte=" + document.getElementById('txtDataMorte').value
            Parametri = Parametri + "&ddlContatti=" + document.getElementById('ddlContatti').value
            Parametri = Parametri + "&ddlTributoInvio=" + document.getElementById('ddlTributoInvio').value
            Parametri = Parametri + "&ddlTributoPresente=" + document.getElementById('ddlTributoPresente').value
            //*** 201511 - Funzioni Sovracomunali ***
            Parametri = Parametri + "&IdEnte=" + document.getElementById('ddlEnti').value
            //*** ***
            if (popUp == '1') {
                Parametri = Parametri + "&sessionName=<%=sessionName%>"
			    document.getElementById('loadGrid').src = "SearchResultsAnagraficaGenerale.aspx" + Parametri
			}
			else {
			    DivAttesa.style.display = ''; document.getElementById('loadGrid').src = "../aspVuota.aspx";
			    document.getElementById('loadGrid').src = "SearchResultsAnagrafica.aspx" + Parametri
			}
            return true;
        }
        function PulisciCampi() {
            document.getElementById('txtCognome').value = '';
            document.getElementById('txtNome').value = '';
            document.getElementById('txtCodiceFiscale').value = '';
            document.getElementById('txtCodiceFiscale').value = '';
            document.getElementById('txtCodContribuente').value = '';
            document.getElementById('txtViaResidenza').value = '';
            document.getElementById('txtCodViaResidenza').value = '';

            document.getElementById('chkDaRicontrollare').checked = '';
            document.getElementById('chkNonAgganciate').checked = '';

            document.getElementById('txtComuneResidenza').value = '';
            document.getElementById('txtProvinciaResidenza').value = '';
            document.getElementById('txtDataNascita').value = '';
            document.getElementById('txtDataMorte').value = '';
            //*** 201511 - Funzioni Sovracomunali ***
            document.getElementById('ddlEnti').value = '-1';
            //*** ***

            document.getElementById('loadGrid').src = "../aspVuota.aspx"
            document.getElementById('txtCognome').focus();
        }

        function keyPress(popUp) {
            if (window.event.keyCode == 13) {
                //parent.Comandi.Search.click();
                SearchReturn(popUp);
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

            /*alert(''+FunzioneRitorno);
            alert(''+CodEnte);*/

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
                function RibaltaStrada(objStrada) {

                    // popolo il campo descrizione della via di residenza
                    document.getElementById('txtViaResidenza').value = (objStrada.TipoStrada + ' ' + objStrada.Strada).replace(/&#(\d+);/g, function (m, n) { return String.fromCharCode(n); });
                    // popolo il campo codvia residenza
                    document.getElementById('txtCodViaResidenza').value = objStrada.CodStrada;
                    // popolo il campo frazione della residenza
                    /*document.getElementById('txtFrazioneResidenza').value = objStrada.Frazione;*/
                }
                function VisualizzaParAvanzati() {
                    //btnParametriAvanzati
                    if (document.getElementById("paravanzati").style.display == "none") {
                        document.getElementById("btnParametriAvanzati").value = "Nascondi parametri avanzati"
                        document.getElementById("paravanzati").style.display = ""
                        document.getElementById("paravanzati1").style.display = ""
                        document.getElementById("paravanzati2").style.display = ""
                        document.getElementById("paravanzati3").style.display = ""
                    } else {
                        document.getElementById("btnParametriAvanzati").value = "Visualizza parametri avanzati"
                        document.getElementById("paravanzati").style.display = "none"
                        document.getElementById("paravanzati1").style.display = "none"
                        document.getElementById("paravanzati2").style.display = "none"
                        document.getElementById("paravanzati3").style.display = "none"
                    }

                }
                function VerificaData(Data, desc) {
                    if (!IsBlank(Data.value)) {
                        if (!isDate(Data.value)) {
                            alert("Inserire la " + desc + " correttamente in formato: gg/mm/aaaa !");
                            Setfocus(Data);
                            return false;
                        }
                    }
                }
                function RibaltaComuneResidenza(objComune) {

                    // COD COMUNE RESIDENZA
                    //document.getElementById('hdCodComuneResidenza').value = objComune.CodBelfiore;
                    // COD COMUNE RESIDENZA PER UTILIZZO STRADARIO
                    //document.getElementById('hdCodComStradaResidenza').value = parseInt(objComune.CodIstat, 10);					
                    // COMUNE RESIDENZA
                    document.getElementById('txtComuneResidenza').value = objComune.Denominazione;
                    // CAP RESIDENZA 
                    //document.getElementById('txtCAPResidenza').value = objComune.Cap;					
                    //PROV RESIDENZA
                    document.getElementById('txtProvinciaResidenza').value = objComune.Provincia;

                    // se il comune gestisce lo stradario obbligo l'utente a selezionare lo stradario
                    /*if (objComune.HaStradario == 'True'){
						AbilitaRicercaStradario('linkStradaResidenza', '');
						AbilitaTxtStradario('txtViaResidenza', true);
					}else{
						AbilitaRicercaStradario('linkStradaResidenza', 'none');
						AbilitaTxtStradario('txtViaResidenza',false);
					}*/
                }
                function ControllaCodContribuente() {
                    valore = document.getElementById("txtCodContribuente").value
                    if (valore != "" && isNaN(valore)) {
                        alert("Il codice Contribuente deve essere un numero")
                        document.getElementById("txtCodContribuente").value = ""
                        document.getElementById("txtCodContribuente").focus()
                    }
                }
    </script>
</head>
<body class="Sfondo" bottommargin="0" leftmargin="0" topmargin="0">
    <form id="Form1" runat="server" method="post">
        <br>
        <table id="tabEsterna" style="display: " cellspacing="1" cellpadding="1" width="100%" align="center" border="0">
            <tr>
                <td>
                    <fieldset>
                        <legend class="Legend">Inserimento parametri di Ricerca</legend>
                        <table cellspacing="1" cellpadding="1" width="98%" align="center" border="0">
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblEnti" runat="server" CssClass="Input_Label">Ente</asp:Label><br />
                                    <asp:DropDownList ID="ddlEnti" runat="server" CssClass="Input_Text" DataTextField="string" DataValueField="string"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="Input_Label">Cognome</td>
                                <td colspan="2" class="Input_Label">Nome</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="txtCognome" runat="server" Width="400px" MaxLength="100" CssClass="Input_Text" TabIndex="1"></asp:TextBox></td>
                                <td colspan="2">
                                    <asp:TextBox CssClass="Input_Text" runat="server" MaxLength="50" Width="350px" ID="txtNome" TabIndex="2"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="Input_Label" colspan="2">Codice Fiscale</td>
                                <td class="Input_Label" colspan="2">Partita Iva</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="txtCodiceFiscale" runat="server" Width="200px" MaxLength="16" CssClass="Input_Text" TabIndex="3"></asp:TextBox></td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtPartitaIva" runat="server" Width="200px" MaxLength="11" CssClass="Input_Text" TabIndex="4"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="Input_Label" colspan="2">Codice Contribuente</td>
                                <td class="Input_Label" colspan="2">Via Residenza&nbsp;<asp:LinkButton ID="lnkOpenStradario" runat="server" CssClass="Input_Label">&raquo;</asp:LinkButton></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="txtCodContribuente" onkeyup="disableLetterChar(this);" onblur="ControllaCodContribuente()" runat="server" Width="100px" MaxLength="11" CssClass="Input_Number_Generali" TabIndex="5"></asp:TextBox>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtViaResidenza" runat="server" CssClass="Input_Text" Width="400px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <input class="btnList_botton" id="btnParametriAvanzati" onclick="VisualizzaParAvanzati();" type="button" value="Visualizza parametri avanzati">
                                </td>
                            </tr>
                            <!--Inizio Gestione parametri avanzati-->
                            <tr id="paravanzati" style="display: none">
                                <td>
                                    <asp:CheckBox ID="chkDaRicontrollare" TabIndex="6" runat="server" CssClass="Input_CheckBox_NoBorder" Text="Anagrafiche da Ricontrollare"></asp:CheckBox>
                                </td>
                                <td class="Input_Label">Tipo Contatto</td>
                                <td class="Input_Label">Tributo Spedizione</td>
                                <td class="Input_Label">Presente in</td>
                            </tr>
                            <tr id="paravanzati1" style="display: none">
                                <td>
                                    <asp:TextBox ID="txtCodViaResidenza" runat="server" Style="display: none"></asp:TextBox>
                                    <asp:CheckBox ID="chkNonAgganciate" CssClass="Input_CheckBox_NoBorder" Text="Anagrafiche non agganciate a stradario" runat="server"></asp:CheckBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlContatti" runat="server" CssClass="Input_Text"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTributoInvio" runat="server" CssClass="Input_Text"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTributoPresente" runat="server" CssClass="Input_Text"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="paravanzati2" style="display: none">
                                <td class="Input_Label">Comune di residenza&nbsp;
                                    <div class="tooltip">
                                        <a id="linkapricomuneresidenza" onclick="ApriComuni('RibaltaComuneResidenza','','','','',document.getElementById('txtComuneResidenza').value,document.getElementById('txtProvinciaResidenza').value, '<% = Session("StileStradario") %>', '<% = UrlPopComuni %>');" href="javascript: void(0)" class="nascosto">
                                            <img alt="" class="BottoneSel BottoneLista">
                                            <span class="tooltiptext">Scelta Comuni</span>
                                        </a>
                                    </div>
                                </td>
                                <td class="Input_Label">Provincia&nbsp;
                                    <div class="tooltip">
                                        <a onclick="document.getElementById('linkapricomuneresidenza').click();" href="javascript: void(0)">
                                            <img alt="" class="BottoneSel BottoneLista">
                                            <span class="tooltiptext">Scelta Comuni</span>
                                        </a>
                                    </div>
                                </td>
                                <td class="Input_Label">Data di nascita</td>
                                <td class="Input_Label">Data di morte</td>
                            </tr>
                            <tr id="paravanzati3" style="display: none">
                                <td>
                                    <asp:TextBox ID="txtComuneResidenza" runat="server" CssClass="Input_Text" Width="400px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtProvinciaResidenza" runat="server" CssClass="Input_Text" Width="50px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDataNascita" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="10" onblur="txtDateLostfocus(this);VerificaData(this,'Data di nascita');"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDataMorte" runat="server" CssClass="Input_Text_Right TextDate" MaxLength="10" onblur="txtDateLostfocus(this);VerificaData(this,'Data di morte');"></asp:TextBox>
                                </td>
                            </tr>
                            <!--Fine Gestione parametri avanzati-->
                        </table>
                    </fieldset>
                    <br>
                    <fieldset class="classeFiledSetIframe">
                        <legend class="Legend">Visualizzazione Anagrafiche Estratte</legend>
                        <table style="width:100%">
                            <tr>
                                <td>
                                   <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                                        <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                                        <div class="BottoneClessidra">&nbsp;</div>
                                        <div class="Legend">Attendere Prego</div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <iframe class="bordoIframe" id="loadGrid" style="width: 100%; height: 400px" src="SearchResultsAnagrafica.aspx" frameborder="0"></iframe>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <asp:HiddenField runat="server" id="hdEnteAppartenenza" />
                    <asp:Button ID="btnNuovo" Style="display: none" runat="server"></asp:Button>
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
        <asp:button id="btnStampaExcel" style="DISPLAY: none" runat="server" Text="Button"></asp:button>
    </form>
</body>
</html>
