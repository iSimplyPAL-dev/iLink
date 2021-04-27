<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SearchAccertato.aspx.vb" Inherits="Provvedimenti.SearchAccertato" EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>SearchAccertato</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
		function Associa(){
			document.getElementById ("btnAssocia").click ()
		}
		function Associa1(){
			parent.opener.document.getElementById ("loadGridAccertato").src ="grdAccertato.aspx";
			parent.window.close();
		}
		function Esci(){
			document.getElementById ("btnEsci").click ()
			parent.window.close();
			parent.opener.focus();
		}
		function DeleteAtto(){
			if (confirm('Confermi la cancellazione dell\'atto selezionato\ne di tutti gli immobili dichiarati associati?\n\nNon sara\' piu\' possibile recuperare i dati.'))
				return true
			else
				return false
		}
		</script>
	</HEAD>
	<body class="Sfondo" bottomMargin="0" leftMargin="20" rightMargin="0" topMargin="5">
		<form id="Form1" runat="server" method="post">
            <div id="divHeader"></div><br />
            <Grd:RibesGridView ID="GrdAtti" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                OnRowCommand="GrdRowCommand">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
				<Columns>
					<asp:BoundField DataField="ANNO" SortExpression="ANNO" HeaderText="Anno">
						<HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" Width="5%" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="TRIBUTO" HeaderText="Descrizione Provvedimento">
						<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left" Width="40%" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="NUMERO_ATTO" HeaderText="Numero Atto">
						<HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" Width="15%" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Data Creazione">
						<HeaderStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center" Width="10%" VerticalAlign="Middle"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label3" runat="server" text='<%# FncGrd.GiraData(DataBinder.Eval(Container, "DataItem.DATA_ELABORAZIONE"))%>'>Label</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="IMPORTO_TOTALE" HeaderText="Importo Totale €" DataFormatString="{0:#,##0.00}">
						<HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" Width="15%" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="STATO" HeaderText="Stato Avviso"></asp:BoundField>
					<asp:TemplateField HeaderText="Del">
						<ItemStyle Font-Italic="True" HorizontalAlign="Left" ForeColor="Red" Width="15%" VerticalAlign="Middle"></ItemStyle>
						<ItemTemplate>
							<asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneCancellaGrd" CausesValidation="False" CommandName="RowDelete" CommandArgument='<%# Eval("ID_PROVVEDIMENTO") %>' alt=""/>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
                        <itemtemplate>
                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CausesValidation="False" CommandName="RowBind" CommandArgument='<%# Eval("ID_PROVVEDIMENTO") %>' alt=""/>
                            <asp:HiddenField ID="hfIdProv" runat="server" Value='<%# Eval("ID_PROVVEDIMENTO") %>' />
                            <asp:HiddenField ID="hfCodTipoProv" runat="server" Value='<%# Eval("COD_TIPO_PROVVEDIMENTO") %>' />
                            <asp:HiddenField ID="hfCodTipoProc" runat="server" Value='<%# Eval("COD_TIPO_PROCEDIMENTO") %>' />
                            <asp:HiddenField ID="hfIdTributo" runat="server" Value='<%# Eval("COD_TRIBUTO") %>' />
                            <asp:HiddenField ID="hfIdContrib" runat="server" Value='<%# Eval("COD_CONTRIBUENTE") %>' />
                            <asp:HiddenField ID="hfIdProc" runat="server" Value='<%# Eval("ID_PROCEDIMENTO") %>' />
                            <asp:HiddenField ID="hfRettifica" runat="server" Value='<%# Eval("RETTIFICA") %>' />
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </asp:TemplateField>
				</Columns>
			</Grd:RibesGridView><br>
			<p><asp:label id="lblNotFound" runat="server" CssClass="NormalRed" Visible="False">NO</asp:label></p>			
            <Grd:RibesGridView ID="GrdImmobiliICI" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                OnRowCommand="GrdRowCommand">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
				<Columns>
					<asp:BoundField DataField="Progressivo" ReadOnly="True" HeaderText="Prg">
						<HeaderStyle Width="10px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Dal">
						<HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="lblDal" runat="server" Text='<%# FncGrd.annoBarra(DataBinder.Eval(Container, "DataItem.DAL")) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Al">
						<HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="lblAl" runat="server" Text='<%# FncGrd.annoBarra(DataBinder.Eval(Container, "DataItem.AL")) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="Foglio" ReadOnly="True" HeaderText="Fg">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="Numero" ReadOnly="True" HeaderText="N&#176;">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Sub">
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# FncGrd.IntForGridView(DataBinder.Eval(Container, "DataItem.Subalterno")) %>' ID="Label1" NAME="Label1">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField headertext="Cat">
						<headerstyle horizontalalign="Center" width="15px"></headerstyle>
						<itemtemplate>
							<asp:label runat="server" text='<%# FncGrd.IntForGridView(DataBinder.Eval(Container, "DataItem.Categoria")) %>' id="Label5">
							</asp:label>
						</itemtemplate>
					</asp:TemplateField>
					<asp:TemplateField headertext="Cl">
						<headerstyle horizontalalign="Center" width="15px"></headerstyle>
						<itemtemplate>
							<asp:label runat="server" text='<%# FncGrd.IntForGridView(DataBinder.Eval(Container, "DataItem.CLASSE")) %>' id="Label6">
							</asp:label>
						</itemtemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Cons">
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# FncGrd.FormattaNumero(DataBinder.Eval(Container, "DataItem.Consistenza"),2) %>' ID="Label2" NAME="Label2">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="TipoRendita" ReadOnly="True" HeaderText="TR">
						<HeaderStyle Width="10px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField visible="true" datafield="Zona" readonly="True" headertext="Zona"></asp:BoundField>
					<asp:TemplateField headertext="Rendita">
						<headerstyle horizontalalign="Center" width="25px"></headerstyle>
						<itemstyle horizontalalign="Right"></itemstyle>
						<itemtemplate>
							<asp:label id="lblRendita" runat="server" text='<%# FncGrd.FormattaNumero(DataBinder.Eval(Container, "DataItem.Rendita"),2) %>'>
							</asp:label>
						</itemtemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Valore">
						<HeaderStyle HorizontalAlign="Center" Width="25px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="lblRendita_Valore" runat="server" Text='<%# FncGrd.FormattaNumero(DataBinder.Eval(Container, "DataItem.Valore"), 2) %>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="percPossesso" ReadOnly="True" HeaderText="% Poss">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="TITPOSSESSO" ReadOnly="True" HeaderText="Tit.Poss">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="NUtilizzatori" ReadOnly="True" HeaderText="N. Utiliz">
						<HeaderStyle Width="20px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Princ">
						<HeaderStyle Width="25px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id="chkPrinc" runat="server" Checked='<%# FncGrd.checkFlag(DataBinder.Eval(Container, "DataItem.FLAGPRINCIPALE")) %>' Enabled="False">
							</asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Pert">
						<HeaderStyle Width="25px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id="chkPert" runat="server" Checked='<%# FncGrd.checkPertinenza(DataBinder.Eval(Container, "DataItem.IdImmobilePertinenza"), DataBinder.Eval(Container, "DataItem.ID")) %>' Enabled="False"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Rid">
						<HeaderStyle Width="25px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:CheckBox id="chkRidotto" runat="server" Checked='<%# FncGrd.checkMesiRiduzione(DataBinder.Eval(Container, "DataItem.mesiRiduzione")) %>' Enabled="False">
							</asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField>
                        <itemtemplate>
                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneAssociaGrd" CausesValidation="False" CommandName="RowBind" CommandArgument='<%# Eval("IDLEGAME") %>' alt=""/>
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </asp:TemplateField>
				</Columns>
			</Grd:RibesGridView>
            <Grd:RibesGridView ID="GrdImmobiliTARSU" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                OnRowCommand="GrdRowCommand">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
				<Columns>
					<asp:BoundField DataField="Progressivo" ReadOnly="True" HeaderText="Prg">
						<HeaderStyle Width="10px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Dal">
						<HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# FncGrd.annoBarra(DataBinder.Eval(Container, "DataItem.tDataInizio")) %>' ID="Label7">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Al">
						<HeaderStyle HorizontalAlign="Center" Width="70px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Center"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# FncGrd.annoBarra(DataBinder.Eval(Container, "DataItem.tDataFine")) %>' ID="Label8">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="sVia" ReadOnly="True" HeaderText="Via">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Left"></ItemStyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Civico">
						<HeaderStyle HorizontalAlign="Left" Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
						<ItemTemplate>
							<asp:Label runat="server" Text='<%# FncGrd.FormattaNumeriGrd(DataBinder.Eval(Container, "DataItem.scivico"))%>' ID="Label29">
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="sInterno" ReadOnly="True" HeaderText="Interno">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="sfoglio" ReadOnly="True" HeaderText="Foglio">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="snumero" ReadOnly="True" HeaderText="Numero">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="ssubalterno" ReadOnly="True" HeaderText="Sub.">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="sdescrCategoria" ReadOnly="True" HeaderText="Cat.">
						<headerstyle width="15px"></HeaderStyle>
						<itemstyle horizontalalign="Left" verticalalign="Middle"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="ImpTariffa" ReadOnly="True" HeaderText="Tariffa">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField DataField="nMq" ReadOnly="True" HeaderText="Mq">
						<HeaderStyle Width="15px"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
					</asp:BoundField>
					<asp:BoundField visible="false" datafield="calcola_interessi" readonly="True" headertext="Calcola_Interessi"></asp:BoundField>
					<asp:BoundField visible="false" datafield="Sanzioni" readonly="True" headertext="Sanzioni"></asp:BoundField>
					<asp:BoundField visible="false" datafield="sDescrSanzioni" readonly="True" headertext="DescrSanzioni"></asp:BoundField>
					<asp:TemplateField>
                        <itemtemplate>
                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneAssociaGrd" CausesValidation="False" CommandName="RowBind" CommandArgument='<%# Eval("Progressivo") %>' alt=""/>
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </asp:TemplateField>
				</Columns>
			</Grd:RibesGridView>
            <Grd:RibesGridView ID="GrdImmobiliOSAP" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                OnRowCommand="GrdRowCommand">
                <PagerSettings Position="Bottom"></PagerSettings>
                <PagerStyle CssClass="CartListFooter" />
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
				<Columns>
					<asp:BoundField DataField="Progressivo" ReadOnly="True" HeaderText="Prg">
						<headerstyle width="10px"></headerstyle>
						<itemstyle horizontalalign="Center"></itemstyle>
					</asp:BoundField>
					<asp:TemplateField HeaderText="Ubicazione (Rif.Catastali)">
						<ItemStyle Width="300px"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label9" runat="server" text='<%# OPENgovTOCO.SharedFunction.FormattaVia(DataBinder.Eval(Container, "DataItem.sVia"),DataBinder.Eval(Container, "DataItem.Civico"),DataBinder.Eval(Container, "DataItem.Interno"),DataBinder.Eval(Container, "DataItem.Esponente"),DataBinder.Eval(Container, "DataItem.Scala")) %>'>Label</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Data Inizio">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label10" runat="server" text='<%# OPENgovTOCO.SharedFunction.FormattaData(DataBinder.Eval(Container, "DataItem.DataInizioOccupazione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Data Fine">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label11" runat="server" text='<%# OPENgovTOCO.SharedFunction.FormattaData(DataBinder.Eval(Container, "DataItem.DataFineOccupazione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Durata">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle Width="120px" HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label12" runat="server" text='<%# FncForGrd.FormattaDurCons(DataBinder.Eval(Container, "DataItem.DurataOccupazione"),DataBinder.Eval(Container, "DataItem.TipoDurata.Descrizione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Tipo Occup.">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle Width="300px" HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label13" runat="server" text='<%# (DataBinder.Eval(Container, "DataItem.TipologiaOccupazione.Descrizione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Cat.">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle Width="300px" HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label14" runat="server" text='<%# (DataBinder.Eval(Container, "DataItem.Categoria.Descrizione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Cons.">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemStyle HorizontalAlign="Right"></ItemStyle>
						<ItemTemplate>
							<asp:Label id="Label15" runat="server" text='<%# FncForGrd.FormattaDurCons(DataBinder.Eval(Container, "DataItem.Consistenza"), DataBinder.Eval(Container, "DataItem.TipoConsistenzaTOCO.Descrizione"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Tariffa">
						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
						<ItemTemplate>
							<asp:Label id="Label16" runat="server" text='<%# FncForGrd.FormattaCalcolo("T",DataBinder.Eval(Container, "DataItem.Calcolo"))%>'>
							</asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField visible="false" datafield="calcola_interessi" readonly="True" headertext="Calcola_Interessi"></asp:BoundField>
					<asp:BoundField visible="false" datafield="Sanzioni" readonly="True" headertext="Sanzioni"></asp:BoundField>
					<asp:BoundField visible="false" datafield="DescrSanzioni" readonly="True" headertext="DescrSanzioni"></asp:BoundField>
					<asp:TemplateField>
                        <itemtemplate>
                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneAssociaGrd" CausesValidation="False" CommandName="RowBind" CommandArgument='<%# Eval("Progressivo") %>' alt=""/>
                        </itemtemplate>
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Center"></itemstyle>
                    </asp:TemplateField>
				</Columns>
			</Grd:RibesGridView>
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
			<asp:Button id="btnAssocia" runat="server" Text="Associa Immobili" style="DISPLAY: none" />
			<asp:Button id="btnEsci" runat="server" Text="Esci" style="DISPLAY: none" />
			<asp:TextBox ID="TxtTributo" Runat="server" style="DISPLAY:none"></asp:TextBox>
		</form>
	</body>
</HTML>
