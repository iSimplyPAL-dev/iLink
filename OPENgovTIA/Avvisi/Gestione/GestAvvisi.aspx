<%@ Page Language="vb" AutoEventWireup="false" Codebehind="GestAvvisi.aspx.vb" Inherits="OPENgovTIA.GestAvvisi" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>GestAvvisi</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%        End If%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		    function LoadArticolo(IdArticolo, AzioneProv, TipoPartita,Provenienza) {
    		    parent.Comandi.location.href = '../../../aspVuotaRemoveComandi.aspx';
    		    parent.Basso.location.href = '../../../aspVuotaRemoveComandi.aspx';
    		    parent.Nascosto.location.href = '../../../aspVuotaRemoveComandi.aspx';
				location.href = 'GestPF.aspx?IdUniqueIdArticolo=' + IdArticolo + '&TipoPartita=' + TipoPartita + '&AzioneProv=' + AzioneProv + '&IsBloccato=' + document.getElementById('TxtIsBloccato').value + '&Provenienza=' + Provenienza
		    }
		    function EnableNewPartita() {
		        //document.getElementById('LnkNewPF').disabled=false;
		        //document.getElementById('LnkNewPC').disabled=false;
                $('#LnkNewPF').removeClass('DisableBtn');
                $('#LnkNewPC').removeClass('DisableBtn');
		    }
		    function DisableNewPartita() {
		        parent.parent.Comandi.document.getElementById('BloccaSgravio').disabled=true;
		        //document.getElementById('LnkNewPF').disabled=true;
		        //document.getElementById('LnkNewPC').disabled=true;
                $('#LnkNewPF').addClass('DisableBtn');
                $('#LnkNewPC').addClass('DisableBtn');
		    }
		</script>
	</head>
	<body class="Sfondo" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
        <div class="SfondoGenerale" style="width: 100%; height: 45px; overflow: hidden">
            <table cellspacing="0" cellpadding="0" width="100%" align="right" border="0" id="Table1">
                <tr valign="top">
                    <td align="left">
                        <span class="ContentHead_Title" id="infoEnte" style="width: 400px">
                            <asp:Label ID="lblTitolo" runat="server"></asp:Label>
                        </span>
                    </td>
                    <td align="right" rowspan="2">
						<INPUT class="Bottone BottoneWord" id="ElaborazioneDocumenti" title="Elaborazione Documenti" onclick="DivAttesa.style.display=''; document.getElementById('divStampa').style.display = ''; document.getElementById('divAvviso').style.display = 'none'; document.getElementById('CmdStampaDoc').click()" type="button" name="ElaborazioneDocumenti">
						<INPUT class="Bottone BottoneCancella" id="Delete" title="Elimina Sgravio" onclick="getElementById('CmdDeleteSgravi').click()" type="button" name="Delete"> 
						<INPUT class="Bottone BottoneAttivaSgravi" id="BloccaSgravio" title="Attiva\Disattiva Sgravio." onclick="<%if Session("BloccoSgravio")=-1 or Session("BloccoSgravio") Is Nothing then%>if (confirm('Si vuole attivare la procedura di Sgravio?')){getElementById('CmdSgravi').click()}<%else%>DivAttesa.style.display='';getElementById('CmdSgravi').click()<%end if%>" type="button" name="Modifica">  
						<INPUT class="Bottone BottoneSalva" style="DISPLAY:none" id="Salva" title="Salva Avviso" onclick="CheckDatiAvviso(0)" type="button" name="Salva">
						<INPUT class="Bottone BottoneAnnulla" id="Cancel" title="Torna alla ricerca" onclick="document.getElementById('CmdGoBack').click();" type="button" name="Cancel">
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <span class="NormalBold_title" id="info" runat="server" style="width: 400px">- Avvisi - Gestione</span>
                    </td>
                </tr>
            </table>
        </div>
        &nbsp;
		<div class="col-md-12">
			<table width="100%">
			    <tr>
				    <td colspan="2" class="hidden">
				        <asp:Label ID="Label13" CssClass="lstTabRow" runat="server">Tipologia Calcolo</asp:Label><br />
				        <fieldset id="Fieldset1" class="classeFieldSetRicerca" runat="server">
				            <table>
				                <tr>
				                    <td>
				                        <asp:Label ID="Label16" runat="server" CssClass="Input_Label">Tipo Calcolo</asp:Label>&nbsp;
				                        <asp:Label runat="server" CssClass="Input_Label" ID="LblTipoCalcolo"></asp:Label>
				                    </td>
				                    <td>
				                        <asp:CheckBox ID="ChkConferimenti" runat="server" CssClass="Input_Label" Text="presenza Conferimenti" />
				                    </td>
				                    <td>
				                        <asp:CheckBox ID="ChkMaggiorazione" runat="server" CssClass="Input_Label" text="presenza Maggiorazione"/>
				                    </td>
				                    <td>
				                        <asp:Label ID="Label18" runat="server" CssClass="Input_Label">Tipo Superfici</asp:Label>&nbsp;
				                        <asp:Label runat="server" CssClass="Input_Label" ID="LblTipoMQ"></asp:Label>
				                    </td>
				                </tr>
				            </table>
				        </fieldset>
				    </td>
			    </tr>
				<!--blocco dati contribuente--><%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
				<tr id="TRPlainAnag">
				    <td colspan="2">
				        <iframe id="ifrmAnag" runat="server" src="../../../aspVuota.aspx" style="height:200px; width:100%" frameborder="0" marginheight="0"></iframe>
				        <asp:HiddenField id="hdIdContribuente" runat="server" Value="-1" />
				    </td>
				</tr>
				<tr id="TRSpecAnag">
					<td colspan="2" width="100%">
						<iframe id="LoadAnagrafica" src="../../../aspVuota.aspx" frameBorder="0" width="100%" height="100" runat="server" style="Z-INDEX: 0"></iframe>
					</td>
				</tr>
				<tr>
					<td align="right" colspan="2">
						<asp:label id="LblSgravio" Font-Bold="True" CssClass="DettagliContribuente" Runat="server" Width="100%" Visible="False">(*) Sgravato</asp:label>
					</td>
				</tr>
				<!--blocco dati avviso e pagamenti-->
				<tr>
					<td><asp:label id="lblIntestAvviso" Width="100%" CssClass="lstTabRow" Runat="server">Dati Avviso</asp:label></td>
					<td><asp:label id="Label17" Width="100%" CssClass="lstTabRow" Runat="server">Dati Pagamenti</asp:label></td>
				</tr>
				<tr>
					<td>
						<table id="TblDatiAvviso" cellSpacing="0" cellPadding="0" width="100%" border="1">
							<tr>
								<td borderColor="darkblue">
									<table id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="0">
										<tr>
											<td class="DettagliContribuente" colspan="2">
												<asp:label id="LblDatiAvviso" runat="server" CssClass="DettagliContribuente">Label</asp:label>
											</td>
										</tr>
										<tr>
											<td class="DettagliContribuente">
												<asp:label id="Label10" runat="server" CssClass="DettagliContribuente">Emesso</asp:label>
											</td>
											<td class="DettagliContribuente" align="right"">
												<asp:label id="LblEmesso" runat="server" CssClass="DettagliContribuente">Label</asp:label>
											</td>
										</tr>
										<tr>
											<td class="DettagliContribuente">
												<asp:label id="Label11" runat="server" CssClass="DettagliContribuente">Pagato</asp:label>
											</td>
											<td class="DettagliContribuente" align="right"">
												<asp:label id="LblPagato" runat="server" CssClass="DettagliContribuente">Label</asp:label>
											</td>
										</tr>
										<tr>
											<td class="DettagliContribuente">
												<asp:label id="Label12" runat="server" CssClass="DettagliContribuente">Saldo</asp:label>
											</td>
											<td class="DettagliContribuente" align="right"">
												<asp:label id="LblSaldo" runat="server" CssClass="DettagliContribuente">Label</asp:label>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
		            <td valign="top">
			            <asp:label id="LblResultPagamenti" Runat="server" CssClass="Legend">Non sono presenti Pagamenti</asp:label>
                        <Grd:RibesGridView ID="GrdPagamenti" runat="server" BorderStyle="None" 
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
								<asp:BoundField DataField="sProvenienza" HeaderText="Provenienza"></asp:BoundField>
								<asp:TemplateField HeaderText="Data Pagamento">
									<ItemStyle Width="50px"></ItemStyle>
									<ItemTemplate>
										<asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataPagamento"))%>' ID="Label3">
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="dImportoPagamento" HeaderText="Imp." DataFormatString="{0:N}">
									<ItemStyle Width="50px" HorizontalAlign="Right"></ItemStyle>
								</asp:BoundField>
							</Columns>
						</Grd:RibesGridView>
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
                    <td colspan="2">
                        <div id="divAvviso">
                            <table class="col-md-12">
				                <!--Blocco Dati Tessere-->
				                <tr>
					                <td colspan="2">
					                    <div id="DivTessere" runat="server">
						                    <asp:Label CssClass="lstTabRow" Runat="server" id="Label23">Dati Parte Conferimenti</asp:Label>
						                    <asp:imagebutton id="LnkNewPV" runat="server" ImageUrl="../../../images/Bottoni/Listasel.png" ToolTip="Nuova Parte Conferimenti"
							                    CausesValidation="False" imagealign="Bottom" style="DISPLAY: none"></asp:imagebutton>&nbsp;
						                    <asp:Label CssClass="lstTabRow" Runat="server" id="Label7" Width="590px">&nbsp;</asp:Label>
						                    <asp:textbox id="TxtPV" Runat="server" style="DISPLAY: none" Width="10px">-1</asp:textbox><br />
						                    <asp:Label CssClass="Legend" Runat="server" ID="LblResultTessere">Non sono presenti Tessere</asp:Label><br />
                                            <Grd:RibesGridView ID="GrdTessere" runat="server" BorderStyle="None" 
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
								                    <asp:BoundField DataField="sNumeroTessera" HeaderText="N.Tessera">
									                    <ItemStyle Width="80px"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="sCodUtente" HeaderText="N.Utente">
									                    <ItemStyle Width="80px"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:TemplateField HeaderText="Data Rilascio">
									                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label id="Label34" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataRilascio")) %>'>
										                    </asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:TemplateField HeaderText="Data Cessazione">
									                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
									                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label id="Label25" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataCessazione")) %>'>
										                    </asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:TemplateField HeaderText="N.Conferimenti">
									                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label runat="server" text='<%# FncGrd.FormattaConferimenti("N", DataBinder.Eval(Container, "DataItem.oPesature")) %>' id="Label8">
										                    </asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:TemplateField HeaderText="Tot.Volume">
									                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:Label runat="server" text='<%# FncGrd.FormattaConferimenti("P", DataBinder.Eval(Container, "DataItem.oPesature")) %>' id="Label9">
										                    </asp:Label>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:TemplateField HeaderText="Agev.">
									                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
									                    <ItemTemplate>
										                    <asp:ImageButton id="Imagebutton1" runat="server" Height="15px" Width="15px" CommandName="Edit" ImageUrl='<%# FncGrd.FormattaRidDet(DataBinder.Eval(Container, "DataItem.impRiduzione")) %>'>
										                    </asp:ImageButton>
									                    </ItemTemplate>
								                    </asp:TemplateField>
								                    <asp:TemplateField HeaderText="">
									                    <headerstyle horizontalalign="Center"></headerstyle>
									                    <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									                    <itemtemplate>
										                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>' alt=""></asp:ImageButton>
					                                        <asp:HiddenField runat="server" ID="hfIDTESTATA" Value='<%# Eval("IDTESTATA") %>' />
                                                            <asp:HiddenField runat="server" ID="hfIDTESSERA" Value='<%# Eval("IDTESSERA") %>' />
									                    </itemtemplate>
								                    </asp:TemplateField>
							                    </Columns>
						                    </Grd:RibesGridView>
					                    </div>
					                </td>
				                </tr>
				                <!--Dati Scaglioni-->
				                <tr>
					                <td colspan="2">
					                    <div id="DivScaglioni" runat="server">
						                    <asp:Label CssClass="lstTabRow" Runat="server" id="Label6" Width="100%">Dati Scaglioni</asp:Label><br />
						                    <asp:label id="LblResultScaglioni" CssClass="Legend" Runat="server">Non sono presenti Scaglioni</asp:label><br />
                                            <Grd:RibesGridView ID="GrdScaglioni" runat="server" BorderStyle="None" 
                                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                                <PagerSettings Position="Bottom"></PagerSettings>
                                                <PagerStyle CssClass="CartListFooter" />
                                                <RowStyle CssClass="CartListItem"></RowStyle>
                                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							                    <Columns>
								                    <asp:BoundField DataField="nDa" HeaderText="Da">
									                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="nA" HeaderText="A">
									                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="impTariffa" HeaderText="Tariffa " DataFormatString="{0:0.000000}">
									                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="impMinimo" HeaderText="Minimo " DataFormatString="{0:N}">
									                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="nAliquota" HeaderText="Aliquota" DataFormatString="{0:N}">
									                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="nQuantita" HeaderText="Consumo">
									                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                    </asp:BoundField>
								                    <asp:BoundField DataField="impScaglione" HeaderText="Importo " DataFormatString="{0:N}">
									                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                    </asp:BoundField>
							                    </Columns>
						                    </Grd:RibesGridView>
					                    </div>
					                </td>
				                </tr>
				                <!--Dati Articoli-->
				                <tr>
					                <td colspan="2">
						                <asp:Label CssClass="lstTabRow" Runat="server" id="Label1">Dati Parte Fissa</asp:Label>&nbsp;
						                <asp:imagebutton id="LnkNewPF" runat="server" ToolTip="Nuovo Immobile" CausesValidation="False" imagealign="Bottom" ImageUrl="../../../images/Bottoni/nuovoinserisci.png" Height="15px" Width="15px"></asp:imagebutton>&nbsp;
						                <asp:imagebutton CssClass="hidden" id="LnkNewPC" runat="server" ToolTip="Nuovo Conferimento" CausesValidation="False" imagealign="Bottom" ImageUrl="../../../images/Bottoni/payment-card.png" Width="15px" Height="15px"></asp:imagebutton>&nbsp;
						                <asp:Label CssClass="lstTabRow" Runat="server" id="Label5" Width="605px">&nbsp;</asp:Label>
						                <asp:textbox id="TxtPF" Runat="server" style="DISPLAY: none" Width="10px">-1</asp:textbox>
					                </td>
				                </tr>
				                <tr>
					                <td colspan="2">
						                <asp:label id="LblResultArticoli" Runat="server" CssClass="Legend">Non sono presenti Articoli</asp:label>
                                        <Grd:RibesGridView ID="GrdArticoli" runat="server" BorderStyle="None" 
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
								                <asp:TemplateField HeaderText="Ubicazione (Rif.Catastali)">
									                <ItemStyle Width="300px"></ItemStyle>
									                <ItemTemplate>
										                <asp:Label id="Label13" runat="server" text='<%# FncGrd.FormattaVia(DataBinder.Eval(Container, "DataItem.sVia"), DataBinder.Eval(Container, "DataItem.scivico"), DataBinder.Eval(Container, "DataItem.sInterno"), DataBinder.Eval(Container, "DataItem.sEsponente"), DataBinder.Eval(Container, "DataItem.sScala"), DataBinder.Eval(Container, "DataItem.sfoglio"), DataBinder.Eval(Container, "DataItem.snumero"), DataBinder.Eval(Container, "DataItem.ssubalterno")) %>'>Label</asp:Label>
									                </ItemTemplate>
								                </asp:TemplateField>
								                <asp:TemplateField HeaderText="Cat.">
									                <ItemStyle Width="250px"></ItemStyle>
									                <ItemTemplate>
										                <asp:Label runat="server" Text='<%# FncGrd.FormattaCategoria(DataBinder.Eval(Container, "DataItem.sCategoria"), DataBinder.Eval(Container, "DataItem.sDescrCategoria"))%>' ID="Label2">
										                </asp:Label>
									                </ItemTemplate>
								                </asp:TemplateField>
								                <asp:BoundField DataField="impTariffa" HeaderText="Tariffa " DataFormatString="{0:0.000000}">
									                <ItemStyle Width="50px" HorizontalAlign="Right"></ItemStyle>
								                </asp:BoundField>
								                <asp:BoundField DataField="Nmq" HeaderText="MQ" DataFormatString="{0:N}">
									                <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                </asp:BoundField>
								                <asp:BoundField DataField="Nbimestri" HeaderText="Tempo">
									                <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                </asp:BoundField>
								                <asp:BoundField DataField="impNetto" HeaderText="Imp." DataFormatString="{0:N}">
									                <ItemStyle Width="50px" HorizontalAlign="Right"></ItemStyle>
								                </asp:BoundField>
								                <asp:TemplateField HeaderText="Rid.">
									                <ItemStyle HorizontalAlign="Center"></ItemStyle>
									                <ItemTemplate>
                                                        <div class="tooltip">
										                <asp:ImageButton id="Imagebutton2" runat="server" Height="15px" Width="15px" ImageUrl='<%# FncGrd.FormattaRidDet(DataBinder.Eval(Container, "DataItem.impRiduzione")) %>'></asp:ImageButton>
                                                        <span class="tooltiptext" style="width: 250px;"><%# FncGrd.FormattaToolTipRidDet(DataBinder.Eval(Container, "DataItem.oRiduzioni")) %></span></div>
									                </ItemTemplate>
								                </asp:TemplateField>
								                <asp:TemplateField HeaderText="Esenz.">
									                <ItemStyle HorizontalAlign="Center"></ItemStyle>
									                <ItemTemplate>
										                <asp:ImageButton id="Imagebutton3" runat="server" Height="15px" Width="15px" ImageUrl='<%# FncGrd.FormattaRidDet(DataBinder.Eval(Container, "DataItem.impDetassazione")) %>'>
										                </asp:ImageButton>
									                </ItemTemplate>
								                </asp:TemplateField>
								                <asp:TemplateField HeaderText="">
									                <headerstyle horizontalalign="Center"></headerstyle>
									                <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									                <itemtemplate>
										                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("Id") %>' alt=""></asp:ImageButton>
                                                        <asp:HiddenField runat="server" ID="hfid" Value='<%# Eval("Id") %>' />
                                                        <asp:HiddenField runat="server" ID="hfIdContribuente" Value='<%# Eval("IdContribuente") %>' />
                                                        <asp:HiddenField runat="server" ID="hfTipoPartita" Value='<%# Eval("TipoPartita") %>' />
									                </itemtemplate>
								                </asp:TemplateField>
							                </Columns>
						                </Grd:RibesGridView>
						                <br />
					                </td>
				                </tr>
				                <!--Dati Dettaglio Avviso e Rate-->
				                <tr>
					                <td><asp:label id="Label20" Width="100%" CssClass="lstTabRow" Runat="server">Dati Dettaglio Avviso</asp:label></td>
					                <td><asp:label id="Label4" Width="100%" CssClass="lstTabRow" Runat="server">Dati Rate</asp:label></td>
				                </tr>
				                <tr>
					                <td valign="top">
						                <asp:label id="LblResultDettVoci" Runat="server" CssClass="Legend">Non è presente il Dettaglio dell'Avviso</asp:label>
                                        <Grd:RibesGridView ID="GrdDettaglioVoci" runat="server" BorderStyle="None" 
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
								                <asp:BoundField DataField="sDescrizione" HeaderText="Descrizione">
									                <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                </asp:BoundField>
								                <asp:BoundField DataField="impDettaglio" HeaderText="Importo " DataFormatString="{0:N}">
									                <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                </asp:BoundField>
							                </Columns>
						                </Grd:RibesGridView>
					                </td>
					                <td valign="top">
						                <asp:label id="LblResultRate" Runat="server" CssClass="Legend">Non sono presenti Rate</asp:label>
                                        <Grd:RibesGridView ID="GrdRate" runat="server" BorderStyle="None" 
                                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                            <PagerSettings Position="Bottom"></PagerSettings>
                                            <PagerStyle CssClass="CartListFooter" />
                                            <RowStyle CssClass="CartListItem"></RowStyle>
                                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							                <Columns>
								                <asp:BoundField DataField="sNRata" HeaderText="N.Rata">
									                <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                </asp:BoundField>
								                <asp:BoundField DataField="sDescrRata" HeaderText="Descrizione">
									                <ItemStyle HorizontalAlign="Left"></ItemStyle>
								                </asp:BoundField>
								                <asp:TemplateField HeaderText="Data Scadenza">
									                <ItemStyle Width="50px"></ItemStyle>
									                <ItemTemplate>
										                <asp:Label runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataScadenza"))%>' ID="Label3">
										                </asp:Label>
									                </ItemTemplate>
								                </asp:TemplateField>
								                <asp:BoundField DataField="impRata" HeaderText="Importo " DataFormatString="{0:N}">
									                <ItemStyle HorizontalAlign="Right"></ItemStyle>
								                </asp:BoundField>
							                </Columns>
						                </Grd:RibesGridView>
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
		<asp:TextBox ID="TxtIsBloccato" runat="server" CssClass="hidden">-1</asp:TextBox>
		<asp:button id="CmdSgravi" runat="server" CssClass="hidden"></asp:button>
		<asp:button id="CmdStampaDoc" runat="server" CssClass="hidden"></asp:button>
		<asp:button id="CmdDeleteSgravi" runat="server" CssClass="hidden"></asp:button>
        <asp:button id="CmdGoBack" runat="server" CssClass="hidden"></asp:button>
        <button id="CmdEnableNewPartita" class="hidden" onclick="EnableNewPartita();"></button>
        <button id="CmdDisableNewPartita" class="hidden" onclick="DisableNewPartita();"></button>
        <asp:button ID="CmdReloadPage" runat="server" CssClass="hidden" />
		</form>
	</body>
</HTML>
