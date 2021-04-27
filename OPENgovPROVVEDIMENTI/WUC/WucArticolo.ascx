<%@ Control Language="vb" AutoEventWireup="false" Codebehind="WucArticolo.ascx.vb" Inherits="Provvedimenti.WucArticolo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<%@ Register Src="./WucDatiContribuente.ascx" TagName="wucDatiContribuente" TagPrefix="uc1" %>
<table id="TblGenDich" style="WIDTH: 770px" cellSpacing="1" cellPadding="1" border="0">
	<!--blocco dati contribuente-->
	<tr>
		<td><uc1:wucdaticontribuente id="wucContribuente" runat="server"></uc1:wucdaticontribuente></td>
	</tr>
	<!--Blocco Dati UI-->
	<tr>
		<td>
			<table id="TblDati" width="100%">
				<tr>
					<td colSpan="9"><asp:label id="Label10" Runat="server" Width="100%" CssClass="lstTabRow">Dati Articolo</asp:label></td>
				</tr>
				<tr>
					<td colSpan="9">
						<asp:label id="Label3" Runat="server" CssClass="Input_Label">Via</asp:label><asp:label id="Label14" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" Runat="server">*</asp:label>&nbsp;
						<asp:imagebutton id="LnkOpenStradario" runat="server" ImageUrl="../../images/Bottoni/Listasel.png"
							imagealign="Bottom" CausesValidation="False" ToolTip="Ubicazione Immobile da Stradario."></asp:imagebutton>
						<br>
						<asp:textbox id="TxtVia" Runat="server" Width="700px" CssClass="Input_Text" ReadOnly="True"></asp:textbox>
                        <asp:textbox id="TxtCodVia" style="DISPLAY: none" Runat="server" CssClass="Input_Text"></asp:textbox>
						<asp:textbox id="TxtViaRibaltata" style="DISPLAY: none" Runat="server"></asp:textbox>
					</td>
				</tr>
				<tr>
					<td>
						<asp:label id="Label4" Runat="server" CssClass="Input_Label">Civico</asp:label><br>
						<asp:textbox id="TxtCivico" onblur="isNumber(this,'','','',999999)" Runat="server" Width="70px"
							CssClass="Input_Text_Right"></asp:textbox>
					</td>
					<td>
						<asp:label id="Label5" Runat="server" CssClass="Input_Label">Esponente</asp:label><br>
						<asp:textbox id="TxtEsponente" Runat="server" Width="70px" CssClass="Input_Text_Right"></asp:textbox>
					</td>
					<td>
						<asp:label id="Label6" Runat="server" CssClass="Input_Label">Interno</asp:label><br>
						<asp:textbox id="TxtInterno" Runat="server" Width="70px" CssClass="Input_Text_Right"></asp:textbox>
					</td>
					<td>
						<asp:label id="Label8" Runat="server" CssClass="Input_Label">Scala</asp:label><br>
						<asp:textbox id="TxtScala" Runat="server" Width="70px" CssClass="Input_Text_Right"></asp:textbox>
					</td>
					<td>
						<asp:label id="Label11" Runat="server" CssClass="Input_Label">Data Inizio</asp:label>
						<asp:label id="Label17" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" Runat="server">*</asp:label><br>
						<asp:textbox id="TxtDataInizio" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" Runat="server" CssClass="Input_Text_Right TextDate" maxlength="10"></asp:textbox>
					</td>
					<td>
						<asp:label id="Label19" Runat="server" CssClass="Input_Label">Tipo Durata</asp:label>
						<asp:label id="Label21" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" Runat="server">*</asp:label><br>
						<asp:dropdownlist id="cmbTipoDurata" Runat="server" CssClass="Input_Text"></asp:dropdownlist>
					</td>
					<td>
						<asp:label id="Label15" Runat="server" CssClass="Input_Label">Durata</asp:label>
						<asp:label id="Label16" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" Runat="server">*</asp:label><br>
						<asp:textbox id="txtDurata" onblur="isNumber(this,'','','',999999)" style="TEXT-ALIGN: right"
							Runat="server" Width="50px" CssClass="Input_Text" Enabled="true"></asp:textbox>
					</td>
					<td>
						<asp:label id="Label12" Runat="server" CssClass="Input_Label">Data fine</asp:label>
						<asp:label id="Label24" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" Runat="server">*</asp:label><br>
						<asp:textbox id="txtDataFine" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" Runat="server" CssClass="Input_Text_Right TextDate" maxlength="10"></asp:textbox>
					</td>
					<td>
						<br>
						<asp:CheckBox Runat="server" CssClass="Input_CheckBox_NoBorder" Text="Attrazione" id="chkAttrazione"></asp:CheckBox>
					</td>
				</tr>
				<tr>
					<td colspan="5">
						<asp:label id="Label13" Runat="server" CssClass="Input_Label">Tipologia Occupazione</asp:label>
						<asp:label id="Label23" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" Runat="server">*</asp:label><br>
						<asp:dropdownlist id="cmbTipologiaOccupazione" Runat="server" Width="460px" CssClass="Input_Text"></asp:dropdownlist>
					</td>
					<td>
						<asp:label id="Label18" Runat="server" CssClass="Input_Label"> Consistenza</asp:label>
						<asp:label id="Label22" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" Runat="server">*</asp:label><br>
						<asp:textbox id="txtConsistenza" onblur="isNumber(this,'',2,'',999999)" Runat="server" Width="100px"
							CssClass="Input_Text_Right"></asp:textbox>
					</td>
					<td>
						<br>
						<asp:dropdownlist id="cmbTipoConsistenza" Runat="server" Width="70px" CssClass="Input_Text"></asp:dropdownlist>
					</td>
					<td colspan="2">
						<asp:label id="Label20" Runat="server" CssClass="Input_Label">Categoria</asp:label>
						<asp:label id="Label9" style="FONT-FAMILY: Verdana; COLOR: red; FONT-SIZE: 11px" Runat="server">*</asp:label><br>
						<asp:dropdownlist id="cmbCategoria" Runat="server" Width="128px" CssClass="Input_Text"></asp:dropdownlist>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td>
			<table id="TblUnitaImmo" cellSpacing="0" cellPadding="0" border="0">
				<!--Blocco Dati Catastali-->
				<tr>
					<td>
						<table id="TblCatastali" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td colSpan="6"></td>
							</tr>
							<tr>
								<td><br>
								</td>
								<td><br>
								</td>
								<td><br>
								</td>
								<td><br>
								</td>
								<td><br>
								</td>
								<td></td>
							</tr>
						</table>
					</td>
				</tr>
				<!--Blocco Dati Riduzioni/Detassazioni-->
				<tr>
					<td>
						<table id="TblRidEse" cellSpacing="0" cellPadding="0" border="0">
							<tr>
								<td vAlign="top">
									<table style="WIDTH: 382px; HEIGHT: 28px">
										<tr>
											<td colspan="4">
												<asp:label id="Label33" Runat="server" Width="250px" CssClass="lstTabRow">Maggiorazione</asp:label>
											</td>
											<td>
												<asp:label id="Label2" Runat="server" Width="100px" CssClass="lstTabRow">Detrazioni</asp:label>
											</td>
											<td>
												<asp:label id="Label1" Runat="server" Width="200px" CssClass="lstTabRow">Agevolazioni</asp:label>
											</td>
										</tr>
										<tr>
											<td vAlign="top">
												<asp:textbox id="txtMaggiorazioni" onblur="if (isNumber(this,'',2,'',999999)) this.value = number_format(this.value, 2, ',', '');" Runat="server" Width="80px" CssClass="Input_Text_right" MaxLength="10"></asp:textbox>
											</td>
											<td vAlign="top">
												<asp:label id="Label25" Runat="server" CssClass="Input_Label" Width="80px">Importo fisso</asp:label>
											</td>
											<td vAlign="top">
												<asp:textbox id="txtMaggiorazioniPerc" onblur="if (isNumber(this,'',2,'',999999)) this.value = number_format(this.value, 2, ',', '');" Runat="server" Width="50px" CssClass="Input_Text_right" MaxLength="10"></asp:textbox>
											</td>
											<td vAlign="top">
												<asp:label id="Label26" Runat="server" CssClass="Input_Label">%</asp:label>
											</td>
											<td vAlign="top">
												<asp:textbox id="txtDetrazioni" onblur="this.value = number_format(this.value, 2, ',', '');" Runat="server" Width="60px" CssClass="Input_Text_right" MaxLength="10"></asp:textbox>
											</td>
											<td>
                                                <Grd:RibesGridView ID="GrdAgevolazioni" runat="server" BorderStyle="None" 
                                                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                                    AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false"
                                                    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                                    <PagerSettings Position="Bottom"></PagerSettings>
                                                    <PagerStyle CssClass="CartListFooter" />
                                                    <RowStyle CssClass="CartListItem"></RowStyle>
                                                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
													<Columns>
                                                        <asp:BoundField DataField="descrizione" HeaderText="Agevolazione">
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Sel.">
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" Width="30px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkSelezionato" runat="server" AutoPostBack="true" Checked='<%# DataBinder.Eval(Container, "DataItem.Selezionato")%>'></asp:CheckBox>
                                                                <asp:HiddenField ID="hfIdAgevolazione" runat="server" Value='<%# Eval("IdAgevolazione") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
												</Grd:RibesGridView>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<table>
										<tr>
										</tr>
										<tr>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<!--Blocco Note UI-->
				<tr>
					<td>
						<table id="TblNoteUI" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td><asp:label id="Label7" Runat="server" Width="762px" CssClass="lstTabRow">Note</asp:label><br>
									<asp:textbox id="TxtNoteUI" Runat="server" CssClass="Input_Text" MaxLength="4000" width="700px" TextMode="MultiLine" Height="32px"></asp:textbox></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<asp:button id="updateGrdRiduzioni" style="DISPLAY: none" Runat="server"></asp:button><asp:button id="btnUpdate" style="DISPLAY: none" Runat="server"></asp:button>
<asp:textbox id="TxtIdArticoloPadre" style="DISPLAY: none" Runat="server" CssClass="Input_Text">0</asp:textbox>
