<%@ Page language="c#" Codebehind="CImmobileDettaglio.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CImmobileDettaglio" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CImmobileDettaglio</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1"){%> 
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%}%> 
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
	</HEAD>
	<body class="SfondoGenerale" bottomMargin="0" bgColor="#ffcc66" leftMargin="2" topMargin="6"
		rightMargin="2" marginwidth="0" marginheight="0" MMS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table id="Table1" cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td style="WIDTH: 464px; HEIGHT: 20px" align="left"><span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px"><asp:label id="lblTitolo" runat="server"></asp:label></span></td>
					<td align="right" width="800" colSpan="2" rowSpan="2">
						<!--*** 20131003 - gestione atti compravendita ***-->
						<input class="Bottone BottoneDuplica" id="Precarica" title="Precarica" onclick="parent.Visualizza.document.getElementById('cmdPrecarica').click();" type="button" name="Precarica">							
						<input class="Bottone BottoneApri" style="display:none;" id="Unlock" title="Abilita i contolli per scrivere." onclick="parent.Visualizza.document.getElementById('btnAbilita').click()" type="button" name="Unlock">&nbsp;
						<input class="Bottone BottoneSoggetti hidden" id="Contitolari" title="Visualizza i contitolari." onclick="parent.Visualizza.document.getElementById('btnAggiungiContitolare').click()" type="button" name="Insert">
						<input class="Bottone BottoneSalva" id="Insert" title="Salva i dati dell'immobile." onclick="parent.Visualizza.document.getElementById('btnSalva').click()" type="button" name="Insert">
						<input class="Bottone Bottonecancella" id="Delete" title="Cancella l'immobile dalla dichiarazione." onclick="parent.Visualizza.document.getElementById('btnElimina').click()" type="button" name="Delete">
						<input class="Bottone Bottoneannulla" id="Cancel" title="Torna alla dichiarazione." onclick="parent.Visualizza.document.getElementById('btnIndietro').click()" type="button" name="Delete">
					</td>
				</tr>
				<tr>
					<td style="WIDTH: 463px" align="left"><span class="NormalBold_title" id="info" style="WIDTH: 496px; HEIGHT: 24px;">ICI/IMU - Dichiarazioni - Dettaglio immobile</span></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
