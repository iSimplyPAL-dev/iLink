<%@ Page language="c#" Codebehind="CGestione.aspx.cs" AutoEventWireup="True" Inherits="DichiarazioniICI.CGestione" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CGestione</title>
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
					<td align="right" width="800" colSpan="2" rowSpan="2">&nbsp; 
						<input class="Bottone BottoneGIS" id="GIS" title="Visualizza GIS" onclick="parent.Visualizza.document.getElementById('CmdGIS').click()" type="button" name="GIS" />
						<input class="Bottone BottoneExcel" id="Excel" title="Stampa elenco dati immobili in formato Excel" onclick="parent.Visualizza.document.getElementById('btnStampaExcel').click();" type="button" name="Excel" />
					    <input class="Bottone BottoneNewInsert" id="New" title="Inserisci una nuova dichiarazione in Effettivo." onclick="parent.Visualizza.document.getElementById('btnInserimentoDichiarazione').click()" type="button" name="Insert" />
						<input class="Bottone Bottonericerca" id="Delete" title="Ricerca" onclick="parent.Visualizza.Search();" type="button" name="Delete" />
					</td>
				</tr>
				<tr>
					<td style="WIDTH: 463px" align="left">
						<span class="NormalBold_title" id="info" style="WIDTH: 400px; HEIGHT: 20px;">ICI/IMU - Dichiarazioni - Gestione</span>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
