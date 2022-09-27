<%@ Page language="c#" Codebehind="ElencoAliquote.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CalcoloICI.ElencoAliquote" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<head>
		<title>ResultRicercaAliquote</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		    function setHeight(e) {
		        e.height = e.contentWindow.document.body.scrollHeight + 35;
		    }
		</script>
	</head>
	<body class="Sfondo" bottomMargin="0" leftMargin="0" topMargin="0">
		<form id="Form1" runat="server" method="post" style="padding:0;margin:0">
			<!--*** 20130422 - aggiornamento IMU ***--><!--*** 20140509 - TASI ***--><!--*** 20150430 - TASI Inquilino ***-->
			<Grd:RibesGridView ID="GrdCategorie" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                OnPageIndexChanging="GrdPageIndexChanging">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
				<Columns>
					<asp:BoundField DataField="DESCRTRIBUTO" HeaderText="Tributo">
						<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="DESCR" HeaderText="Tipo Aliquota">
						<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle HorizontalAlign="left" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="VALORE" HeaderText="Percentuale/Valore">
						<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="ALIQUOTA_STATALE" HeaderText="Percentuale/Valore Stat.">
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
					<asp:BoundField DataField="TOT_COMUNE" HeaderText="Importo €" DataFormatString="{0:N}">
						<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="TOT_STATALE" HeaderText="Importo Stat. €" DataFormatString="{0:N}">
						<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
				</Columns>
			</Grd:RibesGridView>
			<asp:Label id="lblMessage" runat="server" Width="560px" CssClass="Input_Label"></asp:Label>
		</form>
	</body>
</HTML>
