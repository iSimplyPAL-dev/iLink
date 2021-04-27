<%@ Page Language="vb" AutoEventWireup="False" Codebehind="DettLetture.aspx.vb" Inherits="OpenUtenze.DettLetture" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Dettaglio Letture</title>
		<META http-equiv="Content-Type" content="text/html; charset=windows-1252">
		<META content="True" name="vs_showGrid">
		<META content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
		<META content="Visual Basic 7.0" name="CODE_LANGUAGE">
		<META content="JavaScript" name="vs_defaultClientScript">
		<META content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../_js/MyUtility.js?newversion"></script>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/vbscript" src="../../_vbs/OperazioniSuCampi.vbs"></script>
		<script type="text/vbscript" src="../../_vbs/ControlliFormali.vbs"></script>
	</HEAD>
	<BODY class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<asp:label id="lblMessage" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 5px" runat="server"
				CssClass="NormalRed"></asp:label>
			<Grd:RibesGridView ID="GrdLetture" runat="server" BorderStyle="None" 
				BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
				AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
				ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
				OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
				<PagerSettings Position="Bottom"></PagerSettings>
				<PagerStyle CssClass="CartListFooter" />
				<RowStyle CssClass="CartListItem"></RowStyle>
				<HeaderStyle CssClass="CartListHead"></HeaderStyle>
				<AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
				<columns>
					<asp:BoundField DataField="MATRICOLA" SortExpression="MATRICOLA" HeaderText="Matricola">
					</asp:BoundField>
					<asp:TemplateField SortExpression="PERIODO" HeaderText="Periodo">
						<itemtemplate>
							<asp:Label id="lblPeriodo" runat="server" Text='<%#FncGrd.FormattaPeriodo(DataBinder.Eval(Container, "DataItem.PERIODO"))%>'>
							</asp:Label>
						</itemtemplate>
					</asp:TemplateField>
					<asp:TemplateField SortExpression="DATALETTURA" HeaderText="Data Lettura">
						<itemtemplate>
							<asp:Label id="lblDataLettura" runat="server" Text='<%#FncGrd.FormatGrdData(DataBinder.Eval(Container, "DataItem.DATALETTURA"))%>'>
							</asp:Label>
						</itemtemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="LETTURA" SortExpression="LETTURA" HeaderText="Lettura Rilevata">
					</asp:BoundField>
					<asp:TemplateField SortExpression="PRIMA" HeaderText="Prima Lettura">
						<itemtemplate>
							<asp:Label id="lblPrimaLettura" runat="server" Text='<%#FncGrd.CheckStatus(DataBinder.Eval(Container, "DataItem.PRIMA"))%>'>
							</asp:Label>
						</itemtemplate>
					</asp:TemplateField>
					<asp:TemplateField SortExpression="FATTURATA" HeaderText="Fatturata">
						<itemtemplate>
							<asp:Label id="lblFatturata" runat="server" Text='<%#FncGrd.CheckStatus(DataBinder.Eval(Container, "DataItem.FATTURATA"))%>'>
							</asp:Label>
						</itemtemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="CONSUMO" SortExpression="CONSUMO" HeaderText="Consumo">
					</asp:BoundField>
                    <asp:TemplateField HeaderText="">
					    <headerstyle horizontalalign="Center"></headerstyle>
					    <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
					    <itemtemplate>
					    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("CODLETTURA") %>' alt=""></asp:ImageButton>
						    <asp:HiddenField runat="server" ID="hfCODCONTATORE" Value='<%# Eval("CODCONTATORE") %>' />
                            <asp:HiddenField runat="server" ID="hfCODLETTURA" Value='<%# Eval("CODLETTURA") %>' />
					    </itemtemplate>
				    </asp:TemplateField>				
				</columns>
				</Grd:RibesGridView>
		</form>
	</BODY>
</HTML>
