<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ResultAnalisiEconomiche.aspx.vb" Inherits="OpenUtenze.ResultAnalisiEconomiche" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ResultAnalisiEconomiche</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<% If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<% end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function LoadGrafico()
			{
				parent.LoadResult.location.href='ChartAnalisiEconomiche.aspx?ImpIncassato='+document.SituazioneAnalisiEconomiche.TxtIncassato.value+'&ImpInsoluto='+document.SituazioneAnalisiEconomiche.TxtInsoluto.value
			}

			function LoadStampa()
			{
				document.SituazioneAnalisiEconomiche.CmdStampa.click()
			}
		</script>
	</HEAD>
	<body class="Sfondo" leftMargin="3" topMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="760" border="0">
				<tr>
					<td>
						<!--Riepilogo Fatturato/Incassato-->
						<fieldset class="FiledSetRicerca" style="WIDTH: 100%">
							<table height="50" width="100%">
								<!--Intestazione-->
								<tr>
									<td colSpan="5"><asp:label id="Label1" CssClass="Legend" Runat="server">Dati Fatturato/Incassato</asp:label></td>
								</tr>
								<!--Emesso-->
								<tr>
									<td><asp:label id="Label52" CssClass="Input_Label" Runat="server">N. Fatture Emesse</asp:label></td>
									<td align="right"><asp:label id="LblNFatture" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td width="20"></td>
									<td align="right"><asp:label id="Label55" CssClass="Input_Label" Runat="server">Totale Fatture €</asp:label></td>
									<td align="right"><asp:label id="LblTotImpFatture" CssClass="Input_Label" Runat="server"></asp:label></td>
								</tr>
								<tr>
									<td><asp:label id="Label4" CssClass="Input_Label" Runat="server">N. Note di Credito Emesse</asp:label></td>
									<td align="right"><asp:label id="LblNNoteCredito" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td width="20"></td>
									<td align="right"><asp:label id="Label5" CssClass="Input_Label" Runat="server">Totale Note di Credito €</asp:label></td>
									<td align="right"><asp:label id="LblTotImpNoteCredito" CssClass="Input_Label" Runat="server"></asp:label></td>
								</tr>
								<tr>
									<td><asp:label id="Label2" CssClass="Input_Label" Runat="server">N. Note di Credito da Emettere</asp:label></td>
									<td align="right"><asp:label id="LblNNoteDaEmettere" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td width="20"></td>
									<td align="right"><asp:label id="Label32" CssClass="Input_Label" Runat="server">Totale Note di Credito da Emettere€</asp:label></td>
									<td style="BORDER-BOTTOM: #00008b 1px solid" align="right"><asp:label id="LblTotImpNoteDaEmettere" CssClass="Input_Label" Runat="server"></asp:label></td>
								</tr>
								<tr>
									<td align="right" colSpan="4"><asp:label id="Label53" CssClass="Legend" Runat="server">Totale al Netto di Note di Credito €</asp:label></td>
									<td align="right"><asp:label id="LblTotEmesso" CssClass="Legend" Runat="server"></asp:label></td>
								</tr>
								<!--Pagato-->
								<tr>
									<td><br>
									</td>
								</tr>
								<!--Pagati Totalmente-->
								<tr>
									<td><asp:label id="Label7" CssClass="Input_Label" Runat="server">N. Fatture Pagate Totalmente</asp:label></td>
									<td align="right"><asp:label id="LblNPagTot" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td width="20"></td>
									<td align="right"><asp:label id="Label9" CssClass="Input_Label" Runat="server">Totale Pagato €</asp:label></td>
									<td align="right"><asp:label id="LblImpPagTot" CssClass="Input_Label" Runat="server"></asp:label></td>
								</tr>
								<!--Pagati Parzialmente-->
								<tr>
									<td><asp:label id="Label11" CssClass="Input_Label" Runat="server">N. Fatture Pagate Parzialmente</asp:label></td>
									<td align="right"><asp:label id="LblNPagParz" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td width="20"></td>
									<td align="right"><asp:label id="Label13" CssClass="Input_Label" Runat="server">Totale Acconti €</asp:label></td>
									<td style="BORDER-BOTTOM: #00008b 1px solid" align="right"><asp:label id="LblImpPagParz" CssClass="Input_Label" Runat="server"></asp:label></td>
								</tr>
								<!--Totale Incassato-->
								<tr>
									<td align="right" colSpan="4"><asp:label id="Label15" CssClass="Legend" Runat="server">Totale Incassato €</asp:label></td>
									<td align="right"><asp:label id="LblTotIncassato" CssClass="Legend" Runat="server"></asp:label></td>
								</tr>
							</table>
							<!--Riepilogo Generale-->
							<table width="100%">
								<!--Intestazione-->
								<tr>
									<td colSpan="7"><asp:label id="Label8" style="TOP: 150px; LEFT: 3px" CssClass="Legend" Runat="server">Riepilogo Generale</asp:label></td>
								</tr>
								<tr>
									<td width="100"><asp:label id="Label10" CssClass="Input_Label" Runat="server">N.Utenti</asp:label></td>
									<td align="center" width="250"><asp:label id="Label12" CssClass="Input_Label" Runat="server">Emesso al Netto di Note di Credito €</asp:label></td>
									<td align="center" width="15"></td>
									<td align="center" width="100"><asp:label id="Label16" CssClass="Input_Label" Runat="server">Incassato €</asp:label></td>
									<td align="center" width="15"></td>
									<td align="center" width="100"><asp:label id="Label18" CssClass="Input_Label" Runat="server">Insoluto €</asp:label></td>
									<td align="center" width="120"><asp:label id="Label19" CssClass="Legend" Runat="server">% Insoluto</asp:label></td>
								</tr>
								<tr>
									<td align="center"><asp:label id="LblNUtenti" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td align="center"><asp:label id="LblEmesso" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td align="center"><asp:label id="Label17" CssClass="Input_Label" Runat="server">-</asp:label></td>
									<td align="center"><asp:label id="LblIncassato" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td align="center"><asp:label id="Label39" CssClass="Input_Label" Runat="server">=</asp:label></td>
									<td align="center"><asp:label id="LblInsoluto" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td align="center"><asp:label id="LblPercentualeInsoluto" CssClass="Legend" Runat="server"></asp:label></td>
									<td><asp:textbox id="TxtIncassato" CssClass="Legend" Runat="server" Width="0">0</asp:textbox><asp:textbox id="TxtInsoluto" CssClass="Legend" Runat="server" Width="0">0</asp:textbox></td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td><br>
						<!--Dettaglio Emesso/Incassato-->
						<fieldset class="FiledSetRicerca" style="WIDTH: 100%">
							<table width="100%">
								<tr>
									<td colSpan="3">
										<!--Intestazione--><asp:label id="Label20" CssClass="Legend" Runat="server">Dettaglio Fatturato</asp:label></td>
									<td width="10"></td>
									<td colSpan="3">
										<!--Intestazione--><asp:label id="Label27" CssClass="Legend" Runat="server">Dettaglio Incassato</asp:label></td>
								</tr>
								<tr>
									<!--Consumo Acqua Fatturato-->
									<td><asp:label id="Label22" CssClass="Input_Label" Runat="server">Consumo Acqua €</asp:label></td>
									<td align="right" width="20%"><asp:label id="LblFatturatoConsumo" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label14" CssClass="Input_Label" Runat="server">+</asp:label></td>
									<td></td>
									<!--Consumo Acqua Incassato-->
									<td><asp:label id="Label29" CssClass="Input_Label" Runat="server">Consumo Acqua €</asp:label></td>
									<td align="right" width="20%"><asp:label id="LblIncassatoConsumo" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label44" CssClass="Input_Label" Runat="server">+</asp:label></td>
								</tr>
								<tr>
									<!--Fognatura Fatturato-->
									<td><asp:label id="Label23" CssClass="Input_Label" Runat="server">Fognatura €</asp:label></td>
									<td align="right"><asp:label id="LblFatturatoFognatura" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label21" CssClass="Input_Label" Runat="server">+</asp:label></td>
									<td></td>
									<!--Fognatura Incassato-->
									<td><asp:label id="Label30" CssClass="Input_Label" Runat="server">Fognatura €</asp:label></td>
									<td align="right"><asp:label id="LblIncassatoFognatura" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label45" CssClass="Input_Label" Runat="server">+</asp:label></td>
								</tr>
								<tr>
									<!--Depurazione Fatturato-->
									<td><asp:label id="Label24" CssClass="Input_Label" Runat="server">Depurazione €</asp:label></td>
									<td align="right"><asp:label id="LblFatturatoDepurazione" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label25" CssClass="Input_Label" Runat="server">+</asp:label></td>
									<td></td>
									<!--Depurazione Incassato-->
									<td><asp:label id="Label28" CssClass="Input_Label" Runat="server">Depurazione €</asp:label></td>
									<td align="right"><asp:label id="LblIncassatoDepurazione" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label43" CssClass="Input_Label" Runat="server">+</asp:label></td>
								</tr>
								<tr>
									<!--Nolo Fatturato-->
									<td><asp:label id="Label6" CssClass="Input_Label" Runat="server">Nolo €</asp:label></td>
									<td align="right"><asp:label id="LblFatturatoNolo" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label41" CssClass="Input_Label" Runat="server">+</asp:label></td>
									<td></td>
									<!--Nolo Incassato-->
									<td><asp:label id="Label42" CssClass="Input_Label" Runat="server">Nolo €</asp:label></td>
									<td align="right"><asp:label id="LblIncassatoNolo" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label49" CssClass="Input_Label" Runat="server">+</asp:label></td>
								</tr>
								<tr>
									<!--Quota Fissa Fatturato-->
									<td><asp:label id="Label51" CssClass="Input_Label" Runat="server">Quota Fissa Acqua €</asp:label></td>
									<td align="right"><asp:label id="LblFatturatoQuotaFissa" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label54" CssClass="Input_Label" Runat="server">+</asp:label></td>
									<td></td>
									<!--Quota Fissa Incassato-->
									<td><asp:label id="Label56" CssClass="Input_Label" Runat="server">Quota Fissa Acqua €</asp:label></td>
									<td align="right"><asp:label id="LblIncassatoQuotaFissa" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label58" CssClass="Input_Label" Runat="server">+</asp:label></td>
								</tr>
								<tr>
									<!--Fognatura Quota Fissa Fatturato-->
									<td><asp:label id="Label3" CssClass="Input_Label" Runat="server">Quota Fissa Fognatura €</asp:label></td>
									<td align="right"><asp:label id="LblFatturatoFognaturaQF" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label38" CssClass="Input_Label" Runat="server">+</asp:label></td>
									<td></td>
									<!--Fognatura Quota Fissa  Incassato-->
									<td><asp:label id="Label48" CssClass="Input_Label" Runat="server">Quota Fissa Fognatura €</asp:label></td>
									<td align="right"><asp:label id="LblIncassatoFognaturaQF" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label59" CssClass="Input_Label" Runat="server">+</asp:label></td>
								</tr>
								<tr>
									<!--Depurazione Quota Fissa Fatturato-->
									<td><asp:label id="Label60" CssClass="Input_Label" Runat="server">Quota Fissa Depurazione €</asp:label></td>
									<td align="right"><asp:label id="LblFatturatoDepurazioneQF" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label62" CssClass="Input_Label" Runat="server">+</asp:label></td>
									<td></td>
									<!--Depurazione Quota Fissa Incassato-->
									<td><asp:label id="Label63" CssClass="Input_Label" Runat="server">Quota Fissa Depurazione €</asp:label></td>
									<td align="right"><asp:label id="LblIncassatoDepurazioneQF" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label65" CssClass="Input_Label" Runat="server">+</asp:label></td>
								</tr>
								<tr>
									<!--Addizionali Fatturato-->
									<td><asp:label id="Label50" CssClass="Input_Label" Runat="server">Addizionali €</asp:label></td>
									<td align="right"><asp:label id="LblFatturatoAddizionali" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label37" CssClass="Input_Label" Runat="server">+</asp:label></td>
									<td></td>
									<!--Addizionali Incassato-->
									<td><asp:label id="Label31" CssClass="Input_Label" Runat="server">Addizionali €</asp:label></td>
									<td align="right"><asp:label id="LblIncassatoAddizionali" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label46" CssClass="Input_Label" Runat="server">+</asp:label></td>
								</tr>
								<tr>
									<!--IVA Fatturato-->
									<td><asp:label id="Label34" CssClass="Input_Label" Runat="server">IVA €</asp:label></td>
									<td style="BORDER-BOTTOM: #00008b 1px solid" align="right"><asp:label id="LblFatturatoIVA" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label40" CssClass="Input_Label" Runat="server">=</asp:label></td>
									<td></td>
									<!--IVA Incassato-->
									<td><asp:label id="Label36" CssClass="Input_Label" Runat="server">IVA €</asp:label></td>
									<td style="BORDER-BOTTOM: #00008b 1px solid" align="right"><asp:label id="LblIncassatoIVA" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label47" CssClass="Input_Label" Runat="server">=</asp:label></td>
								</tr>
								<tr> 
									<!--Totale Fatturato-->
									<td><asp:label id="Label26" CssClass="Legend" Runat="server">Importo Fatturato €</asp:label></td>
									<td align="right"><asp:label id="LblFatturatoTot" CssClass="Legend" Runat="server"></asp:label></td>
									<td></td>
									<td></td>
									<!--Totale Incassato-->
									<td><asp:label id="Label33" CssClass="Legend" Runat="server">Importo Incassato €</asp:label></td>
									<td align="right"><asp:label id="LblIncassatoTot" CssClass="Legend" Runat="server"></asp:label></td>
									<td></td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
			</table>
			<asp:button id="CmdStampa" style="DISPLAY: none" runat="server" Text="Stampa" Width="136px"></asp:button>
		</form>
	</body>
</HTML>
