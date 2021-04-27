<%@ Page language="c#" Codebehind="CCalcoloICIMassivo.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CalcoloICI.CCalcoloICIMassivo" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<head>
		<title>CCalcoloICIMassivo</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" Content="C#"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
	</head>
	<body onload="VisualizzaLabel();" MS_POSITIONING="GridLayout" class="SfondoGenerale" bottomMargin="0"
		bgColor="#ffcc66" leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table id="Table1" cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td align="left" style="WIDTH: 641px"><span class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label></span></td>
					<td align="right" width="800" colSpan="2" rowSpan="2">
						<!--*** 20140509 - TASI ***-->
						<input class="Bottone BottoneWord" id="btnStampaMassiva" title="Elabora i Documenti" type="button" name="btnStampaMassiva" onclick="parent.Visualizza.ApriStampaMassiva();">
						<input class="Bottone BottoneCreaFile hidden" id="Extract" title="Estrazione File per Bollettini" onclick="parent.Visualizza.ApriParametriEsclusione();" type="button" name="Extract"> 
						<input class="Bottone BottoneLettera hidden" id="ExtractPostel" title="Estrazione Tracciato Postel" onclick="parent.Visualizza.ApriPostel();" type="button" name="ExtractPostel"> 
						<input class="Bottone BottoneStampa" id="Excel" title="Stampa Minuta Calcolo" onclick="parent.Visualizza.estraiExcel();" type="button" name="Excel"> 
						<input class="Bottone BottoneExcel" id="ExcelCatCl" title="Stampa Riepilogo Calcolo - Categoria e Classe" onclick="parent.Visualizza.estraiExcelCatClasse();" type="button" name="Excel">
						<!--*** 20120704 - IMU ***-->
						<input class="Bottone BottoneCalcolo" id="Insert" title="Effettua Calcolo Massivo" onclick="parent.Visualizza.Controlli();" type="button" name="Insert">
					</td>
				</tr>
				<tr>
					<td style="WIDTH: 641px" align="left"><span class="NormalBold_title" id="info" style="WIDTH: 580px; HEIGHT: 24px">Calcolo ICI/IMU/TASI - Elaborazione Massiva</span></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
