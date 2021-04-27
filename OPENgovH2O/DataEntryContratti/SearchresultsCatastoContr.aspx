<%@ Register TagPrefix="ribesinformaticadatagrid" Namespace="RibesDataGrid.RibesDataGrid" Assembly="RibesDataGrid" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchresultsCatastoContr.aspx.vb" Inherits="OpenUtenze.SearchresultsCatastoContr"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>SearchResultsCatasto</title>
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
		<script language="javascript" type="text/javascript">
		function ApriModifica(parametro,parametro2)
		{
		myleft=((screen.width)/2)-350;
		mytop=((screen.height)/2)-250;	
		window.open("FrameModificaDatoCatasto.aspx?IDCatasto="+parametro+"&IDContatore="+parametro2,"Catasto","width=700, height=500, toolbar=no,top="+mytop+",left="+myleft+", menubar=no");
		}
		</script>
	</HEAD>
	<BODY class="Sfondo_Giallo" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0"
		marginwidth="0" marginheight="0">
		<form id="Form1" method="post" runat="server">
			<br>
			<RIBESINFORMATICADATAGRID:RIBESDATAGRID id="RibesDataGrid" onmouseover="this.className='riga_tabella_mouse_over'" onmouseout="this.className='riga_tabella'"
				runat="server" Width="600px" BorderWidth="1px" CellPadding="3" BorderStyle="Solid" AlternatingItemColor="White" ItemColor="WhiteSmoke"
				OnSelectedIndexChanged="RibesDataGrid_SelectedIndexChanged" AllowCustomPaging="True" AutoGenerateColumns="False" star_index="0"
				strNameTable="Table" strFooter="Contatori" blnUseStoreProcedure="False" blnUseQuery="True" EventClick="OnClick" blnUseView="False"
				BackColor="White" BorderColor="Gainsboro" PathImages="../gfx/medium_blackarrow/">
				<AlternatingItemStyle CssClass="CartListItemAlt"></AlternatingItemStyle>
				<FooterStyle HorizontalAlign="Center" CssClass="cartlistfooter"></FooterStyle>
				<ItemStyle CssClass="CartListItem"></ItemStyle>
				<Columns>
					<asp:BoundColumn DataField="IDCont_catas" HeaderText="ID"></asp:BoundColumn>
					<asp:BoundColumn DataField="CODCONTATORE"></asp:BoundColumn>
					<asp:BoundColumn DataField="INTERNO" HeaderText="Interno">
						<HeaderStyle VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle VerticalAlign="Middle"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="PIANO" HeaderText="Piano">
						<HeaderStyle VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle VerticalAlign="Middle"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="FOGLIO" HeaderText="Foglio">
						<HeaderStyle VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle VerticalAlign="Middle"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="NUMERO" HeaderText="Numero">
						<HeaderStyle VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle VerticalAlign="Middle"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="SUBALTERNO" HeaderText="Subalterno">
						<HeaderStyle VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle VerticalAlign="Middle"></ItemStyle>
					</asp:BoundColumn>
				</Columns>
				<HeaderStyle Font-Bold="True" CssClass="CartListHead"></HeaderStyle>
				<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="Gainsboro"></SelectedItemStyle>
				<PagerStyle HorizontalAlign="Left" CssClass="Paging" Mode="NumericPages"></PagerStyle>
			</RIBESINFORMATICADATAGRID:RIBESDATAGRID>
			<div align="center"><asp:label id="lblMessage" CssClass="NormalRed" Runat="server"></asp:label></div>
		</form>
	</BODY>
</HTML>
