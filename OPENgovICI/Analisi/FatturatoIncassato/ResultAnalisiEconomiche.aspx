<%@ Page language="c#" Codebehind="ResultAnalisiEconomiche.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.ResultAnalisiEconomiche" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ResultAnalisiEconomiche</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
		<script type="text/javascript">
			function LoadGrafico()
			{
				parametri='AbiPrinDovuto='+document.getElementById('TxtAbiPrinDovuto').value;
				parametri+='&AbiPrinVersato='+document.getElementById('TxtAbiPrinVersato').value;
				parametri+='&TerAgrDovuto='+document.getElementById('TxtTerAgrDovuto').value;
				parametri+='&TerAgrVersato='+document.getElementById('TxtTerAgrVersato').value;
				parametri+='&TerFabDovuto='+document.getElementById('TxtTerFabDovuto').value;
				parametri+='&TerFabVersato='+document.getElementById('TxtTerFabVersato').value;
				parametri+='&AltriFabDovuto='+document.getElementById('TxtAltriFabDovuto').value;
				parametri+='&AltriFabVersato='+document.getElementById('TxtAltriFabVersato').value;
				/**** 20120828 - IMU adeguamento per importi statali ****/
				parametri+='&TerAgrDovutoStato='+document.getElementById('TxtTerAgrDovutoStato').value;
				parametri+='&TerAgrVersatoStato='+document.getElementById('TxtTerAgrVersatoStato').value;
				parametri+='&TerFabDovutoStato='+document.getElementById('TxtTerFabDovutoStato').value;
				parametri+='&TerFabVersatoStato='+document.getElementById('TxtTerFabVersatoStato').value;
				parametri+='&AltriFabDovutoStato='+document.getElementById('TxtAltriFabDovutoStato').value;
				parametri+='&AltriFabVersatoStato='+document.getElementById('TxtAltriFabVersatoStato').value;
				parametri+='&FabRurUsoStrumDovuto='+document.getElementById('TxtFabRurUsoStrumDovuto').value;
				parametri+='&FabRurUsoStrumVersato='+document.getElementById('TxtFabRurUsoStrumVersato').value;
				/**** ****/
				/**** 20130422 - aggiornamento IMU ****/
				parametri+='&FabRurUsoStrumDovutoStato='+document.getElementById('TxtFabRurUsoStrumDovutoStato').value;
				parametri+='&FabRurUsoStrumVersatoStato='+document.getElementById('TxtFabRurUsoStrumVersatoStato').value;
				
				parametri+='&UsoProdCatDDovuto='+document.getElementById('TxtUsoProdCatDDovuto').value;
				parametri+='&UsoProdCatDVersato='+document.getElementById('TxtUsoProdCatDVersato').value;
				parametri+='&UsoProdCatDDovutoStato='+document.getElementById('TxtUsoProdCatDDovutoStato').value;
				parametri+='&UsoProdCatDVersatoStato='+document.getElementById('TxtUsoProdCatDVersatoStato').value;
				/**** ****/
				parametri+='&DetrazDovuto='+document.getElementById('TxtDetrazDovuto').value;
				parametri+='&DetrazVersato='+document.getElementById('TxtDetrazVersato').value;
				parametri+='&TotaleDovuto='+document.getElementById('TxtTotaleDovuto').value;
				parametri+='&TotaleVersato='+document.getElementById('TxtTotaleVersato').value;
				
				parent.frames.item('LoadResult').location.src='ChartAnalisiEconomiche.aspx?'+parametri
			}
		</script>
	</HEAD>
	<body class="Sfondo" leftMargin="3" topMargin="0" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="760" border="0">
				<tr>
					<td>
						<!--Riepilogo Ruolo/Avvisi-->
						<fieldset class="FiledSetRicerca" style="WIDTH: 100%">
							<table height="50" width="100%">
								<!--Intestazione-->
								<tr>
									<td colSpan="5"><asp:label id="Label1" CssClass="Input_Label_bold" Runat="server">Dati Dovuto/Versamenti</asp:label></td>
								</tr>
								<!--Dovuto-->
								<tr>
									<td><asp:label id="Label52" CssClass="Input_Label" Runat="server">N. Contribuenti Dovuto</asp:label></td>
									<td align="right"><asp:label id="LblNUtentiDovuto" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td width="20"></td>
									<td align="right"><asp:label id="Label55" CssClass="Input_Label" Runat="server">Totale Imposta Dovuta €</asp:label></td>
									<td align="right"><asp:label id="LblTotImpDovuto" CssClass="Input_Label" Runat="server"></asp:label></td>
								</tr>
								<tr>
									<td><asp:label id="Label4" CssClass="Input_Label" Runat="server">N. Contribuenti Versato</asp:label></td>
									<td align="right"><asp:label id="LblNUtentiVersato" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td width="20"></td>
									<td align="right"><asp:label id="Label5" CssClass="Legend" Runat="server">Totale Imposta Versata €</asp:label></td>
									<td align="right"><asp:label id="LblTotImpVersato" CssClass="Legend" Runat="server"></asp:label></td>
								</tr>
								<!--Pagato-->
								<tr>
									<td><br />
									</td>
								</tr>
								<!--Pagati Totalmente-->
								<tr>
									<td><asp:label id="Label7" CssClass="Input_Label" Runat="server">N. Versamenti Pagati in Unica Soluzione</asp:label></td>
									<td align="right"><asp:label id="LblNPagUS" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td width="20"></td>
									<td align="right"><asp:label id="Label9" CssClass="Input_Label" Runat="server">Totale Versato €</asp:label></td>
									<td align="right"><asp:label id="LblImpPagUS" CssClass="Input_Label" Runat="server"></asp:label></td>
								</tr>
								<!--Pagati Parzialmente-->
								<tr>
									<td><asp:label id="Label11" CssClass="Input_Label" Runat="server">N. Versamenti Pagati in rate</asp:label></td>
									<td align="right"><asp:label id="LblNPagRate" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td width="20"></td>
									<td align="right"><asp:label id="Label13" CssClass="Input_Label" Runat="server">Totale Versato €</asp:label></td>
									<td style="BORDER-BOTTOM: #00008b 1px solid" align="right"><asp:label id="LblImpPagRate" CssClass="Input_Label" Runat="server"></asp:label></td>
								</tr>
								<!--Totale Versato-->
								<tr>
									<td align="right" colSpan="4"><asp:label id="Label15" CssClass="Legend" Runat="server">Totale Incassato €</asp:label></td>
									<td align="right"><asp:label id="LblTotVersato" CssClass="Legend" Runat="server"></asp:label></td>
								</tr>
							</table>
							<!--Riepilogo Generale-->
							<table width="100%">
								<!--Intestazione-->
								<tr>
									<td colSpan="7"><asp:label id="Label8" style="TOP: 150px; LEFT: 3px" CssClass="Input_Label_bold" Runat="server">Riepilogo Generale</asp:label></td>
								</tr>
								<tr>
									<td width="100"><asp:label id="Label10" CssClass="Input_Label" Runat="server">N.Utenti</asp:label></td>
									<td align="center" width="250"><asp:label id="Label12" CssClass="Input_Label" Runat="server">Dovuto €</asp:label></td>
									<td align="center" width="15"></td>
									<td align="center" width="100"><asp:label id="Label16" CssClass="Input_Label" Runat="server">Incassato €</asp:label></td>
									<td align="center" width="15"></td>
									<td align="center" width="100"><asp:label id="Label18" CssClass="Input_Label" Runat="server">Insoluto €</asp:label></td>
									<td align="center" width="120"><asp:label id="Label19" CssClass="Legend" Runat="server">% Insoluto</asp:label></td>
								</tr>
								<tr>
									<td align="center"><asp:label id="LblNUtenti" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td align="center"><asp:label id="LblDovuto" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td align="center"><asp:label id="Label17" CssClass="Input_Label" Runat="server">-</asp:label></td>
									<td align="center"><asp:label id="LblVersato" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td align="center"><asp:label id="Label39" CssClass="Input_Label" Runat="server">=</asp:label></td>
									<td align="center"><asp:label id="LblInsoluto" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td align="center"><asp:label id="LblPercentualeInsoluto" CssClass="Legend" Runat="server"></asp:label></td>
									<td></td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
				<tr>
					<td><br />
						<!--Dettaglio Dovuto/Versato-->
						<fieldset class="FiledSetRicerca" style="WIDTH: 100%">
							<table width="100%">
								<tr>
									<td colSpan="3">
										<!--Intestazione--><asp:label id="Label20" CssClass="Input_Label_bold" Runat="server">Dettaglio Dovuto</asp:label></td>
									<td width="10"></td>
									<td colSpan="3">
										<!--Intestazione--><asp:label id="Label27" CssClass="Input_Label_bold" Runat="server">Dettaglio Versato</asp:label></td>
								</tr>
								<tr>
									<!--Utenti Dovuto-->
									<td><asp:label id="Label22" Runat="server" CssClass="Input_Label">Utenti</asp:label></td>
									<td align="right" width="20%"><asp:label id="LblDovutoUtenti" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td></td>
									<td></td>
									<!--Utenti Versato-->
									<td><asp:label id="Label29" Runat="server" CssClass="Input_Label">Utenti</asp:label></td>
									<td align="right" width="20%"><asp:label id="LblVersatoUtenti" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td></td>
								</tr>
								<tr>
									<!--Abitazione Principale Dovuto-->
									<td><asp:label id="Label23" Runat="server" CssClass="Input_Label">Abitazione Principale €</asp:label></td>
									<td align="right"><asp:label id="LblDovutoAbiPrin" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label21" CssClass="Input_Label" Runat="server">+</asp:label></td>
									<td></td>
									<!--Abitazione Principale Versato-->
									<td><asp:label id="Label30" Runat="server" CssClass="Input_Label">Abitazione Principale €</asp:label></td>
									<td align="right"><asp:label id="LblVersatoAbiPrin" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label45" CssClass="Input_Label" Runat="server">+</asp:label></td>
								</tr>
								<tr>
									<!--Terreni Agricoli Dovuto-->
									<td><asp:label id="Label14" Runat="server" CssClass="Input_Label">Terreni Agricoli Comune €</asp:label></td>
									<td align="right"><asp:label id="LblDovutoTerAgr" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label44" CssClass="Input_Label" Runat="server">+</asp:label></td>
									<td></td>
									<!--Terreni Agricoli Versato-->
									<td><asp:label id="Label48" Runat="server" CssClass="Input_Label">Terreni Agricoli Comune €</asp:label></td>
									<td align="right"><asp:label id="LblVersatoTerAgr" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label57" CssClass="Input_Label" Runat="server">+</asp:label></td>
								</tr>
								<tr>
									<!--Terreni Agricoli Dovuto Stato-->
									<td><asp:label id="Label2" Runat="server" CssClass="Input_Label">Terreni Agricoli Stato €</asp:label></td>
									<td align="right"><asp:label id="LblDovutoStatoTerAgr" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label3" CssClass="Input_Label" Runat="server">+</asp:label></td>
									<td></td>
									<!--Terreni Agricoli Versato Stato-->
									<td><asp:label id="Label6" Runat="server" CssClass="Input_Label">Terreni Agricoli Stato €</asp:label></td>
									<td align="right"><asp:label id="LblVersatoStatoTerAgr" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label24" CssClass="Input_Label" Runat="server">+</asp:label></td>
								</tr>
								<tr>
									<!--Terreni Fabbricabili Dovuto-->
									<td><asp:label id="Label25" Runat="server" CssClass="Input_Label">Aree Fabbricabili Comune €</asp:label></td>
									<td align="right" width="20%"><asp:label id="LblDovutoTerFab" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label28" CssClass="Input_Label" Runat="server">+</asp:label></td>
									<td></td>
									<!--Terreni Fabbricabili Versato-->
									<td><asp:label id="Label32" Runat="server" CssClass="Input_Label">Aree Fabbricabili Comune €</asp:label></td>
									<td align="right" width="20%"><asp:label id="LblVersatoTerFab" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label35" CssClass="Input_Label" Runat="server">+</asp:label></td>
								</tr>
								<tr>
									<!--Terreni Fabbricabili Dovuto Stato-->
									<td><asp:label id="Label31" Runat="server" CssClass="Input_Label">Aree Fabbricabili Stato €</asp:label></td>
									<td align="right" width="20%"><asp:label id="LblDovutoStatoTerFab" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label37" CssClass="Input_Label" Runat="server">+</asp:label></td>
									<td></td>
									<!--Terreni Fabbricabili Versato Stato-->
									<td><asp:label id="Label38" Runat="server" CssClass="Input_Label">Aree Fabbricabili Stato €</asp:label></td>
									<td align="right" width="20%"><asp:label id="LblVersatoStatoTerFab" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label40" CssClass="Input_Label" Runat="server">+</asp:label></td>
								</tr>
								<tr>
									<!--Altri Fabbricati Dovuto-->
									<td><asp:label id="Label41" Runat="server" CssClass="Input_Label">Altri Fabbricati Comune €</asp:label></td>
									<td align="right"><asp:label id="LblDovutoAltriFab" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label42" CssClass="Input_Label" Runat="server">=</asp:label></td>
									<td></td>
									<!--Altri Fabbricati Versato-->
									<td><asp:label id="Label43" Runat="server" CssClass="Input_Label">Altri Fabbricati Comune €</asp:label></td>
									<td align="right"><asp:label id="LblVersatoAltriFab" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label47" CssClass="Input_Label" Runat="server">=</asp:label></td>
								</tr>
								<tr>
									<!--Altri Fabbricati Dovuto Stato-->
									<td><asp:label id="Label46" Runat="server" CssClass="Input_Label">Altri Fabbricati Stato €</asp:label></td>
									<td align="right"><asp:label id="LblDovutoStatoAltriFab" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label49" CssClass="Input_Label" Runat="server">=</asp:label></td>
									<td></td>
									<!--Altri Fabbricati Versato Stato-->
									<td><asp:label id="Label50" Runat="server" CssClass="Input_Label">Altri Fabbricati Stato €</asp:label></td>
									<td align="right"><asp:label id="LblVersatoStatoAltriFab" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label51" CssClass="Input_Label" Runat="server">=</asp:label></td>
								</tr>
								<tr>
									<!--Fabbricati Rurali Uso Strumentale Dovuto-->
									<td><asp:label id="Label59" Runat="server" CssClass="Input_Label">Fabbricati Rurali Uso Strumentale €</asp:label></td>
									<td align="right"><asp:label id="LblDovutoFabRurUsoStrum" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label61" CssClass="Input_Label" Runat="server">+</asp:label></td>
									<td></td>
									<!--Fabbricati Rurali Uso Strumentale Versato-->
									<td><asp:label id="Label62" Runat="server" CssClass="Input_Label">Fabbricati Rurali Uso Strumentale €</asp:label></td>
									<td align="right"><asp:label id="LblVersatoFabRurUsoStrum" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label64" CssClass="Input_Label" Runat="server">+</asp:label></td>
								</tr>
								<tr>
									<!--Fabbricati Rurali Uso Strumentale Dovuto Stato-->
									<td><asp:label id="Label53" Runat="server" CssClass="Input_Label">Fabbricati Rurali Uso Strumentale Stato €</asp:label></td>
									<td align="right"><asp:label id="LblDovutoStatoFabRurUsoStrum" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label54" CssClass="Input_Label" Runat="server">+</asp:label></td>
									<td></td>
									<!--Fabbricati Rurali Uso Strumentale Versato Stato-->
									<td><asp:label id="Label56" Runat="server" CssClass="Input_Label">Fabbricati Rurali Uso Strumentale Stato €</asp:label></td>
									<td align="right"><asp:label id="LblVersatoStatoFabRurUsoStrum" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label58" CssClass="Input_Label" Runat="server">+</asp:label></td>
								</tr>
								<tr>
									<!--UsoProdCatD Dovuto-->
									<td><asp:label id="Label60" Runat="server" CssClass="Input_Label">UsoProdCatD €</asp:label></td>
									<td align="right"><asp:label id="LblDovutoUsoProdCatD" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label63" CssClass="Input_Label" Runat="server">+</asp:label></td>
									<td></td>
									<!--UsoProdCatD Versato-->
									<td><asp:label id="Label65" Runat="server" CssClass="Input_Label">UsoProdCatD €</asp:label></td>
									<td align="right"><asp:label id="LblVersatoUsoProdCatD" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label66" CssClass="Input_Label" Runat="server">+</asp:label></td>
								</tr>
								<tr>
									<!--UsoProdCatD Dovuto Stato-->
									<td><asp:label id="Label67" Runat="server" CssClass="Input_Label">UsoProdCatD Stato €</asp:label></td>
									<td style="BORDER-BOTTOM: #00008b 1px solid" align="right"><asp:label id="LblDovutoStatoUsoProdCatD" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label68" CssClass="Input_Label" Runat="server">+</asp:label></td>
									<td></td>
									<!--UsoProdCatD Versato Stato-->
									<td><asp:label id="Label69" Runat="server" CssClass="Input_Label">UsoProdCatD Stato €</asp:label></td>
									<td style="BORDER-BOTTOM: #00008b 1px solid" align="right"><asp:label id="LblVersatoStatoUsoProdCatD" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td><asp:label id="Label70" CssClass="Input_Label" Runat="server">+</asp:label></td>
								</tr>
								<tr>
									<!--Totale Dovuto-->
									<td><asp:label id="Label26" Runat="server" CssClass="Legend">Importo Dovuto €</asp:label></td>
									<td align="right"><asp:label id="LblDovutoTot" CssClass="Legend" Runat="server"></asp:label></td>
									<td></td>
									<td></td>
									<!--Totale Versato-->
									<td><asp:label id="Label33" Runat="server" CssClass="Legend">Importo Versato €</asp:label></td>
									<td align="right"><asp:label id="LblVersatoTot" CssClass="Legend" Runat="server"></asp:label></td>
									<td></td>
								</tr>
								<tr>
									<!--Detrazione Dovuto-->
									<td><asp:label id="Label34" Runat="server" CssClass="Input_Label">Detrazione €</asp:label></td>
									<td align="right"><asp:label id="LblDovutoDetrazione" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td></td>
									<td></td>
									<!--Detrazione Versato-->
									<td><asp:label id="Label36" Runat="server" CssClass="Input_Label">Detrazione €</asp:label></td>
									<td align="right"><asp:label id="LblVersatoDetrazione" CssClass="Input_Label" Runat="server"></asp:label></td>
									<td></td>
								</tr>
							</table>
						</fieldset>
					</td>
				</tr>
			</table>
			<asp:textbox id="TxtAbiPrinDovuto" style="DISPLAY: none" Runat="server">0</asp:textbox>
			<asp:textbox id="TxtAbiPrinVersato" style="DISPLAY: none" Runat="server">0</asp:textbox>
			<asp:textbox id="TxtTerAgrDovuto" Runat="server" style="DISPLAY:none">0</asp:textbox>
			<asp:textbox id="TxtTerAgrVersato" Runat="server" style="DISPLAY:none">0</asp:textbox>
			<asp:textbox id="TxtTerFabDovuto" Runat="server" style="DISPLAY:none">0</asp:textbox>
			<asp:textbox id="TxtTerFabVersato" Runat="server" style="DISPLAY:none">0</asp:textbox>
			<asp:textbox id="TxtAltriFabDovuto" Runat="server" style="DISPLAY:none">0</asp:textbox>
			<asp:textbox id="TxtAltriFabVersato" Runat="server" style="DISPLAY:none">0</asp:textbox>
			<asp:textbox id="TxtTerAgrDovutoStato" Runat="server" style="DISPLAY:none">0</asp:textbox>
			<asp:textbox id="TxtTerAgrVersatoStato" Runat="server" style="DISPLAY:none">0</asp:textbox>
			<asp:textbox id="TxtTerFabDovutoStato" Runat="server" style="DISPLAY:none">0</asp:textbox>
			<asp:textbox id="TxtTerFabVersatoStato" Runat="server" style="DISPLAY:none">0</asp:textbox>
			<asp:textbox id="TxtAltriFabDovutoStato" Runat="server" style="DISPLAY:none">0</asp:textbox>
			<asp:textbox id="TxtAltriFabVersatoStato" Runat="server" style="DISPLAY:none">0</asp:textbox>
			<asp:textbox id="TxtFabRurUsoStrumDovuto" style="DISPLAY: none" Runat="server">0</asp:textbox>
			<asp:textbox id="TxtFabRurUsoStrumVersato" style="DISPLAY: none" Runat="server">0</asp:textbox>
			<asp:textbox id="TxtFabRurUsoStrumDovutoStato" style="DISPLAY: none" Runat="server">0</asp:textbox>
			<asp:textbox id="TxtFabRurUsoStrumVersatoStato" style="DISPLAY: none" Runat="server">0</asp:textbox>
			<asp:textbox id="TxtUsoProdCatDDovuto" style="DISPLAY: none" Runat="server">0</asp:textbox>
			<asp:textbox id="TxtUsoProdCatDVersato" style="DISPLAY: none" Runat="server">0</asp:textbox>
			<asp:textbox id="TxtUsoProdCatDDovutoStato" style="DISPLAY: none" Runat="server">0</asp:textbox>
			<asp:textbox id="TxtUsoProdCatDVersatoStato" style="DISPLAY: none" Runat="server">0</asp:textbox>
			<asp:textbox id="TxtDetrazDovuto" Runat="server" style="DISPLAY:none">0</asp:textbox>
			<asp:textbox id="TxtDetrazVersato" Runat="server" style="DISPLAY:none">0</asp:textbox>
			<asp:textbox id="TxtTotaleDovuto" Runat="server" style="DISPLAY:none">0</asp:textbox>
			<asp:textbox id="TxtTotaleVersato" Runat="server" style="DISPLAY:none">0</asp:textbox>
		</form>
	</body>
</HTML>
