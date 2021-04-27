<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ConsultaDocElaborati.aspx.vb" Inherits="OpenUtenze.ConsultaDocElaborati"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ConsultaDocElaborati</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
	</HEAD>
	<body class="Sfondo">
		<form id="Form1" runat="server" method="post">
			<Grd:RibesGridView ID="GrdResult" runat="server" BorderStyle="None" 
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
						<asp:BoundField DataField="name" SortExpression="name" HeaderText="Nome Documento">
							<HeaderStyle HorizontalAlign="Left" ForeColor="White"></HeaderStyle>
							<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle"></ItemStyle>
						</asp:BoundField>
						<asp:TemplateField>
							<HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<A href='<%# DataBinder.Eval(Container, "DataItem.url") %>' target="_blank">Apri</A>
							</ItemTemplate>
							<EditItemTemplate>
								<A href='<%# DataBinder.Eval(Container, "DataItem.url") %>' target="_blank">Apri</A>
							</EditItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="">
							<headerstyle horizontalalign="Center"></headerstyle>
							<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
							<itemtemplate>
							<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("CODICE_CONTRIBUENTE") %>' alt=""></asp:ImageButton>
							</itemtemplate>
						</asp:TemplateField>
					</Columns>
			</Grd:RibesGridView>
		</form>
	</body>
</HTML>
