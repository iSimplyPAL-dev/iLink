<%@ Page Language="vb" CodeBehind="SearchPeriodo.aspx.vb" AutoEventWireup="false" Inherits="OpenUtenze.SearchPeriodo" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<HTML>
	<HEAD>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
	</HEAD>
	<BODY class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" marginwidth="0" marginheight="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="1" cellPadding="1" width="98%" align="center" border="0">
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td class="NormalBold">Risultati della ricerca</td>
				</tr>

				<tr>
					<td>
						<asp:Label id="lblMessage" runat="server" Cssclass="NormalBold"></asp:Label></td>
				</tr>
				<tr>
					<td>
					    <Grd:RibesGridView ID="GrdPeriodo" runat="server" BorderStyle="None" 
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
								<asp:BoundField DataField="PERIODO" HeaderText="Periodo">
									<HeaderStyle Width="40px"></HeaderStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="Storico">
									<ItemTemplate>
										<asp:Label runat="server" Text='<%# CheckStatus(DataBinder.Eval(Container, "DataItem.STORICO")) %>' ID="Label1">
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Da">
									<ItemTemplate>
										<asp:Label runat="server" Text='<%# FncGrd.FormatGrdData(DataBinder.Eval(Container, "DataItem.DADATA")) %>' ID="Label2">
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="A">
									<ItemTemplate>
										<asp:Label runat="server" Text='<%# FncGrd.FormatGrdData(DataBinder.Eval(Container, "DataItem.ADATA")) %>' ID="Label3">
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="">
									<headerstyle horizontalalign="Center"></headerstyle>
									<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									<itemtemplate>
									<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("CODPERIODO") %>' alt=""></asp:ImageButton>
									<asp:HiddenField runat="server" ID="hfCODPERIODO" Value='<%# Eval("CODPERIODO") %>' />
									</itemtemplate>
								</asp:TemplateField>
							</Columns>
							</Grd:RibesGridView>
					</td>
				</tr>
				<tr>
					<td>
						<iframe id="iframetabella" name="iframetabella" width="95%" src="Periodo.aspx" frameborder="0" height="400px"></iframe>
					</td>
				</tr>
			</table>
			<input id="hdEnteAppartenenza" type="hidden" name="hdEnteAppartenenza"> 
            <input id="paginacomandi" type="hidden" name="paginacomandi">
			<asp:Button id="btnNuovo" runat="server" CssClass="hidden"></asp:Button>
		</FORM>
	</BODY>
</HTML>
