<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiRicGestPag.aspx.vb" Inherits="OPENgovTIA.ComandiRicGestPag" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>ComandiRicGestPag</title>
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
						<input class="Bottone BottoneRiversamento" id="Riversamento" type="button" name="Riversamento" onclick="parent.Visualizza.DivAttesa.style.display='';parent.Visualizza.document.getElementById('btnStampaRiversamento').click();" title="Riversamento"> 
						<input class="Bottone BottoneQuadratura" id="Quadratura" type="button" name="Quadratura" onclick="parent.Visualizza.DivAttesa.style.display='';parent.Visualizza.document.getElementById('btnStampaQuadratura').click();" title="Quadratura"> 
						<input class="Bottone BottoneNoMoney" id="StampaNonP" type="button" name="StampaNonP" onclick="parent.Visualizza.DivAttesa.style.display='';parent.Visualizza.document.getElementById('btnStampaNonPag').click();" title="Elenco soggetti che non hanno pagato"> 
						<INPUT class="Bottone BottoneMoreMoney" id="StampaPmag" type="button" name="StampaPmag" onclick="parent.Visualizza.DivAttesa.style.display='';parent.Visualizza.document.getElementById('btnStampaPMag').click();" title="Elenco soggetti con importo pagato maggiore di importo emesso"> 
						<INPUT class="Bottone BottoneLessMoney" id="StampaPMin" type="button" name="StampaPMin" onclick="parent.Visualizza.DivAttesa.style.display='';parent.Visualizza.document.getElementById('btnStampaPMin').click();" title="Elenco soggetti con importo pagato minore di importo emesso"> 
						<input class="Bottone BottoneMoney" id="StampaPag" type="button" name="StampaPag" onclick="parent.Visualizza.DivAttesa.style.display='';parent.Visualizza.document.getElementById('btnStampaPag').click();" title="Elenco pagamenti"> 
						<INPUT class="Bottone BottoneNewInsert" id="NewInsert" type="button" name="NewInsert" onclick="parent.Visualizza.NewPagamento(-1)" title="Inserimento nuovo pagamento"> 
						<INPUT class="Bottone Bottonericerca" id="Search" type="button" name="Search" onclick="parent.Visualizza.DivAttesa.style.display='';parent.Visualizza.Search()" title="Ricerca">
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 463px" align="left">
						<SPAN class="NormalBold_title" id="info" runat="server" style="WIDTH: 400px; HEIGHT: 20px;">
							 - Pagamenti - Ricerca</SPAN>
					</TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
