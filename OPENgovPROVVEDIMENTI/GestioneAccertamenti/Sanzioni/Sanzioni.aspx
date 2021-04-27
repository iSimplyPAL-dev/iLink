<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Sanzioni.aspx.vb" Inherits="Provvedimenti.Sanzioni" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
	</HEAD>
	<BODY class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<P>
				&nbsp;
				<asp:Label ID="lblTitolo1" Runat="server" CssClass="Input_Label_14 bold"> Configurazione Sanzioni, Interessi e Motivazioni per Accertamento</asp:Label></P>
			<fieldset>
				<legend class="Legend">Sanzioni </legend>
				<table height="159" cellPadding="0" width="488" align="center" border="0">
					<tr>
						<td vAlign="top" align="center">
                            <Grd:RibesGridView ID="GrdSanzioni" runat="server" BorderStyle="None" 
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                OnRowDataBound="GrdRowDataBound" OnRowCommand="GrdRowCommand">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <PagerStyle CssClass="CartListFooter" />
                                <RowStyle CssClass="CartListItem"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
								<Columns>
									<asp:BoundField DataField="DESCRIZIONE" HeaderText="Descrizione Sanzione">
										<HeaderStyle></HeaderStyle>
									</asp:BoundField>
									<asp:TemplateField HeaderText="Sanz.">
										<HeaderStyle Width="25px"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
										<ItemTemplate>
											<asp:CheckBox id="chkSanzioni" runat="server" tooltip="Seleziona Sanzione per Accertamento"></asp:CheckBox>
									        <asp:HiddenField runat="server" ID="hfCodVoce" Value='<%# Eval("COD_VOCE") %>' />
									        <asp:HiddenField runat="server" ID="hfMotivazione" Value='<%# Eval("Motivazione") %>' />
									        <asp:HiddenField runat="server" ID="hfCheckSanzione" Value='<%# Eval("CHECKSANZIONE") %>' />
									        <asp:HiddenField runat="server" ID="hfCodTipoProv" Value='<%# Eval("COD_TIPO_PROVVEDIMENTO") %>' />
										</ItemTemplate>
									</asp:TemplateField>
					                <asp:TemplateField HeaderText="Motivazione">
						                <HeaderStyle horizontalalign="Center"></HeaderStyle>
						                <ItemStyle horizontalalign="Center" Width="40px"></ItemStyle>
						                <itemtemplate>
                                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowViewMotivazioni" CommandArgument='<%# Eval("COD_VOCE") %>' alt=""></asp:ImageButton>
							                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneSalvaGrd" CommandName="RowMotivazioni" CommandArgument='<%# Eval("COD_VOCE") %>' alt=""></asp:ImageButton>
						                </ItemTemplate>
					                </asp:TemplateField>
								</Columns>
							</Grd:RibesGridView>
							<asp:Label id="lblSanzioni" visible="false" runat="server" CssClass="Input_Label"></asp:Label>
						</td>
					</tr>
					<tr>
						<td vAlign="top" align="left">
							<asp:CheckBox id="ChkSanzGlobal" runat="server" CssClass="Input_Label" Text="Calcola Sanzioni per tutti gli immobili dell'accertamento"></asp:CheckBox>
						</td>
					</tr>
				</table>
			</fieldset>
			<fieldset>
				<legend class="Legend">Motivazioni </legend>
				<table height="94" cellPadding="0" width="493" align="center" border="0">
					<tr>
						<td vAlign="top" align="center"><asp:textbox id="txtMotivazioni" runat="server" Width="487px" Rows="15" TextMode="MultiLine" Height="88px"></asp:textbox></td>
					</tr>
				</table>
			</fieldset>
			<fieldset>
				<legend class="Legend">Interessi </legend>
				<table height="32" cellPadding="0" width="493" align="center" border="0">
					<tr>
						<td vAlign="top" align="left">
							<asp:CheckBox id="chkCalcolaInteressi" runat="server" CssClass="Input_Label" Text="Calcola Interessi per tutti gli immobili dell'accertamento"></asp:CheckBox></td>
					</tr>
					<tr>
						<td vAlign="top" align="left">
							<asp:CheckBox style="DISPLAY:none" id="ChkIntGlobal" runat="server" CssClass="Input_Label" Text="Calcola Interessi per tutti gli immobili dell'accertamento"></asp:CheckBox>
						</td>
					</tr>
				</table>
			</fieldset>
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
			<asp:button id="btnSalvaMotivazioni" style="DISPLAY: none" runat="server" Text="SalvaMotivazioniSanzioni"></asp:button><asp:button id="btnAggiornaGriglia" style="DISPLAY: none" runat="server" Text="AggiornaGrigliaConSanzioni"></asp:button>
		</FORM>
	</BODY>
</HTML>
