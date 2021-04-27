<%@ Page Language="c#" CodeBehind="GestionePagamentiSearch.aspx.cs" AutoEventWireup="True" Inherits="OPENGovTOCO.GestionePagamenti.GestionePagamentiSearch" EnableEventValidation="false" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>GestionePagamentiSearch</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
		<script type="text/javascript" src="../../_js/VerificaCampi.js?newversion"></script>
		<script type="text/javascript" src="../../_js/Toco.js?newversion"></script>
	</HEAD>
	<body class="Sfondo" ms_positioning="GridLayout" bottommargin="0" leftmargin="2"
		topmargin="6" rightmargin="2" marginheight="0" marginwidth="0" style="HEIGHT: 100%">
		<form id="Form1" runat="server" method="post">
			<table class="SfondoGenerale" cellspacing="0" cellpadding="0" width="100%" border="0" id="Table1">
				<tr>
					<td style="WIDTH: 464px; HEIGHT: 20px" align="left">
						<span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label>
						</span>
					</td>
					<td align="right" width="800" colspan="2" rowspan="2">
						<div style="float:right;">
							<asp:Button runat="server" ID="btSearch" OnClick="btSearch_OnClick" Cssclass="Bottone Bottonericerca"></asp:Button>
						</div>
						<div>
							<asp:Button runat="server" ID="btStampaNonP" CommandName="_ExpNonPagati" OnClick="Export_OnClick" Cssclass="Bottone BottoneNoMoney" title="Elenco soggetti che non hanno pagato" ></asp:Button>
							<asp:Button runat="server" ID="btStampaPmag" CommandName="_ExpImportoMaggiore" OnClick="Export_OnClick" Cssclass="Bottone BottoneMoreMoney" title="Elenco soggetti con importo pagato maggiore di importo emesso"></asp:Button>
							<asp:Button runat="server" ID="btStampaPMin" CommandName="_ExpImportoMinore" OnClick="Export_OnClick" Cssclass="Bottone BottoneLessMoney" title="Elenco soggetti con importo pagato minore di importo emesso" ></asp:Button>
							<asp:Button runat="server" ID="btStampaPag" CommandName="_ExpAll" OnClick="Export_OnClick" Cssclass="Bottone BottoneMoney" title="Elenco pagamenti"></asp:Button>								
							<a class=BottoneNewInsert style="MARGIN-RIGHT: 0px; display:inline-block" href="GestionePagamentiAdd.aspx"></a>					
						</div>
					</td>
				</tr>
				<tr>
					<td style="WIDTH: 463px">
						<span id="info" runat="server" class="NormalBold_title" style="WIDTH: 400px; HEIGHT: 20px">Tosap/Cosap - Pagamenti - Gestione Pagamenti</span>
					</td>
				</tr>
			</table>
			<div>
				<table id="TblRicerca" cellspacing="1" cellpadding="1" width="100%" border="0">
					<tr>
						<td>
							<fieldset class="FiledSetRicerca">
								<legend class="Legend">Inserimento parametri di ricerca</legend>
								<table>
									<tr>
										<td colspan="2">
											<asp:Label ID="lblAnno" CssClass="Input_Label" runat="server">Anno riferimento</asp:Label><br>
											<asp:TextBox ID="txtAnnoRif" CssClass="Input_Text_Right OnlyNumber" runat="server" Width="80px" MaxLength="4"></asp:TextBox>
										</td>
									</tr>
									<tr>
										<td>
											<asp:Label ID="lblCognome" CssClass="Input_Label" runat="server">Cognome</asp:Label><br>
											<asp:TextBox ID="txtCognome" CssClass="Input_Text" runat="server" Width="376px"></asp:TextBox>
										</td>
										<td>
											<asp:Label ID="lblNome" CssClass="Input_Label" runat="server">Nome</asp:Label><br>
											<asp:TextBox ID="txtNome" CssClass="Input_Text" runat="server" Width="185px"></asp:TextBox>
										</td>
										<td>
											<asp:Label ID="lblCodiceFiscale" CssClass="Input_Label" runat="server">Codice Fiscale</asp:Label><br>
											<asp:TextBox ID="txtCodiceFiscale" CssClass="Input_Text" runat="server" Width="160px"></asp:TextBox>
										</td>
										<td>
											<asp:Label ID="lblPartitaIva" CssClass="Input_Label" runat="server">Partita IVA</asp:Label><br>
											<asp:TextBox ID="txtPartitaIva" CssClass="Input_Text" runat="server" Width="160px"></asp:TextBox>
										</td>
									</tr>
								</table>
							</fieldset>
						</td>
					</tr>
					<tr>
						<td>
							<fieldset class="FiledSetRicerca">
								<legend class="Legend">
									Inserimento parametri di ricerca avanzata</legend>
								<table>
									<tr>
										<td style="WIDTH: 200px">
											<asp:Label ID="lblNAvviso" CssClass="Input_Label" runat="server">Numero Avviso</asp:Label><br>
											<asp:TextBox ID="txtNAvviso" CssClass="Input_Text" runat="server" Width="185px"></asp:TextBox>
										</td>
										<td>
											<asp:Label CssClass="Input_Label" runat="server" ID="lblTesto">Data di Riversamento</asp:Label><br>
											<asp:Label ID="lblDataAccrDal" CssClass="Input_Label" runat="server">Dal</asp:Label>&nbsp;
                                            <asp:TextBox runat="server" ID="txtDataAccreditoDal" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
											<asp:Label ID="lblDataAccreditoAl" CssClass="Input_Label" runat="server">Al</asp:Label>&nbsp;
                                            <asp:TextBox runat="server" ID="txtDataAccreditoAl" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Input_Text_Right TextDate"></asp:TextBox>
										</td>
									</tr>
								</table>
							</fieldset>
						</td>
					</tr>
				</table>
				<!--
				<asp:Button ID="btnStampaPag" Style="DISPLAY: none" runat="server"></asp:Button>
				<asp:Button ID="btnStampaNonPag" Style="DISPLAY: none" runat="server"></asp:Button>
				<asp:Button ID="btnStampaPMag" Style="DISPLAY: none" runat="server"></asp:Button>
				<asp:Button ID="btnStampaPMin" Style="DISPLAY: none" runat="server"></asp:Button>
				-->
				<table width="100%">
					<tr>
						<td width="100%">
							<asp:Label ID="LblResultPagamenti" runat="server" CssClass="Legend">Risultati della Ricerca</asp:Label>
							<Grd:RibesGridView ID="GrdPagamenti" runat="server" BorderStyle="None" 
                                  BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                  AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                                  ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                  OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                                  <PagerSettings Position="Bottom"></PagerSettings>
                                  <PagerStyle CssClass="CartListFooter" />
                                  <RowStyle CssClass="CartListItem"></RowStyle>
                                  <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                  <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
								<columns>
									<asp:BoundField DataField="Anno" SortExpression="Anno" HeaderText="Anno"></asp:BoundField>
									<asp:BoundField DataField="CodiceCartella" SortExpression="CodiceCartella" HeaderText="Avviso"></asp:BoundField>
									<asp:BoundField DataField="Nominativo" SortExpression="Nominativo" HeaderText="Nominativo"></asp:BoundField>
									<asp:BoundField DataField="CFPIVA" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
									<asp:BoundField DataField="NumeroRataString" SortExpression="NumeroRataString" HeaderText="N. Rata">
									    <ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundField>
									<asp:TemplateField HeaderText="Data Pagamento">
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
										<ItemTemplate>
											<asp:Label runat="server" Text='<%# FormattaDataGrd((DateTime)DataBinder.Eval(Container, "DataItem.DataPagamento")) %>' ID="Label2" NAME="Label1">
											</asp:Label>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Data Riversamento">
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
										<ItemTemplate>
											<asp:Label runat="server" Text='<%# FormattaDataGrd((DateTime)DataBinder.Eval(Container, "DataItem.DataAccredito")) %>' ID="Label1" NAME="Label1">
											</asp:Label>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:BoundField DataField="ImportoPagato" HeaderText="Imp. Pagato &amp;#8364" DataFormatString="{0:0.00}">
										<ItemStyle HorizontalAlign="Right"></ItemStyle>
									</asp:BoundField>
									<asp:TemplateField HeaderText="">
	                                    <headerstyle horizontalalign="Center"></headerstyle>
	                                    <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
	                                    <itemtemplate>
	                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IdPagamento") %>' alt=""></asp:ImageButton>
	                                    <asp:HiddenField runat="server" ID="hfCodContribuente" Value='<%# Eval("CodContribuente") %>' />
                                        <asp:HiddenField runat="server" ID="hfIdPagamento" Value='<%# Eval("IdPagamento") %>' />
	                                    </itemtemplate>
                                    </asp:TemplateField>
								</columns>
								</Grd:RibesGridView>
						</td>
					</tr>
				</table>
				<div style="HEIGHT:5px"></div>
				<asp:Panel Runat="server" ID="pnlTotali" Visible="False" align="right">
					<TABLE class="tableTotali" border="0">
						<THEAD>
							<TR>
								<TD colSpan="2">Totali</TD>
							</TR>
						</THEAD>
						<TR>
							<TD>Totale pagamenti</TD>
							<TD align="right">
								<asp:Label id="lblTot" Runat="server"></asp:Label></TD>
						</TR>
						<TR>
							<TD width="200">Totale importo pagato €</TD>
							<TD width="80" align="right">
								<asp:Label id="lblTotImp" Runat="server"></asp:Label></TD>
						</TR>
					</TABLE>
				</asp:Panel>
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
		</form>
	</body>
</HTML>
