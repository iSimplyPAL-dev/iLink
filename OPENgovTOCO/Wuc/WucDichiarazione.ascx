<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="WucDichiarazione.ascx.cs" Inherits="OPENgovTOCO.Wuc.WucDichiarazione" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<script type="text/javascript" src="../_js/Toco.js?newversion"></script>
<table style="WIDTH: 100%" id="TblGenDich" border="0" cellSpacing="1" cellPadding="1">
	<!--blocco dati testata-->
	<tr>
		<td colSpan="5"><asp:label id="Label1" Width="100%" Runat="server" CssClass="lstTabRow">Dati Testata</asp:label></td>
	</tr>
	<tr>
		<td>
			<div id="DivTestata" runat="server">
				<table id="TblTestata" border="0" cellSpacing="0" cellPadding="0">
					<tr>
						<td width="170"><asp:label id="Label2" Runat="server" CssClass="Input_Label">Data Autoriz./Conces.</asp:label><asp:label style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" id="Label30" Runat="server">*</asp:label><br>
							<asp:textbox Runat="server" id="TxtDataDichiarazione" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate" maxlength="10"></asp:textbox></td>
						<td width="170"><asp:label id="Label3" Runat="server" CssClass="Input_Label">N.Autoriz./Conces.</asp:label><asp:label style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" id="Label31" Runat="server">*</asp:label><br>
							<asp:textbox id="TxtNDichiarazione" Runat="server" CssClass="Input_Text" MaxLength="6"></asp:textbox></td>
						<td width="130"><asp:label id="Label4" Runat="server" CssClass="Input_Label">Tipo Atto</asp:label><asp:label style="Z-INDEX: 0; FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" id="Label24"
								Runat="server">*</asp:label><br>
							<asp:dropdownlist id="cmbTipoAtto" Width="120px" Runat="server" CssClass="Input_Text">
								<asp:ListItem Value="-1">...</asp:ListItem>
								<asp:ListItem Value="1">AUTORIZZAZIONE</asp:ListItem>
								<asp:ListItem Value="2">CONCESSIONE</asp:ListItem>
							</asp:dropdownlist>
						</td>
						<td width="150"><asp:label id="Label5" Runat="server" CssClass="Input_Label">Titolo Richiedente</asp:label><asp:label style="Z-INDEX: 0; FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" id="Label25"
								Runat="server">*</asp:label><br>
							<asp:dropdownlist style="Z-INDEX: 0" id="cmbTitoloRichiedente" Width="136px" Runat="server" CssClass="Input_Text"></asp:dropdownlist></td>
						<td width="150">
							<asp:label id="Label6" Width="120px" Runat="server" CssClass="Input_Label">Ufficio Richiedente</asp:label>
							<asp:label id="Label34" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" Runat="server">*</asp:label><br>
							<asp:dropdownlist style="Z-INDEX: 0" id="cmbUffici" Width="136px" Runat="server" CssClass="Input_Text"></asp:dropdownlist></td>
					</tr>
				</table>
			</div>
		</td>
	</tr>
	<!--blocco dati contribuente--><%--*** 201504 - Nuova Gestione anagrafica con form unico ***--%>
	<tr id="TRPlainAnag">
	    <td>
	        <iframe id="ifrmAnag" runat="server" src="../../aspVuotaRemoveComandi.aspx" style="height:200px; width:100%" frameborder="0" marginheight="0"></iframe>
	        <asp:HiddenField id="hdIdContribuente" runat="server" Value="-1" />
	    </td>
	</tr>
	<tr id="TRSpecAnag">
		<td style="HEIGHT: 19px" width="100%">
			<div id="DivContribuente" runat="server">
				<table id="TblContribuente" border="0" cellSpacing="0" cellPadding="0">
	                <tr>
		                <td>
		                    <asp:label id="Label45" Runat="server" CssClass="lstTabRow">Dati Contribuente</asp:label>
		                    <asp:label style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" id="Label32" Runat="server">*</asp:label>
				                    <asp:imagebutton id="LnkAnagTributi" runat="server" imagealign="Bottom" CausesValidation="False"
				                    ToolTip="Ricerca Anagrafica da Tributi" ImageUrl="../../images/Bottoni/Listasel.png" OnClick="LnkAnagTributi_Click"></asp:imagebutton>&nbsp;
		                    <asp:imagebutton id="LnkAnagAnater" runat="server" imagealign="Bottom" CausesValidation="False" ToolTip="Ricerca Anagrafica da Anater"
				                    ImageUrl="../../images/Bottoni/Listasel.png" Visible="False"></asp:imagebutton>&nbsp;
		                    <asp:imagebutton id="LnkPulisciContr" runat="server" imagealign="Bottom" CausesValidation="False"
				                    ToolTip="Pulisci i campi Contribuente" ImageUrl="../../images/Bottoni/cancel.png" onclick="LnkPulisciContr_Click"></asp:imagebutton>
		                    <asp:label id="Label26" Width="555px" Runat="server" CssClass="lstTabRow">&nbsp;</asp:label>
                        </td>
	                </tr>
					<!--prima riga-->
					<tr>
						<td width="280"><asp:label id="Label8" Runat="server" CssClass="Input_Label">Cod.Fiscale</asp:label><br>
							<asp:textbox id="TxtCodFiscale" Width="185px" Runat="server" CssClass="Input_Text" ReadOnly="True"
								Enabled="False"></asp:textbox></td>
						<td width="275" colSpan="3"><asp:label id="Label9" Runat="server" CssClass="Input_Label">Partita Iva</asp:label><br>
							<asp:textbox id="TxtPIva" Width="140px" Runat="server" CssClass="Input_Text" ReadOnly="True"
								Enabled="False"></asp:textbox></td>
						<td><asp:label id="Label10" Runat="server" CssClass="Input_Label">Sesso</asp:label><br>
							<asp:radiobutton id="F" Runat="server" CssClass="Input_Label" GroupName="Sesso" Text="F" Enabled="False"></asp:radiobutton><asp:radiobutton id="M" Runat="server" CssClass="Input_Label" GroupName="Sesso" Text="M" Enabled="False"></asp:radiobutton><asp:radiobutton id="G" Runat="server" CssClass="Input_Label" GroupName="Sesso" Text="G" Enabled="False"></asp:radiobutton></td>
						<td><asp:textbox id="TxtIdDataAnagrafica" Width="10px" Runat="server" Visible="False">-1</asp:textbox><asp:button style="DISPLAY: none" id="btnRibaltaAnagAnater" Runat="server"></asp:button></td>
					</tr>
					<!--seconda riga-->
					<tr>
						<td width="280"><asp:label id="Label11" Runat="server" CssClass="Input_Label">Cognome/Rag.Soc</asp:label><br>
							<asp:textbox id="TxtCognome" Width="265px" Runat="server" CssClass="Input_Text" ReadOnly="True"
								Enabled="False"></asp:textbox></td>
						<td width="275" colSpan="3"><asp:label id="Label12" Runat="server" CssClass="Input_Label">Nome</asp:label><br>
							<asp:textbox id="TxtNome" Width="230px" Runat="server" CssClass="Input_Text" ReadOnly="True"
								Enabled="False"></asp:textbox></td>
					</tr>
					<!--terza riga-->
					<tr>
						<td width="280"><asp:label id="Label13" Runat="server" CssClass="Input_Label">Data Nascita</asp:label><br>
							<asp:textbox style="TEXT-ALIGN: right" id="TxtDataNascita" Width="90px" Runat="server" CssClass="Input_Text_Right"
								ReadOnly="True" Enabled="False"></asp:textbox></td>
						<td width="275" colSpan="3"><asp:label id="Label14" Runat="server" CssClass="Input_Label">Luogo Nascita</asp:label><br>
							<asp:textbox id="TxtLuogoNascita" Width="250px" Runat="server" CssClass="Input_Text" ReadOnly="True"
								Enabled="False"></asp:textbox></td>
					</tr>
					<!--quarta riga-->
					<tr>
						<td width="280"><asp:label id="Label15" Runat="server" CssClass="Input_Label">Via</asp:label><br>
							<asp:textbox id="TxtResVia" Width="265px" Runat="server" CssClass="Input_Text" ReadOnly="True"
								Enabled="False"></asp:textbox></td>
						<td><asp:label id="Label16" Runat="server" CssClass="Input_Label">Civico</asp:label><br>
							<asp:textbox id="TxtResCivico" Width="50px" Runat="server" CssClass="Input_Text_Right" ReadOnly="True"
								Enabled="False"></asp:textbox></td>
						<td><asp:label id="Label17" Runat="server" CssClass="Input_Label">Esponente</asp:label><br>
							<asp:textbox id="TxtResEsponente" Width="50px" Runat="server" CssClass="Input_Text" ReadOnly="True"
								Enabled="False"></asp:textbox></td>
						<td width="70"><asp:label id="Label18" Runat="server" CssClass="Input_Label">Interno</asp:label><br>
							<asp:textbox id="TxtResInterno" Width="50px" Runat="server" CssClass="Input_Text" ReadOnly="True"
								Enabled="False"></asp:textbox></td>
						<td><asp:label id="Label19" Runat="server" CssClass="Input_Label">Scala</asp:label><br>
							<asp:textbox id="TxtResScala" Width="50px" Runat="server" CssClass="Input_Text" ReadOnly="True"
								Enabled="False"></asp:textbox></td>
					</tr>
					<!--quinta riga-->
					<tr>
						<td width="280"><asp:label id="Label20" Runat="server" CssClass="Input_Label">CAP</asp:label><br>
							<asp:textbox id="TxtResCAP" Width="80px" Runat="server" CssClass="Input_Text_Right" ReadOnly="True"
								Enabled="False"></asp:textbox></td>
						<td width="275" colSpan="3"><asp:label id="Label21" Runat="server" CssClass="Input_Label">Comune</asp:label><br>
							<asp:textbox id="TxtResComune" Width="250px" Runat="server" CssClass="Input_Text" ReadOnly="True"
								Enabled="False"></asp:textbox></td>
						<td><asp:label id="Label22" Runat="server" CssClass="Input_Label">Provincia</asp:label><br>
							<asp:textbox id="TxtResPv" Width="50px" Runat="server" CssClass="Input_Text" ReadOnly="True"
								Enabled="False"></asp:textbox></td>
					</tr>
				</table>
			</div>
		</td>
	</tr>
	<tr>
		<td><br>
		</td>
	</tr>
	<!--Blocco Dati Articoli-->
	<tr>
		<td style="HEIGHT: 25px">
			<asp:textbox style="DISPLAY: none" id="TxtTessere" Width="10px" Runat="server">-1</asp:textbox>
			<asp:label style="Z-INDEX: 0" id="Label23" Runat="server" CssClass="lstTabRow">Articoli</asp:label>
			<asp:imagebutton style="Z-INDEX: 0" id="LnkNewUIAnater" runat="server" imagealign="Bottom" CausesValidation="False"
				ImageUrl="../../images/Bottoni/Listasel.png" Visible="False"></asp:imagebutton>&nbsp;
			<asp:imagebutton style="Z-INDEX: 0" id="lnkGestImmobili" runat="server" imagealign="Bottom" CausesValidation="False" ImageUrl="../../images/Bottoni/nuovoinserisci.png" Height="15px" Width="15px" onclick="lnkGestImmobili_Click"></asp:imagebutton>						
			<asp:label style="Z-INDEX: 0" id="Label7" Width="605px" Runat="server" CssClass="lstTabRow">&nbsp;</asp:label><br>
			<asp:label style="Z-INDEX: 0" id="LblResultImmobili" Runat="server" CssClass="Legend">Non sono presenti articoli</asp:label>			
			<!--<div style="overflow:auto; height:256px;width:764px;">-->			
			<Grd:RibesGridView ID="GrdArticoli" runat="server" BorderStyle="None" 
                      BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                      AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                      ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                      OnRowCommand="GrdRowCommand" OnRowDataBound="GrdRowDataBound">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
				<Columns>
					<asp:TemplateField HeaderText="Ubicazione (Rif.Catastali)">
						<ItemStyle Width="300px"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label35" runat="server" text='<%# OPENgovTOCO.SharedFunction.FormattaVia(DataBinder.Eval(Container, "DataItem.sVia"),DataBinder.Eval(Container, "DataItem.Civico"),DataBinder.Eval(Container, "DataItem.Interno"),DataBinder.Eval(Container, "DataItem.Esponente"),DataBinder.Eval(Container, "DataItem.Scala")) %>'>Label</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Data Inizio">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label29" runat="server" text='<%# OPENgovTOCO.SharedFunction.FormattaData(DataBinder.Eval(Container, "DataItem.DataInizioOccupazione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Data Fine">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" text='<%# OPENgovTOCO.SharedFunction.FormattaData(DataBinder.Eval(Container, "DataItem.DataFineOccupazione"))%>'></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Durata">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle Width="120px" HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label28" runat="server" text='<%# (DataBinder.Eval(Container, "DataItem.DurataOccupazione")) + " " + (DataBinder.Eval(Container, "DataItem.TipoDurata.Descrizione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Tipologia Occupazione">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle Width="300px" HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label27" runat="server" text='<%# (DataBinder.Eval(Container, "DataItem.TipologiaOccupazione.Descrizione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Consistenza">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label33" runat="server" text='<%# (DataBinder.Eval(Container, "DataItem.Consistenza")) + " " + (DataBinder.Eval(Container, "DataItem.TipoConsistenzaTOCO.Descrizione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="Note" HeaderText="Note">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle Width="200px"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="E">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<div class="tooltip">
                                <asp:ImageButton id="Imagebutton1" runat="server" CssClass="BottoneGrd BottoneApriGrd" CommandName="Mod" CommandArgument='<%# Eval("IdArticolo") %>' alt=""></asp:ImageButton>
                                <span class="tooltiptext">Modifica articolo</span>
                            </div>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="V">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
						    <div class="tooltip">
                                <asp:ImageButton id="imgView" runat="server" CssClass="BottoneGrd BottoneApriGrd" CommandName="Open" CommandArgument='<%# Eval("IdArticolo") %>' alt=""></asp:ImageButton>
                                <span class="tooltiptext">Visualizza articolo</span>
                            </div>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="D">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<div class="tooltip">
                                <asp:ImageButton id="imgDelete" runat="server" CssClass="BottoneGrd BottoneCancellaGrd" CommandName="Canc" CommandArgument='<%# Eval("IdArticolo") %>' alt=""></asp:ImageButton>
                                <span class="tooltiptext">Elimina Articolo da Dichiarazione</span>
                            </div>
	                        <asp:HiddenField runat="server" ID="hfDataInserimento" Value='<%# Eval("DataInserimento") %>' />
                            <asp:HiddenField runat="server" ID="hfIdArticolo" Value='<%# Eval("IdArticolo") %>' />
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
				</Grd:RibesGridView>
			
			<!--</div>-->
		</td>
	</tr>
	<tr>
		<td><br>
		</td>
	</tr>
	<!--Blocco Dati Provenienza-->
	<tr>
		<td><asp:label id="Label41" Width="100%" Runat="server" CssClass="lstTabRow">Note</asp:label></td>
	</tr>
	<tr>
		<td>
			<div id="DivNote" runat="server">
				<table id="TblNote" border="0" cellSpacing="0" cellPadding="0">
					<tr>
						<td><asp:textbox id="TxtNoteDich" Runat="server" CssClass="Input_Text" Height="32px" width="500px"
								MaxLength="4000"></asp:textbox></td>
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
<asp:button style="DISPLAY: none" id="CmdRibaltaUIAnater" Runat="server"></asp:button>
<asp:textbox style="DISPLAY: none" id="txtNumeroArticoli" Runat="server">-1</asp:textbox>
