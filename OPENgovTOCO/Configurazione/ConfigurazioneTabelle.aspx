<%@ Page language="c#" Codebehind="ConfigurazioneTabelle.aspx.cs" AutoEventWireup="True" Inherits="OPENGovTOCO.Configurazione.ConfigurazioneTabelle"   EnableEventValidation="false"%>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ConfigurazioneTabelle</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
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
		<script>
			function confermaEliminazione()
			{
				if (confirm('Confermi l\'eliminazione?') == true)
				{
					//alert('si');	
				    document.getElementById('btDel').click();
				}
				else
				{
					//alert('no');
					return false;
				}
			}
		</script>
	</HEAD>
	<body class="Sfondo">
		<form id="Form1" runat="server" method="post">
            <div class="SfondoGenerale" style="width: 100%; height: 45px; overflow: hidden">
			    <table cellspacing="0" cellpadding="0" width="100%" border="0" id="Table1">
				    <tr>
					    <td><span style="WIDTH: 400px" id="infoEnte" class="ContentHead_Title"><asp:label id="lblTitolo" runat="server"></asp:label></span></td>
					    <td align="right" width="800" colspan="2" rowspan="2">
						    <asp:Button runat="server" ID="btNew" Cssclass="Bottone BottoneNewInsert" onclick="btNew_Click"></asp:Button>
						    <asp:Button runat="server" ID="btSalva" Cssclass="Bottone BottoneSalva" onclick="btSalva_Click"></asp:Button>
						    <asp:Button runat="server" ID="btDel" Cssclass="Bottone BottoneCancella" onclick="btDel_Click"></asp:Button>
						    <a title="Annulla" href="ConfigurazioneTabelle.aspx" class="Bottone BottoneAnnulla"></a>
					    </td>
				    </tr>
				    <tr>
					    <td colSpan="2"><span style="WIDTH: 400px; HEIGHT: 20px" id="info" class="NormalBold_title">TOSAP/COSAP - Configurazione - Tabelle</span></td>
				    </tr>
			    </table>
            </div>
			<div class="col-md-12">
				<FIELDSET class="FiledSetRicerca" style="WIDTH: 98%; HEIGHT: 49px"><LEGEND class="Legend">Inserimento filtri di ricerca</LEGEND>
					<table width="100%">
						<tr>
							<td style="WIDTH: 432px; HEIGHT: 3px">
								<asp:radiobutton id="RBCategoria" tabIndex="1" runat="server" Text="Categoria" TextAlign="Right"
									Checked="True" GroupName="RB" AutoPostBack="True" CssClass="Input_Label" oncheckedchanged="RBCategoria_CheckedChanged"></asp:radiobutton>&nbsp;&nbsp;
								<asp:radiobutton id="RBAgevolazione" tabIndex="1" runat="server" Text="Agevolazione" TextAlign="Right"
									GroupName="RB" AutoPostBack="True" CssClass="Input_Label" oncheckedchanged="RBAgevolazione_CheckedChanged"></asp:radiobutton>&nbsp;&nbsp;
								<asp:radiobutton id="RBTipologiaOccupazioni" tabIndex="2" runat="server" Text="Tipologia Occupazione"
									TextAlign="Right" GroupName="RB" AutoPostBack="True" CssClass="Input_Label" oncheckedchanged="RBTipologiaOccupazioni_CheckedChanged"></asp:radiobutton>
							</td>
						</tr>
					</table>
				</FIELDSET>
				
				<asp:Label id="lblResultList" Runat="server" Visible="False" style="font-size:10px;font-family:verdana;"></asp:Label>
				<asp:Panel ID="pnlCategorie" Runat="server">
					<Grd:RibesGridView ID="GrdCategorie" runat="server" BorderStyle="None" 
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
							<asp:BoundField DataField="Descrizione" HeaderText="Descrizione"></asp:BoundField>
                            <asp:TemplateField HeaderText="">
	                            <headerstyle horizontalalign="Center"></headerstyle>
	                            <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
	                            <itemtemplate>
	                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IdCategoria") %>' alt=""></asp:ImageButton>
	                            <asp:HiddenField runat="server" ID="hfIdCategoria" Value='<%# Eval("IdCategoria") %>' />
	                            </itemtemplate>
                            </asp:TemplateField>
						</columns>
					</Grd:RibesGridView>
				</asp:Panel>
				<asp:Panel ID="pnlAgevolazioni" Runat="server">
					<Grd:RibesGridView ID="GrdAgevolazioni" runat="server" BorderStyle="None" 
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
							<asp:BoundField DataField="Descrizione" HeaderText="Descrizione"></asp:BoundField>
							<asp:TemplateField HeaderText="">
	                            <headerstyle horizontalalign="Center"></headerstyle>
	                            <itemstyle horizontalalign="Center" Width="40px"></itemstyle>
	                            <itemtemplate>
	                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IdAgevolazione") %>' alt=""></asp:ImageButton>
	                            <asp:HiddenField runat="server" ID="hfIdAgevolazione" Value='<%# Eval("IdAgevolazione") %>' />
	                            </itemtemplate>
                            </asp:TemplateField>
						</Columns>
						</Grd:RibesGridView>
				</asp:Panel>
				<asp:Panel ID="pnlTipoOccupazioni" Runat="server">
					<Grd:RibesGridView ID="GrdTipOccupazioni" runat="server" BorderStyle="None" 
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
							<asp:BoundField DataField="Descrizione" HeaderText="Descrizione"></asp:BoundField>
							<asp:TemplateField HeaderText="">
	                            <headerstyle horizontalalign="Center"></headerstyle>
	                            <itemstyle horizontalalign="Center"></itemstyle>
	                            <itemtemplate>
	                            <asp:ImageButton runat="server" Cssclass="BottoneGrd BottoneApriGrd" CommandName="RowOpen" CommandArgument='<%# Eval("IdTipologiaOccupazione") %>' alt=""></asp:ImageButton>
	                            <asp:HiddenField runat="server" ID="hfIdTipologiaOccupazione" Value='<%# Eval("IdTipologiaOccupazione") %>' />
	                            </itemtemplate>
                            </asp:TemplateField>
						</Columns>
						</Grd:RibesGridView>
				</asp:Panel>
				<asp:Panel Runat="server" ID="pnlModifica" Visible="False">
					<TABLE border="0" cellPadding="0" width="100%">
						<TR>
							<TD style="HEIGHT: 16px">
								<asp:label id="lblDescrizioneOperazione" runat="server" CssClass="Legend" Width="100%">Dati Codice/Descrizione - </asp:label>&nbsp;
							</TD>
						</TR>
					</TABLE>
					<DIV class="Input_Label">Tabella selezionata:
						<asp:Label id="lblTabSel" Runat="server"></asp:Label></DIV>
					<DIV class="Input_Label">Descrizione</DIV>
					<asp:TextBox id="txbDescrizione" Runat="server" TextMode="MultiLine" Rows="5" Columns="60"></asp:TextBox>
					<asp:TextBox id="txbIdRecordToMod" Runat="server" Visible="False"></asp:TextBox>
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
