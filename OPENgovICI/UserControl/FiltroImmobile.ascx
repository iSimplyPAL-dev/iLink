<%@ Control Language="c#" AutoEventWireup="false" Codebehind="FiltroImmobile.ascx.cs" Inherits="DichiarazioniICI.UserControl.FiltroImmobile" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
<table id="tblCorpo" cellSpacing="1" cellPadding="1" width="100%" align="center" border="0">
	<tr>
		<td class="Legend" align="left" style="HEIGHT: 16px">Ricerca per Immobile</td>
	</tr>
	<tr>
		<td>
			<table id="tblFiltri" borderColor="#dcdcdc" cellSpacing="1" cellPadding="5" width="100%" border="0">
			</table>
			<table id="tblFiltri1" borderColor="#dcdcdc" cellSpacing="1" cellPadding="5" width="100%" border="0">
				<tr>
					<td class="Input_Label" colspan="4">Via<br />
						<asp:textbox id="txtVia" runat="server" Width="376px" CssClass="Input_Text"></asp:textbox></td>
					<td class="Input_Label">Foglio<br />
						<asp:textbox id="txtFoglio" runat="server" Width="50px" MaxLength="5" CssClass="Input_Text"></asp:textbox>
					</td>
					<td class="Input_Label">Numero<br />
						<asp:textbox id="txtNumero" runat="server" Width="50px" MaxLength="5" CssClass="Input_Text"></asp:textbox></td>
					<td class="Input_Label">Subalterno<br />
						<asp:textbox id="txtSubalterno" runat="server" Width="50px" MaxLength="4" CssClass="Input_Text"></asp:textbox></td>
					<td colspan="2" valign="bottom"><asp:CheckBox ID="ckbImmNoAgganciati" runat="server" Text="Immobili non agganciati a stradario" CssClass="Input_Label"></asp:CheckBox></td>
				</tr>
				<tr>
					<td class="Input_Label">Caratteristica<br />
					    <asp:dropdownlist id="ddlCaratteristica" runat="server" CssClass="Input_Text"></asp:dropdownlist>
					</td>
					<td colspan="2" class="Input_Label">Categoria<br />
						<asp:dropdownlist id="ddlCategoriaCatastale" runat="server" CssClass="Input_Text"></asp:dropdownlist></td>
					<td class="Input_Label">Classe<br />
						<asp:dropdownlist id="ddlClasse" runat="server" CssClass="Input_Text" DataTextField="Classe" DataValueField="Classe"></asp:dropdownlist></td>
					<td colspan="2" valign="bottom">
					    <asp:CheckBox ID="chkAbiPrinc" runat="server" CssClass="Input_Label" Text="Abitazione Principale" /><br />
                        <asp:CheckBox ID="chkPertinenza" runat="server" CssClass="Input_Label" Text="Pertinenza" />
					</td>
					<td colspan="2" class="Input_Label">Tipo Utilizzo<br />
					    <asp:dropdownlist id="ddlTipoUtilizzo" runat="server" CssClass="Input_Text"></asp:dropdownlist>
					</td>
					<td class="Input_Label">Tipo Possesso<br />
					    <asp:dropdownlist id="ddlTipoPossesso" runat="server" CssClass="Input_Text"></asp:dropdownlist>
					</td>
					<td>
						<label class="Input_Label" for="txtPercentualePos">% Possesso</label><br />
						<asp:TextBox id="txtPercentualePos" Width="70px" CssClass="Input_Text_Right OnlyNumber" Runat="server" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td colspan="2">
					    <label class="Input_Label" for="txtValore">Valore Immobile</label><br />
						<asp:DropDownList id="ddlTipoValore" CssClass="Input_Text" Runat="server">
							<asp:ListItem Value="-1">[Nessun Importo]</asp:ListItem>
							<asp:ListItem Value="0">Maggiore di</asp:ListItem>
							<asp:ListItem Value="1">Minore di</asp:ListItem>
							<asp:ListItem Value="2">Uguale a</asp:ListItem>
						</asp:DropDownList>
						<asp:textbox id="txtValore" Width="100px" CssClass="Input_Text_Right OnlyNumber" Runat="server" onkeypress="return NumbersOnly(event, true, false, 2);"></asp:textbox>
					</td>
				    <td colspan="4" valign="bottom">
				        <asp:CheckBox id="chkColtivatori" runat="server" CssClass="Input_Label" Text="Coltivatore diretto"></asp:CheckBox>
				    </td>
					<td class="Input_Label">Partita Catastale<br />
						<asp:textbox id="txtPartitaCatastale" runat="server" Width="96px" MaxLength="3" CssClass="Input_Text"></asp:textbox></td>
					<td class="Input_Label">Sezione<br />
						<asp:textbox id="txtSezione" runat="server" Width="64px" MaxLength="3" CssClass="Input_Text"></asp:textbox></td>
					<td class="Input_Label">Protocollo<br />
						<asp:textbox id="txtProtocollo" runat="server" Width="64px" MaxLength="6" CssClass="Input_Text"></asp:textbox></td>
					<td class="Input_Label">Anno<br />
						<asp:textbox id="txtAnno" runat="server" Width="64px" MaxLength="5" CssClass="Input_Text"></asp:textbox></td>
				</tr>
			</table>
		    <asp:button id="btnTrova" style="DISPLAY: none" runat="server" Text="Trova"></asp:button>
		</td>
	</tr>
</table>
