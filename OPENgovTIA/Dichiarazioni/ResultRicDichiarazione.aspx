<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ResultRicDichiarazione.aspx.vb" Inherits="OPENgovTIA.ResultRicDichiarazione" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>ResultRicDichiarazione</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../../Styles.css" type="text/css" rel="stylesheet">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../_js/skype_killer.js?newversion"></script>
		<script type="text/javascript">
		    function LoadDichiarazione(IdDichiarazione) {
		        parent.parent.Visualizza.DivAttesa.style.display = '';
				parent.parent.Visualizza.location.href = 'GestDichiarazione.aspx?AzioneProv=1&IdUniqueTestata=' + IdDichiarazione
				parent.parent.Comandi.location.href='ComandiGestDichiarazioni.aspx'
    		}
    		window.onload =killSkype;
		</script>
	</head>
	<body class="Sfondo" MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="0" rightMargin="0" style="width:99%">
		<form id="Form1" runat="server" method="post">
			<div class="col-md-12">
				<asp:label id="LblResult" Runat="server" CssClass="Legend">La ricerca non ha prodotto risultati.</asp:label>
                <Grd:RibesGridView ID="GrdUtenti" runat="server" BorderStyle="None" 
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
                        <asp:BoundField DataField="DescrizioneEnte" HeaderText="Ente">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="sCognome" HeaderText="Cognome">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="sNome" HeaderText="Nome">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="sCFPIva" HeaderText="Cod.Fiscale/P.IVA">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="N. e Data Dichiarazione">
                            <HeaderStyle horizontalalign="Center"></HeaderStyle>
                            <ItemStyle horizontalalign="Justify"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblRifDich" Text='<%# DataBinder.Eval(Container, "DataItem.sNdichiarazione") + " - " + FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataDichiarazione"))%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
						<asp:TemplateField HeaderText="Data Inizio">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<asp:Label id="Label4" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.TDATAINIZIO")) %>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Data Fine">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<asp:Label id="Label5" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.TDATAFINE")) %>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Chiusa">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<asp:CheckBox Enabled="False" id="ChkClose" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.chiusa") %>'></asp:CheckBox>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Sel.">
							<HeaderStyle horizontalalign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<asp:CheckBox ID="ChkSel" runat="server" AutoPostBack="true" Checked='<%# DataBinder.Eval(Container, "DataItem.bSel") %>' /></ItemTemplate>
						</asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>' alt=""></asp:ImageButton>
                                <asp:HiddenField ID="hfIdUI" runat="server" Value='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
                <Grd:RibesGridView ID="GrdImmobili" runat="server" BorderStyle="None" 
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
                        <asp:BoundField DataField="DescrizioneEnte" HeaderText="Ente">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
						<asp:TemplateField SortExpression="sIndirizzo" HeaderText="Ubicazione (Rif.Catastali)">
							<ItemTemplate>
								<asp:Label id="Label3" runat="server" text='<%# FncGrd.FormattaVia(DataBinder.Eval(Container, "DataItem.sVia"), DataBinder.Eval(Container, "DataItem.scivico"), DataBinder.Eval(Container, "DataItem.sInterno"), DataBinder.Eval(Container, "DataItem.sEsponente"), DataBinder.Eval(Container, "DataItem.sscala"), DataBinder.Eval(Container, "DataItem.sfoglio"), DataBinder.Eval(Container, "DataItem.snumero"), DataBinder.Eval(Container, "DataItem.ssubalterno")) %>'>Label</asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
                        <asp:BoundField DataField="sCognome" HeaderText="Cognome">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="sNome" HeaderText="Nome">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="sCFPIva" HeaderText="Cod.Fiscale/P.IVA">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="N. e Data Dichiarazione">
                            <HeaderStyle horizontalalign="Center"></HeaderStyle>
                            <ItemStyle horizontalalign="Justify"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="Label1" Text='<%# DataBinder.Eval(Container, "DataItem.sNdichiarazione") + " - " + FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.tDataDichiarazione"))%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
						<asp:TemplateField HeaderText="Data Inizio">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<asp:Label id="Label6" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.TDATAINIZIO")) %>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Data Fine">
							<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<asp:Label id="Label7" runat="server" Text='<%# FncGrd.FormattaDataGrd(DataBinder.Eval(Container, "DataItem.TDATAFINE")) %>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Chiusa">
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<asp:CheckBox Enabled="False" id="ChkClose" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.chiusa") %>'></asp:CheckBox>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Sel.">
							<HeaderStyle horizontalalign="Center"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<ItemTemplate>
								<asp:CheckBox ID="ChkSel" runat="server" AutoPostBack="true" Checked='<%# DataBinder.Eval(Container, "DataItem.bSel") %>' /></ItemTemplate>
						</asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="40px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>' alt=""></asp:ImageButton>
                                <asp:HiddenField ID="hfIdUI" runat="server" Value='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Grd:RibesGridView>
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
			<asp:Button ID="CmdStampa" Runat="server" style="DISPLAY: none"></asp:Button>
			<asp:Button ID="CmdStampaSintetica" Runat="server" style="DISPLAY: none"></asp:Button>
			<asp:Button ID="CmdStampaAnalitica" Runat="server" style="DISPLAY: none"></asp:Button>
		</form>
	</body>
</HTML>
