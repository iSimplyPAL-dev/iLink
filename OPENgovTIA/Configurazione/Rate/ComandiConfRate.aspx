<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ComandiConfRate.aspx.vb" Inherits="OPENgovTIA.ComandiConfRate" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<head>
		<title>ComandiConfRate</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="../../../Styles.css" type="text/css" rel="stylesheet">
		<%if Session("SOLA_LETTURA")="1" then%>
		<link href="../../../solalettura.css" type="text/css" rel="stylesheet">
		<%end if%>
	    <script type="text/javascript" src="../../../_js/jquery.min.js?newversion"></script>
        <script type="text/javascript" src="../../../_js/Custom.js?newversion"></script>
	</head>
	<body class="SfondoGenerale" MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="2" topMargin="6" rightMargin="2" marginheight="0" marginwidth="0">
		<form id="Form1" runat="server" method="post">
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0" id="Table1">
				<tr>
					<td align="left">
						<span class="ContentHead_Title" id="infoEnte">
							<asp:Label id="lblTitolo" runat="server"></asp:Label>
						</span>
					</td>
					<td align="right" colSpan="2" rowSpan="2">
						<input class="Bottone BottoneSalva" id="Insert" title="Salva Rata" onclick="parent.Visualizza.getElementById('CmdSalva').click();" type="button" name="Insert"> 
						<input class="Bottone BottoneAnnulla" id="Cancel" title="Torna alla Gestione" onclick="parent.Comandi.location.href='../../Avvisi/Calcolo/ComandiCalcolo.aspx?IsFromVariabile=<%=Request.Item("IsFromVariabile")%>';parent.Visualizza.location.href='../../Avvisi/Calcolo/Calcolo.aspx?IsFromVariabile=<%=Request.Item("IsFromVariabile")%>';" type="button" name="Delete">
					</td>
				</tr>
				<tr>
					<td align="left">
						<span class="NormalBold_title" id="info" runat="server"> Variabile - Configurazioni - Rate</span>
					</td>
				</tr>
			</table>
			&nbsp;
		</form>
	</body>
</HTML>
