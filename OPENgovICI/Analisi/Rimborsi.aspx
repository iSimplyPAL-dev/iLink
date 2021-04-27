<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Rimborsi.aspx.cs" Inherits="DichiarazioniICI.Analisi.Rimborsi" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <title></title>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
    </head>
	<body class="Sfondo">
		<form id="Form1" runat="server" method="post">
		    <div class="SfondoGenerale" style="WIDTH: 100%; HEIGHT: 45px; OVERFLOW: hidden">
				<div class="col-md-6">
					<span class="ContentHead_Title col-md-12" id="infoEnte">
						<asp:Label id="lblTitolo" runat="server"></asp:Label><br />
					</span>
					<span class="NormalBold_title col-md-12" id="info" runat="server" runat="server">ICI/IMU - Rimborsi Stato</span>
				</div>
				<div class="col-md-5" align="right">
                    <input class="Bottone BottoneExcel" id="Print" title="Stampa" onclick="DivAttesa.style.display = ''; document.getElementById('CmdPrint').click();" type="button" name="Print"> 
					<input class="Bottone BottoneRicerca" id="Search" title="Ricerca" onclick="if ($('#ddlAnno').val()!= '') { DivAttesa.style.display = ''; document.getElementById('CmdSearch').click(); } else { GestAlert('a', 'warning', '', '', 'Selezionare un anno!'); }" type="button" name="Search">
				</div>
		    </div>
		    &nbsp;
			<div id="TblRicerca" class="col-md-12">
				<div class="col-md-12">
					<fieldset class="classeFieldSetRicerca col-md-12">
						<div class="col-md-12">
							<p><label class="Input_Label">Anno</label></p>
							<asp:dropdownlist id="ddlAnno" runat="server" CssClass="Input_Text" AutoPostBack="True" Width="120px"></asp:dropdownlist>
                        </div>
					</fieldset>
				</div>
				<div class="col-md-12">
                    <label id="LblResult" class="Legend col-md-12">La ricerca non ha prodotto risultati.</label>                    
                    <Grd:RibesGridView ID="GrdRimborsi" runat="server" BorderStyle="None" 
                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                    AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="20"
                    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                    OnPageIndexChanging="GrdPageIndexChanging">
						<PagerSettings Position="Bottom"></PagerSettings>
                        <PagerStyle CssClass="CartListFooter" />
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
						<Columns>
                            <asp:BoundField DataField="Nominativo" HeaderText="Nominativo">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CFPIva" HeaderText="Cod.Fiscale/P.IVA">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
							<asp:TemplateField HeaderText="Imp. Altri Fab. €">
								<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
								<ItemTemplate>
									<asp:Label id="Label1" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.AltriFab")) %>'>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Imp. Aree Fab. €">
								<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
								<ItemTemplate>
									<asp:Label id="Label2" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.AreeFab")) %>'>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Imp. Terreni €">
								<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
								<ItemTemplate>
									<asp:Label id="Label9" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.Terreni")) %>'>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Imp. Fab.Rur. €">
								<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
								<ItemTemplate>
									<asp:Label runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.fabrurusostrum")) %>' ID="Label11">
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Imp. Uso Prod.Cat.D €">
								<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
								<ItemTemplate>
									<asp:Label runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.UsoProdCatD")) %>' ID="Label13">
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Dovuto €">
								<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
								<ItemTemplate>
									<asp:Label id="Label6" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.Dovuto")) %>'>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Pagato €">
								<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
								<ItemTemplate>
									<asp:Label id="Label5" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.Pagato")) %>'>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Rimborso €">
								<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
								<ItemStyle HorizontalAlign="Right"></ItemStyle>
								<ItemTemplate>
									<asp:Label id="Label7" runat="server" Text='<%# Business.CoreUtility.FormattaGrdEuro(DataBinder.Eval(Container, "DataItem.Rimborso")) %>'>
									</asp:Label>
								</ItemTemplate>
							</asp:TemplateField>
						</Columns>
					</Grd:RibesGridView>
				</div>
				<div class="col-md-12 hidden">
					<fieldset id="DivRiepTotali" class="classeFieldSetRicerca" runat="server">
						<div class="col-md-12">
							<div class="col-md-12">
								<div class="col-md-2"><asp:label id="LblIntestNUtenti" CssClass="Input_Label" Runat="server">N.Utenti</asp:label></div>
								<div class="col-md-2"><asp:label id="LblNUtenti" CssClass="Input_Label" Runat="server"></asp:label></div>
							</div>
							<div class="col-md-12">
								<div class="col-md-2"><asp:label id="LblIntestDovuto" CssClass="Input_Label" Runat="server">Tot.Dovuto</asp:label></div>
								<div class="col-md-2"><asp:label id="LblDovuto" CssClass="Input_Label" Runat="server"></asp:label></div>
								<div class="col-md-2"><asp:label id="LblIntestPagato" CssClass="Input_Label" Runat="server">Tot.Pagato</asp:label></div>
								<div class="col-md-2"><asp:label id="LblPagato" CssClass="Input_Label" Runat="server"></asp:label></div>
								<div class="col-md-2"><asp:label id="LblIntestRimborso" CssClass="Input_Label" Runat="server">Tot.Rimborso</asp:label></div>
								<div class="col-md-2"><asp:label id="LblRimborso" CssClass="Input_Label" Runat="server"></asp:label></div>
							</div>
						</div>
					</fieldset>
				</div>
                <div id="DivAttesa" runat="server" style="z-index: 101; position: absolute;display:none;">
                    <div class="Legend" style="margin-top:40px;">Elaborazione in Corso...</div>
                    <div class="BottoneClessidra">&nbsp;</div>
                    <div class="Legend">Attendere Prego...<asp:Label ID="LblAvanzamento" runat="server"></asp:Label></div>
                </div>
			</div>
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
			<asp:button style="DISPLAY: none" id="CmdSearch" runat="server" OnClick="CmdSearch_Click"></asp:button>
			<asp:button style="DISPLAY: none" id="CmdPrint" runat="server" OnClick="CmdPrint_Click"></asp:button>
		</form>
	</body>
</html>
