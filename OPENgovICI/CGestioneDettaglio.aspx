<%@ Page language="c#" Codebehind="CGestioneDettaglio.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CGestioneDettaglio" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CGestioneDettaglio</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1"){%> 
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%}%> 
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout" class="SfondoGenerale" leftMargin="2" topMargin="6" rightMargin="2">
		<form id="Form1" runat="server" method="post">
			<table id="Table1" cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td style="WIDTH: 464px; HEIGHT: 20px" align="left">
					    <span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label>
						</span>
					</td>
					<td align="right" width="800" colSpan="2" rowSpan="2">&nbsp;&nbsp;&nbsp;&nbsp; 
						<div class="tooltip">
						    <input class="Bottone BottoneGIS" id="GIS" title="Visualizza GIS" onclick="parent.Visualizza.document.getElementById('CmdGIS').click()" type="button" name="GIS" />
                            <span class="tooltiptext">Visualizza GIS</span>
                        </div>
						<!--*** 20131003 - gestione atti compravendita ***-->
						<div class="tooltip">
						    <input class="Bottone BottoneRibalta" id="NewImmobile" title="Inserisci immobile da compravendita" onclick="parent.Visualizza.document.getElementById('cmdNewImmobile').click()" type="button" name="NewImmobile"> 
                            <span class="tooltiptext">Inserisci immobile da compravendita</span>
                        </div>
						<!--*** 20120704 - IMU ***-->
						<div class="tooltip">
						    <input class="Bottone BottoneElabora" id="CalcoloICIpuntuale" title="Vai al Calcolo ICI/IMU puntuale del Contribuente" onclick="parent.Visualizza.gotoCalcoloICIpuntualeContribuente();" type="button" name="CalcoloICIpuntuale">&nbsp; 
                            <span class="tooltiptext">Vai al Calcolo ICI/IMU puntuale del Contribuente</span>
                        </div>
						<div class="tooltip">
						    <input class="Bottone BottoneGoToCalcoloICI hidden" id="ControlloPreAccertamento" title="Vai alla Gestione di Controllo Pre-Accertamento" onclick="parent.Visualizza.gotoControlloPreAccertamentoContribuente();" type="button" name="CalcoloICIpuntuale">
                            <span class="tooltiptext">Vai alla Gestione di Controllo Pre-Accertamento</span>
                        </div>
						<div class="tooltip">
    						<input class="Bottone BottoneAnnulla" id="Indietro" title="Torna alla pagina di gestione." onclick="parent.Visualizza.document.getElementById('btnIndietro').click()" type="button" name="Insert">
                            <span class="tooltiptext">Torna alla pagina di gestione</span>
                        </div>
					</td>
				</tr>
				<tr>
					<td style="WIDTH: 463px" align="left">
					    <span class="NormalBold_title" id="info" style="WIDTH: 400px; HEIGHT: 20px">ICI/IMU - Dichiarazioni - Dettaglio</span>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
