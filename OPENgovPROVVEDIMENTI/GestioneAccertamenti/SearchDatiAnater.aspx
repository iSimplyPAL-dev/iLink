<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchDatiAnater.aspx.vb" Inherits="Provvedimenti.SearchDatiAnater" %>
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
						<asp:button id="btnAssocia" style="DISPLAY: none" runat="server" Text="AssociaImmobili"></asp:button></TD>
				</TR>
			</table>
			<Grd:RibesGridView ID="GrdAnagrafica" runat="server" BorderStyle="None" 
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
					<asp:TemplateField HeaderText="Dal">
						<HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# FormatDataEmpty(DataBinder.Eval(Container, "DataItem.UniDataInizio")) %>' ID="lblDal">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Al">
						<HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# FormatDataEmpty(DataBinder.Eval(Container, "DataItem.UniDataFine")) %>' ID="lblAl">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="UniFoglio" ReadOnly="True" HeaderText="Fg.">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="UniNumMapp" ReadOnly="True" HeaderText="Num.">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="UniSubalterno" ReadOnly="True" HeaderText="Sub.">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="UniCodRicerca" ReadOnly="True" HeaderText="Cod. Ric.">
						<HeaderStyle Width="60px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="UniNumProgrFabbricato" ReadOnly="True" HeaderText="Prog. Fabbr.">
						<HeaderStyle Width="40px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Indirizzo">
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# FormattaIndirizzo(DataBinder.Eval(Container, "DataItem.UniDescrizioneVia"), DataBinder.Eval(Container, "DataItem.UniNumeroCiv")) %>' ID="Label1">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Cat.">
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# FormattaCategoria(DataBinder.Eval(Container, "DataItem.UniCategCatastale")) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="UniClasseCatastale" ReadOnly="True" HeaderText="Cl.">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="UniConsistenza" ReadOnly="True" HeaderText="Cons.">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="TipoRendita" ReadOnly="True" HeaderText="TR">
						<HeaderStyle Width="10px"></HeaderStyle>
					</asp:BoundField>
					<asp:BoundField DataField="UniRenditaCatastale" ReadOnly="True" HeaderText="Rend/Val" DataFormatString="{0:#,##0.00}">
						<HeaderStyle Width="25px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField ReadOnly="True" HeaderText="% Poss.">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField ReadOnly="True" HeaderText="Titolo Possesso">
						<HeaderStyle Width="80px"></HeaderStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Princ.">
						<HeaderStyle Width="25px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox Enabled="False" id="chkPrinc" runat="server"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Pert.">
						<HeaderStyle Width="25px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox Enabled="False" id="chkPert" runat="server"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Rid">
						<HeaderStyle Width="25px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox Enabled="False" id="chkRidotto" runat="server"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Sel">
						<HeaderStyle Width="20px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id="chkSeleziona" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Selezionato")%>'>
							</asp:CheckBox>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id="TextBox1" runat="server"></asp:TextBox>
						</EditItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="">
						<headerstyle horizontalalign="Center"></headerstyle>
						<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
						<itemtemplate>
						<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("CODICE_CONTRIBUENTE") %>' alt=""></asp:ImageButton>
						<asp:HiddenField runat="server" ID="hfIDProgressivo" Value='<%# Eval("IDProgressivo") %>' />
						</itemtemplate>
					</asp:TemplateField>
				</Columns>
				</Grd:RibesGridView>
                </FORM>
	</BODY>
</HTML>
