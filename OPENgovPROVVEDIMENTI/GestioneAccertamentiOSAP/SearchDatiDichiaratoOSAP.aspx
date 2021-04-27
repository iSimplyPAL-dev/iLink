<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchDatiDichiaratoOSAP.aspx.vb" Inherits="Provvedimenti.SearchDatiAccertamentiOSAP"%>
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
			alert('Impossibile lasciare il campo legame vuoto!')
		}

		function RibaltaImmobileAccertamento(){
		    document.getElementById('btnRibaltaImmobiliAcc').click();
		}
			</script>
</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" marginheight="0"
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
					<td><asp:label id="lblTesto" runat="server" cssclass="Input_Label" visible="False">Per Ribaltare in automatico tutti gli immobili selezionare l'opzione "Seleziona 
							tutti" e cliccare sul Bottone di ribaltamento; se si vuole ribaltare solamente 
							alcuni immobili presenti nella griglia selezionare la colonna Sel della riga 
							corrispondente.</asp:label></td>
				</tr>
				<tr>
					<td align="right"><asp:checkbox id="chkSelTutti" runat="server" cssclass="Input_Label" text="Seleziona tutti" visible="False"
							autopostback="True"></asp:checkbox></td>
				</tr>
			</table>
			<Grd:RibesGridView ID="GrdDichiarato" runat="server" BorderStyle="None" 
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
					<asp:BoundField DataField="Progressivo" ReadOnly="True" HeaderText="Prg">
						<headerstyle width="10px"></HeaderStyle>
						<itemstyle horizontalalign="Center"></ItemStyle>
					</asp:BoundField>
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
							<asp:Label id="Label1" runat="server" text='<%# OPENgovTOCO.SharedFunction.FormattaData(DataBinder.Eval(Container, "DataItem.DataFineOccupazione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Durata">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle Width="120px" HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label28" runat="server" text='<%# FncForGrd.FormattaDurCons(DataBinder.Eval(Container, "DataItem.DurataOccupazione"),DataBinder.Eval(Container, "DataItem.TipoDurata.Descrizione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Tipo Occup.">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle Width="300px" HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label27" runat="server" text='<%# (DataBinder.Eval(Container, "DataItem.TipologiaOccupazione.Descrizione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Cat.">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle Width="300px" HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label2" runat="server" text='<%# (DataBinder.Eval(Container, "DataItem.Categoria.Descrizione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Cons.">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label33" runat="server" text='<%# FncForGrd.FormattaDurCons(DataBinder.Eval(Container, "DataItem.Consistenza"), DataBinder.Eval(Container, "DataItem.TipoConsistenzaTOCO.Descrizione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Tariffa">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemTemplate>
							<asp:Label id="Label4" runat="server" text='<%# FncForGrd.FormattaCalcolo("T",DataBinder.Eval(Container, "DataItem.Calcolo"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Imp.">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemTemplate>
							<asp:Label id="Label5" runat="server" text='<%# FncForGrd.FormattaCalcolo("I",DataBinder.Eval(Container, "DataItem.Calcolo"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Leg">
						<headerstyle width="25px"></headerstyle>
						<itemstyle horizontalalign="Right"></itemstyle>
						<itemtemplate>
							<asp:Label id="lblLegame" runat="server" CssClass="Input_Label" Text='<%# DataBinder.Eval(Container, "DataItem.IdLegame") %>'>
							</asp:Label>
						</itemtemplate>
						<edititemtemplate>
							<asp:TextBox id="txtLegame" runat="server" CssClass="Input_Text_Right" Width="24px"></asp:TextBox>
						</edititemtemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Modif">
						<itemstyle horizontalalign="Center"></itemstyle>
						<itemtemplate>
							<asp:ImageButton id="imgEdit" runat="server" Width="16px" ImageUrl="..\..\images\Bottoni\modifica.png" Height="19px" CommandName="RowEdit" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
						</itemtemplate>
						<edititemtemplate>
							<asp:ImageButton id="imgUpdate" runat="server" Width="16px" ImageUrl="..\..\images\Bottoni\modifica.png" Height="19px" CommandName="RowUpdate" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
							<asp:ImageButton id="ImageButton2" runat="server" Width="14px" ImageUrl="..\..\images\Bottoni\cancel.png" Height="17px" CommandName="RowCancel" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
						</edititemtemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Del">
						<headerstyle horizontalalign="Center"></headerstyle>
						<itemstyle horizontalalign="Center"></itemstyle>
						<itemtemplate>
							<asp:ImageButton id="imgDelete" runat="server" Width="16px" ImageUrl="..\..\images\Bottoni\cestino.png" Height="19px" CommandName="RowDelete" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
						    <asp:HiddenField runat="server" ID="hfIdArticolo" Value='<%# Eval("IdArticolo") %>' />
                            <asp:HiddenField runat="server" ID="hfIdLegame" Value='<%# Eval("IdLegame") %>' />
                            <asp:HiddenField runat="server" ID="hfprogressivo" Value='<%# Eval("Progressivo") %>' />
						</itemtemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Sel">
						<itemstyle horizontalalign="Center"></itemstyle>
						<itemtemplate>
							<asp:CheckBox id="chkRibaltaIm" runat="server"></asp:CheckBox>
						</itemtemplate>
					</asp:TemplateField>
					<asp:TemplateField visible="false" HeaderText="Tipo Occup.">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle Width="300px" HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label3" runat="server" text='<%# (DataBinder.Eval(Container, "DataItem.Dichiarazione.IdDichiarazione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
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
			<asp:button id="btnRibaltaImmobiliAcc" style="DISPLAY: none" runat="server" Width="1px" Height="1px"></asp:button>
		</form>
	</body>
</HTML>
