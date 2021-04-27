<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchDatiAccertamenti.aspx.vb" Inherits="Provvedimenti.SearchDatiAccertamenti" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
			<script>
		function msgLegameVuoto()
		{
				GestAlert('a', 'warning', '', '', 'Impossibile lasciare il campo legame vuoto!')
		}
		
		function RibaltaImmobileAccertamento(){
		    document.getElementById('btnRibaltaImmobiliAcc').click();
		}
		
		function VerificaRibaltamento()
		{
			/*if (parent.parent.Visualizza.frames.item('loadGridAccertato').document.getElementById('RibesGridAnagrafica')!=null){
				GestAlert('a', 'warning', '', '', 'ATTENZIONE! Sono già stati ribaltati degli immobili.');
				return false;
			}
			else{
				return true;
			}*/
		}
			</script>
	</HEAD>
	<BODY class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" marginheight="0"
		marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellPadding="0" width="100%" border="0">
				<TR>
					<TD><asp:label id="lblMessage" runat="server" CssClass="NormalRed"></asp:label></TD>
				</TR>
				<TR>
					<TD><INPUT id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza">
					</TD>
				</TR>
				<tr>
					<td><asp:label id="lblTesto" runat="server" CssClass="Input_Label" Visible="False">Per Ribaltare in automatico tutti gli immobili selezionare l'opzione "<i>Seleziona 
							tutti</i>" e cliccare sul Bottone di ribaltamento; se si vuole ribaltare solamente 
							alcuni degli immobili presenti nella griglia selezionare la colonna "<i>Sel</i>" della riga 
							corrispondente.</asp:label></td>
				</tr>
				<tr>
					<td align="right">
						<asp:checkbox id="chkSelTutti" runat="server" CssClass="Input_Label" Text="Seleziona tutti" Visible="False"
							AutoPostBack="True"></asp:checkbox>
						<asp:TextBox ID="TxtDataFineRibalta" Runat="server" CssClass="Input_Text_Right TextDate" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" tooltip="Data da forzare come fine nell'accertato"></asp:TextBox>
					</td>
				</tr>
			</table>
			<Grd:RibesGridView ID="GrdDichiarato" runat="server" BorderStyle="None" 
				BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
				AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
				ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
				OnRowCommand="GrdRowCommand" OnRowDataBound="GrdRowDataBound">
				<PagerSettings Position="Bottom"></PagerSettings>
				<PagerStyle CssClass="CartListFooter" />
				<RowStyle CssClass="CartListItem"></RowStyle>
				<HeaderStyle CssClass="CartListHead"></HeaderStyle>
				<AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
				<columns>
					<asp:BoundField DataField="Progressivo" ReadOnly="True" HeaderText="Prg">
						<HeaderStyle Width="10px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Dal">
						<HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# FncGrd.annoBarra(DataBinder.Eval(Container, "DataItem.DAL")) %>' ID="lblDal">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Al">
						<HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# FncGrd.annoBarra(DataBinder.Eval(Container, "DataItem.AL")) %>' ID="lblAl">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="Foglio" ReadOnly="True" HeaderText="Fg">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="Numero" ReadOnly="True" HeaderText="N&#176;">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="Subalterno" ReadOnly="True" HeaderText="Sub">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="Categoria" ReadOnly="True" HeaderText="Cat">
						<HeaderStyle Width="12px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="CLASSE" ReadOnly="True" HeaderText="Cl">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Cons">
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# FncGrd.IntForGridView(DataBinder.Eval(Container, "DataItem.Consistenza")) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField visible="true" datafield="Zona" ReadOnly="True" headertext="Zona"></asp:BoundField>
					<asp:BoundField DataField="TIPORENDITA" ReadOnly="True" HeaderText="TR">
						<HeaderStyle Width="10px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField datafield="Rendita" readonly="True" headertext="Rendita" dataformatstring="{0:#,##0.00}">
						<headerstyle width="25px"></headerstyle>
						<itemstyle horizontalalign="Right"></itemstyle>
					</asp:BoundField>
					<asp:BoundField DataField="Valore" ReadOnly="True" HeaderText="Valore" DataFormatString="{0:#,##0.00}">
						<HeaderStyle Width="25px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="percPossesso" ReadOnly="True" HeaderText="% Poss">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="TITPOSSESSO" ReadOnly="True" HeaderText="Tit.Poss">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="NUtilizzatori" ReadOnly="True" HeaderText="N. Utiliz">
						<HeaderStyle Width="20px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Princ">
						<HeaderStyle Width="25px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox Enabled="False" id="chkPrinc" runat="server" Checked='<%# FncGrd.checkFlag(DataBinder.Eval(Container, "DataItem.flagprincipale")) %>' >
							</asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Pert">
						<HeaderStyle Width="25px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox Enabled="False" id="chkPert" runat="server" Checked='<%# FncGrd.checkPertinenza(DataBinder.Eval(Container, "DataItem.IdImmobilePertinenza"), DataBinder.Eval(Container, "DataItem.ID")) %>'>
							</asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Rid">
						<HeaderStyle Width="25px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox Enabled="False" id="chkRidotto" runat="server" Checked='<%# FncGrd.checkMesiRiduzione(DataBinder.Eval(Container, "DataItem.mesiriduzione")) %>'>
							</asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="TotDovuto" ReadOnly="True" HeaderText="Imp." DataFormatString="{0:#,##0.00}">
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Leg">
						<HeaderStyle Width="25px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="lblLegame" runat="server" CssClass="Input_Label" Text='<%# DataBinder.Eval(Container, "DataItem.IDLEGAME") %>'></asp:Label>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id="txtLegame" runat="server" CssClass="Input_Text_Right" Width="24px" Text='<%# DataBinder.Eval(Container, "DataItem.IDLEGAME") %>'></asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Modif">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:ImageButton id="imgEdit" runat="server" Width="16px" ImageUrl="..\..\images\Bottoni\modifica.png" Height="19px" CommandName="RowEdit" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:ImageButton id="imgUpdate" runat="server" Width="16px" ImageUrl="..\..\images\Bottoni\modifica.png" Height="19px" CommandName="RowUpdate" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
							<asp:ImageButton id="ImageButton2" runat="server" Width="14px" ImageUrl="..\..\images\Bottoni\cestino.png" Height="17px" CommandName="RowCancel" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
						</EditItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField Visible="False" HeaderText="Del">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:ImageButton id="imgDelete" runat="server" Width="16px" ImageUrl="..\..\images\Bottoni\cestino.png" Height="19px" CommandName="RowDelete" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Sel">
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id="chkRibaltaIm" runat="server"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateField>
				</columns>
				</Grd:RibesGridView>
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
                <asp:button id="btnRibaltaImmobiliAcc" style="DISPLAY: none" runat="server" Width="1px" Height="1px"></asp:button></FORM>
	</BODY>
</HTML>
