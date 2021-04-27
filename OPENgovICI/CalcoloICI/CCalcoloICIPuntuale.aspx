<%@ Page language="c#" Codebehind="CCalcoloICIPuntuale.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CalcoloICI.CCalcoloICIPuntuale" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>CCalcoloICIPuntuale</title>
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
	</head>
	<body class="SfondoGenerale" >
		<form id="Form1" runat="server" method="post">
			<table id="Table1" cellSpacing="0" cellPadding="0" width="100%">
				<tr>
					<td>
						<span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label></span>
						<br />
						<!--*** 20120704 - IMU ***-->
						<span id="info">ICI/IMU - Calcolo Puntuale</span>
					</td>
					<td align="right">
						<input class="Bottone BottoneWord" id="Stampa" title="Ristampa l'informativa e i bollettini" onclick="parent.Visualizza.document.getElementById('DivAttesa').style.display = ''; parent.Visualizza.document.getElementById('divStampa').style.display = ''; parent.Visualizza.document.getElementById('DivCalcolo').style.display = 'none'; parent.Visualizza.document.getElementById('btnElaboraDoc').click();" type="button" name="Stampa"> 
						<input class="Bottone BottoneExcel" id="StampaExcel" title="Stampa Minuta Calcolo" onclick="parent.Visualizza.document.getElementById('btnStampaExcel').click();" type="button" name="StampaExcel">
						<input class="Bottone BottonePulisci" id="Conferma" title="Escludi il contribuente selezionato dall'estrazione dati per le informative" onclick="parent.Visualizza.document.getElementById('btnConfermaCalcolo').click();" type="button" name="Conferma" style="display:none"> 
						<input class="Bottone BottoneUserHome" id="DichImmContrib" title="Visualizza Dichiarazioni e Immobili del Contribuente" onclick="parent.Visualizza.gotoDichImmContribuente();" type="button" name="DichImmContrib">
						<input class="Bottone BottoneCalcolo" id="Insert" title="Effettua Calcolo Puntuale" onclick="parent.Visualizza.Controlli();" type="button" name="Insert">
						<input class="Bottone Bottoneannulla hidden" id="Cancel" title="Torna indietro" onclick="parent.Visualizza.document.getElementById('btnIndietro').click()" type="button" name="Delete">
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
