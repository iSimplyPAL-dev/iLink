<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="WucGestionePagamenti.ascx.cs" Inherits="OPENGovTOCO.Wuc.WucGestionePagamenti" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<div style="PADDING-BOTTOM: 5px; PADDING-LEFT: 5px; PADDING-RIGHT: 5px; HEIGHT: 600px; PADDING-TOP: 5px" class="sfondo">
	<table id="TblGenPag" cellspacing="0" cellpadding="0" border="0" width="100%">
		<tr>
			<td width="100%">
				<fieldset class="FiledSetRicerca">
					<legend class="Legend">
						Tipo Inserimento</legend>
					<!--Tipo Operazione-->
					<table id="tblTipoOperazione" width="100%">
						<tr>
							<td>
								<asp:radiobutton id="rdbDaCartellazione" runat="server" cssclass="Input_Label" groupname="Versamento"
									text="Da Cartellazione" onclick="switchSceltaInserimento('cartellazione');"></asp:radiobutton>
							</td>
							<td width="10%">
								&nbsp;
							</td>
							<td>
								<asp:radiobutton id="rdbDataEntry" runat="server" cssclass="Input_Label" groupname="Versamento" text="Data Entry Manuale"
									onclick="switchSceltaInserimento('dataEntry');"></asp:radiobutton>
							</td>
						</tr>
					</table>
				</fieldset>
			</td>
		</tr>
		<tr>
			<td style="HEIGHT: 13px">
			</td>
		</tr>
		<tr>
			<!--Anno del versamento-->
			<td width="100%">
				<fieldset class="FiledSetRicerca">
					<legend class="Legend">
						Inserimento Parametri di Ricerca Emesso</legend>
					<table id="TblTestata" width="100%">
						<tr>
							<td>
								<asp:label id="lblAnnoVersamento" width="129px" runat="server" cssclass="Input_Label">Anno</asp:label>
							</td>
							<td style="WIDTH: 118px">
								<asp:textbox id="txtAnnoVersamento" width="60" runat="server" cssclass="Input_Text" maxlength="4"></asp:textbox>
							</td>
							<td style="WIDTH: 95px">
								<asp:label id="lblNavviso" runat="server" cssclass="Input_Label">Num. Avviso</asp:label>
							</td>
							<td>
								<asp:textbox id="txtNAvviso" width="227px" runat="server" cssclass="Input_Text"></asp:textbox>
							</td>
						</tr>
					</table>
				</fieldset>
			</td>
		</tr>
		<tr>
			<td style="HEIGHT: 13px">
			</td>
		</tr>
		<!--blocco dati contribuente--><%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
		<tr id="TRPlainAnag">
		    <td colspan="2">
		        <iframe id="ifrmAnag" runat="server" src="../../aspVuotaRemoveComandi.aspx" style="height:200px; width:100%" frameborder="0" marginheight="0"></iframe>
		        <asp:HiddenField id="hdIdContribuente" runat="server" Value="-1" />
		    </td>
		</tr>
		<tr id="TRSpecAnag">
			<td>
				<div id="DivContribuente" runat="server">
					<table id="TblContribuente" cellspacing="0" cellpadding="0" border="0">
		            <tr>
			            <td width="100%">
				            <asp:label id="Label45" runat="server" cssclass="lstTabRow">Dati Contribuente</asp:label>
				            <asp:label id="Label32" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" runat="server">*</asp:label>&nbsp;
				            <asp:imagebutton id="LnkAnagAnater" runat="server" imagealign="Bottom" CausesValidation="False" ToolTip="Ricerca Anagrafica da Anater"
					            ImageUrl="../../images/Bottoni/Listasel.png" Visible="False"></asp:imagebutton>&nbsp;
				            <asp:imagebutton id="LnkAnagrafica" runat="server" imagealign="Bottom" causesvalidation="False" tooltip="Ricerca Anagrafica da Tributi"
					            imageurl="../../images/Bottoni/Listasel.png" OnClick="LnkAnagTributi_Click"></asp:imagebutton>&nbsp;
				            <asp:imagebutton id="LnkPulisciContr" runat="server" imagealign="Bottom" causesvalidation="False"
					            OnClick="LnkPulisciContr_Click" tooltip="Pulisci i campi Contribuente" imageurl="../../images/Bottoni/cancel.png"></asp:imagebutton>
				            <asp:label id="Label26" width="576px" runat="server" cssclass="lstTabRow">&nbsp;</asp:label>
			            </td>
		            </tr>
						<!--prima riga-->
						<tr>
							<td width="280">
								<asp:label id="Label8" runat="server" cssclass="Input_Label">Cod.Fiscale</asp:label>
								<br>
								<asp:textbox id="TxtCodFiscale" width="185px" runat="server" cssclass="Input_Text" ReadOnly="True"
									Enabled="False"></asp:textbox>
							</td>
							<td width="275" colspan="3">
								<asp:label id="Label9" runat="server" cssclass="Input_Label">Partita Iva</asp:label>
								<br>
								<asp:textbox id="TxtPIva" width="140px" runat="server" cssclass="Input_Text" ReadOnly="True"
									Enabled="False"></asp:textbox>
							</td>
							<td>
								<asp:label id="Label10" runat="server" cssclass="Input_Label">Sesso</asp:label>
								<br>
								<asp:radiobutton id="F" runat="server" cssclass="Input_Label" groupname="Sesso" text="F" Enabled="False"></asp:radiobutton>
								<asp:radiobutton id="M" runat="server" cssclass="Input_Label" groupname="Sesso" text="M" Enabled="False"></asp:radiobutton>
								<asp:radiobutton id="G" runat="server" cssclass="Input_Label" groupname="Sesso" text="G" Enabled="False"></asp:radiobutton>
							</td>
							<td>
								<asp:textbox id="TxtIdDataAnagrafica" width="10px" runat="server" visible="False" ReadOnly="True" Enabled="False">-1</asp:textbox>
							</td>
						</tr>
						<!--seconda riga-->
						<tr>
							<td width="280">
								<asp:label id="Label11" runat="server" cssclass="Input_Label">Cognome/Rag.Soc</asp:label>
								<br>
								<asp:textbox id="TxtCognome" width="265px" runat="server" cssclass="Input_Text" ReadOnly="True"
									Enabled="False"></asp:textbox>
							</td>
							<td width="275" colspan="3">
								<asp:label id="Label12" runat="server" cssclass="Input_Label">Nome</asp:label>
								<br>
								<asp:textbox id="TxtNome" width="230px" runat="server" cssclass="Input_Text" ReadOnly="True"
									Enabled="False"></asp:textbox>
							</td>
						</tr>
						<!--terza riga-->
						<tr>
							<td width="280">
								<asp:label id="Label13" runat="server" cssclass="Input_Label">Data Nascita</asp:label>
								<br>
								<asp:textbox id="TxtDataNascita" style="TEXT-ALIGN: right" width="90px" runat="server" cssclass="Input_Text_Right"
									ReadOnly="True" Enabled="False"></asp:textbox>
							</td>
							<td width="275" colspan="3">
								<asp:label id="Label14" runat="server" cssclass="Input_Label">Luogo Nascita</asp:label>
								<br>
								<asp:textbox id="TxtLuogoNascita" width="250px" runat="server" cssclass="Input_Text" ReadOnly="True"
									Enabled="False"></asp:textbox>
							</td>
						</tr>
						<!--quarta riga-->
						<tr>
							<td width="280">
								<asp:label id="Label15" runat="server" cssclass="Input_Label">Via</asp:label>
								<br>
								<asp:textbox id="TxtResVia" width="265px" runat="server" cssclass="Input_Text" ReadOnly="True"
									Enabled="False"></asp:textbox>
							</td>
							<td>
								<asp:label id="Label16" runat="server" cssclass="Input_Label">Civico</asp:label>
								<br>
								<asp:textbox id="TxtResCivico" width="50px" runat="server" cssclass="Input_Text_Right" ReadOnly="True"
									Enabled="False"></asp:textbox>
							</td>
							<td>
								<asp:label id="Label17" runat="server" cssclass="Input_Label">Esponente</asp:label>
								<br>
								<asp:textbox id="TxtResEsponente" width="50px" runat="server" cssclass="Input_Text" ReadOnly="True"
									Enabled="False"></asp:textbox>
							</td>
							<td width="70">
								<asp:label id="Label18" runat="server" cssclass="Input_Label">Interno</asp:label>
								<br>
								<asp:textbox id="TxtResInterno" width="50px" runat="server" cssclass="Input_Text" ReadOnly="True"
									Enabled="False"></asp:textbox>
							</td>
							<td>
								<asp:label id="Label19" runat="server" cssclass="Input_Label">Scala</asp:label>
								<br>
								<asp:textbox id="TxtResScala" width="50px" runat="server" cssclass="Input_Text" ReadOnly="True"
									Enabled="False"></asp:textbox>
							</td>
						</tr>
						<!--quinta riga-->
						<tr>
							<td width="280">
								<asp:label id="Label20" runat="server" cssclass="Input_Label">CAP</asp:label>
								<br>
								<asp:textbox id="TxtResCAP" width="80px" runat="server" cssclass="Input_Text_Right" ReadOnly="True"
									Enabled="False"></asp:textbox>
							</td>
							<td width="275" colspan="3">
								<asp:label id="Label21" runat="server" cssclass="Input_Label">Comune</asp:label>
								<br>
								<asp:textbox id="TxtResComune" width="250px" runat="server" cssclass="Input_Text" ReadOnly="True"
									Enabled="False"></asp:textbox>
							</td>
							<td>
								<asp:label id="Label22" runat="server" cssclass="Input_Label">Provincia</asp:label>
								<br>
								<asp:textbox id="TxtResPv" width="50px" runat="server" cssclass="Input_Text" ReadOnly="True"
									Enabled="False"></asp:textbox>
							</td>
						</tr>
					</table>
				</div>
			</td>
		</tr>
		<tr>
			<td>
				&nbsp;
			</td>
		</tr>
		<tr>
			<td width="100%">
				<asp:label id="Label1" runat="server" cssclass="lstTabRow">Dati Versamento</asp:label>
				<asp:label id="Label2" width="642px" runat="server" cssclass="lstTabRow">&nbsp;</asp:label>
			</td>
		</tr>
		<tr>
			<td>
				<div id="divCartellazione" style="display:<%= displayCartellazione %>">
					<asp:Label ID="lblRisultato" runat="server" CssClass="Input_Label">Dati Cartellazione</asp:Label>
					<Grd:RibesGridView ID="GrdRate" runat="server" BorderStyle="None" 
                          BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                          AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                          ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                          <PagerSettings Position="Bottom"></PagerSettings>
                          <PagerStyle CssClass="CartListFooter" />
                          <RowStyle CssClass="CartListItem"></RowStyle>
                          <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                          <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
						<columns>
							<asp:TemplateField HeaderText="Sel *">
								<ItemTemplate>
									<asp:CheckBox id="ckbSelezione" runat="server"></asp:CheckBox>
                                    <asp:HiddenField runat="server" ID="hfCodiceCartella" Value='<%# Eval("CodiceCartella") %>' />
                                    <asp:HiddenField runat="server" ID="hfNumeroRata" Value='<%# Eval("NumeroRata") %>' />
                                    <asp:HiddenField runat="server" ID="hfAnno" Value='<%# Eval("Anno") %>' />
                                    <asp:HiddenField runat="server" ID="hfImportoRata" Value='<%# Eval("ImportoRata") %>' />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:BoundField DataField="Anno" HeaderText="Anno"></asp:BoundField>
							<asp:BoundField DataField="CodiceCartella" HeaderText="N. Avviso"></asp:BoundField>
							<asp:BoundField DataField="NumeroRata" HeaderText="N. Rata">
								<ItemStyle HorizontalAlign="Center"></ItemStyle>
							</asp:BoundField>
							<asp:BoundField DataField="ImportoRata" HeaderText="Imp. Rata &amp;#8364" DataFormatString="{0:0.00}">
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
							</asp:BoundField>
							<asp:TemplateField HeaderText="Scadenza">
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
								<ItemTemplate>
									<asp:Label runat="server" Text='<%# FormattaDataGrd((DateTime)DataBinder.Eval(Container, "DataItem.DataScadenza")) %>' ID="Label6" NAME="Label1">
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Data Pagamento *">
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
								<ItemTemplate>
									<asp:TextBox runat="server" id="txtDataPagamento" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate" Text='<%# FormattaDataGrd((DateTime)DataBinder.Eval(Container, "DataItem.DataPagamento")) %>'></asp:TextBox>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Data Riversamento">
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
								<ItemTemplate>
									<asp:TextBox runat="server" id="txtDataAccredito" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate" Text='<%# FormattaDataGrd((DateTime)DataBinder.Eval(Container, "DataItem.DataAccredito")) %>'></asp:TextBox>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Imp. Pagato &amp;#8364*">
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
								<ItemTemplate>
									<asp:TextBox id="txtTotalePagamento" onkeypress="return NumbersOnly(event, true, false, 2);" runat="server" Width="100px" CssClass="Input_Text_Right OnlyNumber" Text="0">
									</asp:TextBox>
								</ItemTemplate>
							</asp:TemplateField>
						</columns>
						</Grd:RibesGridView>
				</div>
			</td>
		</tr>
		<tr>
			<td>
				<div id="divDataEntry" style="WIDTH: 100%" style="display:<%= displayDataEntry %>">
					<table cellspacing="0" cellpadding="0" border="0">
						<tr>
							<td width="15%">
								<asp:label id="lblAnno" runat="server" cssclass="Input_Label">Anno</asp:label>
								<asp:label id="Label3" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" runat="server">*</asp:label>
								<br>
								<asp:textbox id="TxtAnno" width="60" runat="server" cssclass="Input_Text" maxlength="4"></asp:textbox>
							</td>
							<td width="27%">
								<asp:label id="lblNumAvvisoDE" runat="server" cssclass="Input_Label">N. Avviso</asp:label>
								<asp:label id="Label7" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" runat="server">*</asp:label>
								<br>
								<asp:textbox id="txtNAvvisoDE" width="150" runat="server" cssclass="Input_Text"></asp:textbox>
							</td>
							<td width="15%">
								<asp:label id="lblNRata" runat="server" cssclass="Input_Label">N. Rata</asp:label>
								<br>
								<asp:textbox id="txtNRata" width="60" runat="server" cssclass="Input_Text" maxlength="2"></asp:textbox>
							</td>
							<td width="20%">
								<asp:label id="lblDataPag" runat="server" cssclass="Input_Label">Data Pagamento</asp:label>
								<asp:label id="Label4" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" runat="server">*</asp:label>
								<br>
								<asp:textbox runat="server" id="txtDataPag" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" cssclass="Input_Text_Right TextDate" maxlength="10"></asp:textbox>
							</td>
							<td width="20%">
								<asp:label id="lblDataAccredito" runat="server" cssclass="Input_Label">Data Riversamento</asp:label>
								<br>
								<asp:textbox runat="server" id="txtDataAccreditoDE" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate" maxlength="10"></asp:textbox>
							</td>
							<td width="27%">
								<asp:label id="lblImportoPag" runat="server" cssclass="Input_Label">Importo Pagato</asp:label>
								<asp:label id="Label5" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" runat="server">*</asp:label>
								<br>
								<asp:textbox onkeypress="return NumbersOnly(event, true, false, 2);" id="txtImportoPag" width="120" runat="server" cssclass="Input_Text_Right OnlyNumber"></asp:textbox>
							</td>
						</tr>
					</table>
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
	<asp:textbox id="TxtIdPag" style="DISPLAY: none" runat="server">-1</asp:textbox>
	<asp:textbox id="TxtIdPagamento" style="DISPLAY: none" runat="server">-1</asp:textbox>
	<asp:textbox id="TxtCodiceCartellaUpd" style="DISPLAY: none" runat="server">-1</asp:textbox>
	<asp:textbox id="TxtImportoPagatoUpd" style="DISPLAY: none" runat="server">0</asp:textbox>
	<asp:button id="btnSalvaDati" style="DISPLAY: none" runat="server"></asp:button>
	<asp:button id="btnClearDatiPag" style="DISPLAY: none" runat="server"></asp:button>
	<asp:button id="btnModPagamenti" style="DISPLAY: none" runat="server"></asp:button>
	<asp:button id="btnCancellaPag" style="DISPLAY: none" runat="server"></asp:button>
	<asp:button id="btnTrovaRate" style="DISPLAY: none" runat="server"></asp:button>
</div>
<script type="text/javascript">
	var rdbDataEntry = '<%= rdbDataEntry.ClientID %>';
	var rdbDaCartellazione = '<%= rdbDaCartellazione.ClientID  %>';
		
	function unlockPagamento()
	{
		document.getElementById('<%= txtDataPag.ClientID %>').removeAttribute("disabled");
		document.getElementById('<%= txtDataAccreditoDE.ClientID %>').removeAttribute("disabled");
		document.getElementById('<%= txtNRata.ClientID %>').removeAttribute("disabled");
		document.getElementById('<%= txtImportoPag.ClientID %>').removeAttribute("disabled");
		
		unlockPagamentoBtn();
	}		
			
</script>
