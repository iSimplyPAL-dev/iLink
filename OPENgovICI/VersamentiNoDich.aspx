<%@ Page language="c#" Codebehind="VersamentiNoDich.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.VersamentiNoDich" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>VersamentiNoDich</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1"){%> 
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%}%> 
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
        <script type="text/javascript">
            function estraiExcel() {
                if (document.getElementById('GrdRisultati') == null) {
                    GestAlert('a', 'warning', '', '', 'Impossibile accedere alla funzionalità di estrazione elenco in formato excel.\n Eseguire la ricerca.');
                }
                else {
                    document.getElementById('btnStampaExcel').click();
                }

                return false;
            }
        </script>
  </HEAD>
	<body MS_POSITIONING="GridLayout" class="Sfondo">
		<form id="Form1" runat="server" method="post">
			<table id="tblCorpo" cellSpacing="1" cellPadding="1" width="100%" align="center" border="0">
				<tr>
					<td>
						<fieldset class="classeFiledSetRicerca"><legend class="Legend">Inserimento parametri di ricerca - Versamenti non abbinati a Dichiarazioni</legend>
							<table id="tblFiltri" cellSpacing="1" cellPadding="5" width="100%" border="0">
								<tr>
									<td>
										<table cellSpacing="0" cellPadding="0">
											<tr>
												<td><label class="Input_Label">Anno riferimento</label><br />
													<asp:dropdownlist id="ddlAnno" runat="server" CssClass="Input_Text"></asp:dropdownlist>
													<asp:button id="btnTrova" runat="server" style="display:none" Text="Trova" ToolTip="Permette di eseguire una ricerca in funzione dei filtri utilizzati" onclick="btnTrova_Click"></asp:button>
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td><br />
						&nbsp;<asp:label id="lblRisultati" runat="server" CssClass="Legend">Risultati della Ricerca</asp:label>
						<br />
					</td>
				</tr>
				<tr>
					<td>
						<fieldset class="classeFiledSetNoBorder">
							<table id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0">
								<tr>
									<td>
										<Grd:RibesGridView ID="GrdRisultati" runat="server" BorderStyle="None" 
                                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                            AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="18"
                                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                            OnPageIndexChanging="GrdPageIndexChanging">
                                            <PagerSettings Position="Bottom"></PagerSettings>
                                            <PagerStyle CssClass="CartListFooter" />
                                            <RowStyle CssClass="CartListItem"></RowStyle>
                                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
											<Columns>
												<asp:TemplateField HeaderText="Anno">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
													<ItemTemplate>
														<asp:Label id=lblAnnoRiferimento runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AnnoRiferimento") %>'></asp:Label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Contribuente">
													<ItemTemplate>
														<asp:Label id=lblContribuente runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CONTRIBUENTE") %>'></asp:Label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Im. Pagato">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
													<ItemTemplate>
														<asp:label id="Label6" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ImportoPagato")) %>'></asp:label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Acconto/Saldo">
													<ItemTemplate>
														<asp:Label id=lblSaldo runat="server" Text='<%# Business.CoreUtility.FormattaGrdAccontoSaldo(DataBinder.Eval(Container, "DataItem.Saldo"), DataBinder.Eval(Container, "DataItem.Acconto")) %>'></asp:Label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Imp. Terreni">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
													<ItemTemplate>
														<asp:label id="Label2" runat="server" Text='<%# Business.CoreUtility.FormattaGrdSumEuro(DataBinder.Eval(Container, "DataItem.ImpoTerreni"),DataBinder.Eval(Container, "DataItem.IMPORTOTERRENISTATALE")) %>'>
														</asp:label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Imp. Aree Fabb.">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
													<ItemTemplate>
														<asp:label id="Label1" runat="server" Text='<%# Business.CoreUtility.FormattaGrdSumEuro(DataBinder.Eval(Container, "DataItem.ImportoAreeFabbric"),DataBinder.Eval(Container, "DataItem.IMPORTOAREEFABBRICSTATALE")) %>'>
														</asp:label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Imp. Altri Fabb.">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
													<ItemTemplate>
														<asp:label id="Label3" runat="server" Text='<%# Business.CoreUtility.FormattaGrdSumEuro(DataBinder.Eval(Container, "DataItem.ImportoAltriFabbric"),DataBinder.Eval(Container, "DataItem.IMPORTOALTRIFABBRICSTATALE")) %>'>
														</asp:label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Imp. Fab.Rurali">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
													<ItemTemplate>
														<asp:label id="Label9" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.IMPORTOFABRURUSOSTRUM")) %>'>
														</asp:label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Imp. Abi. Princ.">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
													<ItemTemplate>
														<asp:label id="Label4" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.ImportoAbitazPrincipale")) %>'></asp:label>
													</ItemTemplate>
												</asp:TemplateField>
												<asp:TemplateField HeaderText="Detrazione">
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
													<ItemTemplate>
														<asp:label id="Label5" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.DetrazioneAbitazPrincipale")) %>'></asp:label>
													</ItemTemplate>
												</asp:TemplateField>
											</Columns>
										</Grd:RibesGridView>
										<asp:button id="btnStampaExcel" runat="server" Text="Button" style="display:none" onclick="btnStampaExcel_Click"></asp:button>
									</td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
				</tr>
				<tr>
				</tr>
			</table>
		</form>
	</body>
</HTML>
