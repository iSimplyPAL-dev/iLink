<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiGestioneAnagrafeResidenti.aspx.vb" Inherits="OPENgov.ComandiGestioneAnagrafeResidenti"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ComandiGestioneAnagrafeResidenti</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../Styles.css" type="text/css" rel="stylesheet">
		<%        If Session("SOLA_LETTURA") = "1" Then%>
		<link href="../solalettura.css" type="text/css" rel="stylesheet">
		<%        End If%>
	    <script type="text/javascript" src="../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../_js/Custom.js?newversion"></script>
	</HEAD>
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
						<input class="Bottone BottoneExcel" id="Excel" title="Stampa elenco Anagrafiche in formato Excel" onclick="parent.Visualizza.DivAttesa.style.display = ''; parent.Visualizza.document.getElementById('btnStampaExcel').click();" type="button" name="Excel">
						<div class="tooltip">
                            <input class="Bottone BottoneSalva" id="btnSalva" title="Imposta come Trattati" onclick="parent.Visualizza.DivAttesa.style.display='';parent.Visualizza.document.getElementById('btnSalva').click();" type="button" name="Save"> 
                            <span class="tooltiptext">Setta le posizioni selezionate come trattate</span>
                        </div>
						<input class="Bottone Bottonericerca" id="btnRicerca" title="Ricerca" onclick="parent.Visualizza.DivAttesa.style.display='';parent.Visualizza.document.getElementById('btnRicerca').click();" type="button" name="Search"> 
					</TD>
				</TR>
				<TR>
					<TD style="WIDTH: 463px" align="left">
						<SPAN class="NormalBold_title" id="info" style="WIDTH: 400px; HEIGHT: 20px"> Anagrafe Residenti - Gestione</SPAN>
					</TD>
				</TR>
			</table>
			&nbsp;
		</form>
	</body>
</HTML>
