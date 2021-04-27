<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RicercaDoc.aspx.vb" Inherits="OPENgovTIA.RicercaDoc" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>RicercaDoc</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function Search()
			{
			    document.getElementById('loadGrid').src = 'ResultDoc.aspx?NominativoDa=' + document.getElementById('txtNominativoDa').value + '&NominativoA=' + document.getElementById('txtNominativoA').value + '&CodiceCartella=' + document.getElementById('txtCodiceCartella').value + '&IdRuolo=' + document.getElementById('txtIdRuolo').value				
			}			
			function ElaborazioniDocumenti()
			{
			    if (confirm('Si vuole procedere con l\'elaborazione dei documenti?')) {
			        //loadGrid.DivAttesa.style.display='';
			        document.getElementById('DivAttesa').style.display = '';
			        document.getElementById('divStampa').style.display = '';
			        document.getElementById('divAvviso').style.display = 'none';
			        document.getElementById('CmdElaborazione').click()
			    }
			}
			function MinutaRate() {
			    if (confirm('Si vuole procedere con la stampa delle rate?'))
			        document.getElementById('CmdStampaMinutaRate').click()
			}
			function ApprovaDocumenti()
			{
				if (confirm('Si vuole procedere con l\'approvazione dell\'elaborazione dei documenti?'))
					document.getElementById('CmdApprovaDoc').click()
			}			
			function EliminaElaborazione()
			{
				if (confirm('Si vuole procedere con l\'eliminazione dei documenti effettivi già elaborati?'))
					document.getElementById('CmdEliminaDoc').click()
			}
			function ViewDocElab() {
			        document.getElementById('CmdViewDocElab').click()
			}
			function ConfermaUscita()
			{
				document.getElementById('CmdUscita').click()
			}			
			function ApriVisualizzaDocElaborati()
			{			
				// apro il popup di visualizzazione doc elaborati
				/*winWidth=980 
				winHeight=500 
				myleft=(screen.availWidth-winWidth)/2 
				mytop=(screen.availheight-winHeight)/2 - 40 
				WinPopDoc=window.open('ViewDocumentiElaborati.aspx','','width='+winWidth+',height='+winHeight+',top='+mytop+',left='+myleft+' status=yes, toolbar=no,scrollbar=no, resizable=no') */
			    document.getElementById('DivAttesa').style.display = 'none';
			    document.getElementById('divStampa').style.display = '';
			    document.getElementById('divAvviso').style.display = 'none';
			    document.getElementById('loadStampa').src = 'ViewDocElaborati.aspx?Provenienza=E';
			}			
			function BackToCalcolo(){
			    parent.Comandi.location.href = '../../../aspVuotaRemoveComandi.aspx'
				parent.Visualizza.location.href='../Calcolo/Calcolo.aspx'
			}
		</script>
	</head>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table style="Z-INDEX: 100; POSITION: absolute; TOP: 0px; LEFT: 0px" cellSpacing="0" cellPadding="0"
				width="100%" border="0">
				<tr>
				    <td>
				        <asp:Label ID="Label2" CssClass="lstTabRow" runat="server">Tipologia Calcolo</asp:Label><br />
				        <fieldset id="Fieldset1" class="classeFieldSetRicerca" runat="server">
				            <table>
				                <tr>
				                    <td>
				                        <asp:Label ID="Label3" runat="server" CssClass="Input_Label">Tipo Calcolo</asp:Label>&nbsp;
				                        <asp:Label runat="server" CssClass="Input_Label" ID="LblTipoCalcolo"></asp:Label>
				                    </td>
				                    <td>
				                        <asp:CheckBox ID="ChkConferimenti" runat="server" CssClass="Input_Label" Text="presenza Conferimenti" />
				                    </td>
				                    <td>
				                        <asp:CheckBox ID="ChkMaggiorazione" runat="server" CssClass="Input_Label" text="presenza Maggiorazione"/>
				                    </td>
				                    <td>
				                        <asp:Label ID="Label5" runat="server" CssClass="Input_Label">Tipo Superfici</asp:Label>&nbsp;
				                        <asp:Label runat="server" CssClass="Input_Label" ID="LblTipoMQ"></asp:Label>
				                    </td>
				                </tr>
				            </table>
				        </fieldset>
				    </td>
				</tr>
				<tr>
					<td>
						<table id="TblRicerca" cellSpacing="1" cellPadding="1" width="100%" border="0">
							<tr>
								<td style="HEIGHT: 70px">
									<table id="TblDatiContribuente" cellSpacing="0" cellPadding="0" width="100%" class="dati_anagrafe_tarsu_blu" border="1">
										<tr>
											<td>
												<table id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
													<tr>
														<td class="Input_Label" colSpan="6" height="20"><strong>DATI RUOLO</strong></td>
													</tr>
													<tr>
														<td class="DettagliContribuente" style="WIDTH: 61px" width="61">RUOLO
														</td>
														<td class="DettagliContribuente" style="WIDTH: 157px" width="157"><asp:label id="lblTipoRuolo" runat="server" CssClass="DettagliContribuente">Label</asp:label></td>
														<td class="DettagliContribuente" style="WIDTH: 152px" width="152">RELATIVO ALL'ANNO
														</td>
														<td class="DettagliContribuente" style="WIDTH: 78px"><asp:label id="lblAnnoRuolo" runat="server" CssClass="DettagliContribuente">Label</asp:label></td>
														<td class="DettagliContribuente" style="WIDTH: 152px">CARTELLATO IN DATA
														</td>
														<td class="DettagliContribuente"><asp:label id="lblDataCartellazione" runat="server" CssClass="DettagliContribuente">Label</asp:label></td>
													</tr>
													<tr>
														<td class="DettagliContribuente" colSpan="3">
															<asp:label id="lblDocElaborati" CssClass="DettagliContribuente" Runat="server" Width="100%">DOCUMENTI EFFETTIVI GIÀ ELABORATI:     </asp:label>
														</td>
														<td class="DettagliContribuente" colSpan="3">
															<asp:label id="lblDocDaElaborare" CssClass="DettagliContribuente" Runat="server" Width="100%">DOCUMENTI DA ELABORARE:     </asp:label>
														</td>
													</tr>
													<tr>
														<td colSpan="6">
															<asp:Label id="lblElaborazioniEffettuate" runat="server" CssClass="NormalRed"></asp:Label>
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
				<tr>
					<td>
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
                        <div id="divAvviso" class="col-md-12">
						    <table id="TblParametriRicerca" cellSpacing="1" cellPadding="1" width="100%" border="0">
							    <tr>
								    <td>
									    <fieldset class="FiledSetRicerca"><LEGEND class="Legend">Inserimento filtri di ricerca</LEGEND>
										    <table id="TblParametri" cellSpacing="1" cellPadding="1" width="100%" border="0">
											    <tr>
												    <td><asp:label id="Label7" CssClass="Input_Label" Runat="server">Nominativo Da</asp:label><BR>
													    <asp:textbox id="txtNominativoDa" runat="server" CssClass="Input_Text" Width="250px"></asp:textbox></td>
												    <td></td>
												    <td><asp:label id="Label4" CssClass="Input_Label" Runat="server">Nominativo A</asp:label><BR>
													    <asp:textbox id="txtNominativoA" runat="server" CssClass="Input_Text" Width="250px"></asp:textbox></td>
												    <td><asp:label id="Label1" CssClass="Input_Label" Runat="server">Codice Cartella</asp:label><BR>
													    <asp:textbox id="txtCodiceCartella" runat="server" CssClass="Input_Text" Width="250px"></asp:textbox></td>
											    </tr>
										    </table>
									    </fieldset>
								    </td>
							    </tr>
                                <tr>
					                <td width="100%" colSpan="5">
						                <iframe id="loadGrid" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="450"></iframe>
					                </td>
                                </tr>
						    </table>
                        </div>
                        <div id="divStampa" class="col-md-12 classeFiledSetRicerca" style="display:none;">
                            <iframe runat="server" id="loadStampa" name="loadStampa" frameborder="0" width="100%" height="250px" src="../../../aspvuota.aspx"></iframe>
                        </div>
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
			<asp:button id="CmdStampaMinutaRate" runat="server" CssClass="hidden"></asp:button>
			<asp:button id="CmdElaborazione" runat="server" CssClass="hidden"></asp:button>
			<asp:button id="CmdApprovaDoc" runat="server" CssClass="hidden"></asp:button>
			<asp:button id="CmdEliminaDoc" runat="server" CssClass="hidden"></asp:button>
			<asp:button id="CmdUscita" runat="server" CssClass="hidden"></asp:button>
            <asp:Button ID="CmdViewDocElab" runat="server" CssClass="hidden" />
			<asp:textbox id="txtIdRuolo" style="DISPLAY: none" runat="server" Width="136px" AutoPostBack="True"></asp:textbox>
		</form>
	</body>
</HTML>
