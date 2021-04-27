<%@ Page language="c#" Codebehind="CalcoloICIPuntuale.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CalcoloICI.CalcoloICIPuntuale" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CalcoloICIPuntuale</title>
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
        <script type="text/javascript" src="../../_js/Utility.js?newversion"></script>
		<script type="text/javascript" src="../../_js/ControlloData.js?newversion"></script>
		<script type="text/javascript">		
		    function ApriRicercaAnagrafeCalcoloIci(nomeSessione) {
		        var winWidth = 980;
		        var winHeight = 680;
		        var myleft = (screen.width - winWidth) / 2;
		        var mytop = (screen.height - winHeight) / 2 - 40;

		        Parametri = "sessionName=" + nomeSessione;
		        WinPopUpRicercaAnagrafica = window.open('../../Anagrafica/FrameRicercaAnagraficaGenerale.aspx?' + Parametri, '', 'width=' + winWidth + ',height=' + winHeight + ', status=yes, toolbar=no,top=' + mytop + ',left=' + myleft + ',Fullscreen=no, channelmode=no, location=no, directories=no, menubar=no, scrollbar=no, resizable=no');
		    }
		    function gotoDichImmContribuente()
			{
		        if (document.getElementById('txtCodContribuenteCon').value == '')
				{
					//*** 20120704 - IMU ***
				    GestAlert('a', 'warning', '', '', 'Selezionare un contribuente per il quale visualizzare la situazione!');
					return false;
				}
				
		    COD_CONTRIB=document.getElementById('txtCodContribuenteCon').value;
				
				//alert(COD_CONTRIB);
												
				document.location.href="../GestioneDettaglio.aspx?IDContribuente="+COD_CONTRIB+"&Bonificato=-1";
				
			}	
		
			function LinkCalcoloICI(ANNO)
			{
			
			    ANNO=document.getElementById('ddlAnnoRiferimento').value
				if(ANNO=="")
				{
					//*** 20120704 - IMU ***
				    GestAlert('a', 'warning', '', '', 'Selezionare un anno per il quale visualizzare la situazione!');
					return false;
				}
							
				if(document.getElementById('txtCodContribuenteCon').value=='')				
				{
				    GestAlert('a', 'warning', '', '', 'Selezionare un contribuente per il quale visualizzare la situazione!');
					return false;
				}
				
			COD_CONTRIB=document.getElementById('txtCodContribuenteCon').value;
				
		        winWidth=990;
				winHeight=660;
				myleft=(screen.width-winWidth)/2
				mytop=(screen.height-winHeight)/2 -40;
								
				//*** 20130422 - aggiornamento IMU ****
				if (document.getElementById('rdbCalcoloNetto').checked==true)
				{
					bNettoVersato=true;
				}
				else
				{
					bNettoVersato=false;
				}
				//CalWin = window.open("GetRiepilogoICIframe.aspx?CODCONTRIB="+COD_CONTRIB+"&ANNO="+ANNO+"&blnCalcoloMassivo=false&ID_PROG_ELAB=-1","ShowCalcoloICI", "width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+", scrollbars=no,toolbar=no")
				CalWin = window.open("GetRiepilogoICIframe.aspx?CODCONTRIB="+COD_CONTRIB+"&ANNO="+ANNO+"&blnCalcoloMassivo=false&ID_PROG_ELAB=-1&bNettoVersato="+bNettoVersato,"ShowCalcoloICI", "width="+winWidth+",height="+winHeight+", status=yes, toolbar=no,top="+mytop+",left="+myleft+", scrollbars=no,toolbar=no")
				//*** ***
			}
			
			// richiamata dal frame dei comandi.
			// CCalcoloIciPuntuale
			function Controlli()
			{			
			    ANNO=document.getElementById('ddlAnnoRiferimento').value
				if(ANNO=="")
				{
					//*** 20120704 - IMU ***
				    GestAlert('a', 'warning', '', '', 'Selezionare un anno per il quale visualizzare la situazione!');
					return false;
				}
								
				if(document.getElementById('txtCodContribuenteCon').value=='')
				{
				    GestAlert('a', 'warning', '', '', 'Selezionare un contribuente per il quale effettuare il calcolo!');
					return false;
				}				
				
				if (confirm('Si vuole proseguire con il calcolo puntuale?'))
				{
				    DivAttesa.style.display = '';
				    document.getElementById('btnCalcoloICI').click();
				}								
				else
				{
					return false;
				}
            }
            
	        function ApriTipiRendita(){
		        var obj = document.getElementById('divTipiRendita');
		        if (obj.style.display == '') 
			        obj.style.display = 'none'
		        else
			        obj.style.display = ''        		
	        }

	        function ApriPopUpStampaDocumenti() {
	            //window.open('../ElaborazioneDocumenti/DownloadDocumenti.aspx', 'DownloadDoc', 'top =' + (screen.height - 550) / 2 + ', left=' + (screen.width - 500) / 2 + ' width=600,height=300, status=yes, toolbar=no,scrollbar=no, resizable=no');
	            document.getElementById('DivAttesa').style.display = 'none';
	            $('#divStampa').removeClass('hidden');
	            document.getElementById('DivCalcolo').style.display = 'none';
	            document.getElementById('loadStampa').src = '../ElaborazioneDocumenti/DownloadDocumenti.aspx?Provenienza=C';
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
	</HEAD>
	<body class="Sfondo" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td>
						<asp:label id="lblDatiContribuente" runat="server" CssClass="lstTabRow" Font-Bold="True">Dati Contribuente</asp:label>&nbsp;
						<asp:button id="btnRibalta" style="DISPLAY: none" runat="server" CausesValidation="False" Text="Button" onclick="btnRibalta_Click"></asp:button>
						<asp:imagebutton id="lnkVerificaContribuente" tabIndex="7" runat="server" CausesValidation="False" ImageUrl="../../images/Bottoni/Listasel.png" ToolTip="Verifica in Anagrafica" onclick="lnkVerificaContribuente_Click"></asp:imagebutton>
						<asp:requiredfieldvalidator id="rvalCodContrib" runat="server" Display="None" ControlToValidate="txtCodContribuenteCon1" ErrorMessage="Dati Contribuente obbligatori.">***</asp:requiredfieldvalidator>
						<input id="txtCodContribuenteCon" type="hidden" name="txtCodContribuenteCon" runat="server">
						<asp:textbox id="txtTypeOperation" tabIndex="28" runat="server" Text="" Visible="False" ReadOnly="True"></asp:textbox>
						<asp:textbox id="txtUpdateAnagraficaValue" style="DISPLAY: none" tabIndex="28" runat="server" Text="" ReadOnly="True"></asp:textbox>
						<asp:textbox id="txtCodContribuenteCon1" style="DISPLAY: none" tabIndex="28" runat="server" Text=""></asp:textbox>
					</td>
					<td align="left" colspan="5">
					    <asp:label ID="lblRiemp" CssClass="lstTabRow" Runat="server" Width="100%">&nbsp;</asp:label>
					</td>
				</tr>
				<tr>
					<td>
						<asp:label id="lblCodiceFiscaleContr" runat="server" CssClass="Input_Label">Codice Fiscale</asp:label><br />
						<asp:textbox id="txtCodFiscaleContr" runat="server" CssClass="Input_Text" Width="185px" Text="" ReadOnly="True" MaxLength="16" Enabled="False"></asp:textbox>
					</td>
					<td>
						<asp:label id="lblPartitaIVAContr" runat="server" CssClass="Input_Label">Partita IVA</asp:label><br />
						<asp:textbox id="txtPIVAContr" runat="server" CssClass="Input_Text" Width="140px" Text="" ReadOnly="True" MaxLength="11" Enabled="False"></asp:textbox>
					</td>
					<td>&nbsp;
						<asp:label id="lblSessoContr" runat="server" CssClass="Input_Label">Sesso</asp:label><br />
						<asp:radiobutton id="rdbMaschioContr" runat="server" CssClass="Input_Label" Text="M" Enabled="False"></asp:radiobutton>&nbsp;
						<asp:radiobutton id="rdbFemminaContr" runat="server" CssClass="Input_Label" Text="F" Enabled="False"></asp:radiobutton>&nbsp;
						<asp:radiobutton id="rdbGiuridicaContr" runat="server" CssClass="Input_Label" Text="G" Enabled="False"></asp:radiobutton>
					</td>
					<td>
						<asp:label id="lblDataNascitaContr" runat="server" CssClass="Input_Label">Data Nascita</asp:label><br />
						<asp:textbox id="txtDataNascContr" runat="server" CssClass="IInput_Text_Right TextDate" ReadOnly="True" MaxLength="10" Enabled="False"></asp:textbox>
					</td>
					<td>
						<asp:label id="lblComuneNascitaContr" runat="server" CssClass="Input_Label">Comune Nascita</asp:label><br />
						<asp:textbox id="txtComNascContr" runat="server" CssClass="Input_Text" Width="230px" Text="" ReadOnly="True" MaxLength="30" Enabled="False"></asp:textbox>
					</td>
					<td>
						<asp:label id="lblProvContr" runat="server" CssClass="Input_Label">Prov.</asp:label><br />
						<asp:textbox id="txtProvNascContr" runat="server" CssClass="Input_Text" Width="30px" Text="" ReadOnly="True" MaxLength="2" Enabled="False"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td colspan="3">
						<asp:label id="lblCognomeContr" runat="server" CssClass="Input_Label">Cognome/Rag.Soc</asp:label><br />
						<asp:textbox id="txtCognomeContr" runat="server" CssClass="Input_Text" Width="400px" Text="" ReadOnly="True" MaxLength="70" Enabled="False"></asp:textbox>
					</td>
					<td colspan="3">
						<asp:label id="lblNomeContr" runat="server" CssClass="Input_Label">Nome</asp:label><br />
						<asp:textbox id="txtNomeContr" runat="server" CssClass="Input_Text" Width="230px" Text="" ReadOnly="True" MaxLength="30" Enabled="False"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<asp:label id="lblViaContr" runat="server" CssClass="Input_Label">Via</asp:label><br />
						<asp:textbox id="txtViaResContr" runat="server" CssClass="Input_Text" Width="400px" Text="" ReadOnly="True" MaxLength="60" Enabled="False"></asp:textbox>
					</td>
					<td>
						<asp:label id="lblNumeroCivContr" runat="server" CssClass="Input_Label">Num. Civico</asp:label><br />
						<asp:textbox id="txtNumCivResContr" runat="server" CssClass="Input_Text" Width="65px" ReadOnly="True" Enabled="False"></asp:textbox>
					</td>
					<td>
						<asp:label id="lblEsponenteCivico" runat="server" CssClass="Input_Label">Esp. Civico</asp:label><br />
						<asp:textbox id="txtEsponenteCivico" runat="server" CssClass="Input_Text" Width="65px" Text="" ReadOnly="True" MaxLength="5" Enabled="False"></asp:textbox>
					</td>
					<td>
						<asp:label id="lblIntContr" runat="server" CssClass="Input_Label">Interno</asp:label><br />
						<asp:textbox id="txtIntResContr" runat="server" CssClass="Input_Text" Width="65px" Text="" ReadOnly="True" MaxLength="5" Enabled="False"></asp:textbox>
					</td>
					<td>
						<asp:label id="lblScalaContr" runat="server" CssClass="Input_Label">Scala</asp:label><br />
						<asp:textbox id="txtScalaResContr" runat="server" CssClass="Input_Text" Width="65px" Text="" ReadOnly="True" MaxLength="3" Enabled="False"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<asp:label id="lblComuneResidenzaContr" runat="server" CssClass="Input_Label">Comune Residenza</asp:label><br />
						<asp:textbox id="txtComuneResContr" runat="server" CssClass="Input_Text" Width="400px" Text="" ReadOnly="True" MaxLength="30" Enabled="False"></asp:textbox>
					</td>
					<td>
						<asp:label id="lblCAPContr" runat="server" CssClass="Input_Label">CAP</asp:label><br />
						<asp:textbox id="txtCapResContr" runat="server" CssClass="Input_Text" Width="65px" Text="" ReadOnly="True" MaxLength="5" Enabled="False"></asp:textbox>
					</td>
					<td>
						<asp:label id="lblProvinciaContr" runat="server" CssClass="Input_Label">Prov.</asp:label><br />
						<asp:textbox id="txtProvResContr" runat="server" CssClass="Input_Text" Width="30px" Text="" ReadOnly="True" MaxLength="2" Enabled="False"></asp:textbox>
					</td>
				</tr>
			</table>
			<fieldset class="FiledSetRicerca">
				<legend class="Legend">Selezionare un anno di riferimento ed un contribuente per effettuare il Calcolo Puntuale</legend>
				<table width="100%">
				    <tr>
					    <td class="Input_Label" align="left">Anno Riferimento<br/>
						    <asp:dropdownlist id="ddlAnnoRiferimento" runat="server" CssClass="Input_Text" DataValueField="ANNO" DataTextField="ANNO" DataSource="<%# GetAnniAliquote() %>" Width="120px" AutoPostBack="True" onselectedindexchanged="ddlAnnoRiferimento_SelectedIndexChanged">
						    </asp:dropdownlist>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					    </td>
                        <!--*** 20140509 - TASI ***-->
					    <td>
					        <asp:CheckBox ID="chkICI" runat="server" CssClass="Input_Label" Text="ICI/IMU" Checked="true" AutoPostBack="true" oncheckedchanged="Tributo_CheckedChanged"/>
					        <br />
					        <asp:CheckBox ID="chkTASI" runat="server" CssClass="Input_Label" Text="TASI" Checked="false" AutoPostBack="true" oncheckedchanged="Tributo_CheckedChanged"/>
					    </td>
					    <!--*** 20150430 - TASI Inquilino ***-->
					    <td>
					        <asp:RadioButton ID="optTASIProp" runat="server" CssClass="Input_Radio" Text="inquilino mancante su proprietario" Checked="true" GroupName="TASInoInquilino" />
					        <br />
					        <asp:RadioButton ID="optTASINo" runat="server" CssClass="Input_Radio" Text="quota inquilino a perdere" Checked="false" GroupName="TASInoInquilino" />
					    </td>
					    <!--*** ***-->
					    <!--*** ***-->
					    <td class="hidden disabled">
						    <asp:CheckBox id="chkVersatoNelDovuto" runat="server" CssClass="Input_Label" Text="Versato dell'Anno precedente ribaltato nel dovuto"></asp:CheckBox>&nbsp;&nbsp;
						    <br />
						    <asp:CheckBox id="chkArrotondamento" runat="server" CssClass="Input_Label" Text="Calcola Arrotondamento"></asp:CheckBox>
					    </td>
					    <td class="hidden">
						    <asp:radiobutton id="rdbStandard" runat="server" CssClass="Input_Label" Text="Calcolo standard" Checked="True" GroupName="OptTipoCalcolo"></asp:radiobutton>
						    <br />
						    <asp:radiobutton id="rdbCalcoloNetto" Enabled="false" runat="server" CssClass="Input_Label" Text="Calcolo al netto del Versato" GroupName="OptTipoCalcolo"></asp:radiobutton>
						    <asp:button id="btnCalcoloICI" style="DISPLAY: none" runat="server" Text="Button" onclick="btnCalcoloICI_Click"></asp:button>
					    </td>
				    </tr>
				</table>
			</fieldset>
			<div id="ParamElabDoc" runat="server" style="display:none">
				<table width="100%">
			        <tr>
			            <td>
			                <fieldset class="classeFiledSetRicerca" id="fldSelezione" style="TOP: 8px; LEFT: 16px">
			                    <legend class="Legend">Selezione tipologia di stampa</legend>
				                <table width="100%">
					                <tr>
						                <td valign="top">
						                    <asp:RadioButton id="radioTuttiBollettini" Runat="server" CssClass="Input_Label" Text="Standard Acconto/Saldo" AutoPostBack="false" GroupName="opzBollettini" Checked="True"></asp:RadioButton>
						                </td>
						                <td valign="top">
						                    <asp:RadioButton id="radioSoloAcconto" Runat="server" CssClass="Input_Label" Text="Solo Acconto" AutoPostBack="false" GroupName="opzBollettini" Checked="false"></asp:RadioButton>
						                </td>
						                <td valign="top">
						                    <asp:RadioButton id="radioSoloSaldo" Runat="server" CssClass="Input_Label" Text="Solo Saldo" AutoPostBack="false" GroupName="opzBollettini" Checked="false"></asp:RadioButton>
						                </td>
						                <td valign="top">
						                    <asp:RadioButton id="radioBollettiniSenzaImporti" Runat="server" CssClass="Input_Label" Text="Bollettini Senza Importi" AutoPostBack="false" GroupName="opzBollettini"></asp:RadioButton>
						                </td>
						                <td valign="top">
						                    <asp:RadioButton id="radioNoBollettini" Runat="server" CssClass="Input_Label" Text="Nessun Bollettino" AutoPostBack="false" GroupName="opzBollettini"></asp:RadioButton>
						                </td>
						                <td valign="top" class="hidden">
						                    <A class="Input_Label" href="javascript:ApriTipiRendita();">Tipi Rendita da Escludere »</A>
						                </td>						                            
					                </tr>
					                <tr class="hidden">
					                    <td colspan="5"></td>
					                    <td>
				                            <div id="divTipiRendita" style="DISPLAY: none">
							                    <Grd:RibesGridView ID="GrdTipoRendita" runat="server" BorderStyle="None" 
                                                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                    AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                                                    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                                    <PagerSettings Position="Bottom"></PagerSettings>
                                                    <PagerStyle CssClass="CartListFooter" />
                                                    <RowStyle CssClass="CartListItem"></RowStyle>
                                                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
									                <Columns>
										                <asp:BoundField DataField="SIGLA" HeaderText="Tipo Rendita">
											                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
											                <ItemStyle HorizontalAlign="Left" Width="100px" VerticalAlign="Middle"></ItemStyle>
										                </asp:BoundField>
										                <asp:BoundField DataField="COD_RENDITA" HeaderText="Cod Rendita" visible="false">
											                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
											                <ItemStyle HorizontalAlign="Left" Width="100px" VerticalAlign="Middle"></ItemStyle>
										                </asp:BoundField>
										                <asp:BoundField DataField="DESCRIZIONE" HeaderText="Tipo Rendita">
											                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
											                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
										                </asp:BoundField>
										                <asp:TemplateField HeaderText="Escludi">
											                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
											                <ItemStyle HorizontalAlign="Center" Width="50px" VerticalAlign="Middle"></ItemStyle>
											                <ItemTemplate>
												                <asp:CheckBox id="chkEsclusione" runat="server"></asp:CheckBox>
											                </ItemTemplate>
										                </asp:TemplateField>
									                </Columns>
									            </Grd:RibesGridView>
								            </div>
					                    </td>
					                </tr>
				                </table>
			                </fieldset>
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
            <div id="DivCalcolo" class="col-md-12">
                <div>
					<br />
					<a title="Visualizza Elenco Aliquote Configurate" onclick="nascondi(this,'DivAliquote','Elenco Aliquote Configurate')" href="#" class="lstTabRow">Visualizza Elenco Aliquote Configurate</a>
					<br/>
					<div id="DivAliquote" runat="server" style="display:none">
					    <iframe id="loadGridAliquote" name="loadGridAliquote" frameborder="0" width="99%" height="260px" runat="server" src="../../aspvuota.aspx"></iframe>
					</div>
					<br />
                </div>
                <div>
                    <iframe id="loadGridRiepilogoCalcoloICI" name="loadGridRiepilogoCalcoloICI" frameborder="0" width="100%" height="150px" runat="server" src="../../aspvuota.aspx"></iframe>
                </div>
                <div>
                    <iframe id="loadCalcoloCatVSCl" name="loadCalcoloCatVSCl" frameborder="0" width="100%" height="280px" runat="server" src="../../aspvuota.aspx"></iframe>
                </div>
                <div>
                    <iframe id="loadBollettino" name="loadBollettino" frameborder="0" width="100%" height="180px" runat="server" src="../../aspvuota.aspx"></iframe>
                </div>
                <div>
                    <iframe id="loadVersamenti" name="loadVersamenti" frameborder="0" width="100%" height="200px" runat="server" src="../../aspvuota.aspx"></iframe>
                </div>
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
			<asp:label id="lblLinkCalcolo" runat="server" style="display:none;" CssClass="Input_Label_bold" Width="100%" Font-Underline="True">Visualizza Riepilogo Immobili e Importi Calcolo</asp:label>
            <asp:Label ID="lblConfermato" Runat="server" CssClass="Input_Label" Font-Bold="true" ForeColor="#ff0000"></asp:Label>
			<asp:button id="btnStampaExcel" style="DISPLAY: none" runat="server" Text="Button" onclick="btnStampaExcel_Click"></asp:button>
			<asp:Button id="btnConfermaCalcolo" runat="server" Text="Button" Style="DISPLAY:none" onclick="btnConfermaCalcolo_Click"></asp:Button>
			<asp:button id="btnElaboraDoc" runat="server" Text="Elabora" style="DISPLAY:none" onclick="btnElaboraDoc_Click"></asp:button>
			<asp:button id="btnIndietro" style="DISPLAY: none" runat="server" Text="Indietro" CausesValidation="False" onclick="btnIndietro_Click"></asp:button>
		</form>
	</body>
</HTML>
