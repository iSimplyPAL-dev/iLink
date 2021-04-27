<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiRicPesature.aspx.vb" Inherits="OPENgovTIA.ComandiRicPesature"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>ComandiRicPesature</title>
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
						<input class="Bottone BottoneCancella" id="Cancella" title="Elimina Pesature" onclick="parent.Visualizza.Delete()" type="button" name="Cancella" style="DISPLAY:none">
						<input class="Bottone BottoneExcel" id="StampaExcel" title="Stampa Pesature" onclick="parent.Visualizza.EstraiExcel();" type="button" name="StampaExcel">					
						<INPUT class="Bottone Bottonericerca" id="Search" title="Ricerca" onclick="parent.Visualizza.Search()"
							type="button" name="Search">
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 463px" align="left">
						<SPAN class="NormalBold_title" id="info" runat="server" style="WIDTH: 400px; HEIGHT: 20px">
							 Variabile - Conferimenti - Ricerca</SPAN>
					</TD>
				</TR>
			</table>
			&nbsp;
		</form>
	</body>
</HTML>
