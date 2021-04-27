<%@ Page language="c#" Codebehind="ComandiDocElaborati.aspx.cs" AutoEventWireup="True" Inherits="OPENgovTOCO.Elaborazioni.Documenti.ComandiDocElaborati" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ComandiDocElaborati</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if (Session["SOLA_LETTURA"]=="1") {%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%}%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
	</HEAD>
	<body class="SfondoGenerale" bottomMargin="0" leftMargin="2" topMargin="6" rightMargin="2"
		marginwidth="0" marginheight="0" MS_POSITIONING="GridLayout">
		<form id="Form1" runat="server" method="post">
			<table id="Table1" cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<TR>
					<TD style="WIDTH: 464px; HEIGHT: 20px" align="left"><SPAN class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px"><asp:Label id="lblTitolo" runat="server"></asp:label></SPAN></TD>
					<TD align="right" width="800" colSpan="2" rowSpan="2">
						<INPUT class="Bottone BottoneAnnulla" id="Cancel" title="Uscita" onclick="parent.Visualizza.LoadElaborazioneDocumenti();"
							type="button" name="Delete">
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 463px" align="left"><SPAN class="NormalBold_title" id="info" style="WIDTH: 400px; HEIGHT: 20px">TOSAP/COSAP 
							- Visualizzazione Documenti Elaborati</SPAN>
					</TD>
				</TR>
			</table>
			&nbsp;
		</form>
	</body>
</HTML>
