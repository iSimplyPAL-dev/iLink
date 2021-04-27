<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConfVani.aspx.vb" Inherits="OPENgovTIA.ConfVani" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title></title>
		<script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    </head>
	<body class="Sfondo" MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<div id="divComandi" class="SfondoGenerale" style="WIDTH: 100%; HEIGHT: 45px; OVERFLOW: hidden">
				<div class="col-md-6">
					<span class="ContentHead_Title col-md-12" id="infoEnte">
						<asp:Label id="lblTitolo" runat="server"></asp:Label><br />
					</span>
					<span class="NormalBold_title col-md-12" id="Span1" runat="server" runat="server">
                        <asp:Label id="info" runat="server">- Configurazioni - Visualizzatore Vani</asp:Label>
                    </span>
				</div>
				<div class="col-md-5" align="right">
					<input class="Bottone BottoneRicerca" id="Search" title="Ricerca" onclick="document.getElementById('CmdSearch').click()" type="button" name="Search">
				</div>
			</div>
			&nbsp;
			<div id="divRic" class="col-md-12">
				<div id="divResult" class="col-md-12">
                    <asp:label id="LblResult" CssClass="Legend" Runat="server">Risultati della Ricerca</asp:label>
			        <Grd:RibesGridView ID="GrdResult" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="30"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnPageIndexChanging="GrdPageIndexChanging">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
				        <Columns>
					        <asp:BoundField DataField="IDTIPOVANO" HeaderText="Codice">
						        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
						        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
					        </asp:BoundField>
					        <asp:BoundField DataField="DESCRIZIONE" HeaderText="Descrizione">
						        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
					        </asp:BoundField>
				        </Columns>
			        </Grd:RibesGridView>
				</div>
			</div>
			<asp:button id="CmdSearch" style="DISPLAY: none" runat="server"></asp:button>
		</form>
	</body>
</html>
