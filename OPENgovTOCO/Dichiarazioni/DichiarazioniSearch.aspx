<%@ Page language="c#" Codebehind="DichiarazioniSearch.aspx.cs" AutoEventWireup="True" Inherits="OPENgovTOCO.Dichiarazioni.DichiarazioniSearch" EnableEventValidation="false" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Ricerca Dichiarazioni</title>
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
	<body style="OVERFLOW: auto" bottomMargin="0" leftMargin="0" rightMargin="0" topMargin="0" MS_POSITIONING="GridLayout" class="Sfondo">
		<form id="Form1" runat="server" method="post">
			<div class="SfondoGenerale" style="OVERFLOW: auto; WIDTH: 100%; HEIGHT: 45px">
				<table style="WIDTH: 100%" border="0" cellpadding="0" cellspacing="0">
					<tr valign="top">
						<td><span style="WIDTH: 400px" id="infoEnte" class="ContentHead_Title"><asp:label id="lblTitolo" runat="server"></asp:label></span></td>
						<td align="right" valign="middle" rowspan="2">
							<asp:imagebutton id="Stampa" runat="server" Cssclass="Bottone BottoneExcel" ToolTip="Stampa Elenco Dichiarazione" ImageUrl="../../images/Bottoni/transparent28x28.png" onclick="Stampa_Click"></asp:imagebutton>
							<asp:imagebutton id="NuovaDichiarazione" runat="server" Cssclass="Bottone BottoneNewInsert" ToolTip="Nuova Dichiarazione" ImageUrl="../../images/Bottoni/transparent28x28.png" onclick="NuovaDichiarazione_Click"></asp:imagebutton>
							<asp:imagebutton id="Search" runat="server" ToolTip="Ricerca Dichiarazione" Cssclass="Bottone BottoneRicerca" ImageUrl="../../images/Bottoni/transparent28x28.png" onclick="Search_Click"></asp:imagebutton>
						</td>
					</tr>
					<tr>
						<td colSpan="2"><span style="WIDTH: 400px; HEIGHT: 20px" id="info" class="NormalBold_title">TOSAP/COSAP - Dichiarazioni - Ricerca</span>
						</td>
					</tr>
				</table>
			</div>
			<div>
				<table id="TblRicerca" border="0" cellSpacing="1" cellPadding="1" width="98%">
					<tr>
						<td style="width:100%">
							<fieldset class="FiledSetRicerca">
							    <legend class="Legend">Inserimento filtri di ricerca</legend>
								<table id="TblParametri" border="0" cellSpacing="1" cellPadding="1">
									<tr>
										<td>
										    <asp:panel id="PanelSoggetto" Runat="server">
											    <table>
												    <tr>
													    <td valign="top">
														    <asp:Label id="Label2" CssClass="Input_Label" Runat="server">Cognome</asp:Label><br />
														    <asp:textbox id="TxtCognome" runat="server" CssClass="Input_Text" Width="376px"></asp:textbox></td>
													    <td valign="top">
														    <asp:Label id="Label3" CssClass="Input_Label" Runat="server">Nome</asp:Label><br />
														    <asp:textbox id="TxtNome" runat="server" CssClass="Input_Text" Width="185px"></asp:textbox></td>
													    <td valign="top">
														    <asp:Label id="Label4" CssClass="Input_Label" Runat="server">Codice Fiscale</asp:Label><br />
														    <asp:textbox id="TxtCodFiscale" runat="server" CssClass="Input_Text" Width="160px" MaxLength="16"></asp:textbox></td>
													    <td valign="top">
														    <asp:Label id="Label5" CssClass="Input_Label" Runat="server">Partita IVA</asp:Label><br />
														    <asp:textbox id="TxtPIva" runat="server" CssClass="Input_Text" Width="160px" MaxLength="11"></asp:textbox></td>
													    <td>
													        <asp:Label id="Label9" CssClass="Input_Label" Runat="server">N.Autoriz./Conces.</asp:Label><br />
														    <asp:textbox id="TxtNDich" runat="server" CssClass="Input_Text" Width="100px"></asp:textbox>
													    </td>
												    </tr>
												</table>
											</asp:panel>
                                        </td>
									</tr>
								</table>
							</fieldset>
						</td>
					</tr>
				</table>
				<table width="100%">
					<tr>
						<td>
							<asp:label id="LblResultDichiarazioni" CssClass="Legend" Runat="server">Non sono presenti dichiarazioni</asp:label>
							<Grd:RibesGridView ID="GrdDichiarazioni" runat="server" BorderStyle="None" 
                                  BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                  AutoGenerateColumns="False" AllowPaging="true" AllowSorting="false" PageSize="10"
                                  ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                  OnRowCommand="GrdRowCommand" OnPageIndexChanging="GrdPageIndexChanging">
                                  <PagerSettings Position="Bottom"></PagerSettings>
                                  <PagerStyle CssClass="CartListFooter" />
                                  <RowStyle CssClass="CartListItem"></RowStyle>
                                  <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                  <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
								<COLUMNS>
									<asp:BoundField DataField="Cognome" HeaderText="Cognome">
										<ItemStyle Width="200px"></ItemStyle>
									</asp:BoundField>
									<asp:BoundField DataField="Nome" HeaderText="Nome">
										<ItemStyle Width="150px"></ItemStyle>
									</asp:BoundField>
									<asp:BoundField DataField="CfPiva" HeaderText="Cod.Fiscale/P.IVA"></asp:BoundField>
									<asp:TemplateField Visible="True" HeaderText="Data Dichiarazione">
										<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left" Width="120px"></ItemStyle>
										<ItemTemplate>
											<asp:Label id="Label10" runat="server" Text='<%# OPENgovTOCO.SharedFunction.FormattaData (DataBinder.Eval(Container, "DataItem.DataDichiarazione")) %>'>
											</asp:Label>
										</ItemTemplate>
									</asp:TemplateField>									
									<asp:BoundField DataField="NDichiarazione" HeaderText="N.Dichiarazione"></asp:BoundField>
									<asp:TemplateField HeaderText="">
	                                    <headerstyle horizontalalign="Center"></headerstyle>
	                                    <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
	                                    <itemtemplate>
	                                    <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IdDichiarazione") %>' alt=""></asp:ImageButton>
	                                    <asp:HiddenField runat="server" ID="hfIdDichiarazione" Value='<%# Eval("IdDichiarazione") %>' />
	                                    </itemtemplate>
                                    </asp:TemplateField>
								</COLUMNS>
								</Grd:RibesGridView>
						</td>
					</tr>
				</table>
			</div>
		</form>
	</body>
</HTML>
