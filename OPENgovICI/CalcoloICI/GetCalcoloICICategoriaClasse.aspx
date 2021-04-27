<%@ Page language="c#" Codebehind="GetCalcoloICICategoriaClasse.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CalcoloICI.GetCalcoloICICategoriaClasse" %>
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
			<table id="tblgrid" width="100%">
				<tr>
					<!--*** 20120704 - IMU ***-->
					<td>
					    <asp:label id="Label2" runat="server" CssClass="lstTabRow" Width="100%">Riepilogo Importi Calcolo per Categoria e Classe</asp:label>
					</td>
				</tr>
				<tr>
					<td>
					    <asp:label id="lblMessage" runat="server" CssClass="Input_Label"></asp:label>
					</td>
				</tr>
				<tr>
					<td>
					    <!--*** 20140509 - TASI ***-->
					    <Grd:RibesGridView ID="GrdCalcoloICI" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                            OnPageIndexChanging="GrdPageIndexChanging">
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
								<asp:BoundField DataField="CAT" HeaderText="Cat.">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="CL" HeaderText="Classe">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundField>
								<asp:BoundField DataField="num_fabbricati" HeaderText="N&#176; Immobili">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundField>
								<asp:TemplateField HeaderText="Abi. Princ. €">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate><asp:Label id="lblValore" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.Imp_Abi_Princ")) %>'></asp:Label></ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Altri Fab. €">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate><asp:Label id="Label1" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.Imp_Altri_Fab")) %>'></asp:Label></ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText=" Altri Fab. Stato €">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate><asp:Label id="Label3" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.Imp_Altri_Fab_Stato")) %>'></asp:Label></ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Aree Fab. €">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate><asp:Label id="Label4" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.Imp_Aree_Fab")) %>'></asp:Label></ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText=" Aree Fab. Stato €">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate><asp:Label id="Label8" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.Imp_Aree_Fab_Stato")) %>'></asp:Label></ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Terreni €">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate><asp:Label id="Label9" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.Imp_Terreni")) %>'></asp:Label></ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText=" Terreni Stato €">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate><asp:Label id="Label10" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.Imp_Terreni_Stato")) %>'></asp:Label></ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText=" Fab.Rur. €">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate><asp:Label id="Label11" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.Imp_FabRurUsoStrum")) %>'></asp:Label></ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText=" Fab.Rur. Stato €">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate><asp:Label id="Label12" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.Imp_FabRurUsoStrum_stato")) %>'></asp:Label></ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Detrazione €">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate><asp:Label id="Label5" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.Detrazione")) %>'></asp:Label></ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="DSA €" visible="false">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate><asp:Label id="Label7" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.DSA")) %>'></asp:Label></ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Totale €">
									<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
									<ItemTemplate><asp:Label id="Label6" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.Totale")) %>'></asp:Label></ItemTemplate>
								</asp:TemplateField>
							</Columns>
						</Grd:RibesGridView>
						<asp:button id="btnStampaExcel" style="DISPLAY: none" runat="server" Text="Button" onclick="btnStampaExcel_Click"></asp:button>
					</td>
				</tr>
			</table>
            <div id="divDialogBox" class="col-md-12">
                <div class="modal-box">
                    <div id="divAlert" class="modal-alert">
                        <span class="closebtnalert" onclick="CloseAlert()">&times;</span>
                        <p id="pAlert">testo di esempio</p>
                        <input type="text" class="prompttxt"/>
                        <button id="btnDialogBoxeOK" class="confirmbtn Bottone BottoneOK" onclick="DialogConfirmOK();"></button>&nbsp;
                        <button id="btnDialogBoxeKO" class="confirmbtn Bottone BottoneKO" onclick="DialogConfirmKO();"></button>
                        <button id="CmdDialogConfirmOK" class="hidden" onclick="RaiseDialogConfirmOK();"></button>
                        <button id="CmdDialogConfirmKO" class="hidden" onclick="RaiseDialogConfirmKO();"></button>
                        <input type="hidden" id="hfCloseAlert" />
                        <input type="hidden" id="hfDialogOK" />
                        <input type="hidden" id="hfDialogKO" />
                    </div>
                </div>
                <input type="hidden" id="cmdHeight" value="0" />
            </div>
		</form>
	</body>
</HTML>
