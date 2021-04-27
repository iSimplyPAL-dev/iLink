<%@ Page language="c#" Codebehind="ResultRicercaAliquote.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CalcoloICI.ResultRicercaAliquote" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ResultRicercaAliquote</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
	</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0">
		<form id="Form1" runat="server" method="post">
			<br /><!--*** 20140509 - TASI ***--><!--*** 20150430 - TASI Inquilino ***-->
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
					<asp:BoundField DataField="ANNO" HeaderText="Anno">
						<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="DESCRTRIBUTO" HeaderText="Tributo">
						<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
					</asp:BoundField>
					<asp:BoundField DataField="DESCR" HeaderText="Tipo Aliquota">
						<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle HorizontalAlign="left" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="VALORE" HeaderText="Valore">
						<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="ALIQUOTA_STATALE" HeaderText="Statale">
						<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="DESCRSOGLIARENDITA" HeaderText="Soglia Rendita">
						<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="PERCINQUILINO" HeaderText="% Inquilino" DataFormatString="{0:N}">
						<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="">
						<headerstyle horizontalalign="Center"></headerstyle>
						<ItemStyle horizontalalign="Center" Width="40px"></ItemStyle>
						<itemtemplate>
							<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID_ALIQUOTA") %>' alt=""></asp:ImageButton>
					        <asp:HiddenField runat="server" ID="hfID_ALIQUOTA" Value='<%# Eval("ID_ALIQUOTA") %>' />
                            <asp:HiddenField runat="server" ID="hfSOGLIARENDITA" Value='<%# Eval("SOGLIARENDITA") %>' />
						</itemtemplate>
					</asp:TemplateField>
				</Columns>
			</Grd:RibesGridView>
			<!--*** ***-->
			<asp:Label id="lblMessage" runat="server" Width="560px" CssClass="Input_Label"></asp:Label></form>
	</body>
</HTML>
