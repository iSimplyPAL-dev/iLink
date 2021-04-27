<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiDoc.aspx.vb" Inherits="OPENgovTIA.ComandiDoc" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>ComandiDoc</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
	</head>
	<body class="SfondoGenerale" bottomMargin="0" leftMargin="2" topMargin="6" rightMargin="2"
		marginwidth="0" marginheight="0" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table id="Table1" cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<TR>
					<TD style="WIDTH: 464px; HEIGHT: 20px" align="left"><SPAN class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px"><asp:label id="lblTitolo" runat="server"></asp:label></SPAN></TD>
					<TD align="right" width="800" colSpan="2" rowSpan="2">
						<INPUT class="Bottone Bottonecancella" id="EliminaElabora" title="Eliminazione Elaborazione" onclick="parent.Visualizza.EliminaElaborazione()" type="button" name="Erase"> 
						<input class="Bottone BottoneFolderAccept" id="ApprovaMinuta" title="Approvazione Elaborazione" onclick="parent.Visualizza.ApprovaDocumenti();" type="button" name="ApprovaMinuta"> 
                        <input class="Bottone BottoneDownload" id="DownloadDoc" title="Download Documenti" onclick="parent.Visualizza.ViewDocElab();" type="button" name="DownloadDoc">
                        <input class="Bottone BottoneExcel" id="StampaMinutaRate" title="Stampa Minuta Rate" onclick="parent.Visualizza.MinutaRate()" type="button" name="StampaMinutaRate">
						<INPUT class="Bottone BottoneWord" id="ElaborazioneDocumenti" title="Elaborazione Documenti" onclick="parent.Visualizza.ElaborazioniDocumenti()" type="button" name="Modifica">
						<INPUT class="Bottone Bottonericerca" id="Search" title="Ricerca" onclick="parent.Visualizza.Search()" type="button" name="Search"> 
						<INPUT class="Bottone BottoneAnnulla" id="Cancel" title="Uscita" onclick="parent.Visualizza.BackToCalcolo()" type="button" name="Delete">
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 463px" align="left"><SPAN class="NormalBold_title" id="info" runat="server" style="WIDTH: 400px; HEIGHT: 20px;"> 
							Variabile - Elaborazione Documenti </SPAN>
					</TD>
				</TR>
			</table>
			&nbsp;
		</form>
	</body>
</HTML>
