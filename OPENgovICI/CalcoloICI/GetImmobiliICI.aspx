<%@ Page language="c#" Codebehind="GetImmobiliICI.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.GetImmobiliICI" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
  <head>
		<title>GetImmobiliICI</title>
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
  </head>
	<body class="Sfondo" MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="0" topMargin="0">
		<form id="Form1" runat="server" method="post">
			<table id="tblgrid" style="LEFT: 2px; POSITION: absolute; TOP: 2px" width="99%">
				<tr>
					<!--*** 20120704 - IMU ***--><!--*** 20130422 - aggiornamento IMU ***-->
					<td><asp:label id="lbltitolo1" runat="server" CssClass="lstTabRow" Width="100%">Riepilogo Immobili per Calcolo</asp:label></td>
				</tr>
				<tr>
					<td>
					    <Grd:RibesGridView ID="GrdImmobiliICI" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                            OnPageIndexChanging="GrdPageIndexChanging">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							<columns>							
					            <asp:BoundField DataField="DESCRTRIBUTO" HeaderText="Tributo">
						            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						            <ItemStyle HorizontalAlign="Center"></ItemStyle>
					            </asp:BoundField>
								<asp:BoundField DataField="FOGLIO" HeaderText="Foglio">
									<headerstyle horizontalalign="Center" width="50px"></HeaderStyle>
									<itemstyle horizontalalign="Center"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="NUMERO" HeaderText="Numero">
									<headerstyle horizontalalign="Center" width="50px"></HeaderStyle>
									<itemstyle horizontalalign="Center"></ItemStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="Sub">
									<headerstyle horizontalalign="Center" width="50px"></HeaderStyle>
									<itemstyle horizontalalign="Center"></ItemStyle>
									<itemtemplate>
										<asp:Label id="lblSubalterno" runat="server" Text='<%# Business.CoreUtility.FormattaGrdInt(DataBinder.Eval(Container, "DataItem.SUBALTERNO")) %>'></asp:label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="CATEGORIA" HeaderText="Cat.">
									<headerstyle horizontalalign="Center" width="60px"></HeaderStyle>
									<itemstyle horizontalalign="Center"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="CLASSE" HeaderText="Classe">
									<headerstyle horizontalalign="Center" width="60px"></HeaderStyle>
									<itemstyle horizontalalign="Center"></ItemStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="Valore">
									<headerstyle horizontalalign="Center" width="80px"></HeaderStyle>
									<itemstyle horizontalalign="Right"></ItemStyle>
									<itemtemplate>
										<asp:Label id="lblValore" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.Valore")) %>'></asp:label>
									</itemtemplate>
								</asp:TemplateField>								
								<asp:BoundField DataField="INDIRIZZONEW" HeaderText="Indirizzo">
									<headerstyle width="150px"></HeaderStyle>
								</asp:BoundField>
								<asp:BoundField DataField="TIPO_RENDITA" HeaderText="Tipo Rendita">
									<headerstyle horizontalalign="Center" width="60px"></HeaderStyle>
									<itemstyle horizontalalign="Center"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="PERC_POSSESSO" HeaderText="% Pos.">
									<headerstyle horizontalalign="Center"></HeaderStyle>
									<itemstyle horizontalalign="Right"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="MESI_POSSESSO" HeaderText="Mesi Pos.">
									<headerstyle horizontalalign="Center"></HeaderStyle>
									<itemstyle horizontalalign="Center"></ItemStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="Abit. Princ.">
									<headerstyle horizontalalign="Center"></HeaderStyle>
									<itemstyle horizontalalign="Center"></ItemStyle>
									<itemtemplate>
										<asp:Label runat="server" Text='<%# Business.CoreUtility.FormattaGrdAbiPrinc(DataBinder.Eval(Container, "DataItem.FLAG_PRINCIPALE")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:BoundField DataField="NUMERO_UTILIZZATORI" HeaderText="N.Utiliz.">
									<headerstyle horizontalalign="Center"></HeaderStyle>
									<itemstyle horizontalalign="Center"></ItemStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="Ridotto">
									<headerstyle horizontalalign="Center"></HeaderStyle>
									<itemstyle horizontalalign="Center"></ItemStyle>
									<itemtemplate>
										<asp:Label runat="server" Text='<%# Business.CoreUtility.FormattaGrdBoolToString(1,DataBinder.Eval(Container, "DataItem.FLAG_RIDUZIONE")) %>'>
										</asp:Label>
									</ItemTemplate>
									<edititemtemplate>
										<asp:TextBox runat="server" Text='<%# Business.CoreUtility.FormattaGrdBoolToString(1,DataBinder.Eval(Container, "DataItem.FLAG_RIDUZIONE")) %>'>
										</asp:TextBox>
									</EditItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Esente">
									<headerstyle horizontalalign="Center"></HeaderStyle>
									<itemstyle horizontalalign="Center"></ItemStyle>
									<itemtemplate>
										<asp:Label runat="server" Text='<%# Business.CoreUtility.FormattaGrdBoolToString(1,DataBinder.Eval(Container, "DataItem.FLAG_ESENTE")) %>'>
										</asp:Label>
									</ItemTemplate>
									<edititemtemplate>
										<asp:TextBox runat="server" Text='<%# Business.CoreUtility.FormattaGrdBoolToString(1,DataBinder.Eval(Container, "DataItem.FLAG_ESENTE")) %>'>
										</asp:TextBox>
									</EditItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Colt.Dir.">
									<headerstyle horizontalalign="Center"></HeaderStyle>
									<itemstyle horizontalalign="Center"></ItemStyle>
									<itemtemplate>
										<asp:Label runat="server" Text='<%# Business.CoreUtility.FormattaGrdBoolToString(0,DataBinder.Eval(Container, "DataItem.COLTIVATOREDIRETTO")) %>' ID="Label1" NAME="Label1">
										</asp:Label>
									</ItemTemplate>
									<edititemtemplate>
										<asp:TextBox runat="server" Text='<%# Business.CoreUtility.FormattaGrdBoolToString(0,DataBinder.Eval(Container, "DataItem.COLTIVATOREDIRETTO")) %>' ID="Textbox1" NAME="Textbox1">
										</asp:TextBox>
									</EditItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Figli">
									<headerstyle horizontalalign="Center"></HeaderStyle>
									<itemstyle horizontalalign="Right"></ItemStyle>
									<itemtemplate>
										<asp:Label id="Label2" runat="server" Text='<%# Business.CoreUtility.FormattaGrdCaricoFigli(DataBinder.Eval(Container, "DataItem.NUMEROFIGLI"),DataBinder.Eval(Container, "DataItem.PERCENTCARICOFIGLI")) %>'></asp:label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Aliquota">
									<headerstyle horizontalalign="Center"></HeaderStyle>
									<itemstyle horizontalalign="Right"></ItemStyle>
									<itemtemplate>
										<asp:Label id="Label3" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ICI_VALORE_ALIQUOTA")) %>'></asp:label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Aliquota Stat.">
									<headerstyle horizontalalign="Center"></HeaderStyle>
									<itemstyle horizontalalign="Right"></ItemStyle>
									<itemtemplate>
										<asp:Label id="Label4" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ICI_VALORE_ALIQUOTA_STATALE")) %>'></asp:label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Dovuto €">
									<headerstyle horizontalalign="Center" width="80px"></HeaderStyle>
									<itemstyle horizontalalign="Right"></ItemStyle>
									<itemtemplate>
										<asp:Label id="Label5" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ICI_TOTALE_DOVUTA")) %>'></asp:label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Dovuto Stat. €">
									<headerstyle horizontalalign="Center" width="80px"></HeaderStyle>
									<itemstyle horizontalalign="Right"></ItemStyle>
									<itemtemplate>
										<asp:Label id="lblTotaleDovuto" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ICI_TOTALE_DOVUTA_STATALE")) %>'></asp:label>
									</itemtemplate>
								</asp:TemplateField>
							</Columns>
						</Grd:RibesGridView>
                    </td>
				</tr>
			</table>
		</form>
	</body>
</html>
