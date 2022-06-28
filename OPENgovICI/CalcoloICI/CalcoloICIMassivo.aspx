<%@ Page language="c#" Codebehind="CalcoloICIMassivo.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CalcoloICI.CalcoloICIMassivo" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<head>
		<title>CalcoloICIMassivo</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		    function LinkCalcoloICI(COD_CONTRIB, ANNO) {
		        ANNO = document.getElementById('ddlAnnoRiferimento').value
		        if (ANNO == "") {
		            //*** 20120704 - IMU ***
		            GestAlert('a', 'warning', '', '', 'Selezionare un anno per il quale visualizzare la situazione!');
		            return false;
		        }

		        winWidth = 990;
		        winHeight = 660;
		        myleft = (screen.width - winWidth) / 2
		        mytop = (screen.height - winHeight) / 2 - 40;

		        ANNO = document.getElementById('ddlAnnoRiferimento').value
		        //*** 20130422 - aggiornamento IMU ****
		        if (document.getElementById('rdbCalcoloNetto').checked == true) {
		            bNettoVersato = true;
		        }
		        else {
		            bNettoVersato = false;
		        }
		        //CalWin = window.open("GetRiepilogoICIframe.aspx?CODCONTRIB="+COD_CONTRIB+"&ANNO="+ANNO+"&blnCalcoloMassivo=true&ID_PROG_ELAB=-1","ShowCalcoloICI", "width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+", scrollbars=no,toolbar=no")
		        CalWin = window.open("GetRiepilogoICIframe.aspx?CODCONTRIB=" + COD_CONTRIB + "&ANNO=" + ANNO + "&blnCalcoloMassivo=true&ID_PROG_ELAB=-1&bNettoVersato=" + bNettoVersato, "ShowCalcoloICI", "width=" + winWidth + ",height=" + winHeight + ", status=yes, toolbar=no,top=" + mytop + ",left=" + myleft + ", scrollbars=no,toolbar=no")
		        //*** ***

		        //CalWin = window.open("GetRiepilogoICI.aspx?ANNO="+ANNO,"ShowCalcoloICI", "width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+", scrollbars=no,toolbar=no")								
		        //				CalWin = window.open("GetCalcoloICI.aspx?ANNO="+ANNO,"ShowCalcoloICI", "width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+", scrollbars=no,toolbar=no")
		    }

		    function Controlli() {
		        ANNO = document.getElementById('ddlAnnoRiferimento').value
		        if (ANNO == "") {
		            //*** 20120704 - IMU ***
		            GestAlert('a', 'warning', '', '', 'Selezionare un anno per il quale visualizzare la situazione!');
		            return false;
		        }

		        if (confirm('Si vuole proseguire con il calcolo massivo?')) {
		            document.getElementById('DivAttesa').style.display = '';
		            document.getElementById('btnCalcoloICI').click();
		        }
		        else {
		            return false;
		        }

		    }
		    //*** 20140509 - TASI ***
		    // funzione che apre le pagine di elaborazione dei documenti
		    function ApriStampaMassiva() {
		        var iframe = document.getElementById("loadGridRiepilogoCalcoloICI");

		        if (iframe == null) {
		            GestAlert('a', 'warning', '', '', 'Impossibile accedere alla funzionalità di estrazione elenco in formato excel.\n Il calcolo massivo non è stato eseguito.');
		        }
		        else {
		            var Tributo = '';
		            if (document.getElementById('chkICI').checked == true && document.getElementById('chkTASI').checked == false) {
		                Tributo = '8852';
		            }
            else if (document.getElementById('chkTASI').checked == true && document.getElementById('chkICI').checked == false) {
		                Tributo = 'TASI';
		            }
		            if (document.getElementById('rdbCalcoloNetto').checked == true) {
		                bNettoVersato = true;
		            }
		            else {
		                bNettoVersato = false;
		            }
		            parent.Basso.location.href = '../../aspVuota.aspx';
		            parent.Nascosto.location.href = '../../aspVuota.aspx';
		            parent.Comandi.location.href = '../ElaborazioneDocumenti/cmdElaborazioneDocumenti.aspx?AnnoRiferimento=' + document.getElementById('ddlAnnoRiferimento').value;
		            location.href = '../ElaborazioneDocumenti/ElaborazioneDocumenti.aspx?AnnoRiferimento=' + document.getElementById('ddlAnnoRiferimento').value + '&COD_TRIBUTO=' + Tributo + '&bNettoVersato=' + bNettoVersato + '&IdFlussoRuolo=' + document.getElementById('hdIdRuolo').value;
		        }
		    }

		    function ApriParametriEsclusione() {
		        var iframe = document.getElementById("loadGridRiepilogoCalcoloICI");

		        if (iframe == null) {
		            GestAlert('a', 'warning', '', '', 'Impossibile accedere alla funzionalità di estrazione elenco in formato excel.\n Il calcolo massivo non è stato eseguito.');
		        }
		        else {
		            var urlFinestra = 'PopEsclusione.aspx?optmassivo=1';
		            window.open(urlFinestra, 'fesclusione', 'top =' + (screen.height - 350) / 2 + ', left=' + (screen.width - 500) / 2 + ' width=500,height=350, status=yes, toolbar=no,scrollbar=no, resizable=no');
		        }
		    }

		    function ApriPostel() {
		        var iframe = document.getElementById("loadGridRiepilogoCalcoloICI");

		        if (iframe == null) {
		            GestAlert('a', 'warning', '', '', 'Impossibile accedere alla funzionalità di estrazione elenco in formato excel.\n Il calcolo massivo non è stato eseguito.');
		        }
		        else {
		            parent.Comandi.location.href = './../Postel/ComGestionePostel.aspx?ANNO=' + document.getElementById('ddlAnnoRiferimento').value;
		            parent.Visualizza.location.href = './../Postel/RicercaPostel.aspx?ANNO=' + document.getElementById('ddlAnnoRiferimento').value;
		        }
		    }

		    function estraiExcel() {
		        var iframe = document.getElementById("loadGridRiepilogoCalcoloICI");
		        var iframeDocument = iframe.contentDocument || iframe.contentWindow.document

		        var button = iframeDocument.getElementById("btnStampaExcel");

		        if (button == null) {
		            GestAlert('a', 'warning', '', '', 'Impossibile accedere alla funzionalità di estrazione elenco in formato excel.\n Il calcolo massivo non è stato eseguito.');
		        }
		        else {
		            document.getElementById('DivAttesa').style.display = '';
		            button.click()
		        }
		        return false;
		    }

		    function estraiExcelCatClasse() {
		        var iframe = document.getElementById("loadCalcoloCatVSCl");
		        var iframeDocument = iframe.contentDocument || iframe.contentWindow.document

		        var button = iframeDocument.getElementById("btnStampaExcel");

		        if (button == null) {
		            GestAlert('a', 'warning', '', '', 'Impossibile accedere alla funzionalità di estrazione elenco Categoria e Classe in formato excel.\n Il calcolo massivo non è stato eseguito.');
		        }
		        else {
		            button.click()
		        }

		        return false;
		    }
		    function estraimelo() {
		        //*** 20120704 - IMU ***
		        var iframe = document.getElementById('loadGridRiepilogoCalcoloICI');
		        var iframeDocument = iframe.contentDocument || iframe.contentWindow.document

		        var button = iframeDocument.getElementById("btnExtract");

		        if (button == null) {
		            GestAlert('a', 'warning', '', '', 'Impossibile accedere alla funzionalità di estrazione file TXT per stampa dei Bollettini.\n Il calcolo massivo non è stato eseguito.');
		        }
		        else {
		            document.getElementById('DivAttesa').style.display = '';
		            button.click()
		        }
		        return false;
		    }
		    //*** ***
		    function doIframe() {
		        var $iframes = $("iframe.autoHeight");
		        $iframes.each(function () {
		            var iframe = this;
		            $(iframe).load(function () {
		                setHeight(iframe);
		            });
		        });
		    }

		    function setHeight(e) {
		        e.height = e.contentWindow.document.body.scrollHeight + 35;
		    }

		    $(window).load(function () {
		        doIframe();
		    });		    
        </script>
	</head>
	<body class="Sfondo" leftMargin="0" topMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table id="tblContrib" width="100%">
				<tr><td>
            <div>
			    <table id="Table2" cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" class="SfondoGenerale">
				    <tr>
					    <td align="left" style="WIDTH: 641px"><span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							    <asp:Label id="lblTitolo" runat="server"></asp:Label></span></td>
					    <td align="right" width="800" colSpan="2" rowSpan="2">
						    <!--*** 20140509 - TASI ***--><!--*** 201511 - template documenti per ruolo ***-->
                            <input class="Bottone BottoneDownload" id="DownloadTemplate" title="Download Template" type="button" name="DownloadTemplate" onclick="document.getElementById('DivAttesa').style.display = '';document.getElementById('btnDownload').click();" />
                            <input class="Bottone BottoneUpload" id="Upload" title="Upload Template" type="button" name="UploadTemplate" onclick="document.getElementById('DivAttesa').style.display = '';document.getElementById('btnUpload').click();"/>
						    <input class="Bottone BottoneWord" id="btnStampaMassiva" title="Elabora i Documenti" type="button" name="btnStampaMassiva" onclick="parent.Visualizza.ApriStampaMassiva();">
						    <input class="Bottone BottoneCreaFile hidden" id="Extract" title="Estrazione File per Bollettini" onclick="parent.Visualizza.ApriParametriEsclusione();" type="button" name="Extract"> 
						    <input class="Bottone BottoneLettera hidden" id="ExtractPostel" title="Estrazione Tracciato Postel" onclick="parent.Visualizza.ApriPostel();" type="button" name="ExtractPostel"> 
						    <input class="Bottone BottoneExcel" id="Excel" title="Stampa Minuta Calcolo" onclick="parent.Visualizza.estraiExcel();" type="button" name="Excel"> 
						    <input class="Bottone BottoneStampaAlt" id="ExcelCatCl" title="Stampa Riepilogo Calcolo - Categoria e Classe" onclick="parent.Visualizza.estraiExcelCatClasse();" type="button" name="Excel">
						    <!--*** 20120704 - IMU ***-->
						    <input class="Bottone BottoneCalcolo" id="Insert" title="Effettua Calcolo Massivo" onclick="parent.Visualizza.Controlli();" type="button" name="Insert">
					    </td>
				    </tr>
				    <tr>
					    <td style="WIDTH: 641px" align="left"><span class="NormalBold_title" id="info" style="WIDTH: 580px; HEIGHT: 24px">Calcolo ICI/IMU/TASI - Calcolo Massivo</span></td>
				    </tr>
			    </table>
            </div></td></tr><tr><td>
            <div style="width:100%">
		        <fieldset class="FiledSetRicerca" style="width:100%">
		            <legend class="Legend">Selezionare un anno di riferimento per effettuare il Calcolo Massivo</legend>
		            <table style="WIDTH: 100%">
				        <tr>
					        <td class="Input_Label" align="left">Anno &nbsp;
						        <asp:dropdownlist id="ddlAnnoRiferimento" runat="server" CssClass="Input_Text" DataValueField="ANNO" DataTextField="ANNO" DataSource="<%# GetAnniAliquote() %>" Width="120px" AutoPostBack="True" onselectedindexchanged="ddlAnnoRiferimento_SelectedIndexChanged">
						        </asp:dropdownlist>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <br/>
                                <!--*** 201511 - template documenti per ruolo ***-->
                                <asp:Label ID="lblUploadFile" runat="server" Text="Template" CssClass="Input_Label"></asp:Label>&nbsp;
                                <asp:FileUpload ID="fileUpload" runat="server" CssClass="Input_Text" Width="400px" />
                                <asp:RequiredFieldValidator ID="rfvFileUpload" runat="server" ControlToValidate="fileUpload" ErrorMessage="Seleziona il file da importare" ValidationGroup="UploadValidation"></asp:RequiredFieldValidator>
                                <br />
                                <asp:Label ID="lblMessage" runat="server" Text="" Visible="false" CssClass="ERRORSTYLE"></asp:Label>
					        </td>
                            <!--*** 20140509 - TASI ***-->
					        <td>
					            <asp:CheckBox ID="chkICI" runat="server" CssClass="Input_Label" Text="ICI/IMU" Checked="true" AutoPostBack="true" oncheckedchanged="Tributo_CheckedChanged"/>
					            <br />
					            <asp:CheckBox ID="chkTASI" runat="server" CssClass="Input_Label" Text="TASI" Checked="false" AutoPostBack="true" oncheckedchanged="Tributo_CheckedChanged"/>
					        </td>
					        <!--*** ***-->
					        <!--*** 20150430 - TASI Inquilino ***-->
					        <td>
					            <asp:RadioButton ID="optTASIProp" runat="server" CssClass="Input_Radio" Text="inquilino mancante su proprietario" Checked="true" GroupName="TASInoInquilino" />
					            <br />
					            <asp:RadioButton ID="optTASINo" runat="server" CssClass="Input_Radio" Text="quota inquilino a perdere" Checked="false" GroupName="TASInoInquilino" />
					        </td>
					        <!--*** ***-->
					        <td class="hidden">
						        <asp:CheckBox id="chkVersatoNelDovuto" runat="server" CssClass="Input_Label" Text="Versato dell'Anno precedente ribaltato nel dovuto"></asp:CheckBox>&nbsp;&nbsp;
						        <br />
						        <asp:CheckBox id="chkArrotondamento" runat="server" CssClass="Input_Label" Text="Calcola Arrotondamento"></asp:CheckBox>
					        </td>
					        <td class="hidden">
						        <asp:radiobutton id="rdbStandard" runat="server" CssClass="Input_Label" Text="Calcolo standard" Checked="True" GroupName="OptTipoCalcolo"></asp:radiobutton>
						        <br />
						        <asp:radiobutton id="rdbCalcoloNetto" Enabled="false" runat="server" CssClass="Input_Label" Text="Calcolo al netto del Versato" GroupName="OptTipoCalcolo"></asp:radiobutton>
						    </td>
				        </tr>
						<tr>
							<td class="ERRORSTYLE">
								<br>N.B.
							</td>
				        </tr>
						<tr>
							<td class="ERRORSTYLE">
								L'elaborazione viene fatta in background pertanto, dopo aver lanciato l'esecuzione, si può procedere con altre operazioni da menù.
							</td>
						</tr>
		            </table>
		        </fieldset>
			    <table id="tblContrib" style="WIDTH: 100%">
				    <tr>
				        <td>
				            <div id="ParamElabDoc" runat="server">
				                <table width="100%">
			                        <tr>
			                            <td align="center">
			                            </td>
			                        </tr>
				                </table>
				            </div>
                           <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                                <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                                <div class="BottoneClessidra">&nbsp;</div>
                                <div class="Legend">Attendere Prego</div>
                            </div>
                           <div id="DivLoading" runat="server" style="z-index: 101; position: absolute;display:none;">
                                <div class="Legend" style="margin-top:40px;">Caricamento Dati in Corso...</div>
                                <div class="BottoneClessidra">&nbsp;</div>
                                <div class="Legend">Attendere Prego</div>
                            </div>
				        </td>
				    </tr>
				    <tr>
					    <td>
					        <asp:label id="Label1" runat="server" CssClass="lstTabRow" Width="100%">Elenco Aliquote Configurate</asp:label><br/>
					    </td>
				    </tr>
				    <tr>
					    <td>
					        <iframe id="loadGridAliquote" name="loadGridAliquote" frameborder="0" width="100%" height="250px" scrolling="auto" runat="server" src="../aspvuota.aspx"></iframe>
					    </td>
				    </tr>
				    <tr>
					    <td>
						    <iframe id="loadGridRiepilogoCalcoloICI" name="loadGridRiepilogoCalcoloICI" frameborder="0" width="100%" height="150px" runat="server" src="../aspvuota.aspx"></iframe>
					    </td>
				    </tr>
                    <!--*** 20140509 - TASI ***-->
				    <tr>
					    <td>
					        <iframe id="loadCalcoloCatVSCl" name="loadCalcoloCatVSCl" frameborder="0" width="100%" height="500px" scrolling="auto" runat="server" src="../aspvuota.aspx"></iframe>
					    </td>
				    </tr>
				    <tr>
					    <td>
						    <br/>
						    <asp:label id="lblLinkCalcolo" runat="server" style="display:none;" CssClass="Input_Label_bold" Width="100%" Font-Underline="True">Visualizza Riepilogo Immobili e Importi Calcolo</asp:label>
					    </td>
				    </tr>
                    <!--*** ***-->
			    </table>
                <asp:HiddenField ID="hdIdRuolo" runat="server" Value="-1" />
            </div>
			</td></tr></table>
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
            <asp:button id="btnCalcoloICI" style="DISPLAY: none" runat="server"  onclick="btnCalcoloICI_Click"></asp:button>
            <asp:Button ID="btnDownload" style="display:none" runat="server" OnClick="BtnDownloadClick" />
            <asp:Button ID="btnUpload" style="display:none" runat="server" OnClick="BtnUploadClick" ValidationGroup="UploadValidation"/>
		</form>
	</body>
</HTML>
