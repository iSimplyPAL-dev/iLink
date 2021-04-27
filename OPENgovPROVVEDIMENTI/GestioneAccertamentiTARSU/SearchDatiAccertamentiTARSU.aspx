<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchDatiAccertamentiTARSU.aspx.vb" Inherits="Provvedimenti.SearchDatiAccertamentiTARSU"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
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
		</script>
</head>
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
					<td><asp:label id="lblTesto" runat="server" cssclass="Input_Label"  visible="False">Per Ribaltare in automatico tutti gli immobili selezionare l'opzione <i>"Seleziona 
							tutti"</i> e cliccare sul Bottone di ribaltamento; se si vuole ribaltare solamente 
							alcuni degli immobili presenti nella griglia selezionare la colonna <i>"Sel"</i> della riga 
							corrispondente.</asp:label></td>
				</tr>
				<tr>
					<td align="right"><asp:checkbox id="chkSelTutti" runat="server" cssclass="Input_Label" text="Seleziona tutti" visible="False"
							autopostback="True"></asp:checkbox></td>
				</tr>
			</table>
			<!--*** 20140701 - IMU/TARES ***-->
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
						<headerstyle width="10px"></HeaderStyle>
						<itemstyle horizontalalign="Center"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Dal">
						<headerstyle horizontalalign="Center" width="70px"></HeaderStyle>
						<itemstyle horizontalalign="Center"></ItemStyle>
						<itemtemplate>
							<asp:Label runat="server" Text='<%# FncGrd.annoBarra(DataBinder.Eval(Container, "DataItem.tDataInizio")) %>' ID="lblDal">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Al">
						<headerstyle horizontalalign="Center" width="70px"></HeaderStyle>
						<itemstyle horizontalalign="Center"></ItemStyle>
						<itemtemplate>
							<asp:Label runat="server" Text='<%# FncGrd.annoBarra(DataBinder.Eval(Container, "DataItem.tDataFine")) %>' ID="lblAl">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="sVia" ReadOnly="True" HeaderText="Via">
						<HeaderStyle width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Left"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Civico">
						<headerstyle horizontalalign="Left" width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
						<itemtemplate>
							<asp:Label runat="server" Text='<%# FncGrd.FormattaNumeriGrd(DataBinder.Eval(Container, "DataItem.scivico"))%>' ID="Label29">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="sInterno" ReadOnly="True" HeaderText="Interno">
						<headerstyle width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="sfoglio" ReadOnly="True" HeaderText="Foglio">
						<headerstyle width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="snumero" ReadOnly="True" HeaderText="Numero">
						<headerstyle width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="ssubalterno" ReadOnly="True" HeaderText="Sub.">
						<headerstyle width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="sdescrCategoria" ReadOnly="True" HeaderText="Cat.">
						<headerstyle width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Left" verticalalign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="nComponenti" ReadOnly="True" HeaderText="NC">
						<ItemStyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="nComponentiPV" ReadOnly="True" HeaderText="NC PV">
						<ItemStyle horizontalalign="Right" verticalalign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Forza PV">
						<ItemStyle horizontalalign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id="chkForzaPV" runat="server" ReadOnly="True" Checked='<%# DataBinder.Eval(Container,"DataItem.bForzaPV")%>'></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="ImpTariffa" ReadOnly="True" HeaderText="Tariffa">
						<headerstyle width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="nMq" ReadOnly="True" HeaderText="Mq">
						<headerstyle width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Leg">
						<headerstyle width="25px"></HeaderStyle>
						<itemstyle horizontalalign="Right"></ItemStyle>
						<itemtemplate>
							<asp:Label id="lblLegame" runat="server" CssClass="Input_Label" Text='<%# DataBinder.Eval(Container, "DataItem.IdLegame") %>'>
							</asp:Label>
						</ItemTemplate>
						<edititemtemplate>
							<asp:TextBox id="txtLegame" runat="server" CssClass="Input_Text_Right" Width="24px"></asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Modif">
						<itemstyle horizontalalign="Center"></ItemStyle>
						<itemtemplate>
							<asp:ImageButton id="imgEdit" runat="server" Width="16px" ImageUrl="..\..\images\Bottoni\modifica.png" Height="19px" CommandName="RowEdit" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
						</ItemTemplate>
						<edititemtemplate>
							<asp:ImageButton id="imgUpdate" runat="server" Width="16px" ImageUrl="..\..\images\Bottoni\modifica.png" Height="19px" CommandName="RowUpdate" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
							<asp:ImageButton id="ImageButton2" runat="server" Width="14px" ImageUrl="..\..\images\Bottoni\cancel.png" Height="17px" CommandName="RowCancel" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
						</EditItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Del">
						<headerstyle horizontalalign="Center"></HeaderStyle>
						<itemstyle horizontalalign="Center"></ItemStyle>
						<itemtemplate>
							<asp:ImageButton id="imgDelete" runat="server" Cssclass="BottoneGrd BottoneCancellaGrd" CommandName="RowDelete" CommandArgument='<%# Eval("Progressivo") %>' alt=""></asp:ImageButton>
						    <asp:HiddenField runat="server" ID="hfIdLegame" Value='<%# Eval("IdLegame") %>' />
                            <asp:HiddenField runat="server" ID="hfImpNetto" Value='<%# Eval("ImpNetto") %>' />
                            <asp:HiddenField runat="server" ID="hfImpRuolo" Value='<%# Eval("ImpRuolo") %>' />
                            <asp:HiddenField runat="server" ID="hfImpInteressi" Value='<%# Eval("ImpInteressi") %>' />
                            <asp:HiddenField runat="server" ID="hfImpSanzioni" Value='<%# Eval("ImpSanzioni") %>' />
                            <asp:HiddenField runat="server" ID="hfImpRiduzione" Value='<%# Eval("ImpRiduzione") %>' />
                            <asp:HiddenField runat="server" ID="hfImpDetassazione" Value='<%# Eval("ImpDetassazione") %>' />
                            <asp:HiddenField runat="server" ID="Id" Value='<%# Eval("Id") %>' />
                            <asp:HiddenField runat="server" ID="hfIdDettaglioTestata" Value='<%# Eval("IdDettaglioTestata") %>' />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Sel">
						<itemstyle horizontalalign="Center"></ItemStyle>
						<itemtemplate>
							<asp:CheckBox id="chkRibaltaIm" runat="server"></asp:CheckBox>
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
		</FORM>
	</BODY>
</html>
