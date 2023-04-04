<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchAtti.aspx.vb" Inherits="Provvedimenti.SearchAtti" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<HTML>
	<HEAD>
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
	</HEAD>
	<BODY class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" marginheight="0"
		marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td colSpan="4"><asp:label id="lblMessage" runat="server" CssClass="NormalRed"></asp:label></td>
				</tr>
				<tr>
					<td colSpan="4"><INPUT id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza">
						<Grd:RibesGridView ID="GrdAtti" runat="server" BorderStyle="None" 
						  BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
						  AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="5"
						  ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
						  OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
						  <PagerSettings Position="Bottom"></PagerSettings>
						  <PagerStyle CssClass="CartListFooter" />
						  <RowStyle CssClass="CartListItem"></RowStyle>
						  <HeaderStyle CssClass="CartListHead"></HeaderStyle>
						  <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							<Columns>
								<asp:BoundField DataField="NOMINATIVO" SortExpression="NOMINATIVO" HeaderText="Intestatari dei Provvedimenti">
									<HeaderStyle HorizontalAlign="Left" Width="70%" VerticalAlign="Middle"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="CODICE_FISCALE" HeaderText="Codice Fiscale">
									<HeaderStyle HorizontalAlign="Left" Width="15%" VerticalAlign="Middle"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="PARTITA_IVA" HeaderText="Partita Iva">
									<HeaderStyle HorizontalAlign="Left" Width="15%" VerticalAlign="Middle"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="">
						            <headerstyle horizontalalign="Center"></headerstyle>
						            <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
						            <itemtemplate>
						            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("COD_CONTRIBUENTE") %>' alt=""></asp:ImageButton>
						            <asp:HiddenField runat="server" ID="hfCOD_CONTRIBUENTE" Value='<%# Eval("COD_CONTRIBUENTE") %>' />
						            </itemtemplate>
					            </asp:TemplateField>
							</Columns>
						</Grd:RibesGridView>
                    </td>
				</tr>
			</table>
			<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr height="20">
					<td>
						<hr class="hr" width="100%" SIZE="1">
					</td>
				</tr>
			</TABLE>
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td class="Input_Label">Nominativo:</td>
					<td class="Input_Label">Cod.Fisc/P.Iva</td>
					<td class="Input_Label">Sesso:</td>
					<td class="Input_Label">Data di Nascita:</td>
				</tr>
				<tr>
					<td><asp:textbox id="txtNominativo" runat="server" CssClass="Input_Text" ReadOnly="True" size="50"></asp:textbox></td>
					<td><asp:textbox id="txtCFPI" runat="server" CssClass="Input_Text" ReadOnly="True" size="20"></asp:textbox></td>
					<td><asp:textbox id="txtSesso" runat="server" CssClass="Input_Text" ReadOnly="True" size="3"></asp:textbox></td>
					<td><asp:textbox id="txtDataNascita" runat="server" CssClass="Input_Text_Right TextDate" ReadOnly="True" size="12"></asp:textbox></td>
				</tr>
				<tr>
					<td class="Input_Label">Indirizzo:</td>
					<td class="Input_Label">Città:</td>
					<td class="Input_Label">Prov.:</td>
					<td class="Input_Label">C.A.P.</td>
				</tr>
				<tr>
					<td><asp:textbox id="txtIndirizzo" runat="server" CssClass="Input_Text" ReadOnly="True" size="50"></asp:textbox></td>
					<td><asp:textbox id="txtComune" runat="server" CssClass="Input_Text" ReadOnly="True" size="30"></asp:textbox></td>
					<td><asp:textbox id="txtProvincia" runat="server" CssClass="Input_Text" ReadOnly="True" size="5"></asp:textbox></td>
					<td><asp:textbox id="txtCap" runat="server" CssClass="Input_Text" ReadOnly="True" size="10"></asp:textbox></td>
				</tr>
			</table>
			<TABLE cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr height="10">
					<td>
						<hr class="hr" width="100%" SIZE="1">
					</td>
				</tr>
			</TABLE>
			<TABLE cellPadding="0" width="100%" border="0">
				<tr>
					<td>
                        <iframe id="loadGrid" src="LoadAtti.aspx" frameBorder="0" width="100%" height="360px"></iframe>
					</td>
				</tr>
			</TABLE>
			<DIV id="disabilita_Riepilogativi" style="DISPLAY: none">
				<table cellSpacing="0" cellPadding="0" width="100%" border="0">
					<tr>
						<td colSpan="4">
							<div id="Riepilogativi" style="DISPLAY: none">
								<FIELDSET class="classeFiledSet100">
									<LEGEND class="Legend">
						          Riepilogativi
					            </LEGEND>
									<TABLE cellPadding="0" width="100%" border="0">
										<TR>
											<TD class="Input_Label" width="35%">
												<asp:label id="lblNumeroTotaleAvvisi" runat="server">Numero Totale Provvedimenti:</asp:label>
												<asp:label id="txtTotaleAvvisi" runat="server" Width="215px"></asp:label>
											</TD>
											<TD class="Input_Label" width="25%">
												<asp:label id="lbl" runat="server">Importo Totale Provvedimenti
											</asp:label>
											</TD>
										</TR>
										<TR>
											<TD width="25%">
											</TD>
											<TD width="25%">
												<asp:textbox CssClass="Input_Text_Enable_Red_right OnlyNumber" id="txtImportoTotaleAvvisi" runat="server" ReadOnly="True" Width="215px"></asp:textbox>
											</TD>
										</TR>
										<TR>
											<TD class="Input_Label" width="25%" colspan="2">
												<asp:label id="Label2" runat="server">Importo Totale Provvedimenti al netto delle Rettifiche e degli Annulamenti
											</asp:label>
											</TD>
										</TR>
										<TR>
											<TD width="25%" colspan="2">
												<asp:textbox CssClass="Input_Text_Enable_Red_right" id="txtTotaleRettifiche" runat="server" ReadOnly="True"
													Width="215px"></asp:textbox>
											</TD>
										</TR>
									</TABLE>
								</FIELDSET>
							</div>
						</td>
					</tr>
				</table>
			</DIV>
		</FORM>
	</BODY>
</HTML>
