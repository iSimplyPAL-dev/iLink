<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiRicDichiarazione.aspx.vb" Inherits="OPENgovTIA.ComandiRicDichiarazione" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>ComandiRicDichiarazione</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../_js/Custom.js?newversion"></script>
	</head>
	<body class="SfondoGenerale" MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="2"
		topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" id="Table1">
				<TR>
					<TD style="WIDTH: 464px; HEIGHT: 20px" align="left">
						<SPAN class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label>
						</SPAN>
					</TD>
					<TD align="right" width="800" colSpan="2" rowSpan="2">
					    <input class="Bottone BottoneForzaDati" id="AggMassivo" title="Aggiornamento Massivo" onclick="parent.Visualizza.DivAttesa.style.display = '';parent.Visualizza.document.getElementById('CmdAggMassivo').click();" type="button" name="AggMassivo"> 
						<input class="Bottone BottoneGIS" id="GIS" title="Visualizza GIS" onclick="parent.Visualizza.DivAttesa.style.display = '';parent.Visualizza.document.getElementById('CmdGIS').click();" type="button" name="GIS"> 
						<input class="Bottone BottoneStampaAlt" id="StampaSintetica" title="Stampa Dichiarazioni Sintetica" onclick="parent.Visualizza.fStampaSintetica();" type="button" name="StampaDich">
						<input class="Bottone BottoneExcel" id="StampaAnalitica" title="Stampa Dichiarazioni Analitica" onclick="parent.Visualizza.fStampaAnalitica();" type="button" name="StampaDich">
						<input class="Bottone BottoneExcel" id="StampaDich" title="Elenco Dichiarazioni" onclick="parent.Visualizza.EstraiExcel();" type="button" name="StampaDich" style="DISPLAY: none"> 
						<input class="Bottone BottoneNewInsert" id="NewInsert" title="Inserimento nuova denuncia." onclick="parent.Visualizza.NewDichiarazione(-1)" type="button" name="NewInsert"> 
						<input class="Bottone BottoneRicerca" id="Search" title="Ricerca" onclick="parent.Visualizza.Search()" type="button" name="Search">
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 463px" align="left">
						<SPAN class="NormalBold_title" id="info" runat="server" style="WIDTH: 400px; HEIGHT: 20px;">
							 Variabile - Dichiarazioni - Ricerca</SPAN>
					</TD>
				</TR>
			</table>
			&nbsp;
		</form>
	</body>
</HTML>
