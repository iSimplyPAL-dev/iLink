<%@ Page language="c#" Codebehind="CConfrontaConCatasto.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.ConfrontaConCatasto.CConfrontaConCatasto" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CCalcoloICIMassivo</title>
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
		<script>
			function estraiExcelConfronto()
			{
				//*** 20120704 - IMU ***
				if (confirm('Si vuole proseguire con l\'elaborazione e l\'estrazione del file Excel del confronto ICI/IMU - Catasto?'))
				{
					if (confirm('L\'operazione NON può essere compiuta da più utenti contemporaneamente.\nControllare che nessun altro compia questa elaborazione sull\'Ente corrente.\nSi vuole continuare?')){
						intSelected=parent.Visualizza.document.getElementById('ddlAnnoRiferimento').selectedIndex;
						Anno=parent.Visualizza.document.getElementById('ddlAnnoRiferimento')[intSelected].value;
						parent.Visualizza.document.getElementById('btnClasseCatastato').click();
					}
					else{
						return false;
					}
					
				}								
				else
				{
					return false;
				}	
			
				return false;									
			}
			
						
			function estraiExcelPosizioni()
			{
				//*** 20120704 - IMU ***
				if (confirm('Si vuole proseguire con l\'elaborazione e l\'estrazione del file Excel dell\'elenco delle posizioni di Catasto non presenti in ICI/IMU?'))
				{	
					if (confirm('L\'operazione può essere compiuta solamente da un utente alla volta.\nControllare che nessun altro compia questa elaborazione sull\'Ente corrente.\nSi vuole continuare?')){
						//parent.Visualizza.attesaCarica.style.display='';
						//Anno=document.frmRicerca.ddlAnnoRiferimento[document.frmRicerca.ddlAnnoRiferimento.selectedIndex].value;
						
						//alert(parent.Visualizza.document.getElementById('ddlAnnoRiferimento').selectedIndex);
						intSelected=parent.Visualizza.document.getElementById('ddlAnnoRiferimento').selectedIndex;
						//alert(parent.Visualizza.document.getElementById('ddlAnnoRiferimento')[intSelected].value);
						Anno=parent.Visualizza.document.getElementById('ddlAnnoRiferimento')[intSelected].value;
										
						//parent.nascosto.document.getElementById('txtAnno').value= Anno;					
						parent.Visualizza.document.getElementById('btnICI').click();
					}
					else{
						return false;
					}
				}								
				else
				{
					return false;
				}	
			
				return false;									
			}	
			
			function estraiExcelPassaggioProprieta()
			{
			
				if (confirm('Si vuole proseguire con l\'elaborazione e l\'estrazione del file Excel delle posizioni con passaggio di proprietà (cambio intestatario)?'))
				{
					if (confirm('L\'operazione può essere compiuta solamente da un utente alla volta.\nControllare che nessun altro compia questa elaborazione sull\'Ente corrente.\nSi vuole continuare?')){
						//parent.Visualizza.attesaCarica.style.display='';
						//Anno=document.frmRicerca.ddlAnnoRiferimento[document.frmRicerca.ddlAnnoRiferimento.selectedIndex].value;
						
						//alert(parent.Visualizza.document.getElementById('ddlAnnoRiferimento').selectedIndex);
						intSelected=parent.Visualizza.document.getElementById('ddlAnnoRiferimento').selectedIndex;
						//alert(parent.Visualizza.document.getElementById('ddlAnnoRiferimento')[intSelected].value);
						Anno=parent.Visualizza.document.getElementById('ddlAnnoRiferimento')[intSelected].value;
										
						//parent.nascosto.document.getElementById('txtAnno').value= Anno;					
						parent.Visualizza.document.getElementById('btnPassaggioProprieta').click();
					}
					else{
						return false;
					}
					
				}								
				else
				{
					return false;
				}	
			
				return false;									
			}	
					
		</script>
	</HEAD>
	<body onload="VisualizzaLabel();" MS_POSITIONING="GridLayout" class="SfondoGenerale" bottomMargin="0"
		bgColor="#ffcc66" leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table id="Table1" cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td align="left" style="WIDTH: 641px"><span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label></span></td>
					<td align="right" width="800" colSpan="2" rowSpan="2">
						<!--*** 20120704 - IMU ***-->
						<input class="Bottone BottoneStampa" id="StampaExcelDiffICICAT" title="Elaborazione e stampa in Excel Confronto ICI/IMU - Catasto" onclick="parent.Visualizza.document.getElementById('DivAttesa').style.display = '';estraiExcelConfronto();" type="button" name="StampaExcel">
						<input class="Bottone BottoneExcel" id="StampaExcelCATNOICI" title="Elaborazione e stampa in Excel elenco delle posizioni di Catasto non presenti in ICI/IMU" onclick="parent.Visualizza.document.getElementById('DivAttesa').style.display = '';estraiExcelPosizioni();" type="button" name="StampaExcel">
						<input class="Bottone BottoneStampaAlt" id="StampaExcelPASSPROP" title="Elaborazione e stampa in Excel elenco delle posizioni con passaggio di proprietà (cambio intestatario)" onclick="parent.Visualizza.document.getElementById('DivAttesa').style.display = '';estraiExcelPassaggioProprieta();" type="button" name="StampaExcel">
					</td>
				</tr>
				<tr>
					<td style="WIDTH: 641px" align="left"><span class="NormalBold_title" id="info" style="WIDTH: 580px; HEIGHT: 24px">ICI/IMU - Confronto Con Catasto</span></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
