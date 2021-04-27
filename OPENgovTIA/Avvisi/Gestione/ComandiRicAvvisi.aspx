<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiRicAvvisi.aspx.vb" Inherits="OPENgovTIA.ComandiRicAvvisi" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>ComandiRicAvvisi</title>
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
	<body class="SfondoGenerale" MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" id="Table1">
				<tr>
					<td style="WIDTH: 464px; HEIGHT: 20px" align="left">
						<SPAN class="ContentHead_Title" id="infoEnte" style="WIDTH: 400px">
							<asp:Label id="lblTitolo" runat="server"></asp:Label>
						</SPAN>
					</td>
					<td align="right" width="800" colSpan="2" rowSpan="2">
						<input class="Bottone BottoneExcel" id="Print" title="Stampa" onclick="parent.Visualizza.DivAttesa.style.display = ''; parent.Visualizza.document.getElementById('CmdStampa').click()" type="button" name="Print">
						<input class="Bottone BottoneRicerca" id="Search" title="Ricerca" onclick="parent.Visualizza.Search();" type="button" name="Search">
					</td>
				</tr>
				<tr>
					<td style="WIDTH: 463px" align="left">
						<SPAN class="NormalBold_title" id="info" runat="server" style="WIDTH: 400px; HEIGHT: 20px;" runat="server"> - Avvisi - Ricerca</SPAN>
					</td>
				</tr>
			</table>
			&nbsp;
		</form>
	</body>
</HTML>
