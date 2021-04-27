<%@ Page language="c#" Codebehind="GetBollettinoICI.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CalcoloICI.GetBollettinoICI" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<head>
		<title>GetCalcoloICI</title>
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
	</head>
	<body class="Sfondo" MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="0" topMargin="0">
		<form id="Form1" runat="server" method="post">
			<table id="tblgrid" style="POSITION: absolute; TOP: 2px; LEFT: 2px" width="99%">
				<tr>
					<!--*** 20120704 - IMU ***--><!--*** 20130422 - aggiornamento IMU ***-->
					<td><asp:label id="Label2" runat="server" CssClass="lstTabRow" Width="100%">Riepilogo Bollettino</asp:label></td>
				</tr>
				<tr>
					<td>
					    <asp:label id="lblMessage" runat="server" CssClass="Input_Label" Visible="false">Il dovuto per le aree edificabili è stato forzato secondo il versato dell'anno precedente</asp:label>
					</td>
				</tr>
				<tr>
					<td>
                        <Grd:RibesGridView ID="GrdBollettinoICI" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <PagerStyle CssClass="CartListFooter" />
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
							<Columns>
					            <asp:BoundField DataField="DESCRTRIBUTO" HeaderText="Tributo">
						            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						            <ItemStyle HorizontalAlign="Center"></ItemStyle>
					            </asp:BoundField>
								<asp:BoundField DataField="DESCRIZIONE" HeaderText="Descrizione">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="Imp. Abi. Princ. €">
									<HeaderStyle HorizontalAlign="Center" Width="90px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblABPR" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMP_ABI_PRINC")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Imp. Terr. Agr. €">
									<HeaderStyle HorizontalAlign="Center" Width="90px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblTEAG" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMP_TERRENI")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Imp. Terr. Agr. Stato €">
									<HeaderStyle HorizontalAlign="Center" Width="90px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblTEAGStato" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMP_TERRENISTATO")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Imp. Altri Fab. €">
									<HeaderStyle HorizontalAlign="Center" Width="90px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblALFA" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMP_ALRI_FAB")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Imp. Altri Fab. Stato €">
									<HeaderStyle HorizontalAlign="Center" Width="90px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblALFAStato" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMP_ALTRI_FABSTATO")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Imp. Aree Fab. €">
									<HeaderStyle HorizontalAlign="Center" Width="90px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblARFA" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMP_AREE_FAB")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Imp. Aree Fab. Stato €">
									<HeaderStyle HorizontalAlign="Center" Width="90px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblARFAStato" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMP_AREE_FABSTATO")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Imp. Fab. Rur. €">
									<HeaderStyle HorizontalAlign="Center" Width="90px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblFabRurUsoStrum" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMP_FABRURUSOSTRUM")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Imp. Fab. Rur. Stato €">
									<HeaderStyle HorizontalAlign="Center" Width="90px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblFabRurUsoStrumStato" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMP_FABRURUSOSTRUMSTATO")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Imp. Uso Prod.Cat.D €">
									<HeaderStyle HorizontalAlign="Center" Width="90px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblUsoProdCatD" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMP_USOPRODCATD")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Imp. Uso Prod.Cat.D Stato €">
									<HeaderStyle HorizontalAlign="Center" Width="90px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate>
										<asp:Label id="lblUsoProdCatDStato" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMP_USOPRODCATDSTATO")) %>'>
										</asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Imp. Detraz. €">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<itemtemplate>
										<asp:Label id="Label5" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMP_DET")) %>'>
										</asp:Label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Imp. DSA €" visible="false">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<itemtemplate>
										<asp:Label id="Label9" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMP_DET_DSA")) %>'>
										</asp:Label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Imp.Senza Arr. €">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<itemtemplate>
										<asp:Label id="Label6" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMP_S_ARR")) %>'>
										</asp:Label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Imp. Arr. €">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<itemtemplate>
										<asp:Label id="Label7" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMP_ARROT")) %>'>
										</asp:Label>
									</itemtemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Imp. Totale €">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<itemtemplate>
										<asp:Label id="Label8" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMP_TOTALE")) %>'>
										</asp:Label>
									</itemtemplate>
								</asp:TemplateField>
							</Columns>
						</Grd:RibesGridView>
					</td>
				</tr>
			</table>
			<asp:button id="btnStampaExcel" style="DISPLAY: none" runat="server" Text="Button"></asp:button>
			<asp:button id="btnExtract" style="DISPLAY: none" runat="server" Text="Button"></asp:button>
		</form>
	</body>
</HTML>
