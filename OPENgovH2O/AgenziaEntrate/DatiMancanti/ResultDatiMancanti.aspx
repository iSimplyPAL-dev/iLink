<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ResultDatiMancanti.aspx.vb" Inherits="OpenUtenze.ResultDatiMancanti"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ResultDatiMancanti</title>
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
	<body class="Sfondo" MS_POSITIONING="GridLayout" leftmargin="0">
		<form id="Form1" runat="server" method="post">
			<table id="Table1" cellSpacing="0" cellPadding="0" width="770px" border="0">
				<tr>
					<td><asp:label id="LblMessage" runat="server" CssClass="Legend"></asp:label></td>
				</tr>
				<tr>
					<td>
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
								<asp:TemplateField HeaderText="Nominativo">
									<ItemStyle Width="240px"></ItemStyle>
									<ItemTemplate>
										<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.sCognome") +" "+  DataBinder.Eval(Container, "DataItem.sNome")%>' ID="Label1">
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="sCfPIva" HeaderText="Cod.Fiscale/P.Iva">
									<HeaderStyle ForeColor="White" Width="100px"></HeaderStyle>
								</asp:BoundField>
								<asp:BoundField DataField="sIndirizzoImmo" HeaderText="Ubicazione Immobile">
									<HeaderStyle ForeColor="White" Width="160px"></HeaderStyle>
								</asp:BoundField>
								<asp:BoundField DataField="sDescrAnomalia" HeaderText="Tipo Anomalia">
									<HeaderStyle ForeColor="White"></HeaderStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="">
									<headerstyle horizontalalign="Center"></headerstyle>
									<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									<itemtemplate>
									<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("CODICE_CONTRIBUENTE") %>' alt=""></asp:ImageButton>
										<asp:HiddenField runat="server" ID="hfnIdArticolo" Value='<%# Eval("nIdArticolo") %>' />
                                        <asp:HiddenField runat="server" ID="hfnIdContribuente" Value='<%# Eval("nIdContribuente") %>' />
									</itemtemplate>
								</asp:TemplateField>
							</Columns>
							</Grd:RibesGridView>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
