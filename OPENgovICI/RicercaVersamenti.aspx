<%@ Page language="c#" Codebehind="RicercaVersamenti.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.RicercaVersamenti" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RicercaVersamenti</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1"){%> 
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%}%> 
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript" event="onkeypress" for="document">
			if(window.event.keyCode==13){
				//parent.Comandi.Form1.Insert.click();
			    document.getElementById('DivAttesa').style.display = '';
				document.getElementById('btnTrova').click();
			}
			
			function ControllaSelezione(){
				if (document.getElementById('ddlImportoPagato')==-1){
					document.getElementById('txtImportoPagato').style.visibility='none';
				}
			}
		</script>
		<script type="text/javascript">
			function Search() {
			    document.getElementById('DivAttesa').style.display = '';
				if (document.getElementById('TxtDataRiversamentoDa').value!='' && document.getElementById('TxtDataRiversamentoA').value=='')
				{
					GestAlert('a', 'warning', '', '', 'Valorizzare entrambe le date!');
					return false;
				}
				if (document.getElementById('TxtDataRiversamentoDa').value=='' && document.getElementById('TxtDataRiversamentoA').value!='')
				{
					GestAlert('a', 'warning', '', '', 'Valorizzare entrambe le date!');
					return false;
				}
				document.getElementById('btnTrova').click();
			}
		function estraiExcel()		
		{		
		    if (document.getElementById('GrdRisultati') == null)
			{
		        GestAlert('a', 'warning', '', '', 'Impossibile accedere alla funzionalità di estrazione elenco in formato excel.\n Eseguire la ricerca.');
			}
			else {
			    document.getElementById('DivAttesa').style.display = '';
			    document.getElementById('btnStampaExcel').click()
			}
			return false;				
		}
		</script>
	</HEAD>
	<body class="Sfondo" leftMargin="3" onload="document.getElementById('txtAnnoRiferimento').focus()"
		rightMargin="3" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table id="tblCorpo" cellSpacing="1" cellPadding="1" width="100%" align="center" border="0">
				<tr>
					<td>
						<fieldset class="classeFiledSetRicerca"><legend class="Legend">Inserimento parametri di ricerca</legend>
							<table id="tblFiltri" cellSpacing="1" cellPadding="5" width="100%" border="0">
								<tr>
									<td>
										<asp:Label runat="server" CssClass="Input_Label">Anno riferimento</asp:Label>
										<br />
										<asp:textbox id="txtAnnoRiferimento" runat="server" CssClass="Input_Text_Right OnlyNumber" MaxLength="4" Width="80px"></asp:textbox>
										<asp:button id="btnTrova" style="DISPLAY: none" runat="server" Text="Trova" ToolTip="Permette di eseguire una ricerca in funzione dei filtri utilizzati" onclick="btnTrova_Click"></asp:button>
										<asp:button id="btnNuovoVersamento" style="DISPLAY: none" runat="server" Text="Nuovo Versamento"
											CausesValidation="False" onclick="btnNuovoVersamento_Click"></asp:button>
										<asp:button id="btnRibalta" style="DISPLAY: none" runat="server" Text="Ribalta" CausesValidation="False"></asp:button>
									</td>
									<!--*** 20140630 - TASI ***-->
		                            <td>
		                                <asp:CheckBox ID="chkICI" runat="server" CssClass="Input_Label" Text="ICI/IMU" Checked="true"/>
		                                &nbsp;
		                                <asp:CheckBox ID="chkTASI" runat="server" CssClass="Input_Label" Text="TASI" Checked="false"/>
		                            </td>
									<!--*** ***-->
								</tr>
								<tr>
									<td>
										<label class="Input_Label">Cognome/Ragione Soc.</label>
										<br />
										<asp:textbox id="txtCognome" runat="server" CssClass="Input_Text" Width="280px"></asp:textbox>
									</td>
									<td>
										<label class="Input_Label">Nome</label>
										<br />
										<asp:textbox id="txtNome" runat="server" CssClass="Input_Text" Width="200px"></asp:textbox>
									</td>
									<td>
										<label class="Input_Label" for="txtCodiceFiscale">Codice Fiscale</label>
										<br />
										<asp:textbox id="txtCodiceFiscale" runat="server" CssClass="Input_Text" width="150px" MaxLength="16"></asp:textbox>
									</td>
									<td>
										<label class="Input_Label" for="txtPartitaIva">Partita IVA</label>
										<br />
										<asp:textbox id="txtPartitaIVA" runat="server" CssClass="Input_Text" width="100px" MaxLength="11"></asp:textbox>
									</td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td>
						<!--<p id="pRicerca" style="MARGIN-TOP: 0px; MARGIN-BOTTOM: 0px"><A class="linkRicercaAvanzata" id="linkAvanzate" style="FONT-SIZE: 11px" onclick="VisRicercaAvanzata();"
								href="#"><b>Visualizza Ricerca avanzata &gt;&gt;</b></A>
						</p>
						<br />-->
						<fieldset class="classeFiledSetRicerca" id="RicAvanzata">
							<legend class="Legend">Inserimento parametri di ricerca avanzata</legend>
							<table id="tblFiltriAvanzati" cellSpacing="1" cellPadding="5" width="100%" border="0">
								<tr>
									<td style="WIDTH: 272px" colspan="2">
										<label class="Input_Label" for="txtImportoPagato">Importo Pagato</label><br />
										<asp:DropDownList id="ddlImportoPagato" CssClass="Input_Text" Runat="server">
											<asp:ListItem Value="-1">[Nessun Importo]</asp:ListItem>
											<asp:ListItem Value="0">Maggiore di</asp:ListItem>
											<asp:ListItem Value="1">Minore di</asp:ListItem>
											<asp:ListItem Value="2">Uguale a</asp:ListItem>
										</asp:DropDownList>
										<asp:textbox id="txtImportoPagato" Width="100" CssClass="Input_Text_Right OnlyNumber" Runat="server" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:textbox>
									</td>
									<td style="WIDTH: 215px"><label class="Input_Label" for="ddlTipoPagamento">Tipo 
											Pagamento</label><br />
										<asp:dropdownlist id="ddlTipoPagamento" runat="server" CssClass="Input_Text">
											<asp:ListItem Value="-1">[TUTTI]</asp:ListItem>
											<asp:listitem Value="0">Acconto</asp:listitem>
											<asp:listitem Value="1">Saldo</asp:listitem>
											<asp:listitem Value="2">Unica Soluzione</asp:listitem>
										</asp:dropdownlist>
									</td>
									<td style="WIDTH: 215px"><label class="Input_Label" for="ddlTipologiaV">Tipologia</label><br />
										<asp:dropdownlist id="ddlTipologiaV" CssClass="Input_Text" Runat="server">
											<asp:ListItem Value="-1">[TUTTE]</asp:ListItem>
											<asp:ListItem Value="0">Ordinario</asp:ListItem>
											<asp:ListItem Value="1">Ravvedimento Operoso</asp:ListItem>
											<asp:ListItem Value="2">Violazione</asp:ListItem>
										</asp:dropdownlist>
									</td>
									<td>
										<label class="Input_Label">Data Riversamento</label>
										<br />
										<label class="Input_Label">Da</label>&nbsp;
                                        <asp:textbox id="TxtDataRiversamentoDa" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);TrackBlur(this);" tabIndex="16" runat="server" CssClass="Input_Text_Right TextDate" Text="" MaxLength="8"></asp:textbox>
										&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <label class="Input_Label">A</label>&nbsp;
                                        <asp:textbox id="TxtDataRiversamentoA" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);TrackBlur(this);" tabIndex="16" runat="server" CssClass="Input_Text_Right TextDate" Text="" MaxLength="8"></asp:textbox>
									</td>
								</tr>
                                <tr>
									<td colspan="4">
                                        <asp:label id="lblFlusso" CssClass="Input_Label" Runat="server">Flusso</asp:label><br />
										<asp:textbox id="txtFlusso" CssClass="Input_Text" Runat="server" Width="700px"></asp:textbox>
									</td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td colspan="6">
                       <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                            <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                            <div class="BottoneClessidra">&nbsp;</div>
                            <div class="Legend">Attendere Prego</div>
                        </div>
					</td>
				</tr>
				<tr>
					<td>
						&nbsp;<asp:label id="lblRisultati" runat="server" CssClass="Legend">Risultati della Ricerca</asp:label>
						<br />
					</td>
					<td align="right">
					    &emsp;&emsp;&emsp;<asp:Label ID="LblNPag" CssClass="Legend" runat="server"></asp:Label>
					    &emsp;&emsp;
					    <asp:Label ID="LblImpPag" CssClass="Legend" runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td colspan="2">
						<fieldset class="classeFiledSetNoBorder">
							<table id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
								<tr>
									<td>
                                        <Grd:RibesGridView ID="GrdRisultati" runat="server" BorderStyle="None" 
                                              BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                              AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                                              ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                              OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                                              <PagerSettings Position="Bottom"></PagerSettings>
                                              <PagerStyle CssClass="CartListFooter" />
                                              <RowStyle CssClass="CartListItem"></RowStyle>
                                              <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                              <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
											<Columns>
											    <asp:BoundField DataField="AnnoRiferimento" HeaderText="Anno" SortExpression="AnnoRiferimento">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
											    </asp:BoundField>
												<asp:TemplateField SortExpression="Cognome" HeaderText="Contribuente">
													<ItemTemplate>
														<asp:Label id=lblContribuente runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Cognome") + " " + DataBinder.Eval(Container, "DataItem.Nome") %>'>
														</asp:Label>
													</ItemTemplate>
												</asp:TemplateField>
					                            <asp:BoundField DataField="CFPIVA" HeaderText="Cod.Fiscale/P.IVA">
						                            <HeaderStyle HorizontalAlign="Center" Width="120px"></HeaderStyle>
						                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
					                            </asp:BoundField>
					                            <asp:BoundField DataField="DESCRTRIBUTO" HeaderText="Tributo">
						                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
					                            </asp:BoundField>
												<asp:TemplateField HeaderText="Imp. Pagato">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
													<ItemTemplate>
														<asp:label id="Label6" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ImportoPagato")) %>'>
														</asp:label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Acc./Saldo">
													<ItemTemplate>
														<asp:Label id=lblSaldo runat="server" Text='<%# Business.CoreUtility.FormattaGrdAccontoSaldo(DataBinder.Eval(Container, "DataItem.Saldo"), DataBinder.Eval(Container, "DataItem.Acconto")) %>'>
														</asp:Label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Data Pagam.">
                                                    <ItemStyle Width="80px"></ItemStyle>
													<ItemTemplate>
														<asp:Label runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataPagamento")) %>' ID="lblDataPagamento">
														</asp:Label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Data Rivers.">
                                                    <ItemStyle Width="80px"></ItemStyle>
													<ItemTemplate>
														<asp:Label runat="server" Text='<%# Business.CoreUtility.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.DataRiversamento")) %>' ID="Label7">
														</asp:Label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Imp. Abitaz. Principale">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
													<ItemTemplate>
														<asp:label id="Label4" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ImportoAbitazPrincipale")) %>'>
														</asp:label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Imp. Altri Fabbricati">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
													<ItemTemplate>
														<asp:label id="Label3" runat="server" Text='<%# Business.CoreUtility.FormattaGrdSumEuro(DataBinder.Eval(Container, "DataItem.ImportoAltriFabbric"),DataBinder.Eval(Container, "DataItem.IMPORTOALTRIFABBRICSTATALE")) %>'>
														</asp:label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Imp. Aree Fabbricabili">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
													<ItemTemplate>
														<asp:label id="Label1" runat="server" Text='<%# Business.CoreUtility.FormattaGrdSumEuro(DataBinder.Eval(Container, "DataItem.ImportoAreeFabbric"),DataBinder.Eval(Container, "DataItem.IMPORTOAREEFABBRICSTATALE")) %>'>
														</asp:label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Imp. Terreni">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
													<ItemTemplate>
														<asp:label id="Label2" runat="server" Text='<%# Business.CoreUtility.FormattaGrdSumEuro(DataBinder.Eval(Container, "DataItem.ImpoTerreni"),DataBinder.Eval(Container, "DataItem.IMPORTOTERRENISTATALE")) %>'>
														</asp:label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Imp. Fab.Rurali">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
													<ItemTemplate>
														<asp:label id="Label9" runat="server" Text='<%# Business.CoreUtility.FormattaGrdSumEuro(DataBinder.Eval(Container, "DataItem.IMPORTOFABRURUSOSTRUM"),DataBinder.Eval(Container, "DataItem.IMPORTOFABRURUSOSTRUMSTATALE")) %>'>
														</asp:label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Imp. Detrazione">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
													<ItemTemplate>
														<asp:label id="Label5" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.DetrazioneAbitazPrincipale")) %>'>
														</asp:label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField headertext="Violazione">
													<itemstyle horizontalalign="center"></itemstyle>
													<itemtemplate>
														<asp:label id="Label8" runat="server" text='<%# Business.CoreUtility.FormattaGrdViolazione(DataBinder.Eval(Container, "DataItem.Violazione")) %>'>
														</asp:label>
													</itemtemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="">
	                                                <headerstyle horizontalalign="Center"></headerstyle>
	                                                <ItemStyle horizontalalign="Center" Width="40px"></ItemStyle>
	                                                <itemtemplate>
	                                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>' alt=""></asp:ImageButton>
	                                                </itemtemplate>
                                                </asp:TemplateField>
											</Columns>
										</Grd:RibesGridView>
                                        <asp:button id="btnStampaExcel" style="DISPLAY: none" runat="server" Text="Button" onclick="btnStampaExcel_Click"></asp:button>
									</td>
								</tr>
							</table>
						</fieldset>
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
			<asp:Button id="btnIndietro" runat="server" Text="btnIndietro" style="DISPLAY:none" onclick="btnIndietro_Click"></asp:Button>
		</form>
	</body>
</HTML>
