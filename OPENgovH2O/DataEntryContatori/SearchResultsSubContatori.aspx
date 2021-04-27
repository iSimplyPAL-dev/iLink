<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchResultsSubContatori.aspx.vb" Inherits="OpenUtenze.SearchResultsSubContatori" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>SearchResultsSubContatori</title>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script type="text/javascript">
		function visualizzaID(parametro,matricola){
			parent.document.getElementById('Associato').value=parametro;
			parent.document.getElementById('AssociatoMatricola').value=matricola;
		}
		</script>
	</HEAD>
	<body class="Sfondo">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" border="0">
				<tr>
					<td><asp:label id="lblMessage" runat="server" CssClass="NormalBold"></asp:label></td>
				</tr>
				<tr>
					<td>
						<Grd:RibesGridView ID="GrdContatori" runat="server" BorderStyle="None" 
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
								<asp:BoundField DataField="MATRICOLA" HeaderText="Matricola"></asp:BoundField>
								<asp:BoundField DataField="COGNOME_INT" HeaderText="Cognome"></asp:BoundField>
								<asp:BoundField DataField="NOME_INT" HeaderText="Nome"></asp:BoundField>
								<asp:TemplateField HeaderText="Ubicazione">
									<ItemTemplate>
										<asp:Label id="Label2" runat="server" text='<%# DataBinder.Eval(Container, "DataItem.via_ubicazione") + " " + DataBinder.Eval(Container, "DataItem.civico_ubicazione") %>'>Label</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="">
									<headerstyle horizontalalign="Center"></headerstyle>
									<itemstyle horizontalalign="Center" Width="40px"></itemstyle>
									<itemtemplate>
									<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("CODICE_CONTRIBUENTE") %>' alt=""></asp:ImageButton>
									<asp:HiddenField runat="server" ID="hfCODCONTATORE" Value='<%# Eval("CODCONTATORE") %>' />
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
