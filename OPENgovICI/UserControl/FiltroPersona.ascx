<%@ Control Language="c#" AutoEventWireup="false" Codebehind="FiltroPersona.ascx.cs" Inherits="DichiarazioniICI.UserControl.FiltroPersona" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
<meta content="False" name="vs_showGrid">
<table id="tblCorpo" cellSpacing="1" cellPadding="1" width="100%" align="center" border="0">
	<tr>
		<td class="Legend" align="left" style="HEIGHT: 16px">Ricerca per Persona </td>
	</tr>
	<tr>
		<td>
			<table id="tblFiltri" cellspacing="1" cellpadding="5" border="0">
				<tr>
					<td class="Input_Label" align="left">Cognome<br />
						<asp:textbox id="txtCognome" Width="376px" CssClass="Input_Text" runat="server" onkeydown="if(event.keycode == 13){alert('ok')};"></asp:textbox>
					</td>
					<td class="Input_Label" align="left">Nome<br />
						<asp:textbox id="txtNome" Width="185px" CssClass="Input_Text" runat="server"  onkeydown="if(event.keycode == 13){};"></asp:textbox>
					</td>
					<td class="Input_Label" align="left">Codice Fiscale<br />
						<asp:textbox id="txtCodiceFiscale" Width="185px" CssClass="Input_Text" runat="server" MaxLength="16" onkeydown="if(event.keycode == 13){};"></asp:textbox>
					</td>
					<td class="Input_Label" align="left">Partita IVA<br />
						<asp:textbox id="txtPartitaIva" Width="130px" CssClass="Input_Text" runat="server" MaxLength="70" onkeydown="if(event.keycode == 13){};"></asp:textbox>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<asp:button id="btnTrova" style="DISPLAY: none" runat="server" Text="Trova"></asp:button>
